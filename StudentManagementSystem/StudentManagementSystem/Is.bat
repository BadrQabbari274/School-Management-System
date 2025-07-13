@echo off
echo Creating Services Structure...
echo.

REM Create main directories
mkdir "Services\Interfaces" 2>nul
mkdir "Services\Implementations" 2>nul

echo Creating Interface files...

REM Create IUserService.cs
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IUserService
echo     {
echo         Task^<IEnumerable^<User^>^> GetAllUsersAsync();
echo         Task^<User^> GetUserByIdAsync(int id);
echo         Task^<User^> CreateUserAsync(User user);
echo         Task^<User^> UpdateUserAsync(User user);
echo         Task^<bool^> DeleteUserAsync(int id);
echo         Task^<IEnumerable^<User^>^> GetActiveUsersAsync();
echo         Task^<User^> GetUserByNameAsync(string name);
echo     }
echo }
) > "Services\Interfaces\IUserService.cs"

REM Create IRoleService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IRoleService
echo     {
echo         Task^<IEnumerable^<Role^>^> GetAllRolesAsync();
echo         Task^<Role^> GetRoleByIdAsync(int id);
echo         Task^<Role^> CreateRoleAsync(Role role);
echo         Task^<Role^> UpdateRoleAsync(Role role);
echo         Task^<bool^> DeleteRoleAsync(int id);
echo         Task^<IEnumerable^<Role^>^> GetActiveRolesAsync();
echo     }
echo }
) > "Services\Interfaces\IRoleService.cs"

REM Create IFieldService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IFieldService
echo     {
echo         Task^<IEnumerable^<Field^>^> GetAllFieldsAsync();
echo         Task^<Field^> GetFieldByIdAsync(int id);
echo         Task^<Field^> CreateFieldAsync(Field field);
echo         Task^<Field^> UpdateFieldAsync(Field field);
echo         Task^<bool^> DeleteFieldAsync(int id);
echo         Task^<IEnumerable^<Field^>^> GetActiveFieldsAsync();
echo         Task^<IEnumerable^<Field^>^> GetFieldsByAcademicYearAsync(int academicYearId);
echo         Task^<bool^> AssignUserToFieldAsync(int userId, int fieldId);
echo         Task^<bool^> RemoveUserFromFieldAsync(int userId, int fieldId);
echo     }
echo }
) > "Services\Interfaces\IFieldService.cs"

REM Create IAcademicYearService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IAcademicYearService
echo     {
echo         Task^<IEnumerable^<AcademicYear^>^> GetAllAcademicYearsAsync();
echo         Task^<AcademicYear^> GetAcademicYearByIdAsync(int id);
echo         Task^<AcademicYear^> CreateAcademicYearAsync(AcademicYear academicYear);
echo         Task^<AcademicYear^> UpdateAcademicYearAsync(AcademicYear academicYear);
echo         Task^<bool^> DeleteAcademicYearAsync(int id);
echo         Task^<IEnumerable^<AcademicYear^>^> GetActiveAcademicYearsAsync();
echo     }
echo }
) > "Services\Interfaces\IAcademicYearService.cs"

REM Create IClassService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IClassService
echo     {
echo         Task^<IEnumerable^<Class^>^> GetAllClassesAsync();
echo         Task^<Class^> GetClassByIdAsync(int id);
echo         Task^<Class^> CreateClassAsync(Class classEntity);
echo         Task^<Class^> UpdateClassAsync(Class classEntity);
echo         Task^<bool^> DeleteClassAsync(int id);
echo         Task^<IEnumerable^<Class^>^> GetActiveClassesAsync();
echo         Task^<IEnumerable^<Class^>^> GetClassesByFieldAsync(int fieldId);
echo     }
echo }
) > "Services\Interfaces\IClassService.cs"

REM Create IStudentService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IStudentService
echo     {
echo         Task^<IEnumerable^<Student^>^> GetAllStudentsAsync();
echo         Task^<Student^> GetStudentByIdAsync(int id);
echo         Task^<Student^> CreateStudentAsync(Student student);
echo         Task^<Student^> UpdateStudentAsync(Student student);
echo         Task^<bool^> DeleteStudentAsync(int id);
echo         Task^<IEnumerable^<Student^>^> GetActiveStudentsAsync();
echo         Task^<IEnumerable^<Student^>^> GetStudentsByClassAsync(int classId);
echo         Task^<IEnumerable^<Student^>^> GetStudentsWithTasksAsync();
echo     }
echo }
) > "Services\Interfaces\IStudentService.cs"

