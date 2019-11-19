using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Staff_Name
{
    class BinarySearchTree<T> { 
        public TreeNode<T> Root { get; set; } //Properties of BinarySearchTree<T>

        public BinarySearchTree()
        {
            this.Root = null;
        }

        //Add Method
        public void Add(T x)
        {
            this.Root = Add(x, this.Root);
        }

        //Remove Method
        public void Remove(T x)
        {
            this.Root = Remove(x, this.Root);
        }

        //Remove Min Method
        public void RemoveMin()
        {
            this.Root = RemoveMin (this.Root);
        }

        //Find Min Method
        public T FindMin()
        {
            return ElementAt(FindMin(this.Root));
        }

        //Find Max Method
        public T FindMax()
        {
            return ElementAt(FindMax(this.Root));
        }

        //Search Method
        public T Search(T x)
        {
            return ElementAt(Search(x, this.Root));
        }

        private T ElementAt(TreeNode<T> t)
        {
            return t == null ? default(T) : t.Element; //if null is true, the whole thing is equal to default(T). If false, it equals to t.Element
        }

        private TreeNode<T> Search (T x, TreeNode<T> t)
        {
            while (t != null)
            {
                if((x as IComparable).CompareTo(t.Element) < 0) //Compare X (Value inputted) with t.Element and see whether its less than 0
                
                    t = t.Left;
                
                else if ((x as IComparable).CompareTo(t.Element) > 0) //Compare X (Value inputted) with t.Element whether its more than 0

                    t = t.Right;
                
                else
                
                      return t;
                
            }
            return null;
        }

        private TreeNode<T> FindMin(TreeNode<T> t)
        {
            if(t != null)
            {
                while (t.Left != null) //if left node not null then t = left node
                {
                    t = t.Left;
                }
            }
            return t;
        }

        private TreeNode<T> FindMax(TreeNode<T> t)
        {
            if (t != null)
            {
                while (t.Right != null) //if right node not null then t = right node
                {
                    t = t.Right;
                }
            }
            return t;
        }

        protected TreeNode<T> Add (T x, TreeNode<T> t)
        {
            try
            {
                if (t == null) //if t = null, t = value inputted

                    t = new TreeNode<T>(x);

                else if ((x as IComparable).CompareTo(t.Element) < 0) //Compare X (Value inputted) with t.Element and see whether its less than 0

                    t.Left = Add(x, t.Left);

                else if ((x as IComparable).CompareTo(t.Element) > 0) //Compare X (Value inputted) with t.Element and see whether its more than 0

                    t.Right = Add(x, t.Right);


                return t;
            }
            catch 
            {
                throw new Exception("Duplicate name");
            }             
        }

        protected TreeNode<T> RemoveMin(TreeNode<T> t)
        {
            if(t == null) //if t is not found in the array, then throw "name not found"
            
                throw new Exception("Name not found");
            
            else if(t.Left != null) // if t is found in the array, remove the first value from the listBox
            {
                t.Left = RemoveMin(t.Left);
                return t;
            }
            else
            
                return t.Right;
            
        }

        protected TreeNode<T> Remove(T x, TreeNode<T> t)
        {
            if(t == null) //if t is not found in the array, then throw "name not found"

                throw new Exception("Name not found");
            
            else if ((x as IComparable).CompareTo(t.Element) < 0)//Compare X (Value inputted) with t.Element and see whether its less than 0

                t.Left = Remove(x, t.Left);//If yes, remove left node
            
            else if ((x as IComparable).CompareTo(t.Element) > 0)//Compare X (Value inputted) with t.Element and see whether its more than 0

                t.Right = Remove(x, t.Right); //if yes, remove right node
            
            else if (t.Left != null && t.Right != null) //if both is not null, then find the first value and remove it from the listBox
            {
                t.Element = FindMin(t.Right).Element;
                t.Right = RemoveMin(t.Right);
            }
            else
            
                t = (t.Left != null) ? t.Left : t.Right;
            
            return t;
        }

        public override string ToString()
        {
            return this.Root.ToString();
            
        }

    }
}
