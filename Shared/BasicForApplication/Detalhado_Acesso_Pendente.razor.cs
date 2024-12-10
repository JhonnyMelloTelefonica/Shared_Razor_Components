using Shared_Razor_Components.Layout;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared_Razor_Components.ViewModel;
using Shared_Razor_Components.Shared;
using Shared_Static_Class.Converters;
using Shared_Razor_Components.FundamentalModels;
using System.ComponentModel;
using Shared_Razor_Components.ViewModels;

namespace Shared_Razor_Components.Shared.BasicForApplication;
public partial class Detalhado_Acesso_Pendente : ComponentBase
{
    SetHeader setHeader;

    [Parameter]
    public int id { get; set; }

    private bool FooterOpen { get; set; } = false;
    private SOLICITAR_USUARIO_MODEL user = new();
    private List<Tuple<string, object, object>> propertiesdifferencies { get; set; } = [];
    private ControleUserNoValidationBody FormValidation { get; set; }

    [Inject] IJSRuntime JSRuntime { get; set; }
    [Inject] AcessosPendentesByIdViewModel service { get; set; }
    [Inject] ControleUsuariosAppViewModel ControleUsuariosService { get; set; }
    [Inject] UserCard UserCard { get; set; }
    [Inject] UserService UserService { get; set; }

    protected override async Task OnInitializedAsync()
    {
        //TableRow = service._PainelUsuariosViewModel.data.Where(x => x.ID == id).FirstOrDefault();
        service.PropertyChanged += OnStateChanged;
        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstrender)
    {
        if (firstrender)
        {
            service.IsBusy = true;
            await service.LoadData(id);

            if (service.historico is not null && service.historico.ACESSOS_MOBILE is not null)
            {
                propertiesdifferencies = GetDifferentPropertyValues(
                new
                {
                    EMAIL = service.historico.ACESSOS_MOBILE.EMAIL,
                    MATRICULA = service.historico.ACESSOS_MOBILE.MATRICULA,
                    REGIONAL = service.historico.ACESSOS_MOBILE.REGIONAL,
                    CARGO = ((Cargos)service.historico.ACESSOS_MOBILE.CARGO).GetDisplayName(),
                    CANAL = ((Canal)service.historico.ACESSOS_MOBILE.CANAL).GetDisplayName(),
                    PDV = service.historico.ACESSOS_MOBILE.PDV,
                    CPF = service.historico.ACESSOS_MOBILE.CPF,
                    NOME = service.historico.ACESSOS_MOBILE.NOME,
                    NOME_SOCIAL = service.historico.ACESSOS_MOBILE.NOME_SOCIAL,
                    UF = service.historico.ACESSOS_MOBILE.UF,
                    STATUS = service.historico.ACESSOS_MOBILE.STATUS.HasValue
                    ? (service.historico.ACESSOS_MOBILE.STATUS.Value ? "ATIVO" : "INATIVO") : null,
                    FIXA = service.historico.ACESSOS_MOBILE.FIXA.Value ? "SIM" : "NÃO",
                    UserAvatar = service.historico.ACESSOS_MOBILE.UserAvatar,
                    DDD = service.historico.ACESSOS_MOBILE.DDD
                }, new
                {
                    EMAIL = service.historico.SOLICITACAO.DADOS_SOLICITACAO.EMAIL,
                    MATRICULA = service.historico.SOLICITACAO.DADOS_SOLICITACAO.MATRICULA,
                    REGIONAL = service.historico.SOLICITACAO.DADOS_SOLICITACAO.REGIONAL,
                    CARGO = ((Cargos)service.historico.SOLICITACAO.DADOS_SOLICITACAO.CARGO).GetDisplayName(),
                    CANAL = ((Canal)service.historico.SOLICITACAO.DADOS_SOLICITACAO.CANAL).GetDisplayName(),
                    PDV = service.historico.SOLICITACAO.DADOS_SOLICITACAO.PDV,
                    CPF = service.historico.SOLICITACAO.DADOS_SOLICITACAO.CPF,
                    NOME = service.historico.SOLICITACAO.DADOS_SOLICITACAO.NOME,
                    NOME_SOCIAL = service.historico.SOLICITACAO.DADOS_SOLICITACAO.NOME_SOCIAL,
                    UF = service.historico.SOLICITACAO.DADOS_SOLICITACAO.UF,
                    STATUS = service.historico.SOLICITACAO.DADOS_SOLICITACAO.STATUS.HasValue
                    ? (service.historico.SOLICITACAO.DADOS_SOLICITACAO.STATUS.Value ? "ATIVO" : "INATIVO") : null,
                    FIXA = service.historico.SOLICITACAO.DADOS_SOLICITACAO.FIXA ? "SIM" : "NÃO",
                    UserAvatar = service.historico.SOLICITACAO.DADOS_SOLICITACAO.UserAvatar,
                    DDD = service.historico.SOLICITACAO.DADOS_SOLICITACAO.DDD
                }
                );
            }

            service.IsBusy = false;
            StateHasChanged();
            FormValidation.user.Perfil = service.historico.SOLICITACAO.PERFIS_SOLICITADOS.Select(x => x.ID_PERFIL).ToList();
        }
    }

