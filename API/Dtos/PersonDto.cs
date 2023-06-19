namespace API.Dtos
{
    public class PersonDto
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public int? PictureId { get; set; }
        public PictureDto Picture { get; set; }
        public int Karma { get; set; }
        public int Answers { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime Created { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }
}