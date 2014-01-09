<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="DispalyTable.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    
    <div>
        <asp:Label runat="server">Filtru pe interval de timp</asp:Label>
        <asp:DropDownList runat="server" AutoPostBack="true" ID="StartDate" OnSelectedIndexChanged="StarDate_SelectedIndexChanged"></asp:DropDownList>
        <asp:DropDownList runat="server" ID="EndDate"></asp:DropDownList>
        <asp:Button runat="server" ID="Bttn" OnClick="DateFilterClick" Text="Filter"/>
    </div>
    <div>
        <asp:Label runat="server">Filtru pe adresa IP lista</asp:Label>
        <asp:DropDownList runat="server" ID="IpSelect"></asp:DropDownList>
        <asp:Button runat="server" ID="IpFilter" OnClick="IpFilter_Click" Text="Filter" />
    </div>    
    <div>
        <asp:Label runat="server">Filtru pe adresa de retea sau adresa IP specifica</asp:Label>
        <asp:TextBox runat="server" ID="IpText"></asp:TextBox>
        <asp:Button runat="server" ID="IpTextFilter" Text="Filter" OnClick="IpTextFilter_Click"/>
        <asp:Label runat="server" ID="Label1"></asp:Label>
    </div>
    <div>
        <asp:DataGrid runat="server" ID="WebLogDataGrid" AllowSorting="true" >
        </asp:DataGrid>
 
    </div>
    </form>
</body>
</html>
