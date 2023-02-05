using System.Collections.Generic;
using System;

namespace QANDa.Model
{
    public class QuestionGetSingleResponse
    {
        public int QuestionId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public IEnumerable<AnswerGetResponse> Answers{ get; set; } = new List<AnswerGetResponse>();

    }
}
