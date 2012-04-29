using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.Phone.Controls;
using System.Xml.Linq;
using System.Windows.Resources;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Controls;


namespace DineSafer {
    public partial class MainPage : PhoneApplicationPage {
        XDocument loadedData;
        StreamResourceInfo xml;

        public MainPage() {
            InitializeComponent();
            Debug.WriteLine("(1)" + DateTime.Now);
            xml = Application.GetResourceStream(new Uri("dinesafe.xml", UriKind.Relative));
            loadedData = XDocument.Load(xml.Stream);

            var data = from query in loadedData.Descendants("ROW")
                       select new DineSafe {
                           Name = (string)query.Attribute("N"),
                           FoodType = (string)query.Attribute("T"),
                           Address = (string)query.Attribute("A"),
                           Status = (string)query.Attribute("S"),
                           Details = (string)query.Attribute("D"),
                           Date = (string)query.Attribute("I"),
                           Severity = (string)query.Attribute("X")
                       };

            original = data.ToArray<DineSafe>();
            Dictionary<string, short> seenEntries = new Dictionary<string, short>(); // the short value is a dummy value
            foreach (DineSafe dineSafe in data) {
                string key = dineSafe.GetKey();
                if (!seenEntries.ContainsKey(key)) {
                    uniques.Add(dineSafe);
                    seenEntries.Add(key, 0);
                }
            }
            seenEntries.Clear();
            array = uniques.ToArray();
            Debug.WriteLine("[DONE!]" + DateTime.Now);
        }

        public static DineSafe[] original;
        public static DineSafe[] array;

        private List<DineSafe> uniques = new List<DineSafe>();
        List<DineSafe> filteredData;
        private void performSearch(object sender, EventArgs e) {
            filteredData = new List<DineSafe> { };
            for (int i = 0; i < array.GetLength(0); i++) {
                if (Convert.ToString(array[i].Name).ToLower().Contains(Convert.ToString(toSearch).ToLower()))
                    filteredData.Add(array[i]);
            }
            listBox.ItemsSource = filteredData;
        }

        private void search() {
            object sender = null;
            EventArgs e = new EventArgs();
            performSearch(sender, e);
        }
        string toSearch = "";
        private void searchBox_TextChanged(object sender, RoutedEventArgs e) {
            toSearch = searchBox.Text;
            search();
        }

        private void StackPanel_Tap(object sender, GestureEventArgs e) {
            StackPanel se = (StackPanel)sender;
            TextBlock name = (TextBlock)se.Children.ElementAt(0);
            TextBlock address = (TextBlock)se.Children.ElementAt(1);
            string escapedName = Uri.EscapeDataString(name.Text);
            string escapedAddress = Uri.EscapeDataString(address.Text);
            NavigationService.Navigate(new Uri("/RestaurantInfo.xaml?name=" + escapedName + "&address=" + escapedAddress, UriKind.Relative));
        }
    }
}