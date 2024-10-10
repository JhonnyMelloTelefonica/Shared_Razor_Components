using Blazorise;
using Blazorise.LoadingIndicator;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using static Blazorise.Licensing.Constants;

namespace Shared_Razor_Components.Shared
{
    public partial class FiltersCount : ComponentBase, IDisposable
    {
        public TextInfo textInfo = new CultureInfo("pt-br", false).TextInfo;

        //: ComponentBase, IDisposable
        [Inject] IJSRuntime JSRuntime { get; set; }
        [Inject] public IModalService ModalService { get; set; }

        [Parameter] public object FilterData { get; set; }
        [Parameter] public RenderFragment ChildContent { get; set; }

        //-----------------------

        public Modal modalRef;
        public bool opened = false;
        public bool Opened
        {
            get => opened;
            set
            {
                if (opened == value) return;

                if (value == true)
                    ShowRenderFragment();

                opened = value;

            }
        }
        private List<Tuple<Object, PropertyInfo>> Objects { get; set; } = new();
        public int Count
        {
            get
            {
                int Count = 0;
                if (FilterData is not null)
                {
                    foreach (PropertyInfo p in FilterData.GetType().GetProperties().Where(p => !p.GetGetMethod().GetParameters().Any()))
                    {
                        Object? pt = p.GetValue(FilterData, null);

                        if (pt != null)
                        {
                            Objects.Add(new Tuple<object, PropertyInfo>(pt, p));

                            if (pt is Enum Enum)
                            {
                                if (Enum.CompareTo(0) == 0)
                                    Count++;
                            }
                            else if (pt is ICollection<object> Collection)
                            {
                                if (Collection.Any())
                                    Count++;
                            }
                            else if (pt is IReadOnlyList<object> ReadOnly)
                            {
                                if (ReadOnly.Any())
                                    Count++;
                            }
                            else if (pt is IEnumerable<object> enumerable)
                            {
                                if (enumerable.Any())
                                    Count++;
                            }
                            else if (pt is int intValue)
                            {
                                if (intValue > 0)
                                    Count++;
                            }
                            else if (pt is string stringValue)
                            {
                                if (!string.IsNullOrEmpty(stringValue))
                                    Count++;
                            }
                            else
                            {
                                Count++;
                            }
                        }
                    }
                }
                return Count;
            }
        }

        public event Action OnChange;
        public event Action FilterOpened;

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }
        public Task ShowRenderFragment()
        {
            //return ModalService.Show("Filtros", ChildContent, new ModalInstanceOptions
            //{
            //    Animated = true,
            //    AnimationDuration = 1,
            //    ScrollToTop = true,
            //    Scrollable = true,
            //});

            return modalRef.Show();
        }

        public void Update()
        {
            OnChange?.Invoke();
            this.StateHasChanged();
        }

        public void Dispose()
        {
            OnChange -= Update;
        }
    }
}

