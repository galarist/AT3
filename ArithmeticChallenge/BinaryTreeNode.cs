﻿
/************************
 * Name: Cristovao Galambos
 * Student ID: 459230413
 * Purpose: Network Based Arithmetic Game Challenge
 * Finished Date: 17/09/2018
 * **********************/

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
            return treeEquation.ANumber.ToString() + "(" +treeEquation.FNumber.ToString() + treeEquation.Operator + treeEquation.SNumber.ToString() + "), ";
        }

    }
}