    private void OnStateChanged(object? sender, PropertyChangedEventArgs e)
    {
        StateHasChanged();
        setHeader.Update();
    }

    public void MouseFooterEnter()
    {
        FooterOpen = !FooterOpen;
        StateHasChanged();
    }

    public void MouseFooterLeave()
    {
        FooterOpen = !FooterOpen;
        StateHasChanged();
    }
    private async Task<SweetAlertResult> TriggerSwal(string text, string? title = "Quase lá...")
    {
        return await service.Swal.FireAsync(new SweetAlertOptions
        {
            Title = title,
            Text = text,
            Icon = SweetAlertIcon.Warning,
            InputAutoFocus = true,
            ShowCancelButton = false,
            Input = SweetAlertInputType.Textarea,
            InputValidator = new InputValidatorCallback((string input) => input.Length == 0 ? "Por favor informe algum valor" : null, this),
            ValidationMessage = "Por favor informe algum valor",
            ConfirmButtonText = "Finalizar",
            ShowCloseButton = true,
            ShowDenyButton = false,
            AllowEscapeKey = false,
            AllowEnterKey = true,
            ShowLoaderOnConfirm = true,
            AllowOutsideClick = false,
        });
    }

    private async Task AnswerAcesso(string text, STATUS_ACESSOS_PENDENTES status, string? title)
    {
        FormValidation.TriggerValidate();

        var saida = await TriggerSwal(text, title);

        if (!string.IsNullOrEmpty(saida.Value))
        {
            var perfis = ControleUsuariosService.perfis.Where(x => user.Perfil.Contains(x.ID_PERFIL)).ToList();
            service.historico.SOLICITACAO.PERFIS_SOLICITADOS = perfis;

            await service.AnswerAcesso(
                service.historico,
                service.Userservice.User.MATRICULA,
                service.historico.SOLICITACAO.DADOS_SOLICITACAO.ID,
                saida.Value,
                status,
                service.historico.SOLICITACAO.TIPO == TIPO_ACESSOS_PENDENTES.ALTERACAO.Value
                ? TIPO_ACESSOS_PENDENTES.ALTERACAO : TIPO_ACESSOS_PENDENTES.INCLUSAO);

            FormValidation.user.Perfil = service.historico.SOLICITACAO.PERFIS_SOLICITADOS.Select(x => x.ID_PERFIL).ToList();

            StateHasChanged();
        }

    }

    public void Dispose()
    {
        service.PropertyChanged -= OnStateChanged;
        if (setHeader is not null)
            setHeader.Dispose();
    }

    public static List<Tuple<string, object, object>> GetDifferentPropertyValues(object obj1, object obj2)
    {
        var differentValues = new List<Tuple<string, object, object>>();

        // Get the properties of each object
        var obj1Properties = obj1.GetType().GetProperties();
        var obj2Properties = obj2.GetType().GetProperties();

        // Find common property names
        var commonPropertyNames = obj1Properties.Select(p => p.Name)
                                    .Intersect(obj2Properties.Select(p => p.Name)).ToList();

        // Compare property values
        foreach (var propertyName in commonPropertyNames)
        {
            var prop1 = obj1Properties.First(p => p.Name == propertyName);
            var prop2 = obj2Properties.First(p => p.Name == propertyName);

            var value1 = prop1.GetValue(obj1);
            var value2 = prop2.GetValue(obj2);

            if (value1 is not null && value2 is not null && !value1.Equals(value2))
            {
                differentValues.Add(Tuple.Create(propertyName, value1, value2));
            }
        }

        return differentValues;
    }
}
