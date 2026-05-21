using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Queries.Models;
using SchoolProject.Core.Features.ApplicationUser.Queries.Results;
using SchoolProject.Core.Resources;
using SchoolProject.Core.Wrappers;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Queries.Handlers
{
    internal class UserQueryHandler : ResponseHandler,
                                      IRequestHandler<GetUserPaginationQuery, PaginatedResult<GetUserPaginationReponse>>,
                                      IRequestHandler<GetUserByIdQuery, Response<GetUserByIdResponse>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;
        #endregion

        #region Constructors
        public UserQueryHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                IMapper mapper,
                                UserManager<User> userManager) : base(stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        #endregion


        #region Handle Functions
        public async Task<PaginatedResult<GetUserPaginationReponse>> Handle(GetUserPaginationQuery request, CancellationToken cancellationToken)
        {
            var user = userManager.Users.AsQueryable();
            var paginationList = await mapper.ProjectTo<GetUserPaginationReponse>(user)
                                             .ToPaginatedListAsync(request.PageNumber, request.PageSize);
            return paginationList;
        }

        public async Task<Response<GetUserByIdResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            //var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id);
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user == null) return NotFound<GetUserByIdResponse>(stringLocalizer[SharedResourcesKeys.NotFound]);
            var result = mapper.Map<GetUserByIdResponse>(user);
            return Success(result);
        }
        #endregion
    }
}
