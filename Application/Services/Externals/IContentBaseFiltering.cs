using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface IContentBaseFiltering
    {
        Task<List<PosProduceModel>> RecommendSimilarProductsAsync(List<PosProduceModel> productList, int topN);
    }
    public class PosProduceModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }      
  
    }
}
