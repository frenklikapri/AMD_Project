using System.ComponentModel.DataAnnotations;

namespace Movies.Dtos
{
    public class FilmDto
    {
        public FilmDto()
        {

        }

        public FilmDto(Guid id, string title, DateOnly releaseDate, Guid? parentId = null)
        {
            Id = id;
            Title = title;
            ReleaseDate = releaseDate;
            ParentId = parentId;
        }

        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateOnly ReleaseDate { get; set; }

        public Guid? ParentId { get; set; }

        /// <summary>
        /// This is used in html table to show children
        /// </summary>
        public bool ShowChildren { get; set; }
    }
}
