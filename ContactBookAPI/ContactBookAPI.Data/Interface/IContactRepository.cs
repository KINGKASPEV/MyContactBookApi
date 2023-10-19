using ContactBookAPI.Model;
using ContactBookAPI.Model.DTOs;

namespace ContactBookAPI.Data.Interface
{
    public interface IContactRepository
    {
        Task<List<ContactDto>> Get(PaginationFilter filter);
        Task<Contact> GetById(int Id);
        Task<Contact> GetByIdOrEmail(string emailorId);
        Task<Contact> GetByEmail(string email);
        Task<bool> Create(ContactDto contact);
        Task<bool> Update(int Id, ContactDto contact);
        Task UpdatePhoto(int Id, string photoUrl);
        Task Delete(Contact contact);
        IQueryable<Contact> Search(string name, string state, string city);
    }
}
