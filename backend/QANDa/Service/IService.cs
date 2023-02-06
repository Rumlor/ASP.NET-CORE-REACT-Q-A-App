using QANDa.Model;
using System.Collections.Generic;

namespace QANDa.Service
{
    public interface IService
    {
        QuestionGetSingleResponse PostQuestion(QuestionPostRequest postRequest);
        IEnumerable<QuestionGetManyResponse> GetQuestions(bool includeAnswers);
        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search, bool includeAnswers);
        IEnumerable<QuestionGetManyResponse> GetUnAnsweredQuestions();
        QuestionGetSingleResponse GetQuestion(int questionId);
        bool QuestionExists(int? questionId);
        AnswerGetResponse GetAnswer(int? answerId);
        QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question);
        bool DeleteQuestion(int questionId);
        AnswerGetResponse PostAnswer(AnswerPostRequest answer);
    }
}
