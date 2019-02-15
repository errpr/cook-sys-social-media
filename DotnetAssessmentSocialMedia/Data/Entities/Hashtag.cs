using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DotnetAssessmentSocialMedia.Data.Entities
{
    public class Hashtag
    {
        public Hashtag() { }

        public Hashtag(string label)
        {
            Label = label.ToLower();
            FirstUsed = DateTime.Now;
            LastUsed = DateTime.Now;
        }

        [Key]
        public string Label { get; set; }

        [Required]
        public DateTime FirstUsed { get; set; }

        [Required]
        public DateTime LastUsed { get; set; }

        public List<TweetHashtag> TweetHashtags { get; set; }
    }
}
