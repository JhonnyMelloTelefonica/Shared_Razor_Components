using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Shared_Razor_Components.FundamentalModels;
using Shared_Razor_Components.ViewModels;
using Shared_Static_Class.Converters;
using Shared_Static_Class.Data;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Runtime.CompilerServices;
using static Shared_Razor_Components.Shared.CargoByPerfil;
using static Shared_Static_Class.Helpers.ValidationsHelpers;
using Shared_Static_Class.Helpers;
using Blazorise;
using Microsoft.AspNetCore.Components.Web;
using System.Globalization;

namespace Shared_Razor_Components.Layout;

public partial class CriarUsuarioTemplate : ComponentBase, IDisposable
{
    [Parameter] public SOLICITAR_USUARIO_MODEL user { get; set; }
    [Parameter] public bool MatriculaDisabled { get; set; } = false;
    [Parameter] public bool ToMyself { get; set; } = false;
    [Parameter] public EditContext editContext { get; set; }
    [Parameter] public Action Submit { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }
    [Inject] ControleUsuariosAppViewModel service { get; set; }
    IJSObjectReference _reference;

    FluentWizard MyWizard = default!;
    TextInfo textinfo { get; set; } = new CultureInfo("pt-br", false).TextInfo;
    int Value { get; set; } = 0;
    private GiroVUsuario girovusuario { get; set; } = new();
    private VivoTaskUsuario vivotaskusuario { get; set; } = new();
    private PerfisUsuario perfisusuario { get; set; } = new();
    private DadosPDV dadospdv { get; set; } = new();
    private InfoPessoal infopessoal { get; set; } = new();
    private FormPrincipal formprincipal { get; set; } = new();
    public bool ShouldRender { get; set; } = false;
    public DropContainer<DropItem> drop { get; set; }
    public DropZone<DropItem> dro2p { get; set; }
    private FieldIdentifier Cssclass { get; set; }
    private FieldIdentifier CpfCssclass { get; set; }
    private FieldIdentifier TelefoneCssclass { get; set; }

