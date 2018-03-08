using System;
using System.Collections.Generic;
using System.Text;

namespace AVL
{
    public interface iArbolBinario<T>
    {
        void Insert(T value);
        Nodo<T> Insert(T value, Nodo<T> node);
        void Delete(T value);
        void Delete(T value, Nodo<T> node);
        List<Nodo<T>> PreOrden(Nodo<T> node);
        List<Nodo<T>> InOrden(Nodo<T> node);
        List<Nodo<T>> PostOrden(Nodo<T> node);
    }
}
