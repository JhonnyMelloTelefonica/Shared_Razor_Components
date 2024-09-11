using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Shared_Razor_Components.FundamentalModels;
using Shared_Razor_Components.ViewModels;
using Shared_Static_Class.Converters;
using Shared_Static_Class.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Razor_Components.Shared.BasicForApplication
{
    public partial class ControleUserModalBody : ComponentBase, IDisposable
    {
        [Parameter] public SOLICITAR_USUARIO_MODEL user { get; set; }
        [Parameter] public bool MatriculaDisabled { get; set; } = false;
        [Parameter] public EditContext editContext { get; set; }
        [Inject] IJSRuntime JSRuntime {get;set;}
        [Inject] public ControleUsuariosAppViewModel service { get; set; }
        public bool ShouldRender { get; set; } = false;

        private FieldIdentifier Cssclass { get; set; }
        private FieldIdentifier CpfCssclass { get; set; }

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

                user.PropertyChanged += OnStateChanged;
                ShouldRender = true;
                Cssclass = editContext.Field("Perfil");
                CpfCssclass = editContext.Field("CPF");

                StateHasChanged();

                if (MatriculaDisabled)
                {
                    //user.MATRICULA = service.GetUser_REDECORP.GetMatricula();
                    await JSRuntime.InvokeVoidAsync("disableMatriculaInput");
                }
            }

            base.OnAfterRender(firstRender);
        }

        protected override async Task OnParametersSetAsync()
        {
            if (MatriculaDisabled)
                await JSRuntime.InvokeVoidAsync("disableMatriculaInput");
        }

        private void ChangePerfil(object? sender, int index)
        {
            if (sender is not null)
            {
                var saida = sender as PERFIL_PLATAFORMAS_VIVO;
                if (saida is not null)
                {
                    user.Perfil[index] = saida.ID_PERFIL != 0 ? saida.ID_PERFIL : 0;
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
                    user.CARGO = int.Parse(sender.ToString());
                    if (service.perfis is not null)
                    {
                        service.perfis.ForEach(x => { x.IsDismissible = true; });

                        var saida = service.perfis.Where(x => Converters.ConvertStringToStringList(x.CARGO).Contains(user.CARGO.ToString()));
                        if (saida.Any())
                        {
                            user.Perfil = saida.Select(x => x.ID_PERFIL).ToList();
                            service.perfis.Where(x => saida.Select(y => y.ID_PERFIL).Contains(x.ID_PERFIL)).ToList().ForEach(x =>
                            {
                                x.IsDismissible = false;
                            });
                        }
                    }
                }
            }
        }
        private void HandleSubmit()
        {
            // Lógica para processar o formulário quando válido.
        }
        private void HandleValidSubmit()
        {
            // Lógica para processar o formulário quando válido.
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
        public void AddAlternativa()
        {
            user.Perfil.Add(new());
            editContext.NotifyFieldChanged(Cssclass);
        }

        public void RemoveAlternativa()
        {
            user.Perfil.RemoveAt(user.Perfil.Count - 1);
            editContext.NotifyFieldChanged(Cssclass);
        }

        public void RemoveAlternativaAt(int index)
        {
            user.Perfil.RemoveAt(index);
            editContext.NotifyFieldChanged(Cssclass);
        }

        public void FormatCPF(string args)
        {
            user.CPF = FormatInputs.FormatCPF(args);
        }

        public void Dispose()
        {
            user.PropertyChanged -= OnStateChanged;
        }
    }
}
