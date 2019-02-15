using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Data.Entities
{
    public class TweetHashtag
    {
        [ForeignKey("Tweet")]
        public int TweetId { get; set; }

        [ForeignKey("Hashtag")]
        public string HashtagLabel { get; set; }

        public Tweet Tweet { get; set; }
        public Hashtag Hashtag { get; set; }
    }
}
