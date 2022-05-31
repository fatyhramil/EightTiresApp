using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EightTiresApp.Models
{
    public partial class Product
    {
        public string FullImagePath
        {
            get
            {
                if (Image == null || Image == "" || Image == "нет" || Image == "не указано")
                {
                    return "/Images/picture.png";
                }
                else
                {
                        return "/Images" + Image;
                    
                }
            }
        }
        public SolidColorBrush Color
        {
            get
            {
                bool isSaled =false;
                foreach(ProductSale productSale in ProductSale)
                {
                    if (productSale.SaleDate < DateTime.Now.AddDays(-30))
                    {
                        isSaled = true;
                    }
                }
                if (isSaled)
                {
                    return Brushes.Red;
                }
                else
                {
                    return Brushes.White;
                }
            }
        }
        public decimal FullPrice
        {
            get
            {
                if (ProductMaterial.Count > 0)
                {
                    decimal price = 0;
                    foreach (ProductMaterial productMaterial in ProductMaterial)
                    {
                        if (productMaterial.IsDeleted == null)
                        {
                            price += (decimal)productMaterial.Count * productMaterial.Material.Cost;
                        }
                    }
                    return price;
                }
                else
                {
                    return MinCostForAgent;
                }
            }
        }
    }
}
