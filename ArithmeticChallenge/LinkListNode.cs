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
            return equation.FNumber.ToString() + equation.Operator + equation.SNumber.ToString() + "=" + equation.ANumber.ToString();
        }
    }
}
