using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingProject.Shapes
{
    class Triangle : Geometry
    {
        public Vec3 A;
        public Vec3 B;
        public Vec3 C;

        public Vec3 normal;
    
    public Triangle(color color, Vec3 a, Vec3 b, Vec3 c, double r, double s){
        A = a;
        B = b; 
        C = c; 
        shape_color = color;
        name = "triangle";
        reflectiveness = r;
        shininess = s; // specular shine
         //The cross product of two sides of the triangle equals the surface normal
        Vec3 u = this.B.subtract(A);
        Vec3 v = this.C.subtract(A);
        // the surface normal is based on the ABC points not the hit point unlike curved surfaces
        normal = u.crossProduct(v); // CHECK this may be the surface normal up or down 
        normal.normalize(); // precompute the normal
    }
    
    public override Vec3 hit(Ray r) {
        // 1. check if it hits the plane
        Vec3 i = new Vec3();
        
        // if the ray.dir is parallel
        if (r.direction.dot(normal) == 0.0)
            return null;
        
        if (intersect_plane(A ,r.origin, r.direction)){
            i = new Vec3(r.origin.add(r.direction.scale(x0)));
        }
        else
            return null;
        
        // 2. Inside outside test
        
        // edges of the triangle
        Vec3 e0 = B.subtract(A);
        Vec3 e1 = this.C.subtract(B);
        Vec3 e2 = A.subtract(this.C); // interesting...
        
        // to hit point
        Vec3 p0 = i.subtract(A);
        Vec3 p1 = i.subtract(B);
        Vec3 p2 = i.subtract(this.C);
        
        // Cross check to see if its inside
        Vec3 C = e0.crossProduct(p0);
        if (normal.dot(C) < 0)
            return null;
        
        C = e1.crossProduct(p1);
        if (normal.dot(C) < 0)
            return null;
        
        C = e2.crossProduct(p2);
        if (normal.dot(C) < 0)
            return null;

        return i;
    }

    public override Vec3 get_surface_n(Vec3 hit)
    {
       return normal;
    }
    
    public bool intersect_plane(Vec3 p0, Vec3 r_origin, Vec3 r_direction)
    {
          // t = (plane point - r.origin) * n / dir * n
        x0 = p0.subtract(r_origin).dot(normal) / r_direction.dot(normal);
        if (x0 > 10e-8)
            return true;
        else
            return false;
    }
      
    }
}
