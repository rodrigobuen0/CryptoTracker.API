using CryptoTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace CryptoTracker.API.Services
{
    public interface IAlertasService
    {
        Task<List<Alertas>> GetAlertasPorUsuario(string userId);
        Task AdicionarAlerta(Alertas alerta, string userId);
        Task EditarAlerta(int alertaId, Alertas alertaAtualizado, string userId);
        Task RemoverAlerta(int alertaId, string userId);
    }

    public class AlertasService : IAlertasService
    {
        private readonly AppDbContext _context;

        public AlertasService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Alertas>> GetAlertasPorUsuario(string userId)
        {
            try
            {
                var alertas = await _context.Alertas
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                return alertas;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter alertas por usuário.", ex);
            }
        }

        public async Task AdicionarAlerta(Alertas alerta, string userId)
        {
            try
            {
                alerta.UserId = userId;
                _context.Alertas.Add(alerta);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao adicionar alerta.", ex);
            }
        }

        public async Task EditarAlerta(int alertaId, Alertas alertaAtualizado, string userId)
        {
            try
            {
                var existingAlerta = await _context.Alertas.FindAsync(alertaId);

                if (existingAlerta == null || existingAlerta.UserId != userId)
                {
                    throw new InvalidOperationException($"Alerta com ID {alertaId} não encontrado.");
                }

                alertaAtualizado.UserId = userId;
                alertaAtualizado.Id = alertaId;
                _context.Entry(existingAlerta).CurrentValues.SetValues(alertaAtualizado);

                await _context.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao editar alerta.", ex);
            }
        }

        public async Task RemoverAlerta(int alertaId, string userId)
        {
            try
            {
                var alerta = await _context.Alertas.FindAsync(alertaId);

                if (alerta != null && alerta.UserId == userId)
                {
                    _context.Alertas.Remove(alerta);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    throw new Exception($"Alerta com ID {alertaId} não encontrado.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao remover alerta.", ex);
            }
        }
    }
}
