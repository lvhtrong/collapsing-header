using System;
using Foundation;
using UIKit;

namespace TrongLu.CollapsingHeader
{
    [Register("CollapsingHeaderView")]
    public class CollapsingHeaderView : UIView
    {
        private bool _isFirstTime = true;
        private bool _expandEnabled;
        private nfloat _lastDraggingOffset;
        private nfloat _startDraggingOffset;
        private NSLayoutConstraint _contentTopConstraint;

        internal HeaderView HeaderView
        {
            get { return Source.HeaderView; }
        }

        internal UIView ResizableView
        {
            get { return Source.ResizableView; }
        }

        internal UIScrollView ScrollableView
        {
            get { return Source.ScrollableView; }
        }

        private CollapsingHeaderViewSource _source;

        public CollapsingHeaderViewSource Source
        {
            get { return _source; }
            set
            {
                if (value == null)
                {
                    throw new Exception("Instance of CollapsingHeaderViewSource cannot be null!");
                }
                _source = value;
                Notify();
            }
        }

        public bool OnlyExpandOnTop { get; set; } = false;

        public uint ExpandOffset { get; set; } = 0;

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
            ScrollableView.DraggingStarted += (sender, e) =>
            {
                if (_isFirstTime)
                {
                    _isFirstTime = false;
                    HeaderView.SetMaxHeight((int)HeaderView.Frame.Height);
                    ResetContentViewTopConstraint();
                }

                _lastDraggingOffset = _startDraggingOffset = (nfloat)Math.Max(ScrollableView.ContentOffset.Y, 0f);
            };
            ScrollableView.Scrolled += (sender, e) => OnContentViewScrolled();
        }

        private void ResetContentViewTopConstraint()
        {
            var top = (nfloat)Math.Max(ScrollableView.Frame.Y, HeaderView.MaxHeight);

            if (_contentTopConstraint != null)
            {
                RemoveConstraint(_contentTopConstraint);
            }

            _contentTopConstraint = NSLayoutConstraint.Create(ResizableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, this, NSLayoutAttribute.Top, 1, top);
            AddConstraint(_contentTopConstraint);
        }

        private void OnContentViewScrolled()
        {
            var contentOffset = ScrollableView.ContentOffset;
            var distance = contentOffset.Y - _lastDraggingOffset;
            var distanceFromStart = contentOffset.Y - _startDraggingOffset;

            if (distance > 0)
            {
                if (HeaderView.Collapsible && _lastDraggingOffset > 0)
                {
                    if (_contentTopConstraint.Constant - distance < HeaderView.MinHeight)
                    {
                        _contentTopConstraint.Constant = HeaderView.MinHeight;
                    }
                    else
                    {
                        _contentTopConstraint.Constant -= distance;
                    }
                    Source.HeaderHeightChanged(_contentTopConstraint.Constant);
                    contentOffset.Y = _startDraggingOffset;
                    ScrollableView.ContentOffset = contentOffset;
                    _expandEnabled = false;
                }
            }
            else if (HeaderView.Expandable)
            {
                if (!OnlyExpandOnTop)
                {
                    if (distanceFromStart < -ExpandOffset)
                    {
                        _expandEnabled = true;
                    }

                    if (distance < 0 && _expandEnabled)
                    {
                        _contentTopConstraint.Constant -= distance;
                        contentOffset.Y = _lastDraggingOffset;
                        Source.HeaderHeightChanged(_contentTopConstraint.Constant);
                    }
                }
                else
                {
                    if (contentOffset.Y < 0)
                    {
                        _contentTopConstraint.Constant -= contentOffset.Y;
                        contentOffset.Y = 0;
                        Source.HeaderHeightChanged(_contentTopConstraint.Constant);
                    }
                }
                ScrollableView.ContentOffset = contentOffset;
            }
            _lastDraggingOffset = contentOffset.Y;
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
            ResizableView.TranslatesAutoresizingMaskIntoConstraints = false;

            AddSubview(ResizableView);
            _contentTopConstraint = NSLayoutConstraint.Create(ResizableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Bottom, 1, 0);
            AddConstraints(new NSLayoutConstraint[]
            {
                _contentTopConstraint,
                NSLayoutConstraint.Create(ResizableView, NSLayoutAttribute.Top, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Bottom, 1, 0),
                NSLayoutConstraint.Create(ResizableView, NSLayoutAttribute.Leading, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Leading, 1, 0),
                NSLayoutConstraint.Create(ResizableView, NSLayoutAttribute.Right, NSLayoutRelation.Equal, HeaderView, NSLayoutAttribute.Right, 1, 0),
                NSLayoutConstraint.Create(ResizableView, NSLayoutAttribute.Bottom, NSLayoutRelation.Equal, this, NSLayoutAttribute.Bottom, 1, 0)
            });
        }
    }
}
