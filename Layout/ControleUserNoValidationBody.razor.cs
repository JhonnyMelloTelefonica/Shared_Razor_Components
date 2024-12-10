using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Shared_Razor_Components.ViewModels;
using Shared_Razor_Components.FundamentalModels;
using System.ComponentModel;
using Shared_Static_Class.Data;
using Microsoft.AspNetCore.Components.Forms;
using Shared_Static_Class.Converters;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Implementation;

namespace Shared_Razor_Components.Layout
{
    public partial class ControleUserNoValidationBody : ComponentBase, IDisposable
    {
        [Parameter] public SOLICITAR_USUARIO_MODEL user { get; set; }
        [Parameter] public required bool Editable { get; set; } = true;
        [Parameter] public required bool IsTipoEdição { get; set; } = false;
        [Parameter] public required bool IsMatriculaDisabled { get; set; } = false;
        [Parameter] public Action? Submit { get; set; }
        [Inject] ControleUsuariosAppViewModel service { get; set; }
        [Inject] IJSRuntime JSRuntime { get; set; }

        IJSObjectReference _reference;

        public required List<PERFIL_PLATAFORMAS_VIVO> Perfis_Plataforma { get; set; }
        public EditContext editContext { get; set; }
        public bool ShouldRender { get; set; } = false;

        public event Action CheckValidateEvent;
        void HandleInvalidSubmit() => Console.WriteLine("error");
        void HandleValidSubmit()
        {
            Console.WriteLine("valide");
            if (Submit != null)
            {
                Submit();
            }
        }
        public void TriggerValidate()
        {
            CheckValidateEvent.Invoke();
        }
        private void CheckValidate()
        {
            _ = editContext.Validate();
            StateHasChanged();
        }
        private void OnStateChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (editContext is not null && !string.IsNullOrEmpty(e.PropertyName))
            {
                editContext.NotifyFieldChanged(editContext.Field(e.PropertyName));
            }
            StateHasChanged();
        }
        protected override Task OnInitializedAsync()
        {
            user.PropertyChanged += OnStateChanged;
            return base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                CheckValidateEvent += CheckValidate;

                if (editContext is null)
                    editContext = new(user);

                user.SENHA = "VIVO@2024";
                user.CONFIRMSENHA = "VIVO@2024";

                if (service.perfis is null || !service.perfis.Any())
                {
                    await service.GetPerfisUsuario();
                }

                if (!service.CARTEIRA.Any())
                {
                    await service.LoadDataCarteira();
                }

                Perfis_Plataforma = service.perfis.Select(x => new PERFIL_PLATAFORMAS_VIVO(x.ID_PERFIL, x.Perfil, x.obs, x.CARGO)).ToList();
                ShouldRender = true;
                StateHasChanged();

                if (IsMatriculaDisabled)
                {
                    user.MATRICULA = service.GetUser_REDECORP.GetMatricula();
                    _reference = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Shared_Razor_Components/Layout/ControleUserNoValidationBody.razor.js");
                    await _reference.InvokeVoidAsync("disableMatriculaInput");
                    await _reference.InvokeVoidAsync("log2");
                }
            }

            base.OnAfterRender(firstRender);
        }
        private void ChangePerfil(object? sender, int index)
        {
            if (sender is not null)
            {
                var saida = sender as PERFIL_PLATAFORMAS_VIVO;
                if (saida is not null)
                {
                    user.Perfil[index] = saida.ID_PERFIL != 0 ? saida.ID_PERFIL : 0;
                }
            }
        }
        private void SetCargo(object? sender)
        {
            if (sender is not null)
            {
                if (!string.IsNullOrEmpty(sender.ToString()))
                {
                    user.CARGO = int.Parse(sender.ToString());
                    if (Perfis_Plataforma is not null)
                    {
                        var saida = Perfis_Plataforma.Where(x => Converters.ConvertStringToStringList(x.CARGO).Contains(user.CARGO.ToString()));
                        if (saida.Any())
                        {
                            if (IsTipoEdição)
                            {
                                user.Perfil = user.Perfil.Union(saida.Select(x => x.ID_PERFIL)).ToList();
                            }
                            else
                            {
                                user.Perfil = saida.Select(x => x.ID_PERFIL).ToList();
                            }
                        }
                    }
                }
            }
        }
        private void SetDDD(object? sender)
        {
            if (sender is not null)
            {
                if (!string.IsNullOrEmpty(sender.ToString()))
                {
                    user.DDD = int.Parse(sender.ToString());
                    var saida = service.CARTEIRA.Where(x => x.DDD == user.DDD).FirstOrDefault();
                    if (saida is null)
                    {
                        user.PDV = string.Empty;
                    }
                    else
                    {
                        user.PDV = saida.ADABAS;
                    }
                }
            }
        }
        public void AddAlternativa()
        {
            user.Perfil.Add(new());
        }
        public void RemoveAlternativa()
        {
            user.Perfil.RemoveAt(user.Perfil.Count - 1);
        }
        public void RemoveAlternativaAt(int index)
        {
            user.Perfil.RemoveAt(index);
        }
        public void FormatCPF(string args)
        {
            user.CPF = FormatInputs.FormatCPF(args);
        }
        public void FormatTelefone(string args)
        {
            user.TELEFONE = FormatInputs.FormatTelefone(args);
        }
        public async Task GetPerfilByCargo(int args, List<int> Perfis)
        {
            service.IsBusy = true;
            user.CARGO = args;

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
        public void Dispose()
        {
            CheckValidateEvent -= CheckValidate;
        }
    }
}
