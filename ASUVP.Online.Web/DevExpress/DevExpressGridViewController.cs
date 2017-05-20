using System.Linq;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress
{
    public abstract class DevExpressGridViewController : Controller
    {
        #region Constructor

        protected DevExpressGridViewController(IQueryable query)
        {
            QueryModel = new DevExpressGridViewQueryModel(query);
        }

        #endregion

        #region Properties

        /// <summary>
        ///     represents cotroller name
        /// </summary>
        protected abstract string GridViewName { get; }

        /// <summary>
        ///     represents query model for grid view component
        /// </summary>
        protected DevExpressGridViewQueryModel QueryModel { get; set; }

        #endregion

        #region Action Methods

        /// <summary>
        ///     returns page for grid view component
        /// </summary>
        /// <returns></returns>
        //public abstract ActionResult Index();
        public abstract ActionResult Index(string filter = null);

        /// <summary>
        ///     returns partial grid view component
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GridView()
        {
            var model = GridViewExtension.GetViewModel(GridViewName) ?? InitializeGridViewModel();
            return GridViewActionCore(model);
        }

        /// <summary>
        ///     pagination action for grid view component
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public virtual ActionResult Paging(GridViewPagerState pager)
        {
            var model = GridViewExtension.GetViewModel(GridViewName);
            model.ApplyPagingState(pager);
            return GridViewActionCore(model);
        }

        /// <summary>
        ///     sorting action for grid view component
        /// </summary>
        /// <param name="column"></param>
        /// <param name="reset"></param>
        /// <returns></returns>
        public virtual ActionResult Sorting(GridViewColumnState column, bool reset)
        {
            var model = GridViewExtension.GetViewModel(GridViewName);
            model.SortBy(column, reset);
            return GridViewActionCore(model);
        }


        /// <summary>
        ///     grouping action for grid view component
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        public virtual ActionResult Grouping(GridViewColumnState column)
        {
            var model = GridViewExtension.GetViewModel(GridViewName);
            model.ApplyGroupingState(column);
            return GridViewActionCore(model);
        }

        /// <summary>
        ///     filtering action for grid view component
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public virtual ActionResult Filtering(GridViewFilteringState state)
        {
            var model = GridViewExtension.GetViewModel(GridViewName);
            model.ApplyFilteringState(state);
            return GridViewActionCore(model);
        }


        /// <summary>
        ///     custom binding action for grid view component
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual ActionResult GridViewActionCore(GridViewModel model)
        {
            model.ProcessCustomBinding(QueryModel.GetCount, QueryModel.GetData, QueryModel.GetSummaryValues,
                QueryModel.GetGroupingInfo, QueryModel.GetUniqueHeaderFilterValues);

            return PartialView(GridViewName, model);
        }

        /// <summary>
        ///     returns partial grid view component
        /// </summary>
        /// <returns></returns>
        public virtual ActionResult GridViewResult()
        {
            return GridViewActionCore(GridViewExtension.GetViewModel(GridViewName));
        }

        #endregion

        #region Helper Methods

        /// <summary>
        ///     initializes grid view model with parameters
        /// </summary>
        /// <returns></returns>
        public abstract DevExpressGridViewModel InitializeGridViewModel();

        public ViewResult GridViewIndex(DevExpressGridViewIndexSettings model)
        {
            return View("DevExpress/GridViewIndex", model);
        }

        #endregion
    }
}