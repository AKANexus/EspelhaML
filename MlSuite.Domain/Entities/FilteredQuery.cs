using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MlSuite.Domain.Entities
{
    public class FilteredQuery
    {
        public int? offset { get; set; }
        public int? limit { get; set; }
        public Filter[]? Filters { get; set; }
    }

    public class Filter
    {
        public string Field { get; set; }
        public string Query { get; set; }
        public FilterType Method { get; set; }
    }

    public enum FilterType
    {
        ComecaCom,
        Contem,
        Igual,
        Diferente,
        MaiorQue,
        MenorQue,

    }

    public class SimpleFilteredQuery : FilteredQuery
    {
        public string query { get; set; }
    }
}
