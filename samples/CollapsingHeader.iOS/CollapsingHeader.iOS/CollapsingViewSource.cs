using System;
using TrongLu.CollapsingHeader;
using UIKit;

namespace CollapsingHeader
{
    public class CollapsingViewSource : CollapsingHeaderViewSource
    {
        private HeaderView _headerView;
        private UIScrollView _scrollView;

        public override HeaderView HeaderView
        {
            get { return _headerView; }
        }

        public override UIView ResizableView
        {
            get { return _scrollView; }
        }

        public override UIScrollView ScrollableView
        {
            get { return _scrollView; }
        }

        public CollapsingViewSource(HeaderView headerView, UIScrollView scrollView)
        {
            _headerView = headerView;
            _scrollView = scrollView;
        }
    }
}
