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

            collaspingHeaderView.HeaderView = collapsingHeader;
            collaspingHeaderView.ContentView = collapsingContent;
            collaspingHeaderView.Notify();
        }
    }
}