REM Create ICompetenceService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface ICompetenceService
echo     {
echo         Task^<IEnumerable^<Competence^>^> GetAllCompetencesAsync();
echo         Task^<Competence^> GetCompetenceByIdAsync(int id);
echo         Task^<Competence^> CreateCompetenceAsync(Competence competence);
echo         Task^<Competence^> UpdateCompetenceAsync(Competence competence);
echo         Task^<bool^> DeleteCompetenceAsync(int id);
echo         Task^<IEnumerable^<Competence^>^> GetActiveCompetencesAsync();
echo         Task^<IEnumerable^<Competence^>^> GetCompetencesByFieldAsync(int fieldId);
echo     }
echo }
) > "Services\Interfaces\ICompetenceService.cs"

REM Create IOutcomeService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IOutcomeService
echo     {
echo         Task^<IEnumerable^<Outcome^>^> GetAllOutcomesAsync();
echo         Task^<Outcome^> GetOutcomeByIdAsync(int id);
echo         Task^<Outcome^> CreateOutcomeAsync(Outcome outcome);
echo         Task^<Outcome^> UpdateOutcomeAsync(Outcome outcome);
echo         Task^<bool^> DeleteOutcomeAsync(int id);
echo         Task^<IEnumerable^<Outcome^>^> GetActiveOutcomesAsync();
echo         Task^<IEnumerable^<Outcome^>^> GetOutcomesByCompetenceAsync(int competenceId);
echo     }
echo }
) > "Services\Interfaces\IOutcomeService.cs"

REM Create ITaskEvaluationService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface ITaskEvaluationService
echo     {
echo         Task^<IEnumerable^<TaskEvaluation^>^> GetAllTaskEvaluationsAsync();
echo         Task^<TaskEvaluation^> GetTaskEvaluationByIdAsync(int id);
echo         Task^<TaskEvaluation^> CreateTaskEvaluationAsync(TaskEvaluation taskEvaluation);
echo         Task^<TaskEvaluation^> UpdateTaskEvaluationAsync(TaskEvaluation taskEvaluation);
echo         Task^<bool^> DeleteTaskEvaluationAsync(int id);
echo         Task^<IEnumerable^<TaskEvaluation^>^> GetActiveTaskEvaluationsAsync();
echo         Task^<IEnumerable^<TaskEvaluation^>^> GetTaskEvaluationsByStudentAsync(int studentId);
echo         Task^<IEnumerable^<TaskEvaluation^>^> GetTaskEvaluationsByOutcomeAsync(int outcomeId);
echo     }
echo }
) > "Services\Interfaces\ITaskEvaluationService.cs"

REM Create IPictureService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using Microsoft.AspNetCore.Http;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IPictureService
echo     {
echo         Task^<IEnumerable^<Picture^>^> GetAllPicturesAsync();
echo         Task^<Picture^> GetPictureByIdAsync(int id);
echo         Task^<Picture^> CreatePictureAsync(Picture picture);
echo         Task^<Picture^> UpdatePictureAsync(Picture picture);
echo         Task^<bool^> DeletePictureAsync(int id);
echo         Task^<IEnumerable^<Picture^>^> GetActivePicturesAsync();
echo         Task^<IEnumerable^<Picture^>^> GetPicturesByStudentAsync(int studentId);
echo         Task^<IEnumerable^<Picture^>^> GetPicturesByTaskAsync(int taskId);
echo         Task^<string^> SavePictureAsync(IFormFile file, int? studentId, int? taskId);
echo         Task^<bool^> DeletePictureFileAsync(string filePath);
echo     }
echo }
) > "Services\Interfaces\IPictureService.cs"

