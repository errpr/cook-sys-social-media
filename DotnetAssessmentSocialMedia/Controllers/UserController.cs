using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Dtos;
using DotnetAssessmentSocialMedia.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotnetAssessmentSocialMedia.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        private readonly IMapper _mapper;

        private readonly ILogger _logger;
        
        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }
        
        // GET api/users
        [HttpGet]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<UserResponseDto>> Get()
        {
            var result = _userService.GetAll();
            var users = result.ToList();

            var mappedUsers = _mapper.Map<IEnumerable<User>, IEnumerable<UserResponseDto>>(users);
            return mappedUsers.ToList();
        }

        // GET api/users/@{username}
        [HttpGet("@{username}")]
        [ProducesResponseType(404)]
        public ActionResult<UserResponseDto> Get(string username)
        {
            var user = _userService.GetByUsername(username);
            return _mapper.Map<User, UserResponseDto>(user);
        }

        // POST api/users
        [HttpPost]
        [ProducesResponseType(400)]
        public ActionResult<UserResponseDto> Post([FromBody] CreateUserDto userDto)
        {
            var user = _mapper.Map<User>(userDto);
            return _mapper.Map<UserResponseDto>(_userService.CreateUser(user));
        }

        // DELETE api/users/@{username}
        [HttpDelete("@{username}")]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult<UserResponseDto> Delete(string username, [FromBody] CredentialsDto credentialsDto)
        {
            if (credentialsDto.Username != username)
            {
                return Forbid();
            }
            var credentials = _mapper.Map<Credentials>(credentialsDto);
            return _mapper.Map<UserResponseDto>(_userService.DeleteUser(credentials));
        }

        // PATCH api/users/@{username}
        [HttpPatch("@{username}")]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public UserResponseDto Patch(string username, [FromBody] PatchUserDto patchUserDto)
        {
            var credentials = _mapper.Map<Credentials>(patchUserDto.Credentials);
            var userProfile = _mapper.Map<Data.Entities.Profile>(patchUserDto.Profile);
            var returnedUser = _userService.UpdateUserProfile(userProfile, credentials);
            return _mapper.Map<UserResponseDto>(returnedUser);
        }

        // POST api/users/@{username}/follow
        [HttpPost("@{usernameToFollow}/follow")]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult FollowUser(string usernameToFollow, [FromBody] CredentialsDto credentialsDto)
        {
            var credentials = _mapper.Map<Credentials>(credentialsDto);
            var user = _userService.GetAndValidateUser(credentials);
            _userService.FollowUser(usernameToFollow, user);
            return Ok();
        }

        // POST api/users/@{username}/unfollow
        [HttpPost("@{usernameToUnfollow}/unfollow")]
        [ProducesResponseType(403)]
        [ProducesResponseType(404)]
        public ActionResult UnfollowUser(string usernameToUnfollow, [FromBody] CredentialsDto credentialsDto)
        {
            var credentials = _mapper.Map<Credentials>(credentialsDto);
            var user = _userService.GetAndValidateUser(credentials);
            _userService.UnfollowUser(usernameToUnfollow, user);
            return Ok();
        }

        // GET api/users/@{username}/feed
        [HttpGet("@{username}/feed")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<TweetDto>> GetUserFeed(string username)
        {
            return _mapper.Map<IEnumerable<TweetDto>>(_userService.GetUserFeed(username)).ToList();
        }

        // GET api/users/@{username}/tweets
        [HttpGet("@{username}/tweets")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<TweetDto>> GetUserTweets(string username)
        {
            return _mapper.Map<IEnumerable<TweetDto>>(_userService.GetUserTweets(username)).ToList();
        }

        // GET api/users/@{username}/mentions
        [HttpGet("@{username}/mentions")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<TweetDto>> GetUserMentions(string username)
        {
            return _mapper.Map<IEnumerable<TweetDto>>(_userService.GetUserMentions(username)).ToList();
        }

        // GET api/users/@{username}/followers
        [HttpGet("@{username}/followers")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<UserResponseDto>> GetFollowers(string username)
        {
            return _mapper.Map<IEnumerable<UserResponseDto>>(_userService.GetFollowers(username)).ToList();
        }

        // GET api/users/@{username}/following
        [HttpGet("@{username}/following")]
        [ProducesResponseType(404)]
        public ActionResult<IEnumerable<UserResponseDto>> GetFollowing(string username)
        {
            return _mapper.Map<IEnumerable<UserResponseDto>>(_userService.GetFollowedUsers(username)).ToList();
        }
    }
}