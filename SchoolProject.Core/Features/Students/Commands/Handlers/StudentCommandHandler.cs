using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.Students.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities;
using SchoolProject.Service.Apstracts;

namespace SchoolProject.Core.Features.Students.Commands.Handlers
{
    internal class StudentCommandHandler : ResponseHandler, IRequestHandler<AddStudentCommand, Response<string>>
                                                          , IRequestHandler<EditStudentCommand, Response<string>>
                                                          , IRequestHandler<DeleteStudentCommand, Response<string>>
    {
        private readonly IStudentService studentService;
        private readonly IMapper mapping;
        private readonly IStringLocalizer<SharedResources> localizer;

        public StudentCommandHandler(IStudentService studentService,
                                     IMapper mapping,
                                     IStringLocalizer<SharedResources> localizer) : base(localizer)
        {
            this.studentService = studentService;
            this.mapping = mapping;
            this.localizer = localizer;
        }
        public async Task<Response<string>> Handle(AddStudentCommand request, CancellationToken cancellationToken)
        {
            //mapping Between request 
            var studentMapper = mapping.Map<Student>(request);
            //Add
            var result = await studentService.AddAsync(studentMapper);
            if (result == "Success") return Created("");
            else return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(EditStudentCommand request, CancellationToken cancellationToken)
        {
            // Check The id is Exit  or Not
            var student = await studentService.GetByIdAsync(request.Id);
            // Return  Not Found
            if (student == null)
                return NotFound<string>("student is not found");
            // Mapping Bettwen request and student
            var studentMapper = mapping.Map(request, student);
            // Call Service that Make Edit
            var result = await studentService.EditAsync(studentMapper);
            //return response
            if (result == "Success")
                return Success($"Edit Successfully {studentMapper.StudID}");
            else
                return BadRequest<string>();
        }

        public async Task<Response<string>> Handle(DeleteStudentCommand request, CancellationToken cancellationToken)
        {
            // Check The id is Exit  or Not
            var student = await studentService.GetByIdAsync(request.Id);
            // Return  Not Found
            if (student == null)
                return NotFound<string>("student is not found");
            // Call Service that Make Delete
            var result = await studentService.DeleteAsync(student);
            //return response
            if (result == "Success")
                return Deleted<string>($"Delete Successfuly {request.Id}");
            else
                return BadRequest<string>();
        }
    }
}
