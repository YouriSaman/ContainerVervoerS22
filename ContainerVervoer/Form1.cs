using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ContainerVervoer
{
    public partial class Form1 : Form
    {
        private Ship ship;
        private ConfigureService configureService;
        private PlaceService placeService;
        private BalanceService balanceService;
        private SortingService sortingService = new SortingService();

        public Form1()
        {
            InitializeComponent();
            configureService = new ConfigureService();
            placeService = new PlaceService();
            balanceService = new BalanceService();
        }

        private void btnSchipGewicht_Click(object sender, EventArgs e)
        {
            int maxWeight = Convert.ToInt32(upDownSchipGewicht.Value);
            int length = Convert.ToInt32(upDownLengte.Value);
            int width = Convert.ToInt32(upDownBreedte.Value);
            ship = configureService.NewShip(maxWeight, length, width);

            lblGewichtContainers.Text = "0";
            lblAantalContainers.Text = "0";
            lblMaxGewicht.Text = Convert.ToString(maxWeight);
            lblMinGewicht.Text = Convert.ToString(configureService.CalcMinWeight(maxWeight));
            btnNieuwSchip.Enabled = false;
            btnVoegContainerToe.Enabled = true;
            btnDeleteContainer.Enabled = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void btnVoegContainerToe_Click(object sender, EventArgs e)
        {
            int weight = Convert.ToInt16(upDownContainerGewicht.Value);
            bool valuable = false;
            bool cooled = false;

            //Type container bekijken én aanmaken in het systeem
            if (rBtnWaardevol.Checked) valuable = true;
            if (rBtnGekoeld.Checked) cooled = true;
            var newContainer = new Container(weight, valuable, cooled);

            ToevoegingContainerMetCheck(newContainer);
        }

        private void ToevoegingContainerMetCheck(Container newContainer)
        {
            if (configureService.MaxWeightContainersSchip(ship, newContainer))
            {
                ship.CurrentWeight += newContainer.Weight;

                //Container toevoegen aan lijst en listbox en de labels aanpassen
                configureService.Containers.Add(newContainer);

                lblGewichtContainers.Text = Convert.ToString(ship.CurrentWeight);
                lBContainers.Items.Add(newContainer.ToString());
                lblAantalContainers.Text = Convert.ToString(lBContainers.Items.Count);
                ship.AmountOfContainers = lBContainers.Items.Count;
            }
            else
            {
                MessageBox.Show("Gewicht zorgt ervoor dat het maximum gewicht op het schip wordt overschreden!");
            }

            if (configureService.PreventCapsize(ship))
            {
                btnStartVerdeling.Enabled = true;
            }
        }

        private void btnStartVerdeling_Click(object sender, EventArgs e)
        {
            ship = placeService.PlaceContainers(ship, configureService.Containers);

            PlacesToListBox();

            ChangeLablesAfterPlacement();

            ChangeClickabilatyOfButtons();
        }

        public void PlacesToListBox()
        {
            foreach (var place in ship.Places)
            {
                lBSchip.Items.Add(place);

                place.Containers = sortingService.SortContainers(place.Containers);

                foreach (var container in place.Containers) //First show the heighest container (lowest height) and repeat till lowest container is shown
                {
                    lBSchip.Items.Add(container);
                }

                lBSchip.Items.Add("");
            }
        }

        public void ChangeLablesAfterPlacement()
        {
            lblBalans.Text = Convert.ToString(balanceService.CalculateBalance(ship));
            lblGewichtLinks.Text = Convert.ToString(ship.WeightLeft);
            lblGewichtRechts.Text = Convert.ToString(ship.WeightRight);
            lblTotaalGeplaatstGewicht.Text = Convert.ToString(ship.WeightLeft + ship.WeightRight);
        }

        public void ChangeClickabilatyOfButtons()
        {
            btnVoegContainerToe.Enabled = false;
            btnDeleteContainer.Enabled = false;
            btnStartVerdeling.Enabled = false;
        }

        private void btnDeleteContainer_Click(object sender, EventArgs e)
        {
            var containerIndex = lBContainers.SelectedIndex;
            if (containerIndex == -1)
            {
                MessageBox.Show("Selecteer wel een container om te verwijderen!");
            }
            else
            {
                ship.CurrentWeight = ship.CurrentWeight - configureService.Containers[containerIndex].Weight;
                lBContainers.Items.RemoveAt(containerIndex);
                configureService.Containers.RemoveAt(containerIndex);

                lblAantalContainers.Text = Convert.ToString(lBContainers.Items.Count);
                lblGewichtContainers.Text = Convert.ToString(ship.CurrentWeight);
            }
        }
    }
}
