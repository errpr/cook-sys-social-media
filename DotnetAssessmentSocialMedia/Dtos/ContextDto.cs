using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Dtos
{
    public class ContextDto
    {
        TweetDto Target { get; set; }
        List<TweetDto> Before { get; set; }
        List<TweetDto> After { get; set; }
    }
}
