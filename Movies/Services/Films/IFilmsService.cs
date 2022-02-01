using Movies.Dtos;
using Movies.HelperClasses;

namespace Movies.Services.Films
{
    public interface IFilmsService
    {
        Task<string> SaveFilmAsync(FilmDto filmDto);
        Task<string> DeleteFilmAsync(int id);
        Task<List<FilmDto>> GetAllFilmsAsync();
        Task<List<FilmDto>> GetWatchSuggestionsAsync(int userId);
        List<FilmDto> GetChildrenFilms(int id);
        Task<List<GenreDto>> GetAllGenresAsync();
        Task<List<int>> GetFilmGenresAsync(int filmId);
        Task<List<FilmRelatedPersonDto>> GetFilmRelatedPersonsAsync(int filmId);
        Task<string> RemoveFilmRelatedPersonAsync(int filmRelatedPersonId);
        Task<string> AddFilmRelatedPersonAsync(int filmPersonId, int filmRoleId, int filmId);
        Task<List<FilmRoleDto>> GetAllFilmRolesAsync();
    }
}
