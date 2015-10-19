Public Class preferencesForm
    Private Sub cbOnClose_CheckedChanged(sender As Object, e As EventArgs) Handles cbOnClose.CheckedChanged
        My.Settings.minimizeAppOnCloseForm = cbOnClose.Checked
        My.Settings.Save()
    End Sub

    Private Sub preferencesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RemoveHandler cbOnClose.CheckedChanged, AddressOf cbOnClose_CheckedChanged

        cbOnClose.Checked = My.Settings.minimizeAppOnCloseForm

        AddHandler cbOnClose.CheckedChanged, AddressOf cbOnClose_CheckedChanged
    End Sub
End Class