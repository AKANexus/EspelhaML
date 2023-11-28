namespace MlSuite.Api.DTOs
{
    public class RegisterAgentDto
    {
        public string? Password { get; set; }
        public string? DisplayName { get; set; }
        public Guid? Uuid { get; set; }
    }
}
