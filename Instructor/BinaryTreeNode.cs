
/************************
 * Name: Cristovao Galambos
 * Student ID: 459230413
 * Purpose: Network Based Arithmetic Game Challenge
 * Finished Date: 17/09/2018
 * **********************/

 namespace ArithmeticChallenge.NodeFunctions
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A binary tree node. </summary>
    ///
    /// <remarks>   Galarist, 18/09/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    class BinaryTreeNode
    {
        /// <summary>   The tree equation. </summary>
        public Equations treeEquation;
        /// <summary>   The top. </summary>
        public BinaryTreeNode top;
        /// <summary>   The left. </summary>
        public BinaryTreeNode left;
        /// <summary>   The right. </summary>
        public BinaryTreeNode right;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="equation"> The equation. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public BinaryTreeNode(Equations equation)
        {
            treeEquation = equation;
            top = null;
            left = null;
            right = null;
        }

        //returns a string of a binary tree node

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Node to string. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <returns>   A string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string NodeToString()
        {
            return treeEquation.ANumber.ToString() + "(" +treeEquation.FNumber.ToString() + treeEquation.Operator + treeEquation.SNumber.ToString() + "), ";
        }

    }
}
