using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Quartz;
using System.Windows.Forms;
using Topshelf;
using Commmon;
using System.Data;

namespace WindowsFormsApplication1
{
    public class JCIJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            //MessageBox.Show("JCIJob");
            //string timeStr = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //string morning = "08:00", morningEnd = "18:00";
            //TimeSpan dayAM = DateTime.Parse(morning).TimeOfDay;
            //TimeSpan dayPM = DateTime.Parse(morningEnd).TimeOfDay;
            //DateTime t1 = Convert.ToDateTime(timeStr);
            //TimeSpan dspNow = t1.TimeOfDay;
            //if (dayAM <= dspNow && dspNow <= dayPM)
            //{
            //    SendJCI.Send();
            //}
            try
            {
                SendJCI.Send();
            }
            catch (Exception)
            {

                throw;
            } 
        }
    }
}
