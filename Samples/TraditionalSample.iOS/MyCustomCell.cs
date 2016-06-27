using Foundation;
using System;
using UIKit;

namespace TraditionalSample.iOS
{
    public partial class MyCustomCell : UITableViewCell
    {
        public MyCustomCell (IntPtr handle) : base (handle)
        {
        }

        public MyCustomCell(NSString cellId) : base(UITableViewCellStyle.Default, cellId) { }

        Models.ToDo todo;
        public Models.ToDo Todo
        {
            get
            {
                return todo;
            }
            set
            {
                todo = value;
                text.Text = todo.Text;
                completed.SetState(todo.Completed, true);
            }
        }
        
    }
}