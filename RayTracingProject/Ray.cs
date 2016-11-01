using RayTracingProject.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingProject
{
    /*
    Ray : The Heart of the program
    Will return back drop color if nothing was hit
    otherwise final color will be ret_color
    */
    class Ray 
    {
        public Vec3 direction;
        public Vec3 origin;
        public color ret_color; // default
        public int prev_hit_id = 0; // used to avoid spawn fighting, 0 = nothing hit yet

        public Ray(Vec3 origin, Vec3 direction){
            ret_color = Program.scene.background_color;
            this.origin = origin;
            this.direction = direction;
        }
    
        public void setRay(Vec3 origin, Vec3 direction){
            this.origin = origin;
            this.direction = direction;
        }

        public bool trace(int recursion_level)
        {
            if (recursion_level > 2) // level of recursion
                return false; // end of reflection 

            // the nearest object should have the smallest x0
            bool hit = false;
            List<Double> t_list = new List<Double>();
            List<Geometry> o_list = new List<Geometry>();
            List<Vec3> hp_list = new List<Vec3>();
            Vec3 hit_point = new Vec3();
            foreach (Geometry o in Program.scene.objects)
            {
                hit_point = o.hit(this);
                if (hit_point != null && o.get_id() != prev_hit_id)
                { // use of short circuting Fix this distance is causing problems
                    hit = true;
                    hp_list.Add(hit_point);
                    o_list.Add(o);
                    t_list.Add(o.x0);
                }
            }

            if (hit)
            {
                int nearest = Utility.arg_min(t_list);
                Geometry o = o_list[nearest];
                ret_color = new color(o.shape_color); // a new color each time 
                hit_point = hp_list[nearest]; // changed get() to []


                // Reflections
                if (recursion_level < 1 && o.reflectiveness > 0)
                {
                    // cannot hit itself
                    add_reflection(hit_point, o, recursion_level);
                }

                // add shadows, Diffuse, and Specular
                int s = 0;
                foreach (Lightsource l in Program.scene.lights)
                {
                    add_das(o.get_surface_n(hit_point), o, l.center); // need to fix for triangles
                    Ray r_shadow = new Ray(hit_point, l.center);
                    s += r_shadow.shadow_trace(o.get_id());
                }
                
                if (s != 0)
                    ret_color.add_shadow();

                return true;
            }
            else
                return false;
           }
       
        public void add_reflection(Vec3 hit_point, Geometry o, int recursion_level)
        {
            Vec3 n = o.get_surface_n(hit_point);

            // reflection = 2(n · v)n - v
            Vec3 r_dir = n.scale((2 * n.dot(Program.proj.eye))).subtract(Program.proj.eye);
            r_dir.normalize();
            Ray reflection = new Ray(hit_point, r_dir);
            reflection.prev_hit_id = o.get_id();
            //System.out.println("Level: " + recursion_level);
            bool r_hit = reflection.trace(recursion_level + 1);
            if (r_hit)
            {
                // add in reflection
                reflection.ret_color.scalar_update(o.reflectiveness);
                this.ret_color.add_reflection(reflection.ret_color);
            }
        }

        // add diffuse and ambient lighting
        public void add_das(Vec3 normal, Geometry o, Vec3 light)
        {
            int s = 64; // the size of the specular lighting
                        // diffuse and ambient factor = surface N * light N
            normal.normalize();

            double theta = normal.dot(light);

            if (theta < 0.0)
                theta = 0.0;
            ret_color.diffuse_ambient(theta);

            // blinn phon specular model
            Vec3 eyeN = Program.proj.eye.get_normalized();
            Vec3 h = eyeN.add(light);
            h.normalize();
            double b = Math.Max(normal.dot(h), 0.0); // TODO add a shiny/diffuse factor for the object
            double amount = Math.Pow(b, s);

            ret_color.specular(amount, Program.scene.ls, o.shininess);
        }

        public int shadow_trace(int o_id)
        {

            foreach (Geometry o in Program.scene.objects)
            {
                Vec3 hitp = o.hit(this);
                if (hitp != null && o.get_id() != o_id)
                {
                    return o.get_id();
                }
            }
            return 0; // no obj was hit
        }
    }
}
