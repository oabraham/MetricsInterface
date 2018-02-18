Imports System.Data.SqlClient

Public Class _Default
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

        If (Not Page.IsPostBack) Then

            Setup_Tables()
            Get_Payroll_Count_X_days(DateTime.Now.DayOfWeek) 'wtd
            Get_Ontime_Sent_X_days(DateTime.Now.DayOfWeek) 'wtd
            Get_Login_Count_All(DateTime.Now.DayOfWeek, New ListItemCollection) 'wtd
            Dim ar(10) As Double
            Get_Daily_Reconciliation_All(DateTime.Now.DayOfWeek, ar) 'wtd
            Fill_Grids()

        End If

    End Sub

    Public Function Get_Daily_Payroll_Count() As Double

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim porciento As Double
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_daily_approval_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@totalcomp", SqlDbType.Int)
                cmd.Parameters.Add("@approvcount", SqlDbType.Int)
                cmd.Parameters.Add("@percent", SqlDbType.Float)

                cmd.Parameters("@totalcomp").Direction = ParameterDirection.Output
                cmd.Parameters("@approvcount").Direction = ParameterDirection.Output
                cmd.Parameters("@percent").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                porciento = cmd.Parameters("@percent").Value

                OnTimeApproval.Series("Series1").Points.Clear()
                OnTimeApproval.Series("Series1").Points.AddXY("Ontime " & cmd.Parameters("@percent").Value & "%", cmd.Parameters("@approvcount").Value)
                OnTimeApproval.Series("Series1").Points.AddXY("Late Approval", (cmd.Parameters("@totalcomp").Value - cmd.Parameters("@approvcount").Value))
                OnTimeApproval.Titles("Title1").Text = "Account Approval " & cmd.Parameters("@totalcomp").Value & " Accounts"
                OnTimeApproval.DataBind()

            End Using
        End Using

        Return porciento

    End Function

    Public Function Get_Payroll_Count_X_days(ByVal dias As Int32) As Double

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim porciento As Double
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_approval_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@dias", dias)
                cmd.Parameters.Add("@totalcomp", SqlDbType.Int)
                cmd.Parameters.Add("@approvcount", SqlDbType.Int)
                cmd.Parameters.Add("@percent", SqlDbType.Float)

                cmd.Parameters("@dias").Direction = ParameterDirection.Input
                cmd.Parameters("@totalcomp").Direction = ParameterDirection.Output
                cmd.Parameters("@approvcount").Direction = ParameterDirection.Output
                cmd.Parameters("@percent").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                porciento = cmd.Parameters("@percent").Value

                OnTimeApprovalDays.Series("Series1").Points.Clear()
                OnTimeApprovalDays.Series("Series1").Points.AddXY("Ontime " & cmd.Parameters("@percent").Value & "%", cmd.Parameters("@approvcount").Value)
                OnTimeApprovalDays.Series("Series1").Points.AddXY("Late Approval", (cmd.Parameters("@totalcomp").Value - cmd.Parameters("@approvcount").Value))
                OnTimeApprovalDays.Titles("Title1").Text = "Account Approval " & dias & " days ago: " & cmd.Parameters("@totalcomp").Value & " Accounts"
                OnTimeApprovalDays.DataBind()

            End Using
        End Using

        Return porciento

    End Function

    Protected Sub txtfecha_TextChanged(sender As Object, e As EventArgs)

        'ClientScript.RegisterClientScriptBlock(Me.GetType(), "Opendiv", " $(function () { $('#" & txtfecha.ClientID & "').datepicker()   });", True)

    End Sub

    Protected Sub ddlPeriod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPeriod.SelectedIndexChanged

        Dim number As Integer = ddlPeriod.SelectedValue
        lblbaddate.Visible = False

        Select Case number
            Case 0

                txtfecha.Visible = False
                Get_Payroll_Count_X_days(DateTime.Now.DayOfWeek)

            Case 1

                txtfecha.Visible = False
                Get_Payroll_Count_X_days(DateTime.Now.Day)

            Case 2

                txtfecha.Visible = False
                Get_Payroll_Count_X_days(DateTime.Now.DayOfYear)

            Case 3

                txtfecha.Visible = True

                If fecha_valida(txtfecha.Text) Then

                    Dim fecha = DateTime.Parse(txtfecha.Text) 'need date validation 
                    Dim daysbetween As Long = DateDiff(DateInterval.Day, fecha, DateTime.Now)
                    If daysbetween > 0 Then
                        Get_Payroll_Count_X_days(daysbetween)
                    Else
                        lblbaddate.Visible = True
                    End If
                Else

                    If txtfecha.Text <> "Date" Then
                        lblbaddate.Visible = True
                    End If

                End If

            Case Else
                    Debug.WriteLine("Not between 1 and 10, inclusive")
        End Select

    End Sub

    Public Function fecha_valida(ByVal fecha As String) As Boolean

        Dim testfecha As New DateTime
        Return DateTime.TryParse(fecha, testfecha)

    End Function

    Public Function Get_Ontime_Sent() As Double

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim porciento As Double
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_sent_intime_daily_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@totalcomp", SqlDbType.Int)
                cmd.Parameters.Add("@sentcount", SqlDbType.Int)
                cmd.Parameters.Add("@percent", SqlDbType.Float)

                cmd.Parameters("@totalcomp").Direction = ParameterDirection.Output
                cmd.Parameters("@sentcount").Direction = ParameterDirection.Output
                cmd.Parameters("@percent").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                porciento = cmd.Parameters("@percent").Value

                OntimeSent.Series("Series1").Points.Clear()
                OntimeSent.Series("Series1").Points.AddXY("Ontime Sent " & cmd.Parameters("@percent").Value & "%", cmd.Parameters("@sentcount").Value)
                OntimeSent.Series("Series1").Points.AddXY("Sent Late ", (cmd.Parameters("@totalcomp").Value - cmd.Parameters("@sentcount").Value))
                OntimeSent.Titles("Title1").Text = "Accounts processed ontime " & cmd.Parameters("@totalcomp").Value & " Accounts"
                OntimeSent.DataBind()

            End Using
        End Using

        Return porciento

    End Function


    Protected Sub imgbtnsubmitapproval_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnsubmitapproval.Click

        ddlPeriod_SelectedIndexChanged(Me, e)

    End Sub

    Public Function Get_Ontime_Sent_X_days(ByVal dias As Int32) As Double

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim porciento As Double
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_sent_intime_all_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@dias", dias)
                cmd.Parameters.Add("@totalcomp", SqlDbType.Int)
                cmd.Parameters.Add("@sentcount", SqlDbType.Int)
                cmd.Parameters.Add("@percent", SqlDbType.Float)

                cmd.Parameters("@dias").Direction = ParameterDirection.Input
                cmd.Parameters("@totalcomp").Direction = ParameterDirection.Output
                cmd.Parameters("@sentcount").Direction = ParameterDirection.Output
                cmd.Parameters("@percent").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                porciento = cmd.Parameters("@percent").Value

                SentOnTimeAll.Series("Series1").Points.Clear()
                SentOnTimeAll.Series("Series1").Points.AddXY("Ontime " & cmd.Parameters("@percent").Value & "%", cmd.Parameters("@sentcount").Value)
                SentOnTimeAll.Series("Series1").Points.AddXY("Sent Late ", (cmd.Parameters("@totalcomp").Value - cmd.Parameters("@sentcount").Value))
                SentOnTimeAll.Titles("Title1").Text = "Sent Ontime " & dias & " days ago: " & cmd.Parameters("@totalcomp").Value & " Accounts"
                SentOnTimeAll.DataBind()

            End Using
        End Using

        Return porciento

    End Function

    Protected Sub imgbtnrefreshontimesent_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnrefreshontimesent.Click

        ddlontimesent_SelectedIndexChanged(Me, e)

    End Sub

    Protected Sub ddlontimesent_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlontimesent.SelectedIndexChanged

        Dim number As Integer = ddlontimesent.SelectedValue
        lblinvalidDate2.Visible = False

        Select Case number
            Case 0

                txtOntimeSentDate.Visible = False
                Get_Ontime_Sent_X_days(DateTime.Now.DayOfWeek)

            Case 1

                txtOntimeSentDate.Visible = False
                Get_Ontime_Sent_X_days(DateTime.Now.Day)

            Case 2

                txtOntimeSentDate.Visible = False
                Get_Ontime_Sent_X_days(DateTime.Now.DayOfYear)

            Case 3

                txtOntimeSentDate.Visible = True

                If fecha_valida(txtOntimeSentDate.Text) Then

                    Dim fecha = DateTime.Parse(txtOntimeSentDate.Text) 'need date validation 
                    Dim daysbetween As Long = DateDiff(DateInterval.Day, fecha, DateTime.Now)
                    If daysbetween > 0 Then
                        Get_Ontime_Sent_X_days(daysbetween)
                    Else
                        lblinvalidDate2.Visible = True
                    End If
                Else

                    If txtOntimeSentDate.Text <> "Date" Then
                        lblinvalidDate2.Visible = True
                    End If

                End If

            Case Else
                    Debug.WriteLine("Not between 1 and 10, inclusive")
        End Select

    End Sub

    Public Function Get_Daily_Login_Count(ByVal lista As ListItemCollection) As ListItemCollection

        If lista.Count = 0 Then
            lista.Add(New ListItem("alchavoplus", 0))
            lista.Add(New ListItem("alchavolite", 0))
            lista.Add(New ListItem("alchavomobile", 0))
        End If

        dailyLoginCount.Series("Series1").Points.Clear()
        lista(0).Value = by_platform("alchavoplus", "Alchavo Plus ")
        lista(1).Value = by_platform("alchavolite", "Alchavo Lite ")
        lista(2).Value = by_platform("alchavomobile", "Alchavo Mobile")
        dailyLoginCount.Series("Series1").LegendText = "# of Users"
        dailyLoginCount.Titles("Title1").Text = "Daily Login Counts by Platform "
        dailyLoginCount.DataBind()

        Return lista

    End Function

    Public Function by_platform(ByVal plataforma As String, ByVal titulo As String) As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_daily_login_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@sistema", plataforma)
                cmd.Parameters.Add("@nusers", SqlDbType.Int)

                cmd.Parameters("@sistema").Direction = ParameterDirection.Input
                cmd.Parameters("@nusers").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@nusers").Value

                dailyLoginCount.Series("Series1").Points.AddXY(titulo, cmd.Parameters("@nusers").Value)

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Login_Count_All(ByVal dias As Integer, ByVal lista As ListItemCollection) As ListItemCollection

        If lista.Count = 0 Then
            lista.Add(New ListItem("alchavoplus", 0))
            lista.Add(New ListItem("alchavolite", 0))
            lista.Add(New ListItem("alchavomobile", 0))
        End If

        loginbyplatformall.Series("Series1").Points.Clear()
        lista(0).Value = by_platform_all("alchavoplus", "Alchavo Plus ", dias)
        lista(1).Value = by_platform_all("alchavolite", "Alchavo Lite ", dias)
        lista(2).Value = by_platform_all("alchavomobile", "Alchavo Mobile", dias)
        loginbyplatformall.Series("Series1").LegendText = "# of Users"
        loginbyplatformall.Titles("Title1").Text = "Login Counts by Platform: " & dias & " days"
        loginbyplatformall.DataBind()

        Return lista

    End Function

    Public Function by_platform_all(ByVal plataforma As String, ByVal titulo As String, ByVal dias As Integer) As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_daily_login_count_all", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@sistema", plataforma)
                cmd.Parameters.AddWithValue("@dias", dias)
                cmd.Parameters.Add("@nusers", SqlDbType.Int)

                cmd.Parameters("@sistema").Direction = ParameterDirection.Input
                cmd.Parameters("@dias").Direction = ParameterDirection.Input
                cmd.Parameters("@nusers").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@nusers").Value

                loginbyplatformall.Series("Series1").Points.AddXY(titulo, cmd.Parameters("@nusers").Value)

            End Using
        End Using

        Return conteo

    End Function

    Protected Sub ddlranges_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlranges.SelectedIndexChanged

        Dim number As Integer = ddlranges.SelectedValue
        lblloginbaddate.Visible = False

        Select Case number
            Case 0

                txtLoginDate.Visible = False
                Get_Login_Count_All(DateTime.Now.DayOfWeek, New ListItemCollection)

            Case 1

                txtLoginDate.Visible = False
                Get_Login_Count_All(DateTime.Now.Day, New ListItemCollection)

            Case 2

                txtLoginDate.Visible = False
                Get_Login_Count_All(DateTime.Now.DayOfYear, New ListItemCollection)

            Case 3

                txtLoginDate.Visible = True

                If fecha_valida(txtLoginDate.Text) Then

                    Dim fecha = DateTime.Parse(txtLoginDate.Text) 'need date validation 
                    Dim daysbetween As Long = DateDiff(DateInterval.Day, fecha, DateTime.Now)
                    If daysbetween > 0 Then
                        Get_Login_Count_All(daysbetween, New ListItemCollection)
                    Else
                        lblloginbaddate.Visible = True
                    End If
                Else

                    If txtLoginDate.Text <> "Date" Then
                        lblloginbaddate.Visible = True
                    End If

                End If

            Case Else
                Debug.WriteLine("Not between 1 and 10, inclusive")
        End Select

    End Sub

    Protected Sub imgbtngetlogins_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtngetlogins.Click

        ddlranges_SelectedIndexChanged(Me, e)

    End Sub

    Public Function Get_Daily_Reconciliation(ByRef porcarray() As Double) As Array

        Dim arreglo(3) As Integer
        Dim porciento As Double = 0
        chartReconcil.Series("Series1").Points.Clear()
        arreglo(0) = by_hour(9, porciento)
        porcarray(0) = porciento
        arreglo(1) = by_hour(10, porciento)
        porcarray(1) = porciento
        arreglo(2) = by_hour(11, porciento)
        porcarray(2) = porciento
        arreglo(3) = by_hour(17, porciento)
        porcarray(3) = porciento
        chartReconcil.Series("Series1").LegendText = "# of Accounts"
        chartReconcil.Titles("Title1").Text = "Daily Reconciliated Accounts Before Hour "
        chartReconcil.DataBind()

        Return arreglo

    End Function

    Public Function by_hour(ByVal hora As Integer, ByRef porciento As Double) As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_num_accounts_date_time_all_daily", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@hora", hora)
                cmd.Parameters.AddWithValue("@proc", 1) '1 has been proc 0 needs to be reconc
                cmd.Parameters.Add("@naccounts", SqlDbType.Int)
                cmd.Parameters.Add("@totalaccs", SqlDbType.Int)
                cmd.Parameters.Add("@percent", SqlDbType.Float)

                cmd.Parameters("@hora").Direction = ParameterDirection.Input
                cmd.Parameters("@proc").Direction = ParameterDirection.Input
                cmd.Parameters("@naccounts").Direction = ParameterDirection.Output
                cmd.Parameters("@totalaccs").Direction = ParameterDirection.Output
                cmd.Parameters("@percent").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@naccounts").Value
                porciento = cmd.Parameters("@percent").Value

                chartReconcil.Series("Series1").Points.AddXY(" " & hora & ":00 " & cmd.Parameters("@percent").Value & "%", cmd.Parameters("@naccounts").Value)

            End Using
        End Using

        Return conteo

    End Function

    Protected Sub imgbtnallreconcil_Click(sender As Object, e As ImageClickEventArgs) Handles imgbtnallreconcil.Click

        ddlallreconcil_SelectedIndexChanged(Me, e)

    End Sub

    Public Function Get_Daily_Reconciliation_All(ByVal dias As Integer, ByRef porcarray() As Double) As Array

        Dim arreglo(3) As Integer
        Dim porciento As Double
        reconcilall.Series("Series1").Points.Clear()
        arreglo(0) = by_hour_all(9, dias, porciento)
        porcarray(0) = porciento
        arreglo(1) = by_hour_all(10, dias, porciento)
        porcarray(1) = porciento
        arreglo(2) = by_hour_all(11, dias, porciento)
        porcarray(2) = porciento
        arreglo(3) = by_hour_all(17, dias, porciento)
        porcarray(3) = porciento
        reconcilall.Series("Series1").LegendText = "# of Accounts"
        reconcilall.Titles("Title1").Text = "Reconciliated Accounts Before Hour: " & dias & " days"
        reconcilall.DataBind()

        Return arreglo

    End Function

    Public Function by_hour_all(ByVal hora As Integer, ByVal dias As Integer, ByRef porciento As Double) As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_num_accounts_date_time_all", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@hora", hora)
                cmd.Parameters.AddWithValue("@dias", dias)
                cmd.Parameters.AddWithValue("@proc", 1)
                cmd.Parameters.Add("@totalaccs", SqlDbType.Int)
                cmd.Parameters.Add("@percent", SqlDbType.Float)
                cmd.Parameters.Add("@ytd", SqlDbType.Int)

                cmd.Parameters("@hora").Direction = ParameterDirection.Input
                cmd.Parameters("@dias").Direction = ParameterDirection.Input
                cmd.Parameters("@proc").Direction = ParameterDirection.Input
                cmd.Parameters("@totalaccs").Direction = ParameterDirection.Output
                cmd.Parameters("@percent").Direction = ParameterDirection.Output
                cmd.Parameters("@ytd").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@ytd").Value
                porciento = cmd.Parameters("@percent").Value

                reconcilall.Series("Series1").Points.AddXY(" " & hora & ":00 " & cmd.Parameters("@percent").Value & "%", cmd.Parameters("@ytd").Value)

            End Using
        End Using

        Return conteo

    End Function

    Protected Sub ddlallreconcil_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlallreconcil.SelectedIndexChanged

        Dim number As Integer = ddlallreconcil.SelectedValue
        lblallreconcil.Visible = False
        Dim ar(10) As Double

        Select Case number
            Case 0

                txtallreconcil.Visible = False
                Get_Daily_Reconciliation_All(DateTime.Now.DayOfWeek, ar)

            Case 1

                txtallreconcil.Visible = False
                Get_Daily_Reconciliation_All(DateTime.Now.Day, ar)

            Case 2

                txtallreconcil.Visible = False
                Get_Daily_Reconciliation_All(DateTime.Now.DayOfYear, ar)

            Case 3

                txtallreconcil.Visible = True

                If fecha_valida(txtallreconcil.Text) Then

                    Dim fecha = DateTime.Parse(txtallreconcil.Text) 'need date validation 
                    Dim daysbetween As Long = DateDiff(DateInterval.Day, fecha, DateTime.Now)
                    If daysbetween > 0 Then
                        Get_Daily_Reconciliation_All(daysbetween, ar)
                    Else
                        lblallreconcil.Visible = True
                    End If
                Else

                    If txtallreconcil.Text <> "Date" Then
                        lblallreconcil.Visible = True
                    End If

                End If

            Case Else
                Debug.WriteLine("Not between 1 and 10, inclusive")
        End Select

    End Sub

    Public Function Get_Trans_In_Transit() As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_deposit_in_transit", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@dias", 3)
                cmd.Parameters.Add("@nppstatus", SqlDbType.Int)

                cmd.Parameters("@dias").Direction = ParameterDirection.Input
                cmd.Parameters("@nppstatus").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@nppstatus").Value

                'ppDepositransit.Series("Series1").Points.Clear()
                'ppDepositransit.Series("Series1").Points.AddXY("# of Transactions ", cmd.Parameters("@nppstatus").Value)
                'ppDepositransit.Titles("Title1").Text = "Deposits In Transit : " & cmd.Parameters("@nppstatus").Value
                'ppDepositransit.DataBind()

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Outstanding_EDI_Count() As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_outs_edi_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@dias", 3)
                cmd.Parameters.Add("@nouts", SqlDbType.Int)

                cmd.Parameters("@dias").Direction = ParameterDirection.Input
                cmd.Parameters("@nouts").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@nouts").Value

                'outsEDICount.Series("Series1").Points.Clear()
                'outsEDICount.Series("Series1").Points.AddXY("# of Transactions ", cmd.Parameters("@nouts").Value)
                'outsEDICount.Titles("Title1").Text = "Outstanding EDI Transactions : " & cmd.Parameters("@nouts").Value
                'outsEDICount.DataBind()

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Outstanding_PA_Voucher_With_Out_Clear() As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_outs_PA_voucher_withoutclear_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@dias", 3)
                cmd.Parameters.Add("@nstatus", SqlDbType.Int)

                cmd.Parameters("@dias").Direction = ParameterDirection.Input
                cmd.Parameters("@nstatus").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@nstatus").Value

                'outsPAVouch.Series("Series1").Points.Clear()
                'outsPAVouch.Series("Series1").Points.AddXY("# of Transactions ", cmd.Parameters("@nstatus").Value)
                'outsPAVouch.Titles("Title1").Text = "Outstanding PA Voucher Without Clear: " & cmd.Parameters("@nstatus").Value
                'outsPAVouch.DataBind()

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Outstanding_UW_Matching_Per_Amount() As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_outs_UW_mathcingPerAmount_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@nstatus", SqlDbType.Int)

                cmd.Parameters("@nstatus").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@nstatus").Value

                'outsUWmatch.Series("Series1").Points.Clear()
                'outsUWmatch.Series("Series1").Points.AddXY("# of Transactions ", cmd.Parameters("@nstatus").Value)
                'outsUWmatch.Titles("Title1").Text = "Outstanding UW Matching Per Amount: " & cmd.Parameters("@nstatus").Value
                'outsUWmatch.DataBind()

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Outstanding_UW_Same_Check_With_Different_Amount() As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_Outst_UW_SameCheckNum_DiffAmount_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@nstatus", SqlDbType.Int)

                cmd.Parameters("@nstatus").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                conteo = cmd.Parameters("@nstatus").Value
                'outsUWSameChkDiffAmt.Series("Series1").Points.Clear()
                'outsUWSameChkDiffAmt.Series("Series1").Points.AddXY("# of Transactions ", cmd.Parameters("@nstatus").Value)
                'outsUWSameChkDiffAmt.Titles("Title1").Text = "Outstanding UW Same Check Different Amount: " & cmd.Parameters("@nstatus").Value
                'outsUWSameChkDiffAmt.DataBind()

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Outstanding_UW_Same_Check_And_Amount() As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_outstanding_UW_chk_amount_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@nouts", SqlDbType.Int)

                cmd.Parameters("@nouts").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@nouts").Value

                'outsUWSameChkAmt.Series("Series1").Points.Clear()
                'outsUWSameChkAmt.Series("Series1").Points.AddXY("# of Transactions ", cmd.Parameters("@nouts").Value)
                'outsUWSameChkAmt.Titles("Title1").Text = "Outstanding UW Same Check And Amount: " & cmd.Parameters("@nouts").Value
                'outsUWSameChkAmt.DataBind()

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_AC2_ClientDashReconciliationsToMake() As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_reconciliations_todo", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@conteo", SqlDbType.Int)

                cmd.Parameters("@conteo").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@conteo").Value

            End Using
        End Using

        Return conteo

    End Function

    Public AccummDT As New DataTable


    Public Sub Setup_Tables()

        Dim fila As DataRow
        Dim day_before As DateTime = DateAdd(DateInterval.Day, -1, DateTime.Now)

        AccummDT.Columns.Add(New DataColumn("Description", System.Type.GetType("System.String")))
        AccummDT.Columns.Add(New DataColumn("Current Day", System.Type.GetType("System.String")))
        AccummDT.Columns.Add(New DataColumn("Day Before", System.Type.GetType("System.String")))
        AccummDT.Columns.Add(New DataColumn("Week To Date", System.Type.GetType("System.String")))
        AccummDT.Columns.Add(New DataColumn("Month To Date", System.Type.GetType("System.String")))
        AccummDT.Columns.Add(New DataColumn("Year To Date", System.Type.GetType("System.String")))


        fila = AccummDT.NewRow()
        fila(0) = "-Payroll"
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "Ontime Client Approval"
        fila(1) = "" & Get_Daily_Payroll_Count() & "%"
        fila(2) = Get_Payroll_Approval_Count_By_Date(day_before) & "%"
        fila(3) = "" & Get_Payroll_Count_X_days(DateTime.Now.DayOfWeek) & "%" 'wtd
        fila(4) = "" & Get_Payroll_Count_X_days(DateTime.Now.Day) & "%" 'mtd
        fila(5) = "" & Get_Payroll_Count_X_days(DateTime.Now.DayOfYear) & "%" 'mtd
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "Sent To Bank Ontime"
        fila(1) = "" & Get_Ontime_Sent() & "%"
        fila(2) = Get_Sent_Intime_Count_By_Date(day_before) & "%"
        fila(3) = "" & Get_Ontime_Sent_X_days(DateTime.Now.DayOfWeek) & "%" 'wtd
        fila(4) = "" & Get_Ontime_Sent_X_days(DateTime.Now.Day) & "%" 'mtd
        fila(5) = "" & Get_Ontime_Sent_X_days(DateTime.Now.DayOfYear) & "%" 'wtd
        AccummDT.Rows.Add(fila)

        Dim ddDaily = Get_Direct_Deposit_By_Date(DateTime.Now)
        Dim ddDate = Get_Direct_Deposit_By_Date(day_before)
        Dim ddWtd = Get_Direct_Deposit_X_Days(DateTime.Now.DayOfWeek)
        Dim ddMtd = Get_Direct_Deposit_X_Days(DateTime.Now.Day)
        Dim ddYtd = Get_Direct_Deposit_X_Days(DateTime.Now.DayOfYear)
        fila = AccummDT.NewRow()
        fila(0) = "Direct Deposits Count"
        fila(1) = ddDaily
        fila(2) = ddDate
        fila(3) = ddWtd
        fila(4) = ddMtd
        fila(5) = ddYtd
        AccummDT.Rows.Add(fila)

        chartDD.Series("Series1").Points.Clear()
        chartDD.Series("Series1").Points.AddXY("Daily  ", ddDaily)
        chartDD.Series("Series1").Points.AddXY("Day Before  ", ddDate)
        chartDD.Series("Series1").Points.AddXY("WTD  ", ddWtd)
        chartDD.Series("Series1").Points.AddXY("MTD  ", ddMtd)
        chartDD.Series("Series1").Points.AddXY("YTD  ", ddYtd)
        chartDD.DataBind()

        Dim ckDaily, ckDate, ckWtd, ckMtd, ckYtd As Integer
        ckDaily = Get_Check_Count_By_Date(DateTime.Now)
        ckDate = Get_Check_Count_By_Date(day_before)
        ckWtd = Get_Check_Count_X_Days(DateTime.Now.DayOfWeek)
        ckMtd = Get_Check_Count_X_Days(DateTime.Now.Day)
        ckYtd = Get_Check_Count_X_Days(DateTime.Now.DayOfYear)
        fila = AccummDT.NewRow()
        fila(0) = "Payroll Checks Count"
        fila(1) = ckDaily
        fila(2) = ckDate
        fila(3) = ckWtd
        fila(4) = ckMtd
        fila(5) = ckYtd
        AccummDT.Rows.Add(fila)

        chartChecksprocs.Series("Series1").Points.Clear()
        chartChecksprocs.Series("Series1").Points.AddXY("Daily  ", ckDaily)
        chartChecksprocs.Series("Series1").Points.AddXY("Day Before  ", ckDate)
        chartChecksprocs.Series("Series1").Points.AddXY("WTD  ", ckWtd)
        chartChecksprocs.Series("Series1").Points.AddXY("MTD  ", ckMtd)
        chartChecksprocs.Series("Series1").Points.AddXY("YTD  ", ckYtd)
        chartChecksprocs.DataBind()

        fila = AccummDT.NewRow()
        fila(0) = "  "
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "-Client Logins"
        AccummDT.Rows.Add(fila)

        Dim lista_plataformas As New ListItemCollection
        lista_plataformas.Add(New ListItem("alchavoplus", 0))
        lista_plataformas.Add(New ListItem("alchavolite", 0))
        lista_plataformas.Add(New ListItem("alchavomobile", 0))

        fila = AccummDT.NewRow()
        fila(0) = "Alchavo Plus"
        fila(1) = Get_Daily_Login_Count(lista_plataformas)(0).Value
        fila(2) = Get_Login_Count_By_Date("alchavoplus", day_before)
        fila(3) = Get_Login_Count_All(DateTime.Now.DayOfWeek, lista_plataformas)(0).Value 'wtd
        fila(4) = Get_Login_Count_All(DateTime.Now.Day, lista_plataformas)(0).Value 'mtd
        fila(5) = Get_Login_Count_All(DateTime.Now.DayOfYear, lista_plataformas)(0).Value 'wtd
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "Alchavo Lite"
        fila(1) = Get_Daily_Login_Count(lista_plataformas)(1).Value 'daily
        fila(2) = Get_Login_Count_By_Date("alchavolite", day_before)
        fila(3) = Get_Login_Count_All(DateTime.Now.DayOfWeek, lista_plataformas)(1).Value 'wtd
        fila(4) = Get_Login_Count_All(DateTime.Now.Day, lista_plataformas)(1).Value 'mtd
        fila(5) = Get_Login_Count_All(DateTime.Now.DayOfYear, lista_plataformas)(1).Value 'ytd
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "Alchavo Mobile"
        fila(1) = Get_Daily_Login_Count(lista_plataformas)(2).Value 'daily
        fila(2) = Get_Login_Count_By_Date("alchavomobile", day_before)
        fila(3) = Get_Login_Count_All(DateTime.Now.DayOfWeek, lista_plataformas)(2).Value 'wtd
        fila(4) = Get_Login_Count_All(DateTime.Now.Day, lista_plataformas)(2).Value 'mtd
        fila(5) = Get_Login_Count_All(DateTime.Now.DayOfYear, lista_plataformas)(2).Value 'ytd
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "  "
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "-Reconciliations Before Hour (" & Get_AC2_ClientDashReconciliationsToMake() & " left )"
        AccummDT.Rows.Add(fila)

        Dim porcarray(3) As Double
        Dim resultsar(1) As Double

        fila = AccummDT.NewRow()

        fila(0) = "9 AM"
        fila(1) = Get_Daily_Reconciliation(porcarray)(0) & " (" & porcarray(0) & "%)"
        resultsar = Get_Reconcils_By_Date(day_before, 9)
        fila(2) = resultsar(0) & " (" & resultsar(1) & "%)"
        fila(3) = Get_Daily_Reconciliation_All(DateTime.Now.DayOfWeek, porcarray)(0) & " (" & porcarray(0) & "%)"
        fila(4) = Get_Daily_Reconciliation_All(DateTime.Now.Day, porcarray)(0) & " (" & porcarray(0) & "%)"
        fila(5) = Get_Daily_Reconciliation_All(DateTime.Now.DayOfYear, porcarray)(0) & " (" & porcarray(0) & "%)"
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "10 AM"
        fila(1) = Get_Daily_Reconciliation(porcarray)(1) & " (" & porcarray(1) & "%)"
        resultsar = Get_Reconcils_By_Date(day_before, 10)
        fila(2) = resultsar(0) & " (" & resultsar(1) & "%)"
        fila(3) = Get_Daily_Reconciliation_All(DateTime.Now.DayOfWeek, porcarray)(1) & " (" & porcarray(1) & "%)"
        fila(4) = Get_Daily_Reconciliation_All(DateTime.Now.Day, porcarray)(1) & " (" & porcarray(1) & "%)"
        fila(5) = Get_Daily_Reconciliation_All(DateTime.Now.DayOfYear, porcarray)(1) & " (" & porcarray(1) & "%)"
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "11 AM"
        fila(1) = Get_Daily_Reconciliation(porcarray)(2) & " (" & porcarray(2) & "%)"
        resultsar = Get_Reconcils_By_Date(day_before, 11)
        fila(2) = resultsar(0) & " (" & resultsar(1) & "%)"
        fila(3) = Get_Daily_Reconciliation_All(DateTime.Now.DayOfWeek, porcarray)(2) & " (" & porcarray(2) & "%)"
        fila(4) = Get_Daily_Reconciliation_All(DateTime.Now.Day, porcarray)(2) & " (" & porcarray(2) & "%)"
        fila(5) = Get_Daily_Reconciliation_All(DateTime.Now.DayOfYear, porcarray)(2) & " (" & porcarray(2) & "%)"
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "5 PM"
        fila(1) = Get_Daily_Reconciliation(porcarray)(3) & " (" & porcarray(3) & "%)"
        resultsar = Get_Reconcils_By_Date(day_before, 17)
        fila(2) = resultsar(0) & " (" & resultsar(1) & "%)"
        fila(3) = Get_Daily_Reconciliation_All(DateTime.Now.DayOfWeek, porcarray)(3) & " (" & porcarray(3) & "%)"
        fila(4) = Get_Daily_Reconciliation_All(DateTime.Now.Day, porcarray)(3) & " (" & porcarray(3) & "%)"
        fila(5) = Get_Daily_Reconciliation_All(DateTime.Now.DayOfYear, porcarray)(3) & " (" & porcarray(3) & "%)"
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "  "
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "-Potential Problems"
        fila(1) = "Count"
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "Transactions In Transit 3 Days"
        fila(1) = Get_Trans_In_Transit()
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "Outstanding EDI Transactions"
        fila(1) = Get_Outstanding_EDI_Count()
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "Outstanding PA Voucher Without Clear"
        fila(1) = Get_Outstanding_PA_Voucher_With_Out_Clear()
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "Outstanding UW Matching per Amount"
        fila(1) = Get_Outstanding_UW_Matching_Per_Amount()
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "Outstanding UW Same Check With Different Amount"
        fila(1) = Get_Outstanding_UW_Same_Check_With_Different_Amount()
        AccummDT.Rows.Add(fila)

        fila = AccummDT.NewRow()
        fila(0) = "Outstanding UW Same Check and Amount"
        fila(1) = Get_Outstanding_UW_Same_Check_And_Amount()
        AccummDT.Rows.Add(fila)

    End Sub

    Public Sub Fill_Grids()

        grdAccumulations.DataSource = AccummDT
        grdAccumulations.DataBind()

        For Each fila As GridViewRow In grdAccumulations.Rows

            fila.Cells(0).ForeColor = Drawing.Color.SteelBlue
            For counter As Integer = 1 To fila.Cells.Count - 1

                fila.Cells(counter).HorizontalAlign = HorizontalAlign.Center

            Next

            fila.Cells(0).Font.Bold = True
            fila.Cells(0).Width = New Unit("300px")
        Next

        grdAccumulations.HeaderRow.Cells(0).HorizontalAlign = HorizontalAlign.Right
        AccummDT.Dispose()

    End Sub

    Public Function Get_Payroll_Approval_Count_By_Date(ByVal fecha As DateTime) As Double

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim porciento As Double
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_approval_by_date_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@fecha", fecha)
                cmd.Parameters.Add("@totalcomp", SqlDbType.Int)
                cmd.Parameters.Add("@approvcount", SqlDbType.Int)
                cmd.Parameters.Add("@percent", SqlDbType.Float)

                cmd.Parameters("@fecha").Direction = ParameterDirection.Input
                cmd.Parameters("@totalcomp").Direction = ParameterDirection.Output
                cmd.Parameters("@approvcount").Direction = ParameterDirection.Output
                cmd.Parameters("@percent").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                porciento = cmd.Parameters("@percent").Value

            End Using
        End Using

        Return porciento

    End Function

    Public Function Get_Sent_Intime_Count_By_Date(ByVal fecha As DateTime) As Double

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim porciento As Double
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_sent_intime_by_date_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@fecha", fecha)
                cmd.Parameters.Add("@totalcomp", SqlDbType.Int)
                cmd.Parameters.Add("@sentcount", SqlDbType.Int)
                cmd.Parameters.Add("@percent", SqlDbType.Float)

                cmd.Parameters("@fecha").Direction = ParameterDirection.Input
                cmd.Parameters("@totalcomp").Direction = ParameterDirection.Output
                cmd.Parameters("@sentcount").Direction = ParameterDirection.Output
                cmd.Parameters("@percent").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                porciento = cmd.Parameters("@percent").Value

            End Using
        End Using

        Return porciento

    End Function

    Public Function Get_Login_Count_By_Date(ByVal sistema As String, ByVal fecha As DateTime) As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_login_count_by_date", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@sistema", sistema)
                cmd.Parameters.AddWithValue("@fecha", fecha)
                cmd.Parameters.Add("@nusers", SqlDbType.Int)

                cmd.Parameters("@fecha").Direction = ParameterDirection.Input
                cmd.Parameters("@sistema").Direction = ParameterDirection.Input
                cmd.Parameters("@nusers").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@nusers").Value

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Reconcils_By_Date(ByVal fecha As DateTime, ByVal hora As Integer) As Double()

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo(1) As Double
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_num_accounts_reconcil_by_date", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@hora", hora)
                cmd.Parameters.AddWithValue("@proc", 1)
                cmd.Parameters.AddWithValue("@fecha", fecha)
                cmd.Parameters.Add("@naccounts", SqlDbType.Int)
                cmd.Parameters.Add("@percent", SqlDbType.Float)
                cmd.Parameters.Add("@totalaccs", SqlDbType.Int)

                cmd.Parameters("@fecha").Direction = ParameterDirection.Input
                cmd.Parameters("@hora").Direction = ParameterDirection.Input
                cmd.Parameters("@proc").Direction = ParameterDirection.Input
                cmd.Parameters("@naccounts").Direction = ParameterDirection.Output
                cmd.Parameters("@percent").Direction = ParameterDirection.Output
                cmd.Parameters("@totalaccs").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo(0) = cmd.Parameters("@naccounts").Value
                conteo(1) = cmd.Parameters("@percent").Value

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Direct_Deposit_By_Date(ByVal fecha As DateTime) As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_by_date_dd_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@totalcomp", SqlDbType.Int)
                cmd.Parameters.AddWithValue("@fecha", fecha)

                cmd.Parameters("@totalcomp").Direction = ParameterDirection.Output
                cmd.Parameters("@fecha").Direction = ParameterDirection.Input

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@totalcomp").Value

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Direct_Deposit_X_Days(ByVal dias As Integer) As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_x_days_dd_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@dias", dias)
                cmd.Parameters.Add("@totalcomp", SqlDbType.Int)

                cmd.Parameters("@dias").Direction = ParameterDirection.Input
                cmd.Parameters("@totalcomp").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@totalcomp").Value

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Check_Count_By_Date(ByVal fecha As DateTime) As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_by_date_check_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.Add("@totalcomp", SqlDbType.Int)
                cmd.Parameters.AddWithValue("@fecha", fecha)

                cmd.Parameters("@totalcomp").Direction = ParameterDirection.Output
                cmd.Parameters("@fecha").Direction = ParameterDirection.Input

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@totalcomp").Value

            End Using
        End Using

        Return conteo

    End Function

    Public Function Get_Check_Count_X_Days(ByVal dias As Integer) As Integer

        Dim constring As String = ConfigurationManager.ConnectionStrings("BPOConnectionString").ConnectionString
        Dim conteo As Integer
        Using con As New SqlConnection(constring)
            Using cmd As New SqlCommand("qryMTget_x_days_check_count", con)

                cmd.CommandType = CommandType.StoredProcedure
                cmd.Parameters.AddWithValue("@dias", dias)
                cmd.Parameters.Add("@totalcomp", SqlDbType.Int)

                cmd.Parameters("@dias").Direction = ParameterDirection.Input
                cmd.Parameters("@totalcomp").Direction = ParameterDirection.Output

                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()

                conteo = cmd.Parameters("@totalcomp").Value

            End Using
        End Using

        Return conteo

    End Function


End Class