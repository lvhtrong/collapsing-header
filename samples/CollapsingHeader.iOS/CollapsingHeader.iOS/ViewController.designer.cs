// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace CollapsingHeader
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIScrollView collapsingContent { get; set; }

		[Outlet]
		CollapsingHeader.ExampleHeaderView collapsingHeader { get; set; }

		[Outlet]
		TrongLu.CollapsingHeader.CollapsingHeaderView collaspingHeaderView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (collapsingContent != null) {
				collapsingContent.Dispose ();
				collapsingContent = null;
			}

			if (collapsingHeader != null) {
				collapsingHeader.Dispose ();
				collapsingHeader = null;
			}

			if (collaspingHeaderView != null) {
				collaspingHeaderView.Dispose ();
				collaspingHeaderView = null;
			}
		}
	}
}
