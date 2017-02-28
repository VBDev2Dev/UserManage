Imports System.ComponentModel
Imports System.Data.Entity

Public Class frmUsers


    Dim db As UsersEntities

    Private Sub frmUsers_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        PrepareSave(False)
        db.ChangeTracker.DetectChanges()
        e.Cancel = Not db.ChangeTracker.Entries.Where(Function(c) c.State <> EntityState.Detached AndAlso
                                                                c.State <> EntityState.Unchanged).All(Function(c) c.GetValidationResult.IsValid)
        If Not e.Cancel Then
            db.SaveChanges()
            UserBindingSource.DataSource = Nothing
        Else
            e.Cancel = MessageBox.Show("There are invalid users.  Please fix these errors and then try again.
Press Ok to quit without saving.", "Error Saving", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) = DialogResult.Cancel
            If e.Cancel Then ShowInvalid()

        End If

    End Sub

    Private Sub frmUsers_Load(sender As Object, e As EventArgs) Handles Me.Load
        db = New UsersEntities
        db.Users.Load
        UserBindingSource.DataSource = db.Users.Local.ToBindingList
    End Sub

    Private Sub frmUsers_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        UserBindingNavigatorSaveItem.Enabled = False
    End Sub

    Private Sub UserBindingNavigatorSaveItem_Click(sender As Object, e As EventArgs) Handles UserBindingNavigatorSaveItem.Click
        PrepareSave(False)
        db.SaveChanges()
        UserBindingNavigatorSaveItem.Enabled = False

    End Sub


    Private Sub UserBindingSource_CurrentItemChanged(sender As Object, e As EventArgs) Handles UserBindingSource.CurrentItemChanged
        AllowSave()
    End Sub
    Private Sub UserDataGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles UserDataGridView.CellFormatting
        Select Case e.ColumnIndex
            Case dgvcPassword.Index
                e.Value = If(CType(e.Value, Byte())?.ToHex, String.Empty)
        End Select


    End Sub

    Private Sub UserDataGridView_CellParsing(sender As Object, e As DataGridViewCellParsingEventArgs) Handles UserDataGridView.CellParsing
        Select Case e.ColumnIndex
            Case dgvcPassword.Index
                Dim usr As User = CType(UserDataGridView.Rows(e.RowIndex).DataBoundItem, User)
                Dim tmp As String = DirectCast(e.Value, String)
                e.Value = tmp.HashSHA256(usr.Salt)
                e.ParsingApplied = True
        End Select
    End Sub

    Private Sub UserDataGridView_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles UserDataGridView.DataError
        Console.WriteLine($"{e.Exception.Message}\n{e.Exception.StackTrace}")

        e.Cancel = True
    End Sub

    Private Sub UserDataGridView_EditingControlShowing(sender As Object, e As DataGridViewEditingControlShowingEventArgs) Handles UserDataGridView.EditingControlShowing
        Select Case UserDataGridView.CurrentCell.ColumnIndex
            Case dgvcPassword.Index
                Dim txt As TextBox = CType(e.Control, TextBox)
                txt.Text = ""
        End Select
    End Sub

    Private Sub UserDataGridView_RowHeaderMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles UserDataGridView.RowHeaderMouseDoubleClick
        Dim usr As User = CType(UserDataGridView.Rows(e.RowIndex).DataBoundItem, User)
        MessageBox.Show(usr.ToString, "Display User", MessageBoxButtons.OK, MessageBoxIcon.Information)



    End Sub



    Private Sub UserDataGridView_RowValidating(sender As Object, e As DataGridViewCellCancelEventArgs) Handles UserDataGridView.RowValidating
        ShowInvalid()
    End Sub

    Sub AllowSave()
        UserBindingNavigatorSaveItem.Enabled = True

    End Sub


    Sub PrepareSave(Cancel As Boolean)
        If Cancel Then
            UserDataGridView.CancelEdit()
            UserBindingSource.CancelEdit()
        Else

            UserDataGridView.EndEdit()
            UserBindingSource.EndEdit()
        End If



    End Sub
    Sub ShowInvalid()
        db.ChangeTracker.DetectChanges()

        Dim invalid = db.ChangeTracker.Entries.Where(Function(c) Not c.GetValidationResult.IsValid).
            Select(Function(c) New With {
            .entity = c.Entity,
            .Errors = c.GetValidationResult().
            ValidationErrors})

        Dim invalidUsers = invalid.Where(Function(i) TypeOf i.entity Is User)
        Dim invalidRows = UserDataGridView.Rows.Cast(Of DataGridViewRow).Where(Function(r) Not r.IsNewRow AndAlso
                                                                                   invalidUsers.Select(Function(i) i.entity).
                                                                                   Contains(CType(r.DataBoundItem, User)))


        Dim invalidDetails = From usr In invalidUsers Join row In invalidRows On row.DataBoundItem Equals usr.entity

        For Each detail In invalidDetails
            detail.row.ErrorText = $"Invalid User:{String.Join(",", detail.usr.Errors.Select(Function(ex) ex.ErrorMessage))}"

        Next

        For Each row In UserDataGridView.Rows.Cast(Of DataGridViewRow).Except(invalidRows)
            row.ErrorText = String.Empty
        Next
    End Sub
End Class
