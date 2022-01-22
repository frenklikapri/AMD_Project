using Movies.Dtos;
using Movies.HelperClasses;

namespace Movies.Services.Films
{
    public interface IFilmsService
    {
        Task<PaginatedListResult<FilmDto>> GetFilmsAsync(PaginationParameters paginationParameters);
        Task<FilmDto> SaveFilmAsync(FilmDto filmDto);
        Task<bool> DeleteFilmAsync(Guid id);
        Task<List<FilmDto>> GetAllFilmsAsync();
    }
}
