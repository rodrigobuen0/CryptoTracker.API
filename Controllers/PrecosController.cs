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
    public class PrecosController : ControllerBase
    {
        private readonly IPrecosService _precosService;

        public PrecosController(IPrecosService precosService)
        {
            _precosService = precosService;
        }

        [HttpGet]
        public async Task<ActionResult<List<AssetsData>>> GetPrecos()
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }
                var precosAssets = await _precosService.GetPrecosMeusAssets(userId);

                return Ok(precosAssets);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter preços: {ex.Message}");
            }
        }

        [HttpGet("{asset}")]
        public async Task<ActionResult<List<AssetsData>>> GetPrecosAssetsPesquisa(string asset)
        {
            try
            {
                var userId = User.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("Usuário não autorizado");
                }
                var precosAssets = await _precosService.GetPrecosPesquisa(asset, userId);

                return Ok(precosAssets);
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro ao obter preços: {ex.Message}");
            }
        }
    }
}
