namespace IncrementalSheep;

public class CircularBuffer<T>
{
    private readonly T[] _buffer;
    private int bufferIndex;

    public CircularBuffer(int size)
    {
        _buffer = new T[size];
        bufferIndex = 0;
    }

    public void Add(T item)
    {
        --bufferIndex;
        if (bufferIndex < 0)
        {
            bufferIndex = _buffer.Length - 1;
        }
        _buffer[bufferIndex] = item;
    }

    public T CurrentItem()
        => _buffer[bufferIndex];

    public IEnumerable<T> AllItems()
    {
        for (var i = 0; i < _buffer.Length; i++)
        {
            yield return _buffer[(i + bufferIndex) % _buffer.Length];
        }
    }
}
