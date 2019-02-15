using System;
using System.Collections.Generic;
using System.Linq;
using DotnetAssessmentSocialMedia.Data;
using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Exception.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotnetAssessmentSocialMedia.Services.Impl
{
    public class UserService : IUserService
    {
        private readonly SocialMediaContext _context;

        private readonly ILogger _logger;

        public UserService(SocialMediaContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        ///     Throws if credentials are not valid for given user.
        /// </summary>
        /// <exception cref="InvalidCredentialsException">InvalidCredentialsException</exception>
        public User GetAndValidateUser(Credentials credentials)
        {
            var user = GetByUsername(credentials.Username);
            if (user.Credentials.Password != credentials.Password)
            {
                throw new InvalidCredentialsException();
            }
            return user;
        }
        
        /// <exception cref="UserNotFoundException"></exception>
        public User GetByUsername(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.Credentials.Username == username);

            // If user doesn't exist or is deleted, throw UserNotFoundException
            if (user == null || user.Deleted)
            {
                throw new UserNotFoundException();
            }

            return user;
        }

        public IEnumerable<User> GetAll()
        {
            // Find all non-delete users
            var users = _context.Users.Where(u => !u.Deleted).ToList();
            if (users.Count <= 0)
            {
                throw new NotFoundCustomException("No users found", "No users found");
            }

            return users;
        }

        public User CreateUser(User user)
        {
            user.Joined = DateTime.Now;
            try
            {
                _context.Add(user);
                _context.SaveChanges();
            }
            catch (DbUpdateException e)
            {
                if (e.InnerException.Message.Contains("unique constraint")) // hmm
                {
                    throw new UsernameTakenException();
                }
            }

            return user;
        }

        public User DeleteUser(Credentials credentials)
        {
            // Get user if username matches and user is not deleted
            var user = GetAndValidateUser(credentials);

            user.Deleted = true;
            _context.Update(user);
            _context.SaveChanges();
            return user;
        }

        public User UpdateUserProfile(Profile userProfile, Credentials credentials)
        {
            var user = GetAndValidateUser(credentials);

            user.Profile = userProfile;
            _context.Update(user);
            _context.SaveChanges();
            return user;
        }

        public void FollowUser(string usernameToFollow, User follower)
        {
            var followedUser = GetByUsername(usernameToFollow);
            if (_context.Follows
                .Where(f => f.FollowedUserId == followedUser.Id && f.FollowerUserId == follower.Id)
                .Any())
            {
                throw new UniqueConstraintViolationException(
                    "Already exists.",
                    "User is already following the user with the given username.");
            }
            var follow = new Follow()
            {
                FollowerUserId = follower.Id,
                FollowedUserId = followedUser.Id,
            };
            _context.Add(follow);
            _context.SaveChanges();
        }

        public void UnfollowUser(string usernameToUnfollow, User follower)
        {
            var followedUser = GetByUsername(usernameToUnfollow);
            var follow = _context.Follows
                .Where(f => f.FollowedUserId == followedUser.Id && f.FollowerUserId == follower.Id)
                .FirstOrDefault();
            if (follow == null)
            {
                throw new NotFoundCustomException(
                    "Relationship does not exist", 
                    "User does not follow the user with given username");
            }
            _context.Remove(follow);
            _context.SaveChanges();
        }

        public IEnumerable<Tweet> GetUserTweets(string username)
        {
            var user = GetByUsername(username);
            return GetUserTweets(user);
        }

        private IEnumerable<Tweet> GetUserTweets(User user)
        {
            var result = _context.Tweets
                .Include(t => t.Author)
                .Where(t => t.Author.Id == user.Id && !t.Deleted)
                .ToList();

            result.Sort((a, b) => DateTime.Compare(b.Posted, a.Posted));
            return result;
        }

        public IEnumerable<User> GetFollowedUsers(string username)
        {
            var user = GetByUsername(username);
            return GetFollowedUsers(user);
        }

        private IEnumerable<User> GetFollowedUsers(User user)
        {
            var followedUserIds = _context.Follows
                .Where(f => f.FollowerUserId == user.Id)
                .Select(f => f.FollowedUserId)
                .ToList();

            return _context.Users
                .Where(u => followedUserIds.Contains(u.Id) && !u.Deleted)
                .ToList();
        }

        public IEnumerable<Tweet> GetUserFeed(string username)
        {
            var user = GetByUsername(username);
            var result = new List<Tweet>();
            var followedUsers = GetFollowedUsers(user);
            foreach (var followedUser in followedUsers)
            {
                result.AddRange(GetUserTweets(followedUser));
            }
            result.AddRange(GetUserTweets(user));
            result.Sort((a, b) => DateTime.Compare(b.Posted, a.Posted));
            return result;
        }

        public IEnumerable<User> GetFollowers(string username)
        {
            var user = GetByUsername(username);

            var followerUserIds = _context.Follows
                .Where(f => f.FollowedUserId == user.Id)
                .Select(f => f.FollowerUserId)
                .ToList();

            return _context.Users
                .Where(u => followerUserIds.Contains(u.Id) && !u.Deleted)
                .ToList();
        }

        public IEnumerable<Tweet> GetUserMentions(string username)
        {
            var user = GetByUsername(username);
            var tweetIds = _context.Mentions
                .Where(m => m.UserId == user.Id)
                .Select(m => m.TweetId)
                .ToList();
            var result = _context.Tweets
                .Where(t => tweetIds.Contains(t.Id) && !t.Deleted)
                .ToList();
            result.Sort((a, b) => DateTime.Compare(b.Posted, a.Posted));
            return result;
        }

        public bool CheckUsernameAvailable(string username)
        {
            return _context.Users.Any(u => u.Credentials.Username == username);
        }
    }
}