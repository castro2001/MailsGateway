using Infrastructure.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Seeders.Core
{
    public interface ISeeder
    {
        void Seed(ApplicationDbContext context);
    }
}
