using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Movies.Dtos;
using Movies.HelperClasses.Extensions;
using Movies.Services.Films;
using Movies.Services.Persons;

namespace Movies.Pages
{
    public partial class Films
    {
        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        public IFilmsService FilmsService { get; set; }

        [Inject]
        public IPersonsService PersonsService { get; set; }

        private FilmDto _filmToSave = new();
        private FilmDto _filmToDelete = new();
        private IJSObjectReference _module;
        private List<FilmDto> _films = new();
        private List<GenreDto> _allGenres = new();
        private ElementReference sel;
        private FilmDto _filmToManagePerson = new();
        private List<FilmRelatedPersonDto> _currentPersons = new();
        private List<PersonDto> _allPersons = new List<PersonDto>();
        private int _selectedPersonId;
        private int _selectedRoleId;
        private List<FilmRoleDto> _allRoles = new();
        private List<PersonDto> _personsToAdd
        {
            get
            {
                return _allPersons;
            }
        }

        private List<FilmDto> _filmsToShow
        {
            get
            {
                var list = _films
                    .Where(f => f.ParentId == null)
                    .ToList();

                return list;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await SetFilms();
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

        List<FilmDto> GetChildren(FilmDto film)
        {
            var children = FilmsService
                .GetChildrenFilms(film.Id);

            return children;
        }

        void ChangeShowChildren(FilmDto film)
        {
            film.ShowChildren = !film.ShowChildren;
        }

        List<FilmDto> GetParentFilms()
        {
            var films = _films
                //.Where(f => f.ParentId == null && f.Id != _filmToSave.Id)
                .ToList();
            return films;
        }

        void ParentFilmChanged(ChangeEventArgs args)
        {
            var val = args.Value.ToString();

            if (string.IsNullOrEmpty(val))
            {
                _filmToSave.ParentId = null;
                return;
            }

            int.TryParse(val, out int parentId);

            _filmToSave.ParentId = parentId;
        }

        async Task SetFilms()
        {
            _films = await FilmsService.GetAllFilmsAsync();
            _allGenres = await FilmsService.GetAllGenresAsync();
            _allPersons = await PersonsService.GetAllPersonsAsync();
            _allRoles = await FilmsService.GetAllFilmRolesAsync();
        }

        async Task SaveFilm()
        {
            var result = await FilmsService.SaveFilmAsync(_filmToSave);
            if (string.IsNullOrWhiteSpace(result))
            {
                await JS.ShowSuccessAsync("Film saved successfully!");
                await SetFilms();
                await _module.InvokeAsync<string>("hideModalWithId", "saveFilmModal");
            }
            else
            {
                await JS.ShowErrorAsync($"Couldn't save film: {result}");
            }
        }

        async Task DeleteApproved()
        {
            var result = await FilmsService.DeleteFilmAsync(_filmToDelete.Id);

            if (string.IsNullOrWhiteSpace(result))
            {
                await JS.ShowSuccessAsync("Film deleted successfully!");
                await SetFilms();
                await _module.InvokeAsync<string>("hideModalWithId", "deleteFilmModal");
            }
            else
            {
                await JS.ShowErrorAsync($"Couldn't delete film: {result}");
            }
        }

        async Task EditClicked(FilmDto film)
        {
            _filmToSave = film.Copy();
            _filmToSave.GenresToSave = (await FilmsService.GetFilmGenresAsync(_filmToSave.Id)).ToArray();
            await JS.InvokeAsync<string>("setMultipleValues", "genres", string.Join(',', _filmToSave.GenresToSave));
            await _module.InvokeAsync<string>("showModalWithId", "saveFilmModal");
        }

        async Task DeleteClicked(FilmDto film)
        {
            _filmToDelete = film;
            await _module.InvokeAsync<string>("showModalWithId", "deleteFilmModal");
        }

        async Task AddClicked()
        {
            _filmToSave = new FilmDto
            {
                ReleaseDate = DateTime.Now
            };
            await _module.InvokeAsync<string>("showModalWithId", "saveFilmModal");
        }

        public async Task<List<int>> GetAllSelections()
        {
            var list = (await JS.InvokeAsync<List<string>>("getSelectedValues", "genres")).ToList();

            return list.Select(l => int.Parse(l)).ToList();
        }

        private async Task GenresChanged()
        {
            List<int> genres = await GetAllSelections();
            _filmToSave.GenresToSave = genres.ToArray();
        }

        async void RemoveFilmPerson(FilmRelatedPersonDto person)
        {
            var result = await FilmsService.RemoveFilmRelatedPersonAsync(person.FilmRelatedPersonId);

            if(string.IsNullOrWhiteSpace(result))
            {
                _currentPersons = await FilmsService.GetFilmRelatedPersonsAsync(_filmToManagePerson.Id);
                await JS.ShowSuccessAsync("Film related person removed successfully!");
            }
            else
            {
                await JS.ShowErrorAsync($"Couldn't remove film related person: {result}");
            }
            InvokeAsync(StateHasChanged);
        }

        async void ManagePersons(FilmDto film)
        {
            _filmToManagePerson = film;
            _currentPersons = await FilmsService.GetFilmRelatedPersonsAsync(film.Id);
            await JS.ShowModalWithIdAsync("manageFilmPersonsModal");
        }

        async void AddFilmRelatedPerson()
        {
            if(_selectedRoleId == 0 || _selectedPersonId == 0)
            {
                await JS.ShowErrorAsync($"Please select a role and a person!");
                return;
            }

            var result = await FilmsService.AddFilmRelatedPersonAsync(_selectedPersonId, _selectedRoleId,
                _filmToManagePerson.Id);

            if (string.IsNullOrWhiteSpace(result))
            {
                _currentPersons = await FilmsService.GetFilmRelatedPersonsAsync(_filmToManagePerson.Id);
                await JS.ShowSuccessAsync("Film related person added successfully!");
                _selectedPersonId = 0;
                _selectedRoleId = 0;
            }
            else
            {
                await JS.ShowErrorAsync($"Couldn't add film related person: {result}");
            }
            InvokeAsync(StateHasChanged);
        }
    }
}
