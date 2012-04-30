namespace DineSafer {
    public class DineSafe {
        string ESTABLISHMENT_NAME;
        string ESTABLISHMENTTYPE;
        string ESTABLISHMENT_ADDRESS;
        string ESTABLISHMENT_STATUS;
        string INFRACTION_DETAILS;
        string INFRACTION_DATE;
        string SEVERITY;

        public string Name {
            get { return ESTABLISHMENT_NAME; }
            set { ESTABLISHMENT_NAME = value; }
        }
        public string FoodType {
            get { return ESTABLISHMENTTYPE; }
            set { ESTABLISHMENTTYPE = value; }
        }
        public string Address {
            get { return ESTABLISHMENT_ADDRESS; }
            set { ESTABLISHMENT_ADDRESS = value; }
        }
        public string Status {
            get { return ESTABLISHMENT_STATUS; }
            set { ESTABLISHMENT_STATUS = value; }
        }
        public string Details {
            get { return INFRACTION_DETAILS; }
            set { INFRACTION_DETAILS = value; }
        }
        public string Date {
            get { return INFRACTION_DATE; }
            set { INFRACTION_DATE = value; }
        }
        public string Severity {
            get { return SEVERITY; }
            set { SEVERITY = value; }
        }

        public string GetKey() {
            return string.Format("Name={0},Address={1}", Name, Address);
        }
    }
}
