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
        }
    }
}