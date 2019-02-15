using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Exception.Exceptions
{
    public class RequiredConstraintViolationException : BaseCustomException
    {
        public RequiredConstraintViolationException(string fieldName) : 
            base("Required field empty or null.",
                $"A required field: {fieldName} was empty or null.", 
                (int)HttpStatusCode.BadRequest)
        {
        }
    }
}
