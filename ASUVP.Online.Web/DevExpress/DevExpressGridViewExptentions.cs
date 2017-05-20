using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.Data.Linq;
using DevExpress.Data.Linq.Helpers;
using DevExpress.Data.ODataLinq.Helpers;
using DevExpress.Web.Mvc;

namespace ASUVP.Online.Web.DevExpress
{
    public static class DevExpressGridViewExptentions
    {
        private static ICriteriaToExpressionConverter Converter => new CriteriaToExpressionConverter();

        public static IQueryable Select(this IQueryable query, string fieldName)
        {
            return query.MakeSelect(Converter, new OperandProperty(fieldName));
        }

        public static IQueryable ApplySorting(this IQueryable query, IEnumerable<GridViewColumnState> sortedColumns)
        {
            return sortedColumns.Aggregate(query,
                (current, column) => ApplySorting(current, column.FieldName, column.SortOrder));
        }

        public static IQueryable ApplySorting(this IQueryable query, string fieldName, ColumnSortOrder order)
        {
            return query.MakeOrderBy(Converter,
                new ServerModeOrderDescriptor(new OperandProperty(fieldName), order == ColumnSortOrder.Descending));
        }

        public static IQueryable ApplyFilter(this IQueryable query, string filterExpression)
        {
            var parameters = new Collection<object>();

            const string pattern = @"\[(?<propertyName>\w+)\] [=<>]{1,2} #(?<value>[\w-/.]+)#";
            foreach (Match match in Regex.Matches(filterExpression, pattern, RegexOptions.IgnoreCase))
            {
                var propertyName = match.Groups["propertyName"].Value;
                var valueStr = match.Groups["value"].Value;

                DateTimeOffset value;
                if (DateTimeOffset.TryParse(valueStr, out value))
                {
                    parameters.Add(value);
                }

                var condition = match.Value.Replace(propertyName, propertyName.Replace("Unbound", string.Empty))
                    .Replace($"#{valueStr}#", "?");

                filterExpression = filterExpression.Replace(match.Value, condition);
            }
            return query.AppendWhere(Converter, CriteriaOperator.Parse(filterExpression, parameters.ToArray()));
        }

        public static IQueryable ApplyFilter(this IQueryable query, IList<GridViewGroupInfo> groupInfoList)
        {
            var criteria =
                CriteriaOperator.And(
                    groupInfoList.Select(i => new BinaryOperator(i.FieldName, i.KeyValue, BinaryOperatorType.Equal)));
            return query.ApplyFilter(CriteriaOperator.ToString(criteria));
        }

        public static IQueryable UniqueValuesForField(this IQueryable query, string fieldName)
        {
            query = query.Select(fieldName);
            var expression = Expression.Call(typeof (Queryable), "Distinct", new[] {query.ElementType}, query.Expression);
            return query.Provider.CreateQuery(expression);
        }

        public static IEnumerable<GridViewGroupInfo> GetGroupInfo(this IQueryable query, string fieldName,
            ColumnSortOrder order)
        {
            var list = new List<GridViewGroupInfo>();

            var rowType = query.ElementType;

            var enumerable = query.ConvertToGenericList();
            query = enumerable.AsQueryable();

            query = query.MakeGroupBy(Converter, new OperandProperty(fieldName));
            query = query.MakeOrderBy(Converter,
                new ServerModeOrderDescriptor(new OperandProperty("Key"), order == ColumnSortOrder.Descending));
            query = query.ApplyGroupInfoExpression(rowType);

            foreach (var item in query)
            {
                var obj = (object[]) item;
                list.Add(new GridViewGroupInfo {KeyValue = obj[0], DataRowCount = (int) obj[1]});
            }

            return list;
        }

        public static object[] CalculateSummary(this IQueryable query, List<GridViewSummaryItemState> summaryItems)
        {
            var elementType = query.ElementType;

            query = query.MakeGroupBy(Converter, new OperandValue(0));
            var groupParam = Expression.Parameter(query.ElementType, string.Empty);

            var expressions = GetAggregateExpressions(elementType, summaryItems, groupParam);
            query = query.ApplyExpressions(expressions, groupParam);

            var groupValue = query.OfType<object>().ToArray(); // todo: check
            return groupValue.Length > 0 ? groupValue[0] as object[] : new object[summaryItems.Count];
        }

        public static IList ConvertToGenericList(this IQueryable query)
        {
            //todo: check
            IList list = null;

            foreach (var entity in query)
            {
                if (list == null)
                {
                    var genericListType = typeof (List<>).MakeGenericType(entity.GetType());
                    list = (IList) Activator.CreateInstance(genericListType);
                }

                list.Add(entity);
            }

            return list;
        }

        private static IEnumerable<Expression> GetAggregateExpressions(Type elementType,
            IEnumerable<GridViewSummaryItemState> summaryItems, ParameterExpression groupParam)
        {
            var list = new List<Expression>();
            var elementParam = Expression.Parameter(elementType, "elem");
            foreach (var item in summaryItems)
            {
                Expression e;
                LambdaExpression elementExpr = null;
                if (!string.IsNullOrEmpty(item.FieldName))
                    elementExpr = Expression.Lambda(
                        Converter.Convert(elementParam, new OperandProperty(item.FieldName)), elementParam);

                switch (item.SummaryType)
                {
                    case SummaryItemType.Count:
                        e = Expression.Call(typeof (Enumerable), "Count", new[] {elementType}, groupParam);
                        break;
                    case SummaryItemType.Sum:
                        e = Expression.Call(typeof (Enumerable), "Sum", new[] {elementType}, groupParam,
                            elementExpr);
                        break;
                    case SummaryItemType.Min:
                        e = Expression.Call(typeof (Enumerable), "Min", new[] {elementType}, groupParam,
                            elementExpr);
                        break;
                    case SummaryItemType.Max:
                        e = Expression.Call(typeof (Enumerable), "Max", new[] {elementType}, groupParam,
                            elementExpr);
                        break;
                    case SummaryItemType.Average:
                        e = Expression.Call(typeof (Enumerable), "Average", new[] {elementType}, groupParam,
                            elementExpr);
                        break;
                    default:
                        throw new NotSupportedException(item.SummaryType.ToString());
                }
                list.Add(e);
            }

            return list;
        }

        private static IQueryable ApplyExpressions(this IQueryable query, IEnumerable<Expression> expressions,
            ParameterExpression param)
        {
            var combinedExpr = Expression.NewArrayInit(typeof (object),
                expressions.Select(expr => Expression.Convert(expr, typeof (object))).ToArray());
            return query.ApplyExpression(combinedExpr, param);
        }

        private static IQueryable ApplyExpression(this IQueryable query, Expression expression,
            ParameterExpression param)
        {
            var lambda = Expression.Lambda(expression, param);
            var callExpr = Expression.Call(typeof (Queryable), "Select", new[] {query.ElementType, lambda.Body.Type},
                query.Expression, Expression.Quote(lambda));
            return query.Provider.CreateQuery(callExpr);
        }

        private static IQueryable ApplyGroupInfoExpression(this IQueryable query, Type rowType)
        {
            var param = Expression.Parameter(query.ElementType, string.Empty);

            return
                query.ApplyExpressions(
                    new Expression[]
                    {
                        Expression.Property(param, "Key"),
                        Expression.Call(typeof (Enumerable), "Count", new[] {rowType}, param)
                    }, param);
        }
    }
}