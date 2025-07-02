<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="fabrika_Depo_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <section class="panel">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-3">
                    <asp:Button ID="btnYeniDepo" runat="server" CssClass="btn btn-shadow btn-success btn-block" Text="+ Yeni Depo Ekle" OnClick="btnYeniDepo_Click" />
                </div>
                <div class="col-md-3">
                    <asp:Button ID="btnDepoRaporu" runat="server" CssClass="btn btn-shadow btn-info btn-block" Text="Depo Raporları" OnClick="btnDepoRaporu_Click" />
                </div>
                <div class="col-md-3"></div>
                <div class="col-md-3 text-right">
                    <span class="badge badge-success" style="font-size: 18px;">
                        <asp:Label ID="lblToplamDepo" runat="server" Text="0"></asp:Label>
                        <i class="fa fa-warehouse"></i> Toplam Depo
                    </span>
                </div>
            </div>
        </div>
    </section>

    <section class="panel">
        <div class="panel-body">
            <div class="col-lg-12">
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlDurum" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDurum_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Aktif Depolar" Value="1" />
                        <asp:ListItem Text="Pasif Depolar" Value="0" />
                        <asp:ListItem Text="Tüm Depolar" Value="" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlKapasiteFiltresi" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlKapasiteFiltresi_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Tüm Kapasiteler" Value="" />
                        <asp:ListItem Text="Küçük (0-1000)" Value="1" />
                        <asp:ListItem Text="Orta (1000-5000)" Value="2" />
                        <asp:ListItem Text="Büyük (5000+)" Value="3" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <asp:DropDownList ID="ddlDolulukOrani" runat="server" CssClass="form-control" OnSelectedIndexChanged="ddlDolulukOrani_SelectedIndexChanged" AutoPostBack="true">
                        <asp:ListItem Text="Tüm Doluluk Oranları" Value="" />
                        <asp:ListItem Text="Boş (%0-25)" Value="1" />
                        <asp:ListItem Text="Az Dolu (%25-50)" Value="2" />
                        <asp:ListItem Text="Yarı Dolu (%50-75)" Value="3" />
                        <asp:ListItem Text="Dolu (%75-100)" Value="4" />
                    </asp:DropDownList>
                </div>
                <div class="col-md-3">
                    <div class="input-group">
                        <asp:TextBox ID="txtArama" runat="server" CssClass="form-control" placeholder="Depo adı veya kodu..." AutoPostBack="True" OnTextChanged="txtArama_TextChanged" />
                    </div>
                </div>
            </div>
        </div>
    </section>

    <section class="panel">
        <div class="panel-body">
            <div class="table-responsive">
                <table class="table table-bordered table-striped">
                    <thead class="thead-dark">
                        <tr>
                            <th>Depo Adı</th>
                            <th>Depo Kodu</th>
                            <th>Kapasite (Kg)</th>
                            <th>Dolu Miktar (Kg)</th>
                            <th>Doluluk Oranı</th>
                            <th>Boş Kapasite</th>
                            <th>Durum</th>
                            <th>İşlemler</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptDepolar" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <td>
                                        <div style="border-radius: 6px; padding: 8px 12px; margin-bottom: 4px;">
                                            <a href='DepoDetay.aspx?id=<%# Eval("DepoID") %>' style="text-decoration: none; font-weight: bold;">
                                                <%# Eval("DepoAdi") %>
                                            </a>
                                        </div>
                                    </td>
                                    <td>
                                        <span style="font-weight: bold; color: #007bff;"><%# Eval("DepoKodu") %></span>
                                    </td>
                                    <td>
                                        <span style="font-weight: bold;"><%# Eval("Kapasite", "{0:N2}") %></span>
                                    </td>
                                    <td>
                                        <span style="font-weight: bold;"><%# Eval("DoluMiktar", "{0:N2}") %></span>
                                    </td>
                                    <td>
                                        <div class="progress" style="height: 20px;">
                                            <div class="progress-bar <%# GetProgressBarClass(Convert.ToDecimal(Eval("DolulukOrani"))) %>" 
                                                 role="progressbar" 
                                                 style="width: <%# Eval("DolulukOrani", "{0:N1}") %>%"
                                                 aria-valuenow="<%# Eval("DolulukOrani", "{0:N1}") %>" 
                                                 aria-valuemin="0" 
                                                 aria-valuemax="100">
                                                <%# Eval("DolulukOrani", "{0:N1}") %>%
                                            </div>
                                        </div>
                                    </td>
                                    <td>
                                        <span style="font-weight: bold; color: #28a745;"><%# Eval("BosKapasite", "{0:N2}") %></span>
                                    </td>
                                    <td>
                                        <span class="badge <%# Convert.ToBoolean(Eval("Durum")) ? "badge-success" : "badge-danger" %>">
                                            <%# Convert.ToBoolean(Eval("Durum")) ? "Aktif" : "Pasif" %>
                                        </span>
                                    </td>
                                    <td>
                                        <div class="btn-group btn-group-sm">
                                            <a href='DepoDetay.aspx?id=<%# Eval("DepoID") %>' class="btn btn-info btn-sm">
                                                <i class="fa fa-eye"></i> Detay
                                            </a>
                                            <a href='DepoStok.aspx?id=<%# Eval("DepoID") %>' class="btn btn-warning btn-sm">
                                                <i class="fa fa-boxes"></i> Stok
                                            </a>
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div>
        </div>
    </section>
</asp:Content>

