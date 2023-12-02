using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading;

namespace ManagementSystem
{
    public partial class ManagementSystem : Form
    {
        class Snacks
        {
            public string[] items = new string[40];
            public double[] itemPrice = new double[40];
            public int[] itemNameCount = new int[40];
            public string vendingMachineLocation;
            public string dateString = "";
            public string[] dateArray = new string[40];
        }

        public ManagementSystem()
        {
            Snacks snacks = new Snacks();
            IDictionary<string, int> snackNameCounts = new Dictionary<string, int>();
            InitializeComponent();

            for (int i = 0; i < snacks.itemNameCount.Length; i++)
            {
                snacks.itemNameCount[i] = 15;
            }

            string docPath = @"C:\Users\stani\Documents\VendingBuddies";
            string filePath = "";

            List<string> snackNames = new List<string>();
            List<double> prices = new List<double>();
            List<double> creditCardPayments = new List<double>();
            Dictionary<string, int> snackCountList = new Dictionary<string, int>();
            Dictionary<string, double> snackPriceAssociation = new Dictionary<string, double>();
            double totalCreditCardPayment = 0;

            Dictionary<string, DateTime> snackExpirationDates = new Dictionary<string, DateTime>();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text Files|*.txt";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = openFileDialog.FileName;
                using (var snacksTxt = new StreamReader(filePath))
                {
                    int lastPaymentIndex = -1;
                    string line;

                    while ((line = snacksTxt.ReadLine()) != null)
                    {
                        if (line.StartsWith("Payment:"))
                        {
                            double payment = double.Parse(line.Split('$')[1]);
                            creditCardPayments.Add(payment);
                            totalCreditCardPayment += payment;
                            lastPaymentIndex = snackNames.Count;
                        }
                        else if (line == "Order Canceled")
                        {
                            if (lastPaymentIndex != -1)
                            {
                                RemoveEntries(lastPaymentIndex);
                            }
                        }
                        else
                        {
                            var parts = line.Split(':');
                            if (parts.Length == 2) // Ensure there are exactly 2 parts
                            {
                                var snackName = parts[0];

                                var pricePart = parts[1].Trim().TrimStart('$');
                                if (double.TryParse(pricePart, out double price))
                                {
                                    snackNames.Add(snackName);
                                    prices.Add(price);

                                    // Update snack count
                                    if (!snackCountList.ContainsKey(snackName))
                                    {
                                        snackCountList[snackName] = 0;
                                    }
                                    snackCountList[snackName]++;

                                    // Associate snack with its first price
                                    if (!snackPriceAssociation.ContainsKey(snackName))
                                    {
                                        snackPriceAssociation[snackName] = price;
                                    }
                                }
                                else
                                {
                                    // Handle invalid price format
                                }
                            }
                        }
                    }

                }
            }


            // Read Second File:
            // Prompt User to Choose File
            OpenFileDialog openSnackText = new OpenFileDialog();
            if (openSnackText.ShowDialog() == DialogResult.OK)
            {
                string date;
                // User has selected a file try opening it
                StreamReader snacksTxt2 = new StreamReader(openSnackText.FileName);
                if (snacksTxt2 != null)
                {
                    // Get Location
                    snacks.vendingMachineLocation = snacksTxt2.ReadLine();
                    locationText.Text = snacks.vendingMachineLocation;

                    // Snack Names
                    for (int i = 0; i < 40; i++)
                    {
                        snacks.items[i] = snacksTxt2.ReadLine();
                    }

                    // Grab snack prices
                    for (int i = 0; i < 40; i++)
                    {
                        snacks.itemPrice[i] = double.Parse(snacksTxt2.ReadLine());
                    }

                    // Grab snack Quantities
                    /* Info not needed from this file */
                    for (int i = 0; i < 40; i++)
                    {
                        snacksTxt2.ReadLine();
                    }

                    // Grab the expiration Dates:
                    for (int i = 0; i < 40; i++)
                    {
                        string line = snacksTxt2.ReadLine();

                        snacks.dateArray[i] = line;
                        checkExpiration(line, i);


                    }


                    foreach (var pair in snackCountList)
                    {
                        int index = Array.IndexOf(snacks.items, pair.Key);
                        if (index != -1)
                        {
                            snacks.itemNameCount[index] -= pair.Value;
                        }
                    }


                    // Grab expiration dates
                    // Display Snack Names to Kiosk
                    displaySnackName();

                }

                snacksTxt2.Close();

                DisplayResults();

            }

