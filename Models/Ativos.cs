using System.ComponentModel.DataAnnotations;

namespace CryptoTracker.API.Models
{
    public class Ativos
    {
        [Key]
        [Required]
        public int AssetID { get; set; }
        public required string NomeAtivo { get; set; }
        public required string Simbolo { get; set; }

    }
}
