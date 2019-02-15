
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAssessmentSocialMedia.Data.Entities
{
    public class Mention
    {
        [ForeignKey("Tweet")]
        public int TweetId { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
    }
}
