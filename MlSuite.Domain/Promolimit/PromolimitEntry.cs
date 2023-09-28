using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace MlSuite.Domain
{
    public class PromolimitEntry : EntityBase
    {
        [JsonIgnore]
        public Item Item { get; set; }
        public ItemVariação? Variação { get; set; }
        public int QuantidadeAVenda { get; set; }
        public bool Ativo { get; set; }
        public int Estoque { get; set; }
        [NotMapped] public string Seller => Item.Seller.AccountNickname;
        [NotMapped] public string Mlb => Item.Id;
        [NotMapped] public string Descricao => Item.Título;
        
    }
}
