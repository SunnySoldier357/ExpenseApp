using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseApp.Models.DB
{
    public class ExpenseEntry
    {
        private string _cost;

        private decimal _hotel;
        private decimal _transport;
        private decimal _fuel;
        private decimal _meals;
        private decimal _phone;
        private decimal _entertainment;
        private decimal _misc;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }
        public Account Account { get; set; }
        public string Description { get; set; }
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
        public decimal Hotel
        {
            get => _hotel;
            set => AddCostGroup('A', value);
        }

        [NotMapped]
        public decimal Transport
        {
            get => _transport;
            set => AddCostGroup('B', value);
        }

        [NotMapped]
        public decimal Fuel
        {
            get => _fuel;
            set => AddCostGroup('C', value);
        }

        [NotMapped]
        public decimal Meals
        {
            get => _meals;
            set => AddCostGroup('D', value);
        }

        [NotMapped]
        public decimal Phone
        {
            get => _phone;
            set => AddCostGroup('E', value);
        }

        [NotMapped]
        public decimal Entertainment
        {
            get => _entertainment;
            set => AddCostGroup('F', value);
        }

        [NotMapped]
        public decimal Misc
        {
            get => _misc;
            set => AddCostGroup('G', value);
        }

        [NotMapped]
        public decimal Total 
        {
            get => Hotel + Transport + Fuel + Meals + Phone
                + Entertainment + Misc;
        }

        public string ReceiptId { get; set; } = null;
        
        private void AddCostGroup(char group, decimal cost)
        {
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