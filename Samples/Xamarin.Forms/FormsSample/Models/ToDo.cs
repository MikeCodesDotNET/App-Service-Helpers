using System;
using AppServiceHelpers.Data.Models;

namespace FormsSample.Models
{
    public class ToDo : EntityData
    {
        public string Text { get; set;}
        public bool Completed { get; set;}
    }
}

