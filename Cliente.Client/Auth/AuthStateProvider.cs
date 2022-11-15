
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using System.Text.Json;
using Blazored.LocalStorage;
using System.Net.Http.Json;
using Cliente.Shared;

namespace Cliente.Client.Auth
{
	public class AuthStateProvider : AuthenticationStateProvider, IAuth
	{
        private readonly ILocalStorageService _localStorage;
        private readonly HttpClient http;
        public AuthStateProvider(ILocalStorageService localStorageService, HttpClient http)
        {
            this._localStorage = localStorageService;
            this.http = http;
        }
		public override async Task<AuthenticationState> GetAuthenticationStateAsync()
		{
            try
            {
                string token = await _localStorage.GetItemAsStringAsync("token");
                var data = await http.PutAsJsonAsync("/api/usuario/token",new TokenDTO(token));
                if (data.IsSuccessStatusCode)
                {
                    var state = BuildAuthenticationState(token);
                    NotifyAuthenticationStateChanged(Task.FromResult(state));
			        return state;
                }
                else
                {
                    await _localStorage.RemoveItemAsync("token");
                    var state = BuildAuthenticationState(null);
                    NotifyAuthenticationStateChanged(Task.FromResult(state));
                    return state;
                }
            }
            catch (Exception)
            {
                await _localStorage.RemoveItemAsync("token");
                var state = BuildAuthenticationState(null);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
                return state;
            }
		}
        public async Task<UsuarioDTO?> GetUsuario()
        {
            string token = await _localStorage.GetItemAsStringAsync("token");
            var data = await http.PutAsJsonAsync("/api/usuario/token", new TokenDTO(token));
            if (data.IsSuccessStatusCode)
            {
                var options = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<UsuarioDTO>(await data.Content.ReadAsStringAsync(), options);
            }
            else
            {
                await _localStorage.RemoveItemAsync("token");
                var state = BuildAuthenticationState(null);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
            return null;
        }
        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("token");
            var state = BuildAuthenticationState(null);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }

        public async Task Login(string token)
        {
            await _localStorage.SetItemAsStringAsync("token", token);
            var state = BuildAuthenticationState(token);
            NotifyAuthenticationStateChanged(Task.FromResult(state));
        }

        private AuthenticationState BuildAuthenticationState(string? token)
        {
            var identity = new ClaimsIdentity();
            if (!string.IsNullOrEmpty(token)) identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
