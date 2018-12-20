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
        ConfigureService configureService = new ConfigureService();
        PlaceCheckService placeCheckService = new PlaceCheckService();

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

        public void PlaceCooledContainer(Container container, Ship ship) //Test verschil tussen cooled en normal
        {
            for (int i = 0; i < ship.Width; i++) //First row of ship
            {
                if (ContainerIsPlaced(ship, container, i))
                {
                    break;
                }
            }
        }

        public void PlaceStandardContainer(Container container, Ship ship)
        {
            for (int i = 0; i < ship.Places.Count; i++) //All possible places
            {
                //Try to place container on a possible place
                if (ContainerIsPlaced(ship, container, i))
                {
                    break;
                }
            }
        }

        public void PlaceValuableContainer(Container container, Ship ship)
        {
            for (int i = 0; i < ship.Places.Count; i++)
            {
                if (!placeCheckService.PlaceDoesContainOtherValuable(ship.Places[i].Containers))
                {
                    if (placeCheckService.ContainerCanBePlacedOnOthers(container, ship, i))
                    {
                        if (ship.WeightLeft >= ship.WeightRight)
                        {
                            if (ship.Places[i].Placement == Place.PlacementEnum.Right)
                            {
                                if (placeCheckService.ValuableContainerIsReaceable(ship, i))
                                {
                                    PlaceContainer(container, ship, i);
                                    ship.WeightRight += container.Weight;
                                    break;
                                }
                            }
                        }
                        else if (ship.WeightRight > ship.WeightLeft)
                        {
                            if (ship.Places[i].Placement == Place.PlacementEnum.Left)
                            {
                                if (placeCheckService.ValuableContainerIsReaceable(ship, i))
                                {
                                    PlaceContainer(container, ship, i);
                                    ship.WeightLeft += container.Weight;
                                    break;
                                }
                            }
                        }
                        else if(ship.Places[i].Placement == Place.PlacementEnum.Middle)
                        {
                            PlaceContainer(container, ship, i);
                        }
                    }
                }
            }
        }

        public bool ContainerIsPlaced(Ship ship, Container container, int placeNumber)
        {
            if (placeCheckService.ContainerCanBePlacedOnOthers(container, ship, placeNumber))
            {
                if (ship.WeightLeft >= ship.WeightRight)
                {
                    if (ship.Places[placeNumber].Placement == Place.PlacementEnum.Right)
                    {
                        PlaceContainer(container, ship, placeNumber);
                        ship.WeightRight += container.Weight;
                        return true;
                    }
                }
                else if (ship.WeightRight > ship.WeightLeft)
                {
                    if (ship.Places[placeNumber].Placement == Place.PlacementEnum.Left)
                    {
                        PlaceContainer(container, ship, placeNumber);
                        ship.WeightLeft += container.Weight;
                        return true;
                    }
                }
                else if (ship.Places[placeNumber].Placement == Place.PlacementEnum.Middle)
                {
                    PlaceContainer(container, ship, placeNumber);
                    return true;
                }
            }

            return false;
        }

        public void PlaceContainer(Container container, Ship ship, int placeNumber)
        {
            ship.Places[placeNumber].Containers.Add(container);
            ship.Places[placeNumber].Weight += container.Weight;
            ship.Places[placeNumber].Height += 1;
            //configureService.Containers.Remove(container);
        }
    }
}
