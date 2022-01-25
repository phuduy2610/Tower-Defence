using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticList<T> where T : class
{
    private int count;
    private int size;
    private T[] data;
    private int currIndex;

    public int Count => count;

    public StaticList(int size)
    {
        this.size = size;
        currIndex = 0;
        data = new T[size];
    }

    public int Add(T element)
    {
        int toAdd = currIndex;
        if (count < size)
        {
            data[toAdd] = element;
            count += 1;
            currIndex = SearchEmpty();
        } else
        {
            toAdd = currIndex = -1;
        }
        return toAdd;
    }

    public bool IsFull()
    {
        if (count == size)
        {
            return true;
        }
        return false;
    }

    private int SearchEmpty()
    {
        for (var i = 0; i < size; i++)
        {
            if (data[i] == null)
            {
                return i;
            }
        }
        return -1;
    }

    public bool Contains(T element)
    {
        foreach (T item in data)
        {
            if(element.Equals(item))    
                return true;
        }
        return false;
    }

    public bool SetAt(T element, int index)
    {
        ThrowIfIndexOutOfRange(index);
        if (index < size)
        {
            if (data[index] == null)
            {
                count += 1;
            }
            data[index] = element;
            currIndex = SearchEmpty();
            return true;
        }
        return false;
    }

    public bool RemoveAt(int index)
    {
        ThrowIfIndexOutOfRange(index);
        if (index < size)
        {
            data[index] = null;
            count -= 1;
            if (index < currIndex || currIndex == -1)
            {
                currIndex = index;
            }
            return true;
        }
        return false;
    }

    public bool Remove(T element)
    {
        for (var i = 0; i < size; i++)
        {
            if (element.Equals(data[i]))
            {
                data[i] = null;
                count -= 1;
                if (i < currIndex || currIndex == -1)
                {
                    currIndex = i; 
                }
                return true;
            }
        }
        return false;
    }

    public T this[int i]
    {
        get {
            ThrowIfIndexOutOfRange(i);
            return data[i]; 
        }
        set {
            SetAt(value, i);
        }
    }

    private void ThrowIfIndexOutOfRange(int index)
    {
        if (index > size - 1 || index < 0)
        {
            throw new ArgumentOutOfRangeException(string.Format("The current size of the array is {0}", size));
        }
    }
}
