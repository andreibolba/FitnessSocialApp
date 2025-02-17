﻿using API.Dtos;
using API.Interfaces.Repository;
using API.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class FeedbacksRepository : IFeedbackRepository
    {
        private readonly InternShipAppSystemContext _context;
        private readonly IPictureRepository _pictureRepository;
        private readonly IMapper _mapper;

        public FeedbacksRepository(InternShipAppSystemContext context, IPictureRepository pictureRepository, IMapper mapper)
        {
            _context = context;
            _pictureRepository = pictureRepository;
            _mapper = mapper;
        }

        public FeedbackDto AddFeedback(FeedbackDto feedbackDto)
        {
            var feedback = _mapper.Map<Feedback>(feedbackDto);
            feedback.DateOfPost = DateTime.Now;
            _context.Feedbacks.Add(feedback);

            return SaveAll() ? _mapper.Map<FeedbackDto>(feedback) : null;
        }

        public void DeleteFeedback(int id)
        {
            var feebdack = _mapper.Map<Feedback>(GetFeedbackById(id));
            feebdack.Deleted = true;

            _context.Feedbacks.Update(feebdack);
        }

        public FeedbackDto EditFeedback(FeedbackDto feedbackDto)
        {
            var feebdack = _mapper.Map<Feedback>(_context.Feedbacks.SingleOrDefault(f=>f.FeedbackId==feedbackDto.FeedbackId));

            feebdack.Content = feedbackDto.Content==null ? feebdack.Content : feedbackDto.Content;
            feebdack.Grade = feedbackDto.Grade != null ? feebdack.Grade : feedbackDto.Grade.Value;
            feebdack.ChallangeId = feedbackDto.ChallangeId;
            feebdack.TaskId = feedbackDto.TaskId;
            feebdack.TestId = feedbackDto.TestId;

            _context.Feedbacks.Update(feebdack);

            return SaveAll() ? _mapper.Map<FeedbackDto>(feebdack) : null;
        }

        public IEnumerable<FeedbackDto> GetAllFeedback()
        {
            var res = _context.Feedbacks.Where(f => f.Deleted == false).OrderByDescending(t=>t.DateOfPost)
                .Include(t => t.Task)
                .Include(t => t.Challange)
                .Include(t => t.PersonReceiver)
                .Include(t => t.PersonSender)
                .Include(t => t.Test);

            var result = _mapper.Map<IEnumerable<FeedbackDto>>(res);

            foreach(var item in result)
            {
                item.PersonSender.Picture = item.PersonSender.PictureId!=null? _pictureRepository.GetById(item.PersonSender.PictureId.Value): null;
                item.PersonReceiver.Picture = item.PersonReceiver.PictureId!=null? _pictureRepository.GetById(item.PersonReceiver.PictureId.Value): null;
            }

            return result;
        }

        public IEnumerable<FeedbackDto> GetAllFeedbackForSender(int senderId, int? count = null)
        {
            var getAllFeedback = GetAllFeedback().Where(f => f.PersonSenderId == senderId);
            if (count != null && getAllFeedback.Count() >= count.Value)
                return getAllFeedback.Take(count.Value);
            return getAllFeedback;
        }

        public IEnumerable<FeedbackDto> GetAllFeedbackForReceiver(int receiverId, int? count = null)
        {
            var getAllFeedback = GetAllFeedback().Where(f => f.PersonReceiverId == receiverId);
            if (count != null && getAllFeedback.Count() >= count.Value)
                return getAllFeedback.Take(count.Value);
            return getAllFeedback;
        }

        public FeedbackDto GetFeedbackById(int id)
        {
            return GetAllFeedback().SingleOrDefault(f=>f.FeedbackId == id);
        }

        public bool SaveAll()
        {
            return _context.SaveChanges()>0;
        }
    }
}
