using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using TimerJobExample.TimerJobs;

namespace TimerJobExample.Features.CleanListJobReceiver
{
    /// <summary>
    /// Этот класс обрабатывает события, возникающие в ходе активации, деактивации, установки, удаления и обновления компонентов.
    /// </summary>
    /// <remarks>
    /// GUID, присоединенный к этому классу, может использоваться при создании пакета и не должен изменяться.
    /// </remarks>

    [Guid("c316c77e-e440-46cf-ba32-8495796309e6")]
    public class CleanListJobReceiverEventReceiver : SPFeatureReceiver
    {
        private const string _JOB_NAME = "CleanListJob";
        private string ProjectName = Assembly.GetExecutingAssembly().GetName().Name;
   
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPWeb web = (SPWeb)properties.Feature.Parent)
                    {
                        JobDeleting(web);
                        JobCreating(web);
                    }
                });
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    using (SPWeb web = (SPWeb)properties.Feature.Parent)
                    {
                        JobDeleting(web);
                    }
                });
            }
            catch (Exception)
            {
                Console.WriteLine("Error");
            }
        }

        private void JobCreating(SPWeb web)
        {
            var job = new CleanListJob(_JOB_NAME, web.Site.WebApplication)
            {
                Schedule = new SPMonthlySchedule() { BeginDay = 20, EndDay = 25, BeginHour = 1, EndHour = 2 }
            };

            job.Update();
        }

        private void JobDeleting(SPWeb web)
        {
            foreach (var job in web.Site.WebApplication.JobDefinitions)
                if (job.Name == _JOB_NAME)
                    job.Delete();
        }
    }
}
