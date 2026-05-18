using MediatR;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Queries.Results;


namespace SchoolProject.Core.Features.Students.Queries.Models
{
    public class GetStudentByIDQuery : IRequest<Response<GetSingleStudentResponse>>
    {
        public int id { get; set; }
        public GetStudentByIDQuery(int Id)
        {
            id = Id;
        }
    }
}
