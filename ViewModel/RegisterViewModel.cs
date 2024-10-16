using Blazorise;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Shared_Static_Class.Data;
using Shared_Razor_Components.FundamentalModels;
using Shared_Razor_Components.Services;
using BlazorBootstrap;
using static System.Runtime.InteropServices.JavaScript.JSType;
using CurrieTechnologies.Razor.SweetAlert2;
using Shared_Razor_Components.ViewModels;
using Shared_Razor_Components.ViewModel;
using BlazorDownloadFile;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Shared_Razor_Components.ViewModels
{
    public class RegisterViewModel : VivoAppsViewModel
    {
        public RegisterViewModel(IHttpContextAccessor httpAccessor, IHostEnvironment networkacessor, GetUser_REDECORP getUser_REDECORP, IPageProgressService pageProgressService, IJSRuntime jSRuntime, PreloadService preloadService, IBlazorDownloadFileService dowloader, Radzen.DialogService radzendialog, Microsoft.FluentUI.AspNetCore.Components.IDialogService fluentDialog , Blazorise.IMessageService messageService, ILoadingIndicatorService applicationLoadingIndicatorService, SweetAlertService swal, NavigationManager navigationManager, UserService userservice, Microsoft.FluentUI.AspNetCore.Components.IToastService _ToastService, IAcessoPendenteByIdService _AcessoPendenteByldService, IAcessoTerceirosService _AcessoTerceirosService, IAnswerFormService _AnswerFormService, ICardapioDigitalService _CardapioDigitalService, IConsultarDemandasService _ConsultarDemandasService, IControleDemandaService _ControleDemandaService, IControleUsuariosAppService _ControleUsuariosAppService, ICreateFormService _CreateFormService, ICreateQuestionService _CreateQuestionService, IDesligamentosService _DesligamentosService, IEditQuestionService _EditQuestionService, IEditSingleQuestionService _EditSingleQuestionService, IEditUserService _EditUserService, IJornadaHierarquiaService _JornadaHierarquiaService, IListaFormService _ListaFormService, IPainelProvasRealizadasService _PainelProvasRealizadasService, IPainelUsuariosService _PainelUsuariosService, IPrincipalService _PrincipalService, IPWService _PWService, IRegisterService _RegisterService, IResultadosProvaService _ResultadosProvaService) : base(httpAccessor, networkacessor, getUser_REDECORP, pageProgressService, jSRuntime, preloadService, dowloader, radzendialog, fluentDialog, messageService, applicationLoadingIndicatorService, swal, navigationManager, userservice, _ToastService, _AcessoPendenteByldService, _AcessoTerceirosService, _AnswerFormService, _CardapioDigitalService, _ConsultarDemandasService, _ControleDemandaService, _ControleUsuariosAppService, _CreateFormService, _CreateQuestionService, _DesligamentosService, _EditQuestionService, _EditSingleQuestionService, _EditUserService, _JornadaHierarquiaService, _ListaFormService, _PainelProvasRealizadasService, _PainelUsuariosService, _PrincipalService, _PWService, _RegisterService, _ResultadosProvaService)
        {
            user = new SOLICITAR_USUARIO_MODEL
            {
                MATRICULA = GetUser_REDECORP.GetMatricula()
            };
        }

        public SOLICITAR_USUARIO_MODEL user { get; set; } = new();
        public ACESSOS_MOBILE_PENDENTE? userSolicitado { get; set; } = new();
        public bool AlreadySolicitated { get; set; } = false;
        public byte[] AvatarImage { get; set; }
        public async Task VerifyCurrentUserExists()
        {
            IsBusy = true;
            try
            {
                var result = await RegisterService.VerifyCurrentUserExists(GetUser_REDECORP.GetMatricula());
                if (result.IsSuccess)
                {
                    Response<object> saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                    if (saida.Succeeded)
                    {
                        var saidaSolicitado = JsonConvert.DeserializeObject<ACESSOS_MOBILE_PENDENTE>(saida.Data.ToString());
                        if (saidaSolicitado is not null)
                        {
                            if (saidaSolicitado.ID != 0)
                            {
                                userSolicitado = saidaSolicitado;
                                AlreadySolicitated = true;
                                await MessageService.Warning(saida.Message, "Solicitação em andamento");
                            }
                        }
                    }
                    else
                    {
                        Response<string> Resultado = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                        await ErrorModel(Resultado);
                    }
                }
            }
            catch
            {
                await MessageService.Error("Ocorreu algum erro ao processar sua solicitação", "Erro!");
            }

            IsBusy = false;

            return;
        }
    }
}
