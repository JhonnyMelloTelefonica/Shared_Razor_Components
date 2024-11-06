using BlazorDownloadFile;
using Blazorise.LoadingIndicator;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Shared_Razor_Components.Services;
using Shared_Razor_Components.ViewModels;
using Shared_Razor_Components.Layout;
using Shared_Static_Class.Converters;
using Shared_Static_Class.ErrorModels;
using Shared_Razor_Components.FundamentalModels;
using System.ComponentModel;

namespace Shared_Razor_Components.Shared
{
    public partial class Detalhado_Acesso_Pendente : ComponentBase, IDisposable
    {
        [Parameter]
        public int id { get; set; }

        SetHeader setHeader;
        private bool FooterOpen { get; set; } = false;
        private SOLICITAR_USUARIO_MODEL user = new();
        private List<Tuple<string, object, object>> propertiesdifferencies { get; set; } = [];
        private ControleUserNoValidationBody FormValidation { get; set; }
        [Inject] UserService UserService { get; set; }
        [Inject] public Blazorise.IMessageService MessageService { get; set; }
        [Inject] private ILoadingIndicatorService ApplicationLoadingIndicatorService { get; set; }
        [Inject] public SweetAlertService Swal { get; set; }
        [Inject] public UserService User { get; set; }
        [Inject] public ControleUsuariosAppViewModel service { get; set; }

        public DETALHADO_ACESSO_PENDENTE_MODEL historico { get; set; }

