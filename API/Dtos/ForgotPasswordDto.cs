namespace API.Dtos
{
    public class ForgotPasswordDto
    {
        public string Email { get; set; }
        public int LinkId { get; set; }
        public DateTime Time { get; set; }
        public string Password { get; set; }
    }
}