using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingProject
{
    class Projection
    {
         public Vec3 eye;
    public Vec3 point_at; 
    public double dist;
    public Vec3 u, v, w, up; 
    
    public Projection(Vec3 eye, Vec3 point_at, double FOV){
        this.eye = eye;
        //this.eye.normalize();
        this.point_at = point_at;
        u = new Vec3();
        v = new Vec3();
        w = new Vec3();
        up = new Vec3(0.00424, 1.0, 0.00764);
        
        this.dist = Program.scene.view_plane.height/2/Math.Tan((Math.PI / 180) * FOV); 
       
        calc_uvw();
        //System.out.println("U: " + u.toString() + " V: " + v.toString() + " w: " + w.toString() + "Dist: " + dist);

    }
    
    public Ray make_ray(double x, double y){
        //s = e + ux + vy − wd
        // d = s − e
        Ray r = new Ray(eye, u.scale(x).add(v.scale(y).subtract(w.scale(this.dist))));
        //System.out.println("x:" + x + " y: " + y + " D: " + r.direction);
        r.direction.normalize();
        return r;
    }
    
    public void calc_uvw(){
        w = eye.subtract(point_at);
        w.normalize();
        
        u = up.crossProduct(w);
        u.normalize();
        // right handed coordinate system
        v = w.crossProduct(u);
        v.normalize();
        v.negate();
        }
    }
}
