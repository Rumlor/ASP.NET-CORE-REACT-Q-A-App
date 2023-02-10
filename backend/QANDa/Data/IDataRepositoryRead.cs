using QANDa.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QANDa.Data
{
    public interface IDataRepositoryRead
    {
        IEnumerable<QuestionGetManyResponse> GetQuestionsPaging(string search,int pageNumber,int pageSize);
        IEnumerable<QuestionGetManyResponse> GetQuestions();
        IEnumerable<QuestionGetManyResponse> GetQuestionsWithAnswers();
        IEnumerable<QuestionGetManyResponse> GetUnAnsweredQuestions();
        Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestionsAsync();
        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search,bool includeAnswers);
        QuestionGetSingleResponse GetQuestion(int questionId);
        bool QuestionExists(int? questionId);
        AnswerGetResponse GetAnswer(int? answerId);
    }
}
