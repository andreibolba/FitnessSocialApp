﻿using API.Dtos;
using API.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class FeedbacksController : BaseAPIController
    {
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IPersonRepository _personRepository;

        public FeedbacksController(IFeedbackRepository feedbackRepository, IPersonRepository personRepository)
        {
            _feedbackRepository = feedbackRepository;
            _personRepository = personRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FeedbackDto>> GetAllFeedbacks()
        {
            return Ok(_feedbackRepository.GetAllFeedback());
        }

        [HttpGet("{feedbackId:int}")]
        public ActionResult<FeedbackDto> GetOneFeedback(int meetId)
        {
            return Ok(_feedbackRepository.GetFeedbackById(meetId));
        }

        [HttpGet("{id:int}/{status}/{count?}")]
        public ActionResult<IEnumerable<FeedbackDto>> GetFeedbackForPerson(int id, string status, int? count = null)
        {
            switch (status)
            {
                case "receiver":
                    return Ok(_feedbackRepository.GetAllFeedbackForReceiver(id, count));
                case "sender":
                    return Ok(_feedbackRepository.GetAllFeedbackForSender(id, count));
                default:
                    return BadRequest("Invalid option");
            }
        }

        [HttpPost("add")]
        public ActionResult Create([FromBody] FeedbackDto feedbackDto)
        {
            var feedback = _feedbackRepository.AddFeedback(feedbackDto);
            return feedback != null ? Ok(_feedbackRepository.GetFeedbackById(feedback.FeedbackId)) : BadRequest("Internal Server Error");
        }

        [HttpPost("edit")]
        public ActionResult Update([FromBody] FeedbackDto feedbackDto)
        {
            var feedback = _feedbackRepository.EditFeedback(feedbackDto);
            return feedback!=null ? Ok(feedback) : BadRequest("Internal Server Error");
        }

        [HttpPost("delete/{feedbackId:int}")]
        public ActionResult Delete(int feedbackId)
        {
            _feedbackRepository.DeleteFeedback(feedbackId);
            return _feedbackRepository.SaveAll() ? Ok() : BadRequest("Internal Server Error");
        }
    }
}
