using System;
using System.Collections.Generic;
using System.Text;

namespace AVL
{
    class AVLTree<T> : iArbolBinario<T> where T : IComparable<T>
    {
        public Nodo<T> cabeza;
        private List<Nodo<T>> inOrden = new List<Nodo<T>>();
        private List<Nodo<T>> preOrden = new List<Nodo<T>>();
        private List<Nodo<T>> postOrden = new List<Nodo<T>>();

        public void Delete(T value)
        {
            throw new NotImplementedException();
        }

        public void Delete(T value, Nodo<T> node)
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

        public void Insert(T value)
        {
            Insert(value, cabeza);
        }

        public Nodo<T> Insert(T value, Nodo<T> node)
        {
            if (cabeza.Value == null) // Primer nodo insertado
            {
                cabeza.Value = value;
                return cabeza;
            }

            if (cabeza.Value.CompareTo(default(T)) == 0) //Si es un dato numérico y se inserta el primer nodo
            {
                cabeza.Value = value;
                return cabeza;
            }

            else if (node == null) // se crea un nuevo nodo con el dato ingresado
            {
                node = new Nodo<T>();
                node.Value = value;
            }
            else
            {
                if (node.Value.CompareTo(value) > 0)
                {
                    node.Left = Insert(value, node.Left);
                    node.Left.Parent = node;
                    //  if (Math.Abs(FactorBalance(node.Left.Parent)) == 2)
                    Balancear(node.Parent.Left);

                    return node;
                }
                if (node.Value.CompareTo(value) < 0)
                {
                    node.Right = Insert(value, node.Right);
                    node.Right.Parent = node;
                    //   if (Math.Abs(FactorBalance(node.Right.Parent)) == 2)
                    Balancear(node.Parent.Right);
                    return node;
                }
                else
                {
                    throw new InvalidOperationException("Ya contiene una llave igual");
                }
            }
            return node;
        }

        public void Balancear(Nodo<T> actual)
        {
            int factor = FactorBalance(actual);

            if (factor > 1)
            {
                if (FactorBalance(actual.Left) > 0)
                    actual = RotarIzqIzq(actual);
                else
                    actual = RotarIzqDer(actual);
            }
            else if (factor < -1)
            {
                if (FactorBalance(actual.Right) > 0)
                    actual = RotarDerDer(actual);
                else
                    actual = RotarIzqDer(actual);
            }
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
