<%@ Page Title="Zeytin Kabul İşlemleri" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="ZeytinKabul.aspx.cs" Inherits="fabrika_Zeytinyagi_ZeytinKabul" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        .modal-backdrop {
            background-color: rgba(0, 0, 0, 0.5);
        }
        #mustahsilPanel {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            z-index: 1050;
        }
        .modal-content {
            margin: 5% auto;
            width: 50%;
            max-width: 600px;
            background: white;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0,0,0,0.2);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Hata ve Başarı Mesajları -->
    <asp:Panel ID="pnlHata" runat="server" CssClass="alert alert-danger" Visible="false">
        <asp:Literal ID="ltlHata" runat="server"></asp:Literal>
    </asp:Panel>

    <asp:Panel ID="pnlBasari" runat="server" CssClass="alert alert-success" Visible="false">
        <asp:Literal ID="ltlBasari" runat="server"></asp:Literal>
    </asp:Panel>

    <div class="row mb-4">
        <div class="col-12">
            <div class="card shadow-sm">
                <div class="card-header bg-light d-flex justify-content-between align-items-center">
                    <h5 class="mb-0"><i class="fa fa-leaf me-2"></i>Zeytin Kabul İşlemleri</h5>
                    <div>
                        <asp:HyperLink ID="lnkYeniKayit" runat="server" CssClass="btn btn-success" NavigateUrl="~/fabrika/Zeytinyagi/ZeytinKabulYeni.aspx">
                            <i class="fa fa-plus me-1"></i>Yeni Kayıt Ekle
                        </asp:HyperLink>
                        <asp:HyperLink ID="lnkYeniMustahsil" runat="server" CssClass="btn btn-primary ms-2" NavigateUrl="~/fabrika/Mustahsil/YeniMustahsil.aspx">
                            <i class="fa fa-user-plus me-1"></i>Yeni Müstahsil
                        </asp:HyperLink>
                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-danger ms-2" NavigateUrl="~/fabrika/Zeytinyagi/Default.aspx">
                            <i class="fa fa-arrow-left me-1"></i>İşletme Paneli
                        </asp:HyperLink>
                    </div>
                </div>
                <div class="card-body">
                    <!-- Gelen Zeytin Listesi Tablosu -->
                    <div class="table-responsive">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div class="input-group mb-3">
                                    <span class="input-group-text"><i class="fa fa-search"></i></span>
                                    <asp:TextBox ID="txtArama" runat="server" CssClass="form-control" placeholder="Listede Ara (Parti No, Tedarikçi...)" 
                                        OnTextChanged="txtArama_TextChanged" AutoPostBack="true"></asp:TextBox>
                                </div>
                                
                                <asp:GridView ID="gvZeytinler" runat="server" AutoGenerateColumns="false" 
                                    CssClass="table table-striped table-hover table-sm" 
                                    OnRowCommand="gvZeytinler_RowCommand"
                                    DataKeyNames="ZeytinyagiUretimID" 
                                    AllowPaging="true" 
                                    PageSize="10" 
                                    OnPageIndexChanging="gvZeytinler_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="PartiNo" HeaderText="Parti No" />
                                        <asp:BoundField DataField="MustahsilAdi" HeaderText="Müstahsil" />
                                        <asp:BoundField DataField="PlakaNo" HeaderText="Plaka No" />
                                        <asp:BoundField DataField="ZeytinBoxNo" HeaderText="Box No" />
                                        <asp:BoundField DataField="GelisKg" HeaderText="Miktar (Kg)" />
                                        <asp:BoundField DataField="GelisTarihi" HeaderText="Geliş Tarihi" DataFormatString="{0:dd.MM.yyyy HH:mm}" />
                                        <asp:BoundField DataField="islem_Ad" HeaderText="İşlem Türü" />
                                        <asp:BoundField DataField="UrunAdi" HeaderText="Ürün" />
                                        <asp:TemplateField HeaderText="Durum">
                                            <ItemTemplate>
                                                <asp:Label ID="lblDurum" runat="server" CssClass='<%# GetStatusBadgeClass(Eval("Durum").ToString()) %>'>
                                                    <%# GetStatusText(Eval("Durum").ToString()) %>
                                                </asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="İşlemler" ItemStyle-CssClass="text-center">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="btnDetay" runat="server" CssClass="btn btn-sm btn-info" CommandName="Detay" CommandArgument='<%# Eval("ZeytinyagiUretimID") %>' ToolTip="Detayları Gör">
                                                    <i class="fa fa-eye"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnDuzenle" runat="server" CssClass='<%# Eval("Durum").ToString() == "Beklemede" ? "btn btn-sm btn-primary" : "btn btn-sm btn-secondary disabled" %>' 
                                                    CommandName="Duzenle" CommandArgument='<%# Eval("ZeytinyagiUretimID") %>' ToolTip="Düzenle">
                                                    <i class="fa fa-pencil"></i>
                                                </asp:LinkButton>
                                                <asp:LinkButton ID="btnIslemBaslat" runat="server" CssClass='<%# Eval("Durum").ToString() == "Beklemede" ? "btn btn-sm btn-success" : "btn btn-sm btn-secondary disabled" %>' 
                                                    CommandName="IslemBaslat" CommandArgument='<%# Eval("ZeytinyagiUretimID") %>' ToolTip="İşleme Başlat">
                                                    <i class="fa fa-play"></i>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle CssClass="pagination pagination-sm justify-content-center mt-3" />
                                    <PagerSettings Mode="NumericFirstLast" FirstPageText="İlk" LastPageText="Son" />
                                    <EmptyDataTemplate>
                                        <div class="alert alert-info">Kayıt bulunamadı.</div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>