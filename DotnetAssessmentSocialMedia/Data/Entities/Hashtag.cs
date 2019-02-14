using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Data.Entities
{
    [Table("hashtag")]
    public class Hashtag
    {
        [Required]
        [Key]
        [Column("label")]
        public string Label {
            get {
                return Label;
            }

            set {
                value.ToLower();
            }
        }

        [Required]
        [Column("first_used")]
        public DateTime FirstUsed { get; set; }

        [Required]
        [Column("last_used")]
        public DateTime LastUsed { get; set; }
    }
}
