using Dapper;
using Movies.Dtos;
using Npgsql;

namespace Movies.Services.Users
{
    public class UsersServiceWithDapper : IUsersService
    {
        private string _connString = string.Empty;

        public UsersServiceWithDapper(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("PostgreSQLConnection");
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connString);
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            using var connection = CreateConnection();
            var result = (await connection
                .QueryAsync<UserDto>("select * from GetAllUsers()")).ToList();
            return result;
        }

        public async Task<List<FilmRatingDto>> GetFilmRatingsByUserIdAsync(int userId)
        {
            using var connection = CreateConnection();
            var result = (await connection
                .QueryAsync<FilmRatingDto>("select * from GetUserRatings(@userId)", new
                {
                    userId
                })).ToList();
            return result;
        }

        public async Task<string> RemoveRatingAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection
                    .QueryFirstAsync<string>("select RemoveRating(@id)"
                    , new
                    {
                        id
                    });
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> SaveRatingAsync(int filmRatingId, int rating, int filmId, int userId)
        {
            if(filmRatingId > 0)
            {
                try
                {
                    using var connection = CreateConnection();
                    var result = await connection
                        .QueryFirstAsync<string>("select UpdateRating(@filmRatingId, @rating)"
                        , new
                        {
                            filmRatingId,
                            rating
                        });
                    return result;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
            else
            {
                try
                {
                    using var connection = CreateConnection();
                    var result = await connection
                        .QueryFirstAsync<string>("select AddRating(@filmId, @userId, @rating)"
                        , new
                        {
                            filmId,
                            rating,
                            userId
                        });
                    return result;
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }
        }

        public async Task<List<SimpleFilmDto>> GetFilmsToRateAsync(int userId)
        {
            using var connection = CreateConnection();
            var result = (await connection
                .QueryAsync<SimpleFilmDto>("select * from GetFilmsToRate(@userId)", new
                {
                    userId
                })).ToList();
            return result;
        }
    }
}
