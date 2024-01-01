using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CryptoTracker.API.Models
{
    public class Portfolio
    {
        [Key]
        public int PortfolioID { get; set; }
        public string UserID { get; set; }
        public int AssetID { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal QuantidadeTotal { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorMedio { get; set; }

        [ForeignKey("UserID")]
        public ApplicationUser Usuario { get; set; }
        [ForeignKey("AssetID")]
        public Ativos AtivoCripto { get; set; }
        public List<Transacoes> Transacoes { get; set; }
    }
}
