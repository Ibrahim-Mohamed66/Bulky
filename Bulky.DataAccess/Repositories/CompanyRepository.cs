using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repositories.IRepositories;
using Bulky.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repositories;

public class CompanyRepository: Repository<Company,int>, ICompanyRepository
{
    private readonly BulkyDbContext _context;
    public CompanyRepository(BulkyDbContext context) : base(context)
    {
        _context = context;
    }

    public void Update(Company company)
    {
        var existingCompany = _context.Companies.Find(company.Id);
        if (existingCompany != null)
        {
            company.CreatedAt = existingCompany.CreatedAt;

            company.UpdatedAt = DateTime.UtcNow;

            _context.Entry(existingCompany).CurrentValues.SetValues(company);
        }
    }
}
