using System.Collections;

/************************
 * Name: Cristovao Galambos
 * Student ID: 459230413
 * Purpose: Network Based Arithmetic Game Challenge
 * Finished Date: 17/09/2018
 * **********************/

namespace ArithmeticChallenge.NodeFunctions
{
    class LinkListNodeList
    {
        //in this list a node can only have 3 states
        public LinkListNode HeadNode;
        public LinkListNode CurrentNode;
        public LinkListNode TailNode;

        public static int count = 0;

        public LinkListNodeList(){}

        public LinkListNodeList(LinkListNode node)
        {
            HeadNode = node;
            CurrentNode = node;
            TailNode = node;
            count++;
        }

        //properties to get or set a nodes value
        public LinkListNode getCurrentNode() { return CurrentNode; }
        public LinkListNode getHeadNode() { return HeadNode; }
        public LinkListNode getTailNode() { return TailNode; }

        public void setCurrentNode(LinkListNode node) { CurrentNode = node; }
        public void setHeadNode(LinkListNode node) { HeadNode = node; }
        public void setTailNode(LinkListNode node) { TailNode = node; }

        //add a node to the node list
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
