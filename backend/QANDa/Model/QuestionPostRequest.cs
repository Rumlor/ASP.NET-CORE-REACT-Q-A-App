using System;
using System.ComponentModel.DataAnnotations;

namespace QANDa.Model
{
    public class QuestionPostRequest
    {
        [Required(ErrorMessage ="Please ensure you entered title.")]
        [StringLength(100)]
        public string Title { get; set; }
        [Required(ErrorMessage ="Please ensure you entered content.")]
        public string Content { get; set; }
    }
}
