using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTracker.API.Models
{

    public class Transacoes
    {
        [Key]
        public int TransactionID { get; set; }
        public string UserID { get; set; }
        public int PortfolioID { get; set; }
        public int AssetID { get; set; }
        public TipoTransacao Tipo { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal Quantidade { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal PrecoPorUnidade { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal Taxa { get; set; }
        public DateTime DataTransacao { get; set; }

        [ForeignKey("UserID")]
        public ApplicationUser Usuario { get; set; }
        [ForeignKey("PortfolioID")]
        public Portfolio Portfolio { get; set; }
        [ForeignKey("AssetID")]
        public Ativos Ativos { get; set; }
    }
    public class TransacoesDto
    {
        [Key]
        public int TransactionID { get; set; }
        public string? UserID { get; set; }
        public int PortfolioID { get; set; }
        public int AssetID { get; set; }
        public TipoTransacao Tipo { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Quantidade { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecoPorUnidade { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Taxa { get; set; }
        public DateTime DataTransacao { get; set; }
    }

    public class ResultadoCalculoTransacoes
    {
        public decimal ValorMedio { get; set; }
        public decimal QuantidadeTotal { get; set; }
    }
}
