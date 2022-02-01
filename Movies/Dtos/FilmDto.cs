using System.ComponentModel.DataAnnotations;

namespace Movies.Dtos
{
    public class FilmDto
    {
        public FilmDto()
        {

        }

        public FilmDto(int id, string title, DateTime releaseDate, string filmPic, int? parentId = null)
        {
            Id = id;
            Title = title;
            ReleaseDate = releaseDate;
            ParentId = parentId;
            FilmPic = filmPic;
        }

        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        public int? ParentId { get; set; }

        public bool HasChildren { get; set; }

        /// <summary>
        /// Used to show the subordinated films
        /// </summary>
        public bool ShowChildren { get; set; }

        public string FilmPic { get; set; }

        /// <summary>
        /// This is used only to show
        /// </summary>
        public string Genres { get; set; }

        public int[] GenresToSave { get; set; }
    }
}
