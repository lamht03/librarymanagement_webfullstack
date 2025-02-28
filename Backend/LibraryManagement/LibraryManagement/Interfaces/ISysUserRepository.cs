using System.Collections.Generic;
using System.Threading.Tasks;
using LibraryManagement.Models;

namespace LibraryManagement.Interfaces
{
    public interface ISysUserRepository
    {
        Task<(IEnumerable<SysUser>, int)> GetAll(string? userName, int pageNumber, int pageSize);
        Task<SysUser> GetByID(int userId);
        Task<SysUser> GetByEmail(string email);

        Task<int> Create(SysUserInsertModel user);
        Task<int> Update(SysUserUpdateModel user);
        Task<int> Delete(int userId);
        Task<SysUser> GetByRefreshToken(string refreshToken);
        Task<SysUser> VerifyLogin(string userName, string password);
        Task<int> Register(RegisterModel model);

        Task CreateSession(int userId, string refreshToken, DateTime expiryDate);
        Task<Session> GetSessionByRefreshToken(string refreshToken);
        Task UpdateSessionRefreshToken(int sessionId, string newRefreshToken, DateTime newExpiryDate);
        Task RevokeSession(int sessionId);
        Task RevokeAllSessions(int userId);
        Task DeleteAllSessions(int userId);

    }
}