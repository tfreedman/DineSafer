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
            Debug.WriteLine(DateTime.Now);
            xml = Application.GetResourceStream(new Uri("dinesafe.xml", UriKind.Relative));
            loadedData = XDocument.Load(xml.Stream);

            var data = from query in loadedData.Descendants("ROW")
                       select new DineSafe {
                           Name = (string)query.Attribute("ESTABLISHMENT_NAME"),
                           FoodType = (string)query.Attribute("ESTABLISHMENTTYPE"),
                           Address = (string)query.Attribute("ESTABLISHMENT_ADDRESS"),
                           Status = (string)query.Attribute("ESTABLISHMENT_STATUS"),
                           Details = (string)query.Attribute("INFRACTION_DETAILS"),
                           Date = (string)query.Attribute("INFRACTION_DATE"),
                           Severity = (string)query.Attribute("SEVERITY")
                       };
            uniques = data.Distinct(new DineSafeComparer());
            original = data.ToArray<DineSafe>();
            array = uniques.ToArray<DineSafe>();
            Debug.WriteLine(DateTime.Now);

        }
        public DineSafe[] original;
        DineSafe[] array;
        IEnumerable<DineSafe> uniques;
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

        private void TextBlock_Tap(object sender, GestureEventArgs e) {
            TextBlock se = (TextBlock)sender;
            NavigationService.Navigate(new Uri("/RestaurantInfo.xaml?name=" + se.Text, UriKind.Relative));
        }
    }
}