using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QANDa.Data;
using QANDa.Model;
using QANDa.Service;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace QANDa.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IService _service;
        private readonly IDataCache _cache;
        public QuestionController(IService service, IDataCache cache)
        {
            _service = service;
            _cache = cache;
        }

        [HttpGet]
        public IEnumerable<QuestionGetManyResponse> GetQuestions(string search,bool includeAnswers, int pageSize=20, int page = 1)
        {
            if (string.IsNullOrEmpty(search))
                return _service.GetQuestions(includeAnswers,pageSize,page);
            
            return _service.GetQuestionsBySearch(search,includeAnswers,pageSize,page);
                        
        }

        [HttpGet("unanswered")]
        public  async Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestions()
        {
            return await _service.GetUnAnsweredQuestionsAsnyc();
        }

        [HttpGet("{questionId}")]
        public ActionResult<QuestionGetSingleResponse> GetSingleQuestion(int questionId)
        {
            var cachedQuestion = _cache.Get(questionId);

            if(cachedQuestion == null)
            {
                cachedQuestion = _service.GetQuestion(questionId);
                
                if(cachedQuestion == null)
                    return NotFound ();

                _cache.Set(cachedQuestion);
            }
            
            return cachedQuestion;
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
            if (result)
            {
                _cache.Remove(questionId);
                return NoContent();
            }
            else
                return NotFound();
        }
    }
}
