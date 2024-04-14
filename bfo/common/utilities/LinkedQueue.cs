using System.Collections.Generic;

namespace BFO;

public class LinkedQueue<T> : LinkedList<T>, ICache
{
	public void Enqueue(T value) => 
		AddFirst(value);
	
	public T Dequeue() 
	{
		T value = this.Last.Value;
		RemoveLast();
		return value;
	}
}