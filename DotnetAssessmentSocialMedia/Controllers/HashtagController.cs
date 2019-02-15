using AutoMapper;
using DotnetAssessmentSocialMedia.Data;
using DotnetAssessmentSocialMedia.Dtos;
using DotnetAssessmentSocialMedia.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Controllers
{
    [Route("/api/tags")]
    [ApiController]
    public class HashtagController : ControllerBase
    {
        private SocialMediaContext _context;
        private IMapper _mapper;
        private IHashtagService _hashtagService;
        private ITweetService _tweetService;

        public HashtagController(SocialMediaContext context, IMapper mapper, IHashtagService hashtagService, ITweetService tweetService)
        {
            _context = context;
            _mapper = mapper;
            _hashtagService = hashtagService;
            _tweetService = tweetService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<HashtagDto>> GetAll()
        {
            return _mapper.Map<IEnumerable<HashtagDto>>(_hashtagService.GetAll()).ToList();
        }

        [HttpGet("{label}")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<TweetDto>> GetTaggedTweets(string label)
        {
            var hashtag = _hashtagService.GetByLabel(label);
            return _mapper.Map<IEnumerable<TweetDto>>(_hashtagService.GetTaggedTweets(hashtag)).ToList();
        }
    }
}
