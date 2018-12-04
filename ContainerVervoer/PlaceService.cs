using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContainerVervoer
{
    public class PlaceService
    {
        private ConfigureService configureService;
        private BalanceService balanceService;

        public PlaceService()
        {
            configureService = new ConfigureService();
            balanceService = new BalanceService();
        }

        public Ship PlaatsContainers(List<Container> containers, Ship ship)
        {
            //Lijsten aan maken om de containers te sorteren op soort
            List<Container> valuableContainers = new List<Container>();
            List<Container> cooledContainers = new List<Container>();
            List<Container> standardContainers = new List<Container>();

            foreach (var container in containers)
            {
                if (container.Valuable)
                {
                    valuableContainers.Add(container);
                }
                else if (container.Cooled)
                {
                    cooledContainers.Add(container);
                }
                else
                {
                    standardContainers.Add(container);
                }
            }

            //Plaats containers per soort
            PlaatsWaardevolleContainers(valuableContainers, ship);

            PlaatsGekoeldeContainers(cooledContainers, ship);

            PlaatsStandaardContainers(standardContainers, ship);

            //Balanseren tot balans klopt!
            while (balanceService.BalansCheck(ship) == false)
            {
                ship = balanceService.BalanseerSchip(ship);
            }

            return ship;
        }

        public void PlaatsWaardevolleContainers(List<Container> valuableContainers, Ship ship)
        {
            for (int i = 0; i < 6; i++)
            {
                if (valuableContainers.Count > 0)
                {
                    if (i != 2 && i != 3) //Plaatsen[0 en 1] zijn vooraan en [4 en 5] achteraan
                    {
                        var valuableContainer = valuableContainers.First();
                        ship.Places[i].Containers.Add(valuableContainer);
                        ship.Places[i].Weight += valuableContainer.Weight;
                        ship.AmountOfLeftoverContainers -= 1;
                        valuableContainers.Remove(valuableContainer);
                    }
                }
            }

            //plaatsen[1, 5] zijn links en nu gevuld met de eventuele waardevolle containers die nu het gewicht voor de linkse kant zorgen
            ship.WeightLeft += ship.Places[1].Weight + ship.Places[5].Weight;
            //plaatsen[0, 4] zijn rechts en nu gevuld met de eventuele waardevolle containers die nu het gewicht voor de rechtse kant zorgen
            ship.WeightRight += ship.Places[0].Weight + ship.Places[4].Weight;
        }

        public void PlaatsGekoeldeContainers(List<Container> gekoeldeContainers, Ship ship)
        {
            if (gekoeldeContainers.Count > 0)
            {
                foreach (var gekoeldContainer in gekoeldeContainers)
                {
                    if (ship.WeightLeft >= ship.WeightRight)
                    {
                        //Kijken of de containers die al geslecteerd zijn niet overbelastend zijn voor de nieuwe container, daarna plaatsen
                        if (configureService.MaxGewichtOpContainer(ship, 0)) //Plaats 0 is vooraan
                        {
                            ship = VoegContainerRechtsToe(ship, gekoeldContainer, 0);
                        }
                    }
                    //Kijken of de containers die al geslecteerd zijn niet overbelastend zijn voor de nieuwe container, daarna plaatsen
                    else
                    {
                        if (configureService.MaxGewichtOpContainer(ship, 1)) //Plaats 1 is vooraan
                        {
                            ship = VoegContainerLinksToe(ship, gekoeldContainer, 1);
                        }
                    }
                }
            }
        }

        public void PlaatsStandaardContainers(List<Container> standaardContainers, Ship ship)
        {
            if (standaardContainers.Count > 0)
            {
                foreach (var standaardContainer in standaardContainers)
                {
                    for (int i = 0; i < ship.Places.Count; i++)
                    {
                        if (ship.WeightLeft >= ship.WeightRight)
                        {
                            if (i == 4 || i == 2 || i == 0) //Eerst achterste rij ter compensatie met (eventueel geplaatste) gekoelde containers
                            {
                                if (configureService.MaxGewichtOpContainer(ship, i))
                                {
                                    ship = VoegContainerRechtsToe(ship, standaardContainer, i);
                                    break;
                                }
                            }
                        }

                        else if (ship.WeightLeft < ship.WeightRight)
                        {
                            if (i == 5 || i == 3 || i == 1) //Eerst achterste rij ter compensatie met (eventueel geplaatste) gekoelde containers
                            {
                                if (configureService.MaxGewichtOpContainer(ship, i))
                                {
                                    ship = VoegContainerLinksToe(ship, standaardContainer, i);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public Ship VoegContainerToe(Ship ship, Container container, int plaatsNr)
        {
            ship.Places[plaatsNr].Containers.Add(container);
            ship.Places[plaatsNr].Weight += container.Weight;
            ship.AmountOfLeftoverContainers -= 1;

            return ship;
        }

        public Ship VoegContainerLinksToe(Ship ship, Container container, int plaatsNr)
        {
            ship = VoegContainerToe(ship, container, plaatsNr);
            ship.WeightLeft += container.Weight;

            return ship;
        }

        public Ship VoegContainerRechtsToe(Ship ship, Container container, int plaatsNr)
        {
            ship = VoegContainerToe(ship, container, plaatsNr);
            ship.WeightRight += container.Weight;

            return ship;
        }
    }
}
