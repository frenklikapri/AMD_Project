using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Movies.Dtos;
using Movies.HelperClasses.Extensions;
using Movies.Services.Users;

namespace Movies.Pages
{
    public partial class UserRatings
    {
        [Inject]
        public IJSRuntime JS { get; set; }

        [Inject]
        public IUsersService UsersService { get; set; }

        private List<UserDto> _users = new();
        private List<FilmRatingDto> _ratings = new();
        private int _currentUserId;
        private bool _yourRatings;
        private List<SimpleFilmDto> _filmsToRate = new();

        public bool YourRatings { get => _yourRatings; set => _yourRatings = value; }

        protected override async Task OnInitializedAsync()
        {
            await SetUsers();
            await base.OnInitializedAsync();
        }

        private async Task SetUsers()
        {
            _users = await UsersService.GetAllUsersAsync();
        }

        private async Task UserChanged(ChangeEventArgs e)
        {
            if (e.Value is null)
                return;

            int.TryParse(e.Value.ToString(), out int userId);
            _currentUserId = userId;

            _ratings = await UsersService.GetFilmRatingsByUserIdAsync(userId);
            _filmsToRate = await UsersService.GetFilmsToRateAsync(userId);

            InvokeAsync(StateHasChanged);
        }

        private async Task RemoveRating(FilmRatingDto rating)
        {
            var result = await UsersService.RemoveRatingAsync(rating.FilmUserRatingId);

            if (string.IsNullOrWhiteSpace(result))
            {

                await JS.ShowSuccessAsync("Rating done successfully");
            }
            else
            {
                await JS.ShowErrorAsync($"Couldn't remove rating: {result}");
            }
            _ratings = await UsersService.GetFilmRatingsByUserIdAsync(_currentUserId);
            InvokeAsync(StateHasChanged);
        }

        void RatingMouseOver(FilmRatingDto rating, int index)
        {
            rating.HoverRating = index;
        }

        async Task UpdateRating(FilmRatingDto rating, int index)
        {
            var result = await UsersService.SaveRatingAsync(rating.FilmUserRatingId, index, 0, 0);

            if (string.IsNullOrWhiteSpace(result))
            {
                _ratings = await UsersService.GetFilmRatingsByUserIdAsync(_currentUserId);
                InvokeAsync(StateHasChanged);
                await JS.ShowSuccessAsync("Rating updated successfully");
            }
            else
            {
                await JS.ShowErrorAsync($"Couldn't update rating: {result}");
            }
        }

        void RatingMouseOver(SimpleFilmDto rating, int index)
        {
            rating.Rating = index;
        }

        async Task UpdateRating(SimpleFilmDto rating, int index)
        {
            var result = await UsersService.SaveRatingAsync(0, index, rating.FilmId, _currentUserId);

            if (string.IsNullOrWhiteSpace(result))
            {
                _ratings = await UsersService.GetFilmRatingsByUserIdAsync(_currentUserId);
                _filmsToRate = await UsersService.GetFilmsToRateAsync(_currentUserId);
                InvokeAsync(StateHasChanged);
                await JS.ShowSuccessAsync("Rating updated successfully");
            }
            else
            {
                await JS.ShowErrorAsync($"Couldn't update rating: {result}");
            }
        }
    }
}
