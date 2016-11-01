using RayTracingProject.Shapes;
using System;
using System.Collections.Generic;

namespace RayTracingProject
{
    class Scene
    {
        public List<Geometry> objects = new List<Geometry>();
        public ViewPlane view_plane;

        public static int num_objs; // used as a counter & for obj ids
        public color background_color;

        public List<Lightsource> lights = new List<Lightsource>();
        public double ls = 0.5; // light specular factor

        public Scene()
        {
            num_objs = 0;
        }

        public Scene(color c, int height, int width)
        {
            view_plane = new ViewPlane(width, height, 600);
            background_color = c;
            num_objs = 0;
        }

        
        // functions for building objects
        public void build_layers()
        {
            Random r = new Random();

            for (int l = 0; l < 3; l++)
            {
                for (int i = 0; i < 3; i++) // make a 3 X 3
                {
                    for (int j = 0; j < 3; j++)
                        add_shape(new Sphere(new color(130, 130, 130), new Vec3((i * 70.0), (70.0 * (l + 1)), (j * 70.0)), 30, 1, 0.8));
                }
            }
        }

        // the goal is the keep coordinates as ints
        public void pyramid(Vec3 center, bool isUp, color c, double reflectiveness)
        {
            int length = 20; // future put as parameters 
            int height = 30;
            Vec3 top = new Vec3();
            if (isUp)
                top = new Vec3(center.x, center.y + height, center.z);
            else
                top = new Vec3(center.x, center.y - height, center.z);

            // going clockwise, no bottom
            Vec3 c1 = new Vec3(center.x + length, center.y, center.z + length);
            Vec3 c2 = new Vec3(center.x - length, center.y, center.z + length);
            Vec3 c3 = new Vec3(center.x - length, center.y, center.z - length);
            Vec3 c4 = new Vec3(center.x + length, center.y, center.z - length);
            add_shape(new Triangle(c, c1, top, c2, reflectiveness, 0));
            add_shape(new Triangle(c, c2, top, c3, reflectiveness, 0));
            add_shape(new Triangle(c, c3, top, c4, reflectiveness, 0));
            add_shape(new Triangle(c, c4, top, c1, reflectiveness, 0));

        }

