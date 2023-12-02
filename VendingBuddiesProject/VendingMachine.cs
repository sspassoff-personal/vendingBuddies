using System.Diagnostics;
using System.Security.Policy;
using static System.Windows.Forms.LinkLabel;
using static VendingBuddiesProject.VendingMachine;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace VendingBuddiesProject
{
    public partial class VendingMachine : Form
    {
        /********* Local Globals ***********/
        double snackTotal = 0.0;
        string filePath = "";
        double totalAmount;
        string vendingMachineDirectory;
        string folderPath = "";
        string filePath2 = "";



        class Snacks
        {
            public string[] items = new string[40];
            public double[] itemPrice = new double[40];
            public int[] itemNameCount = new int[40];
        }
        Snacks snack = new Snacks();

        IDictionary<string, int> snackNameCounts = new Dictionary<string, int>();

        Dictionary<string, int> itemsInOrder = new Dictionary<string, int>();

        public VendingMachine()
        {
            InitializeComponent();

            folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filePath = Path.Combine(folderPath, "VendingMachine", "VendingMachine.txt");

            // Ensure the VendingMachine directory exists
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            loadItemsFile();

            filePath2 = Path.Combine(folderPath, "VendingMachine");
            // Initialize File for Items and Prices with Date Time
            DateTime nowTime = new DateTime();
            if (nowTime != null) { nowTime = System.DateTime.Now; }
            string fileName = $"VendingMachine-{nowTime:yyyyMMdd-HHmmss}.txt";
            filePath2 = Path.Combine(filePath2, fileName);
            //filePath2 = Path.Combine(filePath, fileName);
        }

        string location;
        public void loadItemsFile()
        {
            // Prompt User to Choose File
            using (StreamReader snacksTxt = new StreamReader(filePath))
            {

                if (snacksTxt != null)
                {
                    location = snacksTxt.ReadLine();

                    for (int i = 0; i < 40; i++)
                    {
                        string line = snacksTxt.ReadLine();
                        if (line != null)
                        {
                            snack.items[i] = line;
                        }

                    }

                    for (int i = 0; i < 40; i++)
                    {
                        string line = snacksTxt.ReadLine();
                        if (line != null && double.TryParse(line, out double price))
                        {
                            snack.itemPrice[i] = price;
                        }

                    }

                    for (int i = 0; i < 40; i++)
                    {
                        string line = snacksTxt.ReadLine();
                        if (line != null && int.TryParse(line, out int count))
                        {
                            snackNameCounts[snack.items[i]] = count;
                        }

                    }
                    // Display Snack Names to Kiosk
                    displaySnackName();
                }

                snacksTxt.Close();
            }

        }

        public void displaySnackName()
        {
            one.Text = snack.items[0];
            two.Text = snack.items[1];
            three.Text = snack.items[2];
            four.Text = snack.items[3];
            five.Text = snack.items[4];
            six.Text = snack.items[5];
            seven.Text = snack.items[6];
            eight.Text = snack.items[7];
            nine.Text = snack.items[8];
            ten.Text = snack.items[9];
            eleven.Text = snack.items[10];
            twelve.Text = snack.items[11];
            thirteen.Text = snack.items[12];
            fourteen.Text = snack.items[13];
            fifteen.Text = snack.items[14];
            sixteen.Text = snack.items[15];
            seventeen.Text = snack.items[16];
            eighteen.Text = snack.items[17];
            nineteen.Text = snack.items[18];
            twenty.Text = snack.items[19];
            twentyone.Text = snack.items[20];
            twentytwo.Text = snack.items[21];
            twentythree.Text = snack.items[22];
            twentyfour.Text = snack.items[23];
            twentyfive.Text = snack.items[24];
            twentysix.Text = snack.items[25];
            twentyseven.Text = snack.items[26];
            twentyeight.Text = snack.items[27];
            twentynine.Text = snack.items[28];
            thirty.Text = snack.items[29];
            thirtyone.Text = snack.items[30];
            thirtytwo.Text = snack.items[31];
            thirtythree.Text = snack.items[32];
            thirtyfour.Text = snack.items[33];
            thirtyfive.Text = snack.items[34];
            thirtysix.Text = snack.items[35];
            thirtyseven.Text = snack.items[36];
            thirtyeight.Text = snack.items[37];
            thirtynine.Text = snack.items[38];
            fourty.Text = snack.items[39];
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
            StreamWriter fileXfer = File.AppendText(filePath2);
            string line = "";

            if (itemAvailable)
            {
                if (!itemsInOrder.ContainsKey(itemName))
                {
                    itemsInOrder[itemName] = 1;
                }
                else
                {
                    itemsInOrder[itemName] += 1;
                }
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

        private void resetItemsInOrder()
        {
            foreach (var item in itemsInOrder)
            {
                if (snackNameCounts.ContainsKey(item.Key))
                {
                    snackNameCounts[item.Key] += item.Value; // Add back the count
                }
            }
            itemsInOrder.Clear(); // Clear the order tracking
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
                line = ("Payment: $" + snackTotal);
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
            if (snackNameCounts.TryGetValue(snackItem, out count) && count > 0)
            {
                --snackNameCounts[snackItem];
                if (!itemsInOrder.ContainsKey(snackItem))
                {
                    itemsInOrder[snackItem] = 1;
                }
                else
                {
                    itemsInOrder[snackItem]++;
                }
                return true;
            }
            else
            {
                return false;
            }

        }

        /// *************************************************************************************
        /// * Function:  Fourth Item 
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
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[3]);

            updatePrice(snack.items[3] + ": ", snack.itemPrice[3], itemAvailable);
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
            itemAvailable = checkInventory(snack.items[0]);

            updatePrice(snack.items[0] + ": ", snack.itemPrice[0], itemAvailable);
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
            itemAvailable = checkInventory(snack.items[1]);

            updatePrice(snack.items[1] + ": ", snack.itemPrice[1], itemAvailable);
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
            itemAvailable = checkInventory(snack.items[2]);

            updatePrice(snack.items[2] + ": ", snack.itemPrice[2], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function: Sixth item
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
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[5]);

            updatePrice(snack.items[5] + ": ", snack.itemPrice[5], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function: Seventh Item
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
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[6]);

            updatePrice(snack.items[6] + ": ", snack.itemPrice[6], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function: Eigth Item
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
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[7]);

            updatePrice(snack.items[7] + ": ", snack.itemPrice[7], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function: Ninth Item
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
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[8]);

            updatePrice(snack.items[8] + ": ", snack.itemPrice[8], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function: Eleventh Item
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
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[10]);

            updatePrice(snack.items[10] + ": ", snack.itemPrice[10], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function: Twelvth Item
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
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[11]);

            updatePrice(snack.items[11] + ": ", snack.itemPrice[11], itemAvailable);
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

            resetItemsInOrder(); // Reset the counts of items in the order

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

        /// *************************************************************************************
        /// * Function:  Fifth Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void five_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[4]);

            updatePrice(snack.items[4] + ": ", snack.itemPrice[4], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function:  Tenth Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void ten_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[9]);

            updatePrice(snack.items[9] + ": ", snack.itemPrice[9], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function:  Thirteenth Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void thirteen_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[12]);

            updatePrice(snack.items[12] + ": ", snack.itemPrice[12], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function:  Fourteenth Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void fourteen_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[13]);

            updatePrice(snack.items[13] + ": ", snack.itemPrice[13], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function:  Fifteenth Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void fifteen_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[14]);

            updatePrice(snack.items[14] + ": ", snack.itemPrice[14], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function:  Sixteenth Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void sixteen_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[15]);

            updatePrice(snack.items[15] + ": ", snack.itemPrice[15], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function:  Seventeenth Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void seventeen_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[16]);

            updatePrice(snack.items[16] + ": ", snack.itemPrice[16], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function:  Eighteenth Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void eighteen_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[17]);

            updatePrice(snack.items[17] + ": ", snack.itemPrice[17], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function:  Nineteenth Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void nineteen_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[18]);

            updatePrice(snack.items[18] + ": ", snack.itemPrice[18], itemAvailable);
        }

        /// *************************************************************************************
        /// * Function:  Twenty Item 
        /// * 
        /// * Detailed Description:
        /// *  
        /// * Inputs: None
        /// *
        /// * Ouputs: None
        /// *
        /// * Returns: None
        /// *************************************************************************************
        private void twenty_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[19]);

            updatePrice(snack.items[19] + ": ", snack.itemPrice[19], itemAvailable);
        }

        private void twentyone_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[20]);

            updatePrice(snack.items[20] + ": ", snack.itemPrice[20], itemAvailable);
        }

        private void twentytwo_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[21]);

            updatePrice(snack.items[21] + ": ", snack.itemPrice[21], itemAvailable);
        }

        private void twentythree_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[22]);

            updatePrice(snack.items[22] + ": ", snack.itemPrice[22], itemAvailable);
        }

        private void twentyfour_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[23]);

            updatePrice(snack.items[23] + ": ", snack.itemPrice[23], itemAvailable);
        }

        private void twentyfive_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[24]);

            updatePrice(snack.items[24] + ": ", snack.itemPrice[24], itemAvailable);
        }

        private void twentysix_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[25]);

            updatePrice(snack.items[25] + ": ", snack.itemPrice[25], itemAvailable);
        }

        private void twentyseven_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[26]);

            updatePrice(snack.items[26] + ": ", snack.itemPrice[26], itemAvailable);
        }

        private void twentyeight_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[27]);

            updatePrice(snack.items[27] + ": ", snack.itemPrice[27], itemAvailable);
        }

        private void twentynine_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[28]);

            updatePrice(snack.items[28] + ": ", snack.itemPrice[28], itemAvailable);
        }

        private void thirty_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[29]);

            updatePrice(snack.items[29] + ": ", snack.itemPrice[29], itemAvailable);
        }

        private void thirtyone_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[30]);

            updatePrice(snack.items[30] + ": ", snack.itemPrice[30], itemAvailable);
        }

        private void thirtytwo_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[31]);

            updatePrice(snack.items[31] + ": ", snack.itemPrice[31], itemAvailable);
        }

        private void thirtythree_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[32]);

            updatePrice(snack.items[32] + ": ", snack.itemPrice[32], itemAvailable);
        }

        private void thirtyfour_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[33]);

            updatePrice(snack.items[33] + ": ", snack.itemPrice[33], itemAvailable);
        }

        private void thirtyfive_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[34]);

            updatePrice(snack.items[34] + ": ", snack.itemPrice[34], itemAvailable);
        }

        private void thirtysix_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[35]);

            updatePrice(snack.items[35] + ": ", snack.itemPrice[35], itemAvailable);
        }

        private void thirtyseven_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[36]);

            updatePrice(snack.items[36] + ": ", snack.itemPrice[36], itemAvailable);
        }

        private void thirtyeight_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[37]);

            updatePrice(snack.items[37] + ": ", snack.itemPrice[37], itemAvailable);
        }

        private void thirtynine_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[38]);

            updatePrice(snack.items[38] + ": ", snack.itemPrice[38], itemAvailable);
        }

        private void fourty_Click(object sender, EventArgs e)
        {
            bool itemAvailable = true;
            itemAvailable = checkInventory(snack.items[39]);

            updatePrice(snack.items[39] + ": ", snack.itemPrice[39], itemAvailable);
        }

        private void cash_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Please enter the cash amount", "Enter Cash", "0", -1, -1);
            double changeCalc = 0;
            cashChange.Items.Clear();
            if (double.TryParse(input, out double cashReceived))
            {
                if (cashReceived >= snackTotal) // Allowing exact amount
                {
                    changeCalc = cashReceived - snackTotal;
                    cashChange.Items.Add(changeCalc.ToString("0.00"));
                }
                else
                {
                    MessageBox.Show($"Insufficient amount. Total is: {snackTotal.ToString("0.00")}");
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid numeric value.");
            }


            string line = "";
            totalPrice.Items.Clear();
            itemsPrice.Items.Clear();

            StreamWriter fileXfer = File.AppendText(filePath);
            line = ("Payment: $" + snackTotal);
            fileXfer.WriteLine(line);
            fileXfer.Close();
        }
    }
}