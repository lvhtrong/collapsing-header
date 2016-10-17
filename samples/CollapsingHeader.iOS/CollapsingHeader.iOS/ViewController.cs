using System;

using UIKit;

namespace CollapsingHeader
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var collapsingViewSource = new CollapsingViewSource(collapsingHeader, collapsingContent);
            collaspingHeaderView.Source = collapsingViewSource;
            collaspingHeaderView.OnlyExpandOnTop = true;
        }
    }
}
