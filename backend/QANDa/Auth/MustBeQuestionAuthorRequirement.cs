using Microsoft.AspNetCore.Authorization;

namespace QANDa.Auth
{
    public class MustBeQuestionAuthorRequirement : IAuthorizationRequirement
    {
        public MustBeQuestionAuthorRequirement() { }
    }
}
