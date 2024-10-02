using Blazorise.Modules;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Shared_Static_Class.Data;
using Shared_Static_Class.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.ConstrainedExecution;
using static Shared_Static_Class.Converters.OrderByStringProperty;

namespace Shared_Razor_Components.Shared
{
    public partial class ProdutoCard : ComponentBase, IDisposable
    {
        [Parameter] public IEnumerable<PRODUTOS_CARDAPIO> Produtos { get; set; } = [];
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] IJSRuntime JSRuntime { get; set; } = default!;
        IJSObjectReference _jsmodule { get; set; } = default!;
        string Orderby { get; set; } = "Avaliacao.Avaliacao";
        bool AscOrDesc { get; set; } = true;
        IEnumerable<ProdutoCardModel> ProdutosModelToShow
        {
            get
            {
                IEnumerable<PRODUTOS_CARDAPIO> saida = Produtos;

                if (!string.IsNullOrEmpty(Orderby))
                {
                    saida = saida.OrderBy(Orderby, AscOrDesc);
                }

                return saida.Select(x => new ProdutoCardModel(x, PropertyChanged));
            }
        }
        string SearchGeral { get; set; } = string.Empty;
        string SearchCategoria { get; set; } = string.Empty;
        string SearchEspecificação { get; set; } = string.Empty;
        protected FilterPageData filter { get; set; } = new(0, null, null, null, null, false, 0);
        protected override void OnInitialized()
        {
            PropertyChanged += ProdutoCard_PropertyChanged;

            base.OnInitialized();
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //.Where(x => !string.IsNullOrEmpty(SearchCategoria) ? x.GetDisplayName().Contains(SearchCategoria, StringComparison.InvariantCultureIgnoreCase) : x.GetDisplayName() != null)
                //.Where(x => !string.IsNullOrEmpty(SearchEspecificação) ? x.GetDisplayName().Contains(SearchEspecificação, StringComparison.InvariantCultureIgnoreCase) : x.GetDisplayName() != null)

                _jsmodule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Shared_Razor_Components/Shared/ProdutoCard.razor.js");
                await InvokeAsync(StateHasChanged);
            }

            await base.OnAfterRenderAsync(firstRender);
        }
        void ProdutoCard_PropertyChanged() => InvokeAsync(StateHasChanged);

        protected void AddToFilter<T>(bool args, T data, List<T> array)
        {
            if (args)
            {
                array.Add(data);
            }
            else
            {
                if (array.Contains(data))
                {
                    array.Remove(data);
                }
            }
        }

        public void Dispose()
        {
            PropertyChanged -= ProdutoCard_PropertyChanged;
        }

        event Action? PropertyChanged;
        protected class ProdutoCardModel : PRODUTOS_CARDAPIO
        {
            public ProdutoCardModel(PRODUTOS_CARDAPIO item, Action? propertyChanged, bool isOpened = false)
            {
                ID_PRODUTO = item.ID_PRODUTO;
                Nome = item.Nome;
                Descrição = item.Descrição;
                Avaliacao = item.Avaliacao;
                Categoria_Produto = item.Categoria_Produto;
                Fabricante = item.Fabricante;
                Cor = item.Cor;
                IsOferta = item.IsOferta;
                Valor = item.Valor;
                MaxParcelas = item.MaxParcelas;
                MaxParcelasSemJuros = item.MaxParcelasSemJuros;
                DATA_INCLUSÃO = item.DATA_INCLUSÃO;
                DATA_MODIFICAÇÃO = item.DATA_MODIFICAÇÃO;
                MAT_INCLUSÃO = item.MAT_INCLUSÃO;
                MAT_MODIFICAÇÃO = item.MAT_MODIFICAÇÃO;
                Ficha = item.Ficha;
                Imagens = item.Imagens;
                IsOpened = isOpened;
                PropertyChanged = propertyChanged;
            }

            public bool IsOpened { get; private set; }

            public event Action? PropertyChanged;
            public void OpenClose()
            {
                IsOpened = !IsOpened;
                PropertyChanged?.Invoke();
            }
        }

        protected record FilterPageData
        {
            public FilterPageData(int avaliacao, List<Categoria_Produto> categorias, List<Categoria_Especificação> especificações, List<string> cor, List<string> fabricante, bool isOferta, decimal valor)
            {
                this.avaliacao = avaliacao;
                this.categorias = categorias ?? [];
                this.especificações = especificações ?? [];
                this.cor = cor ?? [];
                this.fabricante = fabricante ?? [];
                IsOferta = isOferta;
                Valor = valor;
            }

            public int avaliacao { get; set; } = 0;
            public List<Categoria_Produto> categorias { get; set; } = [];
            public List<Categoria_Especificação> especificações { get; set; } = [];
            public List<string> cor { get; set; } = [];
            public List<string> fabricante { get; set; } = [];
            public bool IsOferta { get; set; } = false;
            public decimal Valor { get; set; } = 0;
        }

        protected class ImagewithPosition : PRODUTO_IMAGEM
        {
            public ImagewithPosition(int position, byte[] imagem, Guid? id_produto = null) : base(imagem, id_produto)
            {
                ID_PRODUTO = id_produto ?? Guid.Empty;
                Position = position;
                Imagem = imagem;
            }

            public int Position { get; set; }
        }

    }

}
