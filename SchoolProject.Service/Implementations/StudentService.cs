using Microsoft.EntityFrameworkCore;
using SchoolProject.Data.Entities;
using SchoolProject.Data.Helpers;
using SchoolProject.Infrustructure.Abstracts;
using SchoolProject.Service.Apstracts;

namespace SchoolProject.Service.Implementations
{
    public class StudentService : IStudentService
    {
        #region Fileds
        private readonly IStudentRepository studentRepository;
        #endregion

        #region Constructors
        public StudentService(IStudentRepository studentRepository)
        {
            this.studentRepository = studentRepository;
        }
        #endregion

        #region Handlers Function
        public async Task<List<Student>> GetStudentListAsync()
        {
            return await studentRepository.GetStudentsListAsync();
        }

        public async Task<Student> GetStudentByIdWithIncludeAsync(int id)
        {
            //var student = studentRepository.GetByIdAsync(id);
            //return student;

            var student = await studentRepository.GetTableNoTracking()
                                           .Include(x => x.Department)
                                           .Where(x => x.StudID.Equals(id))
                                           .FirstOrDefaultAsync();
            return student;
        }

        public async Task<string> AddAsync(Student student)
        {
            //Added Student
            await studentRepository.AddAsync(student);
            return "Success";
        }

        public async Task<bool> IsNameArExist(string nameAr)
        {
            var student = studentRepository.GetTableNoTracking()
                                           .Where(x => x.NameAr.Equals(nameAr))
                                           .FirstOrDefault();
            if (student == null)
                return false;
            return true;
        }

        public async Task<bool> IsNameEnExist(string nameEn)
        {
            var student = studentRepository.GetTableNoTracking()
                                           .Where(x => x.NameEn.Equals(nameEn))
                                           .FirstOrDefault();
            if (student == null)
                return false;
            return true;
        }

        public async Task<bool> IsNameArExistExcludeSelf(string nameAr, int id)
        {
            var student = studentRepository.GetTableNoTracking()
                                            .Where(x => x.NameAr.Equals(nameAr) & !x.StudID.Equals(id))
                                            .FirstOrDefault();
            if (student == null) return false;
            return true;
        }

        public async Task<bool> IsNameEnExistExcludeSelf(string nameEn, int id)
        {
            var student = studentRepository.GetTableNoTracking()
                                           .Where(x => x.NameEn.Equals(nameEn) & !x.StudID.Equals(id))
                                           .FirstOrDefault();
            if (student == null)
                return false;
            return true;
        }

        public async Task<string> EditAsync(Student student)
        {
            await studentRepository.UpdateAsync(student);
            return "Success";
        }

        public async Task<string> DeleteAsync(Student student)
        {
            var trans = studentRepository.BeginTransaction();
            try
            {
                await studentRepository.DeleteAsync(student);
                await trans.CommitAsync();
                return "Success";
            }
            catch
            {
                await trans.RollbackAsync();
                return "Falied";
            }
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            var student = await studentRepository.GetByIdAsync(id);
            return student;
        }

        public IQueryable<Student> GetStudentsQuarable()
        {
            return studentRepository.GetTableNoTracking().Include(x => x.Department).AsQueryable();
        }

        public IQueryable<Student> FilterStudentPaginatedQuerable(StudentOrderingEnum orderingEnum, string search)
        {
            var querable = studentRepository.GetTableNoTracking().Include(x => x.Department).AsQueryable();
            if (search != null)
            {
                querable = querable.Where(x => x.NameAr.Contains(search) || x.Address.Contains(search));
            }

            switch (orderingEnum)
            {
                case StudentOrderingEnum.StudID:
                    querable = querable.OrderBy(x => x.StudID);
                    break;

                case StudentOrderingEnum.Name:
                    querable = querable.OrderBy(x => x.NameAr);
                    break;

                case StudentOrderingEnum.Address:
                    querable = querable.OrderBy(x => x.Address);
                    break;

                case StudentOrderingEnum.DepartmentName:
                    querable = querable.OrderBy(x => x.Department.DNameAr);
                    break;

                default:
                    querable = querable.OrderBy(x => x.StudID);
                    break;
            }
            return querable;
        }

        public IQueryable<Student> GetStudentsByDepartmentIdQuarable(int DID)
        {
            return studentRepository.GetTableNoTracking()
                                    .Where(x => x.DID.Equals(DID))
                                    .AsQueryable();
        }

        #endregion
    }
}
