using Microsoft.AspNetCore.Mvc;
using SchoolProject.Api.Base;
using SchoolProject.Core.Features.Department.Queries.Models;
using SchoolProject.Data.AppMetaData;

namespace SchoolProject.Api.Controllers
{
    [ApiController]
    public class DepartmentController : AppControllerBase
    {

        //[HttpGet(Router.DepartmentRouter.GetByID)]
        //public async Task<IActionResult> GetDepartmentByID([FromRoute] int id)
        //{
        //    return NewResult(await Mediator.Send(new GetDepartmentByIDQuery(id)));
        //}

        [HttpGet(Router.DepartmentRouter.GetByID)]
        public async Task<IActionResult> GetDepartmentByID([FromQuery] GetDepartmentByIDQuery query)
        {
            return NewResult(await Mediator.Send(query));
        }
    }
}
