using AutoMapper;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Core;
using Shop2.Web.Infrastructure.Extensions;
using Shop2.Web.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Shop2.Web.Api
{
    [RoutePrefix("api/page")]
    [Authorize] 
    public class  PageController : ApiControllerBase
    {
        IPageService _pageService;

        public PageController(IErrorService errorService, IPageService pageService) : base(errorService)
        {
            this._pageService = pageService;
        }



        [Route("getall")]
        [HttpGet]

        public HttpResponseMessage Get(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request,
                () => {
                    int totalRow = 0;
                    // tìm kiếm theo keyword
                    var listPage =  _pageService.GetAllByKeyWord(keyword);

                   
                    totalRow = listPage.Count();
                    
                    var query = listPage.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                    var listpage = Mapper.Map<IEnumerable <Page>,IEnumerable <PageViewModel>>(query);

                    var paginationSet = new PaginationSet<PageViewModel>()
                    {
                        Items = listpage,
                        Page = page,
                        TotalCount = totalRow,
                        TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                    };

                    var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                    return response;
                });
        }

        

        [Route("create")]
        [HttpPost]
        [Authorize] 

        public HttpResponseMessage Creat(HttpRequestMessage request, PageViewModel pageViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var newPage = new Page();
                    newPage.UpdatePage(pageViewModel);
                    newPage.CreatedDate = DateTime.Now;
                    newPage.CreatedBy = User.Identity.Name;
                    _pageService.Add(newPage);
                  
                    try
                    {
                        _pageService.Save();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }


                    var responseData = Mapper.Map<Page, PageViewModel>(newPage);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }


                return response;

            });
        }


        // lấy về id để update
        [Route("getbyid/{id:int}")]
        [HttpGet]

        public HttpResponseMessage GetByID(HttpRequestMessage request, int id)
        {
            return CreateHttpResponse(request,
                () => {

                    var listpage = _pageService.GetById(id);

                    var responseData = Mapper.Map<Page, PageViewModel>(listpage);


                    var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return response;
                });
        }



        [Route("update")]
        [HttpPut]
        [AllowAnonymous]

        public HttpResponseMessage Update(HttpRequestMessage request, PageViewModel pageViewModel)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    var dbPage = _pageService.GetById(pageViewModel.ID);

                    dbPage.UpdatePage(pageViewModel);
                    dbPage.UpdatedDate = DateTime.Now;
                    dbPage.CreatedBy = User.Identity.Name;

                    _pageService.Update(dbPage);

                    try
                    {
                        _pageService.Save();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }

                    var responseData = Mapper.Map<Page, Page>(dbPage);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }


                return response;

            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]

        public HttpResponseMessage Delete(HttpRequestMessage request, PageViewModel pageViewModel, int id)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {

                    var dbPage = _pageService.Delete(id);

                    try
                    {
                        _pageService.Save();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }

                    var responseData = Mapper.Map<Page,PageViewModel>(dbPage);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }


                return response;

            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]

        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedPages)
        {
            return CreateHttpResponse(request, () =>
            {
                HttpResponseMessage response = null;

                if (!ModelState.IsValid)
                {
                    response = request.CreateResponse(HttpStatusCode.BadRequest, ModelState);
                }
                else
                {
                    // thư viện JavaScriptSerializer giúp ta dữ liệu text dưới dạng JSON qua một đối tượng tương ứng ở đây 
                    var listPage = new JavaScriptSerializer().Deserialize<List<int>>(checkedPages);

                    foreach (var item in listPage)
                    {
                        _pageService.Delete(item);
                    }

                    try
                    {
                        _pageService.Save();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }


                    response = request.CreateResponse(HttpStatusCode.OK, listPage.Count);
                }


                return response;

            });
        }

    }
}