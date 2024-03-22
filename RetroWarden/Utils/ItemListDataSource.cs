using System.Collections;
using NStack;
using Terminal.Gui;
using Retrowarden.Models;
namespace Retrowarden.Utils
{
    public class ItemListDataSource : IListDataSource
    {
        // Member variables.
        private const int NameColumnWidth = 30;
        private const int UserIdColumnWidth = 30;
        private int _count;
        private int _maxLength;
        private List<VaultItem> _items;
        private BitArray _markedRows;
        
        // This event helps identify when a row is marked or unmarked by a mouse click, because the 
        // SelectedItemChanged event on the listview doesn't fire in that case.
        public event EventHandler? OnSetMark;
        
        public ItemListDataSource(List<VaultItem> items)
        {
            // Set item list and associated values.
            _count = items.Count;
            _markedRows = new BitArray (_count);
            _items = items;
            _maxLength = GetMaxLengthItem ();
        }
        
        public void Render(ListView container, ConsoleDriver driver, bool selected, int item, int col, int line, int width,
            int start = 0)
        {
            container.Move (col, line);
            
            string itemName = String.Format (String.Format ("{{0,{0}}}", -NameColumnWidth), _items [item].ItemName);
            string userId = String.Format (String.Format ("{{0,{0}}}", -UserIdColumnWidth), 
                _items[item].Login == null ? " " : _items[item].Login.UserName);
            
            RenderUstr (driver, $"{itemName} {userId} {_items[item].ItemOwnerName}", col, line, width, start);}
        
        public bool IsMarked(int item)
        {
            bool retVal = false;
            
            if (item >= 0 && item < _count)
            {
                retVal = _markedRows[item];
            }

            return retVal;
        }

        public void SetMark(int item, bool value)
        {
            if (item >= 0 && item < _count)
            {
                _markedRows [item] = value;
                
                // Raise marked event.
                OnSetMark?.Invoke(this, EventArgs.Empty);
            } 
        }
        public IList ToList()
        {
            return _items;
        }
        
        #region Private Methods

        private int GetMaxLengthItem ()
        {
            int retVal = 0;

            if (_items?.Count != 0)
            {
                for (int cnt = 0; cnt < _items.Count; cnt++)
                {
                    var col1 = String.Format(String.Format("{{0,{0}}}", -NameColumnWidth), _items[cnt].ItemName);
                    var col2 = String.Format(String.Format("{{0,{0}}}", -UserIdColumnWidth), 
                        _items[cnt].Login == null ? " " : _items[cnt].Login.UserName);

                    var sc = $"{col1}  {col2} {_items[cnt].ItemOwnerName}";
                    var l = sc.Length;
                    if (l > retVal)
                    {
                        retVal = l;
                    }
                }
            }
            
            return retVal;
        }

        private void RenderUstr (ConsoleDriver driver, ustring ustr, int col, int line, int width, int start = 0)
        {
            int used = 0;
            int index = start;
            while (index < ustr.Length) 
            {
                (uint rune, int size) = Utf8.DecodeRune (ustr, index, index - ustr.Length);
                int count = Rune.ColumnWidth (rune);
                if (used + count >= width) break;
                driver.AddRune (rune);
                used += count;
                index += size;
            }

            while (used < width)
            {
                driver.AddRune (' ');
                used++;
            }
        }
        #endregion
        #region Properties
        public int Count
        {
            get
            {
                return _items != null ? _items.Count : 0;
            }
        }

        public int MarkedItemCount
        {
            get
            {
                int retVal = 0;
                
                // Count items.
                foreach (bool bit in _markedRows)
                {
                    if (bit)
                    {
                        retVal++;
                    }
                }

                return retVal;
            }
        }
        public int Length
        {
            get
            {
                return _maxLength;
            }
        }

        public List<VaultItem> ItemList
        {
            get
            {
                return _items;
            }

            set
            {
                if (value != null) {
                    _count = value.Count;
                    _markedRows = new BitArray (_count);
                    _items = value;
                    _maxLength = GetMaxLengthItem ();
                }
            }
        }

        public List<VaultItem> MarkedItemList
        {
            get
            {
                List<VaultItem> retVal = new List<VaultItem>();
                
                // Loop through marked items.
                for (int cnt = 0; cnt < _markedRows.Count; cnt++)
                {
                    if (_markedRows[cnt])
                    {
                        // Add the item to the return list.
                        retVal.Add(_items[cnt]);
                    }
                }

                return retVal;
            }
        }
        #endregion
    }
}