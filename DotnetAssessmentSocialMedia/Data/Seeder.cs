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

            var user1 = new User
            {
                Credentials = new Credentials()
                {
                    Username = "string",
                    Password = "string",
                },
                Profile = new Profile()
                {
                    FirstName = "string",
                    LastName = "string",
                    Email = "string",
                    Phone = "string",
                },
            };

            var user2 = new User
            {
                Credentials = new Credentials()
                {
                    Username = "string2",
                    Password = "string2",
                },
                Profile = new Profile()
                {
                    FirstName = "string2",
                    LastName = "string2",
                    Email = "string2",
                    Phone = "string2",
                },
            };

            var user3 = new User
            {
                Credentials = new Credentials()
                {
                    Username = "string3",
                    Password = "string3",
                },
                Profile = new Profile()
                {
                    FirstName = "string3",
                    LastName = "string3",
                    Email = "string3",
                    Phone = "string3",
                },
            };

            user1 = _userService.CreateUser(user1);
            user2 = _userService.CreateUser(user2);
            user3 = _userService.CreateUser(user3);

            _userService.FollowUser(user1.Credentials.Username, user2);
            _userService.FollowUser(user2.Credentials.Username, user3);

            var t1 = _tweetService.CreateSimpleTweet(user1, "I am cool #nofilter");
            _tweetService.CreateSimpleTweet(user3, "rise up lol #justGamerThings");
            var t2 = _tweetService.CreateReplyTweet(user2, "omg @string u r so cool #stan", t1.Id);
            var t3 = _tweetService.CreateReplyTweet(user1, "chill baby I know it", t2.Id);
            var t4 = _tweetService.CreateReplyTweet(user2, "omg u replied", t3.Id);
            var t5 = _tweetService.CreateReplyTweet(user3, "@string your a idiot, @string2 I would never treat you like that mlady", t3.Id);
            var t6 = _tweetService.CreateReplyTweet(user2, "uh who tf r u? dont @ me again", t5.Id);
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