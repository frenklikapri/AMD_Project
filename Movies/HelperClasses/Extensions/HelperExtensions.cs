using Movies.Dtos;

namespace Movies.HelperClasses.Extensions
{
    public static class HelperExtensions
    {
        public static FilmDto Copy(this FilmDto film)
        {
            var f = new FilmDto(film.Id, film.Title, film.ReleaseDate, film.FilmPic, film.ParentId);
            f.Genres = film.Genres;
            return f;
        }

        public static PersonDto Copy(this PersonDto person)
        {
            return new PersonDto(person.Id, person.Name, person.Sex, person.Birthday, person.ProfilePic);
        }
    }
}
