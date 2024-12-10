using BlazorDownloadFile;
using Blazorise;
using Blazorise.LoadingIndicator;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Radzen;
using Shared_Razor_Components.Services;
using Shared_Razor_Components.ViewModels;
using Shared_Static_Class.Converters;
using Shared_Static_Class.ErrorModels;
using Shared_Razor_Components.FundamentalModels;
using System.ComponentModel;
using Shared_Razor_Components.ViewModel;
using Microsoft.JSInterop;
using BlazorBootstrap;
using Microsoft.FluentUI.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using Shared_Static_Class.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;

namespace Shared_Razor_Components.ViewModel;

public class AcessosPendentesByIdViewModel : VivoAppsViewModel
{
    public AcessosPendentesByIdViewModel(IHttpContextAccessor httpAccessor, IHostEnvironment networkacessor, GetUser_REDECORP getUser_REDECORP, IPageProgressService pageProgressService, IJSRuntime jSRuntime, PreloadService preloadService, IBlazorDownloadFileService dowloader, Radzen.DialogService radzendialog, Microsoft.FluentUI.AspNetCore.Components.IDialogService fluentDialog , Blazorise.IMessageService messageService, ILoadingIndicatorService applicationLoadingIndicatorService, SweetAlertService swal, NavigationManager navigationManager, UserService userservice, Microsoft.FluentUI.AspNetCore.Components.IToastService _ToastService, IAcessoPendenteByIdService _AcessoPendenteByldService, IAcessoTerceirosService _AcessoTerceirosService, IAnswerFormService _AnswerFormService, ICardapioDigitalService _CardapioDigitalService, IConsultarDemandasService _ConsultarDemandasService, IControleDemandaService _ControleDemandaService, IControleUsuariosAppService _ControleUsuariosAppService, ICreateFormService _CreateFormService, ICreateQuestionService _CreateQuestionService, IDesligamentosService _DesligamentosService, IEditQuestionService _EditQuestionService, IEditSingleQuestionService _EditSingleQuestionService, IEditUserService _EditUserService, IJornadaHierarquiaService _JornadaHierarquiaService, IListaFormService _ListaFormService, IPainelProvasRealizadasService _PainelProvasRealizadasService, IPainelUsuariosService _PainelUsuariosService, IPrincipalService _PrincipalService, IPWService _PWService, IRegisterService _RegisterService, IResultadosProvaService _ResultadosProvaService, IForumRTCZService _ForumRTCZService) : base(httpAccessor, networkacessor, getUser_REDECORP, pageProgressService, jSRuntime, preloadService, dowloader, radzendialog, fluentDialog, messageService, applicationLoadingIndicatorService, swal, navigationManager, userservice, _ToastService, _AcessoPendenteByldService, _AcessoTerceirosService, _AnswerFormService, _CardapioDigitalService, _ConsultarDemandasService, _ControleDemandaService, _ControleUsuariosAppService, _CreateFormService, _CreateQuestionService, _DesligamentosService, _EditQuestionService, _EditSingleQuestionService, _EditUserService, _JornadaHierarquiaService, _ListaFormService, _PainelProvasRealizadasService, _PainelUsuariosService, _PrincipalService, _PWService, _RegisterService, _ResultadosProvaService, _ForumRTCZService)
    {
    }
    public DETALHADO_ACESSO_PENDENTE_MODEL historico { get; set; }
    public async Task LoadData(int idDemanda)
    {
        var result = await AcessoPendenteByIdService.GetAcessoPendenteById(idDemanda);
        if (result.IsSuccess)
        {
            Response<dynamic> saida = JsonConvert.DeserializeObject<Response<dynamic>>(result.Content.ToString());
            if (saida.Succeeded)
            {
                historico = JsonConvert.DeserializeObject<DETALHADO_ACESSO_PENDENTE_MODEL>(saida.Data.ToString());
            }
            else
            {
                var error = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                await ErrorModel(error);
            }
        }

        return;
    }
    public async Task AnswerAcesso(DETALHADO_ACESSO_PENDENTE_MODEL? usuario,
          int matricula,int id,string resposta,
          STATUS_ACESSOS_PENDENTES status,
          TIPO_ACESSOS_PENDENTES tipo)
    {
        IsBusy = true;

        MainResponse result;

        if (tipo.Value == TIPO_ACESSOS_PENDENTES.ALTERACAO.Value)
        {
            if (status.Value == STATUS_ACESSOS_PENDENTES.CONCLUIDO.Value)
            {
                result = await AcessoPendenteByIdService.AnswerAcessoUpdate_usuario(usuario, matricula, id, resposta, status.Value);
            }
            else if (status.Value == STATUS_ACESSOS_PENDENTES.DEVOLVIDO_PARA_SOLICITANTE.Value
                || status.Value == STATUS_ACESSOS_PENDENTES.CANCELADO.Value || status.Value == STATUS_ACESSOS_PENDENTES.REPROVADO.Value)
            {
                result = await AcessoPendenteByIdService.AnswerAcessoChange_Status(matricula, id, resposta, status.Value);
            }
            else if (status.Value == STATUS_ACESSOS_PENDENTES.AGUARDANDO_ANALISTA.Value)
            {
                result = await AcessoPendenteByIdService.AnswerAcessoDevolver_Analista(usuario, matricula, id, resposta, status.Value);
            }
            else
            {
                result = new MainResponse
                { IsSuccess = false };
            }
        }
        else
        {
            if (status.Value == STATUS_ACESSOS_PENDENTES.CONCLUIDO.Value)
            {
                result = await AcessoPendenteByIdService.AnswerAcessoInsert_Novo_Usuario(usuario, matricula, id, resposta, status.Value);
            }
            else if (status.Value == STATUS_ACESSOS_PENDENTES.DEVOLVIDO_PARA_SOLICITANTE.Value
                || status.Value == STATUS_ACESSOS_PENDENTES.CANCELADO.Value || status.Value == STATUS_ACESSOS_PENDENTES.REPROVADO.Value)
            {
                result = await AcessoPendenteByIdService.AnswerAcessoChange_Status(matricula, id, resposta, status.Value);
            }
            else if (status.Value == STATUS_ACESSOS_PENDENTES.AGUARDANDO_ANALISTA.Value)
            {
                result = await AcessoPendenteByIdService.AnswerAcessoDevolver_Analista(usuario, matricula, id, resposta, status.Value);
            }
            else
            {
                result = new MainResponse
                { IsSuccess = false };
            }
        }

        if (result.IsSuccess)
        {
            Response<object> saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
            if (saida.Succeeded)
            {
                historico = JsonConvert.DeserializeObject<DETALHADO_ACESSO_PENDENTE_MODEL>(saida.Data.ToString());
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
}
