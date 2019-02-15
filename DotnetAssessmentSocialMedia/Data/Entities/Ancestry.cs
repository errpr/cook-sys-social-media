using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAssessmentSocialMedia.Data.Entities
{
    public class Ancestry
    {
        [ForeignKey("Tweet")]
        public int ChildId { get; set; }

        [ForeignKey("Tweet")]
        public int AncestorId { get; set; }
    }
}