REM Create IAttendanceService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IAttendanceService
echo     {
echo         Task^<IEnumerable^<MajorAttendance^>^> GetAllMajorAttendancesAsync();
echo         Task^<MajorAttendance^> GetMajorAttendanceByIdAsync(int id);
echo         Task^<MajorAttendance^> CreateMajorAttendanceAsync(MajorAttendance attendance);
echo         Task^<MajorAttendance^> UpdateMajorAttendanceAsync(MajorAttendance attendance);
echo         Task^<bool^> DeleteMajorAttendanceAsync(int id);
echo         Task^<IEnumerable^<MajorAttendance^>^> GetMajorAttendancesByStudentAsync(int studentId);
echo         Task^<IEnumerable^<MajorAttendance^>^> GetMajorAttendancesByDateAsync(DateTime date);
echo         
echo         Task^<IEnumerable^<StudentAttendance^>^> GetAllStudentAttendancesAsync();
echo         Task^<StudentAttendance^> GetStudentAttendanceByIdAsync(int id);
echo         Task^<StudentAttendance^> CreateStudentAttendanceAsync(StudentAttendance attendance);
echo         Task^<StudentAttendance^> UpdateStudentAttendanceAsync(StudentAttendance attendance);
echo         Task^<bool^> DeleteStudentAttendanceAsync(int id);
echo         Task^<IEnumerable^<StudentAttendance^>^> GetStudentAttendancesByStudentAsync(int studentId);
echo     }
echo }
) > "Services\Interfaces\IAttendanceService.cs"

REM Create IAbsenceReasonService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IAbsenceReasonService
echo     {
echo         Task^<IEnumerable^<AbsenceReason^>^> GetAllAbsenceReasonsAsync();
echo         Task^<AbsenceReason^> GetAbsenceReasonByIdAsync(int id);
echo         Task^<AbsenceReason^> CreateAbsenceReasonAsync(AbsenceReason absenceReason);
echo         Task^<AbsenceReason^> UpdateAbsenceReasonAsync(AbsenceReason absenceReason);
echo         Task^<bool^> DeleteAbsenceReasonAsync(int id);
echo         Task^<IEnumerable^<AbsenceReason^>^> GetActiveAbsenceReasonsAsync();
echo     }
echo }
) > "Services\Interfaces\IAbsenceReasonService.cs"

REM Create IStudentManagementService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Threading.Tasks;
echo using Microsoft.AspNetCore.Http;
echo using StudentManagementSystem.Models;
echo.
echo namespace StudentManagementSystem.Services.Interfaces
echo {
echo     public interface IStudentManagementService
echo     {
echo         Task^<IEnumerable^<Student^>^> GetStudentsWithCompleteDataAsync();
echo         Task^<IEnumerable^<TaskEvaluation^>^> GetTaskEvaluationsWithPicturesAsync(int studentId);
echo         Task^<bool^> SubmitTaskWithPicturesAsync(int taskId, int studentId, IList^<IFormFile^> pictures, int createdBy);
echo         Task^<IEnumerable^<Student^>^> GetStudentsByFieldAsync(int fieldId);
echo         Task^<Dictionary^<string, object^>^> GetStudentDashboardDataAsync(int studentId);
echo         Task^<Dictionary^<string, object^>^> GetFieldStatisticsAsync(int fieldId);
echo         Task^<bool^> BulkUpdateAttendanceAsync(List^<StudentAttendance^> attendances);
echo     }
echo }
) > "Services\Interfaces\IStudentManagementService.cs"

echo Interface files created successfully!
echo.
echo Creating Implementation files...

