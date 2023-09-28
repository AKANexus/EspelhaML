using System.ComponentModel.DataAnnotations.Schema;

namespace MlSuite.Domain
{
    public class Item : EntityBase
    {

        public Item(string id, string título, string category, decimal preçoVenda, int quantidadeÀVenda, string permalink, string primeiraFoto, bool éVariação)
        {
            Id = id;
            Título = título;
            Category = category;
            PreçoVenda = preçoVenda;
            QuantidadeÀVenda = quantidadeÀVenda;
            Permalink = permalink;
            PrimeiraFoto = primeiraFoto;
            ÉVariação = éVariação;
        }

        public string Id { get; set; }
        public string Título { get; set; }
        [NotMapped]
        public ulong SellerId => Seller.UserId;
        public string Category { get; set; }
        public decimal PreçoVenda { get; set; }
        public int QuantidadeÀVenda { get; set; }
        public string Permalink { get; set; }
        public string PrimeiraFoto { get; set; }
        public bool ÉVariação { get; set; } = false;
        public List<ItemVariação> Variações { get; set; } = new();
        public MlUserAuthInfo Seller { get; set; }
    }

    public class ItemVariação : EntityBase
    {
        public ItemVariação(ulong id, string descritorVariação, decimal preçoVenda)
        {
            DescritorVariação = descritorVariação;
            PreçoVenda = preçoVenda;
            Id = id;
        }

        public ulong Id { get; set; }
        public decimal PreçoVenda { get; set; }
        public string DescritorVariação { get; set; }
    }
}
