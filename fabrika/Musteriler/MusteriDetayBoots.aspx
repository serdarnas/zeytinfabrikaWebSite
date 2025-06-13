<%@ Page Title="Müşteri Detay Bootstrap" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="MusteriDetayBoots.aspx.cs" Inherits="fabrika_Musteriler_MusteriDetayBoots" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">
    
    <div class="container-fluid py-4">
        <div class="row">
            <!-- Müşteri Profil Kartı -->
            <div class="col-12 col-lg-4 mb-4">
                <div class="card border-0 shadow-sm h-100">
                    <div class="card-body">
                        <div class="text-center mb-4">
                            <asp:Image ID="MusteriResim" runat="server" CssClass="rounded-circle border shadow-sm" Width="120" Height="120" />
                            <h3 class="mt-3 mb-1">
                                <asp:Label ID="lblMusteriAdi" runat="server" />
                            </h3>
                            <p class="text-muted mb-3">
                                <i class="bi bi-geo-alt"></i>
                                <asp:Label ID="lblAdres" runat="server" />
                            </p>
                        </div>

                        <div class="list-group list-group-flush">
                            <div class="list-group-item border-0 px-0">
                                <div class="d-flex align-items-center">
                                    <span class="bg-light rounded-circle p-2 me-3">
                                        <i class="bi bi-person text-primary"></i>
                                    </span>
                                    <div>
                                        <small class="text-muted d-block">Yetkili</small>
                                        <strong><asp:Label ID="lblYetkili" runat="server" /></strong>
                                    </div>
                                </div>
                            </div>
                            <div class="list-group-item border-0 px-0">
                                <div class="d-flex align-items-center">
                                    <span class="bg-light rounded-circle p-2 me-3">
                                        <i class="bi bi-telephone text-primary"></i>
                                    </span>
                                    <div>
                                        <small class="text-muted d-block">Telefon</small>
                                        <strong><asp:Label ID="lblTelefon" runat="server" /></strong>
                                    </div>
                                </div>
                            </div>
                            <div class="list-group-item border-0 px-0">
                                <div class="d-flex align-items-center">
                                    <span class="bg-light rounded-circle p-2 me-3">
                                        <i class="bi bi-phone text-primary"></i>
                                    </span>
                                    <div>
                                        <small class="text-muted d-block">Cep Telefonu</small>
                                        <strong><asp:Label ID="lblCepTelefonu" runat="server" /></strong>
                                    </div>
                                </div>
                            </div>
                            <div class="list-group-item border-0 px-0">
                                <div class="d-flex align-items-center">
                                    <span class="bg-light rounded-circle p-2 me-3">
                                        <i class="bi bi-envelope text-primary"></i>
                                    </span>
                                    <div>
                                        <small class="text-muted d-block">E-posta</small>
                                        <strong><asp:Label ID="lblmail" runat="server" /></strong>
                                    </div>
                                </div>
                            </div>
                            <div class="list-group-item border-0 px-0">
                                <div class="d-flex align-items-center">
                                    <span class="bg-light rounded-circle p-2 me-3">
                                        <i class="bi bi-building text-primary"></i>
                                    </span>
                                    <div>
                                        <small class="text-muted d-block">Vergi Dairesi</small>
                                        <strong><asp:Label ID="lblVergiDairesi" runat="server" /></strong>
                                    </div>
                                </div>
                            </div>
                            <div class="list-group-item border-0 px-0">
                                <div class="d-flex align-items-center">
                                    <span class="bg-light rounded-circle p-2 me-3">
                                        <i class="bi bi-upc text-primary"></i>
                                    </span>
                                    <div>
                                        <small class="text-muted d-block">Vergi No</small>
                                        <strong><asp:Label ID="lblVergiNo" runat="server" /></strong>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="mt-4">
                            <asp:HyperLink ID="hplinkMusteriGuncelleBoots" runat="server" CssClass="btn btn-primary w-100 mb-2">
                                <i class="bi bi-pencil"></i> Müşteriyi Güncelle
                            </asp:HyperLink>
                            <asp:HyperLink ID="hplinkSatisYapBoots" runat="server" CssClass="btn btn-success w-100">
                                <i class="bi bi-cart-plus"></i> Yeni Satış
                            </asp:HyperLink>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Müşteri İstatistikleri ve Satışlar -->
            <div class="col-12 col-lg-8">
                <!-- İstatistik Kartları -->
                <div class="row g-3 mb-4">
                    <div class="col-6 col-md-3">
                        <div class="card border-0 shadow-sm h-100">
                            <div class="card-body text-center">
                                <div class="mb-2 text-primary fs-3">
                                    <i class="bi bi-currency-dollar"></i>
                                </div>
                                <h5 class="card-title mb-1">
                                    <asp:Label ID="lblToplamSatis" runat="server" />
                                </h5>
                                <p class="card-text small text-muted mb-0">Toplam Satış</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card border-0 shadow-sm h-100">
                            <div class="card-body text-center">
                                <div class="mb-2 text-success fs-3">
                                    <i class="bi bi-graph-up"></i>
                                </div>
                                <h5 class="card-title mb-1">
                                    <asp:Label ID="lblAcikBakiye" runat="server" />
                                </h5>
                                <p class="card-text small text-muted mb-0">Açık Bakiye</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card border-0 shadow-sm h-100">
                            <div class="card-body text-center">
                                <div class="mb-2 text-warning fs-3">
                                    <i class="bi bi-credit-card"></i>
                                </div>
                                <h5 class="card-title mb-1">
                                    <asp:Label ID="lblCekBakiye" runat="server" />
                                </h5>
                                <p class="card-text small text-muted mb-0">Çek Bakiyesi</p>
                            </div>
                        </div>
                    </div>
                    <div class="col-6 col-md-3">
                        <div class="card border-0 shadow-sm h-100">
                            <div class="card-body text-center">
                                <div class="mb-2 text-info fs-3">
                                    <i class="bi bi-cash"></i>
                                </div>
                                <h5 class="card-title mb-1">
                                    <asp:Label ID="lblSenetBakiye" runat="server" />
                                </h5>
                                <p class="card-text small text-muted mb-0">Senet Bakiyesi</p>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Satışlar Tablosu -->
                <div class="card border-0 shadow-sm">
                    <div class="card-header bg-white py-3">
                        <h5 class="card-title mb-0">Müşteri Satışları</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gridSatislar" runat="server" CssClass="table table-hover" AutoGenerateColumns="false"
                                OnRowDataBound="gridSatislar_RowDataBound" EmptyDataText="Henüz satış kaydı bulunmuyor.">
                                <Columns>
                                    <asp:TemplateField HeaderText="Satış No">
                                        <ItemTemplate>
                                            <a href='../Satislar/SatisDetay.aspx?id=<%# Eval("SatisID") %>' class="text-primary text-decoration-none">
                                                #<%# Eval("SatisID") %>
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SatisTarihi" HeaderText="Satış Tarihi" DataFormatString="{0:dd.MM.yyyy}" />
                                    <asp:BoundField DataField="ToplamTutar" HeaderText="Toplam Tutar" DataFormatString="{0:C2}" />
                                    <asp:TemplateField HeaderText="Ödeme Durumu">
                                        <ItemTemplate>
                                            <asp:Label ID="lblOdemeDurumu" runat="server" Text='<%# Eval("OdemeDurumu") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="SatisDurumu" HeaderText="Satış Durumu" />
                                </Columns>
                                <HeaderStyle CssClass="table-light" />
                                <EmptyDataTemplate>
                                    <div class="text-center py-4 text-muted">
                                        <i class="bi bi-inbox fs-2"></i>
                                        <p class="mt-2 mb-0">Henüz satış kaydı bulunmuyor.</p>
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
