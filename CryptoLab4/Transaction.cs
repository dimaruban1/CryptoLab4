using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLab4
{
    public class Transaction
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public decimal Amount { get; set; }
    }
}
