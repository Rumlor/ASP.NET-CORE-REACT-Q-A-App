using Microsoft.AspNetCore.Mvc;
using QANDa.Data;
using QANDa.Model;
using System.Collections.Generic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QANDa.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswerController : ControllerBase
    {
        private readonly IDataRepositoryRead _dataRepositoryRead;
        private readonly IDataRepositoryWrite _dataRepositoryWrite;
        
        public AnswerController(IDataRepositoryRead read, IDataRepositoryWrite write) {
            _dataRepositoryRead =  read;
            _dataRepositoryWrite = write;
        }

        [HttpGet("{answerId}")]
        public  ActionResult<AnswerGetResponse> GetAnswer(int answerId) {
            var answer = _dataRepositoryRead.GetAnswer(answerId);
            if (answer == null) return NotFound();
            else return new JsonResult(answer);
        }

        [HttpPost]
        public ActionResult<AnswerGetResponse> PostAnswer(AnswerPostRequest answerPost)
        {
           var result = _dataRepositoryWrite.PostAnswer(answerPost);
            if(result == null) return NotFound();
            return result;

        }
  
    }
}
