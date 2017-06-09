Imports System.IO
Imports System.Drawing.Imaging
Imports Microsoft.Office.Interop
Imports System.Drawing.Printing
Imports Microsoft.Reporting.WinForms

Public Class frmBarcode

    Const INTEGRITY_CHECK As String = "ABdkHSjHhCyWyjKX+N2qF64H61OXEuf9W3VqvJnRb9eeeVcUKkXHfsOaln8FbsAFwR4yS8UMxOv2co7QeCElZg=="
    Private BarHash As New Hashtable
    Private isDone As Boolean
    Private count As Integer
    Dim barcode As New DataSet("Barcode")
    Dim barcodeImage As DataTable
    Dim store As DataRow
    Dim myimage As Image

    Private Sub btnBarcode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBarcode.Click
        If txtPath.Text = "" Then Exit Sub
        dgImage.Rows.Clear()
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
        Dim Description As String, ItemCode As String, BranchCode As String, PT As String, Price As Integer
        Me.Enabled = False
        For cnt = 2 To MaxEntries
            BranchCode = oSheet.Cells(cnt, 1).Value.ToString
            PT = oSheet.Cells(cnt, 2).Value.ToString
            ItemCode = oSheet.Cells(cnt, 3).Value.ToString
            Description = oSheet.Cells(cnt, 4).Value.ToString
            Price = oSheet.Cells(cnt, 5).Value.ToString

            dgImage.Rows.Add(BranchCode, PT, Code128(ItemCode, "A", Description), Code128(Description, "A", Price), Description, Price)
            Application.DoEvents()
            'Console.WriteLine("Image Size " & Code128(oSheet.Cells(cnt, 3).value.ToString, "A").Size.ToString)
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
        If isDone Then MsgBox("Success", MsgBoxStyle.Information)

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

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        PrintBarcode()
    End Sub

    'Select Source of Image, Image Pixel width, Image Pexel Height
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

    Private Sub PrintBarcode()
        Dim ans As DialogResult = _
            MsgBox("Do you want to print?", MsgBoxStyle.YesNo + MsgBoxStyle.Information + MsgBoxStyle.DefaultButton2, "Print")
        If ans = Windows.Forms.DialogResult.No Then Exit Sub

        Dim autoPrintPT As Reporting
        Dim report As LocalReport = New LocalReport
        autoPrintPT = New Reporting
        Dim oPS As New System.Drawing.Printing.PrinterSettings
        barcode.Clear()

        If oPS Is Nothing Then Exit Sub

        For Each datarow As DataGridViewRow In dgImage.Rows

            myimage = datarow.Cells(2).Value
            'myimage = ResizeImage(myimage, 120, 75)

            ' Create a new row
            store = barcodeImage.NewRow
            With store
                .Item("Image") = ConvertImage(myimage)
                .Item("Description") = datarow.Cells(3).Value
                .Item("Price") = datarow.Cells(4).Value
                .Item("Pawnticket") = datarow.Cells(1).Value
                .Item("BranchCode") = datarow.Cells(0).Value
            End With

            ' Add it
            barcodeImage.Rows.Add(store)
        Next

        report.ReportPath = "Reports\Report1.rdlc"
        report.DataSources.Add(New ReportDataSource("dsBarcodeImage", barcode.Tables(0)))

        Dim addParameters As New Dictionary(Of String, String)


        If Not addParameters Is Nothing Then
            For Each nPara In addParameters
                Dim tmpPara As New ReportParameter
                tmpPara.Name = nPara.Key
                tmpPara.Values.Add(nPara.Value)
                report.SetParameters(New ReportParameter() {tmpPara})
                Console.WriteLine(String.Format("{0}: {1}", nPara.Key, nPara.Value))
            Next
        End If

        autoPrintPT.Export(report)
        autoPrintPT.m_currentPageIndex = 0
        autoPrintPT.Print(oPS.PrinterName)

        Me.Focus()
    End Sub

    Private Sub frmBarcode_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Add the new table
        barcodeImage = barcode.Tables.Add("BarcodeImage")

        ' Define the columns
        With barcodeImage
            .Columns.Add("Image", GetType(String))
            .Columns.Add("Description", GetType(String))
            .Columns.Add("Price", GetType(Integer))
            .Columns.Add("Pawnticket", GetType(String))
            .Columns.Add("BranchCode", GetType(String))
            .Columns.Add("Image2", GetType(String))
        End With
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        barcode.Clear()
        For Each datarow As DataGridViewRow In dgImage.Rows

            myimage = datarow.Cells(2).Value
            ' myimage = ResizeImage(myimage, 120, 80)

            ' Create a new row
            store = barcodeImage.NewRow
            With store
                .Item("Image") = ConvertImage(myimage)
                .Item("Description") = datarow.Cells(4).Value
                .Item("Price") = datarow.Cells(5).Value
                .Item("Pawnticket") = datarow.Cells(1).Value
                .Item("BranchCode") = datarow.Cells(0).Value
                .Item("Image2") = ConvertImage(datarow.Cells(3).Value)
            End With

            ' Add it
            barcodeImage.Rows.Add(store)
        Next

        Dim rds = New ReportDataSource("dsBarcode", barcode.Tables(0))
        Me.ReportViewer1.LocalReport.DataSources.Clear()
        Me.ReportViewer1.LocalReport.DataSources.Add(rds)
        Me.ReportViewer1.LocalReport.ReportPath = "Reports\rpt_Barcode.rdlc"

        Me.ReportViewer1.RefreshReport()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        PictureBox1.Image = ResizeImage(Code128(TextBox1.Text, "a", "Sample"), 120, 80)
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        PictureBox2.Image = ResizeImage(Code128(TextBox1.Text, "a", "Sample"), 178, 80)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        PictureBox3.Image = Code128(TextBox1.Text, "a", "Sample")
    End Sub

    Private Function ConvertImage(ByVal img As Image)
        Dim mybytearray As Byte()
        Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream
        myimage.Save(ms, System.Drawing.Imaging.ImageFormat.Png)
        mybytearray = ms.ToArray()

        Dim barcodeBase64 As String = Convert.ToBase64String(mybytearray)
        Return barcodeBase64
    End Function
End Class