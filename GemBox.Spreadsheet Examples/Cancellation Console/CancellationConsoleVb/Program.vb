Imports System
Imports System.Diagnostics
Imports GemBox.Spreadsheet

Module Program

    Sub Main()

        ' If using Professional version, put your serial key below.
        SpreadsheetInfo.SetLicense("FREE-LIMITED-KEY")
        ' Use Trial Mode
        AddHandler SpreadsheetInfo.FreeLimitReached,
            Sub(eventSender, args)
                args.FreeLimitReachedAction = FreeLimitReachedAction.ContinueAsTrial
            End Sub

        ' Create workbook
        Dim workbook = New ExcelFile()
        Dim worksheet = workbook.Worksheets.Add("sheet")
        For i As Integer = 0 To 1000000
            worksheet.Cells(i, 0).Value = i
        Next

        Dim stopwatch = New Stopwatch()
        stopwatch.Start()

        ' Create save options
        Dim saveOptions = New XlsxSaveOptions()
        AddHandler saveOptions.ProgressChanged,
            Sub(eventSender, args)
                ' Cancel operation after five seconds
                If stopwatch.Elapsed.Seconds >= 5 Then
                    args.CancelOperation()
                End If
            End Sub

        Try
            workbook.Save("Cancellation.xlsx", saveOptions)
            Console.WriteLine("Operation fully finished")
        Catch ex As OperationCanceledException
            Console.WriteLine("Operation was cancelled")
        End Try
    End Sub
End Module