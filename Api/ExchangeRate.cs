using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CurrencyRate
{
    public struct ExchangeRate
    {
        public string Zeme { get; set; }
        public string Mena { get; set; }
        public int Mnozstvi { get; set; }
        public string Kod { get; set; }
        public decimal Kurz { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ExchangeRate))
                return false;

            var rate = (ExchangeRate)obj;
            // compare elements here
            return Kod == rate.Kod;

        }

    }
}
