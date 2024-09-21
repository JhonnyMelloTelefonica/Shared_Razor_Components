using BlazorDownloadFile;
using Blazorise;
using Blazorise.LoadingIndicator;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Shared_Razor_Components.Services;
using Shared_Static_Class.Data;
using Shared_Static_Class.ErrorModels;
using Shared_Razor_Components.FundamentalModels;
using System.ComponentModel;
using Shared_Razor_Components.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

using Microsoft.JSInterop;
using BlazorBootstrap;


namespace Shared_Razor_Components.ViewModels
{
    public partial class ControleUsuariosAppViewModel : VivoAppsViewModel
    {
        public ControleUsuariosAppViewModel(IHttpContextAccessor httpAccessor, IWebHostEnvironment networkacessor, GetUser_REDECORP getUser_REDECORP, IPageProgressService pageProgressService, IJSRuntime jSRuntime, PreloadService preloadService, IBlazorDownloadFileService dowloader, Radzen.DialogService radzendialog, Microsoft.FluentUI.AspNetCore.Components.IDialogService fluentDialog, Blazorise.IMessageService messageService, ILoadingIndicatorService applicationLoadingIndicatorService, SweetAlertService swal, NavigationManager navigationManager, UserService userservice, Microsoft.FluentUI.AspNetCore.Components.IToastService _ToastService, IAcessoPendenteByIdService _AcessoPendenteByldService, IAcessoTerceirosService _AcessoTerceirosService, IAnswerFormService _AnswerFormService, ICardapioDigitalService _CardapioDigitalService, IConsultarDemandasService _ConsultarDemandasService, IControleDemandaService _ControleDemandaService, IControleUsuariosAppService _ControleUsuariosAppService, ICreateFormService _CreateFormService, ICreateQuestionService _CreateQuestionService, IDesligamentosService _DesligamentosService, IEditQuestionService _EditQuestionService, IEditSingleQuestionService _EditSingleQuestionService, IEditUserService _EditUserService, IJornadaHierarquiaService _JornadaHierarquiaService, IListaFormService _ListaFormService, IPainelProvasRealizadasService _PainelProvasRealizadasService, IPainelUsuariosService _PainelUsuariosService, IPrincipalService _PrincipalService, IPWService _PWService, IRegisterService _RegisterService, IResultadosProvaService _ResultadosProvaService) : base(httpAccessor, networkacessor, getUser_REDECORP, pageProgressService, jSRuntime, preloadService, dowloader, radzendialog, fluentDialog, messageService, applicationLoadingIndicatorService, swal, navigationManager, userservice, _ToastService, _AcessoPendenteByldService, _AcessoTerceirosService, _AnswerFormService, _CardapioDigitalService, _ConsultarDemandasService, _ControleDemandaService, _ControleUsuariosAppService, _CreateFormService, _CreateQuestionService, _DesligamentosService, _EditQuestionService, _EditSingleQuestionService, _EditUserService, _JornadaHierarquiaService, _ListaFormService, _PainelProvasRealizadasService, _PainelUsuariosService, _PrincipalService, _PWService, _RegisterService, _ResultadosProvaService)
        {
            if (Userservice.User.Perfil.Any(x => x.Perfil_Plataforma.ID_PERFIL == 1))
            {
                filter.IsSuporte = true;
            }
            else if (Userservice.User.Perfil.Any(x => x.Perfil_Plataforma.ID_PERFIL == 10))
            {
                filter.IsSuporte = true;
                filter.Regional = new List<string> {
                Userservice.User.REGIONAL
                };
            }
            else
            {
                filter.IsSuporte = false;
            }

            GetPerfisUsuario();
        }

        public FileContent file { get; set; }

        public Blazorise.Modal modalCriarUserMassivoRef;
        public async Task DowloadPage()
        {
            IsBusy = true;
            var result = await ControleUsuariosAppService.GetUsuariosExcel(filter);
            if (result.IsSuccess)
            {
                Response<object> saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                if (saida.Succeeded)
                {
                    file = JsonConvert.DeserializeObject<FileContent>(saida.Data.ToString());
                    IsBusy = false;
                    await MessageService.Success(saida.Message, "Tudo certo!");
                    return;
                }
                else
                {
                    IsBusy = false;
                    var error = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                    await ErrorModel(error);
                    return;
                }
            }
            IsBusy = false;
            return;
        }

        public async Task UpdateUsuarioMassivo(List<SOLICITAR_USUARIO_MODEL> data)
        {
            var saida = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = $"Tem certeza que deseja alterar ({data.Count}) PDV's?",
                Text = "Por favor muita atenção com alterações massivas, revise as informações e garanta que estão corretas antes de subir.",
                Icon = SweetAlertIcon.Warning,
                InputAutoFocus = true,
                ShowCancelButton = false,
                ConfirmButtonText = "Sim",
                CancelButtonText = "Não",
                DenyButtonText = "Cancelar",
                ShowCloseButton = true,
                ShowDenyButton = true,
                AllowEscapeKey = false,
                AllowEnterKey = true,
                ShowLoaderOnConfirm = true,
                AllowOutsideClick = false,
            });

