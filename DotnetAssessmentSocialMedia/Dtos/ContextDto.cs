using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Dtos
{
    public class ContextDto
    {
        TweetDto Target { get; set; }
        IEnumerable<TweetDto> Before { get; set; }
        IEnumerable<TweetDto> After { get; set; }
    }
}
