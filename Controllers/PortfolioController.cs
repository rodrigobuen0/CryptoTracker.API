using CryptoTracker.API.Models;
using CryptoTracker.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CryptoTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService _portfolioService;

        public PortfolioController(IPortfolioService portfolioService)
        {
            _portfolioService = portfolioService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Portfolio>>> GetPortfolio()
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }

                var portfolios = await _portfolioService.GetPortfolio(userId);
                return Ok(portfolios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AdicionarPortfolio([FromBody] Portfolio portfolio)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }

                await _portfolioService.AdicionarPortfolio(portfolio);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPut("{portfolioId}")]
        public async Task<ActionResult> EditarPortfolio(int portfolioId, [FromBody] Portfolio portfolioAtualizado)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }

                await _portfolioService.EditarPortfolio(portfolioId, portfolioAtualizado);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpDelete("{portfolioId}")]
        public async Task<ActionResult> RemoverPortfolio(int portfolioId)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }

                await _portfolioService.RemoverPortfolio(portfolioId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
