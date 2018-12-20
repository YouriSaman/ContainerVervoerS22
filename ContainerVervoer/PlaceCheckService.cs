using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class PlaceCheckService
    {
        public bool ValuableContainerIsReaceable(Ship ship, int placeNumber)
        {
            if (placeNumber - ship.Width > 0)
            {
                if (ship.Places[placeNumber].Height + 1 > ship.Places[placeNumber - ship.Width - 1].Height)
                {
                    return true;
                }
            }
            else if (ship.Places[placeNumber].Height + 1 > ship.Places[placeNumber + ship.Width - 1].Height)
            {
                return true;
            }
            return false;
        }

        public bool ContainerCanBePlacedOnOthers(Container container, Ship ship, int placeNumber)
        {
            return (ship.Places[placeNumber].Weight += container.Weight) <= 120000;
        }

        public bool PlaceDoesContainOtherValuable(List<Container> containers)
        {
            foreach (var container in containers)
            {
                if (container.Valuable)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
