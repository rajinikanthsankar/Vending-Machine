﻿using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Core.Models
{
    public enum TypeCoin : int
    {
        Price1Rub = 1,
        Price2Rub = 2,
        Price5Rub = 5,
        Price10Rub = 10,
    }

    public enum TypeProduct : int
    {
        Coffee = 1,
        CoffeeWithMilk = 2,
        Tea = 3,
        Juice = 4
    }
}
