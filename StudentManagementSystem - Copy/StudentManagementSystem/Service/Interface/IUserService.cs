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
        Task<IEnumerable<Employee>> GetAllUsersAsync();
        Task<Employee> GetUserByIdAsync(int id);
        Task<Employee> CreateUserAsync(Employee user);
        Task<Employee> UpdateUserAsync(Employee user);
        Task<bool> DeleteUserAsync(int id);
        Task<IEnumerable<Employee>> GetActiveUsersAsync();
        Task<Employee> GetUserByNameAsync(string name);
    }
}
