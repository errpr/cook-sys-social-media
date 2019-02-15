using AutoMapper;
using DotnetAssessmentSocialMedia.Data;
using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Dtos;
using DotnetAssessmentSocialMedia.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Controllers
{
    [ApiController]
    [Route("api/tweets")]
    public class TweetsController : ControllerBase
    {
        private IMapper _mapper;
        private SocialMediaContext _context;
        private ITweetService _tweetService;
        private IHashtagService _hashtagService;
        private IUserService _userService;

        public TweetsController(IMapper mapper, SocialMediaContext context, ITweetService tweetService, IHashtagService hashtagService, IUserService userService)
        {
            _mapper = mapper;
            _context = context;
            _tweetService = tweetService;
            _hashtagService = hashtagService;
            _userService = userService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TweetDto>> GetAll()
        {
            var tweets = _tweetService.GetAll();
            return _mapper.Map<IEnumerable<TweetDto>>(tweets).ToList();
        }

        [HttpPost]
        public ActionResult<TweetDto> Post([FromBody] CreateTweetDto createTweetDto)
        {
            var credentials = _mapper.Map<Credentials>(createTweetDto.Credentials);
            var user = _userService.GetAndValidateUser(credentials);
            var tweet = _tweetService.CreateSimpleTweet(
                user,
                createTweetDto.Content
            );
            return _mapper.Map<TweetDto>(tweet); 
        }

        [HttpGet("{id}")]
        [ProducesResponseType(404)]
        public ActionResult<TweetDto> GetTweet(int id)
        {
            return _mapper.Map<TweetDto>(_tweetService.GetTweet(id));
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<TweetDto> DeleteTweet(int id, [FromBody] CredentialsDto credentialsDto)
        {
            var credentials = _mapper.Map<Credentials>(credentialsDto);
            var user = _userService.GetAndValidateUser(credentials);
            var deletedTweet = _tweetService.DeleteTweet(id, user);
            return _mapper.Map<TweetDto>(deletedTweet);
        }

        [HttpPost("{id}/like")]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult LikeTweet(int id, [FromBody] CredentialsDto credentialsDto)
        {
            var credentials = _mapper.Map<Credentials>(credentialsDto);
            var user = _userService.GetAndValidateUser(credentials);
            _tweetService.LikeTweet(id, user);
            return Ok();
        }

        [HttpPost("{id}/reply")]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<TweetDto> ReplyToTweet(int id, [FromBody] CreateTweetDto createTweetDto)
        {
            var credentials = _mapper.Map<Credentials>(createTweetDto.Credentials);
            var user = _userService.GetAndValidateUser(credentials);
            var tweetBeingRepliedTo = _tweetService.GetTweet(id);
            var tweetReply = _tweetService.CreateReplyTweet(user, createTweetDto.Content, tweetBeingRepliedTo.Id);
            return _mapper.Map<TweetDto>(tweetReply);
        }

        [HttpPost("{id}/repost")]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<TweetDto> RepostTweet(int id, [FromBody] CredentialsDto credentialsDto)
        {
            var credentials = _mapper.Map<Credentials>(credentialsDto);
            var user = _userService.GetAndValidateUser(credentials);
            var tweetRepost = _tweetService.CreateRepostTweet(user, id);
            return _mapper.Map<TweetDto>(tweetRepost);
        }

        [HttpGet("{id}/tags")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<HashtagDto>> GetTags(int id)
        {
            return _mapper.Map<IEnumerable<HashtagDto>>(_tweetService.GetTags(id)).ToList();
        }

        [HttpGet("{id}/likes")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<UserResponseDto>> GetLikes(int id)
        {
            return _mapper.Map<IEnumerable<UserResponseDto>>(_tweetService.GetLikes(id)).ToList();
        }
        
        [HttpGet("{id}/context")]
        [ProducesResponseType(404)]
        public ActionResult<ContextDto> GetContext(int id)
        {
            var context = _tweetService.GetContext(id);
            return _mapper.Map<ContextDto>(context);
        }

        [HttpGet("{id}/replies")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<TweetDto>> GetReplies(int id)
        {
            return _mapper.Map<IEnumerable<TweetDto>>(_tweetService.GetReplies(id)).ToList();
        }

        [HttpGet("{id}/reposts")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<TweetDto>> GetReposts(int id)
        {
            return _mapper.Map<IEnumerable<TweetDto>>(_tweetService.GetReposts(id)).ToList();
        }

        [HttpGet("{id}/mentions")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<UserResponseDto>> GetMentions(int id)
        {
            return _mapper.Map<IEnumerable<UserResponseDto>>(_tweetService.GetMentions(id)).ToList();
        }
    }
}
