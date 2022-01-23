using Movies.Dtos;
using Movies.HelperClasses.Extensions;

namespace Movies.Services.Persons
{
    public class DummyPersonsService : IPersonsService
    {
        private List<PersonDto> _persons = new List<PersonDto>()
        {
            new PersonDto(Guid.NewGuid(), "Frenkli Kapri", "M", new DateOnly(1998, 8, 15)),
            new PersonDto(Guid.NewGuid(), "John Doe", "M", new DateOnly(1988, 1, 16)),
        };

        public async Task<bool> DeletePersonAsync(Guid id)
        {
            var person = _persons
                .First(p => p.Id == id);

            _persons.Remove(person);

            return true;
        }

        public async Task<List<PersonDto>> GetAllPersonsAsync()
        {
            return _persons;
        }

        public async Task<PersonDto> SavePersonAsync(PersonDto personDto)
        {
            var person = _persons
                .FirstOrDefault(p => p.Id == personDto.Id);

            if(person is null)
            {
                person = personDto.CopyWithNewId();
                _persons.Add(person);
            }
            else
            {
                person.Sex = personDto.Sex;
                person.Name = personDto.Name;
                person.Birthday = personDto.Birthday;
            }

            return person;
        }
    }
}
