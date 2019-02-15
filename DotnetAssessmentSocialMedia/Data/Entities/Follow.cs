using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAssessmentSocialMedia.Data.Entities
{
    public class Follow
    {
        [ForeignKey("User")]
        public int FollowerUserId { get; set; }

        [ForeignKey("User")]
        public int FollowedUserId { get; set; }
    }
}
