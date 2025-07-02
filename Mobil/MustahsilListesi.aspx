<%@ Page Title="Müstahsil Listesi" Language="C#" MasterPageFile="~/Mobil/MobilMasterPage.master" AutoEventWireup="true" CodeFile="MustahsilListesi.aspx.cs" Inherits="Mobil_MustahsilListesi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .search-container {
            background: white;
            border-radius: 15px;
            padding: 20px;
            margin-bottom: 20px;
            box-shadow: 0 4px 20px rgba(0,0,0,0.08);
        }
        
        .search-input {
            width: 100%;
            padding: 12px 15px;
            border: 2px solid #e0e0e0;
            border-radius: 8px;
            font-size: 16px;
            margin-bottom: 15px;
        }
        
        .search-input:focus {
            border-color: #4CAF50;
            outline: none;
        }
        
        .mustahsil-card {
            background: white;
            border-radius: 12px;
            padding: 20px;
            margin-bottom: 15px;
            box-shadow: 0 4px 15px rgba(0,0,0,0.08);
            border-left: 4px solid #4CAF50;
        }
        
        .mustahsil-name {
            font-size: 18px;
            font-weight: 600;
            color: #333;
            margin-bottom: 8px;
        }
        
        .mustahsil-info {
            color: #666;
            font-size: 14px;
            margin-bottom: 5px;
        }
        
        .mustahsil-info i {
            width: 16px;
            color: #4CAF50;
            margin-right: 8px;
        }
        
        .btn-edit {
            background: #4CAF50;
            color: white;
            border: none;
            border-radius: 8px;
            padding: 8px 16px;
            font-size: 14px;
            margin-top: 10px;
            text-decoration: none;
            display: inline-block;
        }
        
        .btn-malkabul {
            background: #FF9800;
            color: white;
            border: none;
            border-radius: 8px;
            padding: 8px 16px;
            font-size: 14px;
            margin-top: 10px;
            text-decoration: none;
            display: inline-block;
            transition: all 0.3s ease;
        }
        
        .btn-malkabul:hover {
            background: #F57C00;
            color: white;
            text-decoration: none;
            transform: translateY(-1px);
        }
        
        .btn-add {
            position: fixed;
            bottom: 80px;
            right: 20px;
            width: 56px;
            height: 56px;
            background: linear-gradient(135deg, #4CAF50, #45a049);
            border: none;
            border-radius: 50%;
            color: white;
            font-size: 24px;
            box-shadow: 0 4px 15px rgba(76, 175, 80, 0.4);
            z-index: 1000;
            text-decoration: none;
            display: flex;
            align-items: center;
            justify-content: center;
            transition: all 0.3s ease;
        }
        
        .btn-add:hover {
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(76, 175, 80, 0.6);
            color: white;
            text-decoration: none;
        }
        
        .empty-state {
            text-align: center;
            padding: 40px 20px;
            color: #666;
        }
        
        .empty-state i {
            font-size: 48px;
            color: #ddd;
            margin-bottom: 16px;
        }
        
        .status-badge {
            display: inline-block;
            padding: 4px 8px;
            border-radius: 12px;
            font-size: 12px;
            font-weight: 500;
        }
        
        .status-active {
            background: #d4edda;
            color: #155724;
        }
        
        .status-inactive {
            background: #f8d7da;
            color: #721c24;
        }
        
        .loading {
            text-align: center;
            padding: 20px;
            color: #666;
        }
        
        .loading i {
            animation: spin 1s linear infinite;
            margin-right: 8px;
        }
        
        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="search-container">
        <div style="display: flex; justify-content: space-between; align-items: center; margin-bottom: 16px;">
            <h4 style="margin: 0; color: #333;">
                <i class="fas fa-users" style="color: #4CAF50; margin-right: 8px;"></i>
                Müstahsil Listesi
            </h4>
            <asp:Label ID="lblKayitSayisi" runat="server" 
                style="font-size: 14px; color: #666; background: #f8f9fa; padding: 4px 8px; border-radius: 12px;"></asp:Label>
        </div>
        <asp:TextBox ID="txtAra" runat="server" CssClass="search-input" 
            placeholder="Müstahsil adı, soyadı veya telefon ile arayın..." />
        <asp:Button ID="btnAra" runat="server" Text="Ara" 
            style="width: 100%; margin-top: 10px; padding: 12px; background: #4CAF50; color: white; border: none; border-radius: 8px; font-size: 16px;"
            OnClick="txtAra_TextChanged" />
    </div>
    
    <asp:Repeater ID="rptMustahsiller" runat="server">
        <ItemTemplate>
            <div class="mustahsil-card">
                <div class="mustahsil-name">
                    <%# Eval("Ad") %> <%# Eval("Soyad") %>
                    <span class='<%# (Eval("Durum") != null && Convert.ToBoolean(Eval("Durum"))) ? "status-badge status-active" : "status-badge status-inactive" %>'>
                        <%# (Eval("Durum") != null && Convert.ToBoolean(Eval("Durum"))) ? "Aktif" : "Pasif" %>
                    </span>
                </div>
                
                <%# (Eval("Telefon") != null && !string.IsNullOrEmpty(Eval("Telefon").ToString())) ? 
                    "<div class='mustahsil-info'><i class='fas fa-phone'></i>" + Eval("Telefon") + "</div>" : "" %>
                
                <%# (Eval("Email") != null && !string.IsNullOrEmpty(Eval("Email").ToString())) ? 
                    "<div class='mustahsil-info'><i class='fas fa-envelope'></i>" + Eval("Email") + "</div>" : "" %>
                
                <%# (Eval("TCKimlikNo") != null && !string.IsNullOrEmpty(Eval("TCKimlikNo").ToString())) ? 
                    "<div class='mustahsil-info'><i class='fas fa-id-card'></i>TC: " + Eval("TCKimlikNo") + "</div>" : "" %>
                
                <div style="display: flex; gap: 8px; margin-top: 10px;">
                    <a href='YeniMustahsil.aspx?id=<%# Eval("MustahsilID") %>' class="btn-edit" style="flex: 1; text-align: center; margin-top: 0;">
                        <i class="fas fa-edit"></i> Düzenle
                    </a>
                    <a href='MalKabul.aspx?mustahsilID=<%# Eval("MustahsilID") %>' class="btn-malkabul" style="flex: 1; text-align: center;">
                        <i class="fas fa-truck-loading"></i> Mal Kabul
                    </a>
                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
    
    <asp:Panel ID="pnlEmpty" runat="server" Visible="false" CssClass="empty-state">
        <i class="fas fa-users"></i>
        <h5><asp:Label ID="lblEmptyBaslik" runat="server" Text="Henüz müstahsil eklenmemiş"></asp:Label></h5>
        <p><asp:Label ID="lblEmptyAciklama" runat="server" Text="Yeni müstahsil eklemek için aşağıdaki + butonuna tıklayın."></asp:Label></p>
    </asp:Panel>
    
    <!-- Yenileme Butonu -->
    <div style="text-align: center; margin: 20px 0; padding-bottom: 20px;">
        <asp:Button ID="btnYenile" runat="server" Text="Yenile" 
            CssClass="btn-cancel" style="width: auto; margin: 0; padding: 10px 20px; background: #6c757d;"
            OnClick="btnYenile_Click" />
    </div>
    
    <a href="YeniMustahsil.aspx" class="btn-add" title="Yeni Müstahsil Ekle">
        <i class="fas fa-plus"></i>
    </a>
    
    <script>
        // Enter tuşu ile arama
        document.addEventListener('DOMContentLoaded', function() {
            const aramaInput = document.getElementById('<%= txtAra.ClientID %>');
            const aramaButon = document.getElementById('<%= btnAra.ClientID %>');
            
            if (aramaInput && aramaButon) {
                aramaInput.addEventListener('keypress', function(e) {
                    if (e.key === 'Enter') {
                        e.preventDefault();
                        aramaButon.click();
                    }
                });
            }
        });
    </script>
</asp:Content>

