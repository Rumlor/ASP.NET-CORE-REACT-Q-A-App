using QANDa.Model;

namespace QANDa.Data
{
    public interface IDataCache
    {
        QuestionGetSingleResponse Get(int questionId);
        void Remove(int questionId);
        QuestionGetSingleResponse Set(QuestionGetSingleResponse question);
    }
}
