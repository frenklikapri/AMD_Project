namespace Movies.Dtos
{
    public class FilmRelatedPersonDto
    {
        public int FilmRelatedPersonId { get; set; }
        public int FilmPersonId { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public DateTime Birthday { get; set; }
        public string ProfilePic { get; set; }
        public string Role { get; set; }
    }
}
