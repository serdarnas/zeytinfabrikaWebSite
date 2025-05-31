<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="UrunDetay.aspx.cs" Inherits="fabrika_Urunler_UrunDetay" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <!-- Bootstrap ve Bootstrap Icons -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" crossorigin="anonymous" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css" />
    <style>
        body {
            background-color: #f8f9fa;
            font-family: sans-serif;
        }

        .page-header {
            background-color: #007bff;
            color: white;
            padding: 1.25rem 1.5rem;
            margin-bottom: 1.5rem;
            border-radius: 0.375rem;
        }

            .page-header h1 {
                margin-bottom: 0.25rem;
                font-size: 1.75rem;
            }

            .page-header .text-muted {
                color: rgba(255,255,255,0.8) !important;
            }

        .card {
            border-radius: 0.5rem;
            box-shadow: 0 2px 5px rgba(0,0,0,0.08);
            border: none;
        }

        .card-header {
            background-color: #e9ecef;
            font-weight: 600;
            padding: 1rem 1.25rem;
        }

            .card-header i {
                margin-right: 8px;
                color: #007bff;
            }

        .card-body {
            padding: 1.5rem;
        }

        .variant-table th {
            font-size: 0.85rem;
            font-weight: 600;
            text-transform: uppercase;
            color: #495057;
        }

        .variant-table td {
            font-size: 0.9rem;
            vertical-align: middle;
        }

        .variant-actions .btn {
            padding: 0.25rem 0.5rem;
            font-size: 0.8rem;
        }

        .badge.bg-success {
            background-color: #198754 !important;
        }

        .badge.bg-warning {
            background-color: #ffc107 !important;
            color: #000 !important;
        }

        .badge.bg-danger {
            background-color: #dc3545 !important;
        }
    </style>



    <div class="container-fluid">
        <div class="row">
            <!-- Ana İçerik Alanı -->
            <main class="col-12 px-md-4">
                <!-- Ürün Başlığı ve Genel Bilgiler -->

                <!-- Ürün Başlığı ve Kategoriler -->
                <div class="row mb-3">
                    <div class="col-md-9">
                        <div class="card shadow-sm mb-3">
                            <div class="card-body p-3 bg-primary text-white rounded-top">
                                <h4 class="mb-0 font-weight-bold">
                                    <h1>
                                        <asp:label id="lblUrunAdi" runat="server" />
                                    </h1>
                                    <p class="text-muted mb-0">Kategori:
                                        <asp:label id="lblKategori" runat="server" />
                                        | Marka:
                                        <asp:label id="lblMarka" runat="server" />
                                    </p>

                                </h4>
                            </div>
                            <div class="card-body d-flex flex-wrap align-items-center" style="gap: 8px;">
                                <span class="badge badge-info">
                                    <asp:label id="lblBirim" runat="server" />
                                </span>
                                <span class="badge badge-warning">KDV %<asp:label id="lblKDVOrani" runat="server" /></span>
                                <span class="badge badge-secondary">Kodu:
                                    <asp:label id="lblUrunKodu" runat="server" />
                                </span>
                                <span class="badge badge-secondary">Barkod:
                                    <asp:label id="lblBarkod" runat="server" />
                                </span>
                                <span class="badge badge-dark">Stoklu:
                                    <asp:label id="lblUrunTipiStoklu" runat="server" />
                                </span>
                                <span class="badge badge-dark">Durum:
                                    <asp:label id="lblDurum" runat="server" />
                                </span>
                                <span class="badge badge-dark">Mamul:
                                    <asp:label id="lblMamul" runat="server" />
                                </span>
                                <span class="badge badge-dark">Yarı Mamul:
                                    <asp:label id="lblYariMamul" runat="server" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3 d-flex align-items-center justify-content-center">
                        <div>
                            <asp:image id="imgUrunResim" runat="server" cssclass="img-thumbnail mb-2" width="150px" />
                            <asp:repeater id="rptResimler" runat="server">
                                <ItemTemplate>
                                    <img src='<%# Eval("ImageUrl") %>' alt="Ürün Resmi" class="img-thumbnail" style="max-width:100px; max-height:100px; margin:5px;" />
                                </ItemTemplate>
                            </asp:repeater>
                        </div>
                    </div>
                </div>

                <!-- Fiyat ve Stok Kartları -->
                <div class="row mb-3">
                    <div class="col-md-3">
                        <div class="card text-white bg-danger mb-3 shadow-sm">
                            <div class="card-body">
                                <div class="d-flex align-items-center mb-2">
                                    <i class="fa fa-money fa-2x mr-2"></i>
                                    <span>Alış Fiyatı</span>
                                </div>
                                <h4>TL
                                    <asp:label id="lblAlisFiyati" runat="server" />
                                </h4>
                                <span class="badge badge-light text-dark">KDV:
                                    <asp:label id="lblAlisKdv" runat="server" />
                                </span>
                                <span class="badge badge-light text-dark">KDV Dahil:
                                    <asp:label id="lblAlisFiyatiKdvDahilmi" runat="server" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="card text-white bg-info mb-3 shadow-sm">
                            <div class="card-body">
                                <div class="d-flex align-items-center mb-2">
                                    <i class="fa fa-tag fa-2x mr-2"></i>
                                    <span>Satış Fiyatı</span>
                                </div>
                                <h4>TL
                                    <asp:label id="lblSatisFiyati" runat="server" />
                                </h4>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="card text-white bg-success mb-3 shadow-sm">
                            <div class="card-body">
                                <div class="d-flex align-items-center mb-2">
                                    <i class="fa fa-cubes fa-2x mr-2"></i>
                                    <span>Toplam Stok</span>
                                </div>
                                <h4>
                                    <asp:label id="lblStokMiktari" runat="server" />
                                    Adet</h4>
                                <span class="badge badge-light text-dark">Min. Stok:
                                    <asp:label id="lblMinimumStok" runat="server" />
                                </span>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-3">
                        <div class="card text-white bg-warning mb-3 shadow-sm">
                            <div class="card-body">
                                <div class="d-flex align-items-center mb-2">
                                    <i class="fa fa-eye fa-2x mr-2"></i>
                                    <span>Stok Değeri</span>
                                </div>
                                <h4>₺
                                    <asp:label id="lblStokDegeri" runat="server" />
                                </h4>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Butonlar -->
                <div class="mb-3 d-flex flex-wrap" style="gap: 10px;">
                    <asp:hyperlink id="btnGuncelle" runat="server" cssclass="btn btn-warning" text="Güncelle"><i class="fa fa-pencil"></i></asp:hyperlink>
                    <asp:button id="btnStokGiris" runat="server" cssclass="btn btn-success" text="Stoklara Giriş Yap" />
                    <asp:button id="btnStokEkstresi" runat="server" cssclass="btn btn-secondary" text="Stok Ekstresi" />
                    <asp:button id="btnETicaretKodlari" runat="server" cssclass="btn btn-info" text="E-Ticaret Kodları" />
                    <asp:button id="btnRakipFiyatlari" runat="server" cssclass="btn btn-danger" text="Rakip Fiyatları" />
                    <asp:button id="btnDigerIslemler" runat="server" cssclass="btn btn-danger dropdown-toggle" text="Diğer İşlemler" />
                </div>
     <!-- Varyantlar -->
                <div class="row mb-3" id="VaryatDiv" runat="server">
                    <div class="col-md-12">
                        <div class="card shadow-sm">
                            <div class="card-header bg-light text-dark">Varyantlar</div>
                            <div class="card-body">
                                <asp:gridview id="gvVaryantlar" runat="server" cssclass="table table-striped table-bordered table-hover" autogeneratecolumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="VaryantTuru" HeaderText="Varyant Türü" />
                                        <asp:BoundField DataField="VaryantDegeri" HeaderText="Varyant Değeri" />
                                        <asp:TemplateField HeaderText="Resim">
                                            <ItemTemplate>
                                                <img src='<%# Eval("ResimYolu") %>' alt="Varyant Resmi" class="img-thumbnail" style="max-width:50px; max-height:50px;" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:gridview>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- Önceki Satışlar ve Alışlar -->
                <div class="row">
                    <div class="col-md-6">
                        <div class="card mb-3 shadow-sm">
                            <div class="card-header bg-dark text-white d-flex justify-content-between align-items-center">
                                <span>ÖNCEKİ SATIŞLAR <i class="fa fa-refresh"></i></span>
                            </div>
                            <div class="card-body">
                                <asp:panel id="pnlSatislarBos" runat="server" visible="false">
                                    <div class="alert alert-warning">Bu üründen hiç satış yapılmamış.</div>
                                </asp:panel>
                                <asp:repeater id="rptSatislar" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-sm">
                                            <thead>
                                                <tr>
                                                    <th>Tarih</th>
                                                    <th>Müşteri</th>
                                                    <th>Miktar</th>
                                                    <th>Fiyat</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Tarih", "{0:dd.MM.yyyy}") %></td>
                                            <td><%# Eval("Musteri") %></td>
                                            <td><%# Eval("Miktar") %></td>
                                            <td><%# Eval("Fiyat", "{0:N2} TL") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-6">
                        <div class="card mb-3 shadow-sm">
                            <div class="card-header bg-dark text-white d-flex justify-content-between align-items-center">
                                <span>ÖNCEKİ ALIŞLAR <i class="fa fa-refresh"></i></span>
                            </div>
                            <div class="card-body">
                                <asp:panel id="pnlAlislarBos" runat="server" visible="false">
                                    <div class="alert alert-warning">Bu ürüne ait alış yok.</div>
                                </asp:panel>
                                <asp:repeater id="rptAlislar" runat="server">
                                    <HeaderTemplate>
                                        <table class="table table-sm">
                                            <thead>
                                                <tr>
                                                    <th>Tarih</th>
                                                    <th>Tedarikçi</th>
                                                    <th>Miktar</th>
                                                    <th>Fiyat</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td><%# Eval("Tarih", "{0:dd.MM.yyyy}") %></td>
                                            <td><%# Eval("Tedarikci") %></td>
                                            <td><%# Eval("Miktar") %></td>
                                            <td><%# Eval("Fiyat", "{0:N2} TL") %></td>
                                        </tr>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                            </tbody>
                                        </table>
                                    </FooterTemplate>
                                </asp:repeater>
                            </div>
                        </div>
                    </div>
                </div>

           

                <!-- Notlar -->
                <div class="row mb-3">
                    <div class="col-md-12">
                        <div class="card shadow-sm">
                            <div class="card-header bg-light text-dark">Notlar</div>
                            <div class="card-body">
                                <asp:label id="lblNotlar" runat="server" />
                            </div>
                        </div>
                    </div>
                </div>
            </main>
        </div>
    </div>
    <!-- Bootstrap JS -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" crossorigin="anonymous"></script>
</asp:Content>