REM Create UserService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Linq;
echo using System.Threading.Tasks;
echo using Microsoft.EntityFrameworkCore;
echo using StudentManagementSystem.Models;
echo using StudentManagementSystem.Services.Interfaces;
echo.
echo namespace StudentManagementSystem.Services.Implementations
echo {
echo     public class UserService : IUserService
echo     {
echo         private readonly ApplicationDbContext _context;
echo.
echo         public UserService(ApplicationDbContext context)
echo         {
echo             _context = context;
echo         }
echo.
echo         public async Task^<IEnumerable^<User^>^> GetAllUsersAsync()
echo         {
echo             return await _context.Users
echo                 .Include(u =^> u.Role)
echo                 .Include(u =^> u.CreatedByUser)
echo                 .ToListAsync();
echo         }
echo.
echo         public async Task^<User^> GetUserByIdAsync(int id)
echo         {
echo             return await _context.Users
echo                 .Include(u =^> u.Role)
echo                 .Include(u =^> u.CreatedByUser)
echo                 .FirstOrDefaultAsync(u =^> u.Id == id);
echo         }
echo.
echo         public async Task^<User^> CreateUserAsync(User user)
echo         {
echo             user.Date = DateTime.Now;
echo             _context.Users.Add(user);
echo             await _context.SaveChangesAsync();
echo             return user;
echo         }
echo.
echo         public async Task^<User^> UpdateUserAsync(User user)
echo         {
echo             _context.Entry(user).State = EntityState.Modified;
echo             await _context.SaveChangesAsync();
echo             return user;
echo         }
echo.
echo         public async Task^<bool^> DeleteUserAsync(int id)
echo         {
echo             var user = await _context.Users.FindAsync(id);
echo             if (user == null) return false;
echo             user.IsActive = false;
echo             await _context.SaveChangesAsync();
echo             return true;
echo         }
echo.
echo         public async Task^<IEnumerable^<User^>^> GetActiveUsersAsync()
echo         {
echo             return await _context.Users
echo                 .Where(u =^> u.IsActive)
echo                 .Include(u =^> u.Role)
echo                 .ToListAsync();
echo         }
echo.
echo         public async Task^<User^> GetUserByNameAsync(string name)
echo         {
echo             return await _context.Users
echo                 .Include(u =^> u.Role)
echo                 .FirstOrDefaultAsync(u =^> u.Name == name);
echo         }
echo     }
echo }
) > "Services\Implementations\UserService.cs"

REM Create RoleService.cs
(
echo using System;
echo using System.Collections.Generic;
echo using System.Linq;
echo using System.Threading.Tasks;
echo using Microsoft.EntityFrameworkCore;
echo using StudentManagementSystem.Models;
echo using StudentManagementSystem.Services.Interfaces;
echo.
echo namespace StudentManagementSystem.Services.Implementations
echo {
echo     public class RoleService : IRoleService
echo     {
echo         private readonly ApplicationDbContext _context;
echo.
echo         public RoleService(ApplicationDbContext context)
echo         {
echo             _context = context;
echo         }
echo.
echo         public async Task^<IEnumerable^<Role^>^> GetAllRolesAsync()
echo         {
echo             return await _context.Roles
echo                 .Include(r =^> r.CreatedByUser)
echo                 .Where(r =^> !r.IsDeleted)
echo                 .ToListAsync();
echo         }
echo.
echo         public async Task^<Role^> GetRoleByIdAsync(int id)
echo         {
echo             return await _context.Roles
echo                 .Include(r =^> r.CreatedByUser)
echo                 .FirstOrDefaultAsync(r =^> r.Id == id ^&^& !r.IsDeleted);
echo         }
echo.
echo         public async Task^<Role^> CreateRoleAsync(Role role)
echo         {
echo             role.CreatedDate = DateTime.Now;
echo             _context.Roles.Add(role);
echo             await _context.SaveChangesAsync();
echo             return role;
echo         }
echo.
echo         public async Task^<Role^> UpdateRoleAsync(Role role)
echo         {
echo             _context.Entry(role).State = EntityState.Modified;
echo             await _context.SaveChangesAsync();
echo             return role;
echo         }
echo.
echo         public async Task^<bool^> DeleteRoleAsync(int id)
echo         {
echo             var role = await _context.Roles.FindAsync(id);
echo             if (role == null) return false;
echo             role.IsDeleted = true;
echo             await _context.SaveChangesAsync();
echo             return true;
echo         }
echo.
echo         public async Task^<IEnumerable^<Role^>^> GetActiveRolesAsync()
echo         {
echo             return await _context.Roles
echo                 .Where(r =^> !r.IsDeleted)
echo                 .ToListAsync();
echo         }
echo     }
echo }
) > "Services\Implementations\RoleService.cs"

echo Implementation files created successfully!
echo.
echo Services structure created successfully!
echo - Services\Interfaces\ (contains all interface files)
echo - Services\Implementations\ (contains all implementation files)
echo.
echo Don't forget to update your Program.cs or Startup.cs to register the services:
echo builder.Services.AddScoped^<IUserService, UserService^>();
echo builder.Services.AddScoped^<IRoleService, RoleService^>();
echo And so on for all services...
echo.
pause