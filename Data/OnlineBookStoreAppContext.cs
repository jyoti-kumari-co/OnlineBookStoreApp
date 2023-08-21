using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OnlineBookStoreApp.Models;

namespace OnlineBookStoreApp.Data
{
    public class OnlineBookStoreAppContext : DbContext
    {
        public OnlineBookStoreAppContext (DbContextOptions<OnlineBookStoreAppContext> options)
            : base(options)
        {
        }

        public DbSet<OnlineBookStoreApp.Models.BookViewModel> BookViewModel { get; set; } = default!;
    }
}
