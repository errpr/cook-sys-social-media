using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetAssessmentSocialMedia.Data;
using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Exception.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace DotnetAssessmentSocialMedia.Services.Impl
{
    public class TweetService : ITweetService
    {
        private SocialMediaContext _context;
        private IUserService _userService;
        private IHashtagService _hashtagService;

        public TweetService(SocialMediaContext context, IUserService userService, IHashtagService hashtagService)
        {
            _context = context;
            _userService = userService;
            _hashtagService = hashtagService;
        }

        private void EnsureTweetExists(int id)
        {
            if (!_context.Tweets.Where(t => t.Id == id && !t.Deleted).Any())
            {
                throw new TweetNotFoundException();
            }
        }

        public Tweet CreateSimpleTweet(User author, string content)
        {
            if (content == null || content == string.Empty)
            {
                throw new RequiredConstraintViolationException("content");
            }

            return CreateTweet(author, content, null, null);
        }

        public Tweet CreateReplyTweet(User author, string content, int inReplyTo)
        {
            EnsureTweetExists(inReplyTo);

            if (content == null || content == string.Empty)
            {
                throw new RequiredConstraintViolationException("content");
            }

            return CreateTweet(author, content, inReplyTo, null);
        }

        public Tweet CreateRepostTweet(User author, int repostOf)
        {
            EnsureTweetExists(repostOf);

            return CreateTweet(author, null, null, repostOf);
        }
        
        private Tweet CreateTweet(User author, string content, int? inReplyTo, int? repostOf)
        {
            var tweet = new Tweet(author, content, inReplyTo, repostOf);

            _context.Tweets.Add(tweet);
            _context.SaveChanges();
            foreach(var label in tweet.ParseTags())
            {
                _hashtagService.UpsertHashtag(label, tweet);
            }

            foreach (var username in tweet.ParseMentions())
            {
                try
                {
                    var user = _userService.GetByUsername(username);
                    var mention = new Mention()
                    {
                        TweetId = tweet.Id,
                        UserId = user.Id,
                    };
                    _context.Add(mention);
                }
                catch (UserNotFoundException)
                {
                    continue;
                }
            }
            _context.SaveChanges();
            return tweet;
        }

        public Tweet DeleteTweet(int id, User author)
        {
            var tweet = GetTweet(id);
            if (tweet.Author.Id != author.Id)
            {
                throw new InvalidCredentialsException();
            }

            tweet.Deleted = true;
            _context.Tweets.Update(tweet);
            _context.SaveChanges();
            return tweet;
        }

        public IEnumerable<Tweet> GetAll()
        {
            return _context.Tweets.Include(t => t.Author).Where(t => !t.Deleted).ToList();
        }

        public Tweet GetTweet(int id)
        {
            var tweet = _context.Tweets
                .Include(t => t.Author)
                .FirstOrDefault(t => t.Id == id && !t.Deleted);
            if (tweet != null)
            {
                return tweet;
            }
            throw new TweetNotFoundException();
        }

        public void LikeTweet(int tweetId, User liker)
        {
            EnsureTweetExists(tweetId);

            var like = new Like()
            {
                TweetId = tweetId,
                UserId = liker.Id,
            };
            _context.Add(like);
            _context.SaveChanges();
        }

        public IEnumerable<Hashtag> GetTags(int id)
        {
            EnsureTweetExists(id);

            return _context.Hashtags
                .Include(h => h.TweetHashtags)
                .Where(h => h.TweetHashtags.Any(th => th.TweetId == id))
                .ToList();
        }

        public IEnumerable<User> GetLikes(int id)
        {
            EnsureTweetExists(id);

            var userIds = _context.Likes.Where(l => l.TweetId == id).Select(l => l.UserId).ToList();
            return _context.Users.Where(u => userIds.Contains(u.Id) && !u.Deleted).ToList();
        }

        public IEnumerable<Tweet> GetReplies(int id)
        {
            EnsureTweetExists(id);

            return _context.Tweets
                .Include(t => t.Author)
                .Where(t => t.InReplyTo == id)
                .ToList();
        }

        public IEnumerable<Tweet> GetReposts(int id)
        {
            EnsureTweetExists(id);

            return _context.Tweets
                .Include(t => t.Author)
                .Where(t => t.RepostOf == id)
                .ToList();
        }

        public IEnumerable<User> GetMentions(int id)
        {
            EnsureTweetExists(id);

            var userIds = _context.Mentions.Where(m => m.TweetId == id).Select(m => m.UserId).ToList();
            return _context.Users.Where(u => userIds.Contains(u.Id)).ToList();
        }
    }
}
