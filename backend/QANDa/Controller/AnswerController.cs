using Microsoft.AspNetCore.Mvc;
using QANDa.Data;
using QANDa.Model;
using QANDa.Service;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QANDa.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IService _service;
        private readonly IDataCache _cache;
        
        public AnswerController(IService service,IDataCache dataCache) {
            _service = service;
            _cache = dataCache;
        }

        [HttpGet("{answerId}")]
        public  ActionResult<AnswerGetResponse> GetAnswer(int answerId) {
            var answer = _service.GetAnswer(answerId);
            if (answer == null) return NotFound();
            else return new JsonResult(answer);
        }

        [HttpPost]
        public ActionResult<AnswerGetResponse> PostAnswer(AnswerPostRequest answerPost)
        {
           var result = _service.PostAnswer(answerPost);
            if(result == null) return NotFound();
            _cache.Remove(answerPost.QuestionId.Value);
            return result;

        }
  
    }
}
