using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class AddContainerService
    {
        private void AddContainer(Container container, Ship ship, int placeNumber)
        {
            ship.Places[placeNumber].Containers.Add(container);
            ship.Places[placeNumber].Weight += container.Weight;
            ship.Places[placeNumber].Height += 1;
        }

        public void AddContainerAndChangeWeightOnLowestContainer(Container container, Ship ship, int placeNumber)
        {
            AddContainer(container, ship, placeNumber);
            ship.Places[placeNumber].WeightOnLowestContainer += container.Weight;
        }

        public void AddValuableContainer(Container valuableContainer, Ship ship, int placeNumber)
        {
            foreach (var containerOfPlace in ship.Places[placeNumber].Containers)
            {
                //Replace the valuable container with a standard or cooled container
                if (containerOfPlace.PlacementHeight == 1)
                {
                    containerOfPlace.PlacementHeight = ship.Places[placeNumber].Height + 1;
                    valuableContainer.PlacementHeight = 1;
                    AddContainer(valuableContainer, ship, placeNumber);

                    //If the replaceable container is the only one, change the weight on lowest container
                    if (containerOfPlace.PlacementHeight == ship.Places[placeNumber].Height)
                    {
                        ship.Places[placeNumber].WeightOnLowestContainer += valuableContainer.Weight - containerOfPlace.Weight;
                        break;
                    }

                    ship.Places[placeNumber].WeightOnLowestContainer += valuableContainer.Weight;
                    break;
                }
            }
        }
    }
}
