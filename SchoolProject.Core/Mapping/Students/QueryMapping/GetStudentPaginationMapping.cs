using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Data.Entities;

namespace SchoolProject.Core.Mapping.Students
{
    public partial class StudentProfile
    {
        public void GetStudentPaginationMapping()
        {
            CreateMap<Student, GetStudentPaginatedListResponse>()
                .ForMember(des => des.DepartmentName, opt => opt.MapFrom(src => src.GetLocalized(src.Department.DNameAr, src.Department.DNameEn)))
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.GetLocalized(src.NameAr, src.NameEn)))
                .ForMember(des => des.StudID, opt => opt.MapFrom(src => src.StudID))
                .ForMember(des => des.Address, opt => opt.MapFrom(src => src.Address));
        }
    }
}
