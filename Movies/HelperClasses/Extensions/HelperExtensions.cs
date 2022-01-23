using Movies.Dtos;

namespace Movies.HelperClasses.Extensions
{
    public static class HelperExtensions
    {
        public static FilmDto Copy(this FilmDto film)
        {
            return new FilmDto(film.Id, film.Title, film.ReleaseDate, film.ParentId);
        }

        public static FilmDto CopyWithNewId(this FilmDto film)
        {
            return new FilmDto(Guid.NewGuid(), film.Title, film.ReleaseDate, film.ParentId);
        }

        public static PersonDto Copy(this PersonDto person)
        {
            return new PersonDto(person.Id, person.Name, person.Sex, person.Birthday);
        }

        public static PersonDto CopyWithNewId(this PersonDto person)
        {
            return new PersonDto(Guid.NewGuid(), person.Name, person.Sex, person.Birthday);
        }
    }
}
