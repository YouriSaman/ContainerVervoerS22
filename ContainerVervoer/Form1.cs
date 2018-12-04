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

        public Form1()
        {
            configureService = new ConfigureService();
            placeService = new PlaceService();
            InitializeComponent();
        }

        private void btnSchipGewicht_Click(object sender, EventArgs e)
        {
            int maxGewicht = Convert.ToInt32(upDownSchipGewicht.Value);
            int length = Convert.ToInt32(upDownLengte.Value);
            int width = Convert.ToInt32(upDownBreedte.Value);
            ship = configureService.NewShip(maxGewicht, length, width);

            lblGewichtContainers.Text = "0";
            lblAantalContainers.Text = "0";
            lblMaxGewicht.Text = Convert.ToString(maxGewicht);
            lblMinGewicht.Text = Convert.ToString(ship.MinWeight);
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

            //Aantal waardevolle containers tellen
            var amountValuable = configureService.CountAmountOfValuableContainers();

            //Kijken of de container nog op het schip mag en kan
            if (newContainer.Valuable == true)
            {
                if (amountValuable + 1 <= 4) //Kijken of het maximum aantal waardevolle containers word overschreden als deze container ook word toegevoegd
                {
                    ToevoegingContainerMetCheck(newContainer);
                }
                else
                {
                    MessageBox.Show("Het maximum van 4 waardevolle containers word hierbij overschreden!");
                }
            }
            else
            {
                ToevoegingContainerMetCheck(newContainer);
            }
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
                ship.AmountOfLeftoverContainers = lBContainers.Items.Count;
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
            //ship = placeService.PlaatsContainers(configureService.Containers, ship); //service 'stuurt' schip terug na plaatsen met de daarbij behorende methodes

            PlacesToListBox();

            ChangeLablesAfterPlacement();

            ChangeClickabilatyOfButtons();
        }

        public void PlacesToListBox()
        {
            lBSchip.DataSource = ship.Places[0].Containers;
        }

        public void ChangeLablesAfterPlacement()
        {
            //if (ship.AmountOfContainers != 0)
            //{
            //    lblOverGebleven.Visible = true;
            //    lblOverGebleven.Text = "Er zijn nog " + Convert.ToString(ship.AmountOfLeftoverContainers) +
            //                           " containers over die niet worden of zullen geplaatst";
            //}

            lblBalans.Text = Convert.ToString(ship.Balance);
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
