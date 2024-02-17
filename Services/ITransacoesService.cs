using CryptoTracker.API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTracker.API.Services
{
    public interface ITransacoesService
    {
        Task<List<Transacoes>> GetAllTransacoesAsync(string userId);
        Task<Transacoes> GetTransacaoByIdAsync(int transacaoId, string userId);
        Task<List<Transacoes>> GetTransacoesByAssetIdAsync(int assetId, string userId);
        Task CreateTransacaoAsync(Transacoes transacao, string userId);
        Task CreateTransacaoVendaAsync(Transacoes transacao, string userId);
        Task UpdateTransacaoAsync(int transacaoId, Transacoes transacao, string userId);
        Task DeleteTransacaoAsync(int transacaoId, string userId);
    }

    public class TransacoesService : ITransacoesService
    {
        private readonly AppDbContext _context;

        public TransacoesService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Transacoes>> GetAllTransacoesAsync(string userId)
        {
            return await _context.Transacoes
            .Where(t => t.UserID == userId)
            .ToListAsync();
        }

        public async Task<Transacoes> GetTransacaoByIdAsync(int transacaoId, string userId)
        {
            return await _context.Transacoes
                .Where(t => t.TransactionID == transacaoId && t.UserID == userId).OrderByDescending(t => t.DataTransacao)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Transacoes>> GetTransacoesByAssetIdAsync(int assetId, string userId)
        {
            return await _context.Transacoes
                .Where(t => t.AssetID == assetId && t.UserID == userId).Include(t => t.Ativos)
                .ToListAsync();
        }

        public async Task CreateTransacaoAsync(Transacoes transacao, string userId)
        {
            var portfolio = await _context.Portfolio.Where(p => p.AssetID == transacao.AssetID).FirstOrDefaultAsync();
            if (portfolio == null)
            {
                var newPortfolio = new Portfolio { AssetID = transacao.AssetID, QuantidadeTotal = 0, ValorMedio = 0, UserID = userId, CustoTotal = 0};
                _context.Portfolio.Add(newPortfolio);
                await _context.SaveChangesAsync();
                transacao.PortfolioID = newPortfolio.PortfolioID;
                portfolio = newPortfolio;
            }
            else
            {
                transacao.PortfolioID = portfolio.PortfolioID;
            }

            transacao.UserID = userId;
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();

            var calculoVlrMedioQtdTotal = await CalcularValorMedioQtdTotal(transacao.AssetID, userId);
            portfolio.ValorMedio = calculoVlrMedioQtdTotal.ValorMedio;
            portfolio.CustoTotal += transacao.Custo;
            portfolio.QuantidadeTotal = calculoVlrMedioQtdTotal.QuantidadeTotal;

            _context.Update(portfolio);
            await _context.SaveChangesAsync();

        }
        public async Task CreateTransacaoVendaAsync(Transacoes transacaoVenda, string userId)
        {
            var portfolio = await _context.Portfolio
                                            .Where(p => p.AssetID == transacaoVenda.AssetID && p.UserID == userId)
                                            .FirstOrDefaultAsync();

            if (portfolio == null)
            {
                // Portfolio não encontrado para o ativo e usuário - você pode lidar com isso de acordo com sua lógica de negócios
                throw new Exception("Portfolio não encontrado para o ativo e usuário.");
            }
            else
            {
                // Verifica se há quantidade suficiente no portfólio para realizar a venda
                if (portfolio.QuantidadeTotal < transacaoVenda.Quantidade)
                {
                    throw new Exception("Quantidade insuficiente no portfólio para realizar a venda.");
                }

                // Calcula o novo preço médio após a venda
                portfolio.CustoTotal -= transacaoVenda.Custo;

                // Atualiza a quantidade total no portfólio após a venda
                portfolio.QuantidadeTotal -= transacaoVenda.Quantidade;

                // Adiciona a transação de venda ao contexto e salva as alterações
                transacaoVenda.UserID = userId;
                transacaoVenda.PortfolioID = portfolio.PortfolioID;
                _context.Transacoes.Add(transacaoVenda);
                await _context.SaveChangesAsync();

                // Atualiza o portfólio no contexto e salva as alterações
                _context.Portfolio.Update(portfolio);
                await _context.SaveChangesAsync();
            }
        }


        public async Task UpdateTransacaoAsync(int transacaoId, Transacoes transacao, string userId)
        {
            var existingTransacao = await _context.Transacoes.FindAsync(transacaoId);

            if (existingTransacao == null || existingTransacao.UserID != userId)
            {
                throw new InvalidOperationException("Transação não encontrada ou não pertence ao usuário.");
            }
            var portfolio = await _context.Portfolio.Where(p =>p.AssetID == transacao.PortfolioID && p.UserID == userId).FirstOrDefaultAsync();

            var calculoVlrMedioQtdTotal = await CalcularValorMedioQtdTotal(transacao.AssetID, userId);
            portfolio.ValorMedio = calculoVlrMedioQtdTotal.ValorMedio;
            portfolio.CustoTotal -= existingTransacao.Custo;
            portfolio.CustoTotal += transacao.Custo;
            portfolio.QuantidadeTotal = calculoVlrMedioQtdTotal.QuantidadeTotal;
            _context.Update(portfolio);

            transacao.UserID = userId;
            transacao.TransactionID = transacaoId;
            _context.Entry(existingTransacao).CurrentValues.SetValues(transacao);

            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransacaoAsync(int transacaoId, string userId)
        {
            var transacao = await _context.Transacoes.FindAsync(transacaoId);

            if (transacao != null && transacao.UserID == userId)
            {
                _context.Transacoes.Remove(transacao);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<ResultadoCalculoTransacoes> CalcularValorMedioQtdTotal(int assetId, string userId)
        {
            var transacoes = await _context.Transacoes.Where(t => t.AssetID == assetId && t.UserID == userId).ToListAsync();

            if (transacoes == null || !transacoes.Any())
            {
                return new ResultadoCalculoTransacoes { QuantidadeTotal = 0, ValorMedio = 0 };
            }

            var transacoesCompra = transacoes.Where(t => t.Tipo == TipoTransacao.Compra).ToList();
            var transacoesVenda = transacoes.Where(t => t.Tipo == TipoTransacao.Venda).ToList();


            decimal quantidadeTotal = transacoesCompra.Sum(t => t.Quantidade) - transacoesVenda.Sum(t => t.Quantidade);

            decimal valorTotalCompras = transacoesCompra.Sum(t => t.Quantidade * t.PrecoPorUnidade);

            decimal valorTotalVendas = transacoesVenda.Sum(t => t.Quantidade * t.PrecoPorUnidade);

            decimal valorMedio = (valorTotalCompras - valorTotalVendas) / quantidadeTotal;

            return new ResultadoCalculoTransacoes { QuantidadeTotal = quantidadeTotal, ValorMedio = valorMedio };
        }
        private decimal CalcularNovoPrecoMedio(Portfolio portfolio, Transacoes transacaoVenda)
        {
            // Calcula o custo total restante no portfólio após a venda
            decimal custoTotalRestante = portfolio.CustoTotal - (transacaoVenda.Quantidade * transacaoVenda.PrecoPorUnidade);

            // Calcula a quantidade total restante no portfólio após a venda
            decimal quantidadeTotalRestante = portfolio.QuantidadeTotal - transacaoVenda.Quantidade;

            // Calcula o novo preço médio com base no custo total e na quantidade total restante
            decimal novoPrecoMedio = quantidadeTotalRestante > 0 ? custoTotalRestante / quantidadeTotalRestante : 0;

            return novoPrecoMedio;
        }
    }
}
