using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Soundpad.Models
{
    public class ResultModel<T>
    {
        public ResultModel(bool success)
        {
            Success = success;
        }
        public bool Success { get; }
        public int StatusCode { get; }
        public string Message { get; set; }
        public T Content { get; set; }
    }
}
