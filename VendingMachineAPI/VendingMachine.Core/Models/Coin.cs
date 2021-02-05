namespace VendingMachine.Core.Models
{
    // coin and price
    public class Coin
    {
        public TypeCoin TypeCoin { get; set; }

        public Coin(TypeCoin typeCoin)
        {
            TypeCoin = typeCoin;
        }

        // coin price in int
        public int Price => (int)this.TypeCoin;
    }
}