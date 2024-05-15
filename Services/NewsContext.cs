using Microsoft.EntityFrameworkCore;
using NewsAPIMobilePark.Models.DTOs;

namespace NewsAPIMobilePark.Services
{
    public class NewsContext(DbContextOptions<NewsContext> options) : DbContext(options)
    {
        public DbSet<Log> Logs { get; set; }
    }
}
