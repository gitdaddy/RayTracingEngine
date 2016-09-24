using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingProject.Shapes
{
    class Geometry
    {
      
        public color shape_color;
        public double x0, x1; // this may be strange place to store this
        public String name;
        public double reflectiveness;
        public double shininess; // specular 
        private int id;
    
        public virtual Vec3 hit(Ray r) // this must be overidden
            {
                return new Vec3();
            }
    
        public virtual Vec3 get_surface_n(Vec3 hit)
            {
                return new Vec3();
            }

            public void set_id(int i){
            id = i;
        }
    
        public int get_id(){
            return id;
        }
    
        public bool calc_quadratic(double a, double b, double c)
        {
            //System.out.println("A :" + a + " B: " + b + " C: " + c);

            double discr = b * b - 4 * a * c;
            //System.out.println("Delta: " + discr);

            if (discr < 0)
                return false;
            else if (discr == 0)
                x0 = x1 = -0.5 * b / a;
            else
            {
                double q;
                if (b > 0)
                    q = -0.5 * (b + Math.Sqrt(discr));
                else
                    q = -0.5 * (b - Math.Sqrt(discr));

                x0 = q / a;
                x1 = c / q;
            }
            if (x0 > x1)
            {
                //swap
                double temp = x0;
                x0 = x1;
                x1 = temp;
            }

            return true;
        } 
       
    }
}
