using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetAssessmentSocialMedia.Data.Entities
{
    public class User
    {
        public int Id { get; set; }
        
        public Profile Profile { get; set; }
        
        public Credentials Credentials { get; set; }
        
        public DateTime Joined { get; set; }
        
        public bool Deleted { get; set; }
    }
}