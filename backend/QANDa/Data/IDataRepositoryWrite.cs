using QANDa.Model;

namespace QANDa.Data
{
    public interface IDataRepositoryWrite
    {
        QuestionGetSingleResponse PostQuestion(QuestionPostFullRequest question);
        QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question);
        bool DeleteQuestion(int questionId);
        AnswerGetResponse PostAnswer(AnswerPostRequestFull answer);
    }
}
