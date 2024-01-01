﻿using CryptoTracker.API.Models;
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
            transacao.UserID = userId; // Define o ID do usuário na transação
            _context.Transacoes.Add(transacao);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTransacaoAsync(int transacaoId, Transacoes transacao, string userId)
        {
            var existingTransacao = await _context.Transacoes.FindAsync(transacaoId);

            if (existingTransacao == null || existingTransacao.UserID != userId)
            {
                throw new InvalidOperationException("Transação não encontrada ou não pertence ao usuário.");
            }

            transacao.UserID = userId; // Garante que o ID do usuário é mantido durante a atualização
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
    }
}