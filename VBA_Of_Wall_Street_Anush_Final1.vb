'VBA_Of_Wall_Street
'
'This script analyzes stock market data and generates a summary report.
'Each worksheet in the workbook is assumed to contain the following columns:
'1: Ticker, 2: Year, 3: Opening price, 4: High price, 5: Low price, 6: Closing price, 7: Volume
'
'The script adds a new table in columns I to L, summarizing yearly change, percent change, and total volume for each ticker.
'It also highlights positive and negative changes in the Yearly Change column.
'Furthermore, it determines and displays the greatest percent increase, greatest percent decrease, and greatest total volume.
'
'Note: The script does not perform any error handling. Ensure that the worksheets contain the expected columns and that all necessary data is present and correct.

Sub VBA_Of_Wall_Street()

    ' Constants for column indices
    Const TICKER_COL = 1
    Const OPENING_PRICE_COL = 3
    Const CLOSING_PRICE_COL = 6
    Const VOLUME_COL = 7

    ' Iterate over each worksheet
    For Each ws In Worksheets
    
        ' Create new column headers
        ws.Cells(1, 9).Value = "Ticker"  
        ws.Cells(1, 10).Value = "Yearly Change"
        ws.Cells(1, 11).Value = "Percent Change"
        ws.Cells(1, 12).Value = "Total Stock Volume"

        ' Initialize variables
        Dim Ticker As String
        Dim vol_Total As Double: vol_Total = 0
        Dim Summary_Table_Row As Integer: Summary_Table_Row = 2
        Dim Last_Row As Long: Last_Row = ws.Cells(Rows.Count, TICKER_COL).End(xlUp).Row
        Dim open_Price As Double: open_Price = ws.Cells(2, OPENING_PRICE_COL).Value
        Dim close_Price As Double
        Dim Yearly_Change As Double
        Dim Percent_Change As Double

        ' Loop through all tickers
        For i = 2 To Last_Row

            ' If the ticker changes
            If ws.Cells(i + 1, TICKER_COL).Value <> ws.Cells(i, TICKER_COL).Value Then

                ' Set the ticker
                Ticker = ws.Cells(i, TICKER_COL).Value
                
                ' Set the close_Price
                close_Price = ws.Cells(i, CLOSING_PRICE_COL).Value

                ' Calculate vol_Total
                vol_Total = vol_Total + ws.Cells(i, VOLUME_COL).Value

                ' Calculate Yearly_Change
                Yearly_Change = close_Price - open_Price

                ' Calculate Percent_Change
                Percent_Change = Yearly_Change / open_Price

                ' Print the Ticker name to the Summary Table
                ws.Range("I" & Summary_Table_Row).Value = Ticker
                    
                ' Print the Stock Volume to the Summary Table
                ws.Range("L" & Summary_Table_Row).Value = vol_Total

                ' Print the Yearly_Change to the Summary Table
                ws.Range("J" & Summary_Table_Row).Value = Yearly_Change

                ' Print the Percent_Change to the Summary Table
                ws.Range("K" & Summary_Table_Row).Value = FormatPercent(Percent_Change)

                ' Highlight positive/negative change in Yearly_Change
                If Yearly_Change > 0 Then
                    ws.Range("J" & Summary_Table_Row).Interior.Color = RGB(0, 255, 0)
                ElseIf Yearly_Change < 0 Then
                    ws.Range("J" & Summary_Table_Row).Interior.Color = RGB(255, 0, 0)
                End If

                ' Prep open_Price for next ticker
                open_Price = ws.Cells(i + 1, OPENING_PRICE_COL).Value

                ' Prep summary table row for next ticker
                Summary_Table_Row = Summary_Table_Row + 1

                ' Reset the vol_Total
                vol_Total = 0

            ' If no change in ticker
            Else
                ' Add to the vol_Total
                vol_Total = vol_Total + ws.Cells(i, VOLUME_COL).Value
            End If

        Next i

        ' Create new headers
        ws.Cells(1, 16).Value = "Ticker"
        ws.Cells(1, 17).Value = "Value"
        ws.Cells(2, 15).Value = "Greatest % Increase"
        ws.Cells(3, 15).Value = "Greatest % Decrease"
        ws.Cells(4, 15).Value = "Greatest Total Volume"

        ' Initialize variables
        Dim Great_Inc As Double: Great_Inc = 0
        Dim Great_Dec As Double: Great_Dec = 0
        Dim Great_Vol As Double: Great_Vol = 0

        ' Find last row of Ticker Column
        Last_Row = ws.Cells(Rows.Count, 9).End(xlUp).Row

        ' Loop Through Ticker Column
        For i = 2 To Last_Row

            ' Calc and Print Greatest % Increase
            If ws.Cells(i, 11).Value > Great_Inc Then
                Great_Inc = ws.Cells(i, 11).Value
                ws.Range("P2").Value = ws.Cells(i, 9).Value
                ws.Range("Q2").Value = FormatPercent(Great_Inc)
            End If

            ' Calc and Print Greatest % Decrease    
            If ws.Cells(i, 11).Value < Great_Dec Then
                Great_Dec = ws.Cells(i, 11).Value
                ws.Range("P3").Value = ws.Cells(i, 9).Value
                ws.Range("Q3").Value = FormatPercent(Great_Dec)
            End If

            ' Calc and Print Greatest Total Volume
            If ws.Cells(i, 12).Value > Great_Vol Then
                Great_Vol = ws.Cells(i, 12).Value
                ws.Range("P4").Value = ws.Cells(i, 9).Value
                ws.Range("Q4").Value = Great_Vol
            End If

        Next i

        ' Autofit new columns
        ws.Range("I1:Q1").EntireColumn.AutoFit

    Next ws

End Sub

