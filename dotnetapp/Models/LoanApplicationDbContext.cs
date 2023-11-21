using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace dotnetapp.Models;

public class LoanApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
{

    public LoanApplicationDbContext(DbContextOptions<LoanApplicationDbContext> options)
        : base(options)
    {
    }
    public virtual DbSet<LoanApplication> LoanApplications { get; set; }
    public virtual DbSet<Loan> Loans { get; set; }
    public virtual DbSet<User> Users { get; set; }
}
