using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sieci_Neuronowe
{
    /// <summary>
    /// A generic Matrix exception.
    /// </summary>
    public class MatrixException : Exception
    {
        public MatrixException(string message)
            : base(message)
        {
        }

    }
}
