using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared_Razor_Components
{
    internal class LoadJSRuntime
    {
        [Inject]
        public IJSRuntime JsRuntime { get; set; }

        public static void LoadJS(IJSRuntime jsRuntime)
        {
            string import = @"(function() {let scripts = document.getElementsByTagName(""script"");
                let exists = false;
                let index = 0;

                while (index < scripts.length && !exists)
                {
                    exists = scripts[index].src.endsWith('_content/Shared_Razor_Components/Js/VivoFieldSet.js');
                    index++;
                }

                if (!exists)
                {
                    document.body.appendChild(
                        Object.assign(
                            document.createElement('script'), {
                    src: '_content/Shared_Razor_Components/Js/VivoFieldSet.js',
                   type: 'text/javascript'
                            }));
                }
                })()"; 

            jsRuntime.InvokeVoidAsync(import);
        }
    }
}
