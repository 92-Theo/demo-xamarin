using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace App1Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service : IService
    {
        public ReturnCode GetData(string value)
        {
            ReturnCode rc = new ReturnCode
            {
                value = $"You entered: {value}"
            };
            return rc;
        }

        public string HelloWorld()
        {
            return "HelloWorld";
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public ReturnCode Login(string id, string pw)
        {
            ReturnCode rc = new ReturnCode
            {
                value = DBMgr.Login(id, pw).ToString()
            };

            return rc;
        }
    }
}
