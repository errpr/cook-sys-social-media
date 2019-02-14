using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetAssessmentSocialMedia.Data;
using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Exception.Exceptions;
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

        public Hashtag CreateHashtag(Hashtag hashtag)
        {
            try
            {
                _context.Hashtags.Add(hashtag);
                _context.SaveChanges();
                return hashtag;
            }
            catch (Npgsql.PostgresException)
            {
                _logger.LogError("Can this ever happen?");
                throw new UniqueConstraintViolationException(
                    "Hashtag already exists", 
                    "Could not create new Hashtag, a Hashtag with that label already exists.");
            }
        }

        public IEnumerable<Hashtag> GetAll()
        {
            return _context.Hashtags.ToList();
        }

        public Hashtag GetByLabel(string label)
        {
            var tag = _context.Hashtags.FirstOrDefault(h => h.Label == label.ToLower());
            if (tag != null)
            {
                return tag;
            }
            throw new HashtagNotFoundException();
        }
    }
}
