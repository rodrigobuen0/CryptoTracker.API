using CryptoTracker.API.Models;
using CryptoTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using CryptoTracker.API.Controllers;
using System.IdentityModel.Tokens.Jwt;

namespace CryptoTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TransacoesController : ControllerBase
    {
        private readonly ITransacoesService _transacoesService;

        public TransacoesController(ITransacoesService transacoesService)
        {
            _transacoesService = transacoesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Transacoes>>> GetAllTransacoes()
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }
                var transacoes = await _transacoesService.GetAllTransacoesAsync(userId);
                return Ok(transacoes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter transações: {ex.Message}");
            }
        }

        [HttpGet("{transacaoId}")]
        public async Task<ActionResult<Transacoes>> GetTransacaoById(int transacaoId)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                var transacao = await _transacoesService.GetTransacaoByIdAsync(transacaoId, userId);
                if (transacao == null)
                    return NotFound();

                return Ok(transacao);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter transação: {ex.Message}");
            }
        }

        [HttpGet("asset/{assetId}")]
        public async Task<ActionResult<List<Transacoes>>> GetTransacoesByAssetId(int assetId)
        {

            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }
                var transacoes = await _transacoesService.GetTransacoesByAssetIdAsync(assetId, userId);
                return Ok(transacoes);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter transações: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateTransacao([FromBody] TransacoesDto transacaoDto)
        {
            try
            {
                var transacao = new Transacoes
                {
                    TransactionID = transacaoDto.TransactionID,
                    PortfolioID = transacaoDto.PortfolioID,
                    AssetID = transacaoDto.AssetID,
                    Tipo = transacaoDto.Tipo,
                    Quantidade = transacaoDto.Quantidade,
                    PrecoPorUnidade = transacaoDto.PrecoPorUnidade,
                    Taxa = transacaoDto.Taxa,
                    DataTransacao = transacaoDto.DataTransacao,
                };

                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                await _transacoesService.CreateTransacaoAsync(transacao, userId);
                return CreatedAtAction(nameof(GetTransacaoById), new { transacaoId = transacao.TransactionID }, transacao);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao criar transação: {ex.Message}");
            }
        }

        [HttpPut("{transacaoId}")]
        public async Task<ActionResult> UpdateTransacao(int transacaoId, [FromBody] Transacoes transacao)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                await _transacoesService.UpdateTransacaoAsync(transacaoId, transacao, userId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao atualizar transação: {ex.Message}");
            }
        }

        [HttpDelete("{transacaoId}")]
        public async Task<ActionResult> DeleteTransacao(int transacaoId)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
                await _transacoesService.DeleteTransacaoAsync(transacaoId, userId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao excluir transação: {ex.Message}");
            }
        }
    }
}
