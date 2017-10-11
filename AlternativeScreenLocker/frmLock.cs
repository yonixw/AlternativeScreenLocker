using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Config = AlternativeScreenLocker.Properties.Settings;

namespace AlternativeScreenLocker
{
    public partial class frmLock : Form
    {
        #region Set thread state busy
        // SO? 6302309/1997873
        // Import SetThreadExecutionState Win32 API and necessary flags
        [DllImport("kernel32.dll")]
        public static extern uint SetThreadExecutionState(uint esFlags);
        public const uint ES_CONTINUOUS = 0x80000000;
        public const uint ES_SYSTEM_REQUIRED = 0x00000001;

        private uint fPreviousExecutionState;


        void SetBusy() {
            // Set new state to prevent system sleep
            fPreviousExecutionState = SetThreadExecutionState(
                ES_CONTINUOUS | ES_SYSTEM_REQUIRED);
            if (fPreviousExecutionState == 0)
            {
                Console.WriteLine("SetThreadExecutionState failed. Do something here...");
                Close();
            }
        }

        void SetFree() {
            // At this point no erro checking.
            SetThreadExecutionState(fPreviousExecutionState);
        }

        void SetFreeAndRestart(string reason)
        {
            SetFree();
            myApplication.Restart(reason);
        }
        #endregion


        int screenId = 0;
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

        void initForm(int screenID) {
            int myProcessId = Process.GetCurrentProcess().Id;

            // Close same process if exist.
            foreach(Process p in Process.GetProcessesByName("AlternativeScreenLocker")) {
                if (p.Id != myProcessId)
                    p.Kill();
            }


            Rectangle bounds = Screen.AllScreens[screenID].Bounds;

            this.Location = originFromRectangle(bounds);
            this.Size = sizeFromRectangle(bounds); // Need Refreshing

            lblDes.Text = "No. " + screenId + " (" + sizeToString(sizeFromRectangle(bounds)) + ")";
          

            if (screenID != 0) {
                cbMouse.Enabled = false;
                //ttMain.Visible = false; Can cause bugs!
            }

            pbBG.Location = new Point(0, 0);

            grpSetting.Location = new Point(15, this.Bounds.Height - grpSetting.Bounds.Height - 15);

            ttMain.Location = new Point(this.Width / 2 - ttMain.Width/2, this.Height/2 - ttMain.Height/2);

            // Get control:
            this.TopMost = !Config.Default.debug;
            btnDebugQuit.Visible = Config.Default.debug;
            this.WindowState = FormWindowState.Normal;
            ttMain.Focus();

            // Windows 10 Virtual Desktops:
            startLookinfForVD = true;

            // Start playing video in loop:
            string[] videos = Directory.GetFiles(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Videos"),"*.mp4");
            string chosenVideo = videos[(new Random()).Next(videos.Length)];
            Debug.WriteLine("Playing: " + chosenVideo);
            axWindowsMediaPlayer1.URL = chosenVideo;
            axWindowsMediaPlayer1.settings.volume = 0; // mute anyway
            axWindowsMediaPlayer1.settings.mute = true;
            axWindowsMediaPlayer1.PlayStateChange += AxWindowsMediaPlayer1_PlayStateChange;
            axWindowsMediaPlayer1.Ctlcontrols.play();
            axWindowsMediaPlayer1.uiMode = "none";

            tmrMonitor.Enabled = true;
            tmrSync.Enabled = true;

            // Set form as busy:
            SetBusy();
        }

        private void AxWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            //https://social.msdn.microsoft.com/Forums/windows/en-US/ac133255-2f18-4cd6-a6a1-e83549496794/axwindowsmediaplayer1-media-repeat?forum=winforms
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                axWindowsMediaPlayer1.uiMode = "none";
            }
        }

        public class LockWindow
        {
            public frmLock LockForm;
            public string LockBounds;
            public string ScreenName;
        }

        public static List<LockWindow> allOpenedForms = new List<LockWindow>();
        static string passValidChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

        private void validatePasword()
        {
            bool passIsValid = false; // Until proven true;

            if (!string.IsNullOrEmpty(Config.Default.p))
            {
                SharedData.Instance.ActualPassword = Config.Default.p.ToUpper();
                bool everyPassCharisOk = true;

                foreach (char c in SharedData.Instance.ActualPassword)
                {
                    if (!passValidChars.Contains(c))
                    {
                        everyPassCharisOk = false;
                        break;
                    }
                }

                if (everyPassCharisOk) passIsValid = true;
            }
            
            if (!passIsValid)
            {
                MessageBox.Show(
                    "Your password " + Config.Default.p + " is not valid.\n"
                    + "Please only use numbers(0-9) and english(A-Z)"
                    );
                myApplication.Exit("Invalid password");
            }
        }

