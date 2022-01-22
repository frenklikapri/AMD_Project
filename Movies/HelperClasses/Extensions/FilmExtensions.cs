using Movies.Dtos;

namespace Movies.HelperClasses.Extensions
{
    public static class FilmExtensions
    {
        public static FilmDto Copy(this FilmDto film)
        {
            return new FilmDto(film.Id, film.Title, film.ReleaseDate, film.ParentId);
        }

        public static FilmDto CopyWithNewId(this FilmDto film)
        {
            return new FilmDto(Guid.NewGuid(), film.Title, film.ReleaseDate, film.ParentId);
        }
    }
}
