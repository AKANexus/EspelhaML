using System.Text.Json.Serialization;

namespace MlSuite.Api.DTOs
{
    public class FilteredQueryDto
    {
        public FilteredQueryDto(int take = 15, int skip = 0)
        {
            Take = take;
            Skip = skip;
        }

        [JsonPropertyName("limit")]
        public int Take { get; set; }
        [JsonPropertyName("offset")]
        public int Skip { get; set; }
        [JsonPropertyName("filtros")]
        public QueryFilterDto[]? Filters { get; set; }
        [JsonPropertyName("ordens")] 
        public QueryOrderDto[]? OrderDtos { get; set; }
    }

    public class QueryFilterDto
    {
        public QueryFilterDto(string property, string filter, string type)
        {
            Property = property;
            Filter = filter;
            Type = type;
        }

        [JsonPropertyName("propriedade")]
        public string Property { get; set; }
        [JsonPropertyName("filtro")]
        public string Filter { get; set; }
        [JsonPropertyName("tipo")]
        public string Type { get; set; }
    }

    public class QueryOrderDto
    {
        public QueryOrderDto(string property, string order)
        {
            Property = property;
            Order = order;
        }

        [JsonPropertyName("propriedade")] 
        public string Property { get; set; }
        [JsonPropertyName("ordem")] 
        public string Order { get; set; }
    }
}
