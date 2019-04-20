using AutoMapper;
using Shop2.Common;
using Shop2.Model.Models;
using Shop2.Service;
using Shop2.Web.Infrastructure.Core;
using Shop2.Web.Infrastructure.Extensions;
using Shop2.Web.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Shop2.Web.Api
{
    [RoutePrefix("api/product")]
    //[Authorize] // bắt buộc đăng nhập mói vô được
    public class ProductController : ApiControllerBase
    {
        IProductService _productService;

        public ProductController(IErrorService errorService, IProductService productService) : base(errorService)
        {
            this._productService = productService;
        }


        #region CRUD
        [Route("getall")]
        [HttpGet]
        public HttpResponseMessage Get(HttpRequestMessage request, string keyword, int page, int pageSize = 20)
        {
            return CreateHttpResponse(request,
                () => {
                    int totalRow = 0;
                    // tìm kiếm theo keyword
                    
                    var listCategory = _productService.GetAllByKeyWord(keyword);
                    

                    // lấy ra số bản ghi và thực hiện phân trang
                    totalRow = listCategory.Count();

                    var query = listCategory.OrderByDescending(x => x.CreatedDate).Skip(page * pageSize).Take(pageSize);

                    var listproduct = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(query);

                    var paginationSet = new PaginationSet<ProductViewModel>()
                    {
                        Items = listproduct,
                        Page = page,
                        TotalCount = totalRow,
                        // làm tròn số trang (lên )
                        TotalPages = (int)Math.Ceiling((decimal)totalRow / pageSize)
                    };

                    var response = request.CreateResponse(HttpStatusCode.OK, paginationSet);
                    return response;
                });
        }




        [Route("getallparents")]
        [HttpGet]
        public HttpResponseMessage GetAllParents(HttpRequestMessage request)
        {
            // c1 viết function nặc danh
            return CreateHttpResponse(request,
                () => {

                    var listproduct = _productService.GetAll();

                    var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(listproduct);


                    var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return response;
                });
            // c2 : 
            //Func<HttpResponseMessage> func = () => // đây là 1 delegate thực thi khối lệnh bên trong
            //{
            //    var listproduct = _productService.GetAll();

            //    var responseData = Mapper.Map<IEnumerable<Product>, IEnumerable<ProductViewModel>>(listproduct);


            //    var response = request.CreateResponse(HttpStatusCode.OK, responseData);
            //    return response;
            //}
            // return CreateHttpResponse(request, func);
        }




        [Route("create")]
        [HttpPost]
        [AllowAnonymous] // cho phép post vào nặc danh chưa cần đăng nhập
        public HttpResponseMessage Create(HttpRequestMessage request, ProductViewModel ProductViewModel)
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
                    var newProduct = new Product();
                    newProduct.UpdateProduct(ProductViewModel);
                    newProduct.CreatedDate = DateTime.Now;
                    newProduct.CreatedBy = User.Identity.Name;
                    _productService.Add(newProduct);
                   

                    try
                    {
                        _productService.Save();
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

                    var responseData = Mapper.Map<Product, ProductViewModel>(newProduct);
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

                    var listproduct = _productService.GetById(id);

                    var responseData = Mapper.Map<Product, ProductViewModel>(listproduct);


                    var response = request.CreateResponse(HttpStatusCode.OK, responseData);
                    return response;
                });
        }



        [Route("update")]
        [HttpPut]
        [AllowAnonymous]
        public HttpResponseMessage Update(HttpRequestMessage request, ProductViewModel ProductViewModel)
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
                    var dbProduct = _productService.GetById(ProductViewModel.ID);

                    dbProduct.UpdateProduct(ProductViewModel);
                    dbProduct.UpdatedDate = DateTime.Now;
                    dbProduct.CreatedBy = User.Identity.Name;

                    _productService.Update(dbProduct);

                    try
                    {
                        _productService.Save();
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

                    var responseData = Mapper.Map<Product, ProductViewModel>(dbProduct);
                    response = request.CreateResponse(HttpStatusCode.Created, responseData);
                }


                return response;

            });
        }

        [Route("delete")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage Delete(HttpRequestMessage request, ProductViewModel ProductViewModel, int id)
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

                    var dbProduct = _productService.Delete(id);

                    try
                    {
                        _productService.Save();
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

                    var responseData = Mapper.Map<Product, ProductViewModel>(dbProduct);
                    response = request.CreateResponse(HttpStatusCode.OK, responseData);
                }


                return response;

            });
        }

        [Route("deletemulti")]
        [HttpDelete]
        [AllowAnonymous]
        public HttpResponseMessage DeleteMulti(HttpRequestMessage request, string checkedProducts)
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
                    // thư viện JavaScriptSerializer giúp chuyển dữ liệu text dưới dạng JSON qua một đối tượng tương ứng ở đây là lấy về list id
                    //  // thư viện JavaScriptSerializer giúp chuyển đối một đối tượng .NET (hay CLR) bất kì thành một chuỗi JSON và ngược lại. 
                    var listproduct = new JavaScriptSerializer().Deserialize<List<int>>(checkedProducts);
                    foreach (var item in listproduct)
                    {
                        _productService.Delete(item);
                    }

                    try
                    {
                        _productService.Save();
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


                    response = request.CreateResponse(HttpStatusCode.OK, listproduct.Count);
                }


                return response;

            });
        }
        #endregion

        #region import excel add nuget :  EPPLUS , HtmlRenderer.PdfSharp

        [Route("import")]
        [HttpPost]
        public async Task<HttpResponseMessage> Import()
        {
            // kiểm tra định dạng nào hỗ trợ request
            if (!Request.Content.IsMimeMultipartContent())
            {
                Request.CreateErrorResponse(HttpStatusCode.UnsupportedMediaType, "Định dạng không được server hỗ trợ");
            }

            var root = HttpContext.Current.Server.MapPath("~/UploadedFiles/Excels"); // lấy ra đường dẫn gốc lưu file

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            // đọc bất đồng bộ file 
            var provider = new MultipartFormDataStreamProvider(root);
            var result = await Request.Content.ReadAsMultipartAsync(provider);

            //do stuff with files if you wish
            if (result.FormData["categoryId"] == null)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Bạn chưa chọn danh mục sản phẩm.");
            }

            //Upload files
            int addedCount = 0;
            int categoryId = 0;
            int.TryParse(result.FormData["categoryId"], out categoryId); // lấy ra categoryId

            foreach (MultipartFileData fileData in result.FileData)// MultipartFileData các file sẽ được băm ra nhiều phần
            {
                if (string.IsNullOrEmpty(fileData.Headers.ContentDisposition.FileName))
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, "Yêu cầu không đúng định dạng");
                }

                string fileName = fileData.Headers.ContentDisposition.FileName;
                if (fileName.StartsWith("\"") && fileName.EndsWith("\""))
                {
                    fileName = fileName.Trim('"');
                }
                if (fileName.Contains(@"/") || fileName.Contains(@"\"))
                {
                    fileName = Path.GetFileName(fileName);
                }

                var fullPath = Path.Combine(root, fileName);
                File.Copy(fileData.LocalFileName, fullPath, true); // copy location => root

                //insert to DB 
                var listProduct = this.ReadProductFromExcel(fullPath, categoryId); 
                if (listProduct.Count > 0)
                {
                    foreach (var product in listProduct)
                    {
                        _productService.Add(product);
                        addedCount++;
                    }
                    _productService.Save();
                }
            }
            return Request.CreateResponse(HttpStatusCode.OK, "Đã nhập thành công " + addedCount + " sản phẩm thành công.");
        }

        [HttpGet]
        [Route("ExportXls")]
        public async Task<HttpResponseMessage> ExportXls(HttpRequestMessage request, string filter = null)
        {
           
            string fileName = string.Concat("Product_" + DateTime.Now.ToString("yyyyMMddhhmmsss") + ".xlsx");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var data = _productService.GetListProduct(filter).ToList();
                await ReportHelper.GenerateXls(data, fullPath);
                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("ExportPdf")]
        public async Task<HttpResponseMessage> ExportPdf(HttpRequestMessage request, int id)
        {
            // add nuget HtmlRenderer.PdfSharp
            string fileName = string.Concat("Product" + DateTime.Now.ToString("yyyyMMddhhmmssfff") + ".pdf");
            var folderReport = ConfigHelper.GetByKey("ReportFolder");
            string filePath = HttpContext.Current.Server.MapPath(folderReport);
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }
            string fullPath = Path.Combine(filePath, fileName);
            try
            {
                var template = File.ReadAllText(HttpContext.Current.Server.MapPath("/Assets/admin/templates/product-detail.html"));
                var replaces = new Dictionary<string, string>();
                var product = _productService.GetById(id);

                replaces.Add("{{ProductName}}", product.Name);
                replaces.Add("{{Price}}", product.Price.ToString("N0"));
                replaces.Add("{{Description}}", product.Description);
                replaces.Add("{{Warranty}}", product.Warranty + " tháng");

                template = template.Parse(replaces);

                await ReportHelper.GeneratePdf(template, fullPath);
                return request.CreateErrorResponse(HttpStatusCode.OK, Path.Combine(folderReport, fileName));
            }
            catch (Exception ex)
            {
                return request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }



        private List<Product> ReadProductFromExcel(string fullPath, int categoryId)
        {
            // add nuget : epplus
            using (var package = new ExcelPackage(new FileInfo(fullPath)))
            {
                ExcelWorksheet workSheet = package.Workbook.Worksheets[1]; // mảng trong excel bắt đầu từ 1
                List<Product> listProduct = new List<Product>();
                ProductViewModel productViewModel;
                Product product;

                decimal originalPrice = 0;
                decimal price = 0;
                decimal promotionPrice;

                int quantity;
                bool status = false;
                bool showHome = false;
                bool isHot = false;
                int warranty;
                // dòng 1 trong execl là header => chạy từ dòng 2
                for (int i = workSheet.Dimension.Start.Row + 1; i <= workSheet.Dimension.End.Row; i++)
                {
                    // tạo mới 1 product và đọc từng row => mapping
                    productViewModel = new ProductViewModel();
                    product = new Product();

                    productViewModel.Name = workSheet.Cells[i, 1].Value.ToString();
                    productViewModel.Alias = StringHelper.ToUnsignString(productViewModel.Name);
                    productViewModel.Description = workSheet.Cells[i, 2].Value.ToString();

                    if (int.TryParse(workSheet.Cells[i, 3].Value.ToString(), out warranty))
                    {
                        productViewModel.Warranty = warranty;

                    }
                    //TryParse tránh trường hợp throw exception thay vào đó là đưa về giá trị mặc đinh
                    decimal.TryParse(workSheet.Cells[i, 4].Value.ToString().Replace(",", ""), out originalPrice);
                    productViewModel.OriginalPrice = originalPrice;

                    decimal.TryParse(workSheet.Cells[i, 5].Value.ToString().Replace(",", ""), out price);
                    productViewModel.Price = price;

                    if (decimal.TryParse(workSheet.Cells[i, 6].Value.ToString(), out promotionPrice))
                    {
                        productViewModel.PromotionPrice = promotionPrice;

                    }
                    if (int.TryParse(workSheet.Cells[i, 7].Value.ToString(), out quantity))
                    {
                        productViewModel.Quantity = quantity;

                    }

                    productViewModel.Content = workSheet.Cells[i, 8].Value.ToString();
                    productViewModel.MetaKeyword = workSheet.Cells[i, 9].Value.ToString();
                    productViewModel.MetaDescription = workSheet.Cells[i, 10].Value.ToString();
                    productViewModel.Tags = workSheet.Cells[i, 11].Value.ToString();

                    productViewModel.CategoryID = categoryId;

                    bool.TryParse(workSheet.Cells[i, 12].Value.ToString(), out status);
                    productViewModel.Status = status;

                    bool.TryParse(workSheet.Cells[i, 13].Value.ToString(), out showHome);
                    productViewModel.HomeFlag = showHome;

                    bool.TryParse(workSheet.Cells[i, 14].Value.ToString(), out isHot);
                    productViewModel.HotFlag = isHot;

                    //mapping
                    product.UpdateProduct(productViewModel);
                    listProduct.Add(product);
                }
                return listProduct;
            }
        }
        #endregion


    }
}
