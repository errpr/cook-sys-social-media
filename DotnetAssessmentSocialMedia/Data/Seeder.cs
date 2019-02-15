using System;
using System.Linq;
using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Services;

namespace DotnetAssessmentSocialMedia.Data
{
    public class Seeder
    {
        private IUserService _userService;
        private ITweetService _tweetService;
        public Seeder(IUserService userService, ITweetService tweetService)
        {
            _userService = userService;
            _tweetService = tweetService;
        }

        public void Seed()
        {
            GenerateUsers(20);
            GenerateTweets(20);
        }

        private void GenerateTweets(int count)
        {
            var users = _userService.GetAll().ToList();
            for (var i = 0; i < count; i++)
            {
                var fakeTweetContent = $"{GenerateName(GenerateInt(3, 9))} {GenerateName(GenerateInt(3, 9))} #{GenerateName(GenerateInt(5, 12))}";
                _tweetService.CreateSimpleTweet(users[i], fakeTweetContent);
            }
        }

        private void GenerateUsers(int count)
        {
            for (var i = 0; i < count; ++i)
            {
                var firstName = GenerateName(GenerateInt(3, 9));
                var lastName = GenerateName(GenerateInt(4, 7));
                var email = $"{firstName.ToLower()[0]}.{lastName.ToLower()}@gmail.com";
                var password = $"password{GenerateInt(1000, 9999)}";
                var username = $"{GenerateName(GenerateInt(5, 9))}{GenerateInt(10, 99)}";

                var addPhoneNumber = GenerateInt(0, 10) % 2 == 0;          

                var credentials = new Credentials
                {
                    Username = username,
                    Password = password
                };

                var profile = new Profile
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    Phone = addPhoneNumber ? "555-555-1234" : null
                };


                var user = new User
                {
                    Credentials = credentials,
                    Profile = profile
                };

                _userService.CreateUser(user);
            }
        } 
        
        private static string GenerateName(int len)
        { 
            Random r = new Random();
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string name = "";
            name += consonants[r.Next(consonants.Length)].ToUpper();
            name += vowels[r.Next(vowels.Length)];
            int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
            while (b < len)
            {
                name += consonants[r.Next(consonants.Length)];
                b++;
                name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return name;
        }
        
        private static int GenerateInt(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }
}