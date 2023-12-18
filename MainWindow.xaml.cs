using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Hardcodet.Wpf.TaskbarNotification;
using Hardcodet.Wpf.TaskbarNotification.Interop;
using System.Windows.Forms;

namespace Contrursor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Controller controller = new Controller(UserIndex.One);

        public bool sentUpEvent = true;
        public bool sentDownEvent = false;

        public bool rightSentUpEvent = true;
        public bool rightSentDownEvent = false;

        public int sensitivity = 10;

        public bool sentStartKeyEvent = false;

        public int scrollspeed = 50;

        
        static bool upDown = false;
        static bool upUp = true;

        static bool downDown = false;
        static bool downUp = true;

        static bool leftDown = false;
        static bool leftUp = true;

        static bool rightDown = false;
        static bool rightUp = true;

        //NotifyIconData iconData = new NotifyIconData();

        public MainWindow()
        {
            InitializeComponent();
            //iconData.BalloonTitle = "Contrursor";
            DispatcherTimer timer = new DispatcherTimer();
            //NotifyIcon thisicon = new NotifyIcon();
            //BitmapImage bitmapImage = new BitmapImage();
            //thisicon.Icon = new System.Drawing.Icon("pack://application:,,,/icon.ico");
            //thisicon.Text = "Contrursor";
            //thisicon.ShowBalloonTip(0);
            timer.Interval = new TimeSpan(10000);
            timer.Tick += GamePadStuff;
            timer.Start();
        }

        public void GamePadStuff(object? sender, EventArgs e)
        {
            if (controller.IsConnected)
            {
                Gamepad gamepad = controller.GetState().Gamepad;

                //Axis 
                //LeftThumbX, LeftThumbY, RightThumbX, RightThumbY

                //these are short so to get a float from 0 to 1 instead :
                short gx = gamepad.LeftThumbX;
                float tx = (gamepad.LeftThumbX / (float)short.MaxValue) * sensitivity;
                Simulator.MouseX += (int)tx;

                short gy = gamepad.LeftThumbY;
                float ty = (gamepad.LeftThumbY / (float)short.MaxValue) * sensitivity;
                Simulator.MouseY -= (int)ty;

                //Buttons
                GamepadButtonFlags Buttons = gamepad.Buttons;
                if (Buttons.HasFlag(GamepadButtonFlags.Start) && !sentStartKeyEvent)
                {
                    Simulator.PressKey(0x5B);
                    sentStartKeyEvent = true;
                } else
                {
                    sentStartKeyEvent = false;
                }
                if ((Buttons.HasFlag(GamepadButtonFlags.A) || gamepad.LeftTrigger > 0.5f) && !sentDownEvent)
                {
                    Simulator.MouseEvent(Simulator.MouseEventFlags.LEFTDOWN, 0, 0, Simulator.MouseEventDataXButtons.XBUTTON1);
                    sentUpEvent = false;
                    sentDownEvent = true;
                } else if (sentUpEvent == false && !Buttons.HasFlag(GamepadButtonFlags.A) && gamepad.LeftTrigger < 0.5f)
                {
                    Simulator.MouseEvent(Simulator.MouseEventFlags.LEFTUP, 0, 0, Simulator.MouseEventDataXButtons.XBUTTON1);
                    sentUpEvent = true;
                    sentDownEvent = false;
                }

                if (Buttons.HasFlag(GamepadButtonFlags.RightShoulder))
                {
                    Simulator.MouseEvent(Simulator.MouseEventFlags.WHEEL, 0, 0, scrollspeed);
                }

                if (Buttons.HasFlag(GamepadButtonFlags.LeftShoulder))
                {
                    Simulator.MouseEvent(Simulator.MouseEventFlags.WHEEL, 0, 0, -scrollspeed);
                }

                if (gamepad.RightTrigger > 0.5f && !rightSentDownEvent)
                {
                    Simulator.MouseEvent(Simulator.MouseEventFlags.RIGHTDOWN, 0, 0, Simulator.MouseEventDataXButtons.XBUTTON1);
                    rightSentUpEvent = false;
                    rightSentDownEvent = true;
                }
                else if (rightSentUpEvent == false && gamepad.RightTrigger < 0.5f)
                {
                    Simulator.MouseEvent(Simulator.MouseEventFlags.RIGHTUP, 0, 0, Simulator.MouseEventDataXButtons.XBUTTON1);
                    rightSentUpEvent = true;
                    rightSentDownEvent = false;
                }

                #region DPad -> arrow keys
                if (Buttons.HasFlag(GamepadButtonFlags.DPadUp) && !upDown)
                {
                    Simulator.keyDown(0x26);
                    upDown = true;
                    upUp = false;
                } else if (!Buttons.HasFlag(GamepadButtonFlags.DPadUp) && !upUp)
                {
                    Simulator.keyUp(0x26);
                    upDown = false;
                    upUp = true;
                }

                if (Buttons.HasFlag(GamepadButtonFlags.DPadDown) && !downDown)
                {
                    Simulator.keyDown(0x28);
                    downDown = true;
                    downUp = false;
                }
                else if (!Buttons.HasFlag(GamepadButtonFlags.DPadDown) && !downUp)
                {
                    Simulator.keyUp(0x28);
                    downDown = false;
                    downUp = true;
                }

                if (Buttons.HasFlag(GamepadButtonFlags.DPadLeft) && !leftDown)
                {
                    Simulator.keyDown(0x25);
                    leftDown = true;
                    leftUp = false;
                }
                else if (!Buttons.HasFlag(GamepadButtonFlags.DPadLeft) && !leftUp)
                {
                    Simulator.keyUp(0x25);
                    leftDown = false;
                    leftUp = true;
                }

                if (Buttons.HasFlag(GamepadButtonFlags.DPadRight) && !rightDown)
                {
                    Simulator.keyDown(0x27);
                    rightDown = true;
                    rightUp = false;
                }
                else if (!Buttons.HasFlag(GamepadButtonFlags.DPadRight) && !rightUp)
                {
                    Simulator.keyUp(0x27);
                    rightDown = false;
                    rightUp = true;
                }
                #endregion
            }
        }
    }
}
