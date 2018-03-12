using System;
using System.Collections.Generic;
using System.Text;

namespace AVL
{
   public class AVLTree<T> : iArbolBinario<T> where T : IComparable<T>
    {
        public Nodo<T> cabeza;
        private List<Nodo<T>> inOrden = new List<Nodo<T>>();
        private List<Nodo<T>> preOrden = new List<Nodo<T>>();
        private List<Nodo<T>> postOrden = new List<Nodo<T>>();

        public void Delete(T value)
        {
            throw new NotImplementedException();
        }

        public void Delete(Nodo<T> node, T value)
        {
            throw new NotImplementedException();
        }

        public List<Nodo<T>> InOrden(Nodo<T> node)
        {
            throw new NotImplementedException();
        } 

        public List<Nodo<T>> PostOrden(Nodo<T> node)
        {
            throw new NotImplementedException();
        }

        public List<Nodo<T>> PreOrden(Nodo<T> node)
        {
            throw new NotImplementedException();
        }

        public Nodo<T> Insert(T value)
        {
            return cabeza = Insert(cabeza, value);
        }

        public Nodo<T> Insert(Nodo<T> node, T value)
        {
            if (cabeza == null)
            {
                cabeza = new Nodo<T>();
                cabeza.Value = value;
                return cabeza;
            }
            if (node == null)
            {
                node = new Nodo<T>();
                node.Value = value;
            }

            else if (value.CompareTo(node.Value) == -1)
            {
                node.Left = Insert(node.Left, value);
                node.Left.Parent = node;
                node = Balancear(node);
            }
            else
            {
                node.Right = Insert(node.Right, value);
                node.Right.Parent = node;
                node = Balancear(node);
            }
            return node;
        }

        public Nodo<T> Balancear(Nodo<T> actual)
        {
            int factor = FactorBalance(actual);

            if (factor > 1)
            {
                if (FactorBalance(actual.Left) > 0)
                   return actual = RotarDerIzq(actual);
                else
                   return actual = RotarIzqIzq(actual);
            }
            else if (factor < -1)
            {
                if (FactorBalance(actual.Right) > 0)
                    return actual = RotarIzqDer(actual);
                else                   
                   return actual = RotarDerDer(actual);
            }
            return actual;
        }

        public int ObtenerAltura(Nodo<T> node)
        {
            int height = 0;

            if (node != null)
            {
                int l = ObtenerAltura(node.Left);
                int r = ObtenerAltura(node.Right);
                int m = Math.Max(l, r);
                height = m + 1;
            }

            return height;
        }

        private int FactorBalance(Nodo<T> node)
        {
            int l = ObtenerAltura(node.Left);
            int r = ObtenerAltura(node.Right);
            int factor = l - r;
            return factor;
        }

        private Nodo<T> RotarIzqIzq(Nodo<T> node)
        {
            Nodo<T> pivote = node.Left;
            node.Left = pivote.Right;
            pivote.Right = node;
            return pivote;
        }

        private Nodo<T> RotarDerDer(Nodo<T> node)
        {
            Nodo<T> pivote = node.Right;
            node.Right = pivote.Left;
            pivote.Left = node;
            return pivote;
        }

        private Nodo<T> RotarIzqDer(Nodo<T> node)
        {
            Nodo<T> pivote = node.Right;
            node.Right = RotarIzqIzq(pivote);
            return RotarDerDer(node);
        }

        private Nodo<T> RotarDerIzq(Nodo<T> node)
        {
            Nodo<T> pivote = node.Left;
            node.Left = RotarDerDer(pivote);
            return RotarIzqIzq(node);
        }
    }
}
