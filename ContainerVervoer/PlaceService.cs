using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class PlaceService
    {
        private ConfigureService _configureService = new ConfigureService();
        private PlaceCheckService _placeCheckService = new PlaceCheckService();
        private AddContainerService _addContainerService = new AddContainerService();

        public Ship PlaceContainers(Ship ship, List<Container> containers)
        {
            foreach (var container in containers)
            {
                if (container.Cooled)
                {
                    PlaceCooledContainer(container, ship);
                }
                else if (container.Valuable)
                {
                    PlaceValuableContainer(container, ship);
                }
                else if (!container.Valuable && !container.Cooled)
                {
                    PlaceStandardContainer(container, ship);
                }
            }

            return ship;
        }

        private void PlaceCooledContainer(Container container, Ship ship)
        {
            for (int i = 0; i < ship.Width; i++) //First row of ship
            {
                if (ContainerIsPlaced(ship, container, i))
                {
                    container.PlacementHeight = ship.Places[i].Height;
                    break;
                }
            }
        }

        private void PlaceStandardContainer(Container container, Ship ship)
        {
            for (int i = 0; i < ship.Places.Count; i++) //All possible places
            {
                if (ContainerIsPlaced(ship, container, i))
                {
                    container.PlacementHeight = ship.Places[i].Height;
                    break;
                }
            }
        }

        private void PlaceValuableContainer(Container container, Ship ship)
        {
            for (int i = 0; i < ship.Places.Count; i++)
            {
                if (!_placeCheckService.PlaceDoesContainOtherValuable(ship.Places[i].Containers))
                {
                    if (_placeCheckService.ValuableContainerIsReaceableAfterPlacement(ship, i))
                    {
                        PlaceContainerOnCorrectSide(container, ship, i); //This function is a void so we don't need to return the bool
                    }
                }
            }
        }

        private bool ContainerIsPlaced(Ship ship, Container container, int placeNumber)
        {
            return PlaceContainerOnCorrectSide(container, ship, placeNumber);
        }

        private void PlaceContainer(Container container, Ship ship, int placeNumber)
        {
            if (ship.Places[placeNumber].Containers.Count > 0 && container.Valuable)
            {
                _addContainerService.AddValuableContainer(container, ship, placeNumber);
                
            } else
            {
                _addContainerService.AddContainerAndChangeWeightOnLowestContainer(container, ship, placeNumber);
            }
        }

        ///<summary>
        ///Place container on the side with the least weight to keep the ship in balance
        ///</summary>
        public bool PlaceContainerOnCorrectSide(Container container, Ship ship, int placenumber)
        {
            if (_placeCheckService.ContainerCanBePlacedOnOthers(container, ship, placenumber))
            {
                if (ship.WeightLeft >= ship.WeightRight)
                {
                    if (ship.Places[placenumber].Placement == Place.PlacementEnum.Right)
                    {
                        PlaceContainer(container, ship, placenumber);
                        ship.WeightRight += container.Weight;
                        return true;
                    }
                }
                else if (ship.WeightRight > ship.WeightLeft)
                {
                    if (ship.Places[placenumber].Placement == Place.PlacementEnum.Left)
                    {
                        PlaceContainer(container, ship, placenumber);
                        ship.WeightLeft += container.Weight;
                        return true;
                    }
                }
                else if(ship.Places[placenumber].Placement == Place.PlacementEnum.Middle)
                {
                    PlaceContainer(container, ship, placenumber);
                    return true;
                }
            }

            return false;
        }
    }
}
