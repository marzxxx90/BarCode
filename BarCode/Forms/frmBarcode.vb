Imports System.IO
Imports System.Drawing.Imaging
Imports Microsoft.Office.Interop
Imports System.Drawing.Printing
Imports Microsoft.Reporting.WinForms

Public Class frmBarcode

    Const INTEGRITY_CHECK As String = "9nKFB3fmcquj4CNHjDz7anVffZbhB8GHWwqe9YzrZFgDEJqmtKEaBA=="
    Private BarHash As New Hashtable
    Private isDone As Boolean
    Private count As Integer

    Private Sub btnBarcode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBarcode.Click
        If txtPath.Text = "" Then Exit Sub
        'Load Excel
        Dim oXL As New Excel.Application
        Dim oWB As Excel.Workbook
        Dim oSheet As Excel.Worksheet

        oWB = oXL.Workbooks.Open(txtPath.Text)
        oSheet = oWB.Worksheets(1)

        Dim MaxColumn As Integer = oSheet.Cells(1, oSheet.Columns.Count).End(Excel.XlDirection.xlToLeft).column
        Dim MaxEntries As Integer = oSheet.Cells(oSheet.Rows.Count, 1).End(Excel.XlDirection.xlUp).row

        Dim checkHeaders(MaxColumn) As String
        For cnt As Integer = 0 To MaxColumn - 1
            checkHeaders(cnt) = oSheet.Cells(1, cnt + 1).value
        Next : checkHeaders(MaxColumn) = oWB.Worksheets(1).name

        If Not TemplateCheck(checkHeaders) Then
            MsgBox("Wrong File or Template was tampered", MsgBoxStyle.Critical, "Error")
            GoTo unloadObj
        End If
        Dim height As Integer = 0

        Me.Enabled = False
        For cnt = 2 To MaxEntries
            dgImage.Rows.Add(Code128(oSheet.Cells(cnt, 1).Value.ToString, "A"), oSheet.Cells(cnt, 2).Value.ToString, oSheet.Cells(cnt, 3).Value.ToString)

            Console.WriteLine("ITEM CODE " & oSheet.Cells(cnt, 1).Value)
            Console.WriteLine("DESCRIPTION " & oSheet.Cells(cnt, 2).Value)
            Console.WriteLine("PRICE " & oSheet.Cells(cnt, 3).Value)
            Application.DoEvents()
        Next

        Me.Enabled = True
        isDone = True

unloadObj:
        'Memory Unload
        oSheet = Nothing
        oWB = Nothing
        oXL.Quit()
        oXL = Nothing

        txtPath.Text = ""
        If isDone Then MsgBox("Item Loaded", MsgBoxStyle.Information)

    End Sub

    Private Sub btnBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowse.Click
        ofdIMD.ShowDialog()

        Dim fileName As String = ofdIMD.FileName
        isDone = False

        If fileName = "" Then Exit Sub
        txtPath.Text = fileName
    End Sub

    Private Function TemplateCheck(ByVal headers() As String) As Boolean
        Dim mergeHeaders As String = ""
        For Each head In headers
            mergeHeaders &= head
        Next
        Console.WriteLine("Template " & HashString(mergeHeaders))
        If HashString(mergeHeaders) = INTEGRITY_CHECK Then Return True
        Return False
    End Function

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        'Dim bm As New Bitmap(Me.dgImage.Width, Me.dgImage.Height)
        'dgImage.DrawToBitmap(bm, New Rectangle(0, 0, Me.dgImage.Width, Me.dgImage.Height))
        'e.Graphics.DrawImage(bm, 0, 0)

        'For Each dr As DataGridViewRow In dgImage.Rows
        '    Dim bm As New Bitmap(dr.Cells(0).Value.Width, dr.Cells(0).Value.Height)
        '    dgImage.DrawToBitmap(bm, New Rectangle(0, 0, Me.dgImage.Width, Me.dgImage.Height))
        'Next
       
        'e.Graphics.DrawImage(bm, 0, 0)
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        PrintDocument1.Print()
    End Sub

    Public Overloads Shared Function ResizeImage(ByVal bmSource As Drawing.Bitmap, ByVal TargetWidth As Int32, ByVal TargetHeight As Int32) As Drawing.Bitmap
        Dim bmDest As New Drawing.Bitmap(TargetWidth, TargetHeight, Drawing.Imaging.PixelFormat.Format32bppArgb)

        Dim nSourceAspectRatio = bmSource.Width / bmSource.Height
        Dim nDestAspectRatio = bmDest.Width / bmDest.Height

        Dim NewX = 0
        Dim NewY = 0
        Dim NewWidth = bmDest.Width
        Dim NewHeight = bmDest.Height

        If nDestAspectRatio = nSourceAspectRatio Then
            'same ratio
        ElseIf nDestAspectRatio > nSourceAspectRatio Then
            'Source is taller
            NewWidth = Convert.ToInt32(Math.Floor(nSourceAspectRatio * NewHeight))
            NewX = Convert.ToInt32(Math.Floor((bmDest.Width - NewWidth) / 2))
        Else
            'Source is wider
            NewHeight = Convert.ToInt32(Math.Floor((1 / nSourceAspectRatio) * NewWidth))
            NewY = Convert.ToInt32(Math.Floor((bmDest.Height - NewHeight) / 2))
        End If

        Using grDest = Drawing.Graphics.FromImage(bmDest)
            With grDest
                .CompositingQuality = Drawing.Drawing2D.CompositingQuality.HighQuality
                .InterpolationMode = Drawing.Drawing2D.InterpolationMode.HighQualityBicubic
                .PixelOffsetMode = Drawing.Drawing2D.PixelOffsetMode.HighQuality
                .SmoothingMode = Drawing.Drawing2D.SmoothingMode.AntiAlias
                .CompositingMode = Drawing.Drawing2D.CompositingMode.SourceOver

                .DrawImage(bmSource, NewX, NewY, NewWidth, NewHeight)
            End With
        End Using

        Return bmDest
    End Function

    Private Sub dgImage_CellContentClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgImage.CellContentClick
        'If e.RowIndex >= 0 Then
        '    Dim row As DataGridViewRow
        '    row = Me.dgImage.Rows(e.RowIndex)
        '    PictureBox1.Image = row.Cells("Column1").Value

        'End If
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        'For Each dr As DataGridViewRow In dgImage.Rows


        'Next

    End Sub

    Private Sub btnReport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim myimage As Image
        myimage = dgImage.CurrentRow.Cells(0).Value
        Dim mybytearray As Byte()
        Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream
        myimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        mybytearray = ms.ToArray()

        Dim barcodeBase64 As String = Convert.ToBase64String(mybytearray)

        'Set report parameter
        Dim param As New Microsoft.Reporting.WinForms.ReportParameter("txtImage", barcodeBase64, False)
        Me.ReportViewer1.LocalReport.SetParameters(New Microsoft.Reporting.WinForms.ReportParameter() {param})

        Me.ReportViewer1.RefreshReport()
    End Sub
End Class