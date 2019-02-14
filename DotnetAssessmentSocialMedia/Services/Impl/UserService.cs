using System;
using System.Collections.Generic;
using System.Linq;
using DotnetAssessmentSocialMedia.Data;
using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Dtos;
using DotnetAssessmentSocialMedia.Exception.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;

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

        public User GetByUsername(string username)
        {
            var user = _context.Users.SingleOrDefault(u => u.Credentials.Username == username);

            // If user doesn't exists or is deleted, throw UserNotFoundException
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

        public User DeleteUser(string username, Credentials credentials)
        {
            // Get user if username matches and user is not deleted
            var user = GetByUsername(username);
            ValidateCredentialsForUser(credentials, user);

            user.Deleted = true;
            _context.SaveChanges();
            return user;
        }

        /// <summary>
        ///     Throws if credentials are not valid for given user.
        /// </summary>
        /// <exception cref="InvalidCredentialsException">InvalidCredentialsException</exception>
        public void ValidateCredentialsForUser(Credentials credentials, User user)
        {
            if (user.Credentials.Username != credentials.Username
                || user.Credentials.Password != credentials.Password)
            {
                throw new InvalidCredentialsException();
            }
        }
    }
}