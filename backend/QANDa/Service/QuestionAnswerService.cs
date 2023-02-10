using Microsoft.VisualBasic;
using QANDa.Data;
using QANDa.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QANDa.Service
{
    public class QuestionAnswerService : IService
    {
        private readonly IDataRepositoryRead _dataRepositoryRead;
        private readonly IDataRepositoryWrite _dataRepositoryWrite;

        public QuestionAnswerService(IDataRepositoryRead dataRepositoryRead, IDataRepositoryWrite dataRepositoryWrite)
        {
            _dataRepositoryRead = dataRepositoryRead;
            _dataRepositoryWrite = dataRepositoryWrite;
        }

        public AnswerGetResponse GetAnswer(int? answerId)
        {
           return _dataRepositoryRead.GetAnswer(answerId);
        }

        public QuestionGetSingleResponse GetQuestion(int questionId)
        {
            return _dataRepositoryRead.GetQuestion(questionId);
        }

        public bool QuestionExists(int? questionId)
        {
            return _dataRepositoryRead.QuestionExists(questionId);
        }

        public IEnumerable<QuestionGetManyResponse> GetQuestions(bool includeAnswers, int page, int pageSize)
        {   //to be implemented
            if (includeAnswers)
                return _dataRepositoryRead.GetQuestionsPaging(null,page, pageSize);

            return _dataRepositoryRead.GetQuestions();
        }

        public IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search,bool includeAnswers,int pageSize,int page)
        {
            return _dataRepositoryRead.GetQuestionsPaging(search,page,pageSize);
        }

        public  async Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestionsAsnyc()
        {
          return await _dataRepositoryRead.GetUnAnsweredQuestionsAsync();
        }

        public QuestionGetSingleResponse PostQuestion(QuestionPostRequest postRequest)
        {
            QuestionPostFullRequest fullRequest = new QuestionPostFullRequest
            {
                Title = postRequest.Title,
                Content = postRequest.Content,
                Created = DateAndTime.Now,
                UserId = "id",
                UserName = "Demo"
            };
            
            return _dataRepositoryWrite.PostQuestion(fullRequest);
        }

        public QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question)
        {
            return _dataRepositoryWrite.PutQuestion(questionId, question);
        }

        public bool DeleteQuestion(int questionId)
        {
            return _dataRepositoryWrite.DeleteQuestion(questionId);
        }

        public AnswerGetResponse PostAnswer(AnswerPostRequest answer)
        {
            return _dataRepositoryWrite.PostAnswer(
                new AnswerPostRequestFull { Content= answer.Content, 
                                            Created= DateAndTime.Now, 
                                            QuestionId=answer.QuestionId , 
                                            UserId="id",
                                            UserName="demo" 
                });
        }

        public IEnumerable<QuestionGetManyResponse> GetUnAnsweredQuestions()
        {
            return _dataRepositoryRead.GetUnAnsweredQuestions();
        }
    }
}
