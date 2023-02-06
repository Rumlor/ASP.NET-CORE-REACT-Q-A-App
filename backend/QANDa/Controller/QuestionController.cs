using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QANDa.Data;
using QANDa.Model;
using QANDa.Service;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QANDa.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IService _service;
        public QuestionController(IService service)
        {
            _service = service;
        }

        [HttpGet]
        public IEnumerable<QuestionGetManyResponse> GetQuestions([FromQuery] string search,[FromQuery]bool includeAnswers)
        {
            if (string.IsNullOrEmpty(search))
            {
                return _service.GetQuestions(includeAnswers);
            }
            else
            {
                return _service.GetQuestionsBySearch(search,includeAnswers);
            }            
        }

        [HttpGet("unanswered")]
        public IEnumerable<QuestionGetManyResponse> GetUnAnsweredQuestions()
        {
            return _service.GetUnAnsweredQuestions();
        }

        [HttpGet("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> GetSingleQuestion(int questionId)
        {
            QuestionGetSingleResponse response = _service.GetQuestion(questionId);
            if(response == null)
            {
                return NotFound();
            }
            return response;
        }
        
        [HttpPost]
        public ActionResult<QuestionGetSingleResponse> PostQuestion(QuestionPostRequest reqeuest)
        {
            var posted = _service.PostQuestion(reqeuest);
            return CreatedAtAction(nameof(PostQuestion), posted);
        }
        
        [HttpPut("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> PutQuestion(int questionId,QuestionPutRequest reqeuest)
        {
           var repsonse = _service.PutQuestion(questionId,reqeuest);
            if (repsonse == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(PutQuestion),repsonse);
        }

        [HttpDelete("{questionId}")]
        public ActionResult DeleteQuestion(int questionId)
        {
           var result = _service.DeleteQuestion(questionId);
            if(result)
                return NoContent();
            else
                return NotFound();
        }
    }
}
