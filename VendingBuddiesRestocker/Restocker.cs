using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Threading;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Windows.Forms;

namespace VendingBuddiesRestocker
{
    public partial class Restocker : Form
    {

        public Restocker()
        {
            InitializeComponent();

            loadCorporateFile();
        }

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

                    //MessageBox.Show(Convert.ToString(words.Count()));
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
        }

        // 0 - 39 Snack Names / 40 - 119 (Expired/Recalled/Good):Quantity / 120 - 159 Quantity to Restock
        public void processSnackDetails(string[] snackDetail)
        {
            /*
            MessageBox.Show(Convert.ToString(snackDetail[0]));
            MessageBox.Show(Convert.ToString(snackDetail[39]));
            MessageBox.Show(Convert.ToString(snackDetail[40]));
            MessageBox.Show(Convert.ToString(snackDetail[119]));
            MessageBox.Show(Convert.ToString(snackDetail[120]));
            MessageBox.Show(Convert.ToString(snackDetail[159]));
            MessageBox.Show(Convert.ToString(snackDetail[160]));
            MessageBox.Show(Convert.ToString(snackDetail[199]));
            */
            int j = 0;
            // Add all the Snack names to the list boxes
            for (int i = 0; i < 40; i++, j++)
            {
                string listBoxName = "snack" + (j + 1).ToString();
                ListBox listBox = this.GetType().GetField(listBoxName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) as ListBox;
                listBox.Items.Add(snackDetail[i]);
            }

            // Reset j
            j = 0;
            // Add all the number of items needed to remove if expired
            for (int i = 40; i < 120; i++, j++)
            {
                string listBoxName = "snack" + (j + 1).ToString();
                ListBox listBox = this.GetType().GetField(listBoxName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) as ListBox;
                if (listBox != null && i < snackDetail.Length)
                {
                    if (Convert.ToString(snackDetail[i]) == "Expired")
                    {
                        listBox.Items.Add("Expired Items to Remove: " + Convert.ToString(snackDetail[++i]));

                    }
                    else
                    {
                        listBox.Items.Add("Expired Items to Remove: " + Convert.ToString(snackDetail[++i]));
                    }
                }
            }

            // Reset j
            j = 0;
            // Add the quantity to restock
            for (int i = 120; i < 160; i++, j++)
            {
                string listBoxName = "snack" + (j + 1).ToString();
                ListBox listBox = this.GetType().GetField(listBoxName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)?.GetValue(this) as ListBox;
                listBox.Items.Add("Restock Amount: " + snackDetail[i]);
            }


            vendingMachineFileCreate(snackDetail);


        }

        //Create the textfile to load for the vending machine
        public void vendingMachineFileCreate(string[] snackDetail)
        {
            string filePath = @"C:\Users\stani\Desktop\VendingBuddies\VendingMachine.txt"; // Specify the file path
            string[] lines = { "First line", "Second line", "Third line" }; // Example string array
            try
            {
                // Check if the file exists
                if (File.Exists(filePath))
                {
                    // Delete the file
                    File.Delete(filePath);
                }
                else
                {
                    // Append text to the existing file or create a new file
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        // Add Each Snack Name
                        for (int i = 0; i < 40; i++)
                        {
                            sw.WriteLine(snackDetail[i]);
                        }

                        // Default price for right now
                        for (int i = 0; i < 40; i++)
                        {
                            sw.WriteLine(1.5);
                        }

                        // Default full quantity
                        for (int i = 0; i < 40; i++)
                        {
                            sw.WriteLine(15);
                        }

                        // Append Expiration Dates
                        for (int i = 160; i < 200; i++)
                        {
                            sw.WriteLine(snackDetail[i]);
                        }
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
    }
}


