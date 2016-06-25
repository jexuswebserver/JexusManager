using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vista.Controls {

	public class BreadcrumbDropDownItems : IList<BreadcrumbDropDownItem> {
		public event EventHandler ItemAdded;
		public event EventHandler ItemRemoved;

		public BreadcrumbDropDownItems () {
			this.Items = new List<BreadcrumbDropDownItem> ();
		}

		public BreadcrumbDropDownItems ( IEnumerable<BreadcrumbDropDownItem> collection ) {
			this.Items = new List<BreadcrumbDropDownItem> ( collection );
		}

		public BreadcrumbDropDownItems ( int capacity ) {
			this.Items = new List<BreadcrumbDropDownItem> ( capacity );
		}

		private List<BreadcrumbDropDownItem> Items { get; set; }

		#region IList<BreadcrumbDropDownItem> Members

		public int IndexOf ( BreadcrumbDropDownItem item ) {
			return this.Items.IndexOf ( item );
		}

		public void Insert ( int index, BreadcrumbDropDownItem item ) {
			this.Items.Insert ( index, item );
			if ( this.ItemAdded != null ) {
				this.ItemAdded ( this, EventArgs.Empty );
			}
		}

		public void RemoveAt ( int index ) {
			this.Items.RemoveAt ( index );
			if ( this.ItemRemoved != null ) {
				this.ItemRemoved ( this, EventArgs.Empty );
			}
		}

		public BreadcrumbDropDownItem this[ int index ] {
			get {
				return this.Items[ index ];
			}
			set {
				this.Items[ index ] = value;
			}
		}

		#endregion

		#region ICollection<BreadcrumbDropDownItem> Members
		public void AddRange ( IEnumerable<BreadcrumbDropDownItem> collection ) {
			this.Items.AddRange ( collection );
			if ( this.ItemAdded != null ) {
				this.ItemAdded ( this, EventArgs.Empty );
			}
		}

		public void Add ( BreadcrumbDropDownItem item ) {
			this.Items.Add ( item );
			if ( this.ItemAdded != null ) {
				this.ItemAdded ( this, EventArgs.Empty );
			}
		}

		public void Clear () {
			this.Items.Clear ();
			if ( this.ItemRemoved != null ) {
				this.ItemRemoved ( this, EventArgs.Empty );
			}
		}

		public bool Contains ( BreadcrumbDropDownItem item ) {
			return this.Items.Contains ( item );
		}

		public void CopyTo ( BreadcrumbDropDownItem[] array, int arrayIndex ) {
			this.Items.CopyTo ( array, arrayIndex );
		}


		public int Count {
			get { return this.Items.Count; }
		}

		public bool IsReadOnly {
			get { return false; }
		}

		public bool Remove ( BreadcrumbDropDownItem item ) {
			bool result = this.Items.Remove ( item );
			if ( this.ItemRemoved != null ) {
				this.ItemRemoved ( this, EventArgs.Empty );
			}
			return result;
		}

		#endregion

		#region IEnumerable<BreadcrumbDropDownItem> Members

		public IEnumerator<BreadcrumbDropDownItem> GetEnumerator () {
			return this.Items.GetEnumerator ();
		}

		#endregion

		#region IEnumerable Members

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator () {
			return this.GetEnumerator ();
		}

		#endregion
	}
}
