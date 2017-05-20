using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using ASUVP.Core.Business.Identity;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Web.Dto;
using AutoMapper;

namespace ASUVP.Middleware.WebApi.Controllers.Account
{
    [RoutePrefix("api/useraccount")]
    public class AccountController : ApiController
    {
        private readonly IdentityManager _manager;
        private readonly IMapper _mapper;

        public AccountController(IdentityManager manager, IMapper mapper)
        {
            _manager = manager;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("companies")]
        public async Task<IHttpActionResult> Companies()
        {
            var entity = await _manager.FindByNameAsync(User.Identity.Name);
            if (entity == null)
            {
                return NotFound();
            }

            var model = _mapper.Map<List<Company>, List<CompanyDto>>(entity.UserCompanies());
            return Ok(model);
        }

        [HttpGet]
        [Route("info")]
        public async Task<IHttpActionResult> Info(Guid companyId)
        {
            var entity = await _manager.FindByNameAsync(User.Identity.Name);
            if (entity == null)
            {
                return NotFound();
            }

            var userinfo = new UserInfoDto
            {
                Id = entity.Id,
                UserName = entity.UserName,
                FullName = entity.FullName(),
                CompanyName = entity.CompanyName(companyId),
                Permissions = entity.EmployeePermissions(companyId)
            };

            return Ok(userinfo);
        }
    }
}