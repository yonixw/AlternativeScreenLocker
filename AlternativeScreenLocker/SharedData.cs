using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlternativeScreenLocker
{
    class SharedData
    {
        private SharedData()
        {

        }

        public static SharedData Instance = new SharedData();

        // Members:
        public string ActualPassword = "";
        public string CurrentPassword = "";
    }
}
