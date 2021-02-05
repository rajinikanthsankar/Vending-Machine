using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VendingMachine.Core.Models;
using VendingMachine.DAL.Entities;

namespace VendingMachine.DAL.Interfaces
{
    public interface IPurseRepository : IRepository<Purse, Guid>
    {
        /// <summary>
        /// We receive a wallet and coins in it
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<Purse> GetPurseAndCoinsAsync(Guid userId);

        /// <summary>
        /// Add a coin to the wallet
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="typeCoin"></param>
        /// <returns></returns>
        Task AddCoinAsync(Guid userId, TypeCoin typeCoin);

        /// <summary>
        /// Add coins
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="coins"></param>
        /// <returns></returns>
        Task AddCoinsAsync(Guid userId, IEnumerable<Coin> coins);

        /// <summary>
        /// Delete coin by type
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="coin"></param>
        /// <returns></returns>
        Task RemoveCoinAsync(Guid userId, TypeCoin typeCoin);

        /// <summary>
        /// Removing coins by amount
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        Task<IEnumerable<Coin>> RemoveCoinsAsync(Guid userId, int sum);
    }
}
