using Microsoft.Extensions.Configuration;
using QANDa.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QANDa.Data
{
    public class DataRepository : DataRepositoryBase,IDataRepositoryRead, IDataRepositoryWrite
    {

        public DataRepository(IConfiguration configuration): base(configuration)
        {
               
        }
        public async Task<AnswerGetResponse> GetAnswer(int? answerId)
        {
            if(answerId == null) return null;
            return await ExecuteQueryWithDefault<AnswerGetResponse>("EXEC [QandA].[Answer_Get_ByAnswerId] @AnswerId=@AnswerId", new { AnswerId = answerId });
        }
        public  async Task<QuestionGetSingleResponse> GetQuestion(int QuestionId)
        {
            string questionAndAnswersStoredProcedure = @"EXEC QandA.Question_GetSingle @QuestionId=@QuestionId; 
                                                         EXEC [QandA].[Answer_Get_ByQuestionId] @QuestionId=@QuestionId";
            return await ExecuteQueryForMultipleAsync<QuestionGetSingleResponse,AnswerGetResponse>(questionAndAnswersStoredProcedure,"Answers",new {QuestionId});
        }
        public  Task<IEnumerable<QuestionGetManyResponse>> GetQuestions()
        {
            return ExecuteQueryForEnumerable<QuestionGetManyResponse>("EXEC QandA.Question_GetMany",null);
        }
        public  Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsBySearch(string search,bool includeAnswers)
        {
            return ExecuteQueryForEnumerable<QuestionGetManyResponse>("EXEC [QandA].[Question_GetMany_BySearch] @Search=@Search", new { Search = search });
        }
        public  Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestions()
        {
         return ExecuteQueryForEnumerable<QuestionGetManyResponse>(" EXEC [QandA].[Question_GetUnanswered]", null);
        }
        public  async Task<bool> QuestionExists(int? questionId)
        {
            if(!questionId.HasValue || (questionId.HasValue && questionId.Value == 0)) return false;
            return await ExecuteQueryWithDefault<bool>("EXEC [QandA].[Question_Exists] @QuestionId=@QuestionId", new { QuestionId = questionId });
        }
        public  async Task<AnswerGetResponse> PostAnswer(AnswerPostRequestFull answer)
        {
           var questionExists = await QuestionExists(answer.QuestionId.Value);
            if (!questionExists) return null;
           return  await ExecuteQueryFirst<AnswerGetResponse>(@"[QandA].[Answer_Post] 
                                                       @QuestionId=@QuestionId
                                                      ,@Content=@Content
                                                      ,@UserId=@UserId
                                                      ,@UserName=@UserName
                                                      ,@Created=@Created", answer);
        }
        public async Task<QuestionGetSingleResponse> PostQuestion(QuestionPostFullRequest question)
        {
            QuestionGetSingleResponse postedQuestion = await ExecuteQueryFirst<QuestionGetSingleResponse>(@"
                                                                EXEC [QandA].[Question_Post]
		                                                        @Title = @Title,
		                                                        @Content = @Content,
		                                                        @UserId = @UserId,
		                                                        @UserName = @UserName,
		                                                        @Created =  @Created", question);
            return postedQuestion;
        }
        public async Task<QuestionGetSingleResponse> PutQuestion(int questionId, QuestionPutRequest question)
        {
           QuestionGetSingleResponse questionFromDB =  await GetQuestion(questionId);
           if(questionFromDB == null) {
                return null;
           }
           question.Content = string.IsNullOrEmpty(question.Content) ? question.Content : questionFromDB.Content;
           question.Title = string.IsNullOrEmpty(question.Title) ? question.Title : questionFromDB.Title;
           await Execute(@"EXEC [QandA].[Question_Put]
                     @QuestionId=@QuestionId,
                     @Title=@Title,
                     @Content=@Content", new {QuestionId=questionId, question.Title,question.Content});
            return new QuestionGetSingleResponse{ QuestionId = questionId, Title=question.Title, Content=question.Content };
        }
        public  async Task<bool> DeleteQuestion(int questionId)
        {
            var question = await GetQuestion(questionId);
            if (question == null)
                return false; 
            await Execute(@"EXEC [QandA].[Question_Delete] @QuestionId=@QuestionId", new { QuestionId = questionId });
            return true;
        }
        public async Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsWithAnswers()
        {
            return  await ExecuteMappedQuery<QuestionGetManyResponse,AnswerGetResponse>("EXEC [QandA].[Question_GetMany_WithAnswers]", null, "Answers", "QuestionId","AnswerId");
            
        }
        public async Task<IEnumerable<QuestionGetManyResponse>> GetUnAnsweredQuestionsAsync()
        {
           return await  ExecuteQueryFromEnumerableAsync<QuestionGetManyResponse>("EXEC QandA.Question_GetUnanswered", null);
            
        }
        public Task<IEnumerable<QuestionGetManyResponse>> GetQuestionsPaging(string search, int pageNumber, int pageSize)
        {
            if (search != null)
            {
                return ExecuteQueryForEnumerable<QuestionGetManyResponse>(
                     @"EXEC QandA.Question_GetMany_BySearch_WithPaging
                  @Search=@Search,@PageNumber=@PageNumber,@PageSize=@PageSize", new { Search = search, PageNumber = pageNumber, PageSize = pageSize });
            }
            else
            {
                return ExecuteQueryForEnumerable<QuestionGetManyResponse>(
                    @"EXEC [QandA].[Question_GetMany_WithPaging] 
                    @PageNumber=@PageNumber,@PageSize=@PageSize", new { PageNumber = pageNumber, PageSize = pageSize });
            }
        }
    }
}
