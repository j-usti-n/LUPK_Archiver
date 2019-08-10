Imports System.IO

Public Class frmMain

    'Private Structure fileEntry
    '    Public Path As String
    '    Public Size As Long

    '    Public Sub New(ByVal p As String, ByVal s As Integer)
    '        Path = p
    '        Size = s
    '    End Sub 'New
    'End Structure

    Friend Class globalVarsClass
        'Friend insertFileList As New List(Of Object())
        'Friend replacementList As New List(Of Integer())
        'Friend takenPackageFiles As New HashSet(Of Integer)
        Friend boxMappingList As New List(Of Integer())
        Friend packagePath As String = Nothing
        Friend packageRecordList(-1) As ArchiverModule.packageRecordInfo
        Friend alphabeticalPackageSort(-1) As Integer
        Friend inverseAlphabeticalPackageSort(-1) As Integer
        Friend offsetPackageSort(-1) As Integer
        Friend canClose As Boolean = True
        Friend indexToBeMapped As Integer = -1
        Friend addRecordPending As Boolean = False

        Friend hashRecords As New Dictionary(Of String, String)
        Friend hashIsImported As Boolean = False

        'Friend autoSelectedFilesToInsert As New List(Of Integer)
        'Friend autoSelectedReplacements As New List(Of Integer)
        'Friend autoSelectedPackages As New List(Of Integer)
    End Class

    Friend globalVars As New globalVarsClass

    Friend Sub updateInformation()
        'Dim numMappings As Integer = 0
        'For Each item As ListViewItem In lstViewReplacementTable.Items
        '    numMappings += item.SubItems.Count
        'Next
        'numMappings /= lstViewReplacementTable.Columns.Count
        lstBoxInfo.Items.Clear()
        lstBoxInfo.Items.AddRange({"Package: " & Path.GetFileName(globalVars.packagePath), "Number of files to insert: " & lstViewInsert.Items.Count, "Number of replacements mapped: " & globalVars.boxMappingList.Count, "Number of files in package: " & globalVars.packageRecordList.Length, "Number of records selected: " & lstViewFilesInPackage.SelectedIndices.Count})
    End Sub

    Friend Sub toggleProcessing(ByRef value As Boolean, Optional ByRef text As String = Nothing)
        'toolStripProgressBar.MarqueeAnimationSpeed = If(toolStripProgressBar.MarqueeAnimationSpeed <> 0, 0, 100)
        globalVars.canClose = Not value 'If processing == true, then we can't close the form
        toolStripStatusLabel.Text = If(text, toolStripStatusLabel.Text)
    End Sub

    Private Function searchListOfArraysForMultiple(ByRef listToSearch As List(Of Integer()), ByRef indexToSearch As Integer, ByRef valueToMatch As Integer) As List(Of Integer)
        Dim matchList As List(Of Integer) = New List(Of Integer)

        For i As Integer = 0 To listToSearch.Count - 1
            If listToSearch(i)(indexToSearch) = valueToMatch Then
                matchList.Add(i)
            End If
        Next

        Return matchList
    End Function

    Private Function searchListOfArraysForSingle(ByRef listToSearch As List(Of Integer()), ByRef indexToSearch As Integer, ByRef valueToMatch As Integer) As Integer
        For i As Integer = 0 To listToSearch.Count - 1
            If listToSearch(i)(indexToSearch) = valueToMatch Then Return i
        Next

        Return -1
    End Function

    'Private Function getListViewItemText(ByRef lv As ListView, ByRef x As Integer, ByRef y As Integer) As String
    '    Dim hoverItem As ListViewItem = lv.GetItemAt(x, y)
    '    If hoverItem.Index >= 0 AndAlso hoverItem.Index < lv.Items.Count Then
    '        'MessageBox.Show(hoverItem.Index)
    '        Return hoverItem.Text
    '    End If
    '    Return ""
    'End Function

    Private Sub setupViews()
        'lstViewFilesInPackage.Sorting = SortOrder.Descending
        lstViewFilesInPackage.View = View.Details
        lstViewFilesInPackage.Columns.Add("File Name", CInt(lstViewFilesInPackage.Size.Width * 0.6) - 2)
        lstViewFilesInPackage.Columns.Add("File Size (KB)", CInt(lstViewFilesInPackage.Size.Width * 0.4) - 2)
        lstViewFilesInPackage.VirtualMode = True
        lstViewFilesInPackage.VirtualListSize = 0
        lstViewFilesInPackage.ShowItemToolTips = True
        lstViewFilesInPackage.HideSelection = False
        'lstViewFilesInPackage.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)

        lstViewInsert.View = View.Details
        lstViewInsert.Columns.Add("File Name", CInt(lstViewInsert.Size.Width * 0.6) - 2)
        lstViewInsert.Columns.Add("File Size (KB)", CInt(lstViewInsert.Size.Width * 0.4) - 2)
        lstViewInsert.HideSelection = False
        lstViewInsert.ShowItemToolTips = True
        'lstViewInsert.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize)

        lstViewReplacementTable.View = View.Details
        lstViewReplacementTable.Columns.Add("Replace With", CInt(lstViewReplacementTable.Width * 0.5) - 2)
        lstViewReplacementTable.Columns.Add("Replace", CInt(lstViewReplacementTable.Width * 0.5) - 2)
        lstViewReplacementTable.HideSelection = False
        'lstViewReplacementTable.ShowItemToolTips = True
        'lstViewReplacementTable.AutoResizeColumn(1, ColumnHeaderAutoResizeStyle.ColumnContent)


        lstBoxInfo.HorizontalScrollbar = True

        'toolStripProgressBar.Visible = False


        'mainToolTip.SetToolTip(lstViewFilesInPackage, "")
    End Sub

    'Private Sub setupGlobals()
    '    'accessGlobal("filesToInsertList", New List(Of FileInfo))
    'End Sub

    Private Sub setupForm()
        'toolStripProgressBar.MarqueeAnimationSpeed = 100
        'toolStripStatusLabel.Text = "Preparing GUI.."
        toggleProcessing(True, "Preparing GUI..")
        ''''''''''''''''''''''''''''
        lblMapping.SendToBack()
        lblInformation.SendToBack()

        ' Check if the screen is too small for the form. If so, maximize
        If My.Computer.Screen.WorkingArea.Width < Me.Width Or My.Computer.Screen.WorkingArea.Height < Me.Height Then Me.WindowState = FormWindowState.Maximized
        setupViews()
        updateInformation()
        ''''''''''''''''''''''''''''
        'toolStripProgressBar.MarqueeAnimationSpeed = 0
        'toolStripStatusLabel.Text = "Ready"
        toggleProcessing(False, "Ready")
    End Sub

    Private Sub updateMappings() ' Should only be called when the hash tables are imported
        lstViewReplacementTable.BeginUpdate()
        For i As Integer = 0 To globalVars.boxMappingList.Count - 1
            If lstViewReplacementTable.Items(i).SubItems.Count = 2 Then
                lstViewReplacementTable.Items(i).SubItems(1).Text = lstViewFilesInPackage.Items(globalVars.inverseAlphabeticalPackageSort(globalVars.boxMappingList(i)(1))).Text
            End If
        Next
        lstViewReplacementTable.EndUpdate()
    End Sub

    Private Sub addMapping(ByVal insertlistIndex As Integer, ByVal packageListIndex As Integer)
        Dim correctedIndex As Integer = globalVars.alphabeticalPackageSort(packageListIndex)

        Dim pkIndex As Integer
        'Dim insertIndex As List(Of Integer)
        pkIndex = searchListOfArraysForSingle(globalVars.boxMappingList, 1, correctedIndex)
        'insertIndex = searchListOfArraysForMultiple(globalVars.boxMappingList, 0, insertlistIndex)
        If pkIndex <> -1 Then
            If MessageBox.Show("Mapping already exists for this package record! Would you like to remove the existing mapping?", "Remove existing mapping?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) <> Windows.Forms.DialogResult.Yes Then
                If globalVars.addRecordPending = True Then lstViewReplacementTable.Items.RemoveAt(lstViewReplacementTable.Items.Count - 1)
                Exit Sub
            End If
            lstViewReplacementTable.BeginUpdate()

            lstViewReplacementTable.Items.RemoveAt(pkIndex)
            globalVars.boxMappingList.RemoveAt(pkIndex)
            'ElseIf insertIndex.Count > 0 Then
            '    For Each i As Integer In insertIndex
            '        If globalVars.boxMappingList(i)(1) = packageListIndex Then
            '            MessageBox.Show("Duplicate mapping already exists!", "Duplicate mapping", MessageBoxButtons.OK, MessageBoxIcon.Information)
            '            Exit Sub
            '        End If
            '    Next
        End If
        globalVars.boxMappingList.Add(New Integer() {insertlistIndex, correctedIndex})
        'globalVars.takenPackageFiles.Add(packageListIndex)
        'globalVars.insertFileList(insertlistIndex)(1).Add(globalVars.replacementList.Count)
        'globalVars.replacementList.Add(New Integer(2) {insertlistIndex, globalVars.insertFileList(insertlistIndex)(1).Count, packageListIndex})
        'lstViewReplacementTable.Items.Add(lstViewInsert.Items(insertlistIndex).Text).SubItems.Add(lstViewFilesInPackage.Items(packageListIndex).Text)
        If globalVars.addRecordPending = True Then
            lstViewReplacementTable.Items(lstViewReplacementTable.Items.Count - 1).SubItems.Add(lstViewFilesInPackage.Items(packageListIndex).Text)
            globalVars.addRecordPending = False
        Else
            lstViewReplacementTable.Items.Add(lstViewInsert.Items(insertlistIndex).Text).SubItems.Add(lstViewFilesInPackage.Items(packageListIndex).Text)
        End If
        'lstViewReplacementTable.Items(lstViewReplacementTable.Items.Count - 1).ToolTipText = lstViewInsert.Items(insertlistIndex).Text ' Add tooltip text
        If pkIndex <> -1 Then lstViewReplacementTable.EndUpdate()
        updateInformation()
    End Sub

    Private Sub removeMapping(ByVal mappingIndex As Integer)
        'globalVars.insertFileList(globalVars.replacementList(mappingIndex)(1))(1) = -1
        'globalVars.takenPackageFiles.Remove(globalVars.replacementList(mappingIndex)(2))
        'globalVars.insertFileList(globalVars.replacementList(mappingIndex)(0))(1).RemoveAt(globalVars.replacementList(mappingIndex)(1))
        'globalVars.replacementList.RemoveAt(mappingIndex)
        ''globalVars.boxMappingList(mappingIndex)(0) = -1
        ''globalVars.boxMappingList(mappingIndex)(1) = -1
        If globalVars.addRecordPending AndAlso mappingIndex = globalVars.boxMappingList.Count Then ' To account for half created mappings
            globalVars.addRecordPending = False
        Else
            globalVars.boxMappingList.RemoveAt(mappingIndex)
        End If
        lstViewReplacementTable.Items.RemoveAt(mappingIndex)

        updateInformation()
    End Sub

    Friend Sub unloadPackage()
        lstViewFilesInPackage.VirtualListSize = 0
        If globalVars.packageRecordList.Length <> 0 Then
            ReDim globalVars.packageRecordList(-1)
            ReDim globalVars.alphabeticalPackageSort(-1)
            ReDim globalVars.inverseAlphabeticalPackageSort(-1)
            ReDim globalVars.offsetPackageSort(-1)
        End If
        lstViewFilesInPackage.Items.Clear()
        lstViewReplacementTable.Items.Clear()
        globalVars.boxMappingList.Clear()
        globalVars.packagePath = Nothing
        globalVars.addRecordPending = False
        updateInformation()
    End Sub

    Private Sub invertSelection(ByRef lv As ListView)
        For i As Integer = 0 To lv.Items.Count - 1
            lv.Items(i).Selected = Not lv.Items(i).Selected
        Next
    End Sub

    Private Sub virtualInvertSelection(lv As ListView)
        ' Build a hashset of the currently selected indicies
        Dim selectedArray(lv.SelectedIndices.Count - 1) As Integer
        lv.SelectedIndices.CopyTo(selectedArray, 0)
        Dim selected As New HashSet(Of Integer)
        selected.UnionWith(selectedArray)

        ' Reselect everything that wasn't selected before
        lv.SelectedIndices.Clear()
        For i As Integer = 0 To lv.VirtualListSize - 1
            If Not selected.Contains(i) Then
                lv.SelectedIndices.Add(i)
            End If
        Next
    End Sub


    Private Sub frmMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If globalVars.canClose = False Then
            e.Cancel = True
            MessageBox.Show("Unable to close until operation is complete", "LUPK Archiver", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'setupGlobals()
        setupForm()
        'MessageBox.Show(String.Join("", Enumerable.Range(0, 2).ToArray))
        'MessageBox.Show(globalVars.offsetPackageSort Is Nothing)
        'ArchiverModule.setup()
    End Sub

    'Public Function accessGlobal(ByVal key As String, Optional ByRef value As Object = Nothing) As Object
    '    Static globalDict As New Dictionary(Of String, Object)

    '    If value <> Nothing Then
    '        globalDict(key) = value
    '        Return value
    '    End If
    '    Dim returnVal As Object = Nothing

    '    globalDict.TryGetValue(key, returnVal)
    '    Return returnVal
    'End Function

    'Public Function canClose(Optional ByVal val As Boolean = Nothing) As Boolean
    '    Static canCloseBool As Boolean = True
    '    If val <> Nothing Then
    '        canCloseBool = val
    '        Return val
    '    End If

    '    Return canCloseBool
    'End Function


    Private Sub frmMain_ResizeEnd(sender As Object, e As EventArgs) Handles Me.ResizeEnd
        'lstViewFilesInPackage.Columns(0).Width = CInt(lstViewFilesInPackage.Size.Width * 0.6)
        'lstViewFilesInPackage.Columns(1).Width = CInt(lstViewFilesInPackage.Size.Width * 0.4)

        'lstViewInsert.Columns(0).Width = CInt(lstViewInsert.Size.Width * 0.6)
        'lstViewInsert.Columns(1).Width = CInt(lstViewInsert.Size.Width * 0.4)

        For i As Integer = 0 To lstViewReplacementTable.Columns.Count - 1
            If i Mod 2 = 0 Then
                lstViewReplacementTable.Columns(i).Width = CInt(Math.Floor(lstViewReplacementTable.Width / lstViewReplacementTable.Columns.Count)) - 2
            Else
                lstViewReplacementTable.Columns(i).Width = CInt(Math.Ceiling(lstViewReplacementTable.Width / lstViewReplacementTable.Columns.Count)) - 2
            End If
        Next
    End Sub

    Private Sub btnBrowse_Click(sender As Object, e As EventArgs) Handles btnBrowse.Click
        'Dim filesToInsertList As List(Of String) = globalVars.insertList

        mainOpenFileDialog.Title = "Select one or more files"
        mainOpenFileDialog.Multiselect = True
        mainOpenFileDialog.FileName = ""
        mainOpenFileDialog.Filter = ""

        If mainOpenFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            For i As Integer = 0 To mainOpenFileDialog.FileNames.Length - 1
                Dim curFilePath As String = mainOpenFileDialog.FileNames(i)
                Dim curFileInfo As FileInfo = New FileInfo(curFilePath)

                'globalVars.insertFileList.Add({curFilePath, New List(Of Integer)})

                Dim curItem As ListViewItem = New ListViewItem(Path.GetFileName(curFilePath))
                curItem.ToolTipText = curFilePath
                curItem.SubItems.Add(Math.Round(curFileInfo.Length / 1024, 2).ToString) 'Kilobytes
                curItem.Tag = curFilePath

                lstViewInsert.Items.Add(curItem)
            Next
            updateInformation()
        End If
    End Sub

    Private Sub btnRemoveSelected_Click(sender As Object, e As EventArgs) Handles btnRemoveSelected.Click
        lstViewInsert.BeginUpdate()
        lstViewReplacementTable.BeginUpdate()
        For i As Integer = 0 To lstViewInsert.SelectedItems.Count - 1
            Dim curIndex As Integer = lstViewInsert.SelectedItems.Item(0).Index
            ' MOVING LINE BELOW || If globalVars.indexToBeMapped = curIndex Then globalVars.indexToBeMapped = -1 'Clear index to be mapped if the index is being removed | Don't think there should be a warning here
            Dim searchIndex As List(Of Integer) = searchListOfArraysForMultiple(globalVars.boxMappingList, 0, curIndex)
            'MessageBox.Show(globalVars.indexToBeMapped)
            If globalVars.indexToBeMapped = curIndex Then
                'MessageBox.Show("hi")
                If globalVars.addRecordPending = True Then
                    searchIndex.Add(globalVars.boxMappingList.Count) ' Account for partial mappings
                    globalVars.addRecordPending = False
                End If
                globalVars.indexToBeMapped = -1 'Clear index to be mapped if the index is being removed | Don't think there should be a warning here
            End If
            If searchIndex.Count > 0 Then
                If MessageBox.Show("Are you sure you want to unload the current file? This will remove all associated mappings.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> Windows.Forms.DialogResult.Yes Then
                    'For i2 As Integer = 0 To globalVars.insertFileList(curIndex)(1).Count - 1
                    '    removeMapping(globalVars.insertFileList(curIndex)(1)(0))
                    'Next
                    'removeMapping(searchIndex)
                    Exit Sub
                End If
                For i2 As Integer = 0 To searchIndex.Count - 1
                    removeMapping(searchIndex(i2) - i2)
                Next
            End If
            'globalVars.insertFileList.RemoveAt(curIndex)
            lstViewInsert.Items.RemoveAt(curIndex)
        Next
        lstViewReplacementTable.EndUpdate()
        lstViewInsert.EndUpdate()
        updateInformation()
    End Sub

    Private Sub btnInvert_Click(sender As Object, e As EventArgs) Handles btnInvert.Click ' lstViewInsert
        lstViewInsert.BeginUpdate()
        invertSelection(lstViewInsert)
        lstViewInsert.Focus()
        lstViewInsert.EndUpdate()
        'For i As Integer = 0 To lstViewInsert.Items.Count - 1
        '    'If lstViewInsert.Items(i).Checked = False Then
        '    '    lstViewInsert.set()
        '    'End If
        '    lstViewInsert.Items(i).Selected = Not lstViewInsert.Items(i).Selected
        '    'If lstViewInsert.SelectedIndices.Contains(i) Then
        '    '    lstViewInsert.Items(i).Selected = False
        '    'End If
        'Next
    End Sub

    Private Sub btnAddMapping_Click(sender As Object, e As EventArgs) Handles btnAddMapping.Click
        'If lstViewInsert.CheckedItems.Count <> 1 OrElse lstViewFilesInPackage.CheckedItems.Count <> 1 Then
        '    MessageBox.Show("Only one item each can be selected in ""Files To Insert"" and ""Files In Package""" & vbNewLine & vbNewLine & "Clearing selections in both boxes...", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        '    For i As Integer = 0 To lstViewInsert.CheckedItems.Count - 1
        '        lstViewInsert.CheckedItems(0).Checked = False
        '    Next
        'Else
        '    globalVars.insertFileList(lstViewInsert.CheckedIndices(0))(1) = lstViewFilesInPackage.CheckedIndices(0)
        '    lstViewReplacementTable.Items.Add(lstViewInsert.CheckedItems(0).Text & " -> " & globalVars.packageRecordList(lstViewFilesInPackage.CheckedIndices(0)).hashId)
        'End If
        MessageBox.Show("To add a mapping, double-click a item in the left box. Then double-click an item in the right box.", "Add Mapping", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnBrowseForPackage_Click(sender As Object, e As EventArgs) Handles btnBrowseForPackage.Click
        If globalVars.boxMappingList.Count > 0 Then
            If MessageBox.Show("Are you sure you want to unload the current package? This will remove all mappings from the mapping table.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes Then
                unloadPackage()
            Else
                Exit Sub
            End If
        End If
        mainOpenFileDialog.Title = "Select a package to load"
        mainOpenFileDialog.Multiselect = False
        mainOpenFileDialog.FileName = ""
        mainOpenFileDialog.Filter = "Package Files (*.pk)|*.pk"
        If mainOpenFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            globalVars.packagePath = mainOpenFileDialog.FileName
            ArchiverModule.readPackageHeadersToArray(globalVars.packagePath, globalVars.packageRecordList)
            If globalVars.hashIsImported = True Then
                ArchiverModule.addRecordFileNames(globalVars.packageRecordList, globalVars.hashRecords)
            End If
            ArchiverModule.populateSortedArays(globalVars.packageRecordList, globalVars.alphabeticalPackageSort, globalVars.inverseAlphabeticalPackageSort, globalVars.offsetPackageSort)
            lstViewFilesInPackage.VirtualListSize = globalVars.packageRecordList.Length
            'lstViewFilesInPackage.BeginUpdate()
            'For i As Integer = 0 To globalVars.packageRecordList.Length - 1
            '    lstViewFilesInPackage.Items.Add(If(globalVars.packageRecordList(i).fullFileName, globalVars.packageRecordList(i).hashId)).SubItems.Add(Math.Round(globalVars.packageRecordList(i).DataSize / 1024, 2).ToString) 'Kilobytes
            'Next
            'lstViewFilesInPackage.EndUpdate()
            updateInformation()
        End If
    End Sub

    Private Sub btnUnload_Click(sender As Object, e As EventArgs) Handles btnUnload.Click
        If globalVars.boxMappingList.Count > 0 AndAlso MessageBox.Show("Are you sure you want to unload the current package? This will remove all mappings from the mapping table.", "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> Windows.Forms.DialogResult.Yes Then
            Exit Sub
        End If
        unloadPackage()
    End Sub

    Private Sub lstViewFilesInPackage_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstViewFilesInPackage.MouseDoubleClick
        If globalVars.indexToBeMapped = -1 Then Exit Sub

        Dim index As Integer = lstViewFilesInPackage.GetItemAt(e.X, e.Y).Index
        If index >= 0 AndAlso index < lstViewFilesInPackage.Items.Count Then
            addMapping(globalVars.indexToBeMapped, index)
            'globalVars.indexToBeMapped = -1
        End If
    End Sub

    'Private Sub lstViewFilesInPackage_MouseMove(sender As Object, e As MouseEventArgs) Handles lstViewFilesInPackage.MouseMove
    '    Dim lv As ListView = DirectCast(sender, ListView)
    '    If lv.Items.Count > 0 Then
    '        Dim text As String = getListViewItemText(lv, e.X, e.Y)
    '        If mainToolTip.GetToolTip(lv) <> text Then
    '            mainToolTip.SetToolTip(lv, text)
    '        End If
    '    End If
    'End Sub

    Private Sub lstViewFilesInPackage_RetrieveVirtualItem(sender As Object, e As RetrieveVirtualItemEventArgs) Handles lstViewFilesInPackage.RetrieveVirtualItem
        Dim index As Integer = globalVars.alphabeticalPackageSort(e.ItemIndex)
        'MessageBox.Show(index)
        'Dim lvItem As New ListViewItem(If(globalVars.packageRecordList(e.ItemIndex).fullFileName, globalVars.packageRecordList(e.ItemIndex).hashId))
        Dim txt As String = If(globalVars.packageRecordList(index).fullFileName, globalVars.packageRecordList(index).hashId)
        Dim lvItem As New ListViewItem(Path.GetFileName(txt))
        'lvItem.SubItems.Add(Math.Round(globalVars.packageRecordList(e.ItemIndex).DataSize / 1024, 2).ToString)
        lvItem.SubItems.Add(Math.Round(globalVars.packageRecordList(index).DataSize / 1024, 2).ToString)
        lvItem.ToolTipText = txt

        e.Item = lvItem
    End Sub


    Private Sub lstViewInsert_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstViewInsert.MouseDoubleClick
        'Dim p As Point = lstViewInsert.PointToClient(e.Location)
        Dim lastIndex As Integer = lstViewReplacementTable.Items.Count - 1
        Dim item As ListViewItem = lstViewInsert.GetItemAt(e.X, e.Y)
        If item.Index >= 0 AndAlso item.Index < lstViewInsert.Items.Count Then
            If globalVars.addRecordPending = True Then
                If lstViewInsert.Items(item.Index).Text <> lstViewReplacementTable.Items(lastIndex).Text Then
                    lstViewReplacementTable.Items.RemoveAt(lastIndex)
                Else
                    Exit Sub
                End If
            End If
            globalVars.indexToBeMapped = item.Index
            globalVars.addRecordPending = True
            lstViewReplacementTable.Items.Add(lstViewInsert.Items(item.Index).Text)
            'item.Checked = False
        End If
    End Sub

    Private Sub btnRemoveMapping_Click(sender As Object, e As EventArgs) Handles btnRemoveMapping.Click
        lstViewReplacementTable.BeginUpdate()
        For i As Integer = 0 To lstViewReplacementTable.SelectedItems.Count - 1
            'Dim curIndex As Integer = lstViewReplacementTable.CheckedItems.Item(0).Index

            'removeMapping(lstViewReplacementTable.CheckedItems.Item(0).Index)
            removeMapping(lstViewReplacementTable.SelectedItems.Item(0).Index)
            'globalVars.insertFileList(globalVars.replacementList(curIndex)(1))(1) = -1
            'globalVars.replacementList.RemoveAt(curIndex)
            'lstViewReplacementTable.Items.RemoveAt(curIndex)
        Next
        lstViewReplacementTable.EndUpdate()
    End Sub

    Private Sub btnInvertReplacementSelection_Click(sender As Object, e As EventArgs) Handles btnInvertReplacementSelection.Click
        lstViewReplacementTable.BeginUpdate()
        invertSelection(lstViewReplacementTable)
        lstViewReplacementTable.Focus()
        lstViewReplacementTable.EndUpdate()
    End Sub

    'Private Sub lstViewInsert_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstViewInsert.SelectedIndexChanged
    '    For Each i As Integer In globalVars.autoSelectedFilesToInsert

    '    Next
    '    For Each i As Integer In lstViewInsert.SelectedIndices
    '        For Each i2 As Integer In searchListOfArraysForMultiple(globalVars.boxMappingList, 0, i)

    '        Next
    '    Next
    'End Sub



    Private Sub btnImportHash_Click(sender As Object, e As EventArgs) Handles btnImportHash.Click
        mainFolderBrowserDialog.ShowNewFolderButton = False
        mainFolderBrowserDialog.Description = "Select a folder containing the hash tables"

        If mainFolderBrowserDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            If ArchiverModule.ImportHashTables(mainFolderBrowserDialog.SelectedPath, globalVars.hashRecords) = 0 Then
                If lstViewFilesInPackage.Items.Count > 0 Then
                    ArchiverModule.addRecordFileNames(globalVars.packageRecordList, globalVars.hashRecords)
                    If globalVars.boxMappingList.Count > 0 Then
                        updateMappings()
                    End If
                    ArchiverModule.populateSortedArays(globalVars.packageRecordList, globalVars.alphabeticalPackageSort, globalVars.inverseAlphabeticalPackageSort) 'Only sorts alphabetically
                    'For Each item As ListViewItem In lstViewFilesInPackage.SelectedItems
                    '    item.Selected = False
                    '    lstViewFilesInPackage.Items(globalVars.inverseAlphabeticalPackageSort(item.Index)).Selected = True
                    'Next
                    lstViewFilesInPackage.SelectedIndices.Clear()
                    lstViewFilesInPackage.RedrawItems(0, lstViewFilesInPackage.Items.Count - 1, True)
                End If
                globalVars.hashIsImported = True
            End If
        End If
    End Sub

    'Private Sub lstViewReplacementTable_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles lstViewReplacementTable.ItemSelectionChanged
    '    If e.IsSelected = True Then
    '        lstViewFilesInPackage.EnsureVisible(globalVars.inverseAlphabeticalPackageSort(globalVars.boxMappingList(e.ItemIndex)(1)))
    '        lstViewFilesInPackage.EnsureVisible(globalVars.boxMappingList(e.ItemIndex)(0))
    '    End If
    'End Sub

    Private Sub lstViewReplacementTable_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lstViewReplacementTable.MouseDoubleClick
        Dim index As Integer = lstViewReplacementTable.GetItemAt(e.X, e.Y).Index
        If index >= 0 AndAlso index < lstViewReplacementTable.Items.Count Then
            lstViewFilesInPackage.EnsureVisible(globalVars.inverseAlphabeticalPackageSort(globalVars.boxMappingList(index)(1)))
            lstViewInsert.EnsureVisible(globalVars.boxMappingList(index)(0))
        End If
    End Sub

    Private Sub btnInvertPackageSelect_Click(sender As Object, e As EventArgs) Handles btnInvertPackageSelect.Click
        If lstViewFilesInPackage.Items.Count > 0 Then
            lstViewFilesInPackage.BeginUpdate()
            virtualInvertSelection(lstViewFilesInPackage)
            lstViewFilesInPackage.Focus()
            lstViewFilesInPackage.EndUpdate()
        End If
    End Sub

    Private Sub btnExtractSelected_Click(sender As Object, e As EventArgs) Handles btnExtractSelected.Click
        mainFolderBrowserDialog.Description = "Select a directory to save the extracted files to"
        mainFolderBrowserDialog.ShowNewFolderButton = True

        If mainFolderBrowserDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim sortedSelectedRecordList As New List(Of ArchiverModule.packageRecordInfo)
            Dim saveFullPath As Boolean = False

            For Each i As Integer In lstViewFilesInPackage.SelectedIndices
                sortedSelectedRecordList.Add(globalVars.packageRecordList(globalVars.alphabeticalPackageSort(i)))
            Next

            sortedSelectedRecordList.Sort(Function(a As ArchiverModule.packageRecordInfo, b As ArchiverModule.packageRecordInfo)
                                              Return (If(a.DataOffset < b.DataOffset, -1, 1))
                                          End Function)
            If MessageBox.Show("Would you like to preserve the full paths of the extracted files?", "Preserve full paths?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then saveFullPath = True

            ArchiverModule.extractFiles(globalVars.packagePath, mainFolderBrowserDialog.SelectedPath, saveFullPath, sortedSelectedRecordList)
        End If
    End Sub

    Private Sub lstViewFilesInPackage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstViewFilesInPackage.SelectedIndexChanged
        updateInformation()
    End Sub

    Private Sub btnReplaceFiles_Click(sender As Object, e As EventArgs) Handles btnReplaceFiles.Click
        mainSaveFileDialog.AddExtension = True
        mainSaveFileDialog.CheckFileExists = False
        mainSaveFileDialog.CheckPathExists = True
        mainSaveFileDialog.CreatePrompt = True
        mainSaveFileDialog.DefaultExt = ".pk"
        mainSaveFileDialog.Filter = "Package Files (*.pk)|*.pk"
        mainSaveFileDialog.OverwritePrompt = True
        mainSaveFileDialog.SupportMultiDottedExtensions = False
        mainSaveFileDialog.ValidateNames = True
        mainSaveFileDialog.Title = "Select/Create a new file for the modified package"


        If mainSaveFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            Dim listOfFullPathsToInsert(lstViewInsert.Items.Count - 1) As String

            For i As Integer = 0 To lstViewInsert.Items.Count - 1
                listOfFullPathsToInsert(i) = lstViewInsert.Items(i).Tag
            Next

            ArchiverModule.replaceFiles(globalVars.packagePath, mainSaveFileDialog.FileName, globalVars.boxMappingList, listOfFullPathsToInsert, globalVars.packageRecordList)
        End If
    End Sub
End Class
