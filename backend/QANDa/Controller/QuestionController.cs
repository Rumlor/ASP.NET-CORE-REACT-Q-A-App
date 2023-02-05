using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QANDa.Data;
using QANDa.Model;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace QANDa.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IDataRepositoryRead _dataRepositoryRead;
        private readonly IDataRepositoryWrite _dataRepositoryWrite;

        public QuestionController(IDataRepositoryRead dataRepositoryRead, IDataRepositoryWrite dataRepositoryWrite)
        {
            _dataRepositoryRead = dataRepositoryRead;
            _dataRepositoryWrite = dataRepositoryWrite;
        }

        [HttpGet]
        public IEnumerable<QuestionGetManyResponse> GetQuestions([FromQuery] string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return _dataRepositoryRead.GetQuestions();
            }
            else
            {
                return _dataRepositoryRead.GetQuestionsBySearch(search);
            }            
        }

        [HttpGet("unanswered")]
        public IEnumerable<QuestionGetManyResponse> GetUnAnsweredQuestions()
        {
            return _dataRepositoryRead.GetUnAnsweredQuestions();
        }

        [HttpGet("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> GetSingleQuestion(int questionId)
        {
            QuestionGetSingleResponse response = _dataRepositoryRead.GetQuestion(questionId);
            if(response == null)
            {
                return NotFound();
            }
            return response;
        }
        
        [HttpPost]
        public ActionResult<QuestionGetSingleResponse> PostQuestion(QuestionPostRequest reqeuest)
        {
            var posted = _dataRepositoryWrite.PostQuestion(reqeuest);
            return CreatedAtAction(nameof(PostQuestion), posted);
        }
        
        [HttpPut("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> PutQuestion(int questionId,QuestionPutRequest reqeuest)
        {
           var repsonse = _dataRepositoryWrite.PutQuestion(questionId,reqeuest);
            if (repsonse == null)
            {
                return NotFound();
            }
            return CreatedAtAction(nameof(PutQuestion),repsonse);
        }

        [HttpDelete("{questionId}")]
        public ActionResult DeleteQuestion(int questionId)
        {
           var result = _dataRepositoryWrite.DeleteQuestion(questionId);
            if(result)
                return NoContent();
            else
                return NotFound();
        }
    }
}
