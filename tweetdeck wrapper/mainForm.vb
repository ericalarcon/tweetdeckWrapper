Public Class mainForm
    Public Event didCloseForm()

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        My.Settings.mainWindowHandle = Process.GetCurrentProcess.MainWindowHandle
        My.Settings.Save()

        GeckoWebBrowser1.Navigate("https://tweetdeck.twitter.com/")
    End Sub

    Private Sub mainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        e.Cancel = True
        Me.Hide()
        If Not My.Settings.minimizeAppOnCloseForm Then
            RaiseEvent didCloseForm()
        End If
    End Sub

    Private Sub GeckoWebBrowser1_GeckoHandleCreated(sender As Object, e As EventArgs) Handles GeckoWebBrowser1.GeckoHandleCreated

    End Sub

    Private Sub GeckoWebBrowser1_CreateWindow(sender As Object, e As Gecko.GeckoCreateWindowEventArgs) Handles GeckoWebBrowser1.CreateWindow
        Process.Start(e.Uri)
        e.Cancel = True
    End Sub
End Class
