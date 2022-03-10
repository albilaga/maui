﻿using ObjCRuntime;
using UIKit;

namespace Microsoft.Maui.Controls.Handlers.Items
{
	public partial class StructuredItemsViewHandler<TItemsView> : ItemsViewHandler<TItemsView> where TItemsView : StructuredItemsView
	{
		protected override ItemsViewController<TItemsView> CreateController(TItemsView itemsView, ItemsViewLayout layout)
				=> new StructuredItemsViewController<TItemsView>(itemsView, layout);

		protected override ItemsViewLayout SelectLayout()
		{
			var itemSizingStrategy = ItemsView.ItemSizingStrategy;
			var itemsLayout = ItemsView.ItemsLayout;

			if (itemsLayout is GridItemsLayout gridItemsLayout)
			{
				return new GridViewLayout(gridItemsLayout, itemSizingStrategy);
			}

			if (itemsLayout is LinearItemsLayout listItemsLayout)
			{
				return new ListViewLayout(listItemsLayout, itemSizingStrategy);
			}

			// Fall back to vertical list
			return new ListViewLayout(new LinearItemsLayout(ItemsLayoutOrientation.Vertical), itemSizingStrategy);
		}

		public static void MapHeaderTemplate(IStructuredItemsViewHandler handler, StructuredItemsView itemsView)
		{
			((handler as StructuredItemsViewHandler<TItemsView>)?.Controller as StructuredItemsViewController<TItemsView>)?.UpdateHeaderView();
		}

		public static void MapFooterTemplate(IStructuredItemsViewHandler handler, StructuredItemsView itemsView)
		{
			((handler as StructuredItemsViewHandler<TItemsView>)?.Controller as StructuredItemsViewController<TItemsView>)?.UpdateFooterView();
		}

		public static void MapItemsLayout(IStructuredItemsViewHandler handler, StructuredItemsView itemsView)
		{
			(handler as StructuredItemsViewHandler<TItemsView>)?.UpdateLayout();
		}

		public static void MapItemSizingStrategy(IStructuredItemsViewHandler handler, StructuredItemsView itemsView)
		{
			(handler as StructuredItemsViewHandler<TItemsView>)?.UpdateLayout();
		}
	}
}
