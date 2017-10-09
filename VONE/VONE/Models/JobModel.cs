using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VONE.Models
{
    public class JobModel                               //到时候通过领域找工作也好找一些
    {
        public int POS_ID;
        public string POS_NAME;
        public int? SALARY;
        public string DESCRIPTION;
        public string RELATED_FIELD;
        public int C_ID;
        public string COMP_NAME;
    }
}