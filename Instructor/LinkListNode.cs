////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	LinkListNode.cs
//
// summary:	Implements the link list node class
////////////////////////////////////////////////////////////////////////////////////////////////////

namespace ArithmeticChallenge.NodeFunctions
{
    /************************
     * Name: Cristovao Galambos
     * Student ID: 459230413
     * Purpose: Network Based Arithmetic Game Challenge
     * Finished Date: 17/09/2018
     * **********************/

    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   A link list node. </summary>
    ///
    /// <remarks>   Galarist, 18/09/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    class LinkListNode
    {
        /// <summary>   The equation. </summary>
        public Equations equation;

        /// <summary>   The previous. </summary>
        public LinkListNode previous;
        /// <summary>   The next. </summary>
        public LinkListNode next;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="equation"> The equation. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LinkListNode(Equations equation)
        {
            this.equation = equation;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets my value. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <returns>   my value. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public Equations GetMyValue()
        {
            return this.equation;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets my value. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="equation"> The equation. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void SetMyValue(Equations equation)
        {
            this.equation = equation;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets a next. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="aNode">    The node. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void SetNext(LinkListNode aNode)
        {
            this.next = aNode;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the next item. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <returns>   The next. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LinkListNode GetNext()
        {
            return this.next;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets the previous. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="aNode">    The node. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void SetPrevious(LinkListNode aNode)
        {
            this.previous = aNode;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets the previous item. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <returns>   The previous. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LinkListNode GetPrevious()
        {
            return this.previous;
        }

        //returns a string of a link list node values

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Node to string. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <returns>   A string. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public string NodeToString()
        {
            return equation.FNumber.ToString() + equation.Operator + equation.SNumber.ToString() + "=" + equation.ANumber.ToString();
        }
    }
}
