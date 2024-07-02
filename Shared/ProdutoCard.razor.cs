﻿using Microsoft.AspNetCore.Components;
using Shared_Static_Class.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ConstrainedExecution;

namespace Shared_Razor_Components.Shared
{
    public partial class ProdutoCard : ComponentBase
    {
        [Parameter] public IEnumerable<PRODUTOS_CARDAPIO> Produtos { get; set; }
        IEnumerable<ProdutoCardModel> ProdutosModel { get; set; } = [];
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                PropertyChanged += ProdutoCard_PropertyChanged;
                ProdutosModel = Produtos.Select(x => new ProdutoCardModel(x, PropertyChanged)).ToList();
                StateHasChanged();
            }

            return base.OnAfterRenderAsync(firstRender);
        }
        void ProdutoCard_PropertyChanged() => InvokeAsync(StateHasChanged);

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

    }

}