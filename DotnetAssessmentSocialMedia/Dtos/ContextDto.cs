using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Dtos
{
    public class ContextDto
    {
        public TweetDto Target { get; set; }
        public IEnumerable<TweetDto> Before { get; set; }
        public IEnumerable<TweetDto> After { get; set; }
    }
}
