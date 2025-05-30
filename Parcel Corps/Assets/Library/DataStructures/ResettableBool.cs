public class ResettableBool
{
    private bool _value;

    public ResettableBool(bool initialValue = false)
    {
        _value = initialValue;
    }

    /// <summary>
    /// Returns true if set, then resets the value to false.
    /// </summary>
    public bool TryConsume()
    {
        if (_value)
        {
            _value = false;
            return true;
        }

        return false;
    }

    /// <summary>
    /// Set the flag to true.
    /// </summary>
    public void SetTrue()
    {
        _value = true;
    }

    /// <summary>
    /// Manually reset the flag to false.
    /// </summary>
    public void Reset()
    {
        _value = false;
    }

    /// <summary>
    /// Returns the current value without clearing it.
    /// </summary>
    public bool Peek => _value;
}
