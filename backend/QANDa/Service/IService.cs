using QANDa.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QANDa.Service
{
    public interface IService
    {
        QuestionGetSingleResponse PostQuestion(QuestionPostRequest postRequest);
        IEnumerable<QuestionGetManyResponse> GetQuestions(bool includeAnswers, int pageSize, int page);
        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search, bool includeAnswers, int pageSize, int page);
        Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestionsAsnyc();
        IEnumerable<QuestionGetManyResponse> GetUnAnsweredQuestions();
        QuestionGetSingleResponse GetQuestion(int questionId);
        bool QuestionExists(int? questionId);
        AnswerGetResponse GetAnswer(int? answerId);
        QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question);
        bool DeleteQuestion(int questionId);
        AnswerGetResponse PostAnswer(AnswerPostRequest answer);
    }
}
