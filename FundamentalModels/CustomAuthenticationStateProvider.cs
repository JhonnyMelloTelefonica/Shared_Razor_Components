
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Shared_Razor_Components.Services;
using Shared_Razor_Components.FundamentalModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Shared_Razor_Components.FundamentalModels
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        public IHttpContextAccessor _contextAccessor;
        public IHostEnvironment _Networkacessor;
        public IPrincipalService _contextservice;
        public GetUser_REDECORP _getUser_REDECORP;
        public AcessoModel? User;
        public UserService UserService;
        public NavigationManager _navigationManager;
        //public bool admin => User.Perfil.Any(x=> x.Perfil_Plataforma.ID_PERFIL == 1);
        //public bool SOLICITANTE_USER => User.Perfil.Any(x=> x.Perfil_Plataforma.ID_PERFIL == 2);
        //public bool JORNADA_CREATOR_MASTER => User.Perfil.Any(x=> x.Perfil_Plataforma.ID_PERFIL == 3);

        public CustomAuthenticationStateProvider(IPrincipalService contextservice,
            NavigationManager navigationManager,
            GetUser_REDECORP getUser_REDECORP,
            IHttpContextAccessor contextAccessor,
            IHostEnvironment Networkacessor,
            UserService _User)
        {
            UserService = _User;
            _navigationManager = navigationManager;
            _contextAccessor = contextAccessor;
            _Networkacessor = Networkacessor;
            _contextservice = contextservice;
            _getUser_REDECORP = getUser_REDECORP;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new();
            await LoadUserData(_getUser_REDECORP.GetMatricula());

            identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, User?.MATRICULA.ToString() ?? "000"),
                new Claim(ClaimTypes.Email, User?.EMAIL ?? "0@telefonica.com"),
                new Claim(ClaimTypes.HomePhone, "(00) 00000-0000"),
                new Claim(ClaimTypes.IsPersistent, "true")
                // Adicione outras reivindicações conforme necessário
            }, "Autenticação REDECORP");

            var user = new ClaimsPrincipal(identity);
            return new AuthenticationState(user);
        }

        public async Task LoadUserData(int matricula)
        {
            var result = await _contextservice.GetUserByMatricula(matricula);
            if (result.IsSuccess)
            {
                Response<object> saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                if (saida.Succeeded)
                {
                    // Salvar informações do usuário no Armazenamento Local
                    var user = JsonConvert.DeserializeObject<AcessoModel>(saida.Data.ToString());
                    UserService.User = user;
                    User = user;
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                }
            }
            return;
        }
    }
}



