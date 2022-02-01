using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Movies.Dtos;
using Movies.HelperClasses.Extensions;
using Movies.Services.Persons;

namespace Movies.Pages
{
    public partial class Persons
    {
        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        public IPersonsService PersonsService { get; set; }

        private PersonDto _personToSave = new();
        private PersonDto _personToDelete = new();
        private List<PersonDto> _persons = new();

        protected override async Task OnInitializedAsync()
        {
            await SetPersons();
            await base.OnInitializedAsync();
        }

        async Task SetPersons()
        {
            _persons = await PersonsService.GetAllPersonsAsync();
        }

        async Task SavePerson()
        {
            var result = await PersonsService.SavePersonAsync(_personToSave);

            if (string.IsNullOrWhiteSpace(result))
            {
                await SetPersons();
                await JS.HideModalWithIdAsync("savePersonModal");
                await JS.ShowSuccessAsync("Person saved successfully!");
            }
            else
            {
                await JS.ShowErrorAsync($"Couldn't save person: {result}");
            }
        }

        async Task DeleteApproved()
        {
            var success = await PersonsService.DeletePersonAsync(_personToDelete.Id);
            await SetPersons();
            await JS.HideModalWithIdAsync("deletePersonModal");
            await JS.ShowSuccessAsync("Person deleted successfully!");
        }

        async Task EditClicked(PersonDto person)
        {
            _personToSave = person.Copy();
            await JS.ShowModalWithIdAsync("savePersonModal");
        }

        async Task DeleteClicked(PersonDto person)
        {
            _personToDelete = person;
            await JS.ShowModalWithIdAsync("deletePersonModal");
        }

        async Task AddClicked()
        {
            _personToSave = new PersonDto
            {
                Birthday = DateTime.Now
            };
            await JS.ShowModalWithIdAsync("savePersonModal");
        }

        void SexChanged(ChangeEventArgs args)
        {
            var val = args.Value.ToString();

            if (string.IsNullOrEmpty(val))
            {
                _personToSave.Sex = null;
                return;
            }

            _personToSave.Sex = val;
        }
    }
}
