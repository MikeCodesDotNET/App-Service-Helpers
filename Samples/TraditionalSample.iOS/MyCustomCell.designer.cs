// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TraditionalSample.iOS
{
    [Register ("MyCustomCell")]
    partial class MyCustomCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch completed { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel text { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (completed != null) {
                completed.Dispose ();
                completed = null;
            }

            if (text != null) {
                text.Dispose ();
                text = null;
            }
        }
    }
}