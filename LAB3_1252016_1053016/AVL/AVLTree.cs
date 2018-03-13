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
        private List<string> logs = new List<string>();

        public Nodo<T> FindMin(Nodo<T> root)
        {
            if (root == null)
            {
                return null;
            }

            else if (root.Left == null)
            {
                return root;
            }

            else
            {
                return FindMin(root.Left);
            }
        }

        public void Delete(T value)
        {
            Delete(value);
        }

        public void Delete(T value, Nodo<T> node)
        {
            Nodo<T> Min;
            if (value == null)
            {
                throw new NullReferenceException();
            }

            if (cabeza.Value.CompareTo(value) == 0 && cabeza.Left == null && cabeza.Right == null)
            {
                cabeza = null;
            }

            else if (value.CompareTo(node.Value) == -1)
            {
                Delete(value, node.Left);
              //  node = Balancear(node);
            } //look in the left

            else if (value.CompareTo(node.Value) > 0)
            {
                Delete(value, node.Right);
               // node = Balancear(node);
            } //look in the right

            else
            { //found node to delete

                if (node.Left != null && node.Right != null) //two children
                {
                    Min = FindMin(node.Right);
                    node.Value = Min.Value;
                    Delete(node.Value, node.Right);
                  //  node = Balancear(node);
                }

                else
                { //one or zero child

                    if (node.Left == null)//The root node is to be deleted
                    {
                        if (node.Parent == null)
                        {
                            cabeza = node.Right;
                            cabeza.Parent = null;
                        }
                        else
                        {
                            if (node.Right != null)
                            {
                                node.Right.Parent = node.Parent;
                            }

                            if (node == node.Parent.Left)
                            {
                                node.Parent.Left = node.Right;
                            }

                            else
                            {
                                node.Parent.Right = node.Right;
                            }
                        }
                        node = Balancear(node);
                    }
                    else if (node.Right == null)
                    {
                        if (node.Parent == null)
                        {
                            cabeza = node.Left;
                            cabeza.Parent = null;
                        }
                        else
                        {

                            node.Left.Parent = node.Parent;

                            if (node == node.Parent.Left)
                            {
                                node.Parent.Left = node.Left;
                            }
                            else
                            {
                                node.Parent.Right = node.Left;
                            }
                        }
                        node = Balancear(node);
                    }
                }
            }
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
                if (FactorBalance(actual.Left) < 0)
                {
                    logs.Add("Se ha hecho una rotación Derecha Izquierda");
                    return actual = RotarDerIzq(actual);
                }
                else
                {
                    logs.Add("Se ha hecho una rotación Izquierda Izquierda");
                    return actual = RotarIzqIzq(actual);
                }
            }
            else if (factor < -1)
            {
                if (FactorBalance(actual.Right) > 0)
                {
                    logs.Add("Se ha hecho una rotación Izquierda Derecha");
                    return actual = RotarIzqDer(actual);
                }
                else
                {
                    logs.Add("Se ha hecho una rotación Derecha Derecha");
                    return actual = RotarDerDer(actual);
                }
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

        public List<string> rotaciones()
        {
            return logs; 
        }
    }
}
