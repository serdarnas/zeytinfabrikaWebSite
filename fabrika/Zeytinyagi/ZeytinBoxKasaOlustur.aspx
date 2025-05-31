<%@ Page Title="Zeytin Box Kasa Oluştur" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="ZeytinBoxKasaOlustur.aspx.cs" Inherits="fabrika_Zeytinyagi_ZeytinBoxKasaOlustur" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Hata ve Başarı Mesajları -->
    
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading d-flex justify-content-between align-items-center">
                    <h3 class="mb-0">Zeytin Box Kasa Yönetimi</h3>
                    <div>
                        <asp:Button ID="btnYeniKasa" runat="server" Text="Yeni Kasa Üret" CssClass="btn btn-shadow btn-success" OnClick="btnYeniKasa_Click" />
                        <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-shadow btn-danger" NavigateUrl="~/fabrika/Zeytinyagi/Default.aspx">İşletme Paneli</asp:HyperLink>
                    </div>
                </header>
                <div class="panel-body">
                    <!-- Yeni Kasa Oluşturma Paneli -->
                    <asp:Panel ID="pnlYeniKasa" runat="server" CssClass="card card-body shadow-sm mb-4" Visible="false">
                        <h5 class="card-title mb-3">Yeni Zeytin Box Kasa Oluştur</h5>
                        <div class="row g-3">
                            <div class="col-md-6">
                                <label for="txtKasaSayisi" class="form-label">Kaç Adet Kasa Oluşturulacak?</label>
                                <asp:TextBox ID="txtKasaSayisi" runat="server" CssClass="form-control" TextMode="Number" min="1" Value="1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvKasaSayisi" runat="server" ControlToValidate="txtKasaSayisi" 
                                    ErrorMessage="Lütfen kasa sayısı girin." CssClass="text-danger" Display="Dynamic">
                                </asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rvKasaSayisi" runat="server" ControlToValidate="txtKasaSayisi"
                                    ErrorMessage="Geçerli bir sayı girin (1-100)." Type="Integer" MinimumValue="1" MaximumValue="100"
                                    CssClass="text-danger" Display="Dynamic">
                                </asp:RangeValidator>
                            </div>
                            <div class="col-md-6">
                                <label for="txtAlimTarihi" class="form-label">Alım Tarihi</label>
                                <asp:TextBox ID="txtAlimTarihi" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvAlimTarihi" runat="server" ControlToValidate="txtAlimTarihi" 
                                    ErrorMessage="Lütfen alım tarihi seçin." CssClass="text-danger" Display="Dynamic">
                                </asp:RequiredFieldValidator>
                            </div>
                            <div class="col-12 text-end">
                                <asp:Button ID="btnIptal" runat="server" Text="İptal" CssClass="btn btn-secondary" OnClick="btnIptal_Click" CausesValidation="false" />
                                <asp:Button ID="btnOlustur" runat="server" Text="Kasaları Oluştur" CssClass="btn btn-primary" OnClick="btnOlustur_Click" />
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </section>
        </div>
    </div>

    <!-- Zeytin Box Kasa Listesi -->
    <div class="card shadow-sm">
        <div class="card-header bg-light d-flex justify-content-between align-items-center">
            <span><i class="fa fa-list"></i> Zeytin Box Kasa Listesi</span>
            <div class="w-50">
                <asp:TextBox ID="txtArama" runat="server" CssClass="form-control form-control-sm" placeholder="Kasa No veya Durum..." 
                    OnTextChanged="txtArama_TextChanged" AutoPostBack="true"></asp:TextBox>
            </div>
        </div>
        <div class="card-body">
            <div class="table-responsive">
                <asp:GridView ID="gvBoxKasalar" runat="server" AutoGenerateColumns="false" 
                    CssClass="table table-striped table-hover table-sm" 
                    DataKeyNames="ZeytinBoxKasaID" 
                    OnRowCommand="gvBoxKasalar_RowCommand"
                    AllowPaging="true" 
                    PageSize="10" 
                    OnPageIndexChanging="gvBoxKasalar_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="ZeytinBoxKasaID" HeaderText="Kasa ID" />
                        <asp:BoundField DataField="ZeytinBoxNo" HeaderText="Box No" />
                        <asp:TemplateField HeaderText="Durum">
                            <ItemTemplate>
                                <asp:Label ID="lblDurum" runat="server" CssClass='<%# Convert.ToBoolean(Eval("Durumu")) ? "badge bg-success" : "badge bg-danger" %>'>
                                    <%# Convert.ToBoolean(Eval("Durumu")) ? "Aktif" : "Kullanılmış" %>
                                </asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="AlimTarihi" HeaderText="Alım Tarihi" DataFormatString="{0:dd.MM.yyyy}" />
                        <asp:BoundField DataField="MustahsilAdi" HeaderText="Kullanan Müstahsil" NullDisplayText="-" />
                        <asp:TemplateField HeaderText="İşlemler" ItemStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnDuzenle" runat="server" CssClass="btn btn-sm btn-primary" CommandName="Duzenle" CommandArgument='<%# Eval("ZeytinBoxKasaID") %>' ToolTip="Düzenle">
                                    <i class="fa fa-pencil"></i>
                                </asp:LinkButton>
                                <asp:LinkButton ID="btnSil" runat="server" CssClass='<%# Convert.ToBoolean(Eval("Durumu")) ? "btn btn-sm btn-danger" : "btn btn-sm btn-secondary disabled" %>' 
                                    CommandName="Sil" CommandArgument='<%# Eval("ZeytinBoxKasaID") %>' ToolTip="Sil" 
                                    OnClientClick="return confirm('Bu kasayı silmek istediğinize emin misiniz?');">
                                    <i class="fa fa-trash"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="pagination pagination-sm justify-content-end" />
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="İlk" LastPageText="Son" />
                    <EmptyDataTemplate>
                        <div class="alert alert-info">Kayıt bulunamadı.</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

