using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingProject
{
    class color
    {
        
    public int r, g, b; // values only 0 - 255
    public static double kd = 0.8; // these values to be constructed in the future
    public static double ka = 0.3;
    public static double cs = 0.5; // shininess
    
    public color(int r, int g, int b){
        this.r = r; this.g = g; this.b = b;
    }
    
     public color(){
        // white
        this.r = 255; this.g = 255; this.b = 255;
    }
     
    public color(color copy){
        this.r = copy.r; this.g = copy.g; this.b = copy.b;
    }
    
    public void scalar_update(double scalar){
        // scalar > 0
        if (scalar < 0)
            scalar = 0;
        // was using  min
        r = (int)(r * scalar);
        g = (int)(g * scalar);
        b = (int)(b * scalar);
        
    }
    
    public void diffuse_ambient(double factor){
         // kd = diffuse factor, ka = ambient
        color temp = new color(this);
        //Set the pixel to (kd*factor*R, kd*factor*G, kd*factor*B).+ (ka*R, ka*G, Ka*B).
        double kdf = factor * kd;
        temp.scalar_update(kdf);
        temp.add( new color( (int)(ka*r), (int)(ka*g), (int)(ka*b)));
        this.setRGB(temp);
    }
    
    public void setRGB(color c){
        this.r = c.r;
        this.g = c.g;
        this.b = c.b;
    }
    
    public color subtract(color c){
        color ret = new color();
        // clamp
        ret.r = Math.Max((r - c.r), 0);
        ret.g = Math.Max((g - c.g), 0);
        ret.b = Math.Max((b - c.b), 0);
       return ret;
    }
    
     public void specular(double factor, double ls, double shininess){
         color spec = new color(); // white for now
         //TODO ls and cs should be colors and s is the shiny factor
         //double r = factor * cs * ls;
         factor *= shininess;
         spec.scalar_update(factor);
         this.add(spec);
    }
    
    public void add_unclamped(color c){
        r = c.r + r;
        g = c.g + g;
        b = c.b + b;
    }
      
    public void add(color c){
        // clamp used
        r = Math.Min((c.r + r), 255);
        g = Math.Min((c.g + g), 255);
        b = Math.Min((c.b + b), 255);
    }
    
     public void add_reflection(color c){
        c.divide(1.5); 
        this.add(c);
    }
    
    public void divide(double s){
            r = (int)(r / s); // modified
            g = (int)(g / s);
            b = (int)(b / s);
        }
    
    public void clamp(){
        // clamps the values for RGB {0 - 255}
        if (r > 255)
            r = 255;
        if (r < 0)
            r = 0;
        if (g > 255)
            g = 255;
        if (g < 0)
            g = 0;
        if (b > 255)
            b = 255;
        if (b < 0)
            b = 0;
    }

        public void add_shadow()
        {
            this.divide(2);
        }

        public int toInteger()
        {
            return ((int)r<<16|(int)g<<8|(int)b);   
        }
    
    }

   
}
