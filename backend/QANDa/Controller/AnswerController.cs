using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QANDa.Data;
using QANDa.Model;
using QANDa.Service;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QANDa.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<ActionResult<AnswerGetResponse>> PostAnswer(AnswerPostRequest answerPost)
        {
           var result = await _service.PostAnswer(answerPost, Request.Headers["Authorization"],User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if(result == null) return NotFound();
            _cache.Remove(answerPost.QuestionId.Value);
            return result;

        }
  
    }
}
