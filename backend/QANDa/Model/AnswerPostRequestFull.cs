using System.ComponentModel.DataAnnotations;
using System;

namespace QANDa.Model
{
    public class AnswerPostRequestFull
    {
        public int? QuestionId { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public DateTime Created { get; set; }
    }
}
