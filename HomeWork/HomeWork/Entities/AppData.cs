using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork.Entities
{
    public class AppData
    {
        public static HomeWorkDBEntities Context
        {
            get; set;
        } = new HomeWorkDBEntities();
    }
}
