using System.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress
{
    public class DevExpressGridViewQueryModel
    {
        public DevExpressGridViewQueryModel(IQueryable query, string filterExpression = "")
        {
            Query = query;
            FilterExpression = filterExpression;
        }

        public IQueryable Query { get; set; }
        private string FilterExpression { get; }

        public void GetCount(GridViewCustomBindingGetDataRowCountArgs e)
        {
                e.DataRowCount = Query.ApplyFilter(e.FilterExpression).Count();
        }

        public void GetData(GridViewCustomBindingGetDataArgs e)
        {
                e.Data = Query
                .ApplySorting(e.State.SortedColumns)
                .ApplyFilter(e.FilterExpression)
                .ApplyFilter(e.GroupInfoList)
                .ApplyFilter(FilterExpression)
                .Skip(e.StartDataRowIndex)
                .Take(e.DataRowCount);
        }

        public void GeExportData(GridViewCustomBindingGetDataArgs e)
        {
                e.Data = Query
               .ApplyFilter(e.FilterExpression)
               .ApplyFilter(e.GroupInfoList)
               .ApplySorting(e.State.SortedColumns);
        }

        public void GetGroupingInfo(GridViewCustomBindingGetGroupingInfoArgs e)
        {
                e.Data = Query
          .ApplyFilter(e.FilterExpression)
          .ApplyFilter(e.GroupInfoList)
          .GetGroupInfo(e.FieldName, e.SortOrder);
        }

        public void GetSummaryValues(GridViewCustomBindingGetSummaryValuesArgs e)
        {
                var query = Query.ApplyFilter(e.FilterExpression).ApplyFilter(e.GroupInfoList);
                var summaryValues = query.CalculateSummary(e.SummaryItems);

                e.Data = summaryValues;
        }

        public void GetUniqueHeaderFilterValues(GridViewCustomBindingGetUniqueHeaderFilterValuesArgs e)
        {
                var list = Query.ConvertToGenericList();
                e.Data = list.AsQueryable().ApplyFilter(e.FilterExpression).UniqueValuesForField(e.FieldName);
        }
    }
}