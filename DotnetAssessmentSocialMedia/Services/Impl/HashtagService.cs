using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetAssessmentSocialMedia.Data;
using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Exception.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotnetAssessmentSocialMedia.Services.Impl
{
    public class HashtagService : IHashtagService
    {
        private readonly SocialMediaContext _context;

        private readonly ILogger _logger;

        public HashtagService(SocialMediaContext context, ILogger<HashtagService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Hashtag> GetAll()
        {
            return _context.Hashtags.ToList();
        }

        public Hashtag GetByLabel(string label)
        {
            var tag = _context.Hashtags
                .FirstOrDefault(h => h.Label == label.ToLower());
            if (tag != null)
            {
                return tag;
            }
            throw new HashtagNotFoundException();
        }

        public IEnumerable<Tweet> GetTaggedTweets(Hashtag hashtag)
        {
            return _context.Hashtags
                .Include(h => h.TweetHashtags)
                    .ThenInclude(th => th.Tweet)
                        .ThenInclude(t => t.Author)
                .First(h => h.Label == hashtag.Label)
                .TweetHashtags
                    .Where(th => !th.Tweet.Deleted)
                    .Select(th => th.Tweet)
                    .ToList();
        }

        /// <summary>
        ///     Create a new Hashtag, or update an existing Hashtag
        /// </summary>
        /// <exception cref="InvalidOperationException">Throws if a Tweet was passed that has no Id.</exception>
        public Hashtag UpsertHashtag(string label, Tweet tweet)
        {
            if (tweet.Id == 0)
            {
                throw new InvalidOperationException("An unsaved tweet can not be passed to this method.");
            }

            try
            {
                var tag = GetByLabel(label);
                tag.LastUsed = DateTime.Now;
                //tag.Tweets.Add(tweet);
                _context.Hashtags.Update(tag);
                var tweetHashtag = new TweetHashtag()
                {
                    Tweet = tweet,
                    Hashtag = tag,
                };
                _context.TweetHashtags.Add(tweetHashtag);
                _context.SaveChanges();
                return tag;
            }
            catch (HashtagNotFoundException)
            {
                var hashtag = new Hashtag(label);
                _context.Hashtags.Add(hashtag);
                var tweetHashtag = new TweetHashtag()
                {
                    Tweet = tweet,
                    Hashtag = hashtag,
                };
                _context.TweetHashtags.Add(tweetHashtag);
                _context.SaveChanges();
                return hashtag;
            }
        }
    }
}
