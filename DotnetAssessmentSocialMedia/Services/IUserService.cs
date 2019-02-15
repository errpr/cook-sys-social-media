using System.Collections.Generic;
using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAssessmentSocialMedia.Services
{
    public interface IUserService
    {
        User GetByUsername(string username);
        User GetAndValidateUser(Credentials credentials);
        IEnumerable<User> GetAll();
        User CreateUser(User user);
        User DeleteUser(Credentials credentials);
        User UpdateUserProfile(Profile userProfile, Credentials credentials);
        void FollowUser(string usernameToFollow, User follower);
        void UnfollowUser(string usernameToUnfollow, User follower);
        IEnumerable<Tweet> GetUserTweets(string username);
        IEnumerable<Tweet> GetUserFeed(string username);
        IEnumerable<User> GetFollowedUsers(string username);
        IEnumerable<User> GetFollowers(string username);
        IEnumerable<Tweet> GetUserMentions(string username);
        bool CheckUsernameAvailable(string username);
    }
}