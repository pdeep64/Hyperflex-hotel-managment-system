using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HYFLEX_HMS.CLASS
{
    class INSTALLED_SYSTEM_DATA
    {
        private int _id;
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private String _SystemMaintain;
        public String SystemMaintain
        {
            get { return _SystemMaintain; }
            set { _SystemMaintain = value; }
        }

        private DateTime _InstalledDate;
        public DateTime InstalledDate
        {
            get { return _InstalledDate; }
            set { _InstalledDate = value; }
        }

        private Double _SystemPrice;
        public Double SystemPrice
        {
            get { return _SystemPrice; }
            set { _SystemPrice = value; }
        }

        private Double _Paid;
        public Double Paid
        {
            get { return _Paid; }
            set { _Paid = value; }
        }

        private Double _Due;
        public Double Due
        {
            get { return _Due; }
            set { _Due = value; }
        }

        private int _CustomerID;
        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }
        
        private string _installationkey;
        public string installationkey
        {
            get { return _installationkey; }
            set { _installationkey = value; }
        }

        private string _STATUS;
        public string STATUS
        {
            get { return _STATUS; }
            set { _STATUS = value; }
        }
        private string _Description;
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        
        private int _TYPE;
        public int TYPE
        {
            get { return _TYPE; }
            set { _TYPE = value; }
        }
    }
}
