using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class Container
    {
        public int Weight { get; set; }
        public bool Valuable { get; set; }
        public bool Cooled { get; set; }

        public Container(int weight, bool valuable, bool cooled)
        {
            Weight = weight;
            Valuable = valuable;
            Cooled = cooled;
        }

        public override string ToString()
        {
            return "Gewicht= " + Weight + "kg, Waardevol= " + Valuable + ", Gekoeld= " + Cooled;
        }
    }
}
