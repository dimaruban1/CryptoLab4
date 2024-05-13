using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLab4.MerkleTree
{
    internal class MerkleTree<T>
    {
        public Node<T> Root { get; set; }
        public MerkleTree(Node<T> root)
        {
            Root = root;
        }

    }
}
