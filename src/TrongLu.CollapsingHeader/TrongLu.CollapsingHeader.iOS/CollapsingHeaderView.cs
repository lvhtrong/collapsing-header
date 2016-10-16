using System;
using Foundation;
using UIKit;

namespace TrongLu.CollapsingHeader
{
    [Register("CollapsingHeaderView")]
    public class CollapsingHeaderView : UIView
    {
        private nfloat _lastDraggingOffset;
        private CollapsingHeaderViewDelegate _delegate;
        private NSLayoutConstraint _contentHeightConstraint;

        private HeaderView _headerView;

        public HeaderView HeaderView
        {
            get { return _headerView; }
            set
            {
                if (_headerView != null && _headerView.Superview != null)
                {
                    _headerView.RemoveFromSuperview();
                    _headerView.Dispose();
                    _headerView = null;
                }
                _headerView = value;
            }
        }

        private UIScrollView _contentView;

        public UIScrollView ContentView
        {
            get { return _contentView; }
            set
            {
                if (_contentView != null && _contentView.Superview != null)
                {
                    _contentView.RemoveFromSuperview();
                    _contentView.Dispose();
                    _contentView = null;
                }
                _contentView = value;
            }
        }

        public CollapsingHeaderViewDelegate Delegate
        {
            get { return _delegate ?? (_delegate = new CollapsingHeaderViewDelegate()); }
            set { _delegate = value; }
        }

        public CollapsingHeaderView()
        {
            Initialize();
        }

        public CollapsingHeaderView(IntPtr handle)
            : base(handle)
        {
            Initialize();
        }

        private void Initialize()
        {
        }

        public void Notify()
        {
            AddHeaderView();
            AddContentView();

            InitEvents();
        }

        private void InitEvents()
        {
            ContentView.DraggingStarted += (sender, e) =>
            {
                _lastDraggingOffset = ContentView.ContentOffset.Y;
            };

            ContentView.Scrolled += (sender, e) =>
            {
                var contentOffset = ContentView.ContentOffset;
                var distance = contentOffset.Y - _lastDraggingOffset;

                if (HeaderView.Collapsable)
                {
                    if (distance > 0)
                    {
                        _contentHeightConstraint.Constant += distance;
                        contentOffset.Y = _lastDraggingOffset;
                    }
                    //else
                    //{
                    //    _contentHeightConstraint.Constant += contentOffset.Y;
                    //    contentOffset.Y = 0;
                    //}
                    ContentView.ContentOffset = contentOffset;
                }
                if (HeaderView.Expandable)
                {
                    if (distance < 0)
                    {
                        _contentHeightConstraint.Constant += distance;
                        contentOffset.Y = _lastDraggingOffset;
                    }
                    //else
                    //{
                    //    _contentHeightConstraint.Constant += contentOffset.Y;
                    //    contentOffset.Y = 0;
                    //}
                    ContentView.ContentOffset = contentOffset;
                }
            };
        }

        private void AddHeaderView()
        {
            if (HeaderView.Superview != null)
            {
                HeaderView.RemoveFromSuperview();
            }
            HeaderView.TranslatesAutoresizingMaskIntoConstraints = false;

            AddSubview(HeaderView);
            AddConstraints(new NSLayoutConstraint[]
            {
                NSLayoutConstraint.Create(HeaderView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, 0),
                NSLayoutConstraint.Create(HeaderView, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, this, NSLayoutAttribute.Leading, 1, 0),
                NSLayoutConstraint.Create(HeaderView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, this, NSLayoutAttribute.Right, 1, 0)
            });
        }

        private void AddContentView()
        {
            ContentView.TranslatesAutoresizingMaskIntoConstraints = false;

            AddSubview(ContentView);
            _contentHeightConstraint = NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Height, NSLayoutRelation.Equal, 1, 300);
            AddConstraints(new NSLayoutConstraint[]
            {
                _contentHeightConstraint,
                NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Bottom, 1, 0),
                NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Leading, 1, 0),
                NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Right, 1, 0),
                NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1, 0)
            });
        }
    }
}
