using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class PlaceCheckService
    {
        public bool ValuableContainerIsReaceableAfterPlacement(Ship ship, int placeNumber)
        {
            if (placeNumber - ship.Width > 0)
            {
                //If placement height is heigher than the row after that place
                if (ship.Places[placeNumber].Height + 1 > ship.Places[placeNumber - ship.Width - 1].Height)
                {
                    return false;
                }
            }
            else if (placeNumber > ship.Width && ship.Places[placeNumber].Height + 1 > ship.Places[placeNumber + ship.Width - 1].Height)
            {
                return false;
            }
            return true;
        }

        public bool ContainerCanBePlacedOnOthers(Container container, Ship ship, int placeNumber)
        {
            return (ship.Places[placeNumber].Weight + container.Weight) <= 120000;
        }

        public bool PlaceDoesContainOtherValuable(List<Container> containers)
        {
            foreach (var container in containers)
            {
                if (container.Valuable)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
