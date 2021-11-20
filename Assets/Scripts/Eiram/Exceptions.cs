using System;

namespace Eiram
{
    class InvalidTypeException : Exception
    {
        public InvalidTypeException(string key, string valueType, string keyType)
            : base($"The key '{key}' is of type {valueType} but you requested type {keyType}")
        { }
    }

    class NotFoundException : Exception
    {
        public NotFoundException(string key)
            : base($"The tag {key} does not exist")
        { }
    }
    
    class UnwrappedNoneException : Exception
    {
        public UnwrappedNoneException()
            : base($"Tried to unwrap a None")
        { }
        
        public UnwrappedNoneException(string message)
            : base(message)
        { }
    }
}