using System;
using System.IO;
using System.Media;
using System.Reflection;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using NAudio.Wave;

namespace siri;

public partial class Form1 : Form
{
    // Global keyboard hook
    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
    private LowLevelKeyboardProc _keyboardProc;
    private IntPtr _hookId = IntPtr.Zero;

    [DllImport("user32.dll")]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll")]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    private const int WH_KEYBOARD_LL = 13;
    private const int WM_KEYDOWN = 0x0100;
    private const int VK_F10 = 0x79;

    // Smoke alarm chirp
    private System.Windows.Forms.Timer _chirpTimer;
    private Random _chirpRandom = new Random();
    private SoundPlayer _chirpPlayer;

    // Address me popup
    private System.Windows.Forms.Timer _addressMePopupTimer;
    private Random _addressMePopupRandom = new Random();
    private Form _addressMePopup;
    private SoundPlayer _vineBoomPlayer;

    // YouTube video popup
    private System.Windows.Forms.Timer _youtubeTimer;
    private Random _youtubeRandom = new Random();

    // Notification popups
    private System.Windows.Forms.Timer _notificationTimer1;
    private System.Windows.Forms.Timer _notificationTimer2;
    private System.Windows.Forms.Timer _notificationTimer3;
    private SoundPlayer _androidNotificationPlayer;

    // Incoming call popup
    private System.Windows.Forms.Timer _incomingCallTimer;
    private Form _incomingCallPopup;
    private SoundPlayer _incomingCallPlayer;

    // Question popup
    private System.Windows.Forms.Timer _questionTimer;
    private List<Form> _fruitPopups = new List<Form>();
    private Random _lemonLimeRandom = new Random();
    private SoundPlayer _loudAlarmPlayer;

    // Final destruction payload
    private System.Windows.Forms.Timer _finalPayloadStartTimer;
    private System.Windows.Forms.Timer _finalPayloadTimer;
    private System.Windows.Forms.Timer _audioLoopTimer;
    private List<Form> _finalFruitPopups = new List<Form>();
    private int _finalPayloadInterval = 2000;
    private Random _finalRandom = new Random();
    private List<WaveOutEvent> _activeOutputs = new List<WaveOutEvent>();

    // Golden Freddy jumpscare
    private System.Windows.Forms.Timer _goldenFreddyTimer;
    private Form _goldenFreddyPopup;
    private SoundPlayer _goldenFreddyPlayer;

    // Kailyn voice lines
    private SoundPlayer _kKirrYouPlayer;
    private SoundPlayer _kDishonourPlayer;
    private SoundPlayer _kRemonsButtPlayer;
    private SoundPlayer _kGrapeYouPlayer;

    // David voice lines
    private SoundPlayer _dGrapeYouPlayer;
    private SoundPlayer _dTakeShitPlayer;
    private SoundPlayer _d7pmBedtimePlayer;
    private SoundPlayer _dFriedRicePlayer;
    private SoundPlayer _dCarDisrespectPlayer;

