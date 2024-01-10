using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTracker.API.Models;
using CryptoTracker.API.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace CryptoTracker.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AlertasController : ControllerBase
    {
        private readonly IAlertasService _alertasService;

        public AlertasController(IAlertasService alertasService)
        {
            _alertasService = alertasService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Alertas>>> GetAlertasPorUsuario()
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }
                var alertas = await _alertasService.GetAlertasPorUsuario(userId);
                return Ok(alertas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult> AdicionarAlerta([FromBody] Alertas alerta)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }
                await _alertasService.AdicionarAlerta(alerta, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpPut("{alertaId}")]
        public async Task<ActionResult> EditarAlerta(int alertaId, [FromBody] Alertas alertaAtualizado)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }
                await _alertasService.EditarAlerta(alertaId, alertaAtualizado, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }

        [HttpDelete("{alertaId}")]
        public async Task<ActionResult> RemoverAlerta(int alertaId)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }
                await _alertasService.RemoverAlerta(alertaId, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro interno: {ex.Message}");
            }
        }
    }
}
