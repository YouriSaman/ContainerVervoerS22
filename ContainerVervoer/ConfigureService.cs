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
            var ship = new Ship();
            ship.MaxWeight = maxWeight;
            ship.Length = length;
            ship.Width = width;
            int amountOfPlaces = length * width;
            int placeId = 1;
            for (int i = 0; i < amountOfPlaces; i++)
            {
                ship.Places.Add(new Place{Id = placeId});
                placeId++;
            }

            int placeNumberAddition = 0;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j < width/2)
                    {
                        ship.Places[i+placeNumberAddition].Placement = Place.PlacementEnum.Left;
                        placeNumberAddition++;
                    }
                    else if(width % 2 != 0 && j == width/2)
                    {
                        ship.Places[i + placeNumberAddition].Placement = Place.PlacementEnum.Middle;
                        placeNumberAddition++;
                    }
                    else
                    {
                        ship.Places[i + placeNumberAddition].Placement = Place.PlacementEnum.Right;
                        placeNumberAddition++;
                    }
                }
                placeNumberAddition--;
            }

            return ship;
        }

        public bool PreventCapsize(Ship ship)
        {
            return ship.CurrentWeight >= CalcMinWeight(ship.MaxWeight);
        }

        public int CalcMinWeight(int maxWeight)
        {
            return maxWeight / 2;
        }

        /// <summary>
        /// Word met het toevoegen van een nieuwe container het maximum niet overschreden
        /// </summary>
        public bool MaxWeightContainersSchip(Ship ship, Container newContainer)
        {
            return ship.CurrentWeight + newContainer.Weight <= ship.MaxWeight;
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
