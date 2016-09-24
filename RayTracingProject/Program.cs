using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SdlDotNet.Graphics;
using SdlDotNet.Core;
using SdlDotNet.Input;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;

namespace RayTracingProject
{
    class Program
    {
        // *(Color - framework; color - personal class)
        public static Scene scene;
        public static Projection proj;
        private static Surface screen;
        private static int s_height = 768;
        private static int s_width = 1024;
        private static int num_threads = 1; // feel free to play around with the multi-threading capabilities

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

            // lights setup
            scene.lights.Add( new Lightsource( new Vec3(-100.0, 100.0, 0.0), new Vec3(0.0, -1.0, 0.0)));
            scene.lights.Add(new Lightsource(new Vec3(100.0, 100.0, 0.0), new Vec3(0.0, -1.0, 0.0)));

            // Check out the premade scenes one at a time!
            //scene.preMade0(); 
            scene.preMade1(); 
            //scene.preMade2(); 

            screen = Video.SetVideoMode(s_width, s_height, 32, false, false, false, true);

            Events.TargetFps = 50; // frames update per second 
            Events.Tick += new EventHandler<TickEventArgs>(TickHandler);
            Events.Quit += new EventHandler<QuitEventArgs>(ApplicationQuitEventHandler);

            Events.Run();
        }


        private static void ApplicationQuitEventHandler(object sender, QuitEventArgs args)
        {
            Events.QuitApplication();
        }

        public static int arg_min(List<Double> list)
        {
            double current = Double.MaxValue;
            int i = 0;
            int ret = 0;
            foreach (double n in list)
            {
                if (Math.Abs(n) < current)
                {  // n closest to the origin of the ray
                    current = n;
                    ret = i;
                }
                i++;
            }
            return ret;
        }

        

        public static void TickHandler(object sender, TickEventArgs args) // every frame update
        {
            screen.Fill(Color.Black);

            int halfh = s_height / 2;
            int halfw = s_width / 2;

            int xPortion_size = s_width / num_threads;

            // create threads
            List<Thread> t_List = new List<Thread>();
            for (int i = 0; i < num_threads; i++)
            {
                int xstart = i * xPortion_size;
                int xend = Math.Min((xstart + xPortion_size), s_width);
                Thread worker = new Thread(() => writePortion(xstart, xend + 1, halfh, halfw));
                worker.Start();
                t_List.Add(worker);
            }

            // join threads
            for (int i = 0; i < num_threads; i++)
            {
                t_List[i].Join(); // join the threads as they complete
            }

            Console.Out.WriteLine("Closing application soon...");
            Thread.Sleep(10000);
            //Events.QuitApplication();
        }

        public static void writePortion(int xstart, int xend, int halfh, int halfw)
        {
            // precomputing AntiAliasing
            bool AA = false;
            int sample = 4;
            int s2 = sample * sample;

            for (int x = xstart; x < xend - 1; x++)
            {
                for (int y = 0; y < s_height - 1; y++)
                {
                    Color result = new Color();
                    if (AA)
                    {
                        color temp = aAliasing(x, y, sample, s2);
                        result = Color.FromArgb(temp.r, temp.g, temp.b);
                    }
                    else
                    {
                        Ray ray = proj.make_ray((x - halfw), (y - halfh));
                        ray.trace(0);
                        result = Color.FromArgb(ray.ret_color.r, ray.ret_color.g, ray.ret_color.b);
                    }
                    
                    Color[,] colorBlock = new Color[1, 1] { { result } };
                    screen.SetPixels(new Point(x, y), colorBlock);
                    // every pixel
                    //screen.Update();
                }

                // update a colum at a time
                screen.Update();
            }
           
        }

        public static List<Double[]> read_obj_file(String fName)
        {
            var lines = File.ReadAllLines(fName);
            //List of double[]. Each entry of the list contains 3D vertex x,y,z in double array form
            var verts = lines.Where(l => Regex.IsMatch(l, @"^v(\s+-?\d+\.?\d+([eE][-+]?\d+)?){3,3}$"))
                .Select(l => Regex.Split(l, @"\s+", RegexOptions.None).Skip(1).ToArray()) //Skip v
                .Select(nums => new double[] { double.Parse(nums[0]), double.Parse(nums[1]), double.Parse(nums[2]) })
                .ToList();

            //List of int[]. Each entry of the list contains zero based index of vertex reference
            /*Obj format is 1 based index. This is converting into C# zero based, so on write out you need to convert back.
            var faces = lines.Where(l => Regex.IsMatch(l, @"^f(\s\d+(\/+\d+)?){3,3}$"))
                .Select(l => Regex.Split(l, @"\s+", RegexOptions.None).Skip(1).ToArray())//Skip f
                .Select(i => i.Select(a => Regex.Match(a, @"\d+", RegexOptions.None).Value).ToArray())
                .Select(nums => new int[] { int.Parse(nums[0]) - 1, int.Parse(nums[1]) - 1, int.Parse(nums[2]) - 1 })
                .ToList();

            // print test
            */

            System.Console.WriteLine("Vertices :" + verts.ToString());

            foreach (double[] vertex in verts)
            {
                System.Console.WriteLine("x: " + vertex[0].ToString() + " y:" + vertex[1].ToString() + " z:" + vertex[2].ToString());
            }

            return verts;
        }

        public static color aAliasing(int x, int y, int sample, int s2)
        {
            // get a sample of the surrounding colors and return the average of all
            color r = new color();
            int half = sample / 2;
            for (int i = 0; i < sample; i++)
            {
                for (int j = 0; j < sample; j++)
                {
                    Ray ray = proj.make_ray((x - half + i), (y - half + i));
                    ray.trace(0);
                    // this calculation must be unclamped
                    r.add_unclamped(ray.ret_color); // summation of colors
                }
            }

            r.divide(s2);
            r.clamp();
            return r;
        }
    }
}
