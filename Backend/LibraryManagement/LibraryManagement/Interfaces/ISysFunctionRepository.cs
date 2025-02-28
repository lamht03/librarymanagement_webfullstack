﻿using LibraryManagement.Controllers;
using LibraryManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManagement.Interfaces
{
    public interface ISysFunctionRepository
    {
        Task<(IEnumerable<SysFunction>, int)> GetAll(string? functionName, int pageNumber, int pageSize);
        Task<SysFunction> GetByID(int functionID);
        Task<int> Create(SysFunctionInsertModel function);
        Task<int> Update(SysFunctionUpdateModel function);
        Task<int> Delete(int functionID);
    }
}
