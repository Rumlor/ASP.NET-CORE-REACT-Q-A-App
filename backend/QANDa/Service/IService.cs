using QANDa.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QANDa.Service
{
    public interface IService
    {
        Task<QuestionGetSingleResponse> PostQuestion(QuestionPostRequest postRequest,string userId,string token);
        Task<IEnumerable<QuestionGetManyResponse>> GetQuestions(bool includeAnswers, int pageSize, int page);
        Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsBySearch(string search, bool includeAnswers, int pageSize, int page);
        Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestionsAsnyc();
        Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestions();
        Task<QuestionGetSingleResponse> GetQuestion(int questionId);
        Task<bool> QuestionExists(int? questionId);
        Task<AnswerGetResponse> GetAnswer(int? answerId);
        Task<QuestionGetSingleResponse> PutQuestion(int questionId, QuestionPutRequest question);
        Task<bool> DeleteQuestion(int questionId);
        Task<AnswerGetResponse> PostAnswer(AnswerPostRequest answer,string token,string userId);
    }
}
