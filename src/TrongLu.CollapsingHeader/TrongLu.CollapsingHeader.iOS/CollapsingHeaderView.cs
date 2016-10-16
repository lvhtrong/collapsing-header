using System;
using Foundation;
using UIKit;

namespace TrongLu.CollapsingHeader
{
    [Register("CollapsingHeaderView")]
    public class CollapsingHeaderView : UIView
    {
        private bool _isFirstTime = true;
        private nfloat _lastDraggingOffset;
        private CollapsingHeaderViewDelegate _delegate;
        private NSLayoutConstraint _contentTopConstraint;

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

        public bool OnlyExpandOnTop { get; set; } = false;

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
                if (_isFirstTime)
                {
                    _isFirstTime = false;
                    HeaderView.SetMaxHeight((int)HeaderView.Frame.Height);
                    ResetContentViewTopConstraint();
                }

                _lastDraggingOffset = (nfloat)Math.Max(ContentView.ContentOffset.Y, 0f);
            };
            ContentView.Scrolled += (sender, e) => OnContentViewScrolled();
        }

        private void ResetContentViewTopConstraint()
        {
            var top = (nfloat)Math.Max(ContentView.Frame.Y, HeaderView.MaxHeight);

            if (_contentTopConstraint != null)
            {
                RemoveConstraint(_contentTopConstraint);
            }

            _contentTopConstraint = NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, top);
            AddConstraint(_contentTopConstraint);
        }

        private void OnContentViewScrolled()
        {
            var contentOffset = ContentView.ContentOffset;
            var distance = contentOffset.Y - _lastDraggingOffset;

            if (HeaderView.Collapsible)
            {
                if (distance > 0)
                {
                    if (_contentTopConstraint.Constant - distance < HeaderView.MinHeight)
                    {
                        _contentTopConstraint.Constant = HeaderView.MinHeight;
                    }
                    else
                    {
                        _contentTopConstraint.Constant -= distance;
                    }
                    contentOffset.Y = _lastDraggingOffset;
                    ContentView.ContentOffset = contentOffset;
                }
            }
            if (HeaderView.Expandable)
            {
                if (!OnlyExpandOnTop)
                {
                    if (distance < 0)
                    {
                        _contentTopConstraint.Constant -= distance;
                        contentOffset.Y = _lastDraggingOffset;
                    }
                }
                else
                {
                    if (contentOffset.Y < 0)
                    {
                        _contentTopConstraint.Constant -= contentOffset.Y;
                        contentOffset.Y = 0;
                    }
                }
                ContentView.ContentOffset = contentOffset;
            }
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
            _contentTopConstraint = NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Bottom, 1, 0);
            AddConstraints(new NSLayoutConstraint[]
            {
                _contentTopConstraint,
                NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Bottom, 1, 0),
                NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Leading, 1, 0),
                NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Right, 1, 0),
                NSLayoutConstraint.Create(ContentView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1, 0)
            });
        }

        protected override void Dispose(bool disposing)
        {
            if (HeaderView != null)
            {
                HeaderView.Dispose();
                HeaderView = null;
            }

            if (ContentView != null)
            {
                ContentView.Dispose();
                ContentView = null;
            }

            base.Dispose(disposing);
        }
    }
}
