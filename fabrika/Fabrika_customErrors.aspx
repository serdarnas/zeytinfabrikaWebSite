<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Fabrika_customErrors.aspx.cs" Inherits="fabrika_Fabrika_customErrors" %>

<!DOCTYPE html>
<html>
<head>
    <title>Hata</title>
</head>
<body>
    <h1>Bir Hata Oluştu</h1>
    <p>Lütfen aşağıdaki hata ID'sini destek ekibine bildirin:</p>
    <p><%= Request.QueryString["errorId"] %></p>
</body>
</html>