        public void checkered_floor(int y, double reflectiveness)
        {
            // make a grid 
            color black = new color(0, 0, 0);
            color white = new color();
            int count = 0;
            double r_size = 30;
            double c_size = 30;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Vec3 c1 = new Vec3(j * c_size, y, i * r_size);
                    Vec3 c2 = new Vec3(j * c_size, y, (i * r_size) + r_size);
                    Vec3 c3 = new Vec3((j * c_size) + c_size, y, (i * r_size) + r_size);
                    Vec3 c4 = new Vec3((j * c_size) + c_size, y, i * r_size);

                    // build either a black or white square
                    if (count % 2 == 0)
                    {
                        add_shape(new Triangle(black, c1, c2, c4, reflectiveness, 0));
                        add_shape(new Triangle(black, c2, c3, c4, reflectiveness, 0));
                    }
                    else
                    {
                        add_shape(new Triangle(white, c1, c2, c4, reflectiveness, 0));
                        add_shape(new Triangle(white, c2, c3, c4, reflectiveness, 0));
                    }
                    count++;
                }
            }
        }

        public void checkered_floor_spheres(int y, double reflectiveness)
        {
            // make a grid 
            color black = new color(0, 0, 0);
            color white = new color();
            int count = 0;
            double r_size = 30;
            double c_size = 30;
            Random rand = new Random();
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    Vec3 c1 = new Vec3(j * c_size, y, i * r_size);
                    Vec3 c2 = new Vec3(j * c_size, y, (i * r_size) + r_size);
                    Vec3 c3 = new Vec3((j * c_size) + c_size, y, (i * r_size) + r_size);
                    Vec3 c4 = new Vec3((j * c_size) + c_size, y, i * r_size);

                    // build either a black or white square
                    if (count % 2 == 0)
                    {
                        add_shape(new Triangle(black, c1, c2, c4, reflectiveness, 0));
                        add_shape(new Triangle(black, c2, c3, c4, reflectiveness, 0));
                    }
                    else
                    {
                        add_shape(new Triangle(white, c1, c2, c4, reflectiveness, 0));
                        add_shape(new Triangle(white, c2, c3, c4, reflectiveness, 0));
                    }

                    // sphere with a random color
                    add_shape(new Sphere(new color(rand.Next(0, 255), rand.Next(0, 255), rand.Next(0, 255)), new Vec3((i * c_size) + c_size/2, 10.0, (j * r_size) + r_size/2), 10, 1, 1));

                    count++;
                }
            }
        }

        // premade functions will display a scene above the checkered tiles
        public void preMade0()
        {
            //checkered_floor(0, 0);
            add_shape(new Sphere(new color(100, 100, 100), new Vec3(50.0, 50.0, 100.0), 50, 0.5, 0.5));
            // added refection adds 
            add_shape(new Sphere(new color(0x5c, 0xff, 0x00), new Vec3(150.0, 50.0, 170.0), 50, 1, 1));
            Program.proj = new Projection(new Vec3(70.0, 100.0, 300.0), new Vec3(100.0, 0.0, 100.0), 45);
        }

        public void preMade1()
        {
            checkered_floor(0, 1.0);
            // left back
            //Program.proj = new Projection(new Vec3(30.0, 150.0, 350.0), new Vec3(200.0, -20.0, 100.0), 45);
            Program.proj = new Projection(new Vec3(0.0, 170.0, 00.0), new Vec3(135.0, -50.0, 135.0), 45);
            pyramid(new Vec3(15, 30, 15), true, new color(0x10, 0xac, 0xff), 0);
            pyramid(new Vec3(15, 30, 15), false, new color(0x10, 0xac, 0xff), 0);

            // right back
            pyramid(new Vec3(255, 30, 15), true, new color(0x10, 0xac, 0xff), 0);
            pyramid(new Vec3(255, 30, 15), false, new color(0x10, 0xac, 0xff), 0);

            // left front
            pyramid(new Vec3(15, 30, 255), true, new color(0x10, 0xac, 0xff), 0);
            pyramid(new Vec3(15, 30, 255), false, new color(0x10, 0xac, 0xff), 0);

            // right front 
            pyramid(new Vec3(255, 30, 255), true, new color(0x10, 0xac, 0xff), 0);
            pyramid(new Vec3(255, 30, 255), false, new color(0x10, 0xac, 0xff), 0);

            add_shape(new Sphere(new color(0xaa, 0xaa, 0xaa), new Vec3(135.0, 50.0, 135.0), 50, 1, 1));

        }

        public void preMade2()
        {
            checkered_floor_spheres(0, 0);
          
            Program.proj = new Projection(new Vec3(70.0, 100.0, 350.0), new Vec3(150.0, -30.0, 100.0), 40);
        }

        //public void preMade3()
        //{
        //    // still under construction
        //    add_shape(new Cube(50, new Vec3(0, 0, 0), new color()));
        //    Program.proj = new Projection(new Vec3(0.0, 150.0, 200.0), new Vec3(0.0, 0.0, 0.0), 50);
        //}

        public void add_shape(Geometry obj)
        {
            objects.Add(obj);
            num_objs++; // 0 is the invalid id
            obj.set_id(num_objs);
        }

        public void SphereFlake(Vec3 center, int radius, int level, color c)
        {
            if (level < 1)
                return;

            add_shape(new Sphere(c, center, radius, 1, 1));

            int nRad = radius / 2;

            // top
            SphereFlake(new Vec3(center.x, (center.y + nRad + radius), center.z), nRad, (level - 1), c);

            // sides
            SphereFlake(new Vec3((center.x + nRad + radius), center.y, center.z), nRad, (level - 1), c);
            SphereFlake(new Vec3((center.x - (nRad + radius)), center.y, center.z), nRad, (level - 1), c);
            SphereFlake(new Vec3(center.x , center.y, (center.z + nRad + radius)), nRad, (level - 1), c);
            SphereFlake(new Vec3(center.x, center.y, center.z - (nRad + radius)), nRad, (level - 1), c);

        }

    }
}
