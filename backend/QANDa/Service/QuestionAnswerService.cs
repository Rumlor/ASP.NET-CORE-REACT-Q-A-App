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

        public async Task<AnswerGetResponse> GetAnswer(int? answerId)
        {
           return  await _dataRepositoryRead.GetAnswer(answerId);
        }

        public async Task<QuestionGetSingleResponse> GetQuestion(int questionId)
        {
            return await _dataRepositoryRead.GetQuestion(questionId);
        }

        public async Task<bool> QuestionExists(int? questionId)
        {
            return await _dataRepositoryRead.QuestionExists(questionId);
        }

        public async Task<IEnumerable<QuestionGetManyResponse>> GetQuestions(bool includeAnswers, int page, int pageSize)
        {   //to be implemented
            if (includeAnswers)
                return await _dataRepositoryRead.GetQuestionsPaging(null,page, pageSize);

            return await _dataRepositoryRead.GetQuestions();
        }

        public async Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsBySearch(string search,bool includeAnswers,int pageSize,int page)
        {
            return await _dataRepositoryRead.GetQuestionsPaging(search,page,pageSize);
        }

        public  async Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestionsAsnyc()
        {
          return await _dataRepositoryRead.GetUnAnsweredQuestionsAsync();
        }

        public async Task<QuestionGetSingleResponse> PostQuestion(QuestionPostRequest postRequest)
        {
            QuestionPostFullRequest fullRequest = new()
            {
                Title = postRequest.Title,
                Content = postRequest.Content,
                Created = DateAndTime.Now,
                UserId = "id",
                UserName = "Demo"
            };
            
            return await _dataRepositoryWrite.PostQuestion(fullRequest);
        }

        public async Task<QuestionGetSingleResponse> PutQuestion(int questionId, QuestionPutRequest question)
        {
            return await _dataRepositoryWrite.PutQuestion(questionId, question);
        }

        public async Task<bool> DeleteQuestion(int questionId)
        {
            return await _dataRepositoryWrite.DeleteQuestion(questionId);
        }

        public async Task<AnswerGetResponse> PostAnswer(AnswerPostRequest answer)
        {
            return await _dataRepositoryWrite.PostAnswer(
                                                        new AnswerPostRequestFull { Content= answer.Content, 
                                                                                    Created= DateAndTime.Now, 
                                                                                    QuestionId=answer.QuestionId , 
                                                                                    UserId="id",
                                                                                    UserName="demo" 
                                                        });
        }

        public async Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestions()
        {
            return await _dataRepositoryRead.GetUnAnsweredQuestions();
        }
    }
}
