using CryptoTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoTracker.API.Services
{
    public interface IPortfolioService
    {
        Task<List<Portfolio>> GetPortfolio(string userId);
        Task AdicionarPortfolio(Portfolio portfolio);
        Task EditarPortfolio(int portfolioId, Portfolio portfolioAtualizado);
        Task RemoverPortfolio(int portfolioId);
    }

    public class PortfolioService : IPortfolioService
    {
        private readonly AppDbContext _context;

        public PortfolioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Portfolio>> GetPortfolio(string userId)
        {
            try
            {
                var portfolios = await _context.Portfolio
                    .Where(p => p.UserID == userId).Include(a => a.AtivoCripto)
                    .ToListAsync();

                return portfolios;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter portfólio do usuário.", ex);
            }
        }

        public async Task AdicionarPortfolio(Portfolio portfolio)
        {
            try
            {
                _context.Portfolio.Add(portfolio);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar portfólio.", ex);
            }
        }

        public async Task EditarPortfolio(int portfolioId, Portfolio portfolioAtualizado)
        {
            try
            {
                var portfolio = await _context.Portfolio.FindAsync(portfolioId);

                if (portfolio == null)
                {
                    throw new Exception($"Portfólio com ID {portfolioId} não encontrado.");
                }

                portfolio.QuantidadeTotal = portfolioAtualizado.QuantidadeTotal;

                // Adicione outras propriedades conforme necessário

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar portfólio.", ex);
            }
        }

        public async Task RemoverPortfolio(int portfolioId)
        {
            try
            {
                var portfolio = await _context.Portfolio.FindAsync(portfolioId);

                if (portfolio == null)
                {
                    throw new Exception($"Portfólio com ID {portfolioId} não encontrado.");
                }

                _context.Portfolio.Remove(portfolio);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover portfólio.", ex);
            }
        }
    }
}
