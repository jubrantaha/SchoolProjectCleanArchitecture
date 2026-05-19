using SchoolProject.Data.Entities;

namespace SchoolProject.Service.Apstracts
{
    public interface IDepartmentService
    {
        public Task<Department> GetDepartmentById(int id);
        public Task<bool> IsDepartmentIdExist(int departmentId);
    }
}
