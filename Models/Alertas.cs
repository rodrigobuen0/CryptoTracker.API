using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CryptoTracker.API.Models
{
    public class Alertas
    {
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal Valor {  get; set; }
        public MaiorMenor MaiorMenor { get; set; }
        public int AssetId { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser Usuario { get; set; }
        [ForeignKey("AssetId")]
        public Ativos Ativos { get; set; }
    }
}
