using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public List<string> Errors { get; set; } = new List<string>();
        public UnauthorizedException(string error) 
        {
            Errors.Add(error);
        }
    }
}
