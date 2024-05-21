using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLab4.MerkleTree
{
    internal class MerkleTree<T>
    {
        public MerkleNode<T> Root { get; set; }
        public string RootHash { get; private set; }
        public MerkleTree(MerkleNode<T> root)
        {
            Root = root;
            RootHash = root.Hash;
        }
        public MerkleTree(IList<T> data)
        {
            List<MerkleNode<T>> nodes = BuildTree(data);
            Root = nodes.Last();
            RootHash = Root.Hash;
        }
        private List<MerkleNode<T>> BuildTree(IList<T> data)
        {
            List<MerkleNode<T>> nodes = new List<MerkleNode<T>>();

            foreach (T item in data)
            {
                nodes.Add(new MerkleNode<T>(item, Hasher.GetHashString(item?.ToString() ?? string.Empty)));
            }

            while (nodes.Count > 1)
            {
                List<MerkleNode<T>> parentLevel = new List<MerkleNode<T>>();
                for (int i = 0; i < nodes.Count; i += 2)
                {
                    if (i + 1 < nodes.Count)
                    {
                        parentLevel.Add(CombineNodes(nodes[i], nodes[i + 1]));
                    }
                    else
                    {
                        parentLevel.Add(new MerkleNode<T>(nodes[i].Data, Hasher.GetHashString(nodes[i].Data?.ToString() ?? string.Empty)));
                    }
                }
                nodes = parentLevel;
            }

            return nodes;
        }

        private MerkleNode<T> CombineNodes(MerkleNode<T> left, MerkleNode<T> right)
        {
            var combinedData = left.Hash + right.Hash;
            var hash = Hasher.GetHashString(combinedData);
            return new MerkleNode<T>(hash, left, right);
        }

        //public bool ValidatePow(byte[] data, int difficulty)
        //{
        //    byte[] targetHash = new byte[32];  // Assuming 32-byte hashes
        //    Array.Fill<byte>(targetHash, 0, difficulty / 8);

        //    // Loop through nonces
        //    for (long nonce = 0; nonce < long.MaxValue; nonce++)
        //    {
        //        // Combine data and nonce
        //        byte[] combinedData = Combine(data, BitConverter.GetBytes(nonce));

        //        // Hash data with Merkle tree (replace with your implementation)
        //        byte[] merkleRoot = GetRoot(HashNode(combinedData));

        //        // Check if hash meets difficulty criteria
        //        if (CompareHash(merkleRoot, targetHash) >= 0)
        //        {
        //            Console.WriteLine("Valid Proof Found! Nonce: {0}", nonce);
        //            return true;
        //        }
        //    }

        //    Console.WriteLine("Proof of Work Failed!");
        //    return false;
        //}

        private byte[] Combine(byte[] data1, byte[] data2)
        {
            // Combine data for hashing (replace with your logic)
            return data1.Concat(data2).ToArray();
        }

        private int CompareHash(byte[] hash1, byte[] hash2)
        {
            // Compare byte arrays for leading zeros (replace with your logic)
            for (int i = 0; i < hash1.Length; i++)
            {
                if (hash1[i] != hash2[i])
                {
                    return hash1[i] - hash2[i];
                }
            }
            return 0;
        }
        public IList<MerkleNode<T>> GetLeaves()
        {
            var res = new List<MerkleNode<T>>();
            Root.IncludeWithChildren(res);
            return res;
        }
    }
}
