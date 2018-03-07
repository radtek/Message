using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bll
{
    public class Sms_outbox
    {
        Dal.Sms_outbox dal = new Dal.Sms_outbox();
        public bool Add(Model.Sms_outbox model)
        {
            return dal.Add(model);
        }
        public bool Exists(string extcode, string phone)
        {
            return dal.Exists(extcode, phone);
        }
        public string getSentSms()
        {
            return dal.getSentSms();
        }
        public bool ExistMinute(string phone, string beginTime, string endTime)
        {
            return dal.ExistMinute(phone, beginTime, endTime);
        }
    }
}
