<%@ Page Title="Menü Yönetimi" Language="C#" MasterPageFile="~/yonetim/YonetimMasterPage.master" AutoEventWireup="true" CodeFile="MenuYonetimi.aspx.cs" Inherits="yonetim_MenuYonetimi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    
    <div class="row">
        <div class="col-lg-12">
            <h3 class="page-header"><i class="fa fa-list"></i> Menü Yönetimi</h3>
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
    
    <!-- Toplu İşlem Butonları -->
    <div class="row margin-bottom-10">
        <div class="col-md-12">
            <div class="btn-group">
                <asp:Button ID="btnTemizleTekrarlar" runat="server" Text="Tekrarlanan URL'leri Temizle" CssClass="btn btn-warning" OnClick="btnTemizleTekrarlar_Click" CausesValidation="false" OnClientClick="return confirm('Tekrarlanan URL\'leri temizlemek istediğinize emin misiniz? Bu işlem geri alınamaz.');" />
            </div>
        </div>
    </div>
    
    <div class="row">
        <!-- Menü Ağacı ve Grid Görünümü -->
        <div class="col-md-8">
            <section class="panel">
                <header class="panel-heading custom-tab">
                    <ul class="nav nav-tabs" id="menuTab">
                        <li class="active"><a href="#treeView" data-toggle="tab"><i class="fa fa-sitemap"></i> Ağaç Görünümü</a></li>
                        <li><a href="#gridView" data-toggle="tab"><i class="fa fa-table"></i> Tablo Görünümü</a></li>
                    </ul>
                </header>
                
                <div class="panel-body">
                    <div class="tab-content">
                        <!-- Ağaç Görünümü -->
                        <div class="tab-pane active" id="treeView">
                            <div style="max-height: 500px; overflow-y: auto;">
                                <asp:TreeView ID="tvMenuler" runat="server" 
                                    ShowLines="true" 
                                    NodeIndent="15"
                                    OnSelectedNodeChanged="tvMenuler_SelectedNodeChanged"
                                    CssClass="menu-tree">
                                    <HoverNodeStyle Font-Bold="true" ForeColor="#0c7cd5" />
                                    <SelectedNodeStyle BackColor="#0c7cd5" Font-Bold="true" ForeColor="White" />
                                </asp:TreeView>
                            </div>
                        </div>
                        
                        <!-- Tablo Görünümü -->
                        <div class="tab-pane" id="gridView">
                            <asp:GridView ID="gvMenuler" runat="server" 
                                AutoGenerateColumns="false" 
                                CssClass="table table-striped table-bordered table-hover"
                                DataKeyNames="MenuID"
                                OnRowCommand="gvMenuler_RowCommand"
                                OnRowDeleting="gvMenuler_RowDeleting"
                                OnRowDataBound="gvMenuler_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="MenuID" HeaderText="ID" />
                                    <asp:BoundField DataField="MenuAdi" HeaderText="Menü Adı" />
                                    <asp:BoundField DataField="SayfaURL" HeaderText="Sayfa URL" />
                                    <asp:BoundField DataField="Sira" HeaderText="Sıra" />
                                    <asp:BoundField DataField="UstMenuAdi" HeaderText="Üst Menü" />
                                    <asp:BoundField DataField="YetkiKodu" HeaderText="Yetki Kodu" />
                                    <asp:TemplateField HeaderText="İşlemler">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbDuzenle" runat="server" CommandName="Duzenle" CommandArgument='<%# Eval("MenuID") %>' CssClass="btn btn-xs btn-primary" ToolTip="Düzenle">
                                                <i class="fa fa-edit"></i>
                                            </asp:LinkButton>
                                            <asp:LinkButton ID="lbSil" runat="server" CommandName="Delete" CommandArgument='<%# Eval("MenuID") %>' CssClass="btn btn-xs btn-danger" ToolTip="Sil" OnClientClick="return confirm('Bu menüyü silmek istediğinize emin misiniz?');">
                                                <i class="fa fa-trash-o"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <div class="alert alert-info">
                                        Kayıtlı menü bulunamadı.
                                    </div>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        
        <!-- Menü Ekleme/Düzenleme Formu -->
        <div class="col-md-4">
            <section class="panel">
                <header class="panel-heading">
                    <i class="fa fa-edit"></i> <asp:Literal ID="ltBaslik" runat="server" Text="Yeni Menü Ekle"></asp:Literal>
                </header>
                
                <div class="panel-body">
                    <div class="form-horizontal">
                        <asp:HiddenField ID="hfMenuID" runat="server" Value="0" />
                        
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Üst Menü</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlUstMenu" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="Ana Menü" Value="0"></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Menü Adı <span class="text-danger">*</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtMenuAdi" runat="server" CssClass="form-control" placeholder="Menü Adı"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvMenuAdi" runat="server" ControlToValidate="txtMenuAdi" ErrorMessage="Menü adı giriniz" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-4 control-label">İkon</label>
                            <div class="col-sm-8">
                                <div class="input-group">
                                    <span class="input-group-addon"><i id="iconPreview" class="fa fa-list"></i></span>
                                    <asp:TextBox ID="txtIkon" runat="server" CssClass="form-control" placeholder="fa-list"></asp:TextBox>
                                </div>
                                <span class="help-block">Örnek: fa-list, fa-user, fa-home</span>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Sayfa URL</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtSayfaURL" runat="server" CssClass="form-control" placeholder="/fabrika/Default.aspx"></asp:TextBox>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Sıra <span class="text-danger">*</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtSira" runat="server" CssClass="form-control" placeholder="10" TextMode="Number"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvSira" runat="server" ControlToValidate="txtSira" ErrorMessage="Sıra numarası giriniz" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                                <asp:RangeValidator ID="rvSira" runat="server" ControlToValidate="txtSira" Type="Integer" MinimumValue="0" MaximumValue="1000" ErrorMessage="0-1000 arası bir değer giriniz" ForeColor="Red" Display="Dynamic"></asp:RangeValidator>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Yetki Kodu <span class="text-danger">*</span></label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtYetkiKodu" runat="server" CssClass="form-control" placeholder="MenuYonetimi"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvYetkiKodu" runat="server" ControlToValidate="txtYetkiKodu" ErrorMessage="Yetki kodu giriniz" ForeColor="Red" Display="Dynamic"></asp:RequiredFieldValidator>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <div class="col-sm-offset-4 col-sm-8">
                                <asp:Button ID="btnKaydet" runat="server" Text="Kaydet" CssClass="btn btn-success" OnClick="btnKaydet_Click" />
                                <asp:Button ID="btnVazgec" runat="server" Text="Vazgeç" CssClass="btn btn-default" OnClick="btnVazgec_Click" CausesValidation="false" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>
    
    <script>
        $(document).ready(function () {
            // İkon önizleme
            $("#<%= txtIkon.ClientID %>").on("keyup", function () {
                var iconClass = $(this).val();
                if (iconClass) {
                    $("#iconPreview").attr("class", "fa " + iconClass);
                } else {
                    $("#iconPreview").attr("class", "fa fa-list");
                }
            });
        });
    </script>
</asp:Content>

