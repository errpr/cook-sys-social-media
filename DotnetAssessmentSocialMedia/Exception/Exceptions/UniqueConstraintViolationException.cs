using System.Net;

namespace DotnetAssessmentSocialMedia.Exception.Exceptions
{
    public class UniqueConstraintViolationException : BaseCustomException
    {
        public UniqueConstraintViolationException(string message, string description) : base(message, description, (int)HttpStatusCode.BadRequest)
        {
        }
    }
}
