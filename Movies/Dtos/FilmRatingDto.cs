namespace Movies.Dtos
{
    public class FilmRatingDto
    {
        public int FilmUserRatingId { get; set; }
        public string Title { get; set; }
        public string Genres { get; set; }
        public string FilmPic { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int Rating { get; set; }

        /// <summary>
        /// Used for mouse hovering
        /// </summary>
        public int HoverRating { get; set; }
    }
}
