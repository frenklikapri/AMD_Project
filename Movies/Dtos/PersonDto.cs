using NpgsqlTypes;
using System;
using System.ComponentModel.DataAnnotations;

namespace Movies.Dtos
{
    public class PersonDto
    {
        public PersonDto()
        {

        }

        public PersonDto(int id, string name, string sex, DateTime birthday, string profilePic)
        {
            Id = id;
            Name = name;
            Sex = sex;
            Birthday = birthday;
            ProfilePic = profilePic;
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Sex { get; set; }

        [Required]
        public DateTime Birthday { get; set; }

        public string ProfilePic { get; set; }
    }
}
