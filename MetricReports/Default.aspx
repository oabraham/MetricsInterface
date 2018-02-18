<%@ Page Title="Metrics Reports" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.vb" Inherits="MetricReports._Default" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content runat="server" ID="FeaturedContent" ContentPlaceHolderID="FeaturedContent">
    <section class="featured">
        <div class="content-wrapper">
            <hgroup class="title">
                <h1><%: Title %>.</h1>
                <h2>MIS Section</h2>
            </hgroup>
            
        </div>
    </section>
</asp:Content>
<asp:Content runat="server" ID="BodyContent" ContentPlaceHolderID="MainContent" >
    <h1>Summary: <%=DateTime.Now%></h1>

    <div style="width:100%" align="center" >

        <asp:GridView ID="grdAccumulations" runat="server" CellPadding="30" Width="100%" cssclass="grid"   >
            
        </asp:GridView>


    </div>
    
    <div style="width:100%;float:left"><h1>Website Statistics:</h1><br /></div>

    <div style="width:100%">

        <iframe style="width:100%" id="iframeStats" height="600px" >


        </iframe>

    </div>

    <br />
    <h1>Payroll Metrics: </h1>
    <h3 id="depurar" runat="server"></h3>
    <div style="float:left">&nbsp;<asp:Chart ID="OnTimeApproval" runat="server" BackColor="" Width="350px"  >
        <series>
            <asp:Series ChartType="Pie"  Name="Series1" Legend="ApprovedOnTime" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" BackColor="">
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="ApprovedOnTime" BackColor="">
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Daily Ontime Approval">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

    <asp:updatepanel runat="server" UpdateMode="Conditional" >
        <ContentTemplate >
        
    <div style="float:left" >

        <asp:Chart ID="OnTimeApprovalDays" runat="server" BackColor="" Width="350px"  >
        <series>
            <asp:Series ChartType="Pie"  Name="Series1" Legend="ApprovedOnTime" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" BackColor="">
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="ApprovedOnTime" BackColor="">
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Account Approval ">
            </asp:Title>
        </Titles>

        </asp:Chart>
     

        <div style="float:left;width:100%" >
            <asp:TextBox runat="server" ID="txtfecha" CssClass="datepic" Visible="false"   Width="100px">Date</asp:TextBox>
            &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlPeriod" runat="server"  style="vertical-align:middle;" AutoPostBack="True" ViewStateMode="Enabled">
                <asp:ListItem Value="0">WTD</asp:ListItem>
                <asp:ListItem Value="1">MTD</asp:ListItem>
                <asp:ListItem Value="2">YTD</asp:ListItem>
                <asp:ListItem Value="3">Arbitrary</asp:ListItem>
            </asp:DropDownList>
            &nbsp; <asp:ImageButton ID="imgbtnsubmitapproval" runat="server" style="vertical-align:middle" ImageUrl="~/Images/Refresh-icon.png" BackColor="#EFEEEF" BorderColor="#EFEEEF" Height="23px" Width="29px"/>
            <br />
            <asp:Label runat="server" Visible="false" ID="lblbaddate" >Invalid Date</asp:Label>
        </div>

    </div>
       
        <script type="text/javascript">
            $(document).ready(function () {
                $('.datepic').datepicker();
                $("#iframeStats").attr('src', 'http://www.mywebreports.net/s0e0k3o7y.html');
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                function EndRequestHandler(sender, args) {
                    $('.datepic').datepicker();
                }

            });
        </script>
    </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="txtfecha" EventName="TextChanged" />
            <asp:AsyncPostBackTrigger ControlID="imgbtnsubmitapproval" EventName="" />
        </Triggers>
     
    </asp:updatepanel>
    
    <div style="float:left">&nbsp;<asp:Chart ID="OntimeSent" runat="server" BackColor="" Width="350px"  >
        <series>
            <asp:Series ChartType="Pie"  Name="Series1" Legend="SentOnTime" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" BackColor="">
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="SentOnTime" BackColor="">
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Daily Ontime Sent">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

    <asp:updatepanel ID="Updatepanel1" runat="server"  UpdateMode="Conditional" >
        <ContentTemplate >
        
    <div style="float:left" >

        <asp:Chart ID="SentOnTimeAll" runat="server" BackColor="" Width="350px"  >
        <series>
            <asp:Series ChartType="Pie"  Name="Series1" Legend="SentOnTime" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" BackColor="">
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="SentOnTime" BackColor="">
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Ontime Sent ">
            </asp:Title>
        </Titles>

        </asp:Chart>
     

        <div style="float:left;width:100%" >
            <asp:TextBox runat="server" ID="txtOntimeSentDate" CssClass="datepic" Visible="false"   Width="100px">Date</asp:TextBox>
            &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlontimesent" runat="server"  style="vertical-align:middle;" AutoPostBack="True" ViewStateMode="Enabled">
                <asp:ListItem Value="0">WTD</asp:ListItem>
                <asp:ListItem Value="1">MTD</asp:ListItem>
                <asp:ListItem Value="2">YTD</asp:ListItem>
                <asp:ListItem Value="3">Arbitrary</asp:ListItem>
            </asp:DropDownList>
            &nbsp; <asp:ImageButton ID="imgbtnrefreshontimesent" runat="server" style="vertical-align:middle" ImageUrl="~/Images/Refresh-icon.png" BackColor="#EFEEEF" BorderColor="#EFEEEF" Height="23px" Width="29px"/>
            <br />
            <asp:Label runat="server" Visible="false" ID="lblinvalidDate2" >Invalid Date</asp:Label>
        </div>

    </div>
       
       
    </ContentTemplate>
        
     
    </asp:updatepanel>

     <div style="float:left">&nbsp;<asp:Chart ID="chartDD" runat="server" BackColor="" Width="350px"  >
        <series>
            <asp:Series ChartType="column"  Name="Series1" Legend="DirectDeposit" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" BackColor="">
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="DirectDeposit" BackColor="">
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Direct Deposits Processed">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

    <div style="float:left">&nbsp;<asp:Chart ID="chartChecksprocs" runat="server" BackColor="" Width="350px"  >
        <series>
            <asp:Series ChartType="column"  Name="Series1" Legend="ChecksProcs" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" BackColor="">
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="ChecksProcs" BackColor="">
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Payroll Checks Processed">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

  <div style="width:100%;float:left"><h1>Login Metrics:</h1><br /></div>
  
    <div style="float:left">
    
        <asp:Chart ID="dailyLoginCount" runat="server"  Width="350px"  height="200px">
        <series>
            <asp:Series ChartType="Bar"  Name="Series1" Legend="DailyLoginCount" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" >
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="DailyLoginCount" >
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Daily Login Count">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

