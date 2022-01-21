using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Movies.Dtos;

namespace Movies.Pages
{
    public partial class Films
    {
        [Inject]
        public IJSRuntime JS { get; set; }

        private FilmDto _filmToSave = new();
        private IJSObjectReference _module;
        private List<FilmDto> _films = new();

        protected override async Task OnInitializedAsync()
        {
            _films = new List<FilmDto>
            {
                new FilmDto(Guid.NewGuid(), "The Battle at Lake Changjin", new DateOnly(2021, 9, 21)),
                new FilmDto(Guid.NewGuid(), "No Time to Die", new DateOnly(2021, 9, 28))
            };
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

        async Task SaveFilm()
        {
            await _module.InvokeAsync<string>("hideModalWithId", "saveFilmModal");
        }

        async Task EditClicked(FilmDto film)
        {
            _filmToSave = film;
            await _module.InvokeAsync<string>("showModalWithId", "saveFilmModal");
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
