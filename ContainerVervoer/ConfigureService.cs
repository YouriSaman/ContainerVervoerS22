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
            var ship = new Ship
            {
                MaxWeight = maxWeight,
                Length = length,
                Width = width
            };
            int amountOfPlaces = length * width;
            int placeId = 1;
            for (int i = 0; i < amountOfPlaces; i++)
            {
                ship.Places.Add(new Place{Id = placeId});
                placeId++;
            }

            int position = 0;
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (j < width/2)
                    {
                        ship.Places[i+position].Placement = Place.PlacementEnum.Left;
                        position++;
                    }
                    else if(width % 2 != 0 && j == width/2) //If width is uneven and middle of width
                    {
                        ship.Places[i + position].Placement = Place.PlacementEnum.Middle;
                        position++;
                    }
                    else
                    {
                        ship.Places[i + position].Placement = Place.PlacementEnum.Right;
                        position++;
                    }
                }
                position--;
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

        public bool MaxWeightContainersSchip(Ship ship, Container newContainer)
        {
            return ship.CurrentWeight + newContainer.Weight <= ship.MaxWeight;
        }
    }
}
