using System.Diagnostics;
using System.Security.Policy;
using static System.Windows.Forms.LinkLabel;
using static VendingBuddiesProject.Form1;
using System.Collections.Generic;

namespace VendingBuddiesProject
{
    public partial class Form1 : Form
    {
        /********* Local Globals ***********/
        double snackTotal = 0.0;
        string docPath = @"C:\Users\stani\Documents\VendingBuddies";
        string filePath = "";

        class Snacks
        {
            public string item1 = "";
            public string item2 = "";
            public string item3 = "";
            public double item1Price = 0.0;
            public double item2Price = 0.0;
            public double item3Price = 0.0;
            public int[] itemNameCount = new int[3];
        }
        Snacks snack = new Snacks();

        IDictionary<string, int> snackNameCounts = new Dictionary<string, int>();

        public Form1()
        {
            InitializeComponent();

            loadItemsFile();

            // Initialize File for Items and Prices with Date Time
            DateTime nowTime = new DateTime();
            if (nowTime != null) { nowTime = System.DateTime.Now; }
            filePath = docPath + "-" + nowTime.Hour.ToString()
                + nowTime.Minute.ToString() + nowTime.Second.ToString();
        }

        public void loadItemsFile()
        {
            // Prompt User to Choose File
            OpenFileDialog openSnackText = new OpenFileDialog();
            if (openSnackText.ShowDialog() == DialogResult.OK)
            {
                // User has selected a file try opening it
                StreamReader snacksTxt = new StreamReader(openSnackText.FileName);
                if (snacksTxt != null)
                {
                    snack.item1 = snacksTxt.ReadLine();
                    one.Text = snack.item1;
                    snack.item2 = snacksTxt.ReadLine();
                    two.Text = snack.item2;
                    snack.item3 = snacksTxt.ReadLine();
                    three.Text = snack.item3;
                    snack.item1Price = double.Parse(snacksTxt.ReadLine());
                    snack.item2Price = double.Parse(snacksTxt.ReadLine());
                    snack.item3Price = double.Parse(snacksTxt.ReadLine());
                    snackNameCounts[snack.item1] = int.Parse(snacksTxt.ReadLine());
                    snackNameCounts[snack.item2] = int.Parse(snacksTxt.ReadLine());
                    snackNameCounts[snack.item3] = int.Parse(snacksTxt.ReadLine());
                }

                snacksTxt.Close();
            }

        }

        /// *************************************************************************************
        /// * Function:  updatePrice 
        /// * 
        /// * Detailed Description:
        /// *   This function takes the name of the item, the price, and adds it to the global
        /// * price to output and write to a text file.
        /// *
        /// * Inputs: String itemName and double Price
        /// *
        /// * Ouputs: ListBox populated with all the items in the order, and total price.
        /// *
        /// * Returns: Void
        /// *************************************************************************************
        public void updatePrice(string itemName, double price, bool itemAvailable)
        {
            StreamWriter fileXfer = File.AppendText(filePath);
            string line = "";

            if (itemAvailable)
            {
                itemsPrice.Items.Add(itemName + "$" + price);
                snackTotal += price;
                totalPrice.Items.Clear();
                totalPrice.Items.Add("Total Price: $" + Convert.ToString(Math.Round(snackTotal, 2)));
                line = itemName + "$" + Convert.ToString(price);
            }
            else
            {
                itemsPrice.Items.Add("Item Not Available");
                line = "SOLD OUT\n" + itemName;
            }
            fileXfer.WriteLine(line);
            fileXfer.Close();
        }

        /// *************************************************************************************
        /// * Function:  creditCard 
        /// * 
        /// * Detailed Description:
        /// *   This function clears the list boxes and updates the textfile with the total price
        /// * of the credit payment.
        /// *
        /// * Inputs: None
        /// *
        /// * Ouputs: Textfile results of total price and cleared list boxes in form.
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void creditCard_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Credit Payment", "Insert Credit Card", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                string line = "";
                totalPrice.Items.Clear();
                itemsPrice.Items.Clear();

                StreamWriter fileXfer = File.AppendText(filePath);
                line = ("Credit Card Payment: $" + snackTotal);
                fileXfer.WriteLine(line);
                fileXfer.Close();
            }
            else if (dialogResult == DialogResult.No)
            {
                itemsPrice.Items.Add("Credit Card Payment Canceled");
            }
        }

        bool checkInventory(string snackItem)
        {
            int count = 0;
            count = snackNameCounts[snackItem];

            if (count != 0)
            {
                --snackNameCounts[snackItem];
                return true;
            }
            else
            {
                return false;
            }

        }

        /// *************************************************************************************
        /// * Function:  SunChips 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void button5_Click(object sender, EventArgs e)
        {

        }

        /// *************************************************************************************
        /// * Function:  First Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void one_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.item1);

            updatePrice(snack.item1 + ": ", snack.item1Price, itemAvailable);
        }

        /// *************************************************************************************
        /// * Function: Second Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void two_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.item2);

            updatePrice(snack.item2 + ": ", snack.item2Price, itemAvailable);
        }

        /// *************************************************************************************
        /// * Function:  Third Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void three_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.item3);

            updatePrice(snack.item3 + ": ", snack.item3Price, itemAvailable);
        }

        /// *************************************************************************************
        /// * Function: Reeses Peanut Butter Cups
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void reesesPeanutButterCups_Click(object sender, EventArgs e)
        {
            //updatePrice("Reeses Peanut Butter Cup: ", snackPrices.reesesPeanutButterCups);
        }

        /// *************************************************************************************
        /// * Function: Planters Trail Mix
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void plantersTrailMix_Click(object sender, EventArgs e)
        {
            //updatePrice("Planters Trail Mix: ", snackPrices.plantersTrailMix);
        }

        /// *************************************************************************************
        /// * Function: Granola Bars
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void granolaBars_Click(object sender, EventArgs e)
        {
            //updatePrice("Granola Bars: ", snackPrices.granolaBars);
        }

        /// *************************************************************************************
        /// * Function: Cheez Its
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void cheezIts_Click(object sender, EventArgs e)
        {
            //updatePrice("Cheeze-Its: ", snackPrices.cheezIts);
        }

        /// *************************************************************************************
        /// * Function: Chex Mix
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void chexMix_Click(object sender, EventArgs e)
        {
            //updatePrice("Chex Mix: ", snackPrices.chexMix);
        }

        /// *************************************************************************************
        /// * Function: Pretzels
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void pretzels_Click(object sender, EventArgs e)
        {
            //updatePrice("Pretzels: ", snackPrices.Pretzels);
        }

        /// *************************************************************************************
        /// * Function: cancelOrder
        /// * 
        /// * Detailed Description:
        /// *   When clicked clear the list boxes and update the textFile that the order is 
        /// * canceled.
        /// *
        /// * Inputs: None
        /// *
        /// * Ouputs: "Order Canceled" to textFile.
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void cancelOrder_Click(object sender, EventArgs e)
        {
            string line = "";
            totalPrice.Items.Clear();
            itemsPrice.Items.Clear();

            StreamWriter fileXfer = File.AppendText(filePath);
            line = ("Order Canceled");
            fileXfer.WriteLine(line);
            fileXfer.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void itemsPrice_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void totalPrice_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

    }
}