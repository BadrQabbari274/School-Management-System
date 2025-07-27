using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Service.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<Employees>> GetAllUsersAsync();
        Task<Employees> GetUserByIdAsync(int id);
        Task<Employees> CreateUserAsync(Employees user);
        Task<Employees> UpdateUserAsync(Employees user);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<Employees>> GetActiveUsersAsync();
        Task<Employees> GetUserByNameAsync(string name);
    }
}
