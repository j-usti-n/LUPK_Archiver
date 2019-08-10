Imports System.IO
Imports System.Text

Module ArchiverModule


    Private Class archiverGlobalClass
        Friend tableOffset As Integer
        Friend recordCount As Integer
    End Class

    Private archiverGlobals As New archiverGlobalClass

    Private Const readBufferSize As Integer = 16384

    Friend Class packageRecordInfo
        Friend binaryData As Byte()

        'Friend recordIndex As Integer ' Needed for sorting
        Friend recordOffset As Int32
        Friend DataOffset As Int32
        Friend DataSize As Int32
        Friend hashId As String
        Friend fullFileName As String
    End Class

    Private Sub updateRecord(ByRef record As packageRecordInfo)
        Array.Copy(BitConverter.GetBytes(record.DataSize), 0, record.binaryData, 12, 4) ' 32 bit integer size == 4 bytes
        Array.Copy(BitConverter.GetBytes(record.DataOffset), 0, record.binaryData, 92, 4)
    End Sub

    Friend Sub replaceFiles(ByVal packagePath As String, ByVal modifiedPackagePath As String, ByVal boxMappingList As List(Of Integer()), ByRef mappingFileNames() As String, ByVal packageRecordList() As packageRecordInfo)

        frmMain.toggleProcessing(True, "Modifing Package...")

        Dim sortedIndicesToModify(boxMappingList.Count - 1) As Integer
        sortedIndicesToModify = Enumerable.Range(0, boxMappingList.Count).ToArray

        Array.Sort(sortedIndicesToModify, Function(a As Integer, b As Integer)
                                              Return (If(packageRecordList(boxMappingList(a)(1)).DataOffset < packageRecordList(boxMappingList(b)(1)).DataOffset, -1, 1))
                                          End Function)
        Try

            Using writerFs As New FileStream(modifiedPackagePath, FileMode.Create, FileAccess.Write, FileShare.Read)
                Using writer As New BinaryWriter(writerFs, Encoding.ASCII)

                    Dim newTableOffset As Integer

                    Using readFs As New FileStream(packagePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                        Using reader As New BinaryReader(readFs, Encoding.ASCII)

                            Dim totalSizeChange As Integer = 0
                            Dim firstPackageListIndex As Integer = boxMappingList(sortedIndicesToModify(0))(1)

                            ' Write the file portion of the package to the modified package
                            For Each index As Integer In sortedIndicesToModify
                                Dim curMappingArray() As Integer = boxMappingList(index) ' Array size should be 1 (zero based)
                                Dim fileToInsertInfo As New FileInfo(mappingFileNames(curMappingArray(0)))
                                'packageRecordList(curMappingArray(1)).DataOffset += totalSizeChange ' Update DataOffset
                                Dim curRecord As packageRecordInfo = packageRecordList(curMappingArray(1))

                                '''''''''''''
                                Dim toReplaceIsCloser As Boolean = totalSizeChange > 0

                                If toReplaceIsCloser Then
                                    Dim dataToReadUntilNextToReplace As Integer = curRecord.DataOffset - readFs.Position
                                    If dataToReadUntilNextToReplace < 0 Then Throw New Exception("dataToReadUntilNext is less than zero bytes!")

                                    For i As Integer = 0 To Math.Ceiling(dataToReadUntilNextToReplace / readBufferSize) - 1
                                        Dim curBytesRead() As Byte = reader.ReadBytes(Math.Min(readBufferSize, dataToReadUntilNextToReplace))
                                        dataToReadUntilNextToReplace -= curBytesRead.Length
                                        writer.Write(curBytesRead)
                                    Next

                                    readFs.Seek(curRecord.DataSize, SeekOrigin.Current)
                                End If
                                '''''''''''''

                                Dim dataToReadUntilNext As Integer = curRecord.DataOffset - readFs.Position + totalSizeChange
                                If dataToReadUntilNext < 0 Then Throw New Exception("dataToReadUntilNext is less than zero bytes!")

                                For i As Integer = 0 To Math.Ceiling(dataToReadUntilNext / readBufferSize) - 1
                                    Dim curBytesRead() As Byte = reader.ReadBytes(Math.Min(readBufferSize, dataToReadUntilNext))
                                    dataToReadUntilNext -= curBytesRead.Length
                                    writer.Write(curBytesRead)
                                Next

                                Using fileToInsertStream As New FileStream(fileToInsertInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read)
                                    fileToInsertStream.CopyTo(writerFs, readBufferSize)
                                End Using

                                ''''''''''''
                                If Not toReplaceIsCloser Then
                                    Dim dataToReadUntilNextToReplace As Integer = curRecord.DataOffset - readFs.Position
                                    If dataToReadUntilNextToReplace < 0 Then Throw New Exception("dataToReadUntilNext is less than zero bytes!")

                                    If dataToReadUntilNextToReplace > 0 Then
                                        For i As Integer = 0 To Math.Ceiling(dataToReadUntilNextToReplace / readBufferSize) - 1
                                            Dim curBytesRead() As Byte = reader.ReadBytes(Math.Min(readBufferSize, dataToReadUntilNextToReplace))
                                            dataToReadUntilNextToReplace -= curBytesRead.Length
                                            writer.Write(curBytesRead)
                                        Next
                                    End If

                                    readFs.Seek(curRecord.DataSize, SeekOrigin.Current)
                                End If
                                ''''''''''''

                                packageRecordList(curMappingArray(1)).DataSize = fileToInsertInfo.Length ' Update DataSize
                                totalSizeChange += (fileToInsertInfo.Length - curRecord.DataSize)
                                'updateRecord(packageRecordList(curMappingArray(1))) ' Push changes in DataSize and DataOffset to the record's byte array
                            Next

                            'Write the data at the end of the file portion of the package
                            Dim dataToReadUntilTable As Integer = archiverGlobals.tableOffset - readFs.Position
                            If dataToReadUntilTable < 0 Then Throw New Exception("dataToReadUntilTable is less than zero bytes!")
                            For i As Integer = 0 To Math.Ceiling(dataToReadUntilTable / readBufferSize) - 1
                                Dim curBytesRead() As Byte = reader.ReadBytes(Math.Min(readBufferSize, dataToReadUntilTable))
                                dataToReadUntilTable -= curBytesRead.Length
                                writer.Write(curBytesRead)
                            Next

                            newTableOffset = writerFs.Position

                            'Write the record count
                            writer.Write(archiverGlobals.recordCount)

                            Dim firstIndexToChange As Integer = Array.FindIndex(frmMain.globalVars.offsetPackageSort, Function(x) (x = firstPackageListIndex))

                            ' Get the offset of the first file in the archive
                            Dim curOffset As Integer = packageRecordList(frmMain.globalVars.offsetPackageSort(firstIndexToChange)).DataOffset

                            ' Update each record
                            For i2 As Integer = firstIndexToChange To frmMain.globalVars.offsetPackageSort.Length - 1
                                Dim i As Integer = frmMain.globalVars.offsetPackageSort(i2)
                                packageRecordList(i).DataOffset = curOffset
                                updateRecord(packageRecordList(i)) ' Push changes in DataOffset and DataSize to the record's byte array
                                curOffset += packageRecordList(i).DataSize
                            Next

                            ' Write the package record table
                            For i As Integer = 0 To packageRecordList.Length - 1
                                writer.Write(packageRecordList(i).binaryData)
                            Next

                            ' Seek to the end of the record table
                            readFs.Seek(4 + (100 * archiverGlobals.recordCount), SeekOrigin.Current) ' The 4 is for the record count size (32 bit int)

                            Dim dataToRead As Integer = readFs.Length - readFs.Position

                            ' Write all data after the record table
                            For i As Integer = 0 To Math.Ceiling(dataToRead / readBufferSize) - 1
                                Dim curBytesRead() As Byte = reader.ReadBytes(Math.Min(readBufferSize, dataToRead))
                                dataToRead -= curBytesRead.Length
                                writer.Write(curBytesRead)
                            Next

                        End Using
                    End Using

                    'Update table offset
                    writerFs.Seek(writerFs.Length - 8, SeekOrigin.Begin)
                    writer.Write(newTableOffset)

                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error modifing package! Closing and re-opening the application is recomended." & vbNewLine & vbNewLine & ex.Message)
        End Try

        frmMain.toggleProcessing(False, "Done Modifing Package!")
    End Sub

    Friend Sub extractFiles(ByVal packagePath As String, ByVal directoryToSave As String, ByRef saveFullDirectory As Boolean, ByRef packageRecordListToExtract As List(Of packageRecordInfo)) ' recordList Argument should already be sorted by data offset
        frmMain.toggleProcessing(True, "Extracting Files...")

        Try

            Using fs As New FileStream(packagePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                Using reader As New BinaryReader(fs, Encoding.ASCII)
                    For Each record In packageRecordListToExtract
                        Dim fullDirectoryToSave As String = directoryToSave
                        'Dim curRecord As packageRecordInfo = packageRecordList(index)
                        Dim fileName As String = Path.GetFileName(record.fullFileName)
                        If saveFullDirectory = True Then
                            fullDirectoryToSave &= "\" & record.fullFileName.Substring(0, record.fullFileName.Length - fileName.Length - 1) ' Get directory only
                        End If
                        If Not Directory.Exists(fullDirectoryToSave) Then
                            'MessageBox.Show(fullDirectoryToSave)
                            Directory.CreateDirectory(fullDirectoryToSave) ' Create directory(s)
                        End If
                        Dim fileToSavePath As String = fullDirectoryToSave & "\" & fileName
                        If File.Exists(fileToSavePath) AndAlso MessageBox.Show("File at path: """ & fileToSavePath & """ already exists! Would you like to replace the existing file?", "File exists!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
                            Continue For
                        End If
                        'MessageBox.Show(fileToSavePath)
                        'File.Create(fileToSavePath)

                        fs.Seek(CLng(record.DataOffset), SeekOrigin.Begin)

                        Using fileSaveFs As New FileStream(fileToSavePath, FileMode.Create, FileAccess.Write, FileShare.Read)
                            Using fileSaveWriter As New BinaryWriter(fileSaveFs, Encoding.ASCII)
                                Dim dataSizeToRead As Integer = record.DataSize
                                For i As Integer = 0 To Math.Ceiling(dataSizeToRead / readBufferSize) - 1
                                    Dim curBytesRead() As Byte = reader.ReadBytes(Math.Min(readBufferSize, dataSizeToRead))
                                    dataSizeToRead -= curBytesRead.Length
                                    fileSaveWriter.Write(curBytesRead)
                                Next
                            End Using
                        End Using
                    Next
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error extracting package!" & vbNewLine & vbNewLine & ex.Message)
        End Try


        frmMain.toggleProcessing(False, "Done Extracting!")
    End Sub

    Friend Sub readPackageHeadersToArray(ByRef packagePath As String, ByRef packageRecordList() As packageRecordInfo)
        frmMain.toggleProcessing(True, "Reading Package Headers...")

        Try

            Using fs As New FileStream(packagePath, FileMode.Open, FileAccess.Read, FileShare.Read)
                Using reader As New BinaryReader(fs, Encoding.ASCII)

                    ' -- Get record table position.

                    fs.Seek(fs.Length - 8, SeekOrigin.Begin)

                    Dim tableOffset As Integer = reader.ReadInt32()
                    archiverGlobals.tableOffset = tableOffset

                    fs.Seek(CLng(tableOffset), SeekOrigin.Begin)

                    ' -- Read record table.

                    Dim recordCount As Integer = reader.ReadInt32()
                    archiverGlobals.recordCount = recordCount

                    ' Set array size to the amount of records
                    ReDim packageRecordList(recordCount - 1)

                    For i As Integer = 0 To recordCount - 1
                        Dim contentInfo As New packageRecordInfo
                        Dim binaryData(99) As Byte
                        ''contentInfo.recordIndex = i ' Needed for sorting
                        binaryData = reader.ReadBytes(100)
                        contentInfo.DataSize = BitConverter.ToInt32(binaryData, 12)
                        Dim hashIdArray(31) As Byte
                        Array.Copy(binaryData, 16, hashIdArray, 0, 32)
                        contentInfo.hashId = System.Text.Encoding.ASCII.GetString(hashIdArray)
                        contentInfo.DataOffset = BitConverter.ToInt32(binaryData, 92)
                        contentInfo.binaryData = binaryData
                        'reader.ReadBytes(4)
                        'reader.ReadBytes(4)
                        'reader.ReadBytes(4)
                        'contentInfo.DataSize = reader.ReadInt32()
                        'contentInfo.hashId = New String(reader.ReadChars(32))
                        'reader.ReadBytes(4)
                        'reader.ReadBytes(4)
                        '' compressed size
                        'reader.ReadBytes(32)
                        '' alt. hash id
                        'reader.ReadBytes(4)
                        'contentInfo.DataOffset = reader.ReadInt32()
                        'reader.ReadBytes(4)

                        packageRecordList(i) = contentInfo
                    Next
                End Using
            End Using

        Catch ex As Exception
            MessageBox.Show("Error loading package record table!" & vbNewLine & vbNewLine & ex.Message)
            frmMain.unloadPackage()
        End Try

        frmMain.toggleProcessing(False, "Ready")
    End Sub


    Friend Function ImportHashTables(ByRef hashPath As String, ByRef hashRecords As Dictionary(Of String, String)) As Integer
        Dim hashTables As String() = Directory.GetFiles(hashPath, "*.txt", SearchOption.TopDirectoryOnly)

        If hashTables.Length = 0 Then
            MessageBox.Show("Could not locate any hash tables.", "No hash tables!", MessageBoxButtons.OK, MessageBoxIcon.Warning)
            Return -1
        End If

        For i As Integer = 0 To hashTables.Length - 1
            Dim table As String = hashTables(i)

            Using fs As New FileStream(table, FileMode.Open, FileAccess.Read, FileShare.Read)

                If fs Is Nothing Then
                    MessageBox.Show("Could not open " & Path.GetFileName(table) & ".", "Import Hash Tables", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Continue For
                End If

                Using reader As New StreamReader(fs)

                    'Write("  " + Path.GetFileName(table) + "... ")

                    ' First, locate [files] section.

                    While Not reader.EndOfStream
                        Dim entry As String = reader.ReadLine()

                        If [String].Equals(entry, "[files]", StringComparison.OrdinalIgnoreCase) Then
                            Exit While
                        End If
                    End While

                    Dim count As Integer = 0

                    While Not reader.EndOfStream
                        Dim entry As String = reader.ReadLine()

                        Dim elements As String() = entry.Split(","c)

                        If elements.Length < 3 Then
                            Continue While
                        End If

                        ' elements[0] = filename
                        ' elements[1] = data length
                        ' elements[2] = hash id

                        hashRecords(elements(2)) = elements(0).Replace("/"c, "\"c)
                        ' This overwrites duplicate hash records.

                        count += 1
                    End While

                    'reader.Close()
                    'fs.Dispose()

                    'WriteLine(count.ToString() + " records.")
                End Using
            End Using
        Next

        Return 0

    End Function

    Friend Sub addRecordFileNames(ByRef packageRecordList() As packageRecordInfo, ByRef hashRecords As Dictionary(Of String, String))
        For Each record As packageRecordInfo In packageRecordList
            If hashRecords.ContainsKey(record.hashId) Then
                record.fullFileName = hashRecords(record.hashId)
            End If
        Next
    End Sub

    'Friend Function offsetCompare(a As Integer, b As Integer)
    '    Return (If(packageRecordList(a).DataOffset < packageRecordList(b).DataOffset, -1, 1))
    'End Function

    Friend Sub populateSortedArays(ByVal packageRecordList() As packageRecordInfo, ByRef alphabeticalArray() As Integer, ByRef inverseAlphabeticalArray() As Integer, Optional ByRef offsetArray() As Integer = Nothing)
        Dim len As Integer = packageRecordList.Length

        'Dim tempOffsetArray(len - 1)() As Integer
        'Dim tempAlphabeticalArray(len - 1)() As Integer

        ReDim alphabeticalArray(len - 1)

        alphabeticalArray = Enumerable.Range(0, len).ToArray

        Array.Sort(alphabeticalArray, Function(a As Integer, b As Integer)
                                          Return (If(If(packageRecordList(a).fullFileName <> Nothing, Path.GetFileName(packageRecordList(a).fullFileName), packageRecordList(a).hashId).ToLower < If(packageRecordList(b).fullFileName <> Nothing, Path.GetFileName(packageRecordList(b).fullFileName), packageRecordList(b).hashId).ToLower, -1, 1))
                                          'Return (If(Path.GetFileName(packageRecordList(a).fullFileName) < Path.GetFileName(packageRecordList(b).fullFileName), -1, 1))
                                      End Function)

        ReDim inverseAlphabeticalArray(len - 1)

        For i As Integer = 0 To len - 1
            inverseAlphabeticalArray(alphabeticalArray(i)) = i
        Next

        'MessageBox.Show(packageRecordList(inverseAlphabeticalArray(alphabeticalArray(10))).hashId & "  " & packageRecordList(10).hashId)

        If offsetArray IsNot Nothing Then
            ReDim offsetArray(len - 1)

            offsetArray = Enumerable.Range(0, len).ToArray

            Array.Sort(offsetArray, Function(a As Integer, b As Integer)
                                        Return (If(packageRecordList(a).DataOffset < packageRecordList(b).DataOffset, -1, 1))
                                    End Function)
        End If
        'For i As Integer = 0 To len - 1

        'Next
    End Sub

End Module



'Private Shared Function ExtractPackageContents(packagePath As String, outputPath As String, hashRecords As Dictionary(Of String, String)) As Integer
'    Dim fs As New FileStream(packagePath, FileMode.Open, FileAccess.Read, FileShare.Read)

'    Dim reader As New BinaryReader(fs, Encoding.ASCII)

'    ' -- Get record table position.

'    fs.Seek(fs.Length - 8, SeekOrigin.Begin)

'    Dim tableOffset As Integer = reader.ReadInt32()

'    fs.Seek(CLng(tableOffset), SeekOrigin.Begin)

'    ' -- Read record table.

'    Dim recordCount As Integer = reader.ReadInt32()

'    WriteLine("found " + recordCount.ToString() + " records.")

'    Dim contentList As New List(Of PackageContentInfo)(recordCount)

'    For i As Integer = 0 To recordCount - 1
'        Dim contentInfo As New PackageContentInfo()

'        reader.ReadBytes(4)
'        reader.ReadBytes(4)
'        reader.ReadBytes(4)
'        contentInfo.DataSize = reader.ReadInt32()
'        contentInfo.HashID = New String(reader.ReadChars(32))
'        reader.ReadBytes(4)
'        reader.ReadBytes(4)
'        ' compressed size
'        reader.ReadBytes(32)
'        ' alt. hash id
'        reader.ReadBytes(4)
'        contentInfo.DataOffset = reader.ReadInt32()
'        reader.ReadBytes(4)

'        contentList.Add(contentInfo)
'    Next

'    ' -- Sort records by file offset.

'    contentList.Sort(Function(a As PackageContentInfo, b As PackageContentInfo)
'                         Return (If(a.DataOffset < b.DataOffset, -1, 1))

'                     End Function)

'    Write("    Extracting contents...")

'    Dim succeeded As Integer = 0

'    For i As Integer = 0 To contentList.Count - 1
'        Dim item As PackageContentInfo = contentList(i)

'        fs.Seek(CLng(item.DataOffset), SeekOrigin.Begin)

'        Dim data As Byte() = reader.ReadBytes(item.DataSize)

'        Try
'            Dim hashID As String = [String].Empty

'            hashRecords.TryGetValue(item.HashID, hashID)

'            If [String].IsNullOrEmpty(hashID) Then
'                Write("?", ConsoleColor.Yellow)
'            Else
'                Dim filename As String = (outputPath & Convert.ToString("\")) + hashRecords(item.HashID)

'                If removeClientRes Then
'                    filename = filename.Replace("client\res\", "")
'                End If

'                Dim temp As Integer = SaveDataToFile(data, filename)

'                If temp <> 0 Then
'                    bytesWritten += temp
'                    succeeded += 1
'                End If
'            End If
'        Catch generatedExceptionName As KeyNotFoundException
'            Write("X", ConsoleColor.Red)
'        Catch ex As Exception
'            WriteLine(vbLf + ex.Message)
'        End Try
'    Next

'    WriteLine(" done!" & vbLf)

'    Return succeeded

'End Function
' ExtractPackageContents
'------------------------------------------------------------------------------------------------------------------------



'Private Shared Sub ImportHashTables(appPath As String)
'    WriteLine(vbLf & "Importing hash tables..." & vbLf)

'    Dim hashPath As String = appPath & Convert.ToString("\versions")

'    If Not Directory.Exists(hashPath) Then
'        WriteLine("Invalid path structure. Could not find hash tables.", ConsoleColor.Red)
'        Return
'    End If

'    Dim hashTables As String() = Directory.GetFiles(hashPath, "*.txt", SearchOption.TopDirectoryOnly)

'    If hashTables.Length = 0 Then
'        WriteLine("Could not locate any hash tables.", ConsoleColor.Yellow)
'        Return
'    End If


'    Dim hashRecords As New Dictionary(Of String, String)()

'    For i As Integer = 0 To hashTables.Length - 1
'        Dim table As String = hashTables(i)

'        Dim fs As New FileStream(table, FileMode.Open, FileAccess.Read, FileShare.Read)

'        If fs Is Nothing Then
'            WriteLine("Could not open " + Path.GetFileName(table) + ".", ConsoleColor.Red)
'            Continue For
'        End If

'        Dim reader As New StreamReader(fs)

'        Write("  " + Path.GetFileName(table) + "... ")

'        ' First, locate [files] section.

'        While Not reader.EndOfStream
'            Dim entry As String = reader.ReadLine()

'            If [String].Equals(entry, "[files]", StringComparison.OrdinalIgnoreCase) Then
'                Exit While
'            End If
'        End While

'        Dim count As Integer = 0

'        While Not reader.EndOfStream
'            Dim entry As String = reader.ReadLine()

'            Dim elements As String() = entry.Split(","c)

'            If elements.Length < 3 Then
'                Continue While
'            End If

'            ' elements[0] = filename
'            ' elements[1] = data length
'            ' elements[2] = hash id

'            hashRecords(elements(2)) = elements(0).Replace("/"c, "\"c)
'            ' This overwrites duplicate hash records.

'            count += 1
'        End While

'        reader.Close()
'        fs.Dispose()

'        WriteLine(count.ToString() + " records.")
'    Next

'    WriteLine(vbLf & "Found a total of " + hashRecords.Count.ToString("#,##0") + " hash records." & vbLf)

'    Write("Do you want to view these records? (y/N) ")

'    Dim showRecords As ConsoleKeyInfo = Console.ReadKey()

'    Select Case showRecords.Key
'        Case ConsoleKey.Y
'            PrintHashRecords(hashRecords)
'            Exit Select
'        Case Else

'            Console.WriteLine()
'            Exit Select
'    End Select

'    ScanPackages(appPath, hashRecords)

'End Sub
'' ImportHashTables
