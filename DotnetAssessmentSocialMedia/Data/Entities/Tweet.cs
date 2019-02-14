using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Data.Entities
{
    [Table("tweet")]
    public class Tweet
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("author")]
        public User Author { get; set; }

        [Required]
        [Column("posted")]
        public DateTime Posted { get; set; }
        
        [Column("content")]
        public string Content { get; set; }

        [ForeignKey("tweet")]
        [Column("replying_to_id")]
        public int? InReplyTo { get; set; }

        [ForeignKey("tweet")]
        [Column("repost_of_id")]
        public int? RepostOf { get; set; }
    }
}
