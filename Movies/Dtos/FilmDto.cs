using System.ComponentModel.DataAnnotations;

namespace Movies.Dtos
{
    public class FilmDto
    {
        public FilmDto()
        {

        }

        public FilmDto(Guid id, string title, DateOnly releaseDate)
        {
            Id = id;
            Title = title;
            ReleaseDate = releaseDate;
        }

        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateOnly ReleaseDate { get; set; }
    }
}
