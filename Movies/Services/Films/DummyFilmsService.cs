using Movies.Dtos;
using Movies.HelperClasses;

namespace Movies.Services.Films
{
    public class DummyFilmsService : IFilmsService
    {
        private List<FilmDto> _films = new()
        {
            new FilmDto(Guid.NewGuid(), "The Battle at Lake Changjin", new DateOnly(2021, 9, 21)),
            new FilmDto(Guid.NewGuid(), "No Time to Die", new DateOnly(2021, 9, 28))
        };

        public async Task<bool> DeleteFilmAsync(Guid id)
        {
            var film = _films.First(f => f.Id == id);
            _films.Remove(film);
            return true;
        }

        public async Task<PaginatedListResult<FilmDto>> GetFilmsAsync(PaginationParameters paginationParameters)
        {
            if (string.IsNullOrEmpty(paginationParameters.Search))
                paginationParameters.Search = string.Empty;

            var totalFilms = _films
                .Where(f => f.Title.ToLower().Contains(paginationParameters.Search.ToLower()))
                .Count();

            var list = _films
                .Where(f => f.Title.ToLower().Contains(paginationParameters.Search.ToLower()))
                .OrderByDescending(f => f.ReleaseDate)
                .Skip((paginationParameters.Page - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize)
                .Select(f => new FilmDto(f.Id, f.Title, f.ReleaseDate))
                .ToList();

            return new PaginatedListResult<FilmDto>
            {
                CountAll = totalFilms,
                Items = list
            };
        }

        public async Task<FilmDto> SaveFilmAsync(FilmDto filmDto)
        {
            var film = _films.FirstOrDefault(f => f.Id == filmDto.Id);

            if(film is null)
            {
                film = new FilmDto(Guid.NewGuid(), filmDto.Title, filmDto.ReleaseDate);
                _films.Add(film);
            }
            else
            {
                film.Title = filmDto.Title;
                film.ReleaseDate = filmDto.ReleaseDate;
            }

            return film;
        }
    }
}
