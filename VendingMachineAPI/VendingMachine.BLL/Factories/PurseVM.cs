using System.Collections.Generic;
using VendingMachine.Core.Models;

namespace VendingMachine.BLL.Factories
{
    // Vending Machine wallet
    public class PurseVM : PurseBase
    {
        public PurseVM(List<Coin> coins) : base(coins)
        {
        }

        // pay the amount with suitable coins
        public override IEnumerable<Coin> Pay(int summ)
        {
            IEnumerable<Coin> resultList = null;
            if (ValidateAmountCoins(summ))
            {
                resultList = GetCoins(summ);
                // take the coins
                RemoveCoins(resultList);
            }
            return resultList;
        }

        // pay with a certain type of coin
        public override IEnumerable<Coin> Pay(IEnumerable<Coin> coins)
        {
            IEnumerable<Coin> resultList = null;
            if (ValidateAmountCoins(coins))
            {
                resultList = GetCoins(coins);
                // take the coins
                RemoveCoins(resultList);
            }
            return resultList;
        }

        // top up your wallet
        public override void Replenish(IEnumerable<Coin> coins)
        {
            AddCoins(coins);
        }
    }
}