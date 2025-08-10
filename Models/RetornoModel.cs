using System;
using System.Collections.Generic;

namespace to_do_michelin.Models
{
    public class RetornoModel
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> Errors { get; set; } = new();
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }

    public class RetornoModel<T> : RetornoModel
    {
        public T? Data { get; set; }
    }
}
