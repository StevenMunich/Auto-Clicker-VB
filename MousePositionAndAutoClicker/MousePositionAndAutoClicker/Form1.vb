'// Auto Clicker with Mouse Positioning and File Read/Write Capabilities
'// By Steven Munich
'// Version 1.0
'// This program allows users to record mouse positions, automate clicks, type text, and wait for specified durations. It can read commands from a file and save them for later use.

Imports System.Net
Imports System.Text.RegularExpressions
Imports System.IO

Public Class Form1

    '// CONSTANTS FOR MOUSE EVENT
    Public Const MOUSEEVENTF_LEFTDOWN = &H2
    Public Const MOUSEEVENTF_LEFTUP = &H4
    Private Const MOUSEEVENTF_RIGHTDOWN = &H8
    Private Const MOUSEEVENTF_RIGHTUP = &H10

    '// Functions
    '// GETS KEYSTROKES
    Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vkey As Integer) As Short
    '// Mouse_event for clicks,
    Declare Sub mouse_event Lib "user32.dll" Alias "mouse_event" (ByVal dwFlags As Int32, ByVal dx As Int32, ByVal dy As Int32, ByVal cButtons As Int32, ByVal dwExtraInfo As Int32)
    '//UNUSED FUNCTION
    Declare Function apimouse_event Lib "user32.dll" Alias "mouse_event" (ByVal dwFlags As Int32, ByVal dX As Int32, ByVal dY As Int32, ByVal cButtons As Int32, ByVal dwExtraInfo As Int32) As Boolean

    'Private Declare Function GetAsyncKeyState Lib "user32" (ByVal vkey As Integer) As Short

    '// Global Variables
    Dim OpenFileDialongFilenameString As String = 0
    Dim X As Integer
    Dim Y As Integer
    Dim CrownJEWBool As Boolean = False
    Dim LoopRun As Boolean = True

    Private Sub Form3_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        MousePositionTImer.Start()
    End Sub

    Private Sub MousePositionTImer_Tick(sender As Object, e As EventArgs) Handles MousePositionTImer.Tick
        Label13.Text = MousePosition.ToString() 'Shows X,Y coordinates on screen

        If GetAsyncKeyState(Keys.Insert) Then 'copies X,Y coordinates when 'INSERT' key is pressed.
            Me.Cursor = New Cursor(Cursor.Current.Handle)
            TextBox1.Text = Cursor.Position.X
            TextBox2.Text = Cursor.Position.Y
        End If

        If GetAsyncKeyState(Keys.End) Then

            MsgBox("Fire")
            ' LoopRun = False
            ' CheckBox1.Checked = False
            'Timer1.Stop()
        End If




    End Sub



    'LOAD FILE
    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click

        Dim result As DialogResult = OpenFileDialog1.ShowDialog()  ' Call ShowDialog

        If result = Windows.Forms.DialogResult.OK Then

            ' Get the file name.
            Dim path As String = OpenFileDialog1.FileName
            Try

                Dim text As String = path ' Read in text.

                ' For debugging.
                OpenFileDialongFilenameString = text.ToString()

                Call readlinebyline() 'Code to read line-by;line

            Catch ex As Exception
                MsgBox("Error opening" & OpenFileDialongFilenameString)
            End Try
        End If
    End Sub

    'SAVE FILE
    Dim SaveFileDialongFilenameString As String = 0
    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Dim result As DialogResult = SaveFileDialog1.ShowDialog()

        ' Test result.
        If result = Windows.Forms.DialogResult.OK Then

            ' Get the file name.
            Dim path As String = SaveFileDialog1.FileName
            Try
                ' Read in text.
                Dim text As String = path

                ' For debugging.
                SaveFileDialongFilenameString = text.ToString()
            Catch
                MsgBox("ERROR PROCESSING")
            End Try
            'MsgBox(SaveFileDialongFilenameString) '//for debugging
        End If

        Try
            Dim SI As Integer = 0
            Dim SI5 As String = ListBox2.Items.Count
            MsgBox(SI5)

            Dim file As System.IO.StreamWriter 'retrieves file name & location
            file = My.Computer.FileSystem.OpenTextFileWriter(SaveFileDialongFilenameString, True)

            'writes data to the file
            Do While SI < SI5
                file.WriteLine(ListBox2.Items(SI))
                SI = SI + 1
            Loop

            file.Close()
        Catch ex As Exception
            MsgBox("ERROR SAVING")
        End Try

    End Sub

    'TEST
    Private Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Me.Cursor = New Cursor(Cursor.Current.Handle)
        Cursor.Position = New Point(TextBox1.Text, TextBox2.Text)

        If RadioButton1.Checked = True Then
            mouse_event(&H2, 0, 0, 0, 1)
            mouse_event(&H4, 0, 0, 0, 1)
        ElseIf RadioButton2.Checked = True Then
            mouse_event(&H8, 0, 0, 0, 1)
            mouse_event(&H10, 0, 0, 0, 1)
        End If

    End Sub

    'Add Click CMD
    Private Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click

        Dim S As String = "M"
        If RadioButton1.Checked = True Then
            S = "L"
        ElseIf RadioButton2.Checked = True Then
            S = "R"
        End If

        ListBox2.Items.Add(S & TextBox1.Text + " " & TextBox2.Text)

    End Sub

    'Add Type CMD
    Private Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        If TextBox3.Text = Nothing Then
            MsgBox("Add A Text to Say", vbOKOnly)
        Else
            ListBox2.Items.Add("T" & TextBox3.Text)
        End If

    End Sub
    'Add Wait CMD
    Private Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        ListBox2.Items.Add("W" & TextBox4.Text)
    End Sub



    '=======================+++^^^^^^^^^^^CROWNING JEWEL^^^^^^^^^^^^+++========================='



    'RUNS THE BOT
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click



        Timer1.Start()

        '//WORKING CODE
        If CheckBox1.Checked = True Then


            While LoopRun = True
                Call CrownJewel()
            End While
            '//END LoOp when Home Key is pressed(line  ). Boolean is set to false
        Else

            Call CrownJewel()
        End If



    End Sub

    'DELETE
    Private Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Try
            ListBox2.Items.RemoveAt(ListBox2.SelectedIndices(0))
        Catch ex As Exception 'if Value is NULL nothing is done, error is handled
        End Try 'TRY STATEMENT INCASE A NULL VALUE CREATES AN ERROR.
    End Sub

    Private Sub Button8_Click_1(sender As Object, e As EventArgs) Handles Button8.Click
        RadioButton1.Checked = False
        RadioButton2.Checked = False
    End Sub


    '==============================CALLED SUBS===================================
    '// 5 MAIN FUNCTIONS: 1Right_CLICK: 2Left_Click: 3Move_Mouse: 4TYPE_KEYBOARD: 5WAIT_(x)amount: Function/ is called based on first character of line in listbox. R L M T W


    Async Sub CrownJewel()


        Dim I As Integer = 0
        Dim amount As String = ListBox2.Items.Count

        If amount = I Then 'I = the amount on list, needs to be reset to zero for next time.
            I = 0
        Else

            Do Until I = amount
                Dim S As String = ListBox2.Items(I) 'Item on List; Commands Followed by Next.         
                Dim SS As String = S.Chars(0) 'First Character of Line: R L M T W

                '//1st FUNCTION
                If (SS = "R") Then 'Right Clicks
                    If S.Length = 8 Then
                        X = S.Substring(1, 3)
                        Y = S.Substring(5, 3)
                    ElseIf S.Length = 9 Then
                        X = S.Substring(1, 4)
                        Y = S.Substring(6, 3)
                    ElseIf S.Length = 7 Then
                        X = S.Substring(1, 2)
                        Y = S.Substring(4)
                    ElseIf S.Length = 6 Then
                        X = S.Substring(1, 2)
                        Y = S.Substring(3)
                    ElseIf S.Length = 5 Then
                        X = S.Substring(1, 2)
                        Y = S.Substring(3)
                    ElseIf S.Length = 4 Then
                        X = S.Substring(1, 1)
                        Y = S.Substring(3)

                    End If
                    Me.Cursor = New Cursor(Cursor.Current.Handle)
                    Cursor.Position = New Point(X, Y)
                    mouse_event(&H8, 0, 0, 0, 1)
                    mouse_event(&H10, 0, 0, 0, 1)

                    '//2nd FUNCTION
                ElseIf (SS = "L") Then 'Left Click
                    If S.Length = 8 Then
                        X = S.Substring(1, 3)
                        Y = S.Substring(5, 3)
                    ElseIf S.Length = 9 Then 'Incase X is 1000+
                        X = S.Substring(1, 4)
                        Y = S.Substring(6, 3)
                    ElseIf S.Length = 7 Then
                        X = S.Substring(1, 2)
                        Y = S.Substring(4)
                    ElseIf S.Length = 6 Then
                        X = S.Substring(1, 2)
                        Y = S.Substring(3)
                    ElseIf S.Length = 5 Then
                        X = S.Substring(1, 2)
                        Y = S.Substring(3)
                    ElseIf S.Length = 4 Then
                        X = S.Substring(1, 1)
                        Y = S.Substring(3)
                    End If
                    Me.Cursor = New Cursor(Cursor.Current.Handle)
                    Cursor.Position = New Point(X, Y)
                    mouse_event(&H2, 0, 0, 0, 1)
                    mouse_event(&H4, 0, 0, 0, 1)


                    '//3rd FUNCTION
                ElseIf (SS = "M") Then 'Moves mouse without Clicking
                    If S.Length = 8 Then
                        X = S.Substring(1, 2)
                        Y = S.Substring(5, 3)
                        MessageBox.Show("X=" + X + " and Y = " + Y)

                    ElseIf S.Length = 9 Then
                        X = S.Substring(1, 4)
                        Y = S.Substring(6, 3)
                    ElseIf S.Length = 7 Then
                        X = S.Substring(1, 2)
                        Y = S.Substring(4)
                    ElseIf S.Length = 6 Then
                        X = S.Substring(1, 2)
                        Y = S.Substring(3)
                    ElseIf S.Length = 5 Then
                        X = S.Substring(1, 2)
                        Y = S.Substring(3)
                    ElseIf S.Length = 4 Then
                        X = S.Substring(1, 1)
                        Y = S.Substring(3)
                    End If
                    Me.Cursor = New Cursor(Cursor.Current.Handle)
                    Cursor.Position = New Point(X, Y)


                    '//4th FUNCTION
                ElseIf (SS = "T") Then
                    Dim Text2Type As String = S.Substring(1) 'grabs text to send
                    My.Computer.Keyboard.SendKeys(Text2Type) 'types in text

                    '//5th FUNCTION
                ElseIf (SS = "W") Then
                    Dim TIME As String = S.Substring(1) 'gets time value
                    Try
                        'System.Threading.Thread.Sleep(TIME * 1000) 'times by milliseconds ' OLDWAY
                        Await Task.Delay(TIME * 1000) 'new way and sub routine must also be async for the await. Just like JS =D
                    Catch ex As Exception
                    End Try
                Else
                    'null
                End If
                I += 1 'runs next commands
            Loop

        End If



    End Sub




    Sub readlinebyline()
        Dim objStreamReader As StreamReader
        Dim strLine As String

        'Pass the file path and the file name to the StreamReader constructor.
        objStreamReader = New StreamReader(OpenFileDialongFilenameString)

        'Read the first line of text.
        strLine = objStreamReader.ReadLine

        'Continue to read until you reach the end of the file.
        Do While Not strLine Is Nothing

            'Write the line to the Console window.
            '//STEVENS NOTES: Changed to add to a LISTBOX
            ListBox2.Items.Add(strLine)

            'Read the next line.
            strLine = objStreamReader.ReadLine
        Loop

        'Close the file.
        objStreamReader.Close()
    End Sub



    'WriteFile(TEST) -----IS NOT USED
    Private Sub WriteFile()

        Dim SI As Integer = 0
        Dim SI5 As String = ListBox2.Items.Count
        MsgBox(SI5)

        Dim file As System.IO.StreamWriter
        file = My.Computer.FileSystem.OpenTextFileWriter("c:\Gmail\test.txt", True)


        Do While SI < SI5
            file.WriteLine(ListBox2.Items(SI))
            SI = SI + 1
        Loop

        file.Close()



    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        ListBox2.Items.Clear()

    End Sub

    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        LoopRun = True
    End Sub


    '// ===================Integrity Restraints / Input Validation====================


    Private Sub check_Num(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        Dim inputText As String = TextBox1.Text.Trim()
        Dim numberValue As Integer

        ' TryParse returns True if conversion succeeds
        If Integer.TryParse(inputText, numberValue) Then
            'MessageBox.Show("Valid number entered: " & numberValue.ToString(),
            '     "Validation Success",
            'MessageBoxButtons.OK,
            'MessageBoxIcon.Information)
        Else
            MessageBox.Show("Place only Numbers here.",
                            "Validation Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            TextBox1.Undo()

        End If
    End Sub

    Private Sub check_Num2(sender As Object, e As EventArgs) Handles TextBox2.TextChanged
        Dim inputText As String = TextBox2.Text.Trim()
        Dim numberValue As Integer

        ' TryParse returns True if conversion succeeds
        If Integer.TryParse(inputText, numberValue) Then
            'MessageBox.Show("Valid number entered: " & numberValue.ToString(),
            '     "Validation Success",
            'MessageBoxButtons.OK,
            'MessageBoxIcon.Information)
        Else
            MessageBox.Show("Place only Numbers here.",
                            "Validation Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            TextBox2.Undo()

        End If
    End Sub

    Private Sub check_Num3(sender As Object, e As EventArgs) Handles TextBox4.TextChanged
        Dim inputText As String = TextBox4.Text.Trim()
        Dim numberValue As Double

        ' TryParse returns True if conversion succeeds
        If Double.TryParse(inputText, numberValue) Then
            'MessageBox.Show("Valid number entered: " & numberValue.ToString(),
            '     "Validation Success",
            'MessageBoxButtons.OK,
            'MessageBoxIcon.Information)
        Else
            MessageBox.Show("Place only Numbers here.",
                            "Validation Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error)
            TextBox4.Text = "30.00"

        End If
    End Sub

End Class