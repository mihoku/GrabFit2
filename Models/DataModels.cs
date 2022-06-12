using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GrabFit2.Models
{
    public class FoodReferences
    {
        [Key]
        public int ID { get; set; }
        public string FoodName { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public float Calorie { get; set; }
        public float Fat { get; set; }
        public float Carbohydrate { get; set; }
        public float Protein { get; set; }
        public virtual ICollection<OrderTagged> OrderTagged { get; set; }
    }

    public class FoodOrdered
    {
        [Key]
        public int ID { get; set; }
        public DateTime orderDate { get; set; }
        public int merchantID { get; set; }
        public string merchantName { get; set; }
        public int itemID { get; set; }
        public string itemName { get; set; }
        public string itemDescription { get; set; }
        public string imageURL { get; set; }
        public float itemOrdered { get; set; }
        public bool tagged { get; set; }
        public virtual ICollection<OrderTagged> OrderTagged { get; set; }
    }

    public class OrderTagged
    {
        [Key]
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int FoodID { get; set; }
        [ForeignKey("OrderID")]
        public virtual FoodOrdered FoodOrdered { get; set; }
        [ForeignKey("FoodID")]
        public virtual FoodReferences FoodReferences { get; set; }
    }

    public class Merchant
    {
        public int ID { get; set; }
        public string merchantName { get; set; }
        public string imageURL { get; set; }
    }

    public class Menu
    {
        public int ID { get; set; }
        public string merchantName { get; set; }
        public string itemName { get; set; }
        public string imageURL { get; set; }

    }
}