<asp:updatepanel ID="Updatepanel2" runat="server"  UpdateMode="Conditional" >
        <ContentTemplate >
        
    <div style="float:left" >

        <asp:Chart ID="loginbyplatformall" runat="server" BackColor="" Width="350px" height="200px" >
        <series>
            <asp:Series ChartType="Bar"  Name="Series1" Legend="LoginByPlatform" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" BackColor="">
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="LoginByPlatform" BackColor="">
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Login Counts ">
            </asp:Title>
        </Titles>

        </asp:Chart>
     

        <div style="float:left;width:100%" >
            <asp:TextBox runat="server" ID="txtLoginDate" CssClass="datepic" Visible="false"   Width="100px">Date</asp:TextBox>
            &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlranges" runat="server"  style="vertical-align:middle;" AutoPostBack="True" ViewStateMode="Enabled">
                <asp:ListItem Value="0">WTD</asp:ListItem>
                <asp:ListItem Value="1">MTD</asp:ListItem>
                <asp:ListItem Value="2">YTD</asp:ListItem>
                <asp:ListItem Value="3">Arbitrary</asp:ListItem>
            </asp:DropDownList>
            &nbsp; <asp:ImageButton ID="imgbtngetlogins" runat="server" style="vertical-align:middle" ImageUrl="~/Images/Refresh-icon.png" BackColor="#EFEEEF" BorderColor="#EFEEEF" Height="23px" Width="29px"/>
            <br />
            <asp:Label runat="server" Visible="false" ID="lblloginbaddate" >Invalid Date</asp:Label>
        </div>

    </div>
       
        
    </ContentTemplate>
        
     
    </asp:updatepanel>

    <div style="width:100%;float:left"><h1>Reconciliations:</h1><br /></div>

      <div style="float:left">
   
        <asp:Chart ID="chartReconcil" runat="server"  Width="350px"  height="200px">
        <series>
            <asp:Series ChartType="Bar"  Name="Series1" Legend="DailyReconcilCount" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" >
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="DailyReconcilCount" >
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Reconciliated Accounts">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

        
    
