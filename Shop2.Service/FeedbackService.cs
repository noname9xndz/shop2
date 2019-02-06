﻿using Shop2.Data.Infrastructure;
using Shop2.Data.Repositories;
using Shop2.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop2.Service
{
    //public interface IContactDetailService
    //{
    //    ContactDetail GetDefaultContact();
    //}
    //public class ContactDetailService : IContactDetailService
    //{
    //    IContactDetailRepository _contactDetailRepository;
    //    IUnitOfWork _unitOfWork;

    public interface IFeedbackService
    {
        Feedback Create(Feedback feedback);
        void Save();
    }
    public  class FeedbackService:IFeedbackService
    {
        IFeedbackRepository _feedbackRepository;
        IUnitOfWork _unitOfWork;
        public FeedbackService(IFeedbackRepository feedbackRepository,IUnitOfWork unitOfWork)
        {
            this._feedbackRepository = feedbackRepository;
            this._unitOfWork = unitOfWork;
        }

        public Feedback Create(Feedback feedback)
        {
            return _feedbackRepository.Add(feedback);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }
    }
}
