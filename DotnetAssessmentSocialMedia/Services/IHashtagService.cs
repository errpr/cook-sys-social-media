using DotnetAssessmentSocialMedia.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Services
{
    public interface IHashtagService
    {
        Hashtag GetByLabel(string label);
        IEnumerable<Hashtag> GetAll();
        Hashtag UpsertHashtag(string label, Tweet tweet);
        IEnumerable<Tweet> GetTaggedTweets(Hashtag hashtag);
    }
}
