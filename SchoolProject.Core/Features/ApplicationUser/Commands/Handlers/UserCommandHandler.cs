using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Handlers
{
    public class UserCommandHandler : ResponseHandler,
                                      IRequestHandler<AddUserCommand, Response<string>>,
                                      IRequestHandler<EditUserCommand, Response<string>>
    {
        #region Fields
        private readonly IStringLocalizer<SharedResources> stringLocalizer;
        private readonly IMapper mapper;
        private readonly UserManager<User> userManager;

        #endregion

        #region Constructors
        public UserCommandHandler(IStringLocalizer<SharedResources> stringLocalizer,
                                  IMapper mapper,
                                  UserManager<User> userManager) : base(stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
            this.mapper = mapper;
            this.userManager = userManager;
        }
        #endregion

        #region Handle Functions
        public async Task<Response<string>> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            // If Email is Exist
            var user = await userManager.FindByEmailAsync(request.Email);
            // Email is Exist
            if (user != null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.EmailIsExist]);
            // If UserName is Exist
            var userByUserName = await userManager.FindByNameAsync(request.UserName);
            // UserName is Exist
            if (userByUserName != null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UserNameIsExist]);
            // Mapping
            var identityUser = mapper.Map<User>(request);
            // Create
            var createResult = await userManager.CreateAsync(identityUser, request.Password);
            // Create = Failed => message
            if (!createResult.Succeeded) return BadRequest<string>(createResult.Errors.FirstOrDefault().Description);
            // Create = Success => message
            return Created("");
        }

        public async Task<Response<string>> Handle(EditUserCommand request, CancellationToken cancellationToken)
        {
            // Check if User is Exist
            var oldUser = await userManager.FindByIdAsync(request.Id.ToString());
            // if User Not Exist NotFound
            if (oldUser == null) return NotFound<string>();
            // Mapping
            var newUser = mapper.Map(request, oldUser);
            // Update
            var result = await userManager.UpdateAsync(newUser);
            // Create = Failed => message
            if (!result.Succeeded) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UpdateFailed]);
            // Create = Success => message
            return Success((string)stringLocalizer[SharedResourcesKeys.Updated]);
        }
        #endregion
    }
}
