using Application.Services.CQS.Queries;
using Application.Services.Externals;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.RecommentderSystem
{
    public class ContentBaseFiltering : IContentBaseFiltering
    {
        private List<PosProduceModel> products;
        private readonly IQueryContext _context;
        private readonly IMapper _mapper;

        public ContentBaseFiltering(IQueryContext context, IMapper mapper)
        {
          
            _context = context;
            _mapper = mapper;
         
        }
        private string PreProcessText(string text)
        {
            if (text == null)
                return null;
            string[] punctuation = { ".", ",", "!", "?", ";", ":", "-", "_", "(", ")", "[", "]", "{", "}", "\"", "'", "<", ">", "/", "\\", "|", "@", "#", "$", "%", "^", "&", "*", "+", "=", "~" };
            foreach (var p in punctuation)
            {
                text = text.Replace(p, "");
            }
            text = text.ToLower();
            string[] stopWords = {"là", "của", "và", "trong", "để", "có", "đã", "không", "ở", "có thể", "này", "đó", "như", "thì",
                "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín", "mười" ,"nhưng", "hoặc", "nếu", "khi","với","qua","giữa","vào","xuống","lên","ra"};
            foreach (var sw in stopWords)
            {
                text = text.Replace(" " + sw + " ", " ");
            }
            text = text.Trim();
            return text;
        }
        private Dictionary<string, Dictionary<string, double>> CalculateTF()
        {
            var tfDict = new Dictionary<string, Dictionary<string, double>>();
            foreach (var product in products)
            {
                var tf = new Dictionary<string, double>();
                var totalWords = 0;
                var description = PreProcessText($"{product.Name} {product.Category}");
                var words = description.Split(' ');
                foreach (var word in words)
                {
                    if (!string.IsNullOrWhiteSpace(word))
                    {
                        if (tf.ContainsKey(word))
                        {
                            tf[word]++;
                        }
                        else
                        {
                            tf[word] = 1;
                        }
                        totalWords++;
                    }

                }
                foreach (var key in tf.Keys.ToList())
                {
                    tf[key] = tf[key] / totalWords;
                }
                tfDict[product.Id] = tf;
            }
            return tfDict;
        }
        private Dictionary<string, double> CalculateIDF()
        {
            var idf = new Dictionary<string, double>();
            var totalDocuments = products.Count;

            foreach (var product in products)
            {
                var processedText = PreProcessText($"{product.Name} {product.Category}");
                var words = processedText.Split(' ').Distinct();
                foreach (var word in words)
                {
                    if (!string.IsNullOrWhiteSpace(word))
                    {
                        if (idf.ContainsKey(word))
                        {
                            idf[word]++;
                        }
                        else
                        {
                            idf[word] = 1;
                        }
                    }
                }

            }
            foreach (var key in idf.Keys.ToList())
            {
                idf[key] = Math.Log(totalDocuments / (1 + idf[key]));
            }

            return idf;
        }
        private Dictionary<string, Dictionary<string, double>> CalculateTFIDF()
        {
            var tf = CalculateTF();
            var idf = CalculateIDF();
            var tfidf = new Dictionary<string, Dictionary<string, double>>();

            foreach (var productGuid in tf.Keys)
            {
                var tfidfDict = new Dictionary<string, double>();
                foreach (var word in tf[productGuid].Keys)
                {

                    tfidfDict[word] = tf[productGuid][word] * idf[word];

                }
                tfidf[productGuid] = tfidfDict;
            }
            return tfidf;
        }
        private double ConsineSimilarity(Dictionary<string, double> vectorA, Dictionary<string, double> vectorB)
        {
            var dotProduct = 0.0;
            var magnitudeA = 0.0;
            var magnitudeB = 0.0;

            foreach (var key in vectorA.Keys)
            {
                if (vectorB.ContainsKey(key))
                {
                    dotProduct += vectorA[key] * vectorB[key];
                }
                magnitudeA += Math.Pow(vectorA[key], 2);
            }
            foreach (var val in vectorB.Values)
            {
                magnitudeB += Math.Pow(val, 2);
            }
            magnitudeA = Math.Sqrt(magnitudeA);
            magnitudeB = Math.Sqrt(magnitudeB);
            if (magnitudeA == 0 || magnitudeB == 0)
                return 0;
            return dotProduct / (magnitudeA * magnitudeB);
        }
        public async Task<List<PosProduceModel>> RecommendSimilarProductsAsync(List<PosProduceModel> productList, int topN)
        {
            this.products = await _context.Product
                    .Select(x => new PosProduceModel { Id = x.Id, Name = x.Title, Category = x.ProductCategory.Title })
                    .ToListAsync();
            var tfidf = CalculateTFIDF();
            var recommendations = new Dictionary<string, double>();

            var productGuids = productList.Select(x => x.Id).ToList();
            foreach (var productId in productGuids)
            {
                var productVector = tfidf[productId];
                foreach (var otherPRoductId in tfidf.Keys)
                {
                    if (!productGuids.Contains(otherPRoductId))
                    {

                        var similarity = ConsineSimilarity(productVector, tfidf[otherPRoductId]);
                        if (similarity > 0.1)
                        {
                            recommendations[otherPRoductId] = similarity;
                        }
                    }
                }
            }
            var sortedRecommendations = recommendations.OrderByDescending(pair => pair.Value);
            var recommendedProducts = new List<PosProduceModel>();
            var count = 0;
            foreach (var pair in sortedRecommendations)
            {
                if (count >= topN)
                    break;
                var product = products.FirstOrDefault(x => x.Id == pair.Key);
                if (product != null && !productGuids.Contains(product.Id))
                {
                    recommendedProducts.Add(product);
                    count++;
                }

            }
            return recommendedProducts;
        }
    }
   
}
