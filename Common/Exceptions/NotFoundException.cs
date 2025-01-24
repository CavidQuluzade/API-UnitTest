using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public List<string> Errors { get; set; } = new List<string>();
        public NotFoundException(string error) 
        {
            Errors.Add(error);
        }
    }
}
