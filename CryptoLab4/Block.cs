using CryptoLab4.MerkleTree;

namespace CryptoLab4
{
    internal class Block
    {
        public string Hash { get; set; }
        public string PreviousHash { get; set; }
        public Block(string hash, string previousHash)
        {
            Hash = hash;
            PreviousHash = previousHash;
        }
        public string CalculateHash()
        {
            return Hasher.GetHashString("sussy");
        }
        public bool ValidateHash()
        {
            return Hash == CalculateHash();
        }
        public MerkleTree<Transaction> Transactions { get; set; }
    }
}
