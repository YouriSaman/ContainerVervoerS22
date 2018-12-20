using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class Ship
    {
        public int MaxWeight { get; set; }
        public int CurrentWeight { get; set; }
        public int WeightLeft { get; set; }
        public int WeightRight { get; set; }
        public decimal Balance { get; set; }
        public int AmountOfContainers { get; set; }
        public int AmountOfLeftoverContainers { get; set; }
        public List<Place> Places { get; }
        public int Width { get; set; }
        public int Length { get; set; }

        //Lege constructor en minder properties

        public Ship()
        {
            Places = new List<Place>();
        }
    }
}
