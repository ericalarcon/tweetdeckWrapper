Imports System.IO
Imports System.Runtime.InteropServices
Imports Gecko

Public Class startActions
    Dim WithEvents mainForm As New mainForm

    Public Sub New()
        checkRunningInstances()
        installXulRunner()

        initializeGecko()
        initializeTrayIcon()

        checkUpdates()

        mainForm.Show()
    End Sub

    Private Sub checkUpdates()

        Try
            Dim sourceString As String = New System.Net.WebClient().DownloadString("http://www.ericalarcon.com/downloads/tweetdecked/tweetdecked info.txt")
            Dim lines() As String = sourceString.Split(vbNewLine)
            Dim appname As String = lines(0).Split("=")(1)
            Dim version As String = lines(1).Split("=")(1)
            Dim downloadUri As String = lines(2).Split("=")(1)

            If appname.ToLower = "tweetdecked" Then
                If My.Application.Info.Version < New Version(version) Then
                    If MessageBox.Show(My.Resources.appstrings.newVersionMessage, My.Resources.appstrings.newVersionCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Information) = DialogResult.Yes Then
                        Process.Start(downloadUri)
                    End If

                End If

            End If
        Catch ex As Exception

        End Try

    End Sub

    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Private Shared Function ShowWindow(ByVal hwnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    End Function

    Private Sub checkRunningInstances()
        Dim processes As New List(Of Process)
        processes.AddRange(Process.GetProcessesByName("TweetDeckED"))


        If processes.Count > 1 Then

            For Each p As Process In processes
                If p.Id <> Process.GetCurrentProcess.Id Then

                    ShowWindow(My.Settings.mainWindowHandle, 3)

                End If
            Next



            Process.GetCurrentProcess.Kill()
        End If



    End Sub

    Private Sub installXulRunner()


        Try
            If My.Settings.xulrunnerVersion > My.Settings.currentXulRunnerVersion Then
                Dim programDataDirectory As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\TweetDeck Wrapper\"
                If Not Directory.Exists(programDataDirectory) Then
                    Directory.CreateDirectory(programDataDirectory)
                End If
                Dim zipPath As String = programDataDirectory + "xulrunner.zip"
                If Not File.Exists(zipPath) Then
                    File.WriteAllBytes(zipPath, My.Resources.xulrunner)
                    Compression.ZipFile.ExtractToDirectory(zipPath, programDataDirectory)
                End If

                My.Settings.currentXulRunnerVersion = My.Settings.xulrunnerVersion
                My.Settings.Save()

                Try
                    File.Delete(zipPath)
                Catch ex As Exception

                End Try
            End If
        Catch ex As Exception

        End Try






    End Sub

    Private Sub initializeGecko()
        Dim programDataDirectory As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\TweetDeck Wrapper\"

        Dim ProfileDirectory As String = programDataDirectory & "GeckoFX\"

        If Not Directory.Exists(ProfileDirectory) Then
            Directory.CreateDirectory(ProfileDirectory)
        End If
        Xpcom.ProfileDirectory = ProfileDirectory

        Dim xrPath As String = System.Reflection.Assembly.GetExecutingAssembly.Location
        xrPath = programDataDirectory & "xulrunner"
        Xpcom.Initialize(xrPath)

    End Sub


    Private Sub initializeTrayIcon()
        mnuDisplayForm = New ToolStripMenuItem(My.Resources.appstrings.contextmenu1)
        mnuPreferencesForm = New ToolStripMenuItem(My.Resources.appstrings.contextmenu2)
        mnuSep1 = New ToolStripSeparator()
        mnuExit = New ToolStripMenuItem(My.Resources.appstrings.contextmenu3)
        MainMenu = New ContextMenuStrip
        MainMenu.Items.AddRange(New ToolStripItem() {mnuDisplayForm, mnuPreferencesForm, mnuSep1, mnuExit})

        'Initialize the tray
        Tray = New NotifyIcon
        Tray.Icon = My.Resources.twitter_tray_icon
        Tray.ContextMenuStrip = MainMenu
        Tray.Text = "Tweetdeck"

        'Display
        Tray.Visible = True
    End Sub

    Private WithEvents Tray As NotifyIcon
    Private WithEvents MainMenu As ContextMenuStrip
    Private WithEvents mnuDisplayForm As ToolStripMenuItem
    Private WithEvents mnuPreferencesForm As ToolStripMenuItem
    Private WithEvents mnuSep1 As ToolStripSeparator
    Private WithEvents mnuExit As ToolStripMenuItem


    Private Sub mnuDisplayForm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuDisplayForm.Click
        mainForm.Show()
    End Sub

    Private Sub mnuExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuExit.Click
        closeApp()
    End Sub

    Private Sub mnuPreferences_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles mnuPreferencesForm.Click
        Dim f As New preferencesForm
        f.ShowDialog()

    End Sub

    Private Sub Tray_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Tray.DoubleClick
        mainForm.Show()
    End Sub

    Private Sub closeApp()
        Tray.Visible = False

        Process.GetCurrentProcess.Kill()
    End Sub

    Private Sub mainForm_closed() Handles mainForm.didCloseForm
        closeApp()
    End Sub
End Class
