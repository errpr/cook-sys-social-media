using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Exception.Exceptions
{
    public class HashtagNotFoundException : NotFoundCustomException
    {
        public HashtagNotFoundException() : base("Not Found", $"A Hashtag with given label does not exist")
        {
        }
    }
}
