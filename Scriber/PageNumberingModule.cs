using System;
using System.Collections.Generic;
using System.Text;

namespace Scriber
{
    public class PageNumberingModule
    {
        public int Current { get; private set; } = 1;

        public void Set(int number)
        {
            Current = number;
        }

        public string Next()
        {
            return Current++.ToString();
        }
    }
}