            if (saida.IsConfirmed)
            {
                IsBusy = true;
                var result = await ControleUsuariosAppService.UpdateUsuarioMassivo(data, Userservice.User.MATRICULA);
                if (result.IsSuccess)
                {
                    Response<object> retorno = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                    if (retorno.Succeeded)
                    {
                        await MessageService.Success(retorno.Message, "Tudo Certo!");
                        IsBusy = false;
                        return;
                    }
                    else
                    {
                        IsBusy = false;
                        var error = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                        await ErrorModel(error);
                        return;
                    }
                }
                else
                {
                    IsBusy = false;
                    var error = result.Content as ErrorResponse;
                    await MessageService.Error("Erro(s): " + string.Join(" - ", error.Errors.Select((x, y) => $"Linha N° {y + 1} {x.Key.Split('.')[1]}: {string.Join(", ", x.Value)}")), error.Title);
                    return;
                }
            }
            await modalCriarUserMassivoRef.Hide();
            return;
        }
        public async Task CommaReloadFilterData()
        {
            IsFilterBusy = true;
            var result = await ControleUsuariosAppService.GetUsuariosFilter();
            if (result.IsSuccess)
            {
                dynamic var = JsonConvert.DeserializeObject<dynamic>(result.Content.ToString());

                datacarteira = ((JArray)var.datacarteira).ToObject<List<JORNADA_BD_HIERARQUIum>>();
                dataCanal_Cargo = ((JArray)var.dataCanal_Cargo).ToObject<List<JORNADA_BD_CARGOS_CANAL>>();
            }
            else
            {
                var error = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                await ErrorModel(error);
            }
            IsFilterBusy = false;
        }
        public async Task CommaReloadPage()
        {
            var result = await ControleUsuariosAppService.GetUsuarios(filter);
            if (result.IsSuccess)
            {
                actual_StatePage = JsonConvert.DeserializeObject<GenericStatePage<SOLICITAR_USUARIO_MODEL>>(result.Content.ToString());
                data = actual_StatePage.Data;
            }
            else
            {
                var error = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                await ErrorModel(error);
            }
        }
        public async Task GetPerfisUsuario()
        {
            var result = await ControleUsuariosAppService.GetPerfisUsuario();
            if (result.IsSuccess)
            {
                var saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                if (saida.Succeeded)
                {
                    var listperfis = JsonConvert.DeserializeObject<List<PERFIL_PLATAFORMAS_VIVO>>(saida.Data.ToString());
                    perfis = listperfis.Select(x => new PERFIL_PLATAFORMAS_VIVO_DISSMISS(x.ID_PERFIL, x.Perfil, x.obs, x.CARGO)).ToList();
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                    await ErrorModel(error);
                }
            }
            return;
        }

        public async Task<MainResponse> UpdateUserSuporte(SOLICITAR_USUARIO_MODEL user)
        {
            var result = await ControleUsuariosAppService.UpdateUsuariosSuporte(user, Userservice.User.MATRICULA);
            if (result.IsSuccess)
            {
                return result;
            }
            else
            {
                return new MainResponse
                {
                    Content = "",
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> BloquearUsuarioSuporte(string TP_AFASTAMENTO, string OBS, int id)
        {
            var result = await ControleUsuariosAppService.BloquearUsuariosSuporte(TP_AFASTAMENTO, OBS, id, Userservice.User.MATRICULA);
            if (result.IsSuccess)
            {
                return result;
            }
            else
            {
                return new MainResponse
                {
                    Content = "",
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task<MainResponse> DesbloquearUsuarioSuporte(int id)
        {
            var result = await ControleUsuariosAppService.DesbloquearUsuariosSuporte(id, Userservice.User.MATRICULA);
            if (result.IsSuccess)
            {
                return result;
            }
            else
            {
                return new MainResponse
                {
                    Content = "",
                    IsSuccess = false,
                    ErrorMessage = "algum erro ocorreu"
                };
            }
        }
        public async Task UpdateUser(SOLICITAR_USUARIO_MODEL user, int ID_ACESSOS_MOBILE)
        {
            IsBusy = true;
            var result = await ControleUsuariosAppService.UpdateUsuarios(user, Userservice.User.MATRICULA, ID_ACESSOS_MOBILE);
            if (result.IsSuccess)
            {
                Response<object> saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                if (saida.Succeeded)
                {
                    await MessageService.Success(saida.Message, "Tudo Certo!");
                }
                else
                {
                    var error = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                    await ErrorModel(error);
                }
            }
            else
            {
                var error = result.Content as ErrorResponse;
                await MessageService.Error("Erro(s): " + string.Join(" - ", error.Errors.Select(x => $"{x.Key}: {string.Join(", ", x.Value)}")), error.Title);
            }
            IsBusy = false;
            return;

        }
        public async Task BloquearUsuario(int id)
        {
            IsBusy = true;
            var result = await ControleUsuariosAppService.BloquearUsuarios(id, Userservice.User.MATRICULA);
            if (result.IsSuccess)
            {
                Response<string> saida = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                if (saida.Succeeded)
                {
                    await MessageService.Success(saida.Message, "Tudo Certo!");
                }
                else
                {
                    await ErrorModel(saida);
                }
            }
            IsBusy = false;
            return;
        }
        public async Task DesbloquearUsuario(int id)
        {
            IsBusy = true;

            var result = await ControleUsuariosAppService.DesbloquearUsuarios(id, Userservice.User.MATRICULA);
            if (result.IsSuccess)
            {
                Response<string> saida = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                if (saida.Succeeded)
                {
                    await MessageService.Success(saida.Message, "Tudo Certo!");
                }
                else
                {
                    await ErrorModel(saida);
                }
            }
            IsBusy = false;
            return;
        }
        public async Task CriarUsuario(SOLICITAR_USUARIO_MODEL usuario,
            string OBS)
        {
            IsBusy = true;
            try
            {
                var result = await ControleUsuariosAppService.CriarUsuario(usuario, OBS, Userservice.User.MATRICULA);
                if (result.IsSuccess || result.Content is null)
                {
                    var response = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                    if (response.Succeeded)
                    {
                        var novoacesso = JsonConvert.DeserializeObject<ACESSOS_MOBILE_PENDENTE>(response.Data.ToString());
                        IsBusy = false;
                        await MessageService.Success(response.Message, "Atenção!");
                        NavigationManager.NavigateTo("/ControleUsuarios/solicitacao-acesso/" + novoacesso.ID, true);
                        return;
                    }
                    else
                    {
                        var error = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                        IsBusy = false;
                        await ErrorModel(error);
                        return;
                    }
                }
                else
                {
                    IsBusy = false;
                    await ErrorModel(new Response<string>
                    {
                        Data = string.Empty,
                        Message = "Houve algum erro na solicitação, por favor revise os valores enviados",
                        Errors = new string[] {
                        "Houve algum erro na solicitação, por favor revise os valores enviados"
                        },
                        Succeeded = false
                    });
                    return;
                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ErrorModel(new Response<string>
                {
                    Data = string.Empty,
                    Message = "Houve algum erro na solicitação, por favor revise os valores enviados",
                    Errors = new string[] {
                        "Houve algum erro na solicitação, por favor revise os valores enviados",
                        ""
                    },
                    Succeeded = false
                });

            }
        }
        public async Task CriarUsuarioMassivo(List<SOLICITAR_USUARIO_MODEL> usuarios,
            string OBS)
        {
            IsBusy = true;
            try
            {
                MainResponse result = await ControleUsuariosAppService.CriarUsuarioMassivo(usuarios, OBS, Userservice.User.MATRICULA);
                if (result.IsSuccess || result.Content is null)
                {
                    var saida = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                    IsBusy = false;
                    if (saida.Succeeded)
                    {
                        await MessageService.Info(saida.Message, "Tudo Certo!");
                    }
                    else
                    {
                        await MessageService.Error(saida.Message, "Algum erro ocorreu!");
                    }
                    return;
                }
                else
                {
                    IsBusy = false;
                    await ErrorModel(new Response<string>
                    {
                        Data = string.Empty,
                        Message = "Houve algum erro na solicitação, por favor revise os valores enviados",
                        Errors = new string[] { "Houve algum erro na solicitação, por favor revise os valores enviados" },
                        Succeeded = false
                    });
                    return;
                }

            }
            catch (Exception ex)
            {
                IsBusy = false;
                await ErrorModel(new Response<string>
                {
                    Data = string.Empty,
                    Message = "Houve algum erro na solicitação, por favor revise os valores enviados",
                    Errors = new string[] {
                        "Houve algum erro na solicitação, por favor revise os valores enviados",
                        ""
                    },
                    Succeeded = false
                });

            }
        }
        public async Task GoLastPage(int TotalPage)
        {
            filter.PageNumber = TotalPage;
            IsBusy = true;

            var result = await ControleUsuariosAppService.GetUsuarios(filter);

            actual_StatePage = JsonConvert.DeserializeObject<GenericStatePage<SOLICITAR_USUARIO_MODEL>>(result.Content.ToString());
            data = actual_StatePage.Data;
            IsBusy = !IsBusy;
        }
        public async Task GoFirstPage()
        {
            filter.PageNumber = 1;
            IsBusy = true;

            var result = await ControleUsuariosAppService.GetUsuarios(filter);

            actual_StatePage = JsonConvert.DeserializeObject<GenericStatePage<SOLICITAR_USUARIO_MODEL>>(result.Content.ToString());
            data = actual_StatePage.Data;
            IsBusy = !IsBusy;
        }
        public async Task GoToPage(int PageNumber)
        {
            filter.PageNumber = PageNumber;
            IsBusy = true;

            var result = await ControleUsuariosAppService.GetUsuarios(filter);

            actual_StatePage = JsonConvert.DeserializeObject<GenericStatePage<SOLICITAR_USUARIO_MODEL>>(result.Content.ToString());
            data = actual_StatePage.Data;
            IsBusy = !IsBusy;
        }
        public async Task<int> ValidarMatricula(ChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value.ToString()))
            {
                var inteiro = int.Parse(args.Value.ToString());
                if (inteiro > 0)
                {
                    IsBusy = true;
                    var result = await ControleUsuariosAppService.ValidarMatricula(inteiro);
                    if (result.IsSuccess)
                    {
                        Response<string> saida = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                        if (saida.Succeeded)
                        {
                            await MessageService.Success(saida.Message, "Tudo Certo!");
                            IsBusy = false;
                            return inteiro;
                        }
                        else
                        {
                            await ErrorModel(saida);
                        }
                    }
                    IsBusy = false;

                }
            }
            return 0;
        }
        public async Task<string> ValidarEmail(ChangeEventArgs args)
        {
            if (!string.IsNullOrEmpty(args.Value.ToString()) && args.Value.ToString().Contains('@'))
            {
                var email = args.Value.ToString();
                IsBusy = true;
                var result = await ControleUsuariosAppService.ValidarEmail(email);
                if (result.IsSuccess)
                {
                    Response<string> saida = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                    if (saida.Succeeded)
                    {
                        await MessageService.Success(saida.Message, "Tudo Certo!");
                        IsBusy = false;
                        return email;
                    }
                    else
                    {
                        await ErrorModel(saida);
                    }
                }
            }
            IsBusy = false;
            return string.Empty;
        }
        public async Task<bool> ValidarMassivo(List<SOLICITAR_USUARIO_MODEL> usuarios)
        {
            if (usuarios.GroupBy(x => x.MATRICULA).Any(group => group.Count() > 1))
            {
                await MessageService.Warning($"foi encontrado matrículas repetidas em seu arquivo por favor corrija e tente novamente", "Revise os dados!");
                return false;

            }
            else if (usuarios.GroupBy(x => x.EMAIL).Any(group => group.Count() > 1))
            {
                await MessageService.Warning($"foi encontrado e-mails repetidos em seu arquivo por favor corrija e tente novamente", "Revise os dados!");
                return false;
            }

            var result = await ControleUsuariosAppService.ValidateUsuarioMassivo(usuarios);
            if (result.IsSuccess)
            {
                Response<object> saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                if (saida.Succeeded)
                {
                    await MessageService.Success(saida.Message, "Tudo Certo!");
                    return true;
                }
                else
                {
                    var saidaList = JsonConvert.DeserializeObject<Response<List<string>>>(result.Content.ToString());
                    await MessageService.Error($"{saida.Message}; Verifique as seguinte informações em seu arquivo: \n {string.Join("", saidaList.Data)}", "Erro!");
                    return false;
                }
            }
            return false;
        }
        public async Task LoadDataCarteira()
        {
            var result = await PrincipalService.GetDataCarteiraNoRegional();

            if (result.IsSuccess)
            {
                Response<object> saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                if (saida.Succeeded)
                {
                    CARTEIRA = JsonConvert.DeserializeObject<IEnumerable<JORNADA_BD_HIERARQUIum>>(saida.Data.ToString());
                    return;
                }
                else
                {
                    var erro = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                    await ErrorModel(erro);
                    return;
                }
            }
            return;
        }
        public List<SOLICITAR_USUARIO_MODEL> data { get; set; } = [];
        public List<PERFIL_PLATAFORMAS_VIVO_DISSMISS> perfis { get; set; } = [];
        public ControleUsuariosFilterModel filter { get; set; } = new();
        public List<JORNADA_BD_HIERARQUIum> datacarteira { get; set; }
        public List<JORNADA_BD_CARGOS_CANAL> dataCanal_Cargo { get; set; }
        public GenericStatePage<SOLICITAR_USUARIO_MODEL> actual_StatePage { get; set; } = new();
        public IEnumerable<JORNADA_BD_HIERARQUIum> CARTEIRA { get; set; } = new List<JORNADA_BD_HIERARQUIum>();
    }
}











