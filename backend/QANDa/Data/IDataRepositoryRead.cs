using QANDa.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QANDa.Data
{
    public interface IDataRepositoryRead
    {
        Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsPaging(string search,int pageNumber,int pageSize);
        Task<IEnumerable<QuestionGetManyResponse>> GetQuestions();
        Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsWithAnswers();
        Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestions();
        Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestionsAsync();
        Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsBySearch(string search,bool includeAnswers);
        Task<QuestionGetSingleResponse> GetQuestion(int questionId);
        Task<bool> QuestionExists(int? questionId);
        Task<AnswerGetResponse> GetAnswer(int? answerId);
    }
}
