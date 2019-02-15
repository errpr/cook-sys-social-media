namespace DotnetAssessmentSocialMedia.Dtos
{
    public class PatchUserDto
    {
        public CredentialsDto Credentials { get; set; }
        public ProfileDto Profile { get; set; }
    }
}