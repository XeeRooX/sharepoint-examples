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

        public CleanListJob() { }
        public CleanListJob(string jobName, SPWebApplication webApplication):base(jobName, webApplication, null, SPJobLockType.Job)
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
                    var list = web.GetList(SPUrlUtility.CombineUrl(web.ServerRelativeUrl, _LIST_NAME));
                    CleanList(web, list, Months.April | Months.August);
                }
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }

        [Flags]
        enum Month
        {
            Januar,
            Februar
        }

        private void CleanList(SPWeb web, SPList list, Months months)
        {
            if (!months.GetNumbersOfMonth().Contains(DateTime.Now.Month))
                return;

            // Дата, до которой нужно удалить все записи (оставить только за квартал)
            var beginDate = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Now.AddMonths(-3));

            var query = new SPQuery()
            {
                //Query = $@"
                //<Where>
                //    <Lt>
                //        <FieldRef Name='Created'/>
                //        <Value IncludeTimeValue='FALSE' Type='DateTime'>{beginDate}</Value>
                //    </Lt>
                //</Where>"
                Query = $@"
                <Where>
                    <Lt>
                        <FieldRef Name='{list.Fields["JustDateTime"].InternalName}'/>
                        <Value Type='DateTime'>{beginDate}</Value>
                    </Lt>
                </Where>"
            };

            var items = SPListProcessing.GetSPListItems(list, query);
            SPListProcessing.BatchDeleteItems(items, web);
        }
    }
}
