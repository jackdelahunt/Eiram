namespace Eiram
{
    public static class Handles
    {
        public static Option<T> Some<T>(T value)
        {
            return new Option<T>(value);
        }

        public static Option<T> None<T>()
        {
            return new Option<T>
            {
                IsNone = true,
                Value = default(T),
            };
        }
    }
    
    public struct Option<T>
    {
        public T Value;
        public bool IsNone { get;  set; }

        public Option(T value)
        {
            this.Value = value;
            this.IsNone = false;
        }
        
        public T Unwrap()
        {
            if (IsNone) throw new UnwrappedNoneException();

            return Value;
        }
        
        public bool IsSome(out T value)
        {
            value = this.Value;
            return !IsNone;
        }
        
        public static implicit operator Option<T>(T value)
        {
            return new Option<T>(value);
        }
    }
}