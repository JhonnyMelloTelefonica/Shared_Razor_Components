using Microsoft.AspNetCore.Components;
using Shared_Razor_Components.FundamentalModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Razor_Components.Shared
{
    public partial class NotificationsModal : ComponentBase
    {

        [Parameter]
        public List<Notification> notifications { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        private string BaseUrl
        {
            get
            {
                return "http://brtdtbgs0090sl:8081";
            }
        }

        private async Task GoTo(Notification item)
        {
            item.IsChecked = true;
            NavManager.NavigateTo(BaseUrl + item.link);
        }
    }
}
