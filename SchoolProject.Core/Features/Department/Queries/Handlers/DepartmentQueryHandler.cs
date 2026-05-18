using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Department.Queries.Models;
using SchoolProject.Core.Features.Department.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Apstracts;
using System.Linq.Expressions;

namespace SchoolProject.Core.Features.Department.Queries.Handlers
{
    public class DepartmentQueryHandler : ResponseHandler,
                                          IRequestHandler<GetDepartmentByIDQuery, Response<GetDepartmentByIDResponse>>
    {
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        #region Fields
        private readonly IDepartmentService departmentService;
        private readonly IMapper mapper;
        private readonly IStudentService studentService;
        #endregion


        #region Constructors
        public DepartmentQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                      IDepartmentService departmentService,
                                      IMapper mapper,
                                      IStudentService studentService) : base(stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            this.departmentService = departmentService;
            this.mapper = mapper;
            this.studentService = studentService;
        }
        #endregion


        #region Handle Functions
        public async Task<Response<GetDepartmentByIDResponse>> Handle(GetDepartmentByIDQuery request, CancellationToken cancellationToken)
        {
            // Get By Id include st sub ins
            var response = await departmentService.GetDepartmentById(request.Id);
            // Check Is Not Exist
            if (response == null)
                return NotFound<GetDepartmentByIDResponse>(stringLocalizer[SharedResourcesKeys.NotFound]);
            // Mapping
            var deptmapper = mapper.Map<GetDepartmentByIDResponse>(response);

            Expression<Func<Student, StudentResponse>> expression = e => new StudentResponse(e.StudID, e.GetLocalized(e.NameAr, e.NameEn));
            var studentQeryable = studentService.GetStudentsByDepartmentIdQuarable(request.Id);
            var paginatedList = await studentQeryable.Select(expression).ToPaginatedListAsync(request.StudentPageNumber, request.StudentPageSize);
            deptmapper.StudentList = paginatedList;
            // return response
            return Success(deptmapper);
        }
        #endregion

    }
}
