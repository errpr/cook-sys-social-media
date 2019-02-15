using DotnetAssessmentSocialMedia.Data.Entities;
using System.Collections.Generic;

namespace DotnetAssessmentSocialMedia.Data
{
    public class Context
    {
        public Tweet Target { get; set; }
        public List<Tweet> Before { get; set; }
        public List<Tweet> After { get; set; }
    }
}
