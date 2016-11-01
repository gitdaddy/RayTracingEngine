using System;
using System.Collections.Generic;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using System.Threading;
using System.IO;

namespace RayTracingProject
{
    class Program
    {
        // *(Color - framework; color - personal class)
        public static Scene scene;
        public static Projection proj;
        public static Surface screen;
        public static BaseOutput OutObject;
        public static int num_threads = 2; // only for SDL output
        public const int s_height = 900;
        public const int s_width = 1500;
        public const bool outToPng = true;
        public static string outPath = Path.GetFullPath(@"..\..\Resource\results.bmp"); // store results in the resource folder
        public const bool AA = false;

        /*
            My own Ray Tracing Engine 
            * Ray Tracing Engines are used to render high quality graphics used by companies like Pixar
                - see https://renderman.pixar.com/view/raytracing-fundamentals
            This file is where the fun starts. 
            Change the comments on your local machine to test premade scenes (lines 41-43) - feel free to make some of your own!
            This program using SDL for real time tracing - make sure your machine is compatable with SdlDotNet http://cs-sdl.sourceforge.net/
        */

        static void Main(string[] args)
        {
            // set up the scene
            scene = new Scene(new color(0x74, 0x8c, 0xab), s_width, s_height);

            // lights setup - currently they all give off the same color of white light
            //scene.lights.Add( new Lightsource( new Vec3(-100.0, 100.0, 0.0), new Vec3(0.0, -1.0, 0.0)));
            scene.lights.Add(new Lightsource(new Vec3(100.0, 100.0, 100.0), new Vec3(0.0, -1.0, 0.0)));

            // Check out the premade scenes one at a time!
            //scene.preMade0(); 
            //scene.preMade1(); 
            //scene.preMade2(); 
            Program.proj = new Projection(new Vec3(50.0, 150.0, 150.0), new Vec3(0.0, 0.0, 0.0), 45);
            scene.SphereFlake(new Vec3(), 30, 3, new color(150, 150, 150));


            if (Program.outToPng)
            {
                OutObject = new OutputPng();
                num_threads = 1;
            }
            else
            {
                OutObject = new OutputSDL();
            }

            Events.TargetFps = 50; // frames update per second 
            Events.Tick += new EventHandler<TickEventArgs>(TickHandler);
            Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuitEventHandler);

            Events.Run();
        }

        private static void ApplicationQuitEventHandler(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }
        

        public static void TickHandler(object sender, TickEventArgs args) // every frame update
        {
            int halfh = s_height / 2;
            int halfw = s_width / 2;

            int xPortion_size = s_width / num_threads;

            // create threads
            List<Thread> t_List = new List<Thread>();
            for (int i = 0; i < num_threads; i++)
            {
                int xstart = i * xPortion_size;
                int xend = Math.Min((xstart + xPortion_size), s_width);
                Thread worker = new Thread(() => OutObject.writePortion(xstart, xend + 1, halfh, halfw));
                worker.Start();
                t_List.Add(worker);
            }

            // join threads
            for (int i = 0; i < num_threads; i++)
            {
                t_List[i].Join(); // join the threads as they complete
            }

            if (outToPng)
            {
                OutObject.saveImage(outPath);
            }
            Events.QuitApplication();
        }

    }
}
