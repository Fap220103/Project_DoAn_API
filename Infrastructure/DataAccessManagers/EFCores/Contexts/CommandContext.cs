using Application.Services.CQS.Commands;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.DataAccessManagers.EFCores.Contexts
{
    public class CommandContext : DataContext, ICommandContext
    {
        public CommandContext(DbContextOptions<DataContext> options)
        : base(options)
        {
        }
    }
}
