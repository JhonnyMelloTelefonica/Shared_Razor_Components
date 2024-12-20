﻿using Blazorise.Modules;
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
using Shared_Razor_Components.FundamentalModels;
using Shared_Static_Class.Model_DTO.FilterModels;
using Microsoft.AspNetCore.Components.Web;
using Shared_Razor_Components.Services;
using Radzen.Blazor;
using Newtonsoft.Json;
using Microsoft.FluentUI.AspNetCore.Components;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Shared_Razor_Components.Shared
{
    public partial class ProdutoCard : ComponentBase, IDisposable
    {
        [Parameter] public IEnumerable<PRODUTOS_CARDAPIO> Produtos { get; set; } = [];
        IEnumerable<PRODUTOS_CARDAPIO> ProdutosBySearch { get; set; } = [];
        [Inject] NavigationManager NavigationManager { get; set; } = default!;
        [Inject] IJSRuntime JSRuntime { get; set; } = default!;
        [Inject] ICardapioDigitalService _service { get; set; } = default!;
        [Inject] IDialogService FluentDialog { get; set; } = default!;
        IJSObjectReference _jsmodule { get; set; } = default!;
        string Orderby { get; set; } = "Avaliacao.Avaliacao";
        bool AscOrDesc { get; set; } = true;
        GenericPaginationModel<PainelCardapioDigital> filterPagination { get; set; } = new(new(string.Empty, 0, [], [], [], [], null, 0));
        GenericPagedResponse<IEnumerable<PRODUTOS_CARDAPIO>> DataResponse { get; set; } = new([], 1, 50);
        protected PainelCardapioDigital filter { get; set; } = new(string.Empty, 0, null, null, null, null, null, 0);
        SearchProdutos search { get; set; }
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
        IEnumerable<ProdutoCardModel> ProdutosFiltered
        {
            get
            {
                IEnumerable<PRODUTOS_CARDAPIO> saida = ProdutosBySearch;

                if (!string.IsNullOrEmpty(Orderby))
                {
                    saida = saida.OrderBy(Orderby, AscOrDesc);
                }

                return saida.Select(x => new ProdutoCardModel(x, PropertyChanged));
            }
        }

        string SearchCategoria { get; set; } = string.Empty;
        string SearchEspecificação { get; set; } = string.Empty;
        protected override void OnInitialized()
        {
            PropertyChanged += ProdutoCard_PropertyChanged;
            filterPagination = new(new PainelCardapioDigital(filter.search,
                filter.avaliacao,
                filter.categorias,
                filter.especificações,
                filter.cor,
                filter.fabricante,
                filter.IsOferta,
                filter.Valor), 1, 25);
            base.OnInitialized();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //.Where(x => !string.IsNullOrEmpty(SearchCategoria) ? x.GetDisplayName().Contains(SearchCategoria, StringComparison.InvariantCultureIgnoreCase) : x.GetDisplayName() != null)
                //.Where(x => !string.IsNullOrEmpty(SearchEspecificação) ? x.GetDisplayName().Contains(SearchEspecificação, StringComparison.InvariantCultureIgnoreCase) : x.GetDisplayName() != null)
                _jsmodule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Shared_Razor_Components/Shared/ProdutoCard.razor.js");
                search.TriggerSearch += SearchByFilters;
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

        async void SearchByFilters(string value)
        {
            filter.search = value;
            await SearchAction(1);
        }

        async void SearchByFilters(int value, bool HasSearchByName = true)
        {
            await SearchAction(value, HasSearchByName);
        }

        private async Task SearchAction(int? NewPage, bool HasSearchByName = true)
        {
            if (NewPage.HasValue)
                filterPagination.PageNumber = NewPage.Value;
            if (!HasSearchByName)
                filter.search = string.Empty;

            filterPagination.Value = new PainelCardapioDigital(filter.search, filter.avaliacao, filter.categorias, filter.especificações, filter.cor, filter.fabricante, filter.IsOferta, filter.Valor);
            var result = await _service.Search(filterPagination);
            if (result.IsSuccess)
            {
                var saida = JsonConvert.DeserializeObject<Response<object>>(result.Content.ToString());
                if (saida.Succeeded)
                {
                    DataResponse = JsonConvert.DeserializeObject<GenericPagedResponse<IEnumerable<PRODUTOS_CARDAPIO>>>(saida.Data.ToString());
                    ProdutosBySearch = DataResponse.Data;
                }
                else
                {
                    await FluentDialog.ShowErrorAsync(saida.Message, "Erro!");

                    if (JSRuntime != null && saida.Errors != null && saida.Errors.Any())
                    {
                        await JSRuntime.InvokeVoidAsync("console.log", string.Join(';', saida.Errors));
                    }
                }
            }
            else
            {
                await FluentDialog.ShowErrorAsync(result.ErrorMessage, "Algum erro ocorreu");
            }

            await InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            PropertyChanged -= ProdutoCard_PropertyChanged;
            search.TriggerSearch -= SearchByFilters;
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
