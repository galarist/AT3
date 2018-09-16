using ArithmeticChallenge.NodeFunctions;
using System;
using System.Collections;
using System.Linq;
using System.Text;

namespace ArithmeticChallenge.Controllers
{
    class InstructorController
    {
        //returns the result of the calculation performed      
        public static int PerformCalculation(string first, string second, string symbol)
        {
            int firstNum = 0;
            int secondNum = 0;

            if (string.IsNullOrWhiteSpace(first))
            {
                firstNum = 0;
            }
            else
            {
                firstNum = Convert.ToInt32(first);                
            }

            if (string.IsNullOrWhiteSpace(second))
            {
                secondNum = 0;
            }
            else
            {
                secondNum = Convert.ToInt32(second);
            }

            int result = 0;
            switch (symbol)
            {
                case "+":
                    result = firstNum + secondNum;
                    break;
                case "-":
                    result = firstNum - secondNum;
                    break;
                case "x":
                    result = firstNum * secondNum;
                    break;
                case "/":
                    if (secondNum == 0)
                    {
                        result = 0;
                        break;
                    }
                    else
                    {
                        result = firstNum / secondNum;
                        break;
                    }
                default:
                    break;
            }
            return result;
        }

        //Hash table of all nodes
        public static Hashtable NodeHastTable(LinkListNodeList nodeList)
        {
            //returns a hash table containing all the nodes
            Hashtable tempTable = new Hashtable();
            int count = 1;

            //loop over the link list and add all the nodes to the hash table
            for (LinkListNode i = nodeList.CurrentNode; i.GetNext() != null; i = i.GetNext())
            {
                tempTable.Add(count.ToString(), i);
                count++;
            }
            return tempTable;
        }

        public static LinkListNode SearchNodeDict(Hashtable nodeHashtable, LinkListNode searchResult)
        {
            //uses linq to search through the hastable for a node looking at the result
            return (LinkListNode)nodeHashtable.Values.OfType<LinkListNode>().Where(x => x == searchResult);
        }

        public static string PrintLinkList(LinkListNodeList nodeList)
        {
            //goes through each node in the link list and builds a string to be displayed for all incorrect answers
            StringBuilder sb = new StringBuilder();

            sb.Append("HEAD");
           if (nodeList.HeadNode.GetNext() != null)
            {
                //add each incorrect node string to the string builder until the tail node is reached
                sb.Append("<-> " + nodeList.HeadNode.NodeToString());
                nodeList.CurrentNode = nodeList.HeadNode.GetNext();
                while (nodeList.CurrentNode != null)
                {
                    if (nodeList.CurrentNode.GetMyValue().IsCorrect == false)
                    {
                        sb.Append(" <-> " + nodeList.CurrentNode.NodeToString());
                        nodeList.CurrentNode = nodeList.CurrentNode.GetNext();
                    }
                    else
                    {
                        nodeList.CurrentNode = nodeList.CurrentNode.GetNext();
                    }
                }
            }
            sb.Append(" <-> TAIL");
            return sb.ToString();
        }
    }
}
