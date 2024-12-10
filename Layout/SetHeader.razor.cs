using Microsoft.AspNetCore.Components;

namespace Shared_Razor_Components.Layout;

public partial class SetHeader : ComponentBase, IDisposable
{
    [CascadingParameter] public MainLayout? MainLayout { get; set; }
    [CascadingParameter] public UnauthLayout? UnauthLayout { get; set; }
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public bool ContentWithDefaultHeader { get; set; } = false;

    public event Action OnChange;
    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            if (UnauthLayout is not null)
            {
                UnauthLayout.SetHeader(this);
            }

            if (MainLayout is not null)
            {
                MainLayout.SetHeader(this);
            }
        }
        base.OnAfterRender(firstRender);
    }

    public void Update() //Ao Executar Update ele apenas sinaliza um Trigger e atualiza a página
    {
        OnChange?.Invoke();
        StateHasChanged();
    }

    public void UpdatePage()
    {
        StateHasChanged();
    }

    public void Dispose()
    {
        UnauthLayout?.SetHeader(null);
        MainLayout?.SetHeader(null);
        OnChange -= Update;
    }

}
