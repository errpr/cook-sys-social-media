using DotnetAssessmentSocialMedia.Data.Entities;
using DotnetAssessmentSocialMedia.Dtos;
using Profile = AutoMapper.Profile;

namespace DotnetAssessmentSocialMedia
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.Joined, opts => opts.Ignore())
                .ForMember(dest => dest.Deleted, opts => opts.Ignore());

            CreateMap<CreateTweetDto, TweetDto>()
                .ForAllOtherMembers(opts => opts.Ignore());

            // TODO possibly should map from the TweetHashtags list instead of parsing each time
            CreateMap<Tweet, TweetDto>()
                .ForMember(
                    dest => dest.Hashtags,
                    opt => opt.MapFrom(src => src.ParseTags())
                );

            CreateMap<Hashtag, HashtagDto>();

            CreateMap<Credentials, CredentialsDto>();
            
            CreateMap<Profile, ProfileDto>();
            
            CreateMap<User, UserResponseDto>()
                .ForMember(
                    dest => dest.Username,
                    opt => opt.MapFrom(src => src.Credentials.Username)
                );
        }
    }
}