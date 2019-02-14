using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAssessmentSocialMedia.Data.Entities
{
    [Table("user")]
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        
        public Profile Profile { get; set; }
        
        public Credentials Credentials { get; set; }
        
        [Column("joined")]
        public DateTime Joined { get; set; }

        [Column("deleted")]
        public Boolean Deleted { get; set; }
    }
}