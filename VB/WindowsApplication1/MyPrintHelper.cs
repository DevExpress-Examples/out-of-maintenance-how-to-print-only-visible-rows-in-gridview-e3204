using System;
using System.Collections.Generic;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Base;

namespace WindowsApplication1
{
    public class MyPrintHelper
    {
        private List<int> _VisibleRows = new List<int>();
        private readonly GridView _View;
        public MyPrintHelper(GridView view)
        {
            _View = view;
        }
        public void BeforePrintGridView()
        {
            SetPrintOnlyVisibleRowsMode(true);
            _View.BeginUpdate();
            _VisibleRows.Clear();
            int row = _View.TopRowIndex;
            while (_View.IsRowVisible(row) == RowVisibleState.Visible)
            {
                _VisibleRows.Add(_View.GetDataSourceRowIndex(row));
                row++;
            }
            _View.RefreshData();
        }
        public void AfterPrintGridView()
        {
            SetPrintOnlyVisibleRowsMode(false);
            _View.RefreshData();
            _View.EndUpdate();
        }
        void SetPrintOnlyVisibleRowsMode(bool printOnlyVisibleRows)
        {
            if (printOnlyVisibleRows)
                _View.CustomRowFilter += _View_CustomRowFilter;
            else
                _View.CustomRowFilter -= _View_CustomRowFilter;
        }

        void _View_CustomRowFilter(object sender, RowFilterEventArgs e)
        {
            e.Visible = _VisibleRows.Contains(e.ListSourceRow);
            e.Handled = true;
        }
    }
}
