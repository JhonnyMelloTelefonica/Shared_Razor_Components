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
    public partial class VivoAppsViewModel : INotifyPropertyChanged
    {
        public IHttpContextAccessor HttpAccessor { get; set; }
        public IHostEnvironment Networkacessor { get; set; }
        public GetUser_REDECORP GetUser_REDECORP { get; set; }
        public Blazorise.IPageProgressService PageProgressService { get; set; }
        public IJSRuntime JSRuntime { get; set; }
        public PreloadService PreloadService { get; set; }
        public Blazorise.IMessageService MessageService { get; set; }
        public ILoadingIndicatorService ApplicationLoadingIndicatorService { get; set; }
        public IBlazorDownloadFileService Dowloader { get; set; }
        public SweetAlertService Swal { get; set; }
        public Radzen.DialogService RadzenDialog { get; set; }
        public IDialogService FluentDialog { get; set; }
        public NavigationManager NavigationManager { get; set; }
        public UserService Userservice { get; set; }
        public IToastService ToastService { get; set; }
        public IAcessoPendenteByIdService AcessoPendenteByIdService { get; set; }
        public IAcessoTerceirosService AcessoTerceirosService { get; set; }
        public IAnswerFormService AnswerFormService { get; set; }
        public ICardapioDigitalService CardapioDigitalService { get; set; }
        public IConsultarDemandasService ConsultarDemandasService { get; set; }
        public IControleDemandaService ControleDemandaService { get; set; }
        public IControleUsuariosAppService ControleUsuariosAppService { get; set; }
        public ICreateFormService CreateFormService { get; set; }
        public ICreateQuestionService CreateQuestionService { get; set; }
        public IDesligamentosService DesligamentosService { get; set; }
        public IEditQuestionService EditQuestionService { get; set; }
        public IEditSingleQuestionService EditSingleQuestionService { get; set; }
        public IEditUserService EditUserService { get; set; }
        public IJornadaHierarquiaService JornadaHierarquiaService { get; set; }
        public IListaFormService ListaFormService { get; set; }
        public IPainelProvasRealizadasService PainelProvasRealizadasService { get; set; }
        public IPainelUsuariosService PainelUsuariosService { get; set; }
        public IPrincipalService PrincipalService { get; set; }
        public IPWService PWService { get; set; }
        public IQualidadeService QualidadeService { get; set; }
        public IRegisterService RegisterService { get; set; }
        public IResultadosProvaService ResultadosProvaService { get; set; }

        public VivoAppsViewModel(IHttpContextAccessor httpAccessor, IHostEnvironment networkacessor, GetUser_REDECORP getUser_REDECORP, Blazorise.IPageProgressService pageProgressService, IJSRuntime jSRuntime, PreloadService preloadService, IBlazorDownloadFileService dowloader, Radzen.DialogService radzendialog, IDialogService fluentDialog, Blazorise.IMessageService messageService, ILoadingIndicatorService applicationLoadingIndicatorService, SweetAlertService swal, NavigationManager navigationManager, UserService userservice, Microsoft.FluentUI.AspNetCore.Components.IToastService _ToastService, IAcessoPendenteByIdService _AcessoPendenteByldService, IAcessoTerceirosService _AcessoTerceirosService, IAnswerFormService _AnswerFormService, ICardapioDigitalService _CardapioDigitalService, IConsultarDemandasService _ConsultarDemandasService, IControleDemandaService _ControleDemandaService, IControleUsuariosAppService _ControleUsuariosAppService, ICreateFormService _CreateFormService, ICreateQuestionService _CreateQuestionService, IDesligamentosService _DesligamentosService, IEditQuestionService _EditQuestionService, IEditSingleQuestionService _EditSingleQuestionService, IEditUserService _EditUserService, IJornadaHierarquiaService _JornadaHierarquiaService, IListaFormService _ListaFormService, IPainelProvasRealizadasService _PainelProvasRealizadasService, IPainelUsuariosService _PainelUsuariosService, IPrincipalService _PrincipalService, IPWService _PWService, IRegisterService _RegisterService, IResultadosProvaService _ResultadosProvaService)
        {
            HttpAccessor = httpAccessor;
            Networkacessor = networkacessor;
            GetUser_REDECORP = getUser_REDECORP;
            PageProgressService = pageProgressService;
            JSRuntime = jSRuntime;
            PreloadService = preloadService;
            RadzenDialog = radzendialog;
            FluentDialog = fluentDialog;
            Dowloader = dowloader;
            MessageService = messageService;
            Swal = swal;
            ApplicationLoadingIndicatorService = applicationLoadingIndicatorService;
            NavigationManager = navigationManager;
            Userservice = userservice;
            ToastService = _ToastService;
            AcessoPendenteByIdService = _AcessoPendenteByldService;
            AnswerFormService = _AnswerFormService;
            CardapioDigitalService = _CardapioDigitalService;
            ConsultarDemandasService = _ConsultarDemandasService;
            ControleDemandaService = _ControleDemandaService;
            ControleUsuariosAppService = _ControleUsuariosAppService;
            CreateFormService = _CreateFormService;
            CreateQuestionService = _CreateQuestionService;
            DesligamentosService = _DesligamentosService;
            EditQuestionService = _EditQuestionService;
            EditSingleQuestionService = _EditSingleQuestionService;
            EditUserService = _EditUserService;
            JornadaHierarquiaService = _JornadaHierarquiaService;
            ListaFormService = _ListaFormService;
            PainelProvasRealizadasService = _PainelProvasRealizadasService;
            PainelUsuariosService = _PainelUsuariosService;
            PrincipalService = _PrincipalService;
            PWService = _PWService;
            RegisterService = _RegisterService;
            ResultadosProvaService = _ResultadosProvaService;
            AcessoTerceirosService = _AcessoTerceirosService;
        }

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
            await FluentDialog.ShowErrorAsync(data.Message, "Erro!");

            if (JSRuntime != null && data.Errors != null && data.Errors.Any())
            {
                await JSRuntime.InvokeVoidAsync("console.log", string.Join(';', data.Errors));
            }
        }

    }
}
