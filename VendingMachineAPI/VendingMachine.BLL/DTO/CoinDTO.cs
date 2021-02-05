using VendingMachine.Core.Models;

namespace VendingMachine.BLL.DTO
{
    public class CoinDTO
    {
        public TypeCoin TypeCoin { get; set; }

        // coin price in int
        //public int Price => (int)this.TypeCoin;
    }
}