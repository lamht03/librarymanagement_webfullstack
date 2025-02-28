using LibraryManagement.Models;

namespace LibraryManagement.Interfaces
{
    public interface ISysGroupRepository
    {
        Task<(IEnumerable<SysGroup>, int)> GetAll(string? groupName, int pageNumber, int pageSize);
        Task<SysGroup> GetByID(int groupID);
        Task<int> Create(SysGroupInsertModel group);
        Task<int> Update(SysGroupUpdateModel group);
        Task<int> Delete(int groupID);

    }
}
