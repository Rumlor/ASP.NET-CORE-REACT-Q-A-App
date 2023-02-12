using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using QANDa.Data;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QANDa.Auth
{
    public class MustBeQuestionAuthorHandler : AuthorizationHandler<MustBeQuestionAuthorRequirement>
    {
        private readonly IDataRepositoryRead _dataRepositoryRead;
        private readonly IHttpContextAccessor _contextAccessor;
        
        public MustBeQuestionAuthorHandler(IDataRepositoryRead read, IHttpContextAccessor accessor)
        {
            _dataRepositoryRead = read;
            _contextAccessor = accessor;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, MustBeQuestionAuthorRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return;
            }
            int questionId = Convert.ToInt32(_contextAccessor.HttpContext.Request.RouteValues["questionId"]);
            string userId = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var question = await _dataRepositoryRead.GetQuestion(questionId);
            
            if (question == null)
            {
                context.Succeed(requirement);
                return;
            }
            if(question.UserId != userId)
            {
                context.Fail();
                return;
            }
            context.Succeed(requirement);
        }
    }
}