        private void frmLock_Load(object sender, EventArgs e)
        {
            if (screenId == 0) {
                // Check if pass is ok:
                validatePasword();

                allOpenedForms.Add(
                        new LockWindow()
                        {
                            LockForm = this,
                            LockBounds = Screen.AllScreens[0].Bounds.ToString(),
                            ScreenName = Screen.AllScreens[0].DeviceName
                        }
                        );

                for (int i=1; i<Screen.AllScreens.Length;i++) {
                    frmLock item = new frmLock(i);
                    allOpenedForms.Add(
                        new LockWindow()
                        {
                            LockForm = item,
                            LockBounds = Screen.AllScreens[i].Bounds.ToString(),
                            ScreenName = Screen.AllScreens[i].DeviceName
                        }
                        );
                    item.Show();

                    //Console.WriteLine("Added form to screen {0} with borders {1}"
                    //    , Screen.AllScreens[i].DeviceName, Screen.AllScreens[i].Bounds.ToString());
                }
            }

            // Load all componenets with some lag so form is loaded instantly.
            tmrInit.Enabled = true;
        }


        // SO? 570045/1997873
        bool startLookinfForVD = false;
        private void frmLock_Deactivate(object sender, EventArgs e)
        {
            this.TopMost = !Config.Default.debug;
            this.WindowState = FormWindowState.Normal;
        }

        private void frmLock_Activated(object sender, EventArgs e)
        {
            //ttMain.Focus(); Only in main Form!
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

        static Point centerBox(Rectangle Bound)
        {
            return new Point(Bound.X + Bound.Width / 2, Bound.Y + Bound.Height / 2);
        }

        // Flag to move forward or backward
        bool mouseDirection = true;

        // For all forms:
        public static DateTime startTime = DateTime.Now;
        public TimeSpan timePassed = TimeSpan.FromSeconds(0);

        public static VirtualDesktopManager VDmanager = new VirtualDesktopManager();

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

                    // Try using SendInput Win32 API
                    // SO ? 5094398
                    MouseSimulator.ClickLeftMouseButton(); // No point with right

                    // For next time:
                    mouseDirection = !mouseDirection;
                }

                if (startLookinfForVD )
                {
                    if (!VDmanager.IsWindowOnCurrentVirtualDesktop(this.Handle))
                    {
                        // Open in new window in selected virtual desktop!
                        SetFreeAndRestart("Switched virtual desktop");
                    }
                }


                // Detect if new screen was added 
                foreach (Screen scr in Screen.AllScreens)
                {
                    bool found = false;
                    foreach (LockWindow objLock in allOpenedForms)
                    {
                        if (
                            scr.Bounds.ToString() == objLock.LockBounds
                            && scr.DeviceName == objLock.ScreenName
                            && scr.Bounds.Contains(
                                centerBox(objLock.LockForm.Bounds)
                                )
                            )
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found) // A screen without locking form to it
                    {
                        Console.WriteLine("Didnt found: " + scr.DeviceName);
                        SetFreeAndRestart("A screen doesnt have locking on it.");
                    }
                }

            }

            // Exit on password
            if (SharedData.Instance.CurrentPassword == SharedData.Instance.ActualPassword)
            {
                myApplication.Exit("Correct password");
            }
            else
            {
                ttMain.Text =  SharedData.Instance.CurrentPassword.Length + " Chars";
            }

            // Get control:
            this.TopMost = !Config.Default.debug;
            this.WindowState = FormWindowState.Normal;


            timePassed += TimeSpan.FromMilliseconds(tmrSync.Interval);

            lblUptime.Text = "Uptime: " 
                + timePassed.ToString(@"d\.hh\:mm\:ss")
                + ", Loaded: "
                + startTime.ToShortDateString() + " " + startTime.ToShortTimeString()
                ;
        }

        private void ttMain_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void tmrMonitor_Tick(object sender, EventArgs e)
        {
           


            if (Config.Default.debug == false)
            {
                foreach (Process p in Process.GetProcessesByName("Taskmgr"))
                {
                    try
                    {
                        p.Kill();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message); // Usually only on Access is Denied
                    }
                } 
            }
        }

        private void frmLock_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prevent closing from alt-tab
            if (e.CloseReason == CloseReason.UserClosing)
            {
                SetFreeAndRestart("Prevent user from closing.");
            }
        }

        private void tmrInit_Tick(object sender, EventArgs e)
        {
            tmrInit.Enabled = false;
            initForm(screenId);
        }

        private void myKeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Back && SharedData.Instance.CurrentPassword.Length > 0)
            {
                SharedData.Instance.CurrentPassword = SharedData.Instance.CurrentPassword.Substring(0, SharedData.Instance.CurrentPassword.Length - 1);
            }
            else
            {
                int key = (int)e.KeyCode;

                if (key >= (int)Keys.D0 && key <= (int)Keys.D9)
                    SharedData.Instance.CurrentPassword += (char)(key - Keys.D0 + '0');
                else if (key >= (int)Keys.NumPad0 && key <= (int)Keys.NumPad9)
                    SharedData.Instance.CurrentPassword += (char)(key - Keys.D0 + '0');
                else if (key >= (int)Keys.A && key <= (int)Keys.Z)
                    SharedData.Instance.CurrentPassword += (char)(key - Keys.A + 'A');
            }

            
        }

        private void btnDebugQuit_Click(object sender, EventArgs e)
        {
            myApplication.Exit("Exit button debug");
        }
    }
}
