using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Linq.Expressions;

namespace Shared_Razor_Components.VivoCustomComponents
{
    public partial class VivoRating : ComponentBase, IDisposable
    {
        [Parameter] public required string LabelText { get; set; }
        [Parameter] public int Value { get; set; }
        [Parameter] public string? Id { get; set; }
        [Parameter] public string? Style { get; set; }
        [Parameter] public bool Disable { get; set; } = false;
        [Parameter] public bool IsGold { get; set; } = false;
        [Inject] public IJSRuntime JSRuntime { get; set; } = default!;
        [Parameter] public Action<int> ActionValueChanged { get; set; } = default!;
        ElementReference[] Icons = new ElementReference[6];

        void ChangeValue(int value)
        {
            if (!Disable)
            {
                Value = value;
                ActionValueChanged.Invoke(Value);
                StateHasChanged();
            }
        }
        public void Dispose()
        {
        }
    }
}