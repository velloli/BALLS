using System;
using System.Collections.Generic;

public class CircularBuffer<T>
{
    public Queue<T> buffer;
    public int capacity;
    T lastElement;

    public CircularBuffer(int capacity)
    {
        this.capacity = capacity;
        buffer = new Queue<T>(capacity);
    }

    public void Push(T item)
    {
        if (buffer.Count == capacity)
        {
            buffer.Dequeue(); // Remove oldest data
        }
        buffer.Enqueue(item);
        lastElement = item;
    }

    public List<T> GetItems()
    {
        return new List<T>(buffer);
    }

    public T GetElementAt(int index)
    {
        if (index >= 0 && index < buffer.Count)
        {
            T[] array = buffer.ToArray();
            return array[index];
        }
        throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range.");
    }

    public T GetLastElement()
    {
        return lastElement;
    }
}