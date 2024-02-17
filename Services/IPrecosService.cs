using CryptoTracker.API.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Linq;
using System.Text.Json.Serialization;

namespace CryptoTracker.API.Services
{
    public interface IPrecosService
    {
        Task<List<AssetsData>> GetPrecosMeusAssets(string userId);
        Task<List<AssetsData>> GetPrecosPesquisa(string asset, string userId);
        Task<AssetsData> GetPrecoAsset(int assetId);

    }

    public class PrecosService : IPrecosService
    {
        private readonly AppDbContext _context;
        private readonly ICacheService _cacheService;

        public PrecosService(AppDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task<List<AssetsData>> GetPrecosMeusAssets(string userId)
        {
            try
            {
                var listaAssetsId = await _context.Portfolio
                    .Where(p => p.UserID == userId && p.QuantidadeTotal > 0)
                    .Select(p => p.AssetID)
                    .ToListAsync();

                if (listaAssetsId.Count == 0)
                {
                    return new List<AssetsData>();
                }

                var assetsPrices = await _cacheService.Get("PortfolioAssetsPrices");

                if (string.IsNullOrEmpty(assetsPrices))
                {
                    throw new Exception("O cache de preços de ativos está vazio.");
                }

                var assetsPricesDeserialized = JsonConvert.DeserializeObject<AssetsPrices>(assetsPrices);
                if (assetsPricesDeserialized == null)
                {
                    throw new Exception("Falha na desserialização do cache de preços de ativos.");
                }

                List<AssetsData> meusAssets = assetsPricesDeserialized.Data
                    .Where(asset => listaAssetsId.Contains((int)asset.Id))
                    .ToList();

                return meusAssets;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter preços de ativos.", ex);
            }
        }

        public async Task<List<AssetsData>> GetPrecosPesquisa(string asset, string userId)
        {
            try
            {
                var assetsPrices = await _cacheService.Get("PortfolioAssetsPrices");

                if (string.IsNullOrEmpty(assetsPrices))
                {
                    throw new Exception("O cache de preços de ativos está vazio.");
                }

                var assetsPricesDeserialized = JsonConvert.DeserializeObject<AssetsPrices>(assetsPrices);
                if (assetsPricesDeserialized == null)
                {
                    throw new Exception("Falha na desserialização do cache de preços de ativos.");
                }
                asset = asset.ToLower();
                List<AssetsData> assets = assetsPricesDeserialized.Data
                    .Where(a => a.Symbol.ToLower().Contains(asset) || a.Name.ToLower().Contains(asset))
                    .ToList();

                return assets;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter preços de ativos.", ex);
            }
        }
        public async Task<AssetsData> GetPrecoAsset(int assetId)
        {
            try
            {
                var assetsPrices = await _cacheService.Get("PortfolioAssetsPrices");

                if (string.IsNullOrEmpty(assetsPrices))
                {
                    throw new Exception("O cache de preços de ativos está vazio.");
                }

                var assetsPricesDeserialized = JsonConvert.DeserializeObject<AssetsPrices>(assetsPrices);
                if (assetsPricesDeserialized == null)
                {
                    throw new Exception("Falha na desserialização do cache de preços de ativos.");
                }
                AssetsData asset = assetsPricesDeserialized.Data
                    .Where(a => a.Id == assetId)
                    .FirstOrDefault();

                return asset;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter preços de ativos.", ex);
            }
        }

    }

}
