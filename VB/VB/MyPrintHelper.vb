Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base

Namespace WindowsApplication1
	Public Class MyPrintHelper
		Private _VisibleRows As New List(Of Integer)()
		Private ReadOnly _View As GridView
		Public Sub New(ByVal view As GridView)
			_View = view
		End Sub
		Public Sub BeforePrintGridView()
			
			_View.BeginUpdate()
			_VisibleRows.Clear()
			Dim row As Integer = _View.TopRowIndex
			Do While _View.IsRowVisible(row) = RowVisibleState.Visible
				_VisibleRows.Add(_View.GetDataSourceRowIndex(row))
				row += 1
			Loop
			SetPrintOnlyVisibleRowsMode(True)
		End Sub
		Public Sub AfterPrintGridView()
			SetPrintOnlyVisibleRowsMode(False)
			_View.RefreshData()
			_View.EndUpdate()
		End Sub
		Private Sub SetPrintOnlyVisibleRowsMode(ByVal printOnlyVisibleRows As Boolean)
			If printOnlyVisibleRows Then
				AddHandler _View.CustomRowFilter, AddressOf _View_CustomRowFilter
			Else
				RemoveHandler _View.CustomRowFilter, AddressOf _View_CustomRowFilter
			End If
		End Sub

		Private Sub _View_CustomRowFilter(ByVal sender As Object, ByVal e As RowFilterEventArgs)
			e.Visible = _VisibleRows.Contains(e.ListSourceRow)
			e.Handled = True
		End Sub
	End Class
End Namespace
