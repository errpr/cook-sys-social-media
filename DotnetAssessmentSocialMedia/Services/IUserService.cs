using System.Collections.Generic;
using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Dtos;

namespace DotnetAssessmentSocialMedia.Services
{
    public interface IUserService
    {
        User GetByUsername(string username);
        IEnumerable<User> GetAll();
        User CreateUser(User user);
        User DeleteUser(string username, Credentials credentials);
        void ValidateCredentialsForUser(Credentials credentials, User user);
    }
}