    public Form1()
    {
        InitializeComponent();

        // Global keyboard hook
        _keyboardProc = HookCallback;
        using var curProcess = System.Diagnostics.Process.GetCurrentProcess();
        using var curModule = curProcess.MainModule;
        _hookId = SetWindowsHookEx(WH_KEYBOARD_LL, _keyboardProc, GetModuleHandle(curModule.ModuleName), 0);

        // Voice lines
        _kKirrYouPlayer = LoadSound("siri.Resources.k-1-kirr-you.wav");
        _kDishonourPlayer = LoadSound("siri.Resources.k-2-dishonour.wav");
        _kRemonsButtPlayer = LoadSound("siri.Resources.k-3-remons-butt.wav");
        _kGrapeYouPlayer = LoadSound("siri.Resources.k-4-grape-you.wav");

        _dGrapeYouPlayer = LoadSound("siri.Resources.d-1-grape-you.wav");
        _dTakeShitPlayer = LoadSound("siri.Resources.d-2-take-shit.wav");
        _d7pmBedtimePlayer = LoadSound("siri.Resources.d-3-7pm-bedtime.wav");
        _dFriedRicePlayer = LoadSound("siri.Resources.d-4-fried-rice.wav");
        _dCarDisrespectPlayer = LoadSound("siri.Resources.d-5-car-disrespect.wav");

        // Misc sounds
        _loudAlarmPlayer = LoadSound("siri.Resources.loud_alarm.wav");
        _androidNotificationPlayer = LoadSound("siri.Resources.loud_android_notification.wav");
        _chirpPlayer = LoadSound("siri.Resources.smoke_alarm_chirp.wav");
        _incomingCallPlayer = LoadSound("siri.Resources.your_phone_linging.wav");
        _vineBoomPlayer = LoadSound("siri.Resources.vine_boom.wav");
        _goldenFreddyPlayer = LoadSound("siri.Resources.golden_freddy_scream.wav");

        // Chirp timer
        _chirpTimer = new System.Windows.Forms.Timer();
        _chirpTimer.Tick += ChirpTimerTick;

        // Address me popup timer
        _addressMePopupTimer = new System.Windows.Forms.Timer();
        _addressMePopupTimer.Tick += AddressMePopupTimerTick;

        // YouTube timer
        _youtubeTimer = new System.Windows.Forms.Timer();
        _youtubeTimer.Tick += YouTubeTimerTick;

        // Incoming call timer - 2 minutes
        _incomingCallTimer = new System.Windows.Forms.Timer();
        _incomingCallTimer.Interval = 120000;
        _incomingCallTimer.Tick += IncomingCallTimerTick;

        // Notification timer 1 - 3 minutes
        _notificationTimer1 = new System.Windows.Forms.Timer();
        _notificationTimer1.Interval = 180000;
        _notificationTimer1.Tick += (s, e) =>
        {
            _notificationTimer1.Stop();
            ShowNotificationPayload("it's 7pm in thailand, time for bed nick", _d7pmBedtimePlayer, 3000);
        };

        // Notification timer 2 - 5 minutes
        _notificationTimer2 = new System.Windows.Forms.Timer();
        _notificationTimer2.Interval = 300000;
        _notificationTimer2.Tick += (s, e) =>
        {
            _notificationTimer2.Stop();
            ShowNotificationPayload("go eat ur fried rice!", _dFriedRicePlayer, 3000);
        };

        // Notification timer 3 - 6 minutes
        _notificationTimer3 = new System.Windows.Forms.Timer();
        _notificationTimer3.Interval = 360000;
        _notificationTimer3.Tick += (s, e) =>
        {
            _notificationTimer3.Stop();
            ShowNotificationPayload("why u disrespect family, computer gone now", _dCarDisrespectPlayer, 3000);
        };

        // Question timer - 4 minutes
        _questionTimer = new System.Windows.Forms.Timer();
        _questionTimer.Interval = 240000;
        _questionTimer.Tick += (s, e) =>
        {
            _questionTimer.Stop();
            ShowQuestionPayload();
        };

        // Final payload start timer - 6 minutes 20 seconds
        _finalPayloadStartTimer = new System.Windows.Forms.Timer();
        _finalPayloadStartTimer.Interval = 380000;
        _finalPayloadStartTimer.Tick += (s, e) =>
        {
            _finalPayloadStartTimer.Stop();
            StartFinalPayload();
        };

        _finalPayloadTimer = new System.Windows.Forms.Timer();
        _finalPayloadTimer.Tick += FinalPayloadTick;

        // Golden Freddy timer - fires 15 seconds after final payload starts
        _goldenFreddyTimer = new System.Windows.Forms.Timer();
        _goldenFreddyTimer.Interval = 15000;
        _goldenFreddyTimer.Tick += (s, e) =>
        {
            _goldenFreddyTimer.Stop();
            ShowGoldenFreddyJumpscare();
        };
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
        {
            int vkCode = Marshal.ReadInt32(lParam);
            if (vkCode == VK_F10 && onButton.Enabled)
            {
                onButton.PerformClick();
            }
        }
        return CallNextHookEx(_hookId, nCode, wParam, lParam);
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        UnhookWindowsHookEx(_hookId);
        base.OnFormClosing(e);
    }

    private static readonly string[] YouTubeVideos =
    {
        "https://www.youtube.com/watch?v=At8v_Yc044Y",
        "https://www.youtube.com/watch?v=Tsxfk33gugM",
        "https://www.youtube.com/watch?v=wk414RfZeVo",
        "https://www.youtube.com/watch?v=1Z-EOytgEhA"
    };

    private void SetFirstInterval()
    {
        _chirpTimer.Interval = 45000;
        _addressMePopupTimer.Interval = 75000;
        _youtubeTimer.Interval = 95000;
    }

