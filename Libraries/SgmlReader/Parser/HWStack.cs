using System.Diagnostics.CodeAnalysis;

namespace Sgml;

/// <summary>
/// This stack maintains a high water mark for allocated objects so the client
/// can reuse the objects in the stack to reduce memory allocations, this is
/// used to maintain current state of the parser for element stack, and attributes
/// in each element.
/// </summary>
internal class HWStack
{
    private object[] m_items;
    private int m_size;
    private int m_count;
    private int m_growth;

    /// <summary>
    /// Initialises a new instance of the HWStack class.
    /// </summary>
    /// <param name="growth">The amount to grow the stack space by, if more space is needed on the stack.</param>
    public HWStack(int growth)
    {
        m_growth = growth;
    }

    /// <summary>
    /// The number of items currently in the stack.
    /// </summary>
    public int Count
    {
        get
        {
            return m_count;
        }
        set
        {
            m_count = value;
        }
    }

    /// <summary>
    /// The size (capacity) of the stack.
    /// </summary>
    [SuppressMessage("Microsoft.Performance", "CA1811", Justification = "Kept for potential future usage.")]
    public int Size
    {
        get
        {
            return m_size;
        }
    }

    /// <summary>
    /// Returns the item at the requested index or null if index is out of bounds
    /// </summary>
    /// <param name="i">The index of the item to retrieve.</param>
    /// <returns>The item at the requested index or null if index is out of bounds.</returns>
    public object this[int i]
    {
        get
        {
            return i >= 0 && i < m_size ? m_items[i] : null;
        }
        set
        {
            m_items[i] = value;
        }
    }

    /// <summary>
    /// Removes and returns the item at the top of the stack
    /// </summary>
    /// <returns>The item at the top of the stack.</returns>
    public object Pop()
    {
        m_count--;
        if (m_count > 0)
        {
            return m_items[m_count - 1];
        }

        return null;
    }

    /// <summary>
    /// Pushes a new slot at the top of the stack.
    /// </summary>
    /// <returns>The object at the top of the stack.</returns>
    /// <remarks>
    /// This method tries to reuse a slot, if it returns null then
    /// the user has to call the other Push method.
    /// </remarks>
    public object Push()
    {
        if (m_count == m_size)
        {
            int newsize = m_size + m_growth;
            object[] newarray = new object[newsize];
            if (m_items != null)
                Array.Copy(m_items, newarray, m_size);

            m_size = newsize;
            m_items = newarray;
        }
        return m_items[m_count++];
    }

    /// <summary>
    /// Remove a specific item from the stack.
    /// </summary>
    /// <param name="i">The index of the item to remove.</param>
    [SuppressMessage("Microsoft.Performance", "CA1811", Justification = "Kept for potential future usage.")]
    public void RemoveAt(int i)
    {
        m_items[i] = null;
        Array.Copy(m_items, i + 1, m_items, i, m_count - i - 1);
        m_count--;
    }
}