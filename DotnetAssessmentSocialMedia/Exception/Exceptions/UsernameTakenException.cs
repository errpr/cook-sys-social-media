using System.Net;

namespace DotnetAssessmentSocialMedia.Exception.Exceptions
{
    public class UsernameTakenException : UniqueConstraintViolationException
    {
        public UsernameTakenException() : base("Username taken", "The username provided is already in use")
        {
        }
    }
}