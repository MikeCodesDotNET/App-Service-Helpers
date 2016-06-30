using System;
using AppServiceHelpers.Models;

namespace TraditionalSample.iOS.Models
{
    public class ToDo : EntityData
    {
        public string Text { get; set;}
        public bool Completed { get; set;}
    }
}

