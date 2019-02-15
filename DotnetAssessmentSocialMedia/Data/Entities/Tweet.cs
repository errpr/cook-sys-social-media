using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Data.Entities
{
    public class Tweet
    {
        public Tweet()
        {
        }

        public Tweet(User author, string content, int? inReplyTo, int? repostOf)
        {
            Author = author;
            Content = content;
            InReplyTo = inReplyTo;
            RepostOf = repostOf;
            Posted = DateTime.Now;
        }

        public List<string> ParseMentions()
        {
            var words = Content.Split(' ');
            var result = new List<string>();
            foreach (var word in words)
            {
                if (word.StartsWith('@'))
                {
                    result.Add(word.Substring(1));
                }
            }
            return result;
        }

        public List<string> ParseTags()
        {
            var words = Content.Split(' ');
            var result = new List<string>();
            foreach (var word in words)
            {
                if (word.StartsWith('#'))
                {
                    result.Add(word.Substring(1));
                }
            }
            return result;
        }

        public int Id { get; set; }

        [Required]
        public User Author { get; set; }

        [Required]
        public DateTime Posted { get; set; }
        
        public string Content { get; set; }

        [ForeignKey("Tweet")]
        public int? InReplyTo { get; set; }

        [ForeignKey("Tweet")]
        public int? RepostOf { get; set; }
       
        public bool Deleted { get; set; }

        public ICollection<TweetHashtag> TweetHashtags { get; set; }
    }
}
