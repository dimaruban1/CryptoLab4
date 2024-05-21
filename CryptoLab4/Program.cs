/*
Реалізувати спрощений варіант блокчейна.
Дані зберігаються блоками.+
Збереження/Загрузка даних з/на диск (файл, бд, тощо)-
Дані - інформація про перерахунок коштів. (кількість та шлях додавання у систему довільні)+
Реалізувати перевірку коректності(цілісності) даних у блокчейні.+
Для вказаного блоку: вивести всіх хто має кошти, мін/макс кількість коштів на рахунку особи за всю попередню історію.+
Використовувати Merkle tree як всередині блоку(для кожної операції) так і по всім блокам.+
Реалізувати “proof-of-work” для додавання блоку.
 */

//using CryptoLab4;
//using CryptoLab4.MerkleTree;

using CryptoLab4;
using CryptoLab4.MerkleTree;
using System.Data;
using System.Text;

class BlockChain
{
    public MerkleTree<Block> BlockTree { get; private set; }
    public List<Transaction> TransactionPool { get; private set; }

    private Block LastBlock;
    private int Difficulty;

    public BlockChain(int difficulty)
    {
        Difficulty = difficulty;
        TransactionPool = new List<Transaction>();
        var rootBlock = new Block("-", new List<Transaction>(), new Dictionary<string, decimal>(), Difficulty);
        var rootNode = new MerkleNode<Block>(rootBlock, "123");
        BlockTree = new MerkleTree<Block>(rootNode);
        LastBlock = rootNode.Data;
    }

    private void CreateBlock()
    {
        var prevLog = LastBlock.LogBalance;
        var newLog = new Dictionary<string, decimal>(prevLog);
        foreach (var transaction in TransactionPool)
        {
            newLog[transaction.Sender] -= transaction.Amount;
            if (!prevLog.Keys.Contains(transaction.Receiver))
            {
                newLog[transaction.Receiver] = 0;
            }
            newLog[transaction.Receiver] += transaction.Amount;
        }

        var newBlock = new Block(LastBlock.Hash, TransactionPool, newLog, Difficulty);
        newBlock.MineBlock();
        LastBlock = newBlock;
        TransactionPool.Clear();
        // Add new block to the Merkle tree (assuming you have a method to add a block)
        // BlockTree.AddBlock(newBlock);  // Implement this method in MerkleTree if necessary
    }

    private void CreateBlock()
    {
        var prevLog = LastBlock.LogBalance;
        var newLog = new Dictionary<string, decimal>(prevLog);
        foreach (var transaction in TransactionPool)
        {
            newLog[transaction.Sender] -= transaction.Amount;
            if (!prevLog.Keys.Contains(transaction.Receiver))
            {
                newLog[transaction.Receiver] = 0;
            }
            newLog[transaction.Receiver] += transaction.Amount;
        }

        var NewLastBlock = new Block(LastBlock.Hash, TransactionPool, newLog);
        LastBlock = NewLastBlock;
        TransactionPool.Clear();
        //BlockTree.AddBlock();
    }
    public IEnumerable<decimal> GetUserBalanceHistoryFromTransactions(string user)
    {
        List<decimal> result = new List<decimal>();
        var blockLeaves = BlockTree.GetLeaves();
        foreach(var blockNode in blockLeaves)
        {
            var transactionLeaves = blockNode.Data.Transactions.GetLeaves();
            var relevantTransactions = transactionLeaves.Select(t => t.Data)
                .Where(transaction => transaction.Sender == user || transaction.Receiver == user);
            
            decimal last = 0;
            foreach (var t in relevantTransactions)
            {
                if (t.Sender == user)
                    last -= t.Amount;
                if (t.Receiver == user)
                    last += t.Amount;
                else
                    throw new Exception("wow");
            }
        }

        return result;
    }

    public IEnumerable<decimal> GetUserBalanceHistoryFromLogs(string user)
    {
        List<decimal> result = new List<decimal>();
        var blockLeaves = BlockTree.GetLeaves();
        foreach (var blockNode in blockLeaves)
        {
            if (!blockNode.Data.LogBalance.ContainsKey(user))
            {
                continue;
            }
            result.Add(blockNode.Data.LogBalance[user]);
        }

        return result;
    }
    
    public string GetFullBlockInfo(Block block)
    {
        StringBuilder res = new StringBuilder();
        res.AppendLine(block.GetLogInfo());
        res.AppendLine("Min/Max balance info:");
        foreach (var user in block.LogBalance.Keys)
        {
            var userHistory = GetUserBalanceHistoryFromLogs(user);
            res.AppendLine($"{user}: min {userHistory.Min()}, max{userHistory.Max()}");
        }
        return res.ToString();
    }

    public bool ProveWork()
    {
        return false;
    }
    public bool CommitTransaction(string sender, string receiver, decimal amount)
    {
        if (sender == "SYSTEM")
        {
            TransactionPool.Add(new Transaction(sender, receiver, amount));
            return true;
        }
        if (!LastBlock.LogBalance.ContainsKey(sender))
        {
            return false;
        }
        
        var senderBalance = LastBlock.LogBalance[sender];
        var senderOutgoingTransactions = TransactionPool.Where(t => t.Sender == sender);
        var senderIncomingTransactions = TransactionPool.Where(t => t.Receiver == sender);
        senderBalance += senderIncomingTransactions.Sum(t => t.Amount);
        senderBalance -= senderOutgoingTransactions.Sum(t => t.Amount);
        if (senderBalance >=  amount)
        {
            TransactionPool.Add(new Transaction(sender, receiver, amount));
            if (TransactionPool.Count >= 12)
            {
                CreateBlock();
            }
            return true;
        }
        return false;
    }
}

class Program
{
    public static void Main()
    {
        // Test Merkle tree
        BlockChain blockChain = new BlockChain();
        for (int i = 1; i < 1112; i++)
        {
            blockChain.CommitTransaction("SYSTEM", $"user{i}", 100 + i * 11);
        }
        var leaves = blockChain.BlockTree.GetLeaves();
        foreach (var leaf in leaves)
        {
            leaf.displayNode();
        }
    }
}
