using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TimerJobExample.Common;
using TimerJobExample.Sharepoint;

namespace TimerJobExample.TimerJobs
{
    class CleanListJob : SPJobDefinition
    {
        private const string _WEB_URL = "http://win-8nosj5h8ulb/";
        private const string _LIST_NAME = "Lists/customList";
        private const string _ORDERS_LIST_NAME = "Lists/orders";
        private const string _ORDERS_BACKUP_LIST_NAME = "Lists/ordersBackup";

        public CleanListJob() { }
        public CleanListJob(string jobName, SPWebApplication webApplication) : base(jobName, webApplication, null, SPJobLockType.Job)
        {
            Title = jobName;
        }

        public override void Execute(Guid targetInstanceId)
        {
            try
            {
                using (var site = new SPSite(_WEB_URL))
                using (var web = site.OpenWeb())
                {
                    var ordersList = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, _ORDERS_LIST_NAME));
                    var ordersBackupList = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, _ORDERS_BACKUP_LIST_NAME));

                    //CleanList(web, list, Months.April | Months.August);
                    CleanList(web, ordersList, ordersBackupList );
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void BatchCreateOrders(IEnumerable<SPListItem> items, SPWeb web, SPList ordersBackupList)
        {
            var copyFields = new List<string>() { "date", "status", "code", "name", "supervisor", "manager", "order" };
            var ordersBackupListGuid = ordersBackupList.ID.ToString();

            var batchCreate = new StringBuilder();

            //batchCreate.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?><ows:Batch OnError=""Continue"">");
            batchCreate.Append(@"<?xml version=""1.0"" encoding=""UTF-8""?><ows:Batch>");

            var methodId = 1;

            foreach (SPListItem item in items)
            {
                

                batchCreate.Append($"<Method ID=\"{methodId}\">");
                batchCreate.Append($"<SetList>{ordersBackupListGuid}</SetList>");
                batchCreate.Append("<SetVar Name=\"ID\">New</SetVar>");
                batchCreate.Append("<SetVar Name=\"Cmd\">Save</SetVar>");

                foreach (var field in copyFields)
                {
                    var value = item[field] == null ? "&nbsp;" : item[field].ToString()
                        .Replace("&", "&amp")
                        .Replace("\"", "&quot;");

                    if(field == "date")
                    {
                        value = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(value));
                        Console.WriteLine();
                    }

                    //if (field == "order")
                    //    field = "order0";

                    batchCreate.Append($"<SetVar Name=\"urn:schemas-microsoft-com:office:office#{(field == "order" ? "order0" : field)}\">{value}</SetVar>");
                    //batchCreate.Append($"<SetVar Name=\"{field}\">{value}</SetVar>");
                }     
                batchCreate.Append("</Method>");
                methodId++;
            }
            batchCreate.Append("</ows:Batch>");
            var batchString = batchCreate.ToString();
            var result = web.ProcessBatchData(batchString);
            Console.WriteLine(result);
        }

        private void CleanList(SPWeb web, SPList ordersList, SPList orderBackupList)
        {
            //if (!months.GetNumbersOfMonth().Contains(DateTime.Now.Month))
            //    return;

            var quarterSize = 3;
            var backupCountMonths = 3;

            // Дата, до которой нужно удалить все записи (оставить только за квартал)
            var cleanDate = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.AddMonths(-quarterSize));

            var beginBackupDate = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.AddMonths(-quarterSize - backupCountMonths));
            var endBackupDate = cleanDate;



            var cleanQuery = new SPQuery()
            {
                //Query = $@"
                //<Where>
                //    <Lt>
                //        <FieldRef Name='Created'/>
                //        <Value IncludeTimeValue='FALSE' Type='DateTime'>{cleanDate}</Value>
                //    </Lt>
                //</Where>"
                Query = $@"
                <Where>
                    <Lt>
                        <FieldRef Name='{ordersList.Fields["date"].InternalName}'/>
                        <Value Type='DateTime'>{cleanDate}</Value>
                    </Lt>
                </Where>"
            };

            var backupQuery = new SPQuery()
            {
                Query = $@"
                    <Where>
                        <And>
                            <Geq>
                                <FieldRef Name='{ordersList.Fields["date"].InternalName}'/>
                                <Value Type='DateTime'>{beginBackupDate}</Value>
                            </Geq>
                            <Lt>
                                <FieldRef Name='{ordersList.Fields["date"].InternalName}'/>
                                <Value Type='DateTime'>{endBackupDate}</Value>
                            </Lt>
                        </And>
                    </Where>
                "
            };
            var cleanItems = SPListProcessing.GetSPListItems(ordersList, cleanQuery);
            var backupItems = SPListProcessing.GetSPListItems(ordersList, backupQuery);

            // Очистка списка бэкапов
            // Очень опасно, если указать не тот список, поэтому наименование списка булет указано через константу
            SPListProcessing.CleanList(orderBackupList, web);

            // Создание бэкапа
            BatchCreateOrders(backupItems, web, orderBackupList);

            // Удаление заказов
            //SPListProcessing.BatchDeleteItems(cleanItems, web);
        }
    }
}
