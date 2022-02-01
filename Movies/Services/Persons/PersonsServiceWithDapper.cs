using Dapper;
using Movies.Dtos;
using Npgsql;

namespace Movies.Services.Persons
{
    public class PersonsServiceWithDapper : IPersonsService
    {
        private string _connString = string.Empty;

        public PersonsServiceWithDapper(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("PostgreSQLConnection");
        }

        public async Task<string> DeletePersonAsync(int id)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection
                    .QueryFirstAsync<string>("select DeleteFilmPerson(@id)", new
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

        public async Task<List<PersonDto>> GetAllPersonsAsync()
        {
            using var connection = CreateConnection();
            var result = (await connection
                .QueryAsync<PersonDto>("select * from GetFilmPersons()")).ToList();
            return result;
        }

        public async Task<string> SavePersonAsync(PersonDto person)
        {
            try
            {
                using var connection = CreateConnection();
                var result = await connection
                    .QueryFirstAsync<string>("select SaveFilmPerson(@Id, @Name, @Birthday::date, @Sex, @ProfilePic)", person);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connString);
        }
    }
}
