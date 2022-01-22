using Movies.Dtos;
using Movies.HelperClasses;
using Movies.HelperClasses.Extensions;

namespace Movies.Services.Films
{
    public class DummyFilmsService : IFilmsService
    {
        private List<FilmDto> _films = new()
        {
            new FilmDto(Guid.NewGuid(), "The Battle at Lake Changjin", new DateOnly(2021, 9, 21)),
            new FilmDto(Guid.NewGuid(), "No Time to Die", new DateOnly(2021, 9, 28)),
            new FilmDto(Guid.Parse("0a9fc60e-2776-47a2-b970-141f10c16c23"), "Spider Man", new DateOnly(2021, 1, 1)),
            new FilmDto(Guid.NewGuid(), "Spider Man 2", new DateOnly(2021, 1, 1), Guid.Parse("0a9fc60e-2776-47a2-b970-141f10c16c23")),
            new FilmDto(Guid.NewGuid(), "Spider Man 3", new DateOnly(2021, 1, 1), Guid.Parse("0a9fc60e-2776-47a2-b970-141f10c16c23")),
            new FilmDto(Guid.NewGuid(), "Spider Man 4", new DateOnly(2021, 1, 1), Guid.Parse("0a9fc60e-2776-47a2-b970-141f10c16c23")),
            new FilmDto(Guid.Parse("0b9fc60e-2776-47a2-b970-141f10c16c23"), "LOTR", new DateOnly(2021, 1, 1)),
            new FilmDto(Guid.NewGuid(), "LOTR 1", new DateOnly(2021, 1, 1), Guid.Parse("0b9fc60e-2776-47a2-b970-141f10c16c23")),
            new FilmDto(Guid.NewGuid(), "LOTR 2", new DateOnly(2021, 1, 1), Guid.Parse("0b9fc60e-2776-47a2-b970-141f10c16c23")),
        };

        public async Task<bool> DeleteFilmAsync(Guid id)
        {
            var film = _films.First(f => f.Id == id);
            _films.Remove(film);
            return true;
        }

        public async Task<List<FilmDto>> GetAllFilmsAsync()
        {
            return _films;
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
                film = filmDto.CopyWithNewId();
                _films.Add(film);
            }
            else
            {
                film.Title = filmDto.Title;
                film.ReleaseDate = filmDto.ReleaseDate;
                film.ParentId = filmDto.ParentId;
            }

            return film;
        }
    }
}
