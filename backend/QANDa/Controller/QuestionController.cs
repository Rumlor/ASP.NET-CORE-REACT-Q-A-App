using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QANDa.Data;
using QANDa.Model;
using QANDa.Service;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QANDa.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IEnumerable<QuestionGetManyResponse>> GetQuestions(string search,bool includeAnswers, int pageSize=20, int page = 1)
        {
            if (string.IsNullOrEmpty(search))
                return await _service.GetQuestions(includeAnswers,pageSize,page);
            
            return await _service.GetQuestionsBySearch(search,includeAnswers,pageSize,page);
                        
        }

        [HttpGet("unanswered")]
        [AllowAnonymous]
        public  async Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestions()
        {
            return await _service.GetUnAnsweredQuestionsAsnyc();
        }

        [HttpGet("{questionId}")]
        [AllowAnonymous]
        public async Task<ActionResult<QuestionGetSingleResponse>> GetSingleQuestion(int questionId)
        {
            var cachedQuestion = _cache.Get(questionId);

            if(cachedQuestion == null)
            {
                cachedQuestion = await _service.GetQuestion(questionId);
                
                if(cachedQuestion == null)
                    return NotFound ();

                _cache.Set(cachedQuestion);
            }
            
            return cachedQuestion;
        }
        
        [HttpPost]
        public async Task<ActionResult<QuestionGetSingleResponse>> PostQuestion(QuestionPostRequest reqeuest)
        {
            if (reqeuest == null)
                return BadRequest();

            string userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            string token = Request.Headers["Authorization"].First();
            var posted = await _service.PostQuestion(reqeuest, userId,token);
            return CreatedAtAction(nameof(PostQuestion), posted);
        }
        
        [HttpPut("{questionId}")]
        [Authorize(Policy = "MustBeQuestionAuthor")]
        public async Task<ActionResult<QuestionGetSingleResponse>> PutQuestion(int questionId,QuestionPutRequest reqeuest)
        {
           var repsonse = await _service.PutQuestion(questionId,reqeuest);
            if (repsonse == null)
            {
                return NotFound();
            }
           
            return CreatedAtAction(nameof(PutQuestion),repsonse);
        }

        [HttpDelete("{questionId}")]
        [Authorize(Policy = "MustBeQuestionAuthor")]
        public async Task<ActionResult> DeleteQuestion(int questionId)
        {
           var result = await _service.DeleteQuestion(questionId);
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
