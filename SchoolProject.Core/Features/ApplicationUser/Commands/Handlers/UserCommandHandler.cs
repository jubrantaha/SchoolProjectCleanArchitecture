using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using SchoolProject.Core.Bases;
using SchoolProject.Core.Features.ApplicationUser.Commands.Models;
using SchoolProject.Core.Resources;
using SchoolProject.Data.Entities.Identity;

namespace SchoolProject.Core.Features.ApplicationUser.Commands.Handlers
{
    public class UserCommandHandler : ResponseHandler,
                                      IRequestHandler<AddUserCommand, Response<string>>,
                                      IRequestHandler<EditUserCommand, Response<string>>,
                                      IRequestHandler<DeleteUserCommand, Response<string>>,
                                      IRequestHandler<ChangeUserPasswordCommand, Response<string>>
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
            // If UserName is Exist
            var userByUserName = await userManager.Users.FirstOrDefaultAsync(x => x.UserName == newUser.UserName && x.Id != newUser.Id);
            // UserName is Exist
            if (userByUserName != null) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UserNameIsExist]);
            // Update
            var result = await userManager.UpdateAsync(newUser);
            // Create = Failed => message
            if (!result.Succeeded) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.UpdateFailed]);
            // Create = Success => message
            return Success((string)stringLocalizer[SharedResourcesKeys.Updated]);
        }

        public async Task<Response<string>> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            // Check if User is Exist
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            // if User Not Exist NotFound
            if (user == null) return NotFound<string>();
            // Call Service that Make Delete
            var result = await userManager.DeleteAsync(user);
            // Deleted = Failed => message
            if (!result.Succeeded) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.DeletedFailed]);
            // Deleted = Success => message
            return Success((string)stringLocalizer[SharedResourcesKeys.Deleted]);

        }

        public async Task<Response<string>> Handle(ChangeUserPasswordCommand request, CancellationToken cancellationToken)
        {
            // Get User
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            // if Not Exist return NotFound
            if (user == null) return NotFound<string>();
            // Change User Password 


            //var passUser = await userManager.HasPasswordAsync(user);
            //await userManager.RemovePasswordAsync(user);
            //var result2 = await userManager.AddPasswordAsync(user, request.NewPassword);
            //// Filed => message
            //if (!result2.Succeeded) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.ChangePasswordFailed] + ":" + result2.Errors.FirstOrDefault().Description);
            //// Success => message
            //return Success((string)stringLocalizer[SharedResourcesKeys.ChangePasswordSuccess]);

            var result1 = await userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
            // Failed => message
            if (!result1.Succeeded) return BadRequest<string>(stringLocalizer[SharedResourcesKeys.ChangePasswordFailed] + ":" + result1.Errors.FirstOrDefault().Description);
            // Success => message
            return Success((string)stringLocalizer[SharedResourcesKeys.ChangePasswordSuccess]);



        }
        #endregion
    }
}
