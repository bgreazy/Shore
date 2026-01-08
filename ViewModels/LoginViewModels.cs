using Sitting.Services;
using System;
using System.Threading.Tasks;

namespace Sitting.ViewModels
{
    public class LoginViewModel
    {
        private readonly FirebaseAuthService _authService;

        public LoginViewModel(FirebaseAuthService authService)
        {
            _authService = authService;
        }

        public async Task<(bool Success, string Role, string Error)> LoginAsync(string email, string password)
        {
            try
            {
                var authResult = await _authService.SignInWithEmailPasswordAsync(email, password);
                var uid = authResult.UserId;

                var role = await _authService.GetUserRoleAsync(uid, authResult.IdToken);

                if (role == null)
                    return (false, null, "User has no assigned role.");

                return (true, role, null);
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }
    }
}