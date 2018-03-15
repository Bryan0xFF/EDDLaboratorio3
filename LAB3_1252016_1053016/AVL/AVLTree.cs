using System;
using System.Collections.Generic;
using System.Text;

namespace AVL
{
   public class AVLTree<T> : iArbolBinario<T> where T : IComparable<T>
    {
        public Nodo<T> cabeza;
        private List<Nodo<T>> inOrder = new List<Nodo<T>>();
        private List<Nodo<T>> preOrden = new List<Nodo<T>>();
        private List<Nodo<T>> postOrden = new List<Nodo<T>>();
        private List<string> logs = new List<string>();
        private List<string> logsDelete = new List<string>();

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

        public List<Nodo<T>> InOrden()
        {
            return InOrden(cabeza);
        }

        public List<Nodo<T>> InOrden(Nodo<T> node)
        {
            if (node != null)
            {
                InOrden(node.Left);
                inOrder.Add(node);
                InOrden(node.Right);
            }
            return inOrder;
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
            cabeza = Insert(cabeza, value);
            cabeza.Parent = null;
            return cabeza;
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

        public Nodo<T> Delete(T value, Nodo<T> node)
        {
            Nodo<T> Min;
            Nodo<T> Replace; 

            if (cabeza.Left == null && cabeza.Right == null)
            {
                cabeza = null;
                return cabeza;
            }
            else

            if (node.Value.CompareTo(cabeza.Value) == 0 && node.Parent == null)
            {
                if (cabeza.Right != null)
                {
                    Replace = FindMin(cabeza.Right);
                    cabeza.Value = Replace.Value;
                    cabeza.Parent = null;
                    cabeza.Right = Delete(Replace.Value, cabeza.Right);
                }

                else if (cabeza.Left != null)
                {
                    Replace = FinMinLeft(cabeza.Left);
                    cabeza.Value = Replace.Value;
                    cabeza.Parent = null;
                    cabeza.Left = Delete(Replace.Value, cabeza.Left);
                }
                else
                {
                    cabeza = null;
                }
                                       
            }          
            else
            {
                if (value.CompareTo(node.Value) == -1)
                {                    
                    node.Left = Delete(value, node.Left);

                    if (FactorBalance(node) == -2)
                    {
                        if (FactorBalance(node.Right) <= 0)
                        {
                            logsDelete.Add("[Delete] Se ha hecho una rotación Derecha Derecha en el nodo " + node.Value);
                            node = RotarDerDer(node);
                        }

                        else
                        {
                            logsDelete.Add("[Delete] Se ha hecho una rotación Derecha Izquierda en el nodo " + node.Value);
                            node = RotarDerIzq(node);
                        }

                    }
                }

                else if (value.CompareTo(node.Value) == 1)
                {
                    node.Right = Delete(value, node.Right);

                    if (FactorBalance(node) == 2)
                    {
                        if (FactorBalance(node.Left) >= 1)
                        {
                            logsDelete.Add("[Delete] Se ha hecho una rotación Izquierda Izquierda en el nodo " + node.Value);
                            node = RotarIzqIzq(node);

                        }
                        else
                        {
                            logsDelete.Add("[Delete] Se ha hecho una rotación Izquierda Derecha en el nodo " + node.Value);
                            node = RotarIzqDer(node);
                        }

                    }
                }

                else
                {
                   
                    if (node.Right != null)
                    {
                        Min = node.Right;

                        while (Min.Left != null)
                        {
                            Min = Min.Left;
                        }
                        node.Value = Min.Value;
                        node.Right = Delete(Min.Value, node.Right);

                        if (FactorBalance(node) == 2)
                        {
                            if (FactorBalance(node.Left) >= 0)
                            {
                                logsDelete.Add("[Delete] Se ha hecho una rotación Izquierda Izquierda en el nodo " + node.Value);
                                node = RotarIzqIzq(node);
                            }

                            else
                            {
                                logsDelete.Add("[Delete] Se ha hecho una rotación Izquierda Derecha en el nodo " + node.Value);
                                node = RotarIzqDer(node);
                            }

                        }
                    }
                    if (node.Left != null)
                    {
                        Min = node.Left;

                        while (Min.Right != null)
                        {
                            Min = Min.Right;
                        }
                        node.Value = Min.Value;
                        node.Left = Delete(Min.Value, node.Left);

                        if (FactorBalance(node) == -2)
                        {
                            if (FactorBalance(node.Left) <= 0)
                            {
                                logsDelete.Add("[Delete] Se ha hecho una rotación Derecha Derecha en el nodo " + node.Value);
                                node = RotarDerDer(node);
                            }

                            else
                            {
                                logsDelete.Add("[Delete] Se ha hecho una rotación Derecha Izquierda en el nodo " + node.Value);
                                node = RotarDerIzq(node);
                            }
                        }
                    }
                    //if (cabeza.Value.CompareTo(value) == 0)
                    //{
                    //    if (cabeza.Right == null)
                    //    {
                    //        Replace = FinMinLeft(node.Right);
                    //        cabeza.Value = Replace.Value;
                    //        cabeza.Parent = null;
                    //        Delete(cabeza.Value, node.Right);
                    //    }
                    //    else if (cabeza.Left == null)
                    //    {
                    //        Replace = FindMin(node.Left);
                    //        cabeza.Value = Replace.Value;
                    //        cabeza.Parent = null;
                    //        Delete(cabeza.Value, node.Left);
                    //    }
                    //    else
                    //    {
                    //        return null;
                    //    }
                    //}
                    else
                    {
                        node = null;
                        return node;
                    }
                }
            }
            return node;
        }

        private Nodo<T> FinMinLeft(Nodo<T> root)
        {
            if (root == null)
            {
                return null;
            }

            else if (root.Right == null)
            {
                return root;

            }

            else
            {
                return FindMin(root.Right);
            }
        }

        #region Rotaciones
        public Nodo<T> Balancear(Nodo<T> actual)
        {
            int factor = FactorBalance(actual);

            if (factor > 1)
            {
                if (FactorBalance(actual.Left) < 0)
                {
                    logs.Add("[Insert] Se ha hecho una rotación Derecha Izquierda en el nodo " + actual.Value);
                    return actual = RotarDerIzq(actual);
                }
                else
                {
                    logs.Add("[Insert] Se ha hecho una rotación Izquierda Izquierda en el nodo " + actual.Value);
                    return actual = RotarIzqIzq(actual);
                }
            }
            else if (factor < -1)
            {
                if (FactorBalance(actual.Right) > 0)
                {
                    logs.Add("[Insert] Se ha hecho una rotación Izquierda Derecha en el nodo " + actual.Value);
                    return actual = RotarIzqDer(actual);
                }
                else
                {
                    logs.Add("[Insert] Se ha hecho una rotación Derecha Derecha en el nodo " + actual.Value);
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
            pivote.Parent = node.Parent;
            node.Parent = pivote;
            node.Right = null;
            return pivote;
        }

        private Nodo<T> RotarDerDer(Nodo<T> node)
        {
            Nodo<T> pivote = node.Right;
            node.Right = pivote.Left;
            pivote.Left = node;
            pivote.Parent = node.Parent;
            node.Parent = pivote;
            node.Left = null;
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

        public List<string> Rotaciones()
        {
            return logs;
        }
        #endregion

        #region Extra
        public List<string> RotacionesDelete()
        {
            return logsDelete;
        }

        public bool Limpiar()
        {
            try
            {
                inOrder.Clear();
                preOrden.Clear();
                postOrden.Clear();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion


    }
}
