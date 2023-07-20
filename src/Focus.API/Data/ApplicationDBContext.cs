using Focus.API.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Focus.API.Data
{
    public class ApplicationDBContext: IdentityDbContext<UserEntity>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options) { }
    }
}