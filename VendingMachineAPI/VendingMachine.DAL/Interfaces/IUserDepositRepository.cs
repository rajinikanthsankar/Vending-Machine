using System;
using System.Threading.Tasks;
using VendingMachine.DAL.Entities;

namespace VendingMachine.DAL.Interfaces
{
    public interface IUserDepositRepository : IRepository<UserDeposit, Guid>
    {
        /// <summary>
        /// How much does the user have on the deposit
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<int> GetAmountDepositAsync(Guid userId);
        /// <summary>
        /// Add a deposit to the user
        /// </summary>
        /// <param name="sum"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task AddAmountDepositAsync(int sum, Guid userId);
        /// <summary>
        /// We will write off the entire deposit (change)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RetrieveDepositAsync(Guid userId);
        /// <summary>
        /// We write off the deposit for the amount
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="sum"></param>
        /// <returns></returns>
        Task RetrieveDepositAsync(Guid userId, int sum);
    }
}
