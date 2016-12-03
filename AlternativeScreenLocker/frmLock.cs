using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AlternativeScreenLocker
{
    public partial class frmLock : Form
    {
        int screenId;
        public frmLock(int id=0)
        {
            this.screenId = id;
            InitializeComponent();
        }

        void Log(object o) {
            Debug.WriteLine(o);
        }

        Point originFromRectangle(Rectangle r) {
            return new Point(r.Left, r.Top);
        }

        Size sizeFromRectangle(Rectangle r) {
            return new Size(r.Width, r.Height);
        }

        string sizeToString(Size s) {
            return s.Width + "x" + s.Height ;
        }

        void initForm(int sID) {
            Rectangle bounds = Screen.AllScreens[sID].Bounds;

            this.Location = originFromRectangle(bounds);
            this.Size = sizeFromRectangle(bounds); // Need Refreshing

            lblDes.Text = "No. " + screenId + " (" + sizeToString(sizeFromRectangle(bounds)) + ")";
          

            if (sID != 0) {
                cbMouse.Enabled = false;
            }

            pbBG.Location = new Point(0, 0);

            grpSetting.Location = new Point(15, this.Bounds.Height - grpSetting.Bounds.Height - 15);

            ttMain.Location = new Point(this.Width / 2 - ttMain.Width/2, this.Height/2 - ttMain.Height/2);

            // Get control:
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
            ttMain.Focus();
        }

        private void frmLock_Load(object sender, EventArgs e)
        {
            if (screenId == 0) {
               

                for(int i=1; i<Screen.AllScreens.Length;i++) {
                    (new frmLock(i)).Show();
                }
            }
            initForm(screenId);
        }


        // SO? 570045/1997873
        private void frmLock_Deactivate(object sender, EventArgs e)
        {
            this.TopMost = true;
            this.WindowState = FormWindowState.Normal;
        }

        private void frmLock_Activated(object sender, EventArgs e)
        {
            ttMain.Focus();
        }

        TimeSpan replaceBGImageInterval = TimeSpan.FromMinutes(5);
        DateTime nextReplaceBGImage = DateTime.Now;

        Image lastImage = null;
        bool newImage = false;
        void getImageAsync() 
        {
            //https://source.unsplash.com/category/nature/<w>x<h>
            //https://source.unsplash.com/category/<category>/<w>x<h>
            // https://source.unsplash.com/<w>x<h>
            // SO? 4071064/1997873

            try
            {
                var request = WebRequest.Create("https://source.unsplash.com/category/nature/" + sizeToString(this.Size));

                using (var response = request.GetResponse())
                using (var stream = response.GetResponseStream())
                {
                    lastImage = Bitmap.FromStream(stream);
                    newImage = true; // flag to use it.
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        // Flag to move forward or backward
        bool mouseDirection = true;

        // For all forms:
        public static DateTime startTime = DateTime.Now; 

        private void tmrSync_Tick(object sender, EventArgs e)
        {
            if (cbBG.Checked)
            {
                if (DateTime.Now > nextReplaceBGImage)
                {
                    // Update next switch time
                    nextReplaceBGImage = DateTime.Now + replaceBGImageInterval;

                    // Replace image:
                    System.Threading.Thread t = new System.Threading.Thread(
                        () => { getImageAsync(); }
                    )
                    { Name = "DownloadImageThread"};
                    t.Start();
                }

                if (newImage) {
                    newImage = false;
                    pbBG.Image = lastImage;
                }
            }

            if (screenId == 0)// Only in main screen
            {

                if (cbMouse.Checked)
                { 
                    Point movement = (mouseDirection) ? new Point(1, 1) : new Point(-1, -1);
                    Point curretnMouse = Cursor.Position;
                    curretnMouse.Offset(movement);
                    Cursor.Position = curretnMouse;

                    // For next time:
                    mouseDirection = !mouseDirection;
                }

                
            }

            lblUptime.Text = "Uptime: " + (DateTime.Now - startTime).ToString(@"d\.hh\:mm\:ss");
        }

        private void ttMain_TextChanged(object sender, EventArgs e)
        {
            // Exit on password
            if (ttMain.Text == AlternativeScreenLocker.Properties.Settings.Default.p)
                Application.Exit();
        }

        private void tmrMonitor_Tick(object sender, EventArgs e)
        {
            foreach (Process p in Process.GetProcessesByName("Taskmgr"))
            {
                p.Kill();
            }
        }

        private void frmLock_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prevent closing from alt-tab
            if (e.CloseReason == CloseReason.UserClosing)
                Application.Restart();
        }
    }
}
