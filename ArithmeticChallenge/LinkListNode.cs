/*
 *      Student Number: 451381461
 *      Name:           Mitchell Stone
 *      Date:           14/09/2018
 *      Purpose:        Contains functions that are used to manipulate and navigate through a list of nodes. Contains a function to print
 *                      a node values.
 *      Known Bugs:     nill
 */

namespace ArithmeticChallenge.NodeFunctions
{
    class LinkListNode
    {
        public Equations equation;

        public LinkListNode previous;
        public LinkListNode next;

        public LinkListNode(Equations equation)
        {
            this.equation = equation;
        }

        public Equations GetMyValue()
        {
            return this.equation;
        }
        public void SetMyValue(Equations equation)
        {
            this.equation = equation;
        }

        public void SetNext(LinkListNode aNode)
        {
            this.next = aNode;
        }

        public LinkListNode GetNext()
        {
            return this.next;
        }

        public void SetPrevious(LinkListNode aNode)
        {
            this.previous = aNode;
        }

        public LinkListNode GetPrevious()
        {
            return this.previous;
        }

        //returns a string of a link list node values
        public string NodeToString()
        {
            return equation.FirstNumber.ToString() + equation.Symbol + equation.SecondNumber.ToString() + "=" + equation.Result.ToString();
        }
    }
}
