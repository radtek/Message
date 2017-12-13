using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Sms_outbox
    {
        public string sismsid { get; set; }
        public string extcode { get; set; }
        public string destaddr { get; set; }
        public string messagecontent { get; set; }
        public int reqdeliveryreport { get; set; }
        public int msgfmt { get; set; }
        public int sendmethod { get; set; }
        public string requesttime { get; set; }
        public string applicationid { get; set; }

    }
}
