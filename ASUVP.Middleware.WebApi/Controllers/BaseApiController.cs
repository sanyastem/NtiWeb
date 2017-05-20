using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using ASUVP.Core.Exceptions;
using ASUVP.Core.Logging;
using AutoMapper;
using Microsoft.AspNet.Identity;

namespace ASUVP.Middleware.WebApi.Controllers
{
    public abstract class BaseApiController<TManager, TEntity, TModel> : ApiController
        where TManager : IEntityManager<TEntity> where TEntity : Entity where TModel : class, new()
    {
        protected readonly IEventLogger Logger;
        protected readonly TManager Manager;
        protected readonly IMapper Mapper;

        protected BaseApiController(TManager manager, IMapper mapper, IEventLogger logger)
        {
            Manager = manager;
            Mapper = mapper;
            Logger = logger;
        }

        protected Guid UserId => new Guid(User.Identity.GetUserId());

        [HttpGet]
        public async Task<IHttpActionResult> Get(Guid id)
        {
            try
            {
                var entity = await Manager.GetByIdAsync(id);
                if (entity == null) throw new EntityNotFoundException();

                var model = Mapper.Map<TEntity, TModel>(entity);
                return Ok(model);
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

        [HttpPost]
        public virtual async Task<IHttpActionResult> Post(TModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await Manager.Create(Mapper.Map<TModel, TEntity>(model), UserId);
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
        public virtual async Task<IHttpActionResult> Put(TModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                await Manager.Update(Mapper.Map<TModel, TEntity>(model), UserId);
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
            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = e.Message
                });
            }
        }

        [HttpDelete]
        public virtual async Task<IHttpActionResult> Delete(Guid id)
        {
            var entity = await Manager.GetByIdAsync(id);
            if (entity == null) return NotFound();

            try
            {
                await Manager.Delete(id, UserId);
                return Ok();
            }

            catch (EntityNotFoundException e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                    ReasonPhrase = e.Message
                });
            }

            catch (Exception e)
            {
                throw new HttpResponseException(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    ReasonPhrase = e.Message
                });
            }
        }
    }
}