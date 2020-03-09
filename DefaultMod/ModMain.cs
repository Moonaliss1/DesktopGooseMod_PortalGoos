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
        // Gets called automatically, passes in a class that contains pointers to

        Vector2 p1pos = new Vector2(300, 500);
        Vector2 p2pos = new Vector2(900, 500);
        Image p1img = Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "p1.png"));
        Image p2img = Image.FromFile(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "p2.png"));
        bool justTeleported = false;

        // useful functions we need to interface with the goose.
        void IMod.Init()
        {
            // Subscribe to whatever events we want
            justTeleported = false;
            InjectionPoints.PostTickEvent += PostTick;
            InjectionPoints.PreRenderEvent += PreRender;
            
        }

        private void PreRender(GooseEntity goos, Graphics g)
        {
            g.DrawImage(p1img, p1pos.x - p1img.Size.Width / 2, p1pos.y - p1img.Size.Height / 2);
            g.DrawImage(p2img, p2pos.x - p2img.Size.Width / 2, p2pos.y - p2img.Size.Height / 2);
        }

        public void PostTick(GooseEntity g)
        {
            if (!justTeleported)
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

            if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.P))
            {

                if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.D1))
                {
                    p1pos = new Vector2(Input.mouseX, Input.mouseY);
                }
                if (System.Windows.Input.Keyboard.IsKeyDown(System.Windows.Input.Key.D2))
                {
                    p2pos = new Vector2(Input.mouseX, Input.mouseY);
                }

            }
            
            // If we're running our mod's task
            if (g.currentTask == API.TaskDatabase.getTaskIndexByID("Portal1"))
            {
                g.targetPos = p1pos;
            }
            if (g.currentTask == API.TaskDatabase.getTaskIndexByID("Portal2"))
            {
                g.targetPos = p2pos;
            }
        }  
        
    }
}
