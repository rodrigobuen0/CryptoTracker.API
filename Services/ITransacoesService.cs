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
        Task<Transacoes> GetTransacaoByIdAsync(int transacaoId, string userId); // Adicionado o userId
        Task CreateTransacaoAsync(Transacoes transacao, string userId); // Adicionado o userId
        Task UpdateTransacaoAsync(int transacaoId, Transacoes transacao, string userId); // Adicionado o userId
        Task DeleteTransacaoAsync(int transacaoId, string userId); // Adicionado o userId
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

        public async Task<List<Transacoes>> GetTransacoesDoUsuarioAsync(string userId)
        {
            return await _context.Transacoes
                .Where(t => t.UserID == userId)
                .ToListAsync();
        }

        public async Task<Transacoes> GetTransacaoByIdAsync(int transacaoId, string userId)
        {
            return await _context.Transacoes
                .Where(t => t.TransactionID == transacaoId && t.UserID == userId)
                .FirstOrDefaultAsync();
        }

        public async Task CreateTransacaoAsync(Transacoes transacao, string userId)
        {
            var portfolio = await _context.Portfolio.Where(p => p.AssetID == transacao.AssetID).FirstOrDefaultAsync();
            if (portfolio == null)
            {
                var newPortfolio = new Portfolio { AssetID = transacao.AssetID, QuantidadeTotal = 0, ValorMedio = 0, UserID = userId };
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
            portfolio.QuantidadeTotal = calculoVlrMedioQtdTotal.QuantidadeTotal;

            _context.Update(portfolio);
            await _context.SaveChangesAsync();

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
    }
}
