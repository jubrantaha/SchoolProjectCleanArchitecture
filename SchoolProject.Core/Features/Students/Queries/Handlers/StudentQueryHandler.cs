using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Queries.Models;
using SchoolProject.Core.Features.Students.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Apstracts;
using System.Linq.Expressions;

namespace SchoolProject.Infrustructure.Features.Students.Queries.Handelrs
{
    public class StudentQueryHandler : ResponseHandler,
                                       IRequestHandler<GetStudentListQuery, Response<List<GetStudentListRespons>>>,
                                       IRequestHandler<GetStudentByIDQuery, Response<GetSingleStudentResponse>>,
                                       IRequestHandler<GetStudentPaginatedListQuery, PaginatedResult<GetStudentPaginatedListResponse>>
    {
        #region Fields
        private readonly IStudentService studentService;
        private readonly IMapper mapper;
        private readonly IStringLocalizer<SharedResources> localizer;
        #endregion

        #region Constraction
        public StudentQueryHandler(IStudentService studentService,
                                   IMapper mapper,
                                   IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            this.studentService = studentService;
            this.mapper = mapper;
            this.localizer = localizer;
        }
        #endregion

        #region Handle Function
        public async Task<Response<List<GetStudentListRespons>>> Handle(GetStudentListQuery request, CancellationToken cancellationToken)
        {
            var studentList = await studentService.GetStudentListAsync();
            var studentListMapper = mapper.Map<List<GetStudentListRespons>>(studentList);
            var result = Success(studentListMapper);
            result.Meta = new { Count = studentListMapper.Count() };
            return result;
        }

        public async Task<Response<GetSingleStudentResponse>> Handle(GetStudentByIDQuery request, CancellationToken cancellationToken)
        {
            var student = await studentService.GetStudentByIdWithIncludeAsync(request.id);
            if (student == null)
                return NotFound<GetSingleStudentResponse>(localizer[SharedResourcesKeys.NotFound]);
            var result = mapper.Map<GetSingleStudentResponse>(student);
            return Success(result);
        }

        public async Task<PaginatedResult<GetStudentPaginatedListResponse>> Handle(GetStudentPaginatedListQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Student, GetStudentPaginatedListResponse>> expression = e => new GetStudentPaginatedListResponse(e.StudID, e.GetLocalized(e.NameAr, e.NameEn), e.Address, e.GetLocalized(e.Department.DNameAr, e.Department.DNameEn));
            //var querable = studentService.GetStudentsQuarable();
            var FilterQuery = studentService.FilterStudentPaginatedQuerable(request.OrederBy, request.Search);
            var paginatedList = await FilterQuery.Select(expression).ToPaginatedListAsync(request.PageNumber, request.PageSize);
            paginatedList.Meta = new { Count = paginatedList.Data.Count() };
            return paginatedList;
        }



        #endregion
    }
}
