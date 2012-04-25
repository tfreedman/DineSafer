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
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Windows.Resources;

namespace DineSafer
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            XDocument loadedData;

            StreamResourceInfo xml = Application.GetResourceStream(new Uri("dinesafe.xml", UriKind.Relative));
            loadedData = XDocument.Load(xml.Stream);

            var data = from query in loadedData.Descendants("ROW")
                       select new DineSafe
                       {
                           Name = (string)query.Element("ESTABLISHMENT_NAME"),
                           FoodType = (string)query.Element("ESTABLISHMENTTYPE"),
                           Address = (string)query.Element("ESTABLISHMENT_ADDRESS"),
                           Status = (string)query.Element("ESTABLISHMENT_STATUS"),
                           Details = (string)query.Element("INFRACTION_DETAILS"),
                           Date = (string)query.Element("INFRACTION_DATE"),
                           Severity = (string)query.Element("SEVERITY")
                       };
            listBox.ItemsSource = data;
        }
    }
}