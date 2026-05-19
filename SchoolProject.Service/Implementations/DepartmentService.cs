using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Service.Apstracts;

namespace SchoolProject.Service.Implementations
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository departmentRepository;
        #region Fields
        #endregion

        #region Constructor
        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            this.departmentRepository = departmentRepository;
        }
        #endregion

        #region Handle Functions
        public async Task<Department> GetDepartmentById(int id)
        {
            var department = departmentRepository.GetTableNoTracking()
                                                    .Where(x => x.DID.Equals(id))
                                                    .Include(x => x.DepartmentSubjects).ThenInclude(x => x.Subject)
                                                    .Include(x => x.Instructors)
                                                    .Include(x => x.Instructor)
                                                    .FirstOrDefault();
            return department;
        }

        public async Task<bool> IsDepartmentIdExist(int departmentId)
        {
            return await departmentRepository.GetTableNoTracking().AnyAsync(x => x.DID.Equals(departmentId));
        }
        #endregion
    }
}
