﻿using VendingMachine.BLL.Factories.Products;
using VendingMachine.Core.Models;

namespace VendingMachine.BLL.Factories.Creators
{
    public abstract class CreatorBase
    {
        /// <summary>
        /// Available portions
        /// </summary>
        public int Availability { get; set; }
        public string Name { get; }
        public TypeProduct TypeProduct { get; }

        protected CreatorBase(int availability, string name, TypeProduct typeProduct)
        {
            Availability = availability;
            Name = name;
            TypeProduct = typeProduct;
        }

        public abstract ProductBase Create();

        public abstract bool ValidateProduct();
    }
}