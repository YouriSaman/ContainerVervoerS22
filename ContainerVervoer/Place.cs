using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class Place
    {
        public List<Container> Containers { get; set; }
        public int Weight { get; set; }

        public Place()
        {
            Containers = new List<Container>();
        }
    }
}
