/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           14/09/2018
 *      Purpose:        Contains variable that are used to build a binary tree. Has a function to print out a binary tree node.
 *      Known Bugs:     nill
 */

namespace ArithmeticChallenge.NodeFunctions
{
    class BinaryTreeNode
    {
        public Equations treeEquation;
        public BinaryTreeNode top;
        public BinaryTreeNode left;
        public BinaryTreeNode right;

        public BinaryTreeNode(Equations equation)
        {
            treeEquation = equation;
            top = null;
            left = null;
            right = null;
        }

        //returns a string of a binary tree node
        public string NodeToString()
        {
            return treeEquation.Result.ToString() + "(" +treeEquation.FirstNumber.ToString() + treeEquation.Symbol + treeEquation.SecondNumber.ToString() + "), ";
        }

    }
}
