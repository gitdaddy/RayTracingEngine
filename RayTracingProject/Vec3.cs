using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingProject
{
    class Vec3
    {
        public double x, y, z;
        bool isnormalized = false; 
    
        public Vec3() { 
        x=0.0;y=0.0;z=0.0;
        }
    
        public Vec3(Vec3 copy) { 
        x=copy.x; y=copy.y; z=copy.z;
        }
    
    public Vec3(double x, double y, double z) { 
        this.x = x; this.y = y; this.z = z; 
    }
    
    public Vec3(double x, double y, double z, bool normalized) { 
        this.x = x; this.y = y; this.z = z; 
        isnormalized = normalized;
    }
    
    public double distanctToPoint(Vec3 point){
        // test
        double dx = this.x - point.x;
        double dy = this.y - point.y;
        double dz = this.z - point.z;

        // We should avoid Math.pow or Math.hypot due to perfomance reasons
        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }
    
    public void set(double x, double y, double z) { 
        this.x = x; this.y = y; this.z = z; 
    }
    
    public Vec3 add(Vec3 v){
        // doesn't modify the calling vector 
        return new Vec3(x + v.x, y + v.y, z + v.z);
    }
    
    public void add_update(Vec3 v){
        // doesn't modify the calling vector 
        x += v.x; y += v.y; z += v.z;
    }
    
    public double length(){
       return Math.Sqrt(x * x + y * y + z * z);   
    }
    
    public Vec3 subtract(Vec3 v){
        // doesn't modify
        return new Vec3(this.x - v.x, this.y - v.y, this.z - v.z);
    }
    
    public double dot(Vec3 v){
        return x * v.x + y * v.y + z * v.z;
    }
     
    public Vec3 scale(double scalar){
        // doesn't modify
        return new Vec3(x * scalar, y * scalar, z * scalar);
    }
    
     public void scale_update(double scalar){
       x *= scalar; y *= scalar; z *= scalar;
    }
     
    public Vec3 crossProduct(Vec3 v){
        double i, j, k;
        i = this.y * v.z - v.y * this.z;
        j = v.x * this.z - this.x * v.z;
        k = this.x * v.y - v.x * this.y;
        
        return new Vec3(i,j,k);
    }
    
    public void normalize(){ // converts to the unit vector
        double mag = Math.Sqrt(x*x + y*y + z*z);
        x /= mag;        
        y /= mag;
        z /= mag;
        isnormalized = true;
    }
    
    public Vec3 get_normalized(){
        double mag = Math.Sqrt(x*x + y*y + z*z);
        return new Vec3(x / mag, y / mag, z / mag, true);
    }
    
    public Vec3 plane_normal(Vec3 C, Vec3 P){        
        return C.subtract(P);
    }
    
    public String toString(){
        return "< " + x + ',' + y + ',' + z + " >"; 
    }
    
    public void negate(){
        this.x = -this.x;
        this.y = -this.y;
        this.z = -this.z;
        }
    }

}
