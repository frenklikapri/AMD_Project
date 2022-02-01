namespace Movies.Dtos
{
    public class SimpleFilmDto
    {
        public int FilmId { get; set; }
        public string Title { get; set; }
        public string FilmPic { get; set; }
        public string Genres { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Rating { get; set; }
    }
}
