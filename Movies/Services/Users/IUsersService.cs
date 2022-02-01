using Movies.Dtos;

namespace Movies.Services.Users
{
    public interface IUsersService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<List<FilmRatingDto>> GetFilmRatingsByUserIdAsync(int userId);
        Task<string> RemoveRatingAsync(int id);
        Task<string> SaveRatingAsync(int filmRatingId, int rating, int filmId, int userId);
        Task<List<SimpleFilmDto>> GetFilmsToRateAsync(int userId);
    }
}
