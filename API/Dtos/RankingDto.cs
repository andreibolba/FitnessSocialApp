namespace API.Dtos
{
    public class RankingDto
    {
        public int Position { get; set; }
        public int PersonId { get; set; }
        public PersonDto Person { get; set; }
        public int Points { get; set; }

    }
}
