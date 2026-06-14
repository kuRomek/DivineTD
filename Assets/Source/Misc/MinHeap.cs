using System;
using System.Collections;
using System.Collections.Generic;

public class MinHeap<T> : IEnumerable<T> where T : IComparable<T>
{
    private readonly List<T> _data = new();

    public int Count => _data.Count;

    public T Peek()
        => _data[0];

    public void Push(T item)
    {
        _data.Add(item);
        HeapifyUp(_data.Count - 1);
    }

    public T Pop()
    {
        T root = _data[0];
        _data[0] = _data[^1];
        _data.RemoveAt(_data.Count - 1);
        HeapifyDown(0);
        return root;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;

            if (_data[index].CompareTo(_data[parent]) >= 0)
                break;

            (_data[index], _data[parent]) = (_data[parent], _data[index]);

            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;

            int smallest = index;

            if (leftChild < _data.Count && _data[leftChild].CompareTo(_data[smallest]) < 0)
                smallest = leftChild;

            if (rightChild < _data.Count && _data[rightChild].CompareTo(_data[smallest]) < 0)
                smallest = rightChild;

            if (smallest == index)
                break;

            (_data[index], _data[smallest]) = (_data[smallest], _data[index]);

            index = smallest;
        }
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        return _data.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _data.GetEnumerator();
    }
}