Public Class LightsOut
    Private onColor As Color = Color.LightGreen
    Private offColor As Color = Color.DarkGreen
    Private gameSize As Integer = 5
    Private lightArr(gameSize, gameSize) As Button
    Public Sub DrawBoard()
        'Create the button objects and draw them onto the window

        Const lightSpacing As Integer = 20
        Const lightSize As Integer = 50

        Const leftOffset As Integer = 3
        Const topOffset As Integer = 50
        'Create buttons for the whole array
        For i As Integer = 0 To gameSize - 1
            For j As Integer = 0 To gameSize - 1
                lightArr(i, j) = New Button
                With lightArr(i, j)
                    .Width = lightSize
                    .Height = lightSize
                    .Name = "btnLightX" & i & "Y" & j
                    'Position each button in a grid
                    .Location = New Point(((lightSize + lightSpacing) * i) + leftOffset, ((lightSize + lightSpacing) * j) + topOffset)
                    .BackColor = offColor
                    'Store the 2D index of the light in the button tag so we can identify them when they are clicked
                    .Tag = {i, j}
                End With
                'Draw the buttons onto the window and add event handlers
                Me.Controls.Add(lightArr(i, j))
                AddHandler lightArr(i, j).Click, AddressOf Me.HandleLightClick
            Next
        Next
        'Size our window for the grid
        Me.AutoSize = True
        Me.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.MaximizeBox = False

    End Sub

    Public Sub HandleLightClick(sender As Object, e As EventArgs)
        'Handles a light button click and checks for winning state

        'Extract 2D index from the event
        Dim btn As Button = sender
        Dim arrPos() As Integer = btn.Tag
        Dim xPos, yPos As Integer
        xPos = arrPos(0)
        yPos = arrPos(1)

        'Flip appropriate lights
        ToggleLight(xPos, yPos)

        'Check if all lights are out
        If CheckWin() Then
            MessageBox.Show("You Win! Try a new puzzle.", "Congratulations!")
            InitialiseGame()
        End If
    End Sub

    Public Function CheckWin()
        Dim result As Boolean = False
        'Check if all buttons on the grid are off (dark) using a LINQ query
        result = lightArr.OfType(Of Button).All(Function(btnTemp) btnTemp.BackColor = offColor)
        Return result
    End Function

    Public Sub ToggleLight(xPos As Integer, yPos As Integer)
        'Change my color
        ChangeButtonColor(xPos, yPos)
        'Check if we are at any of the edges & flip adjacent light colors
        If xPos > 0 Then
            ChangeButtonColor(xPos - 1, yPos)
        End If
        If xPos < gameSize - 1 Then
            ChangeButtonColor(xPos + 1, yPos)
        End If
        If yPos > 0 Then
            ChangeButtonColor(xPos, yPos - 1)
        End If
        If yPos < gameSize - 1 Then
            ChangeButtonColor(xPos, yPos + 1)
        End If
    End Sub
    Private Sub ChangeButtonColor(xPos As Integer, yPos As Integer)
        'Changes the color of a specified button
        With lightArr(xPos, yPos)
            If .BackColor = offColor Then
                .BackColor = onColor
            Else
                .BackColor = offColor
            End If
        End With
    End Sub
    Public Sub InitialiseGame()
        'Set the starting grid of lights
        'To ensure this creates a solvable puzzle, start with all lights off and click random lights

        'Set all lights to off
        For i As Integer = 0 To gameSize - 1
            For j As Integer = 0 To gameSize - 1
                lightArr(i, j).BackColor = offColor
            Next
        Next
        'Create a 1D list of all lights on the board
        Dim numberList = Enumerable.Range(0, (gameSize * gameSize) - 1).ToList()
        Dim randomGenerator As New Random()
        'Click a random number of lights, at least 5 so there is some challenge
        Dim numberOfSwitches As Integer = randomGenerator.Next(5, numberList.Count)
        Dim randomLight, xPos, yPos As Integer
        For i As Integer = 0 To numberOfSwitches
            'Get a random light from the list
            randomLight = randomGenerator.Next(0, numberList.Count)
            'Convert the 1D list index to a 2D grid index and toggle it
            xPos = randomLight Mod gameSize
            yPos = randomLight \ gameSize
            ToggleLight(xPos, yPos)
            'Remove this light from the list so we don't toggle it again
            numberList.RemoveAt(randomLight)
        Next

    End Sub
    Private Sub LightsOut_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DrawBoard()
        InitialiseGame()
    End Sub

    Private Sub btnNewGame_Click(sender As Object, e As EventArgs) Handles btnNewGame.Click
        InitialiseGame()
    End Sub

End Class
