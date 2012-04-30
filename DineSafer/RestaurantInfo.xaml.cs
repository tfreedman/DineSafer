using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
                PageTitle.Text = name.ToUpper();
            }

            List<DineSafe> filteredData = new List<DineSafe> { };
            for (int i = 0; i < MainPage.original.GetLength(0); i++) {
                if (Convert.ToString(MainPage.original[i].Name).ToLower().Contains(Convert.ToString(name).ToLower()) && Convert.ToString(MainPage.original[i].Address).ToLower().Contains(Convert.ToString(address).ToLower()))
                    filteredData.Add(MainPage.original[i]);
            }

            List<StackPanel> events = new List<StackPanel> { };
            Dictionary<string, short> seenDates = new Dictionary<string, short>();
            foreach (DineSafe dineSafe in filteredData) {
                string key = dineSafe.Date;
                if (!seenDates.ContainsKey(key)) {
                    seenDates.Add(key, 0);
                }
            }
            
            var sortedSeenDates = seenDates.Keys.ToList();
            sortedSeenDates.Sort();
            sortedSeenDates.Reverse();

            seenDates.Clear();
            bool closedFlag = false;
            bool conditionalFlag = false;
            foreach (string pair in sortedSeenDates) {
                events.Add(new StackPanel());
                events.ElementAt(events.Count - 1).Children.Add(new TextBlock { Text = pair });
                for (int i = 0; i < filteredData.Count; i++) {
                    if (filteredData[i].Date.Equals(pair) && !filteredData[i].Details.Equals("")) {
                        if (filteredData[i].Status.Equals("Closed")) {
                            closedFlag = true;
                        }
                        else if (filteredData[i].Status.Equals("Conditional Pass")) {
                            conditionalFlag = true;
                        }
                        events.ElementAt(events.Count - 1).Children.Add(new TextBlock { Text = filteredData[i].Details});
                        int eventAt = events.ElementAt(events.Count - 1).Children.Count;
                        ((TextBlock)events.ElementAt(events.Count - 1).Children.ElementAt(eventAt - 1)).TextWrapping = TextWrapping.Wrap;
                        ((TextBlock)events.ElementAt(events.Count - 1).Children.ElementAt(eventAt - 1)).Margin = new Thickness(12,0,0,0);
                        ((TextBlock)events.ElementAt(events.Count - 1).Children.ElementAt(eventAt - 1)).Text = filteredData[i].Severity[0] + " - " + ((TextBlock)events.ElementAt(events.Count - 1).Children.ElementAt(eventAt - 1)).Text;
                        events.ElementAt(events.Count - 1).Children.Add(new TextBlock { Margin = new Thickness(0,5,0,0) });
                    }
                }
                ((TextBlock)(events.ElementAt(events.Count - 1).Children.ElementAt(0))).Text = "\u2022" + ((TextBlock)(events.ElementAt(events.Count - 1).Children.ElementAt(0))).Text;
                if (events.ElementAt(events.Count - 1).Children.Count == 1) {
                    ((TextBlock)(events.ElementAt(events.Count - 1).Children.ElementAt(0))).Foreground = new SolidColorBrush(Colors.Green);
                } else if (conditionalFlag) {
                    for (int i = 0; i < events.ElementAt(events.Count - 1).Children.Count; i++) {
                        ((TextBlock)(events.ElementAt(events.Count - 1).Children.ElementAt(i))).Foreground = new SolidColorBrush(Colors.Yellow);
                    }
                } else if (closedFlag) {
                    for (int i = 0; i < events.ElementAt(events.Count - 1).Children.Count; i++) {
                        ((TextBlock)(events.ElementAt(events.Count - 1).Children.ElementAt(i))).Foreground = new SolidColorBrush(Colors.Red);
                    }
                } 
                conditionalFlag = false;
                closedFlag = false;
            }

            foreach (StackPanel detail in events) {
                detail.Margin = new Thickness(10);
            }

            listBox.ItemsSource = events;
        }
    }
}