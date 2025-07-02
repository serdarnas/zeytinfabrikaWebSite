<%@ Page Title="Yeni Müstahsil" Language="C#" MasterPageFile="~/Mobil/MobilMasterPage.master" AutoEventWireup="true" CodeFile="YeniMustahsil.aspx.cs" Inherits="Mobil_YeniMustahsil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .form-container {
            background: white;
            border-radius: 15px;
            padding: 25px;
            margin-bottom: 20px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.08);
        }
        
        .form-header {
            display: flex;
            align-items: center;
            margin-bottom: 25px;
            padding-bottom: 15px;
            border-bottom: 2px solid #f0f0f0;
        }
        
        .form-header i {
            font-size: 24px;
            color: #4CAF50;
            margin-right: 10px;
        }
        
        .form-header h4 {
            margin: 0;
            color: #333;
            font-weight: 600;
        }
        
        .form-group {
            margin-bottom: 20px;
        }
        
        .form-label {
            font-weight: 600;
            color: #555;
            margin-bottom: 8px;
            display: block;
        }
        
        .form-control, .form-select {
            width: 100%;
            padding: 12px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 16px;
            transition: border-color 0.3s ease;
        }
        
        .form-control:focus, .form-select:focus {
            border-color: #4CAF50;
            outline: none;
            box-shadow: 0 0 10px rgba(76, 175, 80, 0.1);
        }
        
        .form-control[readonly] {
            background-color: #f8f9fa;
            opacity: 0.8;
            cursor: not-allowed;
        }
        
        .form-control.is-invalid {
            border-color: #dc3545;
        }
        
        .invalid-feedback {
            color: #dc3545;
            font-size: 14px;
            margin-top: 5px;
        }
        
        .btn-save {
            background: linear-gradient(135deg, #4CAF50, #45a049);
            color: white;
            border: none;
            border-radius: 12px;
            padding: 15px 30px;
            font-size: 16px;
            font-weight: 600;
            width: 100%;
            margin-top: 20px;
            cursor: pointer;
            box-shadow: 0 4px 15px rgba(76, 175, 80, 0.3);
        }
        
        .btn-save:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(76, 175, 80, 0.4);
        }
        
        .btn-cancel {
            background: #6c757d;
            color: white;
            border: none;
            border-radius: 12px;
            padding: 15px 30px;
            font-size: 16px;
            font-weight: 600;
            width: 100%;
            margin-top: 10px;
            cursor: pointer;
            text-decoration: none;
            display: inline-block;
            text-align: center;
        }
        
        .btn-cancel:hover {
            background: #5a6268;
            color: white;
            text-decoration: none;
        }
        
        .btn-cancel i {
            margin-right: 8px;
        }
        
        .required {
            color: #f44336;
        }
        
        .tc-info {
            background: #e3f2fd;
            border-left: 4px solid #2196F3;
            padding: 12px 15px;
            margin-top: 8px;
            border-radius: 0 8px 8px 0;
            font-size: 14px;
            color: #1976D2;
        }
        
        .switch-container {
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .switch {
            position: relative;
            display: inline-block;
            width: 50px;
            height: 24px;
        }
        
        .switch input {
            opacity: 0;
            width: 0;
            height: 0;
        }
        
        .slider {
            position: absolute;
            cursor: pointer;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            background-color: #ccc;
            transition: .4s;
            border-radius: 24px;
        }
        
        .slider:before {
            position: absolute;
            content: "";
            height: 18px;
            width: 18px;
            left: 3px;
            bottom: 3px;
            background-color: white;
            transition: .4s;
            border-radius: 50%;
        }
        
        input:checked + .slider {
            background-color: #4CAF50;
        }
        
        input:checked + .slider:before {
            transform: translateX(26px);
        }
        
        .alert {
            padding: 15px 20px;
            border-radius: 8px;
            margin-bottom: 20px;
            border: none;
            font-weight: 500;
        }
        
        .alert-success {
            background: #d4edda;
            color: #155724;
        }
        
        .alert-danger {
            background: #f8d7da;
            color: #721c24;
        }
        
        .alert i {
            margin-right: 8px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="form-container">
        <div class="form-header">
            <i class="fas fa-user-plus"></i>
            <h4>
                <asp:Label ID="lblBaslik" runat="server" Text="Yeni Müstahsil Ekle"></asp:Label>
            </h4>
        </div>
        
        <!-- Mesaj Paneli -->
        <asp:Panel ID="pnlMesaj" runat="server" Visible="false">
            <div id="divMesaj" class="alert">
                <i class="fas fa-info-circle"></i>
                <asp:Label ID="lblMesaj" runat="server"></asp:Label>
            </div>
        </asp:Panel>
        
        <!-- Temel Bilgiler -->
        <div class="form-group">
            <label class="form-label">Ad <span class="required">*</span></label>
            <asp:TextBox ID="txtAd" runat="server" CssClass="form-control" placeholder="Adı giriniz" />
            <asp:RequiredFieldValidator ID="rfvAd" runat="server" ControlToValidate="txtAd" 
                ErrorMessage="Ad alanı zorunludur" CssClass="invalid-feedback" Display="Dynamic" />
        </div>
        
        <div class="form-group">
            <label class="form-label">Soyad <span class="required">*</span></label>
            <asp:TextBox ID="txtSoyad" runat="server" CssClass="form-control" placeholder="Soyadı giriniz" />
            <asp:RequiredFieldValidator ID="rfvSoyad" runat="server" ControlToValidate="txtSoyad" 
                ErrorMessage="Soyad alanı zorunludur" CssClass="invalid-feedback" Display="Dynamic" />
        </div>
        
        <div class="form-group">
            <label class="form-label">TC Kimlik No</label>
            <asp:TextBox ID="txtTCKimlikNo" runat="server" CssClass="form-control" 
                placeholder="TC Kimlik No (11 haneli)" MaxLength="11" AutoPostBack="true" OnTextChanged="txtTCKimlikNo_TextChanged" />
            <div class="tc-info">
                <i class="fas fa-info-circle"></i>
                TC Kimlik No benzersiz olmalıdır. Daha önce kaydedilmiş kişi varsa otomatik olarak bulunacaktır.
            </div>
        </div>
        
        <div class="form-group">
            <label class="form-label">Telefon</label>
            <asp:TextBox ID="txtTelefon" runat="server" CssClass="form-control" 
                placeholder="Telefon numarası" TextMode="Phone" AutoPostBack="true" OnTextChanged="txtTelefon_TextChanged" />
            <div class="tc-info">
                <i class="fas fa-info-circle"></i>
                Telefon numarası benzersiz olmalıdır. Daha önce kaydedilmiş telefon varsa otomatik olarak bulunacaktır.
            </div>
        </div>
        
        <div class="form-group">
            <label class="form-label">E-posta</label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" 
                placeholder="E-posta adresi" TextMode="Email" />
        </div>
        
        <div class="form-group">
            <label class="form-label">Adres</label>
            <asp:TextBox ID="txtAdres" runat="server" CssClass="form-control" 
                placeholder="Adres bilgileri" TextMode="MultiLine" Rows="3" />
        </div>
        
        <div class="form-group">
            <label class="form-label">Bakiyesi (₺)</label>
            <asp:TextBox ID="txtBakiyesi" runat="server" CssClass="form-control" 
                placeholder="0,00" TextMode="Number" step="0.01" ReadOnly="true" />
            <small style="color: #666; font-size: 12px; margin-top: 5px; display: block;">
                <i class="fas fa-info-circle"></i> Bakiye sistem tarafından otomatik hesaplanır
            </small>
        </div>
        
        <div class="form-group">
            <label class="form-label">Banka Bilgileri</label>
            <asp:TextBox ID="txtBankaBilgileri" runat="server" CssClass="form-control" 
                placeholder="Banka bilgileri" TextMode="MultiLine" Rows="3" />
        </div>
        
        <div class="form-group">
            <label class="form-label">Notlar</label>
            <asp:TextBox ID="txtNotlar" runat="server" CssClass="form-control" 
                placeholder="Notlar" TextMode="MultiLine" Rows="3" />
        </div>
        
        <div class="form-group">
            <label class="form-label">Durum</label>
            <div class="switch-container">
                <label class="switch">
                    <asp:CheckBox ID="cbAktif" runat="server" />
                    <span class="slider"></span>
                </label>
                <span>Aktif</span>
            </div>
        </div>
        
        <!-- Butonlar -->
        <div class="form-group">
            <asp:Button ID="btnKaydet" runat="server" CssClass="btn-save" 
                Text="Müstahsili Kaydet" OnClick="btnKaydet_Click" />
            <a href="MustahsilListesi.aspx" class="btn-cancel">
                <i class="fas fa-arrow-left"></i> Listeye Dön
            </a>
            <asp:Panel ID="pnlGuncellemeButonlari" runat="server" Visible="false" style="margin-top: 10px;">
                <a href="MustahsilListesi.aspx" class="btn-cancel" style="background: #17a2b8;">
                    <i class="fas fa-list"></i> Müstahsil Listesi
                </a>
                <asp:HyperLink ID="lnkMalKabul" runat="server" CssClass="btn-cancel" 
                    style="background: #FF9800; margin-top: 10px;" Visible="false">
                    <i class="fas fa-truck-loading"></i> Mal Kabul
                </asp:HyperLink>
            </asp:Panel>
        </div>
    </div>
    
    <script>
        // TC Kimlik No için sadece rakam girişine izin ver
        document.addEventListener('DOMContentLoaded', function() {
            const tcInput = document.getElementById('<%= txtTCKimlikNo.ClientID %>');
            if (tcInput) {
                tcInput.addEventListener('input', function() {
                    this.value = this.value.replace(/[^0-9]/g, '');
                });
            }
            
            // Telefon numarası formatı
            const telefonInput = document.getElementById('<%= txtTelefon.ClientID %>');
            if (telefonInput) {
                telefonInput.addEventListener('input', function() {
                    // Sadece rakam, boşluk, (), + ve - karakterlerine izin ver
                    this.value = this.value.replace(/[^0-9\s\(\)\+\-]/g, '');
                });
            }
            
            // Bakiye readonly olduğu için format kontrolü kaldırıldı
        });
        
        // Mesaj auto-hide
        setTimeout(function() {
            const mesajPanel = document.getElementById('<%= pnlMesaj.ClientID %>');
            if (mesajPanel && mesajPanel.style.display !== 'none') {
                mesajPanel.style.opacity = '0';
                setTimeout(function() {
                    mesajPanel.style.display = 'none';
                }, 300);
            }
        }, 5000);
        
        // URL parametresi kontrolü - başarılı işlem sonrası mesajı
        const urlParams = new URLSearchParams(window.location.search);
        if (urlParams.get('success') === 'new') {
            // Yeni kayıt başarılı mesajı göster
            setTimeout(function() {
                if (confirm('Müstahsil başarıyla kaydedildi! Müstahsil listesine dönmek ister misiniz?')) {
                    window.location.href = 'MustahsilListesi.aspx';
                }
            }, 1000);
        }
    </script>
</asp:Content>

