using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vista.Controls {
  public class ExplorerNavigationHistory : IList<ExplorerNavigationHistoryItem> {
    public event EventHandler ItemAdded;
    public event EventHandler ItemRemoved;

    #region Constructors
    public ExplorerNavigationHistory ( ) {
      this.Items = new List<ExplorerNavigationHistoryItem> ( );
    }

    public ExplorerNavigationHistory ( IEnumerable<ExplorerNavigationHistoryItem> collection ) {
      this.Items = new List<ExplorerNavigationHistoryItem> ( collection );
    }

    public ExplorerNavigationHistory ( int capacity ) {
      this.Items = new List<ExplorerNavigationHistoryItem> ( capacity );
    }
    #endregion

    private List<ExplorerNavigationHistoryItem> Items { get; set; }

    #region IList<HistoryItem> Members

    public int IndexOf ( ExplorerNavigationHistoryItem item ) {
      return this.Items.IndexOf ( item );
    }

		public int IndexOf ( string key ) {
			for ( int i = 0; i < this.Items.Count; i++ ) {
				ExplorerNavigationHistoryItem item = this[ i ];
				if ( item.Key.CompareTo ( key ) == 0 ) {
					return i;
				}
			}
			return -1;

		}

    public void Insert ( int index, ExplorerNavigationHistoryItem item ) {
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

    public ExplorerNavigationHistoryItem this[ int index ] {
      get {
        return this.Items[ index ]; 
      }
      set {
        this.Items[ index ] = value;
      }
    }

    #endregion

    #region ICollection<HistoryItem> Members

    public void AddRange ( IEnumerable<ExplorerNavigationHistoryItem> collection ) {
      this.Items.AddRange ( collection );
      if ( this.ItemAdded != null ) {
        this.ItemAdded ( this, EventArgs.Empty );
      }
    }

    public void Add ( ExplorerNavigationHistoryItem item ) {
      this.Items.Add ( item );
      if ( this.ItemAdded != null ) {
        this.ItemAdded ( this, EventArgs.Empty );
      }
    }

    public void Clear ( ) {
      this.Items.Clear ( );
      if ( this.ItemRemoved != null ) {
        this.ItemRemoved ( this, EventArgs.Empty );
      }
    }

    public bool Contains ( ExplorerNavigationHistoryItem item ) {
      return this.Items.Contains ( item );
    }

		public bool ContainsKey ( string key ) {
			return IndexOf ( key ) >= 0;
		}

    public void CopyTo ( ExplorerNavigationHistoryItem[ ] array, int arrayIndex ) {
      this.Items.CopyTo ( array, arrayIndex );
    }

    public int Count {
      get { return this.Items.Count; }
    }

    public bool IsReadOnly {
      get { return false; }
    }

    public bool Remove ( ExplorerNavigationHistoryItem item ) {
      bool result = this.Items.Remove ( item );
      if ( this.ItemRemoved != null ) {
        this.ItemRemoved ( this, EventArgs.Empty );
      }
			return result;
    }

    #endregion

    #region IEnumerable<HistoryItem> Members

    public IEnumerator<ExplorerNavigationHistoryItem> GetEnumerator ( ) {
      return this.Items.GetEnumerator ( );
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ( ) {
      return this.GetEnumerator ( );
    }

    #endregion
  }
}
