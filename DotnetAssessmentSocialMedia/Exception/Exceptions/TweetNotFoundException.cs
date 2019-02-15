using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Exception.Exceptions
{
    public class TweetNotFoundException : NotFoundCustomException
    {
        public TweetNotFoundException() : base("Not Found", "A tweet with given id does not exist.")
        {
        }
    }
}
