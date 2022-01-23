using Movies.Dtos;

namespace Movies.Services.Persons
{
    public interface IPersonsService
    {
        Task<PersonDto> SavePersonAsync(PersonDto person);
        Task<bool> DeletePersonAsync(Guid id);
        Task<List<PersonDto>> GetAllPersonsAsync();
    }
}
