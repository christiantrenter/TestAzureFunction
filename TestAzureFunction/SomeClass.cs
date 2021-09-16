using System;

namespace TestAzureFunction
{
    
    public class SomeClass
    {
        private int _number = 0;
        public int getNumber()
        {
            _number++;
            return _number;
        }
    }
}