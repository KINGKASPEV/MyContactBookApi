using ContactBookAPI.Model.DTOs;

namespace ContactBookAPI.Data.Interface
{
    public interface IUserRepository
    {
        Task<bool> AddUser(UserToAddDTO newUser);
    }
}
