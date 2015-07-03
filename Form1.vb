Imports System.Text.RegularExpressions
Public Class Form1

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode >= Keys.D0 And e.KeyCode <= Keys.D9 Then
            e.SuppressKeyPress = False
        ElseIf e.KeyCode >= Keys.NumPad0 And e.KeyCode <= Keys.NumPad9 Then
            e.SuppressKeyPress = False
        ElseIf e.KeyCode = Keys.Back Then
            e.SuppressKeyPress = False
        Else
            Select Case e.KeyCode
                Case Keys.Left, Keys.Up, Keys.Down, Keys.Right, Keys.Home, Keys.End
                    e.SuppressKeyPress = False
                Case Else
                    My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)
                    e.SuppressKeyPress = True
            End Select
        End If
    End Sub

    ''' <summary>
    ''' 把指定之整數轉為中國大寫數字。
    ''' </summary>
    ''' <param name="inputNumber">要轉換的數值。</param>
    ''' <returns>已轉換之中國大寫數字。</returns>
    ''' <remarks></remarks>
    Public Function ConvertToChineseNumbers(inputNumber As Long) As String

        Dim s As String = String.Format("{0:0}", inputNumber)
        Dim chinNums As String = "零壹貳參肆伍陸柒捌玖"
        Dim chinNums1 As String = "萬億兆"
        Dim chinNums2 As String = "拾佰仟"
        Dim output As String = ""
        Dim i As Long = 0
        Dim j As Long = -1
        Dim k As Long = -1
        Dim z As Long = s.Length - 1
        If s = "0" Then
            output &= "零"
        Else
            While z >= 0 AndAlso s(z) = "0"c
                z -= 1
            End While
            For i = 0 To z
                Dim x As Long = (s.Length - 1 - i) Mod 4
                Dim y As Long = (s.Length - 1 - i) \ 4
                output &= chinNums(Convert.ToInt64(s(i).ToString))
                If x = 0 And y = 0 Then
                ElseIf x = 0 Then
                    output &= chinNums1(y - 1)
                Else
                    If s(i) <> "0"c Then
                        output &= chinNums2(x - 1)
                    End If
                End If
            Next
            If s.Length - 1 - z >= 4 Then
                Dim y As Long = (s.Length - 1 - z) \ 4
                output &= chinNums1(y - 1)
            End If
        End If

        output &= "圓整"

        While output.IndexOf("零零") <> -1
            output = output.Replace("零零", "零")
        End While
        For Each inoo As String In chinNums1
            While output.IndexOf(inoo & inoo) <> -1
                output = output.Replace(inoo & inoo, inoo)
            End While
        Next
        For Each inoo As String In chinNums1
            output = output.Replace("零" & inoo, inoo)
        Next
        output = output.Replace("億萬", "億")
        output = output.Replace("兆億", "兆")
        Return output
    End Function

    ''' <summary>
    ''' 計算指定之銀碼之含稅價格與相應稅金。
    ''' </summary>
    ''' <param name="inputNumber">要轉換的數值。</param>
    ''' <param name="taxRate">要用以計算的稅率。</param>
    ''' <param name="taxValue">用以傳回包含計算出之稅金之參數。</param>
    ''' <returns>計算出之含稅價格。</returns>
    ''' <remarks>計算出的價格將從方法傳回值傳回，而稅金將從 taxValue 參數傳回。</remarks>
    Public Function GetPriceWithTax(inputNumber As Long, taxRate As Double, ByRef taxValue As Long) As Long
        Dim r As Double = inputNumber * (1 + taxRate)
        taxValue = inputNumber * taxRate
        Return r
    End Function

    ''' <summary>
    ''' 計算指定之銀碼之除稅價格與相應稅金。
    ''' </summary>
    ''' <param name="inputNumber">要轉換的數值。</param>
    ''' <param name="taxRate">要用以計算的稅率。</param>
    ''' <param name="taxValue">用以傳回包含計算出之稅金之參數。</param>
    ''' <returns>計算出之除稅價格。</returns>
    ''' <remarks>計算出的價格將從方法傳回值傳回，而稅金將從 taxValue 參數傳回。</remarks>
    Public Function GetPriceWithoutTax(inputNumber As Long, taxRate As Double, ByRef taxValue As Long) As Long
        Dim r As Double = inputNumber / (1 + taxRate)
        taxValue = inputNumber - r
        Return r
    End Function

    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles TextBox1.TextChanged
        If TextBox1.Text = "" Then
        ElseIf TextBox1.Text.Length > 1 AndAlso TextBox1.Text.StartsWith("0") Then
            TextBox1.Text = TextBox1.Text.TrimStart("0")
        End If
        Dim inputVal As Long = If(TextBox1.Text = "", 0, Convert.ToInt64(TextBox1.Text))
        If inputVal > 999999999999 Then
            Return
        End If
        TextBox2.Text = ConvertToChineseNumbers(inputVal)
        Dim tv As Long = 0.0
        TextBox3.Text = String.Format("{0:0}", GetPriceWithTax(inputVal, NumericUpDown1.Value / 100, tv))
        TextBox4.Text = String.Format("{0:0}", tv)
        TextBox5.Text = String.Format("{0:0}", GetPriceWithoutTax(inputVal, NumericUpDown1.Value / 100, tv))
        TextBox6.Text = String.Format("{0:0}", tv)
    End Sub

    Private Sub NumericUpDown1_ValueChanged(sender As Object, e As EventArgs) Handles NumericUpDown1.ValueChanged
        Dim inputVal As Long = Convert.ToInt64(TextBox1.Text)
        Dim tv As Long = 0.0
        TextBox3.Text = String.Format("{0:0}", GetPriceWithTax(inputVal, NumericUpDown1.Value / 100, tv))
        TextBox4.Text = String.Format("{0:0}", tv)
        TextBox5.Text = String.Format("{0:0}", GetPriceWithoutTax(inputVal, NumericUpDown1.Value / 100, tv))
        TextBox6.Text = String.Format("{0:0}", tv)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        TextBox1.Text = ""
        TextBox1.Focus()
    End Sub
End Class
