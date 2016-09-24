using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RayTracingProject.Shapes
{
    class Cube : Geometry
    {
        public Cube(int scale, Vec3 center, color c)
        {
            int neg = scale * -1;
            int one = scale;

            // top side
            Program.scene.add_shape(new Triangle(c, 
                    new Vec3(neg, one, neg),
                    new Vec3(neg, one, one),
                    new Vec3(one, one, one), 0.0, 0.0));


            Program.scene.add_shape(new Triangle(c,
                    new Vec3(one, one, one),
                    new Vec3(neg, one, neg),
                    new Vec3(one, one, neg), 0.0, 0.0));

            // #2
            Program.scene.add_shape(new Triangle(c,
                     new Vec3(neg, neg, neg),
                     new Vec3(neg, one, neg),
                     new Vec3(neg, one, one), 0.0, 0.0));
            Program.scene.add_shape(new Triangle(c,
                    new Vec3(neg, one, one),
                    new Vec3(neg, neg, one),
                    new Vec3(neg, neg, neg), 0.0, 0.0));

            // 3
            Program.scene.add_shape(new Triangle(c,
                    new Vec3(neg, one, neg),
                    new Vec3(neg, neg, neg),
                    new Vec3(one, neg, neg), 0.0, 0.0));
            Program.scene.add_shape(new Triangle(c,
                    new Vec3(neg, one, neg),
                    new Vec3(one, one, neg),
                    new Vec3(one, neg, neg), 0.0, 0.0));
            // 4
            Program.scene.add_shape(new Triangle(c,
                    new Vec3(one, one, neg),
                    new Vec3(one, one, one),
                    new Vec3(one, neg, neg), 0.0, 0.0));
            Program.scene.add_shape(new Triangle(c,
                    new Vec3(one, one, neg),
                    new Vec3(one, one, one),
                    new Vec3(one, neg, one), 0.0, 0.0));
            // 5
            Program.scene.add_shape(new Triangle(c,
                    new Vec3(neg, one, one),
                    new Vec3(neg, neg, one),
                    new Vec3(one, one, one), 0.0, 0.0));
            Program.scene.add_shape(new Triangle(c,
                    new Vec3(one, one, one),
                    new Vec3(neg, neg, one),
                    new Vec3(one, neg, one), 0.0, 0.0));
            // 6
            Program.scene.add_shape(new Triangle(c,
                    new Vec3(neg, neg, one),
                    new Vec3(neg, neg, neg),
                    new Vec3(one, neg, neg), 0.0, 0.0));
            Program.scene.add_shape(new Triangle(c,
                    new Vec3(neg, neg, one),
                    new Vec3(one, neg, neg),
                    new Vec3(one, neg, one), 0.0, 0.0));

        }
    }
}
