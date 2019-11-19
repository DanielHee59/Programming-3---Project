using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Name
{
    class TreeNode<T>
    {
        public T Element { get; set; } //Properties of TreeNode<T>
        public TreeNode<T> Left { get; set; } 
        public TreeNode<T> Right { get; set; }


        //Constructer
        public TreeNode(T element)
        {
            this.Element = element;
        }

        //ToString Method
        public override string ToString()
        {
            string nodeString = "[" + this.Element + " ";

            if(this.Left == null && this.Right == null) //If left and right node is null, then display " (Leaf) "
            
                nodeString += " (Leaf) ";
            
            if (this.Left != null)
            
                nodeString += "Left: " + this.Left.ToString();//If left node is null, then display " Left.ToString() "
            
            if (this.Right != null)
            
                nodeString += "Right: " + this.Right.ToString();//If right node is null, then display " Right.ToString() "
            
        
             nodeString += "] ";
            
            return nodeString;
      
        }
    }
}
