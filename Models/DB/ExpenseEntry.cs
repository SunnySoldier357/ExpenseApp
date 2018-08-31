using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace ExpenseApp.Models.DB
{
    public class ExpenseEntry : IValidatableObject
    {
        // Private Properties
        private decimal _hotel;
        private decimal _transport;
        private decimal _fuel;
        private decimal _meals;
        private decimal _phone;
        private decimal _entertainment;
        private decimal _misc;

        private string _cost;

        // Public Properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Account Account { get; set; }

        public DateTime Date { get; set; }

        [InverseProperty("Entries")]
        public ExpenseForm Form { get; set; }

        public Receipt Receipt { get; set; }

        public string Description { get; set; }

        // Syntax: 'A100.00;'
        //     - The letter represents the type of expense
        //         - A = Hotel
        //         - B = Transport
        //         - C = Fuel
        //         - D = Meals
        //         - E = Phone
        //         - F = Entertainment
        //         - G = Miscellaneous
        //    - The string can include any number of these cost groups but only
        //      one for each type can exist. The order does not matter
        //    - The number included is automatically formatted to 2 decimal places
        public string Cost
        {
            get => _cost;
            set
            {
                _cost = value.ToUpper();

                string[] costs = value.ToUpper().Split(';');
                for (int i = 0; i < costs.Length - 1; i++)
                {
                    switch (costs[i].Substring(0, 1))
                    {
                        case "A":
                            _hotel = decimal.Parse(costs[i].Substring(1));
                            break;

                        case "B":
                            _transport = decimal.Parse(costs[i].Substring(1));
                            break;

                        case "C":
                            _fuel = decimal.Parse(costs[i].Substring(1));
                            break;

                        case "D":
                            _meals = decimal.Parse(costs[i].Substring(1));
                            break;

                        case "E":
                            _phone = decimal.Parse(costs[i].Substring(1));
                            break;

                        case "F":
                            _entertainment = decimal.Parse(costs[i].Substring(1));
                            break;

                        default:
                            _misc = decimal.Parse(costs[i].Substring(1));
                            break;
                    }
                }
            }
        }

        [NotMapped]
        public decimal? Hotel
        {
            get => _hotel;
            set => addCostGroup('A', value);
        }

        [NotMapped]
        public decimal? Transport
        {
            get => _transport;
            set => addCostGroup('B', value);
        }

        [NotMapped]
        public decimal? Fuel
        {
            get => _fuel;
            set => addCostGroup('C', value);
        }

        [NotMapped]
        public decimal? Meals
        {
            get => _meals;
            set => addCostGroup('D', value);
        }

        [NotMapped]
        public decimal? Phone
        {
            get => _phone;
            set => addCostGroup('E', value);
        }

        [NotMapped]
        public decimal? Entertainment
        {
            get => _entertainment;
            set => addCostGroup('F', value);
        }

        [NotMapped]
        [Display(Name = "Misc.")]
        public decimal? Misc
        {
            get => _misc;
            set => addCostGroup('G', value);
        }

        [NotMapped]
        public decimal? Total
        {
            get => Hotel + Transport + Fuel + Meals + Phone
                + Entertainment + Misc;
        }

        [NotMapped]
        public IFormFile ImageFormFile { get; set; }

        // Constructor
        public ExpenseEntry() => _cost = "";

        // Interface Implementations
        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            if (_hotel == 0 && _transport == 0 && _fuel == 0 && _meals == 0 &&
                _phone == 0 && _entertainment == 0 && _misc == 0)
            {
                yield return new ValidationResult(
                    "At least one of the types of expenses (Hotel, Transport, Fuel," +
                        " Meals, Phone, Entertainment, Misc.) has to be filled in."
                );
            }
        }

        // Private Properties
        private void addCostGroup(char group, decimal? cost)
        {
            if (null == cost || cost == 0)
                return;

            if (_cost.Contains(group))
            {
                int index = _cost.IndexOf(group);
                int endIndex = _cost.IndexOf(';', index);
                
                Cost = Cost.Replace(
                    _cost.Substring(index, endIndex - index + 1),
                    string.Format("{0}{1:0.00};", group, cost));
            }
            else
                Cost += string.Format("{0}{1:0.00};", group, cost);
        }
    }
}