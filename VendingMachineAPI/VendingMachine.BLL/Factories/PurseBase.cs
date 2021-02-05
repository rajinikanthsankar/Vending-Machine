using System;
using System.Collections.Generic;
using System.Linq;
using VendingMachine.Core.Models;

namespace VendingMachine.BLL.Factories
{
    // Кошелек базовый класс
    public abstract class PurseBase
    {
        /// <summary>
        /// Coins in the wallet
        /// </summary>
        public List<Coin> Coins { get; }

        protected PurseBase(List<Coin> coins)
        {
            Coins = coins;
        }

        /// <summary>
        /// Payment by amount
        /// </summary>
        /// <param name="summ"></param>
        /// <returns></returns>
        public abstract IEnumerable<Coin> Pay(int summ);

        /// <summary>
        /// Payment with certain types of coins
        /// </summary>
        /// <param name="coins"></param>
        public abstract IEnumerable<Coin> Pay(IEnumerable<Coin> coins);

        /// <summary>
        /// Top up your wallet
        /// </summary>
        /// <param name="coins"></param>
        public abstract void Replenish(IEnumerable<Coin> coins);

        /// <summary>
        /// Checking the availability of coins in the wallet for the amount
        /// </summary>
        /// <param name="summ"></param>
        /// <returns></returns>
        public bool ValidateAmountCoins(int summ)
        {
            // get all matching coins
            var coins = GetCoins(summ);
            // if the sum of the found coins coincides with the total
            return summ == coins.Sum(x => x.Price); ;
        }

        /// <summary>
        /// Checking the required coins in the wallet by coin type
        /// </summary>
        /// <param name="coins"></param>
        /// <returns></returns>
        public bool ValidateAmountCoins(IEnumerable<Coin> coins)
        {
            if (coins == null) throw new ArgumentNullException(nameof(coins));

            // get all matching coins
            var coinsFind = GetCoins(coins);

            // check if all coins were found
            var coinsSum = coins.Sum(x => x.Price);
            var coinsFindSum = coinsFind.Sum(x => x.Price);
            return coinsSum == coinsFindSum;
        }

        /// <summary>
        /// Take away coins
        /// </summary>
        /// <param name="coins"></param>
        protected void RemoveCoins(IEnumerable<Coin> coins)
        {
            Coins.RemoveAll(coins.Contains);
        }

        /// <summary>
        /// Add coins
        /// </summary>
        /// <param name="coins"></param>
        protected void AddCoins(IEnumerable<Coin> coins)
        {
            Coins.AddRange(coins);
        }

        /// <summary>
        /// Get coins worth
        /// </summary>
        /// <param name="summ"></param>
        /// <returns></returns>
        protected IEnumerable<Coin> GetCoins(int summ)
        {
            List<Coin> resultList = new List<Coin>();
            // sort coins in descending order
            var orderCoin = Coins.OrderByDescending(x => x.TypeCoin);
            foreach (var coin in orderCoin)
            {
                if (summ > 0 && (summ - coin.Price) >= 0)
                {
                    summ -= coin.Price;
                    resultList.Add(coin);
                }
                else if (summ == 0)
                    break;
            }
            return resultList;
        }

        /// <summary>
        /// Get coins in a specific type of wallet
        /// </summary>
        /// <param name="coins"></param>
        /// <returns></returns>
        protected IEnumerable<Coin> GetCoins(IEnumerable<Coin> coins)
        {
            var resultList = new List<Coin>();

            // sorting out coins for search
            foreach (var coin in coins)
            {
                // we find all coins of this denomination in the wallet
                var findCoins = Coins.Where(x => x.TypeCoin == coin.TypeCoin);
                foreach (var findCoin in findCoins)
                {
                    // looking for the one that has not yet taken the coin
                    if (resultList.FirstOrDefault(x => x == findCoin) == null)
                    {
                        resultList.Add(findCoin);
                        break;
                    }
                }
            }

            return resultList;
        }
    }
}