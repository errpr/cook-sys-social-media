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
        public UserResponseDto Delete(string username, [FromBody] CredentialsDto credentialsDto)
        {
            var credentials = _mapper.Map<Credentials>(credentialsDto);
            return _mapper.Map<UserResponseDto>(_userService.DeleteUser(username, credentials));
        }

        // PATCH api/users/@{username}
        [HttpPatch("@{username}")]
        public UserResponseDto Patch(string username, [FromBody] PatchUserDto patchUserDto)
        {
            var credentials = _mapper.Map<Credentials>(patchUserDto.Credentials);
            var userProfile = _mapper.Map<Data.Entities.Profile>(patchUserDto.Profile);
            var returnedUser = _userService.UpdateUserProfile(username, userProfile, credentials);
            return _mapper.Map<UserResponseDto>(returnedUser);
        }
    }
}