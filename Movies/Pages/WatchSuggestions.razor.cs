using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Movies.Dtos;
using Movies.Services.Films;
using Movies.Services.Users;

namespace Movies.Pages
{
    public partial class WatchSuggestions
    {
        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        public IFilmsService FilmsService { get; set; }

        [Inject]
        public IUsersService UsersService { get; set; }

        private List<FilmDto> _films = new();
        private List<UserDto> _users = new();

        protected override async Task OnInitializedAsync()
        {
            await SetData();
            await base.OnInitializedAsync();
        }

        async Task SetData()
        {
            _users = await UsersService.GetAllUsersAsync();
        }

        private async Task UserChanged(ChangeEventArgs e)
        {
            if (e.Value is null)
                return;

            int.TryParse(e.Value.ToString(), out int userId);

            _films = await FilmsService.GetWatchSuggestionsAsync(userId);
            InvokeAsync(StateHasChanged);
        }
    }
}
