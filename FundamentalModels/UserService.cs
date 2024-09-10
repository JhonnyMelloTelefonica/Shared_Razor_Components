using Newtonsoft.Json;
using Shared_Razor_Components.Services;

namespace Shared_Razor_Components.FundamentalModels
{
    public class UserService
    {
        private AcessoModel? _User;
        private readonly UsersAtivos usuariosAtivos;
        private GetUser_REDECORP getUser_REDECORP;
        private readonly IPrincipalService _principalService;

        public UserService(UsersAtivos usuariosAtivos, GetUser_REDECORP getUser_REDECORP, IPrincipalService principalService)
        {
            this.usuariosAtivos = usuariosAtivos;
            this.getUser_REDECORP = getUser_REDECORP;
            _principalService = principalService;
        }

        public AcessoModel? User
        {
            get
            {
                if (_User is null)
                {
                    try
                    {
                        int matricula = getUser_REDECORP.GetMatricula();

                        Task.Run(async () => await AttUserData(matricula)).Wait();

                        if (!usuariosAtivos.UsuariosAtivos.Any(x => x.MATRICULA == matricula))
                        {
                            usuariosAtivos.UsuariosAtivos.Add(_User);
                        }

                        return _User;
                    }
                    catch (Exception ex)
                    {
                        return new();
                    }

                }
                return _User;
            }
            set
            {
                //usuariosAtivos.AddUsuario(value);
                _User = value;
            }
        }
        public async Task AttUserData(int matricula)
        {
            AcessoModel usuario = new();
            var result = await _principalService.GetUserByMatricula(matricula);
            if (result.IsSuccess)
            {
                Response<object> saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                if (saida.Succeeded)
                {
                    usuario = JsonConvert.DeserializeObject<AcessoModel>(saida.Data.ToString());
                }
            }
            _User = usuario;
        }

    }
}
