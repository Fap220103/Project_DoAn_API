using Application.Features.Orders.Commands;
using Application.Features.Products.Queries;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Externals
{
    public interface IGeneralReview
    {
        Task<int> GetGeneralReview(string content);
    }
    public class PredictRequest
    {
        public string Text { get; set; }
    }

    public class PredictResponse
    {
        public int Prediction { get; set; }
    }
}
