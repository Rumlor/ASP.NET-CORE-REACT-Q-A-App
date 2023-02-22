using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using QANDa.Data;
using QANDa.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace QANDa.Service
{
    public class QuestionAnswerService : IService
    {
        private readonly IDataRepositoryRead _dataRepositoryRead;
        private readonly IDataRepositoryWrite _dataRepositoryWrite;
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _auth0UserInfoUri;
        public QuestionAnswerService(IConfiguration config ,IDataRepositoryRead dataRepositoryRead, IDataRepositoryWrite dataRepositoryWrite,IHttpClientFactory factory)
        {
            _dataRepositoryRead = dataRepositoryRead;
            _dataRepositoryWrite = dataRepositoryWrite;
            _clientFactory = factory;
            _auth0UserInfoUri = $"{config["Auth0:Authority"]}userinfo";
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

        public async Task<QuestionGetSingleResponse> PostQuestion(QuestionPostRequest postRequest,string userId,string token)
        {
            object userName = "";
            var response = await SendAsyncHttpRequest(HttpMethod.Get, _auth0UserInfoUri, token);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dictionaryResponse = JsonSerializer.Deserialize<Dictionary<string,object>>(content,new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                dictionaryResponse.TryGetValue("name", out userName);
            }

            QuestionPostFullRequest fullRequest = new()
            {
                Title = postRequest.Title,
                Content = postRequest.Content,
                Created = DateAndTime.Now,
                UserId = userId,
                UserName = userName.ToString(),
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

        public async Task<AnswerGetResponse> PostAnswer(AnswerPostRequest answer, string token, string userId)
        {
            object userName = "";
            var response = await SendAsyncHttpRequest(HttpMethod.Get, _auth0UserInfoUri, token);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var dictionaryResponse = JsonSerializer.Deserialize<Dictionary<string, object>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                dictionaryResponse.TryGetValue("name", out userName);
            }

            return await _dataRepositoryWrite.PostAnswer(
                                                        new AnswerPostRequestFull { Content = answer.Content, 
                                                                                    Created = DateAndTime.Now, 
                                                                                    QuestionId = answer.QuestionId , 
                                                                                    UserId = userId,
                                                                                    UserName = userName.ToString()
                                                        });
        }

        public async Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestions()
        {
            return await _dataRepositoryRead.GetUnAnsweredQuestions();
        }
    
        private async Task<HttpResponseMessage> SendAsyncHttpRequest(HttpMethod method,string uri,string token)
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Get, uri);
            httpRequest.Headers.Add("Authorization", token);
            var client = _clientFactory.CreateClient();
            return await client.SendAsync(httpRequest);
        }
    }
}
