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

namespace ASUVP.Middleware.WebApi.Controllers.Employees
{
    public class EmployeeController : BaseApiController<IEmployeeManager, Employee, EmployeeDto>
    {
        public EmployeeController(IEmployeeManager manager, IMapper mapper, IEventLogger logger)
            : base(manager, mapper, logger)
        {
        }

        [HttpPut]
        public override async Task<IHttpActionResult> Put(EmployeeDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var roles = model.Roles ?? new List<Guid>();
                await Manager.Update(Mapper.Map<EmployeeDto, Employee>(model), roles, UserId);
                return Ok(model);
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