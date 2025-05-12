<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Projelerim.aspx.cs" Inherits="fabrika_Projelerim" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="panel">
        <header class="panel-heading">
            Projelerim
        </header>
        <div class="panel-body">
            <div class="adv-table">
                <table cellpadding="0" cellspacing="0" border="0" class="display table table-bordered" id="hidden-table-info">
                    <thead>
                        <tr>
                            <th>Ad</th>
                            <th class="hidden-phone">Detay</th>
                            <th>Proje Başlama Tarihi</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptProjeler">
                            <ItemTemplate>

                                <tr class="gradeA odd">
                                    <td><a href='ProjeDetay.aspx?id=<%# Eval("ProjeID") %>' style="color: white; text-decoration: none; font-weight: bold;">
                                        <div style="background: #4fc3f7; color: white; border-radius: 6px; padding: 8px 12px; margin-bottom: 4px;">
                                            <%# Eval("Ad") %>
                                        </div>
                                    </a>
                                    </td>
                                    <td class="hidden-phone"><%# Eval("Detay") %></td>
                                    <td><%# (DateTime.Parse(Eval("OlusturmaTarihi").ToString()).ToLongDateString()) %></td>
                                </tr>
                            </ItemTemplate>
                            <AlternatingItemTemplate>
                                <tr class="gradeA even">
                                    <td><a href='ProjeDetay.aspx?id=<%# Eval("ProjeID") %>' style="color: white; text-decoration: none; font-weight: bold;">
                                            <div style="background: #4fc3f7; color: white; border-radius: 6px; padding: 8px 12px; margin-bottom: 4px;">
                                                <%# Eval("Ad") %>
                                            </div>
                                        </a>
                                    </td>
                                    <td class="hidden-phone"><%# Eval("Detay") %></td>
                                    <td><%# (DateTime.Parse(Eval("OlusturmaTarihi").ToString()).ToLongDateString()) %></td>
                                </tr>
                            </AlternatingItemTemplate>
                        </asp:Repeater>

                    </tbody>
                </table>
            </div>
        </div>
    </section>
</asp:Content>

