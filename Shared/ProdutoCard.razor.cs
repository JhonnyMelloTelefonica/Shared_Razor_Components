using Microsoft.AspNetCore.Components;
using Shared_Static_Class.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Razor_Components.Shared
{
    public partial class ProdutoCard : ComponentBase
    {
        [Parameter] public IEnumerable<PRODUTOS_CARDAPIO> Produtos { get; set; }

    }
}
