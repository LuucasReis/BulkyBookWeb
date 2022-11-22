using BulkyBook.DataAcess.Data;
using BulkyBook.DataAcess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAcess.Repository
{
	internal class CompanyRepository : Repository<Company>, ICompanyRepository
	{
		private readonly ApplicationDbContext _db;

		public CompanyRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}
		public void Update(Company company)
		{
			_db.CompanyUsers.Update(company);
		}
	}
}
