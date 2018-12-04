using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class BalanceService
    {
        public bool BalansCheck(Ship ship)
        {
            BerekenBalans(ship);
            if (ship.Balance >= -20)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Bereken het verschil tussen de linker en rechter zijde
        /// </summary>
        public Ship BerekenBalans(Ship ship)
        {
            var rechts = ship.WeightRight;
            var links = ship.WeightLeft;
            decimal balans;

            if (rechts >= links)
            {
                balans = links - rechts;
                balans = balans / rechts * 100;
            }
            else
            {
                balans = rechts - links;
                balans = balans / links * 100;
            }

            ship.Balance = Math.Round(balans, 2);

            return ship;
        }

        /// <summary>
        /// Schip proberen te balanceren
        /// </summary>
        public Ship BalanseerSchip(Ship ship)
        {
            for (int i = 0; i < ship.Places.Count; i++)
            {
                if (ship.WeightLeft >= ship.WeightRight)
                {
                    if (i == 1 || i == 5 || i == 3)
                    {
                        if (ship.Places[i].Weight != 0)
                        {
                            VerwijderContainerLinks(ship, i);
                            break;
                        }
                    }
                }
                else if (ship.WeightLeft < ship.WeightRight)
                {
                    if (i == 0 || i == 4 || i == 2)
                    {
                        if (ship.Places[i].Weight != 0)
                        {
                            VerwijderContainerRechts(ship, i);
                            break;
                        }
                    }
                }
            }

            return ship;
        }

        public Ship VerwijderContainer(Ship ship, Container container, int plaatsNr)
        {
            ship.Places[plaatsNr].Weight -= container.Weight;
            ship.AmountOfLeftoverContainers += 1;
            ship.Places[plaatsNr].Containers.Remove(container);

            return ship;
        }

        public Ship VerwijderContainerLinks(Ship ship, int plaatsNr)
        {
            var containers = ship.Places[plaatsNr].Containers;
            foreach (var container in containers)
            {
                if (!container.Cooled && !container.Valuable)
                {
                    ship = VerwijderContainer(ship, container, plaatsNr);
                    ship.WeightLeft -= container.Weight;
                    break;
                }

                if (!container.Valuable)
                {
                    ship = VerwijderContainer(ship, container, plaatsNr);
                    ship.WeightLeft -= container.Weight;
                    break;
                }
            }

            return ship;
        }

        public Ship VerwijderContainerRechts(Ship ship, int plaatsNr)
        {
            var containers = ship.Places[plaatsNr].Containers;
            foreach (var container in containers)
            {
                if (!container.Cooled && !container.Valuable)
                {
                    ship = VerwijderContainer(ship, container, plaatsNr);
                    ship.WeightRight -= container.Weight;
                    break;
                }

                if (!container.Valuable)
                {
                    ship = VerwijderContainer(ship, container, plaatsNr);
                    ship.WeightRight -= container.Weight;
                    break;
                }
            }

            return ship;
        }
    }
}
