<%@ Page Title="Kullanıcı Yetki Yönetimi" Language="C#" MasterPageFile="~/yonetim/YonetimMasterPage.master" AutoEventWireup="true" CodeFile="KullaniciYetki.aspx.cs" Inherits="yonetim_KullaniciYetki" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="row">
        <div class="col-lg-12">
            <h3 class="page-header"><i class="fa fa-key"></i> Kullanıcı Yetki Yönetimi</h3>
        </div>
    </div>
    
    <!-- Hata ve Başarı Mesajları -->
    <asp:Panel ID="pnlHata" runat="server" CssClass="alert alert-block alert-danger fade in" Visible="false">
        <button data-dismiss="alert" class="close close-sm" type="button">
            <i class="fa fa-times"></i>
        </button>
        <h4><i class="fa fa-times-circle"></i> Hata!</h4>
        <asp:Label ID="lblHata" runat="server" Text=""></asp:Label>
    </asp:Panel>
    
    <asp:Panel ID="pnlBasari" runat="server" CssClass="alert alert-block alert-success fade in" Visible="false">
        <button data-dismiss="alert" class="close close-sm" type="button">
            <i class="fa fa-times"></i>
        </button>
        <h4><i class="fa fa-check-circle"></i> Başarılı!</h4>
        <asp:Label ID="lblBasari" runat="server" Text=""></asp:Label>
    </asp:Panel>
    
    <div class="row">
        <div class="col-md-12">
            <section class="panel">
                <header class="panel-heading">
                    Kullanıcı Yetki Yönetimi
                </header>
                <div class="panel-body">
                    <div class="row">
                        <div class="col-md-5">
                            <div class="form-group">
                                <label>Kullanıcı Seçin</label>
                                <asp:DropDownList ID="ddlKullanici" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="ddlKullanici_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                        </div>
                    </div>
                    
                    <div class="row">
                        <div class="col-md-12">
                            <div class="table-responsive">
                                <asp:GridView ID="gvYetkiler" runat="server" CssClass="table table-striped table-bordered table-hover"
                                    AutoGenerateColumns="false" OnRowCommand="gvYetkiler_RowCommand">
                                    <Columns>
                                        <asp:BoundField DataField="YetkiKodu" HeaderText="Yetki Kodu" />
                                        <asp:BoundField DataField="MenuAdi" HeaderText="Menü Adı" />
                                        <asp:CheckBoxField DataField="YetkiDurumu" HeaderText="Yetki Durumu" />
                                        <asp:TemplateField HeaderText="İşlemler">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbYetkiVer" runat="server" CommandName="YetkiVer" CommandArgument='<%# Eval("YetkiKodu") %>' 
                                                    CssClass='<%# Convert.ToBoolean(Eval("YetkiDurumu")) ? "btn btn-xs btn-danger" : "btn btn-xs btn-success" %>' 
                                                    ToolTip='<%# Convert.ToBoolean(Eval("YetkiDurumu")) ? "Yetkiyi Kaldır" : "Yetki Ver" %>'>
                                                    <i class='<%# Convert.ToBoolean(Eval("YetkiDurumu")) ? "fa fa-times" : "fa fa-check" %>'></i>
                                                    <%# Convert.ToBoolean(Eval("YetkiDurumu")) ? " Yetkiyi Kaldır" : " Yetki Ver" %>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <div class="alert alert-info">
                                            Henüz bir menü kaydı bulunmamaktadır. Önce menü ekleyiniz.
                                        </div>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content> 