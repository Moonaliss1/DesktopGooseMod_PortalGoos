using System;
using System.Windows;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Input;
using System.Media;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Reflection;
using System.Windows.Input;
// 1. Added the "GooseModdingAPI" project as a reference.
// 2. Compile this.
// 3. Create a folder with this DLL in the root, and *no GooseModdingAPI DLL*
using GooseShared;
using SamEngine;

namespace portalgoos
{
    public class ModEntryPoint : IMod
    {
        [DllImport("user32.dll")]
        public static extern short GetAsyncKeyState(Keys vKey);
        Vector2 p1pos = new Vector2(300, 500);
        Vector2 p2pos = new Vector2(900, 500);
        Image p1img = Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "p1.png"));
        Image p2img = Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "p2.png"));
        bool justTeleported = false;
        bool portalson = true;
        bool p0pressed = false;

        void IMod.Init()
        {
            justTeleported = false;
            InjectionPoints.PostTickEvent += PostTick;
            InjectionPoints.PreRenderEvent += PreRender;
            
        }

        private void PreRender(GooseEntity goos, Graphics g)
        {
            if (portalson)
            {
                g.DrawImage(p1img, p1pos.x - p1img.Size.Width / 2, p1pos.y - p1img.Size.Height / 2);
                g.DrawImage(p2img, p2pos.x - p2img.Size.Width / 2, p2pos.y - p2img.Size.Height / 2);
            }
        }

        public void PostTick(GooseEntity g)
        {
            if (!justTeleported && portalson)
            {
                if ((g.position.x > p1pos.x - p1img.Size.Width / 2) && (g.position.x < p1pos.x + p1img.Size.Width / 2) && (g.position.y > p1pos.y - p1img.Size.Height / 2) && (g.position.y < p1pos.y + p1img.Size.Height / 2))
                {
                    g.position = p2pos;
                    API.Goose.playHonckSound();
                    justTeleported = true;
                }
                else if ((g.position.x > p2pos.x - p2img.Size.Width / 2) && (g.position.x < p2pos.x + p2img.Size.Width / 2) && (g.position.y > p2pos.y - p2img.Size.Height / 2) && (g.position.y < p2pos.y + p2img.Size.Height / 2))
                {
                    g.position = p1pos;
                    API.Goose.playHonckSound();
                    justTeleported = true;
                }
            }
            if ((g.position.x < p1pos.x - p1img.Size.Width / 2 || g.position.x > p1pos.x + p1img.Size.Width / 2 || g.position.y < p1pos.y - p1img.Size.Height / 2 || g.position.y > p1pos.y + p1img.Size.Height / 2) && (g.position.x < p2pos.x - p2img.Size.Width / 2 || g.position.x > p2pos.x + p2img.Size.Width / 2 || g.position.y < p2pos.y - p2img.Size.Height / 2 || g.position.y > p2pos.y + p2img.Size.Height / 2))
            {
                justTeleported = false;
            }

            if ((GetAsyncKeyState((Keys)Enum.Parse(typeof(Keys), "p" , true)) != 0) )
            {

                if ((GetAsyncKeyState((Keys)Enum.Parse(typeof(Keys), "D1", true)) != 0))
                {
                    p1pos = new Vector2(Input.mouseX, Input.mouseY);
                    }
                if ((GetAsyncKeyState((Keys)Enum.Parse(typeof(Keys), "D2", true)) != 0))
                {
                    p2pos = new Vector2(Input.mouseX, Input.mouseY);
                }
                if ((GetAsyncKeyState((Keys)Enum.Parse(typeof(Keys), "D0", true)) != 0) && !p0pressed)
                {
                    portalson = !portalson;
                    p0pressed = true;
                    System.Threading.Thread.Sleep(300);
                    p0pressed = false;
                }

            }
            
            // If we're running our mod's task
            if (g.currentTask == API.TaskDatabase.getTaskIndexByID("Portal1") && portalson)
            {
                g.targetPos = p1pos;
                g.extendingNeck = true;
            }
            if (g.currentTask == API.TaskDatabase.getTaskIndexByID("Portal2") && portalson)
            {
                g.targetPos = p2pos;
                g.extendingNeck = true;
            }
        }  
        
    }
}
