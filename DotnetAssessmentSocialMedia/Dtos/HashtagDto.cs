using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Dtos
{
    public class HashtagDto
    {
        public string Label { get; set; }
        public DateTime FirstUsed { get; set; }
        public DateTime LastUsed { get; set; }
    }
}
