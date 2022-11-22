using BulkyBook.Models;
namespace BulkyBook.DataAcess.Repository.IRepository
{
    public interface ICoverTypeRepository : IRepository<CoverType>
    {
        void Update(CoverType obj);
    }
}
