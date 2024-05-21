using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLab4.MerkleTree
{
    internal class MerkleNode<T>
    {
        public T Data { get; set; }
        public MerkleNode<T>? Left { get; private set; }
        public MerkleNode<T>? Right { get; private set; }
        public string Hash { get; private set; }

        public MerkleNode(T data, string hash) 
        {
            Data = data;
            Left = null;
            Right = null;
            Hash = hash;
        }
        public MerkleNode()
        {
            Left = null;
            Right = null;
            Hash = string.Empty;
        }
        public MerkleNode(string hash, MerkleNode<T> left, MerkleNode<T> right)
        {
            Left = left;
            Right = right;
            Hash = hash;
        }
        public bool AddNode(MerkleNode<T> node)
        {
            if (Left == null)
            {
                Left = node;
                return true;
            }
            else if (Right == null)
            {
                Right = node;
                return true;
            }
            return false;
        }

        public bool IsLeaf()
        {
            return Left == null && Right == null;
        }
        public string displayNode()
        {
            StringBuilder output = new StringBuilder();
            displayNode(output, 0);
            return output.ToString();
        }

        private void displayNode(StringBuilder output, int depth)
        {

            if (Right != null)
                Right.displayNode(output, depth + 1);

            output.Append('\t', depth);
            output.AppendLine(Data?.ToString() ?? "no data");


            if (Left != null)
                Left.displayNode(output, depth + 1);

        }
        public void IncludeWithChildren(IList<MerkleNode<T>> result)
        {
            if (Data != null)
            {
                result.Add(this);
            }
            Left?.IncludeWithChildren(result);
            Right?.IncludeWithChildren(result);
        }
    }
}
