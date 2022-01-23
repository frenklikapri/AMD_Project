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
        private IJSObjectReference _module;
        private List<PersonDto> _persons = new();

        protected override async Task OnInitializedAsync()
        {
            await SetPersons();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _module = await JS.InvokeAsync<IJSObjectReference>("import",
                    "./js/dialogs.js");
            }
        }

        async Task SetPersons()
        {
            _persons = await PersonsService.GetAllPersonsAsync();
        }

        async Task SavePerson()
        {
            var person = await PersonsService.SavePersonAsync(_personToSave);
            _persons = _persons
                .Where(f => f.Id != person.Id)
                .ToList();
            await SetPersons();
            await _module.InvokeAsync<string>("hideModalWithId", "savePersonModal");
        }

        async Task DeleteApproved()
        {
            var success = await PersonsService.DeletePersonAsync(_personToDelete.Id);
            await SetPersons();
            await _module.InvokeAsync<string>("hideModalWithId", "deletePersonModal");
        }

        async Task EditClicked(PersonDto person)
        {
            _personToSave = person.Copy();
            await _module.InvokeAsync<string>("showModalWithId", "savePersonModal");
        }

        async Task DeleteClicked(PersonDto person)
        {
            _personToDelete = person;
            await _module.InvokeAsync<string>("showModalWithId", "deletePersonModal");
        }

        async Task AddClicked()
        {
            _personToSave = new PersonDto
            {
                Birthday = DateOnly.FromDateTime(DateTime.Now)
            };
            await _module.InvokeAsync<string>("showModalWithId", "savePersonModal");
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
