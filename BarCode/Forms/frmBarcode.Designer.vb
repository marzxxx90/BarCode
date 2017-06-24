<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmBarcode
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.btnPrint = New System.Windows.Forms.Button()
        Me.ofdIMD = New System.Windows.Forms.OpenFileDialog()
        Me.txtPath = New System.Windows.Forms.TextBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.dgImage = New System.Windows.Forms.DataGridView()
        Me.BranchCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Pawnticket = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Image = New System.Windows.Forms.DataGridViewImageColumn()
        Me.Description = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.Price = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.dgImage, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnPrint
        '
        Me.btnPrint.Location = New System.Drawing.Point(719, 19)
        Me.btnPrint.Name = "btnPrint"
        Me.btnPrint.Size = New System.Drawing.Size(79, 32)
        Me.btnPrint.TabIndex = 3
        Me.btnPrint.Text = "Print"
        Me.btnPrint.UseVisualStyleBackColor = True
        '
        'ofdIMD
        '
        Me.ofdIMD.Filter = "Excel 2007|*.xlsx"
        '
        'txtPath
        '
        Me.txtPath.Location = New System.Drawing.Point(6, 19)
        Me.txtPath.Name = "txtPath"
        Me.txtPath.ReadOnly = True
        Me.txtPath.Size = New System.Drawing.Size(743, 20)
        Me.txtPath.TabIndex = 6
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.btnBrowse)
        Me.GroupBox1.Controls.Add(Me.txtPath)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(804, 60)
        Me.GroupBox1.TabIndex = 7
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "File"
        '
        'btnBrowse
        '
        Me.btnBrowse.Location = New System.Drawing.Point(755, 14)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(43, 29)
        Me.btnBrowse.TabIndex = 8
        Me.btnBrowse.Text = ". . ."
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.Button1)
        Me.GroupBox2.Controls.Add(Me.dgImage)
        Me.GroupBox2.Controls.Add(Me.btnPrint)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 78)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(804, 375)
        Me.GroupBox2.TabIndex = 8
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Barcode Info"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(719, 57)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(79, 32)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "Display"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'dgImage
        '
        Me.dgImage.AllowUserToAddRows = False
        Me.dgImage.AllowUserToDeleteRows = False
        Me.dgImage.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.dgImage.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgImage.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.BranchCode, Me.Pawnticket, Me.Image, Me.Description, Me.Price})
        Me.dgImage.Location = New System.Drawing.Point(6, 19)
        Me.dgImage.Name = "dgImage"
        Me.dgImage.ReadOnly = True
        Me.dgImage.RowHeadersVisible = False
        Me.dgImage.RowHeadersWidth = 65
        Me.dgImage.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgImage.Size = New System.Drawing.Size(707, 350)
        Me.dgImage.TabIndex = 9
        '
        'BranchCode
        '
        Me.BranchCode.HeaderText = "Branch Code"
        Me.BranchCode.Name = "BranchCode"
        Me.BranchCode.ReadOnly = True
        '
        'Pawnticket
        '
        Me.Pawnticket.HeaderText = "Pawnticket"
        Me.Pawnticket.Name = "Pawnticket"
        Me.Pawnticket.ReadOnly = True
        '
        'Image
        '
        Me.Image.HeaderText = "Image"
        Me.Image.Name = "Image"
        Me.Image.ReadOnly = True
        Me.Image.Width = 200
        '
        'Description
        '
        Me.Description.HeaderText = "Description"
        Me.Description.Name = "Description"
        Me.Description.ReadOnly = True
        Me.Description.Width = 200
        '
        'Price
        '
        Me.Price.HeaderText = "Price"
        Me.Price.Name = "Price"
        Me.Price.ReadOnly = True
        '
        'frmBarcode
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(825, 465)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "frmBarcode"
        Me.Text = "Barcode Generator"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        CType(Me.dgImage, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnPrint As System.Windows.Forms.Button
    Friend WithEvents ofdIMD As System.Windows.Forms.OpenFileDialog
    Friend WithEvents txtPath As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents dgImage As System.Windows.Forms.DataGridView
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents BranchCode As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Pawnticket As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Image As System.Windows.Forms.DataGridViewImageColumn
    Friend WithEvents Description As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Price As System.Windows.Forms.DataGridViewTextBoxColumn
End Class