    WizardStepSequence StepSequence = WizardStepSequence.Visited;
    private void OnStateChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.PropertyName))
        {
            editContext.NotifyFieldChanged(editContext.Field(e.PropertyName));
        }

        StateHasChanged();
    }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {

            if (!service.CARTEIRA.Any())
            {
                await service.LoadDataCarteira();
            }
            await service.LoadCargos();
            user.PropertyChanged += OnStateChanged;
            ShouldRender = true;
            Cssclass = editContext.Field("Perfil");
            CpfCssclass = editContext.Field("CPF");
            TelefoneCssclass = editContext.Field("TELEFONE");


            if (MatriculaDisabled)
            {
                if (user.MATRICULA == 0)
                {
                    user.MATRICULA = service.GetUser_REDECORP.GetMatricula();
                    formprincipal.MATRICULA = user.MATRICULA;
                }
                _reference = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Shared_Razor_Components/Layout/ControleUserNoValidationBody.razor.js");
                await _reference.InvokeVoidAsync("disableMatriculaInput");
            }
            StateHasChanged();
        }

        base.OnAfterRender(firstRender);
    }

    public async void ValidarMatricula(ChangeEventArgs args)
    {
        //if (!ToMyself)
        var email = await service.ValidarMatricula(args);
        formprincipal.MATRICULA = email;

        StateHasChanged();
    }
    public async void ValidarEmail(ChangeEventArgs args)
    {
        //if (!ToMyself)
        var email = await service.ValidarEmail(args);
        formprincipal.EMAIL = email;
        StateHasChanged();
    }
    private void ChangePerfil(object? sender, int index)
    {
        if (sender is not null)
        {
            var saida = sender as PERFIL_PLATAFORMAS_VIVO;
            if (saida is not null)
            {
                perfisusuario.Perfil[index] = saida.ID_PERFIL != 0 ? saida.ID_PERFIL : 0;
                editContext.NotifyFieldChanged(Cssclass);
            }
        }
    }
    private void SetCargo(object? sender)
    {
        if (sender is not null)
        {
            if (!string.IsNullOrEmpty(sender.ToString()))
            {
                infopessoal.CARGO = int.Parse(sender.ToString());
                if (service.perfis is not null)
                {
                    service.perfis.ForEach(x => { x.IsDismissible = true; });

                    var saida = service.perfis.Where(x => Converters.ConvertStringToStringList(x.CARGO).Contains(infopessoal.CARGO.ToString()));
                    if (saida.Any())
                    {
                        perfisusuario.Perfil = saida.Select(x => x.ID_PERFIL).ToList();
                        service.perfis.Where(x => saida.Select(y => y.ID_PERFIL).Contains(x.ID_PERFIL)).ToList().ForEach(x =>
                        {
                            x.IsDismissible = false;
                        });
                    }
                }
            }
        }
    }
    async Task OnValidSubmit()
    {
        Console.WriteLine($"OnValidSubmit called");
    }

    void OnInvalidSubmit()
    {
        Console.WriteLine($"OnInvalidSubmit called");
    }

    private void OnFinishedAsync()
    {
        user.FIXA = girovusuario.FIXA;
        user.ELEGIVEL = girovusuario.ELEGIVEL;
        user.SENHA = vivotaskusuario.SENHA;
        user.CONFIRMSENHA = vivotaskusuario.CONFIRMSENHA;
        user.Perfil = perfisusuario.Perfil;
        user.REGIONAL = dadospdv.REGIONAL;
        user.PDV = dadospdv.PDV;
        user.UF = dadospdv.UF;
        user.DDD = dadospdv.DDD;
        user.rede = dadospdv.rede;
        user.TELEFONE = infopessoal.TELEFONE;
        user.CARGO = infopessoal.CARGO;
        user.NOME = infopessoal.NOME;
        user.NOME_SOCIAL = infopessoal.NOME_SOCIAL;
        user.CPF = infopessoal.CPF;
        user.MATRICULA = formprincipal.MATRICULA;
        user.EMAIL = formprincipal.EMAIL;

        Submit();
    }

    public async Task GetPerfilByCargo(int args, List<int> Perfis)
    {
        service.IsBusy = true;
        infopessoal.CARGO = args;

        var result = await service.ControleUsuariosAppService.GetPerfilByCargo(args);
        if (result.IsSuccess)
        {
            Response<object> saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
            if (saida.Succeeded)
            {
                var data = JsonConvert.DeserializeObject<List<PERFIL_PLATAFORMAS_VIVO>>(saida.Data.ToString());
                Perfis.Clear();
                Perfis.AddRange(data.Select(x => x.ID_PERFIL).ToList());

                //await MessageService.Success($"Perfis Adicionados: {string.Join(";", data.Select(x => x.Perfil).ToList())}", "Perfis padrão adicionados!");
            }
            else
            {
                var error = JsonConvert.DeserializeObject<Response<string>>(result.Content.ToString());
                await service.ErrorModel(error);
            }
        }

        service.IsBusy = false;
        return;
    }
    public void AddAlternativa()
    {
        perfisusuario.Perfil.Add(0);
        editContext.NotifyFieldChanged(Cssclass);
    }

    public void RemoveAlternativa()
    {
        perfisusuario.Perfil.RemoveAt(user.Perfil.Count - 1);
        editContext.NotifyFieldChanged(Cssclass);
    }

    public void RemoveAlternativaAt(int index)
    {
        perfisusuario.Perfil.RemoveAt(index);
        editContext.NotifyFieldChanged(Cssclass);
    }


    private void SetDDD(object? sender)
    {
        if (sender is not null)
        {
            if (!string.IsNullOrEmpty(sender.ToString()))
            {
                dadospdv.DDD = int.Parse(sender.ToString());
                var saida = service.CARTEIRA.Where(x => x.DDD == dadospdv.DDD).FirstOrDefault();
                if (saida is null)
                {
                    dadospdv.PDV = string.Empty;
                }
                else
                {
                    dadospdv.PDV = saida.ADABAS;
                }
            }
        }
    }
    private void SetPDV(ChangeEventArgs sender)
    {
        if (sender is not null)
        {
            if (!string.IsNullOrEmpty(sender.Value.ToString()))
            {
                dadospdv.PDV = sender.Value.ToString();
                var saida = service.CARTEIRA.Where(x => x.ADABAS == dadospdv.PDV).FirstOrDefault();
                if (saida is null)
                {
                    dadospdv.DDD = 0;
                }
                else
                {
                    dadospdv.DDD = saida.DDD.Value;
                }
            }
        }
    }

    public void Dispose()
    {
        user.PropertyChanged -= OnStateChanged;
    }

    protected class GiroVUsuario
    {
        [Required(ErrorMessage = "Campo fixa é obrigatório")]
        public bool FIXA { get; set; } = true;

        [Required(ErrorMessage = "Campo Elegivel é obrigatório")]
        public bool ELEGIVEL { get; set; } = true;
    }
    protected class VivoTaskUsuario
    {
        public bool _hasVivoTask { get; set; } = false;

        [Required(ErrorMessage = "Campo senha é obrigatório")]
        [MaxLength(14, ErrorMessage = "Número máximo de caracteres excedido, máximo : {1} ")]
        [MinLength(6, ErrorMessage = "Número minímo de caracteres não foi atingido, minímo : {1}")]
        public string SENHA
        {
            get => _hasVivoTask ? _senha : $"VIVO@{DateTime.Now.ToString("yyyy")}";
            set
            {
                if (_senha != value)
                    _senha = value;
            }
        }

        private string _senha = string.Empty;

        [Required(ErrorMessage = "Campo confirmar senha é obrigatório")]
        [Compare(nameof(SENHA), ErrorMessage = "Campos precisam ser iguais")]
        public string CONFIRMSENHA
        {
            get => _hasVivoTask ? _confirmsenha : $"VIVO@{DateTime.Now.ToString("yyyy")}";
            set
            {
                if (_confirmsenha != value)
                    _confirmsenha = value;
            }
        }
        private string _confirmsenha = string.Empty;

    }
    protected class PerfisUsuario
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [ListHasElements<int>(0, ErrorMessage = "Verifique os perfis selecionados!")]
        public List<int> Perfil { get; set; } = [];
    }
    protected class DadosPDV
    {
        [Required(ErrorMessage = "Campo REGIONAL é obrigatório")]
        public string REGIONAL { get; set; }

        [Required(ErrorMessage = "Campo PDV é obrigatório")]
        public string PDV { get; set; }

        [Required(ErrorMessage = "Campo UF é obrigatório")]
        public string UF { get; set; }
        [Required(ErrorMessage = "Campo DDD é obrigatório")]
        public int DDD { get; set; }
        public string rede { get; set; } = string.Empty;

    }
    protected class InfoPessoal
    {

        [MaxLength(15, ErrorMessage = "Qtd. Número max. Telefone: 11")]
        [MinLength(14, ErrorMessage = "Qtd. Número min. Telefone: 10")]
        public string TELEFONE
        {
            get => _telefone;
            set => _telefone = FormatInputs.FormatTelefone(value);
        }
        private string _telefone = string.Empty;


        [Required(ErrorMessage = "Campo cargo é obrigatório")]
        [Range(1, int.MaxValue, ErrorMessage = "Valor de cargo inválido")]
        public int CARGO { get; set; }

        [Required(ErrorMessage = "Campo nome é obrigatório")]
        [StringLength(50, ErrorMessage = "Número máximo de caracteres: {1}")]
        [NomeCompletoValidation(ErrorMessage = "O nome deve conter pelo menos três palavras.")]
        public string NOME { get; set; }
        private string _nome;
        TextInfo textinfo { get; set; } = new CultureInfo("pt-br", false).TextInfo;

        public string NOME_SOCIAL
        {
            get => nome_social;
            set => nome_social = value;
        }

        [Required(ErrorMessage = "Este campo é obrigatório")]
        [StringLength(30, ErrorMessage = "Número máximo de caracteres: {1}")]
        public string nome_social { get; set; } = string.Empty;

        public string[] DisplayName
        {
            get
            {
                if (NOME == null || string.IsNullOrWhiteSpace(NOME.ToString()))
                {
                    return [];
                }

                var palavras = NOME.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                // Conta palavras, ignorando preposições
                var palavrasValidas = palavras.Where(p => !Preposicoes.Contains(p.ToLower())).ToArray();

                if (palavrasValidas.Length < 3)
                {
                    return [];
                    //new ValidationResult("O nome deve conter pelo menos três palavras, ignorando preposições como 'da', 'de', 'do', etc.")
                }

                return palavrasValidas;
            }
        }

        private static readonly string[] Preposicoes = { "da", "de", "do", "das", "dos" };

        [Required(ErrorMessage = "Campo CPF é obrigatório")]
        [MaxLength(14, ErrorMessage = "Preenchimento inválido")]
        [MinLength(14, ErrorMessage = "Preenchimento inválido")]
        public string CPF
        {
            get => _cpf;
            set => _cpf = FormatInputs.FormatCPF(value);
        }
        private string _cpf = string.Empty;

    }
    protected class FormPrincipal
    {
        [Required(ErrorMessage = "Campo obrigatório")]
        [Range(0, 99999999, ErrorMessage = "Matrícula inválida")]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Valor de matrícula inválido")]
        public int MATRICULA { get; set; }
        [Required(ErrorMessage = "Campo E-mail é obrigatório")]
        [EmailAddress(ErrorMessage = "Campo deve ser um e-mail válido")]
        public string EMAIL { get; set; }
    }
    public class DropItem
    {
        public DropItem(string name, string group)
        {
            Name = name;
            Group = group;
        }

        public string Name { get; init; }
        public string Group { get; set; }
    }
}