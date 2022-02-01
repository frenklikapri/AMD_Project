using Movies.Dtos;

namespace Movies.Services.Persons
{
    public interface IPersonsService
    {
        Task<string> SavePersonAsync(PersonDto person);
        Task<string> DeletePersonAsync(int id);
        Task<List<PersonDto>> GetAllPersonsAsync();
    }
}
