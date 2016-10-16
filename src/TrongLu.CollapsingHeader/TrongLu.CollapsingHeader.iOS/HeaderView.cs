using System;
using TrongLu.CollapsingHeader.Interfaces;
using UIKit;

namespace TrongLu.CollapsingHeader
{
    public class HeaderView : UIView, ICollapsable, IExpandable
    {
        protected virtual float MinHeight
        {
            get
            {
                return 50;
            }
        }

        protected virtual float MaxHeight
        {
            get
            {
                return 200;
            }
        }

        public virtual bool Collapsable
        {
            get
            {
                return Frame.Height > MinHeight;
            }
        }

        public virtual bool Expandable
        {
            get
            {
                return Frame.Height < MaxHeight;
            }
        }

        public HeaderView()
        {
        }

        public HeaderView(IntPtr handle)
            : base(handle)
        {
        }
    }
}
