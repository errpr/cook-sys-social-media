using DotnetAssessmentSocialMedia.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Dtos
{
    public class CreateTweetDto
    {
        public Credentials Credentials { get; set; }
        public string Content { get; set; }
    }
}
