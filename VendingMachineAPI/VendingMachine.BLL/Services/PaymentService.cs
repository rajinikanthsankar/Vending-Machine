using System;
using System.Threading.Tasks;
using System.Transactions;
using VendingMachine.BLL.DTO;
using VendingMachine.BLL.Interfaces;
using VendingMachine.Core.Models;
using VendingMachine.DAL.Interfaces;

namespace VendingMachine.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPurseRepository _purseRepository;
        private readonly IUserDepositRepository _userDepositRepository;
        private readonly ICustomerProductRepository _customerProductRepository;
        private readonly IVendingMachineService _vendingMachineService;

        public PaymentService(IPurseRepository purseRepository,
            IUserDepositRepository userDepositRepository,
            ICustomerProductRepository customerProductRepository,
            IVendingMachineService vendingMachineService)
        {
            _purseRepository = purseRepository;
            _userDepositRepository = userDepositRepository;
            _customerProductRepository = customerProductRepository;
            _vendingMachineService = vendingMachineService;
        }

        // The buyer deposited the coin into the coin acceptor (deposit)
        public async Task AddAmountDepositAsync(CoinDTO coin, Guid userId)
        {
            if (coin == null)
            {
                throw new NullReferenceException("Coin is null");
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // Increase the amount of the user's deposit
                    await _userDepositRepository.AddAmountDepositAsync((int)coin.TypeCoin, userId);

                    // we take a coin from the user
                    await _purseRepository.RemoveCoinAsync(userId, coin.TypeCoin);

                    // TODO: To verify a transaction, you need to uncomment this line ->
                    // throw new Exception("To verify a transaction, you need to uncomment this line.");

                    // give a coin to VM
                    await _vendingMachineService.AddCoinAsync(coin.TypeCoin);

                    // Commit transaction if all commands succeed, transaction will auto-rollback
                    // when disposed if either commands fails
                    scope.Complete();
                }
                catch (System.Exception)
                {
                    // TODO: Handle failure
                    throw new ApplicationException("The problem of adding a user deposit amount!");
                }
            };
        }

        public async Task<ProductDTO> BuyProduct(Guid userId, TypeProduct typeProduct)
        {
            ProductDTO product = null;
            var creatorProduct = await _vendingMachineService.GetInfoProductAsync(typeProduct);
            var depositCustomer = await _userDepositRepository.GetAmountDepositAsync(userId);

            if (creatorProduct.Product.Price > depositCustomer)
            {
                throw new ApplicationException("The amount of the deposit is less than the value of the goods");
            }

            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // we reduce the buyer's deposit by the amount of the goods
                    await _userDepositRepository.RetrieveDepositAsync(userId, creatorProduct.Product.Price);
                    // release the goods from the car
                    product = await _vendingMachineService.CreateProductAsync(typeProduct);

                    var customerProduct = new DAL.Entities.CustomerProduct
                    {
                        CustomerId = userId,
                        Name = product.Name,
                        Price = product.Price
                    };

                    // save the issued goods to the user history
                    await _customerProductRepository.Create(customerProduct);

                    scope.Complete();
                }
                catch (System.Exception)
                {
                    // TODO: Handle failure
                    throw new ApplicationException("The problem of buying product!");
                }
            }

            return product;
        }

        // return change to the buyer
        public async Task GetDepositCustomerAsync(Guid userId)
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // find the amount of the deposit
                    var amoutDeposit = await _userDepositRepository.GetAmountDepositAsync(userId);

                    // write off the user's deposit
                    await _userDepositRepository.RetrieveDepositAsync(userId);

                    // take coins from VM
                    var coins = await _vendingMachineService.RetrieveCoinsAsync(amoutDeposit);

                    // give a coin Customer
                    await _purseRepository.AddCoinsAsync(userId, coins);

                    scope.Complete();
                }
                catch (System.Exception)
                {
                    // TODO: Handle failure
                    throw new ApplicationException("The problem of receiving a deposit by the user!");
                }
            }
        }
    }
}
