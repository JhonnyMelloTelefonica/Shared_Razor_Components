using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shared_Razor_Components.Shared
{
    public partial class Authenticator : ComponentBase
    {
        [Parameter] public IHttpContextAccessor httpContextAccessor { get; set; } = default!;
        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
    }
}
