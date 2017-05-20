using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Exceptions;
using ASUVP.Core.Logging;
using ASUVP.Core.Web.Dto;
using AutoMapper;

namespace ASUVP.Middleware.WebApi.Controllers.Users
{
    public class UserController : BaseApiController<IUserManager, User, UserDto>
    {
        public UserController(IUserManager manager, IMapper mapper, IEventLogger logger) : base(manager, mapper, logger)
        {
        }

        [HttpPost]
        public override async Task<IHttpActionResult> Post(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var companies = user.Companies ?? new List<Guid>();
                await Manager.Create(Mapper.Map<UserDto, User>(user), user.Password, user.ContactId, companies, UserId);
                return Ok();
            }

            catch (EntityAlreadyExistsException e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = e.Message
                });
            }
        }

        [HttpPut]
        public override async Task<IHttpActionResult> Put(UserDto user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var companies = user.Companies ?? new List<Guid>();
                await Manager.Update(Mapper.Map<UserDto, User>(user), user.ContactId, companies, UserId);
                return Ok();
            }

            catch (EntityAlreadyExistsException e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = e.Message
                });
            }

            catch (EntityNotFoundException e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = e.Message
                });
            }
        }
    }
}