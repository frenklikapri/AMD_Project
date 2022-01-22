using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Movies.Dtos;
using Movies.Services.Films;

namespace Movies.Pages
{
    public partial class Films
    {
        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        public IFilmsService FilmsService { get; set; }

        private FilmDto _filmToSave = new();
        private FilmDto _filmToDelete = new();
        private IJSObjectReference _module;
        private List<FilmDto> _films = new();

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

        async Task SetFilms()
        {
            _films = (await FilmsService.GetFilmsAsync(new HelperClasses.PaginationParameters
            {
                Page = 1,
                PageSize = 99999
            })).Items;
        }

        async Task SaveFilm()
        {
            var film = await FilmsService.SaveFilmAsync(_filmToSave);
            _films = _films
                .Where(f => f.Id != film.Id)
                .ToList();
            await SetFilms();
            await _module.InvokeAsync<string>("hideModalWithId", "saveFilmModal");
        }

        async Task DeleteApproved()
        {
            var success = await FilmsService.DeleteFilmAsync(_filmToDelete.Id);
            await SetFilms();
            await _module.InvokeAsync<string>("hideModalWithId", "deleteFilmModal");
        }

        async Task EditClicked(FilmDto film)
        {
            _filmToSave = film;
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
                ReleaseDate = DateOnly.FromDateTime(DateTime.Now)
            };
            await _module.InvokeAsync<string>("showModalWithId", "saveFilmModal");
        }
    }
}
