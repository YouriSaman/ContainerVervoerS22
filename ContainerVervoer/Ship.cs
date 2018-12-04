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
        public int MinWeight { get; set; }
        public int CurrentWeight { get; set; }
        public int WeightLeft { get; set; }
        public int WeightRight { get; set; }
        public decimal Balance { get; set; }
        public int AmountOfContainers { get; set; }
        public int AmountOfLeftoverContainers { get; set; }
        public List<Place> Places { get; }

        public Ship(int maxWeight, int minWeight, int currentWeight, int weightLeft, int weightRight, decimal balance, int amountOfContainers, int amountOfLeftoverContainers, List<Place> places)
        {
            MaxWeight = maxWeight;
            MinWeight = minWeight;
            CurrentWeight = currentWeight;
            WeightLeft = weightLeft;
            WeightRight = weightRight;
            Balance = balance;
            AmountOfContainers = amountOfContainers;
            AmountOfContainers = amountOfLeftoverContainers;
            Places = places;
        }
    }
}
