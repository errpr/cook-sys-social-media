using DotnetAssessmentSocialMedia.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Dtos
{
    public class TweetDto
    {
        public int Id { get; set; }
        public UserResponseDto Author { get; set; }
        public DateTime Posted { get; set; }
        public string Content { get; set; }
        public int? InReplyTo { get; set; }
        public int? RepostOf { get; set; }
    }
}
