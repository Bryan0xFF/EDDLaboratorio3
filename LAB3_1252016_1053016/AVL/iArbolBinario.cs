using System;
using System.Collections.Generic;
using System.Text;

namespace AVL
{
    public interface iArbolBinario<T>
    {
        Nodo<T> Insert(T value);
        Nodo<T> Insert(Nodo<T> node, T value);
        void Delete(T value);
        void Delete(Nodo<T> node, T value);
        List<Nodo<T>> PreOrden(Nodo<T> node);
        List<Nodo<T>> InOrden(Nodo<T> node);
        List<Nodo<T>> PostOrden(Nodo<T> node);
    }
}
