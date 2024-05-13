using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLab4.MerkleTree
{
    internal class Node<T>
    {
        public T Data { get; set; }
        public Node<T>? LeftLeaf { get; private set; }
        public Node<T>? RightLeaf { get; private set; }
        public string Hash { get; private set; }

        public Node(T data) 
        {
            Data = data;
            LeftLeaf = null;
            RightLeaf = null;
        }
        public void AddNewData(T data)
        {
            var n = new Node<T>(data);
            AddLeaf(n);
        }
        public void AddLeaf(Node<T> node)
        {
            if (LeftLeaf == null)
            {
                LeftLeaf = node;
            }
            if (RightLeaf == null)
            {
                RightLeaf = node;
            }
            var random = DateTime.Now.Second % 2;
            if (random == 0)
            {
                RightLeaf.AddLeaf(node);
            }
            else
            {
                LeftLeaf.AddLeaf(node);
            }
        }
        public string ComputeHash()
        {
            string leftHash = LeftLeaf?.Hash ?? string.Empty;
            string rightHash = RightLeaf?.Hash ?? string.Empty;

            return Hasher.GetHashString(leftHash + rightHash);
        }
        public string ComputeAndReplaceHash()
        {
            var computedHash = ComputeHash();
            Hash = computedHash;
            return computedHash;
        }
        public bool IsLeaf()
        {
            return LeftLeaf == null && RightLeaf == null;
        }
    }
}
