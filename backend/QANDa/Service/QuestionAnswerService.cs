using Microsoft.VisualBasic;
using QANDa.Data;
using QANDa.Model;
using System.Collections.Generic;

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

        public IEnumerable<QuestionGetManyResponse> GetQuestions(bool includeAnswers)
        {   if (includeAnswers)
                return _dataRepositoryRead.GetQuestionsWithAnswers();

            return _dataRepositoryRead.GetQuestions();
        }

        public IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search,bool includeAnswers)
        {
            return _dataRepositoryRead.GetQuestionsBySearch(search,includeAnswers);
        }

        public IEnumerable<QuestionGetManyResponse> GetUnAnsweredQuestions()
        {
          return _dataRepositoryRead.GetUnAnsweredQuestions();
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
    }
}
