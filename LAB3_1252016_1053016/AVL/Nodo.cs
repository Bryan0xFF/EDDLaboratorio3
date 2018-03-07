using System;
using System.Collections.Generic;
using System.Text;

namespace AVL
{
    public class Nodo<T>
    {
        public T Value { get; set; }
        public Nodo<T> Left { get; set; }
        public Nodo<T> Right { get; set; }
        public Nodo<T> Parent { get; set; }
    }
}
