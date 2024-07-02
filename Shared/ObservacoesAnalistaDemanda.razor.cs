using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using Shared_Class_Vivo_Apps.Services;
using Shared_Static_Class.Data;
using Shared_Static_Class.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared_Razor_Components.Shared
{
    public partial class ObservacoesAnalistaDemanda : ComponentBase
    {
        [Parameter] public Guid Id { get; set; } = Guid.Empty;
        [Parameter] public int Matricula { get; set; }
        IEnumerable<ObservacoesAnalistaDemandaModel> Model { get; set; } = [];
        [Inject] IConsultarDemandasService _service { get; set; }
        [Inject] UserService User { get; set; }

        protected class ObservacoesAnalistaDemandaModel : DEMANDA_OBSERVACOES_ANALISTAS
        {
            public ObservacoesAnalistaDemandaModel(Guid iD_RELACAO, DateTime dATA, int mAT_ANALISTA, string oBSERVACAO) : base(iD_RELACAO, dATA, mAT_ANALISTA, oBSERVACAO)
            {
                ID_RELACAO = iD_RELACAO;
                DATA = dATA;
                MAT_ANALISTA = mAT_ANALISTA;
                OBSERVACAO = oBSERVACAO;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var saida = await _service.GetAnalistaObservacao(Id, Matricula, true);
                if (saida.IsSuccess)
                {
                    var newobservacao = JsonConvert.DeserializeObject<IEnumerable<DEMANDA_OBSERVACOES_ANALISTAS>>(saida.Content.ToString());

                    Model = newobservacao.Select(x =>
                    {
                        var item = new ObservacoesAnalistaDemandaModel(x.ID_RELACAO, x.DATA, x.MAT_ANALISTA, x.OBSERVACAO);
                        item.ID = x.ID;
                        return item;
                    });

                    StateHasChanged();
                }
            }
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}