        public async Task LoadData(int idDemanda)
        {
            var result = await service.AcessoPendenteByIdService.GetAcessoPendenteById(idDemanda);
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
        public async Task AnswerAcesso(
              DETALHADO_ACESSO_PENDENTE_MODEL? usuario,
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
                    result = await service.AcessoPendenteByIdService.AnswerAcessoUpdate_usuario(usuario, matricula, id, resposta, status.Value);
                }
                else if (status.Value == STATUS_ACESSOS_PENDENTES.DEVOLVIDO_PARA_SOLICITANTE.Value
                    || status.Value == STATUS_ACESSOS_PENDENTES.CANCELADO.Value || status.Value == STATUS_ACESSOS_PENDENTES.REPROVADO.Value)
                {
                    result = await service.AcessoPendenteByIdService.AnswerAcessoChange_Status(matricula, id, resposta, status.Value);
                }
                else if (status.Value == STATUS_ACESSOS_PENDENTES.AGUARDANDO_ANALISTA.Value)
                {
                    result = await service.AcessoPendenteByIdService.AnswerAcessoDevolver_Analista(usuario, matricula, id, resposta, status.Value);
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
                    result = await service.AcessoPendenteByIdService.AnswerAcessoInsert_Novo_Usuario(usuario, matricula, id, resposta, status.Value);
                }
                else if (status.Value == STATUS_ACESSOS_PENDENTES.DEVOLVIDO_PARA_SOLICITANTE.Value
                    || status.Value == STATUS_ACESSOS_PENDENTES.CANCELADO.Value || status.Value == STATUS_ACESSOS_PENDENTES.REPROVADO.Value)
                {
                    result = await service.AcessoPendenteByIdService.AnswerAcessoChange_Status(matricula, id, resposta, status.Value);
                }
                else if (status.Value == STATUS_ACESSOS_PENDENTES.AGUARDANDO_ANALISTA.Value)
                {
                    result = await service.AcessoPendenteByIdService.AnswerAcessoDevolver_Analista(usuario, matricula, id, resposta, status.Value);
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

        public async Task ErrorModel(Response<string> data)
        {
            await MessageService.Error(data.Message, "Erro!");
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

                StateHasChanged();
            }
        }

        protected override async Task OnInitializedAsync()
        {
            //TableRow = _PainelUsuariosViewModel.data.Where(x => x.ID == id).FirstOrDefault();
            await base.OnInitializedAsync();
        }

        protected override async Task OnAfterRenderAsync(bool firstrender)
        {
            if (firstrender)
            {
                IsBusy = true;
                await LoadData(id);

                if (historico is not null && historico.ACESSOS_MOBILE is not null)
                {
                    propertiesdifferencies = GetDifferentPropertyValues(
                    new
                    {
                        EMAIL = historico.ACESSOS_MOBILE.EMAIL,
                        MATRICULA = historico.ACESSOS_MOBILE.MATRICULA,
                        REGIONAL = historico.ACESSOS_MOBILE.REGIONAL,
                        CARGO = ((Cargos)historico.ACESSOS_MOBILE.CARGO).GetDisplayName(),
                        CANAL = ((Canal)historico.ACESSOS_MOBILE.CANAL).GetDisplayName(),
                        PDV = historico.ACESSOS_MOBILE.PDV,
                        CPF = historico.ACESSOS_MOBILE.CPF,
                        NOME = historico.ACESSOS_MOBILE.NOME,
                        UF = historico.ACESSOS_MOBILE.UF,
                        STATUS = historico.ACESSOS_MOBILE.STATUS.HasValue
                        ? (historico.ACESSOS_MOBILE.STATUS.Value ? "ATIVO" : "INATIVO") : null,
                        FIXA = historico.ACESSOS_MOBILE.FIXA.Value ? "SIM" : "NÃO",
                        UserAvatar = historico.ACESSOS_MOBILE.UserAvatar,
                        DDD = historico.ACESSOS_MOBILE.DDD
                    }, new
                    {
                        EMAIL = historico.SOLICITACAO.DADOS_SOLICITACAO.EMAIL,
                        MATRICULA = historico.SOLICITACAO.DADOS_SOLICITACAO.MATRICULA,
                        REGIONAL = historico.SOLICITACAO.DADOS_SOLICITACAO.REGIONAL,
                        CARGO = ((Cargos)historico.SOLICITACAO.DADOS_SOLICITACAO.CARGO).GetDisplayName(),
                        CANAL = ((Canal)historico.SOLICITACAO.DADOS_SOLICITACAO.CANAL).GetDisplayName(),
                        PDV = historico.SOLICITACAO.DADOS_SOLICITACAO.PDV,
                        CPF = historico.SOLICITACAO.DADOS_SOLICITACAO.CPF,
                        NOME = historico.SOLICITACAO.DADOS_SOLICITACAO.NOME,
                        UF = historico.SOLICITACAO.DADOS_SOLICITACAO.UF,
                        STATUS = historico.SOLICITACAO.DADOS_SOLICITACAO.STATUS.HasValue
                        ? (historico.SOLICITACAO.DADOS_SOLICITACAO.STATUS.Value ? "ATIVO" : "INATIVO") : null,
                        FIXA = historico.SOLICITACAO.DADOS_SOLICITACAO.FIXA ? "SIM" : "NÃO",
                        UserAvatar = historico.SOLICITACAO.DADOS_SOLICITACAO.UserAvatar,
                        DDD = historico.SOLICITACAO.DADOS_SOLICITACAO.DDD
                    }
                    );
                }

                IsBusy = false;
                StateHasChanged();
                FormValidation.user.Perfil = historico.SOLICITACAO.PERFIS_SOLICITADOS.Select(x => x.ID_PERFIL).ToList();
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
            return await Swal.FireAsync(new SweetAlertOptions
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
                var perfis = service.perfis.Where(x => user.Perfil.Contains(x.ID_PERFIL)).ToList();
                historico.SOLICITACAO.PERFIS_SOLICITADOS = perfis;

                await AnswerAcesso(
                    historico,
                    User.User.MATRICULA,
                    historico.SOLICITACAO.DADOS_SOLICITACAO.ID,
                    saida.Value,
                    status,
                    historico.SOLICITACAO.TIPO == TIPO_ACESSOS_PENDENTES.ALTERACAO.Value
                    ? TIPO_ACESSOS_PENDENTES.ALTERACAO : TIPO_ACESSOS_PENDENTES.INCLUSAO);

                FormValidation.user.Perfil = historico.SOLICITACAO.PERFIS_SOLICITADOS.Select(x => x.ID_PERFIL).ToList();

                StateHasChanged();
            }

        }

        public void Dispose()
        {
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

}
