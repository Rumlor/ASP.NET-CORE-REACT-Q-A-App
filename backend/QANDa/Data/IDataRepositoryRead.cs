using QANDa.Model;
using System.Collections.Generic;

namespace QANDa.Data
{
    public interface IDataRepositoryRead
    {
        IEnumerable<QuestionGetManyResponse> GetQuestions();
        IEnumerable<QuestionGetManyResponse> GetQuestionsWithAnswers();
        IEnumerable<QuestionGetManyResponse> GetUnAnsweredQuestions();
        IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search);
        QuestionGetSingleResponse GetQuestion(int questionId);
        bool QuestionExists(int? questionId);
        AnswerGetResponse GetAnswer(int? answerId);
    }
}
