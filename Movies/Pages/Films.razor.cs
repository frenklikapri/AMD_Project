using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Movies.Dtos;
using Movies.HelperClasses.Extensions;
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

        bool HasChildren(FilmDto film)
        {
            var hasChildren = _films.Any(f => f.ParentId == film.Id);

            return hasChildren;
        }

        List<FilmDto> GetChildren(FilmDto film)
        {
            var children = _films
                .Where(f => f.ParentId == film.Id)
                .ToList();

            return children;
        }

        void ChangeShowChildren(FilmDto film)
        {
            film.ShowChildren = !film.ShowChildren;
        }

        List<FilmDto> GetParentFilms()
        {
            var films = _films
                .Where(f => f.ParentId == null && f.Id != _filmToSave.Id)
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

            var guid = Guid.Parse(val);
            _filmToSave.ParentId = guid;
        }

        async Task SetFilms()
        {
            _films = await FilmsService.GetAllFilmsAsync();
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
            _filmToSave = film.Copy();
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
