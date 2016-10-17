using System;
using TrongLu.CollapsingHeader.Interfaces;
using UIKit;

namespace TrongLu.CollapsingHeader
{
    public abstract class HeaderView : UIView, ICollapsible, IExpandable
    {
        public virtual int MinHeight
        {
            get
            {
                return 0;
            }
        }

        private int _maxHeight = -1;

        public virtual int MaxHeight
        {
            get
            {
                return _maxHeight;
            }
        }

        public virtual bool Collapsible
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

        protected HeaderView()
        {
            Initialize();
        }

        protected HeaderView(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        private void Initialize()
        {
            AddConstraint(NSLayoutConstraint.Create(this, NSLayoutAttribute.Height, NSLayoutRelation.GreaterThanOrEqual, 1, MinHeight));
        }

        internal void SetMaxHeight(int value)
        {
            _maxHeight = value;
        }
    }
}
