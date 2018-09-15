/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           14/09/2018
 *      Purpose:        Contains functions to create and add to the binary tree. Contains recursive functions for the print
 *                      binary tree in post-order, in-order and pre-order
 *      Known Bugs:     nill
 */

using System.Text;

namespace ArithmeticChallenge.NodeFunctions
{
    class BinaryTree
    {
        //the first node of the binary tree
        public BinaryTreeNode root;

        //string used to print the binary tree
        private static string printString = "";

        public BinaryTree()
        {
            root = null;
        }

        public BinaryTree(Equations value)
        {
            root = new BinaryTreeNode(value);
        }

        public void Add(Equations myValue)
        {
            if (root == null)
            {
                //the tree is empty so make the first node the root node
                root = new BinaryTreeNode(myValue);
                return;
            }

            BinaryTreeNode currentNode = root;
            bool insert = false;

            do
            {
                if (myValue.Result < currentNode.treeEquation.Result)
                {
                    //insert to the left
                    if (currentNode.left == null)
                    {
                        currentNode.left = new BinaryTreeNode(myValue);
                        insert = true;
                    }
                    else
                    {
                        //move left
                        currentNode = currentNode.left;
                    }
                }

                if (myValue.Result >= currentNode.treeEquation.Result)
                {
                    // move right
                    if (currentNode.right == null)
                    {
                        currentNode.right = new BinaryTreeNode(myValue);
                        insert = true;
                    }
                    else
                    {
                        currentNode = currentNode.right;
                    }
                }
            } while (!insert);
        }//end function

        public static string PrintPostOrder(BinaryTree tree)
        {
            printString = "";
            PrintPostOrder(tree.root);
            return printString;
        }

        //given a the root node of a binary tree, print in post-order
        public static void PrintPostOrder(BinaryTreeNode node)
        {
            if (node == null)
            {
                return;
            }

            //first recur on left subtree
            PrintPostOrder(node.left);

            //then recur on right subtree
            PrintPostOrder(node.right);

            //add text to the rich text box\
            printString += node.NodeToString();
        }

        public static string PrintInOrder(BinaryTree tree)
        {
            printString = "";
            PrintInOrder(tree.root);
            return printString;
        }

        //given the root node of a binary tree, print in-order
        public static void PrintInOrder(BinaryTreeNode node)
        {
            if (node == null)
                return;

            //first recur on left subtree
            PrintInOrder(node.left);

            //then print the data of node
            printString += node.NodeToString();   

            //now recur on right subtree
            PrintInOrder(node.right);
        }

        public static string PrintPreOrder(BinaryTree tree)
        {
            printString = "";
            PrintPreOrder(tree.root);
            return printString;
        }

        //given the root node of a binary, print in pre-order
        public static void PrintPreOrder(BinaryTreeNode node)
        {
            if (node == null)
                return;

            //first print data of the node
            printString += node.NodeToString();

            //then recur on left subtree
            PrintPreOrder(node.left);

            //now recur on right subtree
            PrintPreOrder(node.right);
        }

        public BinaryTreeNode FindNodeByResultValue(BinaryTreeNode node, int resultValue)
        {
            //returns the node where the result value equals the search value
            if (node == null)
            {
                return null;
            }
            if (node.treeEquation.Result == resultValue)
            {
                return node;
            }
            if (node.treeEquation.Result > resultValue) return FindNodeByResultValue(node.left, resultValue);
            return FindNodeByResultValue(node.right, resultValue);
        }

        private static string PrintNode(BinaryTreeNode node)
        {
            //creates a single string for a nodes properties
            StringBuilder sb = new StringBuilder();

            sb.Append(node.treeEquation.Result.ToString());
            sb.Append("(");
            sb.Append(node.treeEquation.FirstNumber.ToString());
            sb.Append(node.treeEquation.Symbol);
            sb.Append(node.treeEquation.SecondNumber.ToString());
            sb.Append(")");

            return sb.ToString();
        }
    }
}
