using Blazorise.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Shared_Razor_Components.VivoCustomComponents
{
    public partial class VivoFieldSet
    {
        [Parameter]
        public RenderFragment Body { get; set; }
        [Parameter]
        public RenderFragment? Header { get; set; } = null;
        [Parameter]
        public RenderFragment? ClosedBody { get; set; } = null;

        [Parameter]
        public bool IsOpened { get; set; } = false;
        public bool? FormValidated { get; set; } = null;

        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            bool has = HasInput();

            return base.OnAfterRenderAsync(firstRender);
        }

        private bool HasInput()
        {
            var hasInput = false;

            // Create a RenderTreeBuilder
            var builder = new RenderTreeBuilder();

            // Build the render tree for the Body
            Body(builder);

            // Traverse the render tree
            var frames = builder.GetFrames();
            
            foreach (var frame in frames.Array)
            {
                if (frame.FrameType == RenderTreeFrameType.Element)
                {
                    var tagName = frame.ElementName.ToLowerInvariant();
                    if (tagName == "input" || tagName == "select" || tagName == "textarea")
                    {
                        hasInput = true;
                        break;
                    }
                }
            }

            return hasInput;
        }
    }
}
