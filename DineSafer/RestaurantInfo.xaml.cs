using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace DineSafer {
    public partial class RestaurantInfo : PhoneApplicationPage {
        public RestaurantInfo() {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e) {
            base.OnNavigatedTo(e);
            string name = "";
            string address = "";
            if (NavigationContext.QueryString.TryGetValue("name", out name) && NavigationContext.QueryString.TryGetValue("address", out address)) {
                RestAddress.Text = address;
                PageTitle.Text = name;
            }

            List<DineSafe> filteredData = new List<DineSafe> { };
            for (int i = 0; i < MainPage.original.GetLength(0); i++) {
                if (Convert.ToString(MainPage.original[i].Name).ToLower().Contains(Convert.ToString(name).ToLower()) && Convert.ToString(MainPage.original[i].Address).ToLower().Contains(Convert.ToString(address).ToLower()))
                    filteredData.Add(MainPage.original[i]);
            }



            List<StackPanel> events = new List<StackPanel> { };
            Dictionary<string, short> seenDates = new Dictionary<string, short>(); // the short value is a dummy value
            foreach (DineSafe dineSafe in filteredData) {
                string key = dineSafe.Date;
                if (!seenDates.ContainsKey(key)) {
                    seenDates.Add(key, 0);
                }
            }
            var sortedSeenDates = seenDates.Keys.ToList();
            sortedSeenDates.Sort();
            sortedSeenDates.Reverse();


            foreach (string pair in sortedSeenDates) {
                events.Add(new StackPanel());
                events.ElementAt(events.Count - 1).Children.Add(new TextBlock { Text = pair });
                for (int i = 0; i < filteredData.Count; i++) {
                    if (filteredData[i].Date.Equals(pair) && !filteredData[i].Details.Equals("")) {
                        events.ElementAt(events.Count - 1).Children.Add(new TextBlock { Text = filteredData[i].Details });
                        int eventAt = events.ElementAt(events.Count - 1).Children.Count;
                        ((TextBlock)events.ElementAt(events.Count - 1).Children.ElementAt(eventAt - 1)).Text = filteredData[i].Severity[0] + " - " + ((TextBlock)events.ElementAt(events.Count - 1).Children.ElementAt(eventAt - 1)).Text;
                    }
                }
                if (events.ElementAt(events.Count - 1).Children.Count == 1) {
                    ((TextBlock)(events.ElementAt(events.Count - 1).Children.ElementAt(0))).Text = "\u2022" + ((TextBlock)(events.ElementAt(events.Count - 1).Children.ElementAt(0))).Text;
                    ((TextBlock)(events.ElementAt(events.Count - 1).Children.ElementAt(0))).Foreground = new SolidColorBrush(Colors.Green);
                }
            }

            foreach (StackPanel detail in events) {
                detail.Margin = new Thickness(10);
            }

            listBox.ItemsSource = events;
        }
    }
}