    private void SetRandomInterval()
    {
        _chirpTimer.Interval = _chirpRandom.Next(11000, 30001);
        _addressMePopupTimer.Interval = _addressMePopupRandom.Next(11000, 20001);
        _youtubeTimer.Interval = _youtubeRandom.Next(21000, 50001);
    }

    private void ChirpTimerTick(object sender, EventArgs e)
    {
        _chirpTimer.Stop();
        _chirpPlayer.Play();
        SetRandomInterval();
        _chirpTimer.Start();
    }

    private void AddressMePopupTimerTick(object sender, EventArgs e)
    {
        _addressMePopupTimer.Stop();
        ShowAddressMePopup();
        SetRandomInterval();
        _addressMePopupTimer.Start();
    }

    private void YouTubeTimerTick(object sender, EventArgs e)
    {
        _youtubeTimer.Stop();
        OpenRandomYouTubeVideo();
        SetRandomInterval();
        _youtubeTimer.Start();
    }

    private SoundPlayer LoadSound(string resourceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream stream = assembly.GetManifestResourceStream(resourceName);

        if (stream == null) throw new Exception($"Resource '{resourceName}' not found.");

        MemoryStream ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Position = 0;
        stream.Dispose();

        SoundPlayer player = new SoundPlayer(ms);
        player.Load();
        return player;
    }

