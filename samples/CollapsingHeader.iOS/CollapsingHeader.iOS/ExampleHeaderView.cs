using System;
using Foundation;
using ObjCRuntime;
using TrongLu.CollapsingHeader;
using UIKit;

namespace CollapsingHeader
{
    [Register("ExampleHeaderView")]
    public class ExampleHeaderView : HeaderView
    {
        public override int MinHeight
        {
            get
            {
                return 50;
            }
        }

        public ExampleHeaderView()
        {
            Initializer();
        }

        public ExampleHeaderView(IntPtr handle)
            : base(handle)
        {
            Initializer();
        }

        private void Initializer()
        {
            LoadNib();
            InitViews();
        }

        private void LoadNib()
        {
            var arr = NSBundle.MainBundle.LoadNib("ExampleHeaderView", this, null);
            var v = Runtime.GetNSObject(arr.ValueAt(0)) as UIView;
            v.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(v);
            AddConstraints(new NSLayoutConstraint[] {
                NSLayoutConstraint.Create(v, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, 0),
                NSLayoutConstraint.Create(v, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1, 0),
                NSLayoutConstraint.Create(v, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, this, NSLayoutAttribute.Leading, 1, 0),
                NSLayoutConstraint.Create(v, NSLayoutAttribute.Trailing, NSLayoutRelation.Equal, this, NSLayoutAttribute.Trailing, 1, 0),
            });
        }

        private void InitViews()
        {
            AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 100));
        }
    }
}
