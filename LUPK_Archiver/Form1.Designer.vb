<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.statusStripMain = New System.Windows.Forms.StatusStrip()
        Me.lblCopyright = New System.Windows.Forms.ToolStripStatusLabel()
        Me.toolStripProgressBar = New System.Windows.Forms.ToolStripProgressBar()
        Me.toolStripStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.btnBrowse = New System.Windows.Forms.Button()
        Me.groupBoxFileToReplace = New System.Windows.Forms.GroupBox()
        Me.btnInvert = New System.Windows.Forms.Button()
        Me.lstViewInsert = New System.Windows.Forms.ListView()
        Me.btnRemoveSelected = New System.Windows.Forms.Button()
        Me.btnAddMapping = New System.Windows.Forms.Button()
        Me.groupBoxFilesInPackage = New System.Windows.Forms.GroupBox()
        Me.btnInvertPackageSelect = New System.Windows.Forms.Button()
        Me.btnImportHash = New System.Windows.Forms.Button()
        Me.lstViewFilesInPackage = New System.Windows.Forms.ListView()
        Me.btnUnload = New System.Windows.Forms.Button()
        Me.btnBrowseForPackage = New System.Windows.Forms.Button()
        Me.btnExtractSelected = New System.Windows.Forms.Button()
        Me.lblMapping = New System.Windows.Forms.Label()
        Me.btnReplaceFiles = New System.Windows.Forms.Button()
        Me.btnRemoveMapping = New System.Windows.Forms.Button()
        Me.mainOpenFileDialog = New System.Windows.Forms.OpenFileDialog()
        Me.mainFolderBrowserDialog = New System.Windows.Forms.FolderBrowserDialog()
        Me.mainSaveFileDialog = New System.Windows.Forms.SaveFileDialog()
        Me.lstBoxInfo = New System.Windows.Forms.ListBox()
        Me.lblInformation = New System.Windows.Forms.Label()
        Me.lstViewReplacementTable = New System.Windows.Forms.ListView()
        Me.btnInvertReplacementSelection = New System.Windows.Forms.Button()
        Me.statusStripMain.SuspendLayout()
        Me.groupBoxFileToReplace.SuspendLayout()
        Me.groupBoxFilesInPackage.SuspendLayout()
        Me.SuspendLayout()
        '
        'statusStripMain
        '
        Me.statusStripMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.lblCopyright, Me.toolStripProgressBar, Me.toolStripStatusLabel})
        Me.statusStripMain.Location = New System.Drawing.Point(0, 369)
        Me.statusStripMain.Name = "statusStripMain"
        Me.statusStripMain.Size = New System.Drawing.Size(792, 22)
        Me.statusStripMain.TabIndex = 0
        Me.statusStripMain.Text = "StatusStrip1"
        '
        'lblCopyright
        '
        Me.lblCopyright.Name = "lblCopyright"
        Me.lblCopyright.Size = New System.Drawing.Size(225, 17)
        Me.lblCopyright.Text = "Copyright (C) 2015 by Justin Kula"
        '
        'toolStripProgressBar
        '
        Me.toolStripProgressBar.MarqueeAnimationSpeed = 0
        Me.toolStripProgressBar.Name = "toolStripProgressBar"
        Me.toolStripProgressBar.Size = New System.Drawing.Size(100, 16)
        Me.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        '
        'toolStripStatusLabel
        '
        Me.toolStripStatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.toolStripStatusLabel.Name = "toolStripStatusLabel"
        Me.toolStripStatusLabel.Size = New System.Drawing.Size(59, 17)
        Me.toolStripStatusLabel.Text = "Loading..."
        '
        'btnBrowse
        '
        Me.btnBrowse.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnBrowse.Location = New System.Drawing.Point(6, 296)
        Me.btnBrowse.Name = "btnBrowse"
        Me.btnBrowse.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowse.TabIndex = 0
        Me.btnBrowse.Text = "Browse"
        Me.btnBrowse.UseVisualStyleBackColor = True
        '
        'groupBoxFileToReplace
        '
        Me.groupBoxFileToReplace.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.groupBoxFileToReplace.Controls.Add(Me.btnInvert)
        Me.groupBoxFileToReplace.Controls.Add(Me.lstViewInsert)
        Me.groupBoxFileToReplace.Controls.Add(Me.btnRemoveSelected)
        Me.groupBoxFileToReplace.Controls.Add(Me.btnBrowse)
        Me.groupBoxFileToReplace.Location = New System.Drawing.Point(12, 12)
        Me.groupBoxFileToReplace.Name = "groupBoxFileToReplace"
        Me.groupBoxFileToReplace.Size = New System.Drawing.Size(277, 354)
        Me.groupBoxFileToReplace.TabIndex = 5
        Me.groupBoxFileToReplace.TabStop = False
        Me.groupBoxFileToReplace.Text = "Files To Insert"
        '
        'btnInvert
        '
        Me.btnInvert.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnInvert.Location = New System.Drawing.Point(6, 325)
        Me.btnInvert.Name = "btnInvert"
        Me.btnInvert.Size = New System.Drawing.Size(91, 23)
        Me.btnInvert.TabIndex = 3
        Me.btnInvert.Text = "Invert Select"
        Me.btnInvert.UseVisualStyleBackColor = True
        '
        'lstViewInsert
        '
        Me.lstViewInsert.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstViewInsert.FullRowSelect = True
        Me.lstViewInsert.Location = New System.Drawing.Point(6, 19)
        Me.lstViewInsert.Name = "lstViewInsert"
        Me.lstViewInsert.Size = New System.Drawing.Size(265, 271)
        Me.lstViewInsert.TabIndex = 0
        Me.lstViewInsert.UseCompatibleStateImageBehavior = False
        '
        'btnRemoveSelected
        '
        Me.btnRemoveSelected.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveSelected.Location = New System.Drawing.Point(87, 296)
        Me.btnRemoveSelected.Name = "btnRemoveSelected"
        Me.btnRemoveSelected.Size = New System.Drawing.Size(100, 23)
        Me.btnRemoveSelected.TabIndex = 2
        Me.btnRemoveSelected.Text = "Remove Selected"
        Me.btnRemoveSelected.UseVisualStyleBackColor = True
        '
        'btnAddMapping
        '
        Me.btnAddMapping.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnAddMapping.Location = New System.Drawing.Point(295, 308)
        Me.btnAddMapping.Name = "btnAddMapping"
        Me.btnAddMapping.Size = New System.Drawing.Size(79, 23)
        Me.btnAddMapping.TabIndex = 2
        Me.btnAddMapping.Text = "Add Mapping"
        Me.btnAddMapping.UseVisualStyleBackColor = True
        '
        'groupBoxFilesInPackage
        '
        Me.groupBoxFilesInPackage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.groupBoxFilesInPackage.Controls.Add(Me.btnInvertPackageSelect)
        Me.groupBoxFilesInPackage.Controls.Add(Me.btnImportHash)
        Me.groupBoxFilesInPackage.Controls.Add(Me.lstViewFilesInPackage)
        Me.groupBoxFilesInPackage.Controls.Add(Me.btnUnload)
        Me.groupBoxFilesInPackage.Controls.Add(Me.btnBrowseForPackage)
        Me.groupBoxFilesInPackage.Controls.Add(Me.btnExtractSelected)
        Me.groupBoxFilesInPackage.Location = New System.Drawing.Point(506, 12)
        Me.groupBoxFilesInPackage.Name = "groupBoxFilesInPackage"
        Me.groupBoxFilesInPackage.Size = New System.Drawing.Size(277, 354)
        Me.groupBoxFilesInPackage.TabIndex = 6
        Me.groupBoxFilesInPackage.TabStop = False
        Me.groupBoxFilesInPackage.Text = "Files In Package"
        '
        'btnInvertPackageSelect
        '
        Me.btnInvertPackageSelect.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnInvertPackageSelect.Location = New System.Drawing.Point(87, 296)
        Me.btnInvertPackageSelect.Name = "btnInvertPackageSelect"
        Me.btnInvertPackageSelect.Size = New System.Drawing.Size(75, 23)
        Me.btnInvertPackageSelect.TabIndex = 6
        Me.btnInvertPackageSelect.Text = "Invert Select"
        Me.btnInvertPackageSelect.UseVisualStyleBackColor = True
        '
        'btnImportHash
        '
        Me.btnImportHash.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnImportHash.Location = New System.Drawing.Point(103, 325)
        Me.btnImportHash.Name = "btnImportHash"
        Me.btnImportHash.Size = New System.Drawing.Size(108, 23)
        Me.btnImportHash.TabIndex = 5
        Me.btnImportHash.Text = "Import Hash Tables"
        Me.btnImportHash.UseVisualStyleBackColor = True
        '
        'lstViewFilesInPackage
        '
        Me.lstViewFilesInPackage.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstViewFilesInPackage.FullRowSelect = True
        Me.lstViewFilesInPackage.Location = New System.Drawing.Point(6, 19)
        Me.lstViewFilesInPackage.Name = "lstViewFilesInPackage"
        Me.lstViewFilesInPackage.Size = New System.Drawing.Size(265, 271)
        Me.lstViewFilesInPackage.Sorting = System.Windows.Forms.SortOrder.Descending
        Me.lstViewFilesInPackage.TabIndex = 3
        Me.lstViewFilesInPackage.UseCompatibleStateImageBehavior = False
        Me.lstViewFilesInPackage.VirtualMode = True
        '
        'btnUnload
        '
        Me.btnUnload.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnUnload.Location = New System.Drawing.Point(6, 325)
        Me.btnUnload.Name = "btnUnload"
        Me.btnUnload.Size = New System.Drawing.Size(91, 23)
        Me.btnUnload.TabIndex = 4
        Me.btnUnload.Text = "Unload Archive"
        Me.btnUnload.UseVisualStyleBackColor = True
        '
        'btnBrowseForPackage
        '
        Me.btnBrowseForPackage.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnBrowseForPackage.Location = New System.Drawing.Point(6, 296)
        Me.btnBrowseForPackage.Name = "btnBrowseForPackage"
        Me.btnBrowseForPackage.Size = New System.Drawing.Size(75, 23)
        Me.btnBrowseForPackage.TabIndex = 3
        Me.btnBrowseForPackage.Text = "Browse"
        Me.btnBrowseForPackage.UseVisualStyleBackColor = True
        '
        'btnExtractSelected
        '
        Me.btnExtractSelected.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnExtractSelected.Location = New System.Drawing.Point(168, 296)
        Me.btnExtractSelected.Name = "btnExtractSelected"
        Me.btnExtractSelected.Size = New System.Drawing.Size(103, 23)
        Me.btnExtractSelected.TabIndex = 3
        Me.btnExtractSelected.Text = "Extract Selected..."
        Me.btnExtractSelected.UseVisualStyleBackColor = True
        '
        'lblMapping
        '
        Me.lblMapping.AutoSize = True
        Me.lblMapping.Location = New System.Drawing.Point(295, 15)
        Me.lblMapping.Name = "lblMapping"
        Me.lblMapping.Size = New System.Drawing.Size(139, 13)
        Me.lblMapping.TabIndex = 7
        Me.lblMapping.Text = "Replacement mapping table"
        '
        'btnReplaceFiles
        '
        Me.btnReplaceFiles.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnReplaceFiles.Location = New System.Drawing.Point(392, 337)
        Me.btnReplaceFiles.Name = "btnReplaceFiles"
        Me.btnReplaceFiles.Size = New System.Drawing.Size(108, 23)
        Me.btnReplaceFiles.TabIndex = 3
        Me.btnReplaceFiles.Text = "Replace Files..."
        Me.btnReplaceFiles.UseVisualStyleBackColor = True
        '
        'btnRemoveMapping
        '
        Me.btnRemoveMapping.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveMapping.Location = New System.Drawing.Point(380, 308)
        Me.btnRemoveMapping.Name = "btnRemoveMapping"
        Me.btnRemoveMapping.Size = New System.Drawing.Size(105, 23)
        Me.btnRemoveMapping.TabIndex = 8
        Me.btnRemoveMapping.Text = "Remove Selected"
        Me.btnRemoveMapping.UseVisualStyleBackColor = True
        '
        'lstBoxInfo
        '
        Me.lstBoxInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstBoxInfo.BackColor = System.Drawing.SystemColors.Window
        Me.lstBoxInfo.FormattingEnabled = True
        Me.lstBoxInfo.Location = New System.Drawing.Point(292, 246)
        Me.lstBoxInfo.Name = "lstBoxInfo"
        Me.lstBoxInfo.Size = New System.Drawing.Size(208, 56)
        Me.lstBoxInfo.TabIndex = 9
        '
        'lblInformation
        '
        Me.lblInformation.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblInformation.AutoSize = True
        Me.lblInformation.Location = New System.Drawing.Point(292, 230)
        Me.lblInformation.Name = "lblInformation"
        Me.lblInformation.Size = New System.Drawing.Size(59, 13)
        Me.lblInformation.TabIndex = 10
        Me.lblInformation.Text = "Information"
        '
        'lstViewReplacementTable
        '
        Me.lstViewReplacementTable.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lstViewReplacementTable.FullRowSelect = True
        Me.lstViewReplacementTable.Location = New System.Drawing.Point(295, 31)
        Me.lstViewReplacementTable.Name = "lstViewReplacementTable"
        Me.lstViewReplacementTable.Size = New System.Drawing.Size(205, 196)
        Me.lstViewReplacementTable.TabIndex = 5
        Me.lstViewReplacementTable.UseCompatibleStateImageBehavior = False
        '
        'btnInvertReplacementSelection
        '
        Me.btnInvertReplacementSelection.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.btnInvertReplacementSelection.Location = New System.Drawing.Point(295, 337)
        Me.btnInvertReplacementSelection.Name = "btnInvertReplacementSelection"
        Me.btnInvertReplacementSelection.Size = New System.Drawing.Size(91, 23)
        Me.btnInvertReplacementSelection.TabIndex = 4
        Me.btnInvertReplacementSelection.Text = "Invert Select"
        Me.btnInvertReplacementSelection.UseVisualStyleBackColor = True
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 391)
        Me.Controls.Add(Me.btnInvertReplacementSelection)
        Me.Controls.Add(Me.lstViewReplacementTable)
        Me.Controls.Add(Me.lblInformation)
        Me.Controls.Add(Me.btnAddMapping)
        Me.Controls.Add(Me.lstBoxInfo)
        Me.Controls.Add(Me.btnRemoveMapping)
        Me.Controls.Add(Me.btnReplaceFiles)
        Me.Controls.Add(Me.lblMapping)
        Me.Controls.Add(Me.groupBoxFilesInPackage)
        Me.Controls.Add(Me.groupBoxFileToReplace)
        Me.Controls.Add(Me.statusStripMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmMain"
        Me.Text = "LUPK Archiver"
        Me.statusStripMain.ResumeLayout(False)
        Me.statusStripMain.PerformLayout()
        Me.groupBoxFileToReplace.ResumeLayout(False)
        Me.groupBoxFilesInPackage.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents statusStripMain As System.Windows.Forms.StatusStrip
    Friend WithEvents lblCopyright As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents toolStripProgressBar As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents btnBrowse As System.Windows.Forms.Button
    Friend WithEvents groupBoxFileToReplace As System.Windows.Forms.GroupBox
    Friend WithEvents groupBoxFilesInPackage As System.Windows.Forms.GroupBox
    Friend WithEvents lblMapping As System.Windows.Forms.Label
    Friend WithEvents btnAddMapping As System.Windows.Forms.Button
    Friend WithEvents btnRemoveSelected As System.Windows.Forms.Button
    Friend WithEvents btnExtractSelected As System.Windows.Forms.Button
    Friend WithEvents btnReplaceFiles As System.Windows.Forms.Button
    Friend WithEvents btnRemoveMapping As System.Windows.Forms.Button
    Friend WithEvents mainOpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents toolStripStatusLabel As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents mainFolderBrowserDialog As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents mainSaveFileDialog As System.Windows.Forms.SaveFileDialog
    Friend WithEvents lstBoxInfo As System.Windows.Forms.ListBox
    Friend WithEvents lblInformation As System.Windows.Forms.Label
    Friend WithEvents btnUnload As System.Windows.Forms.Button
    Friend WithEvents btnBrowseForPackage As System.Windows.Forms.Button
    Friend WithEvents lstViewInsert As System.Windows.Forms.ListView
    Friend WithEvents lstViewFilesInPackage As System.Windows.Forms.ListView
    Friend WithEvents lstViewReplacementTable As System.Windows.Forms.ListView
    Friend WithEvents btnInvert As System.Windows.Forms.Button
    Friend WithEvents btnInvertReplacementSelection As System.Windows.Forms.Button
    Friend WithEvents btnImportHash As System.Windows.Forms.Button
    Friend WithEvents btnInvertPackageSelect As System.Windows.Forms.Button

End Class
