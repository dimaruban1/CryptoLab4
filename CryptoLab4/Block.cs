using CryptoLab4.MerkleTree;
using System.Text;

namespace CryptoLab4
{
    internal class Block
    {
        public string Hash { get; private set; }
        public string PreviousHash { get; set; }
        public MerkleTree<Transaction> Transactions { get; private set; }
        public readonly Dictionary<string, decimal> LogBalance;
        public int Nonce { get; private set; }
        public int Difficulty { get; private set; }

        public Block(string previousHash, List<Transaction> transactions, Dictionary<string, decimal> logBalance, int difficulty)
        {
            PreviousHash = previousHash;
            Transactions = new MerkleTree<Transaction>(transactions);
            Hash = CalculateHash();
            LogBalance = logBalance;
            Nonce = 0;
            Difficulty = difficulty;
        }

        public string CalculateHash()
        {
            string dataToHash = PreviousHash + Transactions.RootHash + Nonce;
            return Hasher.GetHashString(dataToHash);
        }

        public void MineBlock()
        {
            string target = new string('0', Difficulty);
            while (Hash.Substring(0, Difficulty) != target)
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }

        public bool ValidateHash()
        {
            return Hash == CalculateHash();
        }


        public string GetLogInfo()
        {
            StringBuilder result = new StringBuilder();
            result.AppendLine("User balances:");
            foreach (var kv in LogBalance)
            {
                result.AppendLine($"{kv.Key}: {kv.Value}");
            }
            return result.ToString();
        }
    }
}