            void checkExpiration(string dateString, int index)
            {
                DateTime expirationDate;

                // Try to parse the expiration date
                if (DateTime.TryParseExact(dateString, "MM.dd.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out expirationDate))
                {
                    // Compare the parsed expiration date with the current date
                    if (DateTime.Now.Date > expirationDate.Date)
                    {
                        string textBoxName = "expired" + (index + 1).ToString();
                        TextBox textBox = this.GetType().GetField(textBoxName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) as TextBox;
                        if (textBox != null)
                        {
                            textBox.Text = "EXPIRED";
                        }
                    }
                    else
                    {
                        string textBoxName = "expired" + (index + 1).ToString();
                        TextBox textBox = this.GetType().GetField(textBoxName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) as TextBox;
                        if (textBox != null)
                        {
                            textBox.Text = "NOT EXPIRED";
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Invalid date format.");
                }
            }

            void displaySnackName()
            {

                for (int i = 0; i < 40; i++)
                {
                    string textBoxName = "snack" + (i + 1).ToString();
                    TextBox textBox = this.GetType().GetField(textBoxName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) as TextBox;
                    if (textBox != null)
                    {
                        textBox.Text = snacks.items[i].ToString();
                    }
                }

                for (int i = 0; i < 40; i++)
                {
                    string textBoxName = "price" + (i + 1).ToString();
                    TextBox textBox = this.GetType().GetField(textBoxName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) as TextBox;
                    if (textBox != null)
                    {
                        //MessageBox.Show(snacks.items[i].ToString());
                        textBox.Text = snacks.itemPrice[i].ToString();
                    }
                }


                for (int i = 0; i < snacks.items.Length; i++)
                {
                    string textBoxName = "quantity" + (i + 1).ToString(); // Assuming your TextBox names are like this
                    TextBox textBox = this.Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;
                    if (textBox != null)
                    {
                        textBox.Text = snacks.itemNameCount[i].ToString();
                    }
                }


            }

            void DisplayResults()
            {
                // Display All the Snacks That were Sold:
                HashSet<string> uniqueSnackNames = new HashSet<string>();
                foreach (var kvp in snackNames)
                {
                    uniqueSnackNames.Add(kvp);
                }

                snacksSold.Items.Clear();

                foreach (string snackName in uniqueSnackNames)
                {
                    snacksSold.Items.Add(snackName);
                }

                // Display greatest to least sold snacks
                var sortedSnackCountList = snackCountList.OrderByDescending(kvp => kvp.Value);

                // Now you can iterate through the sorted list and use it as needed
                foreach (var item in sortedSnackCountList)
                {
                    string snackName = item.Key;
                    int count = item.Value;

                    mostSoldinOrder.Items.Add($"{item.Key}: {item.Value}");
                }


                // Calculate total gross income
                double totalGrossIncome = 0;
                foreach (var kvp in snackCountList)
                {
                    string snackName = kvp.Key;
                    int count = kvp.Value;

                    if (snackPriceAssociation.TryGetValue(snackName, out double unitPrice))
                    {
                        totalGrossIncome += unitPrice * count;
                    }
                }

                totalIncome.Items.Add("Total Gross Income: $" + totalGrossIncome);

            }

            void RemoveEntries(int lastIndex)
            {
                for (int i = snackNames.Count - 1; i >= lastIndex; i--)
                {
                    string snackName = snackNames[i];
                    snackNames.RemoveAt(i);
                    prices.RemoveAt(i);

                    // Update snack count and reset quantity
                    if (snackCountList.ContainsKey(snackName))
                    {
                        int index = Array.IndexOf(snacks.items, snackName);
                        if (index != -1)
                        {
                            snacks.itemNameCount[index] += snackCountList[snackName]; // Reset the quantity
                        }

                        snackCountList[snackName]--;
                        if (snackCountList[snackName] == 0)
                        {
                            snackCountList.Remove(snackName);
                        }
                    }
                }
            }
        }

        void GenerateReport()
        {
            StringBuilder reportContent = new StringBuilder();
            Snacks snack = new Snacks();

            reportContent.AppendLine(locationText.Text + ",");

            for (int i = 0; i < 40; i++)
            {
                // Read snack name
                string snackName = ReadTextBoxValue("snack" + (i + 1));
                reportContent.Append(snackName + ",");
            }
            for (int i = 0; i < 40; i++)
            {
                // Read expiration status
                string expirationStatus = ReadTextBoxValue("expired" + (i + 1));
                string statusValue = expirationStatus == "EXPIRED" ? "Expired, 1" : expirationStatus == "RECALLED" ? "Recalled, 0" : "Good, 0";
                reportContent.Append(statusValue + ",");
            }

            for (int i = 0; i < 40; i++)
            {
                // Read quantity
                string quantity = ReadTextBoxValue("quantity" + (i + 1));
                quantity = checkRestockAmount(quantity);
                reportContent.Append(quantity + ",");
            }

            for (int i = 0; i < 40; i++)
            {
                // Read price
                string price = ReadTextBoxValue("price" + (i + 1));
                reportContent.Append(price + ",");
            }

            for (int i = 0; i < 40; i++)
            {
                reportContent.Append("12.4.2023");
                if (i < 39) reportContent.Append(",");
            }

            DateTime nowTime = new DateTime();
            // Write to a file
            string filePath = @"C:\Users\stani\Documents\VendingMachine\" + locationText.Text + nowTime.Month.ToString() +
                nowTime.Day.ToString() + ".txt";
            File.WriteAllText(filePath, reportContent.ToString());


        }

        public string checkRestockAmount(string quantity)
        {
            switch(quantity)
            {
                case "15":
                    quantity = "0"; break;
                case "14":
                    quantity = "1"; break;
                case "13":
                    quantity = "2"; break;
                case "12":
                    quantity = "3"; break;
                case "11":
                    quantity = "4"; break;
                case "10":
                    quantity = "5"; break;
                case "9":
                    quantity = "6"; break;
                case "8":
                    quantity = "7"; break;
                case "7":
                    quantity = "8"; break;
                case "6":
                    quantity = "9"; break;
                case "5":
                    quantity = "10"; break;
                case "4":
                    quantity = "11"; break;
                case "3":
                    quantity = "12"; break;
                case "2":
                    quantity = "13"; break;
                case "1":
                    quantity = "14"; break;
                case "0":
                    quantity = "15"; break;

            }


            return quantity;
        }
        private string ReadTextBoxValue(string textBoxName)
        {
            TextBox textBox = this.Controls.Find(textBoxName, true).FirstOrDefault() as TextBox;
            return textBox != null ? textBox.Text : "";
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBox25_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox73_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            GenerateReport();
            DateTime nowTime = new DateTime();
            // Define the path where the file will be saved
            // This example uses the user's desktop - you can change it as needed
            string desktopPath = @"C:\Users\stani\Documents\VendingMachine\RestockerNotes" + nowTime.Month.ToString() + "." +
                nowTime.Day.ToString() + ".txt";
            //string filePath = Path.Combine(desktopPath, "output.txt");

            // Get the text from the TextBox
            string textToWrite = notesRestocker.Text;

            // Write the text to a file
            File.WriteAllText(desktopPath, textToWrite);

        }

        private void notesRestocker_TextChanged(object sender, EventArgs e)
        {

        }
    }
}