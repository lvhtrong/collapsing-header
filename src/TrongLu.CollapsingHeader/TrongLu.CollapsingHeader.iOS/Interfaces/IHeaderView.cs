using System;
namespace TrongLu.CollapsingHeader.Interfaces
{
    public interface IHeaderView : ICollapsible, IExpandable
    {
        int MinHeight { get; }

        int MaxHeight { get; }
    }
}
