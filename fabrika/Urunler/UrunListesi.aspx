<%@ Page Title="Ürün Listesi" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="UrunListesi.aspx.cs" Inherits="fabrika_Urunler_UrunListesi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="row">
        <div class="col-lg-12">
            <section class="panel">

                <div class="panel-body">
                    <!-- Üst Menü Butonları -->
                    <div class="btn-group">
                        <asp:HyperLink ID="btnYeniUrun" runat="server" NavigateUrl="YeniUrun.aspx" CssClass="btn btn-shadow  btn-success" Style="margin-right: 5px;">
                            <i class="icon-plus"></i> Yeni Ürün Ekle
                        </asp:HyperLink>
                        <asp:HyperLink ID="btnExcelIndir" runat="server" NavigateUrl="Urun_yukle_excel.aspx" CssClass="btn btn-shadow btn-warning" Style="margin-right: 5px;">
                            <i class="icon-file-excel"></i> Excel'den Aktar
                        </asp:HyperLink>
                        <asp:HyperLink ID="btnTopluGuncelle" runat="server" CssClass="btn btn-shadow btn-info" Style="margin-right: 5px;">
                            <i class="icon-refresh"></i> Toplu Güncelleme
                        </asp:HyperLink>
                        <asp:HyperLink ID="btnTopluResim" runat="server" CssClass="btn btn-shadow btn-primary" Style="margin-right: 5px;">
                            <i class="icon-picture"></i> Toplu Resim Yükleme
                        </asp:HyperLink>
                        <asp:HyperLink ID="btnTopluSil" runat="server" CssClass="btn btn-shadow btn-danger" Style="margin-right: 5px;">
                            <i class="icon-trash"></i> Toplu Sil
                        </asp:HyperLink>
                    </div>
                </div>
            </section>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Ürünler</h3>
                    <div class="col-md-3">
                        <div class="form-group">
                            <%--<label>Aktif Ürünler</label>--%>
                            <asp:DropDownList ID="ddlAktifUrunler" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlAktifUrunler_SelectedIndexChanged">
                                <asp:ListItem Text="Tüm Ürünler" Value=""></asp:ListItem>
                                <asp:ListItem Text="Aktif Ürünler" Value="1" Selected="True"></asp:ListItem>
                                <asp:ListItem Text="Pasif Ürünler" Value="0"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </div>
                
                        <div class="col-md-3">
                            <div class="form-group">
                                <%--<label>Tüm Kategoriler</label>--%>
                                <asp:DropDownList ID="ddlKategoriler" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlKategoriler_SelectedIndexChanged">
                                    <asp:ListItem Text="Tüm Kategoriler" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <%--<label>Tüm Markalar</label>--%>
                                <asp:DropDownList ID="ddlMarkalar" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlMarkalar_SelectedIndexChanged">
                                    <asp:ListItem Text="Tüm Markalar" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="col-md-3">
                            <div class="form-group">
                                <%--<label>Ara:</label>--%>
                                <div class="input-group">
                                    <asp:TextBox ID="txtArama" runat="server" CssClass="form-control" placeholder="Ürün adı, kodu veya barkod..."></asp:TextBox>
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btnAra" runat="server" CssClass="btn btn-primary" OnClick="btnAra_Click">
                                                            <i class="icon-search"></i>
                                        </asp:LinkButton>
                                    </span>
                                </div>
                            </div>
                        </div>
                    
                    </header>
                <div class="panel-body">
                    <div class="table-responsive">
                        <table class="table table-bordered table-striped">
                            <thead class="thead-dark">
                                <tr>
                                    <th>Ürün Ad</th>
                                    <th>Stok Miktari</th>
                                    <th>Satiş Fiyati</th>
                                    <th>ürün Görseli</th>
                                </tr>
                            </thead>
                            <tbody>
                                <asp:Repeater ID="rptUrunler" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <div style="background: #197; color: white; border-radius: 6px; padding: 8px 12px; margin-bottom: 4px;">
                                                    <a href='UrunDetay.aspx?UrunID=<%# Eval("UrunID") %>' style="color: white; text-decoration: none; font-weight: bold;">
                                                        <%# Eval("UrunAdi") %>
                                                    </a>
                                                    <%-- <span style="background: #66bb6a; color: white; border-radius: 4px; padding: 2px 8px; margin-left: 10px; font-weight: bold;">
                                                <%# Eval("StokMiktari") %>
                                            </span>--%>
                                                </div>
                                            </td>
                                            <td>
                                                <span style="background: #197; color: white; border-radius: 4px; padding: 4px 10px; font-size: 13px;">
                                                    <%# Eval("StokMiktari") %>
                                                </span>
                                            </td>
                                            <td>
                                                <span style="background: #1976d2; color: white; border-radius: 4px; padding: 4px 10px; font-size: 13px;">
                                                    <%# Eval("SatisFiyati", "{0:N2} TL") %>
                                                </span>
                                            </td>
                                            <td>
                                                <span style="background: #1976d2; color: white; border-radius: 4px; padding: 4px 10px; font-size: 13px;">
                                                    <%-- <%# Eval(UrunlerTools.UrunGorsel.UrunGorselKapak("UrunID")) %>--%>

                                                    <asp:Image ID="Image2" Height="32" Width="32" runat="server" ImageUrl='<%# UrunlerTools.UrunGorsel.UrunGorselKapak(Eval("UrunID").ToString()) %>' />


                                                </span>
                                            </td>
                                            
                               
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>

                </div>
            </section>
        </div>
    </div>

    
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">

                <div class="panel-body">
                    <!-- Filtre Alanı -->
                    <div class="row" style="margin-top: 20px;">



                    </div>
                </div>
            </section>
        </div>
    </div>

    <div class="row">
        <div class="col-lg-12">
            <section class="panel">

                <div class="panel-body">
                    <!-- Ürün Listesi Tablosu -->

                    <div class="row" id="urunKartlari">
                        <asp:Repeater ID="rptUrunler_" runat="server">
                            <ItemTemplate>
                                <div class="col-md-4 col-sm-6 mb-4">
                                    <div class="card h-100 shadow-sm">
                                        <img src='<%# Eval("UrunAdi", "~/Uploads/Urunler/{0}") %>' class="card-img-top" alt='<%# Eval("UrunAdi") %>' style="height: 200px; object-fit: contain;">
                                        <div class="card-body">
                                            <h5 class="card-title"><%# Eval("UrunAdi") %></h5>
                                            <p class="card-text mb-1"><small class="text-muted">Stok Kodu: <%# Eval("StokKodu") %></small></p>
                                            <p class="card-text mb-1"><small class="text-muted">Barkod: <%# Eval("Barkod") %></small></p>
                                            <span class="badge bg-primary">Fiyat: <%# Eval("SatisFiyati", "{0:N2} TL") %></span>
                                            <span class="badge bg-success">Stok: <%# Eval("StokMiktari") %> <%# Eval("Birim") %></span>
                                        </div>
                                        <div class="card-footer bg-white border-0 d-flex justify-content-between">
                                            <a href='UrunDetay.aspx?id=<%# Eval("UrunID") %>' class="btn btn-sm btn-primary"><i class="icon-pencil"></i>Düzenle</a>
                                            <asp:LinkButton ID="btnSil" runat="server" CssClass="btn btn-sm btn-danger" CommandName="Sil" CommandArgument='<%# Eval("UrunID") %>' OnClientClick="return confirm('Bu ürünü silmek istediğinize emin misiniz?');">
                                                        <i class="icon-trash"></i> Sil
                                            </asp:LinkButton>
                                            <a href='UrunKopyala.aspx?id=<%# Eval("UrunID") %>' class="btn btn-sm btn-info"><i class="icon-copy"></i>Kopyala</a>
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                </div>
            </section>
        </div>
    </div>

    <style>
        #urunKartlari .card {
            transition: box-shadow 0.2s;
            border-radius: 12px;
            border: 1px solid #e0e0e0;
        }

            #urunKartlari .card:hover {
                box-shadow: 0 8px 24px rgba(44,62,80,0.15);
                border-color: #b2bec3;
            }

        #urunKartlari .card-img-top {
            background: #f8f9fa;
            border-bottom: 1px solid #eee;
        }

        #urunKartlari .card-footer {
            background: #fff;
            border-top: none;
        }
    </style>

    <!-- JavaScript Kodları -->
    <script type="text/javascript">
        // Tümünü Seç / Kaldır fonksiyonu
        function ToggleCheckUncheckAllItemsGridView1(headerChkbox) {
            var gvCheck = document.getElementById('<%= rptUrunler.ClientID %>');
            var i;
            var inputs = gvCheck.getElementsByTagName("input");
            for (i = 0; i < inputs.length; i++) {
                if (inputs[i].type == "checkbox") {
                    inputs[i].checked = headerChkbox.checked;
                }
            }
        }
    </script>
</asp:Content>

