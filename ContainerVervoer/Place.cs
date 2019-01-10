using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class Place
    {
        public int Id { get; set; }
        public List<Container> Containers { get; set; }
        public int Weight { get; set; }
        public int WeightOnLowestContainer { get; set; }
        public int Height { get; set; }
        public enum PlacementEnum
        {
            Left = 1, Middle = 2, Right = 3
        }
        public PlacementEnum Placement { get; set; } 

        public Place()
        {
            Containers = new List<Container>();
        }

        public override string ToString()
        {
            return "Place " + Id + " Total weight: " + Weight + " Weight on lowest container: " + WeightOnLowestContainer;
        }
    }
}
