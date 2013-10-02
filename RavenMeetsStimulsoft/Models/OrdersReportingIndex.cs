using System;
using System.Linq;
using Raven.Abstractions.Indexing;
using Raven.Client.Indexes;

namespace RavenMeetsStimulsoft.Models
{
    public class OrdersReportingIndex : AbstractIndexCreationTask<Order, OrderInfo>
    {
        public OrdersReportingIndex()
        {
            Map = orders => from order in orders
                            let company = LoadDocument<Company>(order.Company)
                            let employee = LoadDocument<Employee>(order.Employee)
                            select new
                            {
                                OrderId = int.Parse(order.Id.Split('/')[1]),
                                OrderDate = order.OrderedAt,
                                Total = order.Lines.Sum(l => Convert.ToDecimal(l.PricePerUnit) * l.Quantity * (1 - Convert.ToDecimal(l.Discount))),
                                CompanyName = company.Name,
                                EmployeeFirstName = employee.FirstName,
                                EmployeeLastName = employee.LastName,
                            };

            StoreAllFields(FieldStorage.Yes);

            Sort(x => x.Total, SortOptions.Double);
        }
    }

    public class OrderInfo
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal Total { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeFirstName { get; set; }
        public string EmployeeLastName { get; set; }
    }
}