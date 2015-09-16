<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="phantomjsNET.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>www.peng8.net</h1>
            <p>
                <asp:Button ID="Button1" runat="server" Text="向phantomjs传送highchart数据生成图表" OnClick="Button1_Click" />
            </p>
            <p>
                <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Height="167px" Width="353px"></asp:TextBox>
               
            </p>
            <p> <asp:Image ID="Image1" runat="server" /></p>
        </div>
    </form>
</body>
</html>
