Module startup
    Sub main()
        Try
            For Each p As Process In Process.GetProcessesByName("TweetDeckED")
                p.Kill()
            Next

        Catch ex As Exception

        End Try


        Try
            Dim programDataDirectory As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\TweetDeck Wrapper\"
            If IO.Directory.Exists(programDataDirectory) Then
                IO.Directory.Delete(programDataDirectory, True)
            End If
        Catch ex As Exception

        End Try
        Try
            Dim programDataDirectory As String = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\TweetDeck\"
            If IO.Directory.Exists(programDataDirectory) Then
                IO.Directory.Delete(programDataDirectory, True)
            End If
        Catch ex As Exception

        End Try
    End Sub
End Module