    private void PlayOverlapping(string resourceName)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        Stream stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) return;

        MemoryStream ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Position = 0;

        WaveOutEvent output = new WaveOutEvent();
        WaveFileReader reader = new WaveFileReader(ms);
        output.Init(reader);

        _activeOutputs.Add(output);

        output.PlaybackStopped += (s, e) =>
        {
            output.Dispose();
            reader.Dispose();
            _activeOutputs.Remove(output);
        };

        output.Play();
    }

    private Form ShowImagePopup(string resourceName, int x, int y)
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null) throw new Exception($"Resource '{resourceName}' not found.");

        MemoryStream ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Position = 0;
        Image img = Image.FromStream(ms);

        Form popup = new Form();
        popup.FormBorderStyle = FormBorderStyle.None;
        popup.StartPosition = FormStartPosition.Manual;
        popup.TopMost = true;
        popup.BackColor = Color.Magenta;
        popup.TransparencyKey = Color.Magenta;
        popup.ShowInTaskbar = false;

        PictureBox pictureBox = new PictureBox();
        pictureBox.Image = img;
        pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
        pictureBox.BackColor = Color.Magenta;
        popup.Controls.Add(pictureBox);
        popup.ClientSize = pictureBox.PreferredSize;
        popup.Location = new Point(x, y);
        popup.Show();

        return popup;
    }

    private void ShowAddressMePopup()
    {
        _addressMePopup?.Close();

        Rectangle screen = Screen.PrimaryScreen.WorkingArea;
        int x = _addressMePopupRandom.Next(0, screen.Width - 200);
        int y = _addressMePopupRandom.Next(0, screen.Height - 200);

        _addressMePopup = ShowImagePopup("siri.Resources.address_me.png", x, y);
        _vineBoomPlayer.Play();

        System.Windows.Forms.Timer closeTimer = new System.Windows.Forms.Timer();
        closeTimer.Interval = 1000;
        closeTimer.Tick += (s, e) =>
        {
            _addressMePopup?.Close();
            closeTimer.Stop();
            closeTimer.Dispose();
        };
        closeTimer.Start();
    }

    private void OpenRandomYouTubeVideo()
    {
        string url = YouTubeVideos[_youtubeRandom.Next(YouTubeVideos.Length)];
        System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
        {
            FileName = url,
            UseShellExecute = true
        });
    }

    private void IncomingCallTimerTick(object sender, EventArgs e)
    {
        _incomingCallTimer.Stop();
        _chirpTimer.Stop();
        _addressMePopupTimer.Stop();

        Rectangle screen = Screen.PrimaryScreen.WorkingArea;
        _incomingCallPopup = ShowImagePopup(
            "siri.Resources.incoming_call.png",
            (screen.Width - 400) / 2,
            (screen.Height - 300) / 2
        );

        _incomingCallPlayer.PlayLooping();

        System.Windows.Forms.Timer switchTimer = new System.Windows.Forms.Timer();
        switchTimer.Interval = 10000;
        switchTimer.Tick += (s, ev) =>
        {
            switchTimer.Stop();
            switchTimer.Dispose();
            _incomingCallPlayer.Stop();
            _kDishonourPlayer.Play();

            System.Windows.Forms.Timer closeTimer = new System.Windows.Forms.Timer();
            closeTimer.Interval = 14000;
            closeTimer.Tick += (s2, ev2) =>
            {
                closeTimer.Stop();
                closeTimer.Dispose();
                _incomingCallPopup?.Close();
            };
            closeTimer.Start();
        };
        switchTimer.Start();
        _youtubeTimer.Start();
    }

    private void ShowNotificationWindow(string message)
    {
        Form notifWindow = new Form();
        notifWindow.Text = "important notice from mother";
        notifWindow.Size = new Size(400, 200);
        notifWindow.StartPosition = FormStartPosition.CenterScreen;
        notifWindow.TopMost = true;
        notifWindow.ShowInTaskbar = true;
        notifWindow.FormBorderStyle = FormBorderStyle.FixedDialog;
        notifWindow.MaximizeBox = false;
        notifWindow.MinimizeBox = false;

        Label label = new Label();
        label.Text = message;
        label.AutoSize = false;
        label.Dock = DockStyle.Fill;
        label.TextAlign = ContentAlignment.MiddleCenter;
        label.Font = new Font("Arial", 14, FontStyle.Bold);
        notifWindow.Controls.Add(label);

        Button okButton = new Button();
        okButton.Text = "of course, mother!";
        okButton.Dock = DockStyle.Bottom;
        okButton.Height = 40;
        okButton.Click += (s, e) => notifWindow.Close();
        notifWindow.Controls.Add(okButton);

        notifWindow.Show();
    }

    private void ShowNotificationPayload(string message, SoundPlayer voiceLine, int notificationLength)
    {
        _androidNotificationPlayer.Play();

        System.Windows.Forms.Timer voiceTimer = new System.Windows.Forms.Timer();
        voiceTimer.Interval = notificationLength;
        voiceTimer.Tick += (s, ev) =>
        {
            voiceTimer.Stop();
            voiceTimer.Dispose();
            voiceLine.Play();
            ShowNotificationWindow(message);
        };
        voiceTimer.Start();
    }

    private void ShowQuestionPayload()
    {
        _youtubeTimer.Stop();
        _loudAlarmPlayer.PlayLooping();

        string[] options = { "siri.Resources.lemon.png", "siri.Resources.lime.png" };
        int lemonCount = 0;
        int limeCount = 0;
        Rectangle screen = Screen.PrimaryScreen.WorkingArea;

        for (int i = 0; i < 3; i++)
        {
            string resourceName = options[_lemonLimeRandom.Next(options.Length)];
            if (resourceName.Contains("lemon")) lemonCount++;
            else limeCount++;

            int x = _lemonLimeRandom.Next(0, screen.Width - 200);
            int y = _lemonLimeRandom.Next(0, screen.Height - 200);
            _fruitPopups.Add(ShowImagePopup(resourceName, x, y));
        }

        Interaction.InputBox("how many remons and rimes are there?", "thailand citizenship test");

        _loudAlarmPlayer.Stop();
        _kRemonsButtPlayer.Play();
        MessageBox.Show("wrong! there are 2 up ur butt! u are not true son of mine!");

        foreach (Form f in _fruitPopups) f.Close();
        _fruitPopups.Clear();
        _youtubeTimer.Start();
    }

    private void StartFinalPayload()
    {
        _finalPayloadInterval = 2000;
        _finalPayloadTimer.Interval = _finalPayloadInterval;
        _finalPayloadTimer.Start();

        PlayStaggeredAudio();
        _audioLoopTimer = new System.Windows.Forms.Timer();
        _audioLoopTimer.Interval = 12 * 800;
        _audioLoopTimer.Tick += (s, e) => PlayStaggeredAudio();
        _audioLoopTimer.Start();

        _goldenFreddyTimer.Start();
    }

    private void FinalPayloadTick(object sender, EventArgs e)
    {
        string[] options = { "siri.Resources.lemon.png", "siri.Resources.lime.png" };
        Rectangle screen = Screen.PrimaryScreen.Bounds;

        for (int i = 0; i < 3; i++)
        {
            string resourceName = options[_finalRandom.Next(options.Length)];
            int x = _finalRandom.Next(0, Math.Max(1, screen.Width - 200));
            int y = _finalRandom.Next(0, Math.Max(1, screen.Height - 200));
            _finalFruitPopups.Add(ShowImagePopup(resourceName, x, y));
        }

        _finalPayloadInterval = Math.Max(100, _finalPayloadInterval - 150);
        _finalPayloadTimer.Interval = _finalPayloadInterval;
    }

    private void ShowGoldenFreddyJumpscare()
    {
        Assembly assembly = Assembly.GetExecutingAssembly();
        using Stream stream = assembly.GetManifestResourceStream("siri.Resources.golden_freddy.png");
        if (stream == null) throw new Exception("golden_freddy.png not found.");

        MemoryStream ms = new MemoryStream();
        stream.CopyTo(ms);
        ms.Position = 0;
        Image img = Image.FromStream(ms);

        Rectangle screen = Screen.PrimaryScreen.Bounds;

        Form popup = new Form();
        popup.FormBorderStyle = FormBorderStyle.None;
        popup.StartPosition = FormStartPosition.Manual;
        popup.TopMost = true;
        popup.ShowInTaskbar = false;
        popup.Location = new Point(screen.X, screen.Y);
        popup.Size = new Size(screen.Width, screen.Height);
        popup.BackColor = Color.Black;

        PictureBox pictureBox = new PictureBox();
        pictureBox.Image = img;
        pictureBox.Dock = DockStyle.Fill;
        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox.BackColor = Color.Black;
        popup.Controls.Add(pictureBox);

        _goldenFreddyPopup = popup;
        _goldenFreddyPlayer.Play();
        popup.Show();
    }

    private void PlayStaggeredAudio()
    {
        string[] audioFiles =
        {
            "siri.Resources.loud_alarm.wav",
            "siri.Resources.vine_boom.wav",
            "siri.Resources.loud_android_notification.wav",
            "siri.Resources.k-1-kirr-you.wav",
            "siri.Resources.k-2-dishonour.wav",
            "siri.Resources.k-3-remons-butt.wav",
            "siri.Resources.k-4-grape-you.wav",
            "siri.Resources.d-1-grape-you.wav",
            "siri.Resources.d-2-take-shit.wav",
            "siri.Resources.d-3-7pm-bedtime.wav",
            "siri.Resources.d-4-fried-rice.wav",
            "siri.Resources.d-5-car-disrespect.wav"
        };

        for (int i = 0; i < audioFiles.Length; i++)
        {
            string resourceName = audioFiles[i];
            System.Windows.Forms.Timer audioTimer = new System.Windows.Forms.Timer();
            audioTimer.Interval = i == 0 ? 1 : i * 800;
            audioTimer.Tick += (s, ev) =>
            {
                audioTimer.Stop();
                audioTimer.Dispose();
                PlayOverlapping(resourceName);
            };
            audioTimer.Start();
        }
    }

    private void label1_Click(object sender, EventArgs e) { }

    private void onButton_Click(object sender, EventArgs e)
    {
        SetFirstInterval();
        _chirpTimer.Start();
        _addressMePopupTimer.Start();
        _youtubeTimer.Start();
        _incomingCallTimer.Start();
        _notificationTimer1.Start();
        _questionTimer.Start();
        _notificationTimer2.Start();
        _notificationTimer3.Start();
        _finalPayloadStartTimer.Start();

        onButton.Enabled = false;
    }

    private void offButton_Click(object sender, EventArgs e)
    {
        _chirpTimer.Stop();
        _addressMePopupTimer.Stop();
        _addressMePopup?.Close();
        _youtubeTimer.Stop();
        _incomingCallTimer.Stop();
        _incomingCallPopup?.Close();
        _incomingCallPlayer.Stop();
        _notificationTimer1.Stop();
        _notificationTimer2.Stop();
        _notificationTimer3.Stop();
        _questionTimer.Stop();
        _loudAlarmPlayer.Stop();
        foreach (Form f in _fruitPopups) f.Close();
        _fruitPopups.Clear();
        _finalPayloadStartTimer.Stop();
        _finalPayloadTimer.Stop();
        _audioLoopTimer?.Stop();
        _audioLoopTimer?.Dispose();
        foreach (Form f in _finalFruitPopups) f.Close();
        _finalFruitPopups.Clear();
        foreach (WaveOutEvent output in _activeOutputs.ToList())
        {
            output.Stop();
            output.Dispose();
        }
        _activeOutputs.Clear();
        _goldenFreddyTimer.Stop();
        _goldenFreddyPopup?.Close();

        onButton.Enabled = true;
    }

    private void Form1_Load(object sender, EventArgs e) { }
}