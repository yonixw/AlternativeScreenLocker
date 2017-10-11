using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlternativeScreenLocker
{
    static class myApplication
    {
        public static void Restart(string reason)
        {
            Debug.WriteLine("[RESTART] " + reason);
            Application.Restart();
        }

        public static void Exit(string reason)
        {
            Debug.WriteLine("[EXIT] " + reason);
            Application.Exit();
        }
    }
}
