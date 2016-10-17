using System;
using UIKit;

namespace TrongLu.CollapsingHeader
{
    public abstract class CollapsingHeaderViewSource
    {
        public abstract HeaderView HeaderView { get; }

        public abstract UIView ResizableView { get; }

        public abstract UIScrollView ScrollableView { get; }

        public virtual void HeaderHeightChanged(nfloat height) { }
    }
}
