using System.Linq;
using System.Web.Http;
using System.Web.OData;
using ASUVP.Core.Domain.Entities;
using ASUVP.Core.Domain.Managers;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace ASUVP.Middleware.WebApi.Controllers
{
    [EnableQuery]
    public abstract class BaseODataController<TManager, TEntity, TModel> : ODataController
        where TManager : IEntityManager<TEntity> where TEntity : Entity where TModel : class, new()
    {
        protected readonly TManager Manager;
        protected readonly IMapper Mapper;

        protected BaseODataController(TManager manager, IMapper mapper)
        {
            Manager = manager;
            Mapper = mapper;
        }

        [HttpGet]
        public virtual IQueryable<TModel> Get()
        {
            return Manager.GetQueryActive().ProjectTo<TModel>(Mapper.ConfigurationProvider);
        }
    }
}