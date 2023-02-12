using QANDa.Model;
using System.Threading.Tasks;

namespace QANDa.Data
{
    public interface IDataRepositoryWrite
    {
        Task<QuestionGetSingleResponse> PostQuestion(QuestionPostFullRequest question);
        Task<QuestionGetSingleResponse> PutQuestion(int questionId, QuestionPutRequest question);
        Task<bool> DeleteQuestion(int questionId);
        Task<AnswerGetResponse> PostAnswer(AnswerPostRequestFull answer);
    }
}
