using DotnetAssessmentSocialMedia.Data;
using DotnetAssessmentSocialMedia.Exception.Exceptions;
using DotnetAssessmentSocialMedia.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotnetAssessmentSocialMedia.Controllers
{
    [Route("api/validate")]
    [ApiController]
    public class ValidationController : ControllerBase
    {
        private SocialMediaContext _socialMediaContext;
        private IHashtagService _hashtagService;
        private IUserService _userService;

        public ValidationController(SocialMediaContext socialMediaContext, IHashtagService hashtagService, IUserService userService)
        {
            _socialMediaContext = socialMediaContext;
            _hashtagService = hashtagService;
            _userService = userService;
        }

        // GET api/validate/tag/exists/GamersRiseUp
        [HttpGet("tag/exists/{label}")]
        public ActionResult<bool> CheckLabelExists(string label)
        {
            try
            {
                var hashtag = _hashtagService.GetByLabel(label);
                return true;
            }
            catch(HashtagNotFoundException)
            {
                return false;
            }
        }

        [HttpGet("username/exists/{username}")]
        public ActionResult<bool> CheckUsernameExists(string username)
        {
            try
            {
                var user = _userService.GetByUsername(username);
                return true;
            }
            catch(UserNotFoundException)
            {
                return false;
            }
        }

        [HttpGet("username/available/{username}")]
        public ActionResult<bool> CheckUsernameAvailable(string username)
        {
            return !CheckUsernameExists(username).Value;
        }
    }
}
