using Application.Services.Repositories;
using Infrastructure.DataAccessManagers.EFCores.Contexts;
using Domain.Entities;

namespace Infrastructure.SeedManagers.Systems
{
    public class SizeSeeder
    {
        private readonly CommandContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public SizeSeeder(
            CommandContext context,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task GenerateDataAsync()
        {
            if (_context.Size.Any()) return;
            var sizes = new List<Size>
            {
                new Size { Name = "S" },
                new Size { Name = "M" },
                new Size { Name = "L" },
                new Size { Name = "XL" },
                new Size { Name = "XXL" }
            };

            _context.Size.AddRange(sizes);
            await _unitOfWork.SaveAsync();
        }

    }
}
