namespace Eiram
{
    public struct Some<T>
    {
        public T Value;
        public bool isNone;
        
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

        public static implicit operator Some<T>(None none)
        {
            return new Some<T>(true);
        }
    }

    public struct None
    {
        
    }
}