<%@ Page Title="Nakit Tahsilat" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="TahsilatNakit.aspx.cs" Inherits="fabrika_Musteriler_TahsilatNakit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">
    <style>
        .payment-form {
            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
            border-radius: 15px;
            padding: 2rem;
            color: white;
            box-shadow: 0 10px 30px rgba(0,0,0,0.2);
        }
        .form-control, .form-select {
            border-radius: 10px;
            border: 2px solid rgba(255,255,255,0.3);
            background: rgba(255,255,255,0.1);
            color: white;
            padding: 12px 15px;
        }
        .form-control::placeholder {
            color: rgba(255,255,255,0.7);
        }
        .form-control:focus, .form-select:focus {
            background: rgba(255,255,255,0.2);
            border-color: rgba(255,255,255,0.6);
            color: white;
            box-shadow: 0 0 0 0.2rem rgba(255,255,255,0.25);
        }
        .btn-payment {
            background: rgba(255,255,255,0.2);
            border: 2px solid rgba(255,255,255,0.3);
            color: white;
            border-radius: 10px;
            padding: 12px 30px;
            font-weight: 600;
            transition: all 0.3s ease;
        }
        .btn-payment:hover {
            background: rgba(255,255,255,0.3);
            border-color: rgba(255,255,255,0.5);
            color: white;
            transform: translateY(-2px);
        }
        .amount-display {
            background: rgba(255,255,255,0.1);
            border-radius: 10px;
            padding: 1rem;
            text-align: center;
            margin: 1rem 0;
        }
        .amount-value {
            font-size: 2rem;
            font-weight: bold;
            color: #fff;
        }
        .customer-info {
            background: rgba(255,255,255,0.1);
            border-radius: 10px;
            padding: 1rem;
            margin-bottom: 1.5rem;
        }
        @media (max-width: 768px) {
            .payment-form {
                padding: 1.5rem;
                margin: 1rem;
            }
            .amount-value {
                font-size: 1.5rem;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="container-fluid py-4">
        <!-- Başlık -->
        <div class="d-flex justify-content-between align-items-center mb-4">
            <h2 class="mb-0"><i class="bi bi-cash-coin"></i> Nakit Tahsilat</h2>
            <asp:LinkButton ID="btnGeriDon" runat="server" CssClass="btn btn-outline-secondary" OnClick="btnGeriDon_Click">
                <i class="bi bi-arrow-left"></i> Geri Dön
            </asp:LinkButton>
        </div>

        <div class="row justify-content-center">
            <div class="col-lg-8">
                <div class="payment-form">
                    <!-- Müşteri Bilgileri -->
                    <div class="customer-info">
                        <div class="row">
                            <div class="col-md-8">
                                <h5><i class="bi bi-person-circle"></i> <asp:Label ID="lblMusteriAdi" runat="server"></asp:Label></h5>
                                <p class="mb-0"><i class="bi bi-geo-alt"></i> <asp:Label ID="lblMusteriAdres" runat="server"></asp:Label></p>
                            </div>
                            <div class="col-md-4 text-end">
                                <small>Mevcut Bakiye</small>
                                <h4><asp:Label ID="lblMevcutBakiye" runat="server" CssClass="text-warning"></asp:Label> ₺</h4>
                            </div>
                        </div>
                    </div>

                    <!-- Tahsilat Formu -->
                    <div class="row g-3">
                        <div class="col-md-6">
                            <label class="form-label"><i class="bi bi-cash"></i> Tahsilat Tutarı</label>
                            <asp:TextBox ID="txtTutar" runat="server" CssClass="form-control" placeholder="0,00" TextMode="Number" step="0.01"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvTutar" runat="server" ControlToValidate="txtTutar" 
                                ErrorMessage="Tutar girilmelidir" CssClass="text-warning small" Display="Dynamic"></asp:RequiredFieldValidator>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label"><i class="bi bi-calendar"></i> İşlem Tarihi</label>
                            <asp:TextBox ID="txtIslemTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                        </div>
                        <div class="col-12">
                            <label class="form-label"><i class="bi bi-chat-text"></i> Açıklama</label>
                            <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" 
                                Rows="3" placeholder="İşlem açıklaması (opsiyonel)"></asp:TextBox>
                        </div>
                    </div>

                    <!-- Tutar Gösterimi -->
                    <div class="amount-display">
                        <div class="amount-value">
                            <asp:Label ID="lblTutarGoster" runat="server" Text="0,00"></asp:Label> ₺
                        </div>
                        <small>Tahsilat Edilecek Tutar</small>
                    </div>

                    <!-- İşlem Butonları -->
                    <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                        <asp:Button ID="btnTemizle" runat="server" Text="Temizle" CssClass="btn btn-payment" 
                            OnClick="btnTemizle_Click" CausesValidation="false" />
                        <asp:Button ID="btnKaydet" runat="server" Text="Tahsilatı Kaydet" CssClass="btn btn-payment" 
                            OnClick="btnKaydet_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Son İşlemler -->
        <div class="row mt-4">
            <div class="col-12">
                <div class="card shadow-sm">
                    <div class="card-header bg-light">
                        <h5 class="mb-0"><i class="bi bi-clock-history"></i> Son Nakit Tahsilatlar</h5>
                    </div>
                    <div class="card-body">
                        <asp:Repeater ID="rptSonIslemler" runat="server">
                            <ItemTemplate>
                                <div class="d-flex justify-content-between align-items-center py-2 border-bottom">
                                    <div>
                                        <strong><%# String.Format("{0:N2}", Eval("Tutar")) %> ₺</strong>
                                        <small class="text-muted d-block"><%# String.Format("{0:dd.MM.yyyy HH:mm}", Eval("IslemTarihi")) %></small>
                                    </div>
                                    <div class="text-end">
                                        <span class="badge bg-success">Nakit</span>
                                        <%# !String.IsNullOrEmpty(Eval("Aciklama").ToString()) ? "<br><small class=\"text-muted\">" + Eval("Aciklama") + "</small>" : "" %>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <asp:Panel ID="pnlSonIslemYok" runat="server" Visible="false" CssClass="text-center py-4">
                            <i class="bi bi-info-circle text-muted" style="font-size: 2rem;"></i>
                            <p class="text-muted mt-2">Henüz nakit tahsilat işlemi bulunmuyor.</p>
                        </asp:Panel>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        // Tutar değiştiğinde gösterimi güncelle
        document.getElementById('<%= txtTutar.ClientID %>').addEventListener('input', function() {
            var tutar = parseFloat(this.value) || 0;
            document.getElementById('<%= lblTutarGoster.ClientID %>').innerText = tutar.toLocaleString('tr-TR', {minimumFractionDigits: 2});
        });

        // Sayfa yüklendiğinde bugünün tarihini set et
        window.addEventListener('load', function() {
            var today = new Date().toISOString().split('T')[0];
            document.getElementById('<%= txtIslemTarihi.ClientID %>').value = today;
        });
    </script>
</asp:Content>
