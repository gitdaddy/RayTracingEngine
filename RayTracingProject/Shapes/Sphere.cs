using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingProject.Shapes
{
    class Sphere : Geometry
    {
        public Vec3 center;
        public int r;
        public int r2;

        public Sphere(color c, Vec3 center, int r, double reflection, double s)
        {
            this.shape_color = c;
            this.center = center;
            this.r = r;
            this.r2 = r * r;
            name = "sphere";
            reflectiveness = reflection;
            shininess = s; // specular shine
        }

        public override Vec3 get_surface_n(Vec3 hit)
        {
            //N = ((x - cx)/R, (y - cy)/R, (z - cz)/R)
            return new Vec3((hit.x - center.x) / r, (hit.y - center.y) / r, (hit.z - center.z) / r);
        }


        public override Vec3 hit(Ray ray)
        {
            double a, b, c;
            //if (ray.d2 > this.r2) return null;

            Vec3 L = ray.origin.subtract(this.center);
            a = 1.0; // assumes that ray.dir is normalized

            b = 2 * ray.direction.dot(L);

            c = L.dot(L) - r2;

            if (!calc_quadratic(a, b, c))
            {
                return null;
            }

            // ensures that x0 is positive
            if (x0 < 0)
            {
                x0 = x1; // if t0 is negative, let's use t1 instead 
                if (x0 < 0)
                    return null; // both t0 and t1 are negative 
            }

            return new Vec3(ray.origin.add(ray.direction.scale(x0)));

        }
    }
}