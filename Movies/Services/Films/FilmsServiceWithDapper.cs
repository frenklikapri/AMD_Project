using Dapper;
using Movies.Dtos;
using Movies.HelperClasses;
using Npgsql;

namespace Movies.Services.Films
{
    public class FilmsServiceWithDapper : IFilmsService
    {
        private string _connString = string.Empty;

        public FilmsServiceWithDapper(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("PostgreSQLConnection");
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connString);
        }

        public async Task<string> DeleteFilmAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection
                    .QueryFirstAsync<string>("select DeleteFilm(@id)", new
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

        public async Task<List<FilmDto>> GetAllFilmsAsync()
        {
            using var connection = CreateConnection();
            var result = (await connection
                .QueryAsync<FilmDto>("select * from GetAllFilms()")).ToList();
            return result;
        }

        public async Task<string> SaveFilmAsync(FilmDto filmDto)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection
                    .QueryFirstAsync<string>("select SaveFilm(@Id, @Title, @ReleaseDate::date, @FilmPic, @ParentId, @GenresToSave)"
                    , filmDto);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public List<FilmDto> GetChildrenFilms(int id)
        {
            using var connection = CreateConnection();
            var result = connection
                .Query<FilmDto>("select * from GetChildrenFilms(@id)", new { id }).ToList();
            return result;
        }

        public async Task<List<GenreDto>> GetAllGenresAsync()
        {
            using var connection = CreateConnection();
            var result = connection
                .Query<GenreDto>("select * from GetAllGenres()").ToList();
            return result;
        }

        public async Task<List<int>> GetFilmGenresAsync(int filmId)
        {
            using var connection = CreateConnection();
            var result = (await connection
                .QueryAsync<int>("select * from GetFilmGenres(@id)", new
                {
                    id = filmId
                })).ToList();
            return result;
        }

        public async Task<List<FilmRelatedPersonDto>> GetFilmRelatedPersonsAsync(int filmId)
        {
            using var connection = CreateConnection();
            var result = connection
                .Query<FilmRelatedPersonDto>("select * from GetFilmRelatedPersons(@filmId)", new
                {
                    filmId
                }).ToList();
            return result;
        }

        public async Task<string> RemoveFilmRelatedPersonAsync(int filmRelatedPersonId)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection
                    .QueryFirstAsync<string>("select RemoveRelatedPerson(@filmRelatedPersonId)"
                    , new
                    {
                        filmRelatedPersonId
                    });
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> AddFilmRelatedPersonAsync(int filmPersonId, int filmRoleId,
            int filmId)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection
                    .QueryFirstAsync<string>("select AddFilmRelatedPerson(@filmPersonId, @filmRoleId, @filmId)"
                    , new {
                            filmPersonId,
                            filmRoleId,
                            filmId
                    });
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<FilmRoleDto>> GetAllFilmRolesAsync()
        {
            using var connection = CreateConnection();
            var result = connection
                .Query<FilmRoleDto>("select * from GetAllFilmRoles()").ToList();
            return result;
        }

        public async Task<List<FilmDto>> GetWatchSuggestionsAsync(int userId)
        {
            using var connection = CreateConnection();
            var result = (await connection
                .QueryAsync<FilmDto>("select * from GetWatchSuggestions(@userId)", new
                {
                    userId
                })).ToList();
            return result;
        }
    }
}
