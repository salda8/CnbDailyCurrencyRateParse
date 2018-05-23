// ReSharper disable NonReadonlyMemberInGetHashCode, need public setter for xml serialization
namespace CurrencyRate.DataStructures.Dto
{
    public struct ExchangeRate
    {
        public ExchangeRate(string kod, decimal kurz, string mena, int mnozstvi, string zeme)
        {
            Kod = kod;
            Kurz = kurz;
            Mena = mena;
            Mnozstvi = mnozstvi;
            Zeme = zeme;
        }

        public string Kod { get; set; }
        public decimal Kurz { get; set; }
        public string Mena { get; set; }
        public int Mnozstvi { get; set; }
        public string Zeme { get; set; }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is ExchangeRate))
                return false;

            var rate = (ExchangeRate)obj;
            // compare elements here
            return Kod == rate.Kod;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Kod?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Kurz.GetHashCode();
                hashCode = (hashCode * 397) ^ (Mena?.GetHashCode() ?? 0);
                hashCode = (hashCode * 397) ^ Mnozstvi;
                hashCode = (hashCode * 397) ^ (Zeme?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}