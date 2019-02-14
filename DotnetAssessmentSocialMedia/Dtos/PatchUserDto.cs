namespace DotnetAssessmentSocialMedia.Dtos
{
    public class PatchUserDto
    {
        public ProfileDto Profile { get; set; }
        
        public CredentialsDto Credentials { get; set; }
    }
}