<asp:updatepanel ID="Updatepanel3" runat="server"  UpdateMode="Conditional" >
        <ContentTemplate >
        
    <div style="float:left" >

        <asp:Chart ID="reconcilall" runat="server" BackColor="" Width="350px" height="200px" >
        <series>
            <asp:Series ChartType="Bar"  Name="Series1" Legend="AllReconcilsHour" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" BackColor="">
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="AllReconcilsHour" BackColor="">
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Accumulated Reconciliations Before Hour ">
            </asp:Title>
        </Titles>

        </asp:Chart>
     

        <div style="float:left;width:100%" >
            <asp:TextBox runat="server" ID="txtallreconcil" CssClass="datepic" Visible="false"   Width="100px">Date</asp:TextBox>
            &nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddlallreconcil" runat="server"  style="vertical-align:middle;" AutoPostBack="True" ViewStateMode="Enabled">
                <asp:ListItem Value="0">WTD</asp:ListItem>
                <asp:ListItem Value="1">MTD</asp:ListItem>
                <asp:ListItem Value="2">YTD</asp:ListItem>
                <asp:ListItem Value="3">Arbitrary</asp:ListItem>
            </asp:DropDownList>
            &nbsp; <asp:ImageButton ID="imgbtnallreconcil" runat="server" style="vertical-align:middle" ImageUrl="~/Images/Refresh-icon.png" BackColor="#EFEEEF" BorderColor="#EFEEEF" Height="23px" Width="29px"/>
            <br />
            <asp:Label runat="server" Visible="false" ID="lblallreconcil" >Invalid Date</asp:Label>
        </div>

    </div>
       
        
    </ContentTemplate>
        
     
    </asp:updatepanel>

<%--    <div style="width:100%;float:left"><h1>Potential Problems:</h1><br /></div>

    <div style="float:left">
   
        <asp:Chart ID="ppDepositransit" runat="server"  Width="350px"  height="200px">
        <series>
            <asp:Series ChartType="Column"   Name="Series1" Legend="InTransitTrans" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" >
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="InTransitTrans" >
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="# of Transactions In Transit">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>


    <div style="float:left">
   
        <asp:Chart ID="outsEDICount" runat="server"  Width="350px"  height="200px">
        <series>
            <asp:Series ChartType="Column"   Name="Series1" Legend="OutstandingEDICount" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" >
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="OutstandingEDICount" >
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Outstanding EDI Transactions">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

    
    <div style="float:left">
   
        <asp:Chart ID="outsPAVouch" runat="server"  Width="350px"  height="200px">
        <series>
            <asp:Series ChartType="Column"   Name="Series1" Legend="OutStandingPAVouch" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" >
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="OutStandingPAVouch" >
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Outstanding PA Vouchers">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

    <div style="float:left">
   
        <asp:Chart ID="outsUWmatch" runat="server"  Width="350px"  height="200px">
        <series>
            <asp:Series ChartType="Column"   Name="Series1" Legend="OutstandingUWMatch" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" >
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="OutstandingUWMatch" >
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Outstanding UW Matching Per Amount">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

      <div style="float:left">
   
        <asp:Chart ID="outsUWSameChkDiffAmt" runat="server"  Width="350px"  height="200px">
        <series>
            <asp:Series ChartType="Column"   Name="Series1" Legend="OutStandingUW" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" >
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="OutStandingUW" >
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Outstanding UW Same Check With Different Amount">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

   <div style="float:left">
   
        <asp:Chart ID="outsUWSameChkAmt" runat="server"  Width="350px"  height="200px">
        <series>
            <asp:Series ChartType="Column"   Name="Series1" Legend="OutStandingUWSame" ChartArea="ChartArea1">
            </asp:Series>
        </series>
        <chartareas>
            <asp:ChartArea Name="ChartArea1" >
            </asp:ChartArea>
        </chartareas>
        <Legends>
            <asp:Legend Name="OutStandingUWSame" >
            </asp:Legend>
        </Legends>
        <Titles>
            <asp:Title  Name="Title1" Text="Outstanding UW Same Check And Amount">
            </asp:Title>
        </Titles>
        </asp:Chart>
    </div>

    --%>
    </asp:Content>
