namespace ManagementSystem
{
    public partial class ManagementSystem : Form
    {
        public ManagementSystem()
        {
            InitializeComponent();

            string text = "Doritos: $2.5\nLays: $3.5\nLays: $3.5\nDoritos: $2.5\nCredit Card Payment: $12\nDoritos: $2.5\nLays: $3.5\nOrder Canceled";
            List<string> snackNames = new List<string>();
            List<double> prices = new List<double>();
            List<double> creditCardPayments = new List<double>();
            Dictionary<string, int> snackCountList = new Dictionary<string, int>();
            Dictionary<string, double> snackPriceAssociation = new Dictionary<string, double>();
            double totalCreditCardPayment = 0;

            var lines = text.Split('\n');
            int lastPaymentIndex = -1;

            foreach (var line in lines)
            {
                if (line.StartsWith("Credit Card Payment:"))
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
                        RemoveEntries(lastPaymentIndex, snackNames, prices, snackCountList);
                    }
                }
                else
                {
                    var parts = line.Split(':');
                    var snackName = parts[0];
                    var price = double.Parse(parts[1].Trim().TrimStart('$'));

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
            }

            // Print results
            MessageBox.Show("Snacks: " + string.Join(", ", snackNames));
            MessageBox.Show("Prices: " + string.Join(", ", prices));
            MessageBox.Show("Credit Card Payments: " + string.Join(", ", creditCardPayments));
            MessageBox.Show("Total Credit Card Payment: " + totalCreditCardPayment);
            MessageBox.Show("Snack Counts: " + string.Join(", ", snackCountList.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
            MessageBox.Show("Snack Price Associations: " + string.Join(", ", snackPriceAssociation.Select(kvp => $"{kvp.Key}: ${kvp.Value}")));
        }

        static void RemoveEntries(int lastIndex, List<string> snackNames, List<double> prices, Dictionary<string, int> snackCountList)
        {
            for (int i = snackNames.Count - 1; i >= lastIndex; i--)
            {
                string snackName = snackNames[i];
                snackNames.RemoveAt(i);
                prices.RemoveAt(i);

                // Update snack count
                if (snackCountList.ContainsKey(snackName))
                {
                    snackCountList[snackName]--;
                    if (snackCountList[snackName] == 0)
                    {
                        snackCountList.Remove(snackName);
                    }
                }
            }
        }
    }
}