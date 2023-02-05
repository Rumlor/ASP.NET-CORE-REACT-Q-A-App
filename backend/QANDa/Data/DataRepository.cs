using Microsoft.Extensions.Configuration;
using QANDa.Model;
using System.Collections.Generic;

namespace QANDa.Data
{
    public class DataRepository : DataRepositoryBase,IDataRepositoryRead, IDataRepositoryWrite
    {

        public DataRepository(IConfiguration configuration): base(configuration)
        {
               
        }
        public  AnswerGetResponse GetAnswer(int? answerId)
        {
            if(answerId == null) return null;
            return ExecuteQueryWithDefault<AnswerGetResponse>("EXEC [QandA].[Answer_Get_ByAnswerId] @AnswerId=@AnswerId", new { AnswerId = answerId });
        }

        public  QuestionGetSingleResponse GetQuestion(int questionId)
        {
            QuestionGetSingleResponse questionResponse = ExecuteQueryWithDefault<QuestionGetSingleResponse>("EXEC [QandA].[Question_GetSingle] @QuestionId=@QuestionId", new { QuestionId = questionId });
            if(questionResponse != null) {
                questionResponse.Answers = ExecuteQueryForEnumerable<AnswerGetResponse>("EXEC [QandA].[Answer_Get_ByQuestionId] @QuestionId=@QuestionId", new { QuestionId = questionId });
            }
            return questionResponse;
        }

        public  IEnumerable<QuestionGetManyResponse> GetQuestions()
        {
            return ExecuteQueryForEnumerable<QuestionGetManyResponse>("EXEC QandA.Question_GetMany",null);
        }

        public  IEnumerable<QuestionGetManyResponse> GetQuestionsBySearch(string search)
        {
            return ExecuteQueryForEnumerable<QuestionGetManyResponse>("EXEC [QandA].[Question_GetMany_BySearch] @Search=@Search", new {Search=search});
        }

        public  IEnumerable<QuestionGetManyResponse> GetUnAnsweredQuestions()
        {
         return ExecuteQueryForEnumerable<QuestionGetManyResponse>(" EXEC [QandA].[Question_GetUnanswered]", null);
        }
        public  bool QuestionExists(int? questionId)
        {
            if(!questionId.HasValue || (questionId.HasValue && questionId.Value == 0)) return false;
            return ExecuteQueryWithDefault<bool>("EXEC [QandA].[Question_Exists] @QuestionId=@QuestionId", new { QuestionId = questionId });
        }

        public  AnswerGetResponse PostAnswer(AnswerPostRequestFull answer)
        {
           var questionExists = QuestionExists(answer.QuestionId.Value);
            if (!questionExists) return null;
           return  ExecuteQueryFirst<AnswerGetResponse>(@"[QandA].[Answer_Post] 
                                                       @QuestionId=@QuestionId
                                                      ,@Content=@Content
                                                      ,@UserId=@UserId
                                                      ,@UserName=@UserName
                                                      ,@Created=@Created", answer);
        }

        public  QuestionGetSingleResponse PostQuestion(QuestionPostFullRequest question)
        {
            QuestionGetSingleResponse postedQuestion = ExecuteQueryFirst<QuestionGetSingleResponse>(@"
                                                           EXEC [QandA].[Question_Post]
		                                                        @Title = @Title,
		                                                        @Content = @Content,
		                                                        @UserId = @UserId,
		                                                        @UserName = @UserName,
		                                                        @Created =  @Created", question);
            return postedQuestion;
        }

        public  QuestionGetSingleResponse PutQuestion(int questionId, QuestionPutRequest question)
        {
           QuestionGetSingleResponse questionFromDB =  GetQuestion(questionId);
           if(questionFromDB == null) {
                return null;
           }
           question.Content = string.IsNullOrEmpty(question.Content) ? question.Content : questionFromDB.Content;
           question.Title = string.IsNullOrEmpty(question.Title) ? question.Title : questionFromDB.Title;
           Execute(@"EXEC [QandA].[Question_Put]
                     @QuestionId=@QuestionId,
                     @Title=@Title,
                     @Content=@Content", new {QuestionId=questionId, question.Title,question.Content});
            return new QuestionGetSingleResponse{ QuestionId = questionId, Title=question.Title, Content=question.Content };
        }

        public  bool DeleteQuestion(int questionId)
        {
            var question = GetQuestion(questionId);
            if (question == null)
                return false; 
            Execute(@"EXEC [QandA].[Question_Delete] @QuestionId=@QuestionId", new { QuestionId = questionId });
            return true;
        }
    }
}
