using BlazorDownloadFile;
using Blazorise.LoadingIndicator;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Shared_Static_Class.Data;
using Shared_Static_Class.ErrorModels;
using Shared_Razor_Components.FundamentalModels;
using Shared_Razor_Components.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using BlazorBootstrap;
using Microsoft.JSInterop;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Shared_Razor_Components.ViewModel
{
    public partial class VivoAppsViewModel(IHttpContextAccessor httpAccessor, IHostEnvironment networkacessor, GetUser_REDECORP getUser_REDECORP, Blazorise.IPageProgressService pageProgressService, IJSRuntime jSRuntime, PreloadService preloadService, IBlazorDownloadFileService dowloader, Radzen.DialogService radzendialog, IDialogService fluentDialog, Blazorise.IMessageService messageService, ILoadingIndicatorService applicationLoadingIndicatorService, SweetAlertService swal, NavigationManager navigationManager, UserService userservice, Microsoft.FluentUI.AspNetCore.Components.IToastService _ToastService, IAcessoPendenteByIdService _AcessoPendenteByldService, IAcessoTerceirosService _AcessoTerceirosService, IAnswerFormService _AnswerFormService, ICardapioDigitalService _CardapioDigitalService, IConsultarDemandasService _ConsultarDemandasService, IControleDemandaService _ControleDemandaService, IControleUsuariosAppService _ControleUsuariosAppService, ICreateFormService _CreateFormService, ICreateQuestionService _CreateQuestionService, IDesligamentosService _DesligamentosService, IEditQuestionService _EditQuestionService, IEditSingleQuestionService _EditSingleQuestionService, IEditUserService _EditUserService, IJornadaHierarquiaService _JornadaHierarquiaService, IListaFormService _ListaFormService, IPainelProvasRealizadasService _PainelProvasRealizadasService, IPainelUsuariosService _PainelUsuariosService, IPrincipalService _PrincipalService, IPWService _PWService, IRegisterService _RegisterService, IResultadosProvaService _ResultadosProvaService, IForumRTCZService _ForumRTCZService) : INotifyPropertyChanged
    {
        public IHttpContextAccessor HttpAccessor { get; set; } = httpAccessor;
        public IHostEnvironment Networkacessor { get; set; } = networkacessor;
        public GetUser_REDECORP GetUser_REDECORP { get; set; } = getUser_REDECORP;
        public Blazorise.IPageProgressService PageProgressService { get; set; } = pageProgressService;
        public IJSRuntime JSRuntime { get; set; } = jSRuntime;
        public PreloadService PreloadService { get; set; } = preloadService;
        public Blazorise.IMessageService MessageService { get; set; } = messageService;
        public ILoadingIndicatorService ApplicationLoadingIndicatorService { get; set; } = applicationLoadingIndicatorService;
        public IBlazorDownloadFileService Dowloader { get; set; } = dowloader;
        public SweetAlertService Swal { get; set; } = swal;
        public Radzen.DialogService RadzenDialog { get; set; } = radzendialog;
        public IDialogService FluentDialog { get; set; } = fluentDialog;
        public NavigationManager NavigationManager { get; set; } = navigationManager;
        public UserService Userservice { get; set; } = userservice;
        public IToastService ToastService { get; set; } = _ToastService;
        public IAcessoPendenteByIdService AcessoPendenteByIdService { get; set; } = _AcessoPendenteByldService;
        public IAcessoTerceirosService AcessoTerceirosService { get; set; } = _AcessoTerceirosService;
        public IAnswerFormService AnswerFormService { get; set; } = _AnswerFormService;
        public ICardapioDigitalService CardapioDigitalService { get; set; } = _CardapioDigitalService;
        public IConsultarDemandasService ConsultarDemandasService { get; set; } = _ConsultarDemandasService;
        public IControleDemandaService ControleDemandaService { get; set; } = _ControleDemandaService;
        public IControleUsuariosAppService ControleUsuariosAppService { get; set; } = _ControleUsuariosAppService;
        public ICreateFormService CreateFormService { get; set; } = _CreateFormService;
        public ICreateQuestionService CreateQuestionService { get; set; } = _CreateQuestionService;
        public IDesligamentosService DesligamentosService { get; set; } = _DesligamentosService;
        public IEditQuestionService EditQuestionService { get; set; } = _EditQuestionService;
        public IEditSingleQuestionService EditSingleQuestionService { get; set; } = _EditSingleQuestionService;
        public IEditUserService EditUserService { get; set; } = _EditUserService;
        public IJornadaHierarquiaService JornadaHierarquiaService { get; set; } = _JornadaHierarquiaService;
        public IListaFormService ListaFormService { get; set; } = _ListaFormService;
        public IPainelProvasRealizadasService PainelProvasRealizadasService { get; set; } = _PainelProvasRealizadasService;
        public IPainelUsuariosService PainelUsuariosService { get; set; } = _PainelUsuariosService;
        public IPrincipalService PrincipalService { get; set; } = _PrincipalService;
        public IPWService PWService { get; set; } = _PWService;
        public IQualidadeService QualidadeService { get; set; }
        public IRegisterService RegisterService { get; set; } = _RegisterService;
        public IResultadosProvaService ResultadosProvaService { get; set; } = _ResultadosProvaService;
        public IForumRTCZService ForumRTCZService { get; set; } = _ForumRTCZService;

        public bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                if (isBusy == value) return;
                if (value == true)
                {
                    ApplicationLoadingIndicatorService.Show();
                }
                else
                {
                    ApplicationLoadingIndicatorService.Hide();
                }
                isBusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsBusy)));
            }
        }

        public bool isFilterBusy;

        public bool IsFilterBusy
        {
            get => isFilterBusy;
            set
            {
                if (isFilterBusy == value) return;
                isFilterBusy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFilterBusy)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void TriggerPropertyChangedByTargetVM(object? args, string targetProperty)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(targetProperty));
        }

        public async Task ErrorModel(Response<string> data)
        {
            if (data != null)
            {
                await FluentDialog.ShowErrorAsync(data.Message, "Erro!");
                if (JSRuntime != null && data.Errors != null && data.Errors.Any())
                {
                    await JSRuntime.InvokeVoidAsync("console.log", string.Join(';', data.Errors));
                }
            }
        }

    }
}
