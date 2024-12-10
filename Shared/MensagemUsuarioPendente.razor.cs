using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared_Razor_Components.Shared;
using Shared_Razor_Components.FundamentalModels;
using System.Globalization;

namespace Shared_Razor_Components.Shared;
public partial class MensagemUsuarioPendente : ComponentBase
{
    private TextInfo textInfo = new CultureInfo("pt-br", false).TextInfo;
    [Parameter] public MENSAGEM_ACESSO_PENDENTE item { get; set; }
    [Inject] IJSRuntime JSRuntime { get; set; }
    [Inject] UserCard UserCard { get; set; }
    [Inject] UserService User { get; set; }
}
