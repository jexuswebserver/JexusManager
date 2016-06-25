using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Vista.Controls {
	/// <summary>
	/// 
	/// </summary>
	public class BreadcrumbDropDownItem : ExplorerNavigationHistoryItem {
		/// <summary>
		/// Initializes a new instance of the <see cref="BreadcrumbDropDownItem"/> class.
		/// </summary>
		public BreadcrumbDropDownItem ()
			: this ( string.Empty ) {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BreadcrumbDropDownItem"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		public BreadcrumbDropDownItem ( string text )
			: this ( text, null ) {

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BreadcrumbDropDownItem"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="click">The click.</param>
		public BreadcrumbDropDownItem ( string text, EventHandler click )
			: this ( text, null, click, null ) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BreadcrumbDropDownItem"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="image">The image.</param>
		/// <param name="click">The click.</param>
		public BreadcrumbDropDownItem ( string text, Image image, EventHandler click )
			: this ( text, image, click, null ) {
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BreadcrumbDropDownItem"/> class.
		/// </summary>
		/// <param name="text">The text.</param>
		/// <param name="image">The image.</param>
		/// <param name="click">The click.</param>
		/// <param name="tag">The tag.</param>
		public BreadcrumbDropDownItem ( string text, Image image, EventHandler click, object tag )
			: base ( text, image, click, tag ) {
		}

	}
}
