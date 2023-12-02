using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows.Forms;
using System.Security.Cryptography;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Forms.VisualStyles;

namespace VendingBuddiesRestocker
{
    public partial class Restocker : Form
    {

        public Restocker()
        {
            InitializeComponent();

            loadCorporateFile();
        }
        public string locationVendingMachine;
        public string[] pricesList = new string[40];

        public void loadCorporateFile()
        {
            // Prompt User to Choose File
            OpenFileDialog openSnackText = new OpenFileDialog();
            if (openSnackText.ShowDialog() == DialogResult.OK)
            {
                string snacksTxt = openSnackText.FileName;

                try
                {
                    string[] lines = File.ReadAllLines(snacksTxt);
                    List<string> words = new List<string>();

                    foreach (var line in lines)
                    {
                        words.AddRange(line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                    }

                    // Create an array to store processed words
                    string[] snackDetails = new string[words.Count];

                    // Process each word
                    for (int i = 0; i < words.Count; i++)
                    {
                        string processedWord = words[i].Trim();
                        snackDetails[i] = processedWord;

                    }

                    processSnackDetails(snackDetails);
                    // At this point, processedWords array contains all the processed words
                }
                catch (IOException ex)
                {
                    Console.WriteLine("An IO exception has been thrown!");
                    Console.WriteLine(ex.ToString());
                }

            }

            // Prompt User to Choose File
            OpenFileDialog openSnackManageText = new OpenFileDialog();
            if (openSnackManageText.ShowDialog() == DialogResult.OK)
            {
                string snacksTxtManage = openSnackManageText.FileName;
                string[] lines = File.ReadAllLines(snacksTxtManage);

                foreach (string line in lines)
                {
                    managementNotes.Items.Add(line);
                }
            }
        }

        // 0 - 39 Snack Names / 40 - 119 (Expired/Recalled/Good):Quantity / 120 - 159 Quantity to Restock
        public void processSnackDetails(string[] snackDetail)
        {

            int j = 0;

            // Add all the Snack names to the list boxes
            for (int i = 1; i < 41; i++, j++)
            {
                string listBoxName = "snack" + (j + 1).ToString();
                ListBox listBox = this.GetType().GetField(listBoxName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) as ListBox;
                listBox.Items.Add(snackDetail[i]);
            }

            // Reset j
            j = 0;
            // Add all the number of items needed to remove if expired
            for (int i = 41; i < 121; i++, j++)
            {
                string listBoxName = "snack" + (j + 1).ToString();
                ListBox listBox = this.GetType().GetField(listBoxName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) as ListBox;
                if (listBox != null && i < snackDetail.Length)
                {
                    if (Convert.ToString(snackDetail[i]) == "Expired")
                    {
                        listBox.Items.Add("Expired Please Remove");

                    }
                    else if (Convert.ToString(snackDetail[i]) == "Recalled")
                    {
                        listBox.Items.Add("Recalled Please Remove");
                    }
                    else
                    {
                        listBox.Items.Add("Good");
                    }
                }
            }

            // Reset j
            j = 0;
            // Add the quantity to restock
            for (int i = 121; i < 161; i++, j++)
            {
                string listBoxName = "snack" + (j + 1).ToString();
                ListBox listBox = this.GetType().GetField(listBoxName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) as ListBox;
                listBox.Items.Add("Restock Amount: " + snackDetail[i]);
            }

            int k = 0;
            for (int i = 161; i < 201; i++, k++)
            {
                pricesList[k] = snackDetail[i];
            }


            vendingMachineFileCreate(snackDetail);


        }

        //Create the textfile to load for the vending machine
        public void vendingMachineFileCreate(string[] snackDetail)
        {
            string filePath = @"C:\Users\stani\Documents\VendingMachine\VendingMachine.txt"; // Specify the file path
            string[] lines = { "First line", "Second line", "Third line" }; // Example string array
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                // Append text to the existing file or create a new file
                using (StreamWriter sw = File.AppendText(filePath))
                {
                    sw.WriteLine(snackDetail[0]);

                    // Add Each Snack Name
                    for (int i = 1; i < 41; i++)
                    {
                        sw.WriteLine(snackDetail[i]);
                    }

                    // Default price for right now
                    for (int i = 0; i < 40; i++)
                    {
                        sw.WriteLine(pricesList[i]);
                    }

                    // Default full quantity
                    for (int i = 0; i < 40; i++)
                    {
                        sw.WriteLine(15);
                    }

                    // Append Expiration Dates
                    for (int i = 0; i < 40; i++)
                    {
                        sw.WriteLine("12.04.2023");
                    }
                }

            }
            catch (Exception ex)
            {
                // Handle any errors that might occur
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void managementNotes_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}


