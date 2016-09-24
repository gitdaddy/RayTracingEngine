using RayTracingProject.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingProject
{
    class Lightsource
    {
        public Vec3 center;
        public Vec3 direction;

        public Lightsource(Vec3 c, Vec3 d)
        {
            this.center = c;
            //Program.scene.add_shape( new Cube(5, center, new color(0xff, 0xff, 0xff)));
            this.center.normalize();
            this.direction = d;
            this.direction.normalize();
        }

    }
}
