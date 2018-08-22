using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseApp.Models.DB
{
    public class ExpenseEntry
    {
        public string Id { get; set; }

        /*
        public string ExpenseFormId { get; set; }
        public virtual ExpenseForm ExpenseForm { get; set; }
        */
        public DateTime Date { get; set; }
        public string Account { get; set; }
        public string Description { get; set; }
        public string Cost { get; set; }
        
        [NotMapped]
        public decimal Hotel { get; set; }
        
        [NotMapped]
        public decimal Transport { get; set; }
        
        [NotMapped]
        public decimal Fuel { get; set; }
        
        [NotMapped]
        public decimal Meals { get; set; }
        
        [NotMapped]
        public decimal Phone { get; set; }
        
        [NotMapped]
        public decimal Entertainment { get; set; }
        
        [NotMapped]
        public decimal Misc { get; set; }
        
        [NotMapped]
        public decimal Total 
        {
            get => Hotel + Transport + Fuel + Meals + Phone
                + Entertainment + Misc;
        }

        public string ReceiptId { get; set; } = null;
    }

    public static class Account
    {
        public static string Airfare = "Account";
        public static string BroadbandHome = "Broadband – Home";
	    public static string BroadbandTravel = "Broadband – Travel";
        public static string CarRental = "Car Rental";
        public static string CargoParcelShipping = "Cargo, Parcel, Shipping";
        public static string Catering = "Catering";
        public static string CellPhone = "Cell Phone";
        public static string Training = "Training";
        public static string ConferenceSeminar = "Conference/Seminar";
        public static string DuesSubscriptions = "Dues/Subscriptions";
        public static string EmployeeMorale = "Employee Morale";
        public static string EmployeeMoraleMeals = "Employee Morale – Meals";
        public static string EmployeeDevelopmentTraining = "Employee Development/Training";
        public static string Entertainment = "Entertainment";
        public static string EventServices = "Event Services";
        public static string EventSponsorships = "Event Sponsorships";
        public static string EventVenuesSiteSelection = "Event Venues & Site Selection";
        public static string Events = "Events";
        public static string ExpenseMultiPurpose = "Expense – Multi Purpose";
        public static string GiftCertTangibleGifts = "Gift Cert / Tangible Gifts";
        public static string Hardware = "Hardware";
        public static string HardwareDevelopment = "Hardware Development";
        public static string Hotel = "Hotel";
        public static string InfrastructureOpsSupport = "Infrastructure & Ops Support";
        public static string MaintenanceRepairs = "Maintenance – Repairs";
        public static string MarketingServices = "Marketing Services";
        public static string MealsEntertainment = "Meals – Entertainment";
        public static string MealsMultipurpose = "Meals – Multipurpose";
        public static string MealsTravel = "Meals – Travel";
        public static string MealsForMeetings = "Meals for Meetings";
        public static string MembershipDues = "Membership & Dues";
        public static string Mileage = "Mileage";
        public static string NotForReimbursementOthers = "Not for Reimbursement – Others";
        public static string OtherTravelExpenses = "Other Travel Expenses";
        public static string Parking = "Parking";
        public static string Personal = "Personal";
        public static string PhoneFax = "Phone/Fax";
        public static string Postage = "Postage";
        public static string SuppliesComputerEquipment = "Supplies – Computer Equipment";
        public static string SuppliesGeneralOffice = "Supplies – General Office";
        public static string SuppliesReferenceMaterial = "Supplies – Reference Material";
        public static string TipsGratuity = "Tips/Gratuity";
        public static string TransportationOthers = "Transportation – Others";
        public static string TransportationRail = "Transportation – Rail";
        public static string TransportationTaxi = "Transportation – Taxi";
        public static string TravelFee = "Travel Fee";
        public static string VisasVaccinations = "Visas/Vaccinations";
    }
}