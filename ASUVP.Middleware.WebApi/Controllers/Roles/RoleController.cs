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

namespace ASUVP.Middleware.WebApi.Controllers.Roles
{
    public class RoleController : BaseApiController<IRoleManager, Role, RoleDto>
    {
        public RoleController(IRoleManager manager, IMapper mapper, IEventLogger logger) : base(manager, mapper, logger)
        {
        }

        [HttpPost]
        public override async Task<IHttpActionResult> Post(RoleDto role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var permissions = role.Permissions ?? new List<Guid>();
                await Manager.Create(Mapper.Map<RoleDto, Role>(role), permissions, UserId);

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
        public override async Task<IHttpActionResult> Put(RoleDto role)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var permissions = role.Permissions ?? new List<Guid>();
                await Manager.Update(Mapper.Map<RoleDto, Role>(role), permissions, UserId);

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