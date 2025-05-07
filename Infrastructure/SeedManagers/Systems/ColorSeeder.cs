using Application.Services.Repositories;
using Domain.Entities;
using Infrastructure.DataAccessManagers.EFCores.Contexts;


namespace Infrastructure.SeedManagers.Systems
{
    public class ColorSeeder
    {
        private readonly CommandContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public ColorSeeder(
            CommandContext context,
            IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task GenerateDataAsync()
        {
         
            if (_context.Color.Any()) return;

            var colors = new List<Color>
            {
                new Color { Name = "Đỏ", HexCode = "#FF0000" },
                new Color { Name = "Xanh lá", HexCode = "#008000" },
                new Color { Name = "Xanh dương", HexCode = "#0000FF" },
                new Color { Name = "Đen", HexCode = "#000000" },
                new Color { Name = "Trắng", HexCode = "#FFFFFF" },
                new Color { Name = "Xám", HexCode = "#808080" },
                new Color { Name = "Vàng", HexCode = "#FFFF00" },
                new Color { Name = "Cam", HexCode = "#FFA500" },
                new Color { Name = "Tím", HexCode = "#800080" },
                new Color { Name = "Hồng", HexCode = "#FFC0CB" },
                new Color { Name = "Nâu", HexCode = "#A52A2A" },
                new Color { Name = "Lục lam", HexCode = "#00FFFF" }
            };

            _context.Color.AddRange(colors);
            await _unitOfWork.SaveAsync();
        }

    }
}
