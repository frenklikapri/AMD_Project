using System.ComponentModel.DataAnnotations;

namespace Movies.Dtos
{
    public class PersonDto
    {
        public PersonDto()
        {

        }

        public PersonDto(Guid id, string name, string sex, DateOnly birthday)
        {
            Id = id;
            Name = name;
            Sex = sex;
            Birthday = birthday;
        }

        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public DateOnly Birthday { get; set; }
    }
}
