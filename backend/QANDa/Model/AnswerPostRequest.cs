using QANDa.Validator;
using System;
using System.ComponentModel.DataAnnotations;

namespace QANDa.Model
{
    public class AnswerPostRequest
    {
        [Required]
        public int? QuestionId { get; set; }
        [Required]
        public string Content { get; set; }

    }
}
