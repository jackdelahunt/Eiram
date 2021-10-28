namespace Eiram
{
    public static class Handles
    {
        public static readonly EmptySome None = new EmptySome();
    }
    
    public struct Some<T>
    {
        public T Value;
        private bool isNone;
        
        public Some(T value)
        {
            this.Value = value;
            this.isNone = false;
        }
        
        private Some(bool isNone)
        {
            this.isNone = isNone;
            this.Value = default(T);
        }
        
        public T Unwrap()
        {
            if (isNone) throw new UnwrappedNoneException();

            return Value;
        }
        
        public bool IsSome(out T value)
        {
            value = this.Value;
            return !isNone;
        }

        public bool IsNone()
        {
            return isNone;
        }
        
        public static implicit operator Some<T>(T value)
        {
            return new Some<T>(value);
        }

        public static implicit operator Some<T>(EmptySome emptySome)
        {
            return new Some<T>(true);
        }
    }

    public struct EmptySome
    {
    }
}