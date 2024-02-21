using MlSuite.Domain;
using MlSuite.Domain.Enums.JsonConverters;
using System.Text.Json.Serialization;

namespace MlSuite.Api.DTOs
{
    public class SeparaçãoDto
    {
        public string? usuario { get; set; }
        public DateTime? inicio { get; set; }
        public DateTime? fim { get; set; }
        public long identificador { get; set; }
        public EmbalagemDto[] embalagens { get; set; }
        public long seller_id { get; set; }
        public string seller_nick { get; set; }

        public SeparaçãoDto(Separação separação, MlUserAuthInfo seller)
        {
            usuario = separação.Usuário?.DisplayName;
            inicio = separação.Início;
            fim = separação.Fim;
            identificador = separação.Identificador;
            seller_id = (long)seller.UserId;
            seller_nick = seller.AccountNickname;
            embalagens = separação.Embalagens.Select(x => new EmbalagemDto
            {
                embalagem_itens = x.EmbalagemItems.Select(y=> new EmbalagemItemDto
                {
                    descricao = y.Descrição,
                    quantidade = y.QuantidadeAEscanear,
                    separados = y.QuantidadeEscaneada,
                    sku = y.SKU,
                    url_imagem = y.ImageUrl
                }).ToArray(),
                etiqueta = x.Etiqueta,
                referencia_id = x.ReferenciaId,
                status_embalagem = x.StatusEmbalagem,
                tipo_venda_ml = x.TipoVendaMl
            }).ToArray();
        }
    }

    public class EmbalagemDto
    {
        public string? etiqueta { get; set; }
        public ulong referencia_id { get; set; }
        [JsonConverter(typeof(EnumStringConverter<TipoVendaMl>))]
        public TipoVendaMl tipo_venda_ml { get; set; }
        public EmbalagemItemDto[] embalagem_itens { get; set; }
        public StatusEmbalagem status_embalagem { get; set; }
        public DateTime? timestamp_impressao { get; set; }
    }

    public class EmbalagemItemDto
    {
        public string sku { get; set; }
        public string url_imagem { get; set; }
        public string descricao { get; set; }
        public int quantidade { get; set; }
        public int separados { get; set; }
    }
}
