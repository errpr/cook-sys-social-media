using DotnetAssessmentSocialMedia.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Services
{
    public interface ITweetService
    {
        Tweet GetTweet(int id);
        IEnumerable<Tweet> GetAll();
        Tweet CreateSimpleTweet(User author, string content);
        Tweet CreateReplyTweet(User author, string content, int inReplyTo);
        Tweet CreateRepostTweet(User author, int repostOf);
        Tweet DeleteTweet(int id, User author);
        void LikeTweet(int id, User liker);
        IEnumerable<Hashtag> GetTags(int id);
        IEnumerable<User> GetLikes(int id);
        IEnumerable<Tweet> GetReplies(int id);
        IEnumerable<Tweet> GetReposts(int id);
        IEnumerable<User> GetMentions(int id);
    }
}
