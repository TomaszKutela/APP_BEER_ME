﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace APP_BEER_ME.Models
{
    public class Stock
    {
        public int StockID { get; set; }
        public int BeerID { get; set; }
        public int ShopID { get; set; }
        [Display(Name = "Price (€)")]
        public double Price { get; set; }        

        public virtual Beer Beer { get; set; }
        public virtual Shop Shop { get; set; }

    }
}