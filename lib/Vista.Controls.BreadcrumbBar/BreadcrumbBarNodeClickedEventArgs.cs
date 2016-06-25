using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vista.Controls {
	public class BreadcrumbBarNodeClickedEventArgs : EventArgs {
		public BreadcrumbBarNodeClickedEventArgs (BreadcrumbBarNode node) : base() {
			this.Node = node;
		}

		public BreadcrumbBarNode Node { get; set; }
	}
}
