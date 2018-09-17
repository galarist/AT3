////////////////////////////////////////////////////////////////////////////////////////////////////
// file:	LinkListNodeList.cs
//
// summary:	Implements the link list node list class
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;

/************************
 * Name: Cristovao Galambos
 * Student ID: 459230413
 * Purpose: Network Based Arithmetic Game Challenge
 * Finished Date: 17/09/2018
 * **********************/

namespace ArithmeticChallenge.NodeFunctions
{
    ////////////////////////////////////////////////////////////////////////////////////////////////////
    /// <summary>   List of link list nodes. </summary>
    ///
    /// <remarks>   Galarist, 18/09/2018. </remarks>
    ////////////////////////////////////////////////////////////////////////////////////////////////////

    class LinkListNodeList
    {
        //in this list a node can only have 3 states
        /// <summary>   The head node. </summary>
        public LinkListNode HeadNode;
        /// <summary>   The current node. </summary>
        public LinkListNode CurrentNode;
        /// <summary>   The tail node. </summary>
        public LinkListNode TailNode;

        /// <summary>   Number of. </summary>
        public static int count = 0;

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Default constructor. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LinkListNodeList(){}

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="node"> The node. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LinkListNodeList(LinkListNode node)
        {
            HeadNode = node;
            CurrentNode = node;
            TailNode = node;
            count++;
        }

        //properties to get or set a nodes value

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets current node. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <returns>   The current node. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LinkListNode getCurrentNode() { return CurrentNode; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets head node. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <returns>   The head node. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LinkListNode getHeadNode() { return HeadNode; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Gets tail node. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <returns>   The tail node. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public LinkListNode getTailNode() { return TailNode; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets current node. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="node"> The node. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void setCurrentNode(LinkListNode node) { CurrentNode = node; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets head node. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="node"> The node. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void setHeadNode(LinkListNode node) { HeadNode = node; }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sets tail node. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="node"> The node. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void setTailNode(LinkListNode node) { TailNode = node; }

        //add a node to the node list

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Adds an equation node. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="node"> The node. </param>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void AddEquationNode(LinkListNode node)
        {
            if ((HeadNode == null) && (CurrentNode == null) && (TailNode == null))
            {
                // this firstNode in the list
                HeadNode = node;
                CurrentNode = node;
                TailNode = node;
                count++;
            }
            else
            {
                //append the node to the list
                CurrentNode = node;
                HeadNode.SetPrevious(node);
                CurrentNode.SetNext(HeadNode);
                setHeadNode(CurrentNode);
                count++;
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Sort list. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public void SortList()
        {
            //sorts the list by the equation result
            LinkListNode current = HeadNode;
            for (LinkListNode i = current; i.GetNext() != null; i = i.GetNext())
            {
                for (LinkListNode j = i.GetNext(); j != null; j = j.GetNext())
                {
                    if (i.GetMyValue().ANumber > j.GetMyValue().ANumber)
                    {
                        var Temp = j.GetMyValue();
                        j.SetMyValue(i.GetMyValue());
                        i.SetMyValue(Temp);
                    }
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>   Binary search. </summary>
        ///
        /// <remarks>   Galarist, 18/09/2018. </remarks>
        ///
        /// <param name="searchValue">  The search value. </param>
        ///
        /// <returns>   An int. </returns>
        ////////////////////////////////////////////////////////////////////////////////////////////////////

        public int BinarySearch(LinkListNode searchValue)
        {
            //sorts the link list by result
            SortList();
            LinkListNode current = HeadNode;
            ArrayList myTempList = new ArrayList();
            for (LinkListNode i = current; i != null; i = i.GetNext())
            {
                myTempList.Add(i.GetMyValue());
            }

            //returns the integer value of where the node sits in the node list
            return myTempList.BinarySearch(searchValue);
        }
    }
}
