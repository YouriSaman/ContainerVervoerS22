using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class ConfigureService
    {
        public List<Container> Containers;

        public ConfigureService()
        {
            Containers = new List<Container>();
        }

        public Ship NewShip(int maxWeight, int length, int width)
        {
            int minWeight = CalculateMinWeight(maxWeight);
            var places = new List<Place>();
            int containerAmountOfShip = length * width;
            for (int i = 0; i < containerAmountOfShip; i++)
            {
                places.Add(new Place());
            }
            Ship ship = new Ship(maxWeight, minWeight, 0, 0, 0, 0, 0, 0, places);

            return ship;
        }

        public int CalculateMinWeight(int maxWeight)
        {
            return maxWeight / 2;
        }

        public bool PreventCapsize(Ship ship)
        {
            return ship.CurrentWeight >= ship.MinWeight;
        }

        /// <summary>
        /// Word met het toevoegen van een nieuwe container het maximum niet overschreden
        /// </summary>
        public bool MaxWeightContainersSchip(Ship ship, Container newContainer)
        {
            return ship.CurrentWeight + newContainer.Weight <= ship.MaxWeight;
        }

        public int CountAmountOfValuableContainers()
        {
            int amountValuable = 0;
            foreach (var container in Containers)
            {
                if (container.Valuable == true)
                {
                    amountValuable++;
                }
            }

            return amountValuable;
        }

        /// <summary>
        /// Mag niet meer dan 120000 kg op de container
        /// </summary>
        public bool MaxGewichtOpContainer(Ship ship, int plaatsNr)
        {
            return ship.Places[plaatsNr].Weight <= 120000;
        }
    }
}
