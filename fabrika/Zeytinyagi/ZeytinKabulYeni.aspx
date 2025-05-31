<%@ Page Title="Zeytin Kabul Formu" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="ZeytinKabulYeni.aspx.cs" Inherits="fabrika_Zeytinyagi_ZeytinKabulYeni" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <!-- Form specific styles -->
    <link rel="stylesheet" type="text/css" href="/App_Themes/serdarnas_admin_flat/assets/bootstrap-datepicker/css/datepicker.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/serdarnas_admin_flat/assets/bootstrap-colorpicker/css/colorpicker.css" />
    <link rel="stylesheet" type="text/css" href="/App_Themes/serdarnas_admin_flat/assets/bootstrap-daterangepicker/daterangepicker.css" />
    
    <!-- Select2 library for better multi-select -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/css/select2.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/select2-bootstrap-theme/0.1.0-beta.10/select2-bootstrap.min.css" />
    
    <!-- Custom styles for this form -->
    <style type="text/css">
        .form-group.no-margin {
            margin-bottom: 0;
            margin-left: 0;
            margin-right: 0;
        }
        .form-control {
            margin-bottom: 5px;
        }
        small.text-muted {
            display: block;
            font-size: 11px;
        }
        .input-group {
            width: 100%;
        }
        .select2-container {
            width: 100% !important;
        }
        .select2-container--bootstrap .select2-selection {
            border-radius: 0;
            height: auto;
            min-height: 34px;
        }
        .box-kasa-container {
            border: 1px solid #e2e2e2;
            padding: 5px;
            margin-bottom: 10px;
        }
        .box-kasa-list {
            max-height: 150px;
            overflow-y: auto;
        }
        .box-kasa-counter {
            font-size: 12px;
            color: #666;
            margin-top: 5px;
        }
        .progress-indicator {
            display: inline-block;
            padding: 8px 12px;
            background-color: #ffffd6;
            border: 1px solid #e6e6b8;
            color: #666;
            border-radius: 4px;
            font-size: 13px;
            margin: 10px 0;
        }
        .progress-indicator i {
            margin-right: 5px;
            color: #f39c12;
        }
        .mt-3 {
            margin-top: 15px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <div class="d-flex justify-content-between align-items-center">
                        <h5 class="mb-0">
                            <i class="fa fa-plus-circle me-2"></i>
                            <asp:Literal ID="ltlPageTitle" runat="server">Yeni Zeytin Kabul Kaydı</asp:Literal>
                        </h5>
                        <div>
                            <asp:HyperLink ID="HyperLink1" runat="server" CssClass="btn btn-danger" NavigateUrl="~/fabrika/Zeytinyagi/ZeytinKabul.aspx">
                                <i class="fa fa-arrow-left me-1"></i>Listeye Dön
                            </asp:HyperLink>
                        </div>
                    </div>
                </header>
                <div class="panel-body">
                    <asp:HiddenField ID="hfEditMode" runat="server" Value="0" />
                    <asp:HiddenField ID="hfEditID" runat="server" Value="0" />
                    <asp:HiddenField ID="hfPartiNo" runat="server" Value="" />
                    <asp:HiddenField ID="hfSelectedBoxKasalar" runat="server" Value="" />

                    <div class="form-horizontal">
                        <div class="form-group">
                            <label for="<%=ddlMustahsil.ClientID %>" class="col-sm-2 control-label">Müstahsil <span class="text-danger">*</span></label>
                            <div class="col-sm-10">
                                <div class="input-group">
                                    <asp:DropDownList ID="ddlMustahsil" runat="server" CssClass="form-control" ValidationGroup="ZeytinKabulGroup">
                                    </asp:DropDownList>
                                    <span class="input-group-btn">
                                        <asp:HyperLink ID="lnkYeniMustahsil" runat="server" CssClass="btn btn-success" NavigateUrl="~/fabrika/Mustahsil/YeniMustahsil.aspx">
                                            <i class="fa fa-plus"></i>+ Yeni Müstahsil
                                        </asp:HyperLink>
                                    </span>
                                </div>
                                <asp:RequiredFieldValidator ID="rfvMustahsil" runat="server" ControlToValidate="ddlMustahsil"
                                    ErrorMessage="Lütfen müstahsil seçin" CssClass="text-danger" Display="Dynamic" ValidationGroup="ZeytinKabulGroup">
                                </asp:RequiredFieldValidator>
                            </div>
                        </div>

                        <!-- Parti No ve Geliş Tarihi (aynı satırda) -->
                        <div class="form-group">
                            
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group no-margin">
                                            <label for="<%=txtPartiNo.ClientID %>" class="col-sm-4 control-label">Parti No</label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtPartiNo" runat="server" CssClass="form-control" ReadOnly="true" ValidationGroup="ZeytinKabulGroup"></asp:TextBox>
                                                <small class="text-muted">Otomatik oluşturulur</small>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group no-margin">
                                            <label for="<%=txtGelisTarihi.ClientID %>" class="col-sm-4 control-label">Geliş Tarihi <span class="text-danger">*</span></label>
                                            <div class="col-sm-8">
                                                <asp:TextBox ID="txtGelisTarihi" runat="server" CssClass="form-control" TextMode="DateTimeLocal" ValidationGroup="ZeytinKabulGroup"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvGelisTarihi" runat="server" ControlToValidate="txtGelisTarihi"
                                                    ErrorMessage="Lütfen geliş tarihini girin" CssClass="text-danger" Display="Dynamic" ValidationGroup="ZeytinKabulGroup">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <!-- Ürün ve Miktar (aynı satırda) -->
                        <div class="form-group">
                            
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group no-margin">
                                            <label for="<%=ddlUrun.ClientID %>" class="col-sm-4 control-label">Ürün <span class="text-danger">*</span></label>
                                            <div class="col-sm-8">
                                                <asp:DropDownList ID="ddlUrun" runat="server" CssClass="form-control" ValidationGroup="ZeytinKabulGroup">
                                                </asp:DropDownList>
                                                <asp:RequiredFieldValidator ID="rfvUrun" runat="server" ControlToValidate="ddlUrun"
                                                    ErrorMessage="Lütfen ürün seçin" CssClass="text-danger" Display="Dynamic" ValidationGroup="ZeytinKabulGroup">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="col-sm-6">
                                        <div class="form-group no-margin">
                                            <label for="<%=txtMiktar.ClientID %>" class="col-sm-4 control-label">Miktar (Kg)</label>
                                            <div class="col-sm-8">
                                                <div class="input-group">
                                                    <asp:TextBox ID="txtMiktar" runat="server" CssClass="form-control" TextMode="Number" min="0" placeholder="Örn: 1500" ValidationGroup="ZeytinKabulGroup"></asp:TextBox>
                                                    <span class="input-group-addon">Kg</span>
                                                </div>
                                                <asp:CustomValidator ID="cvMiktar" runat="server" ControlToValidate="txtMiktar"
                                                    ErrorMessage="Geçerli bir miktar girin (1-100000)" CssClass="text-danger"
                                                    Display="Dynamic" ValidationGroup="ZeytinKabulGroup" 
                                                    ClientValidationFunction="ValidateMiktar" ValidateEmptyText="false">
                                                </asp:CustomValidator>
                                                <small class="text-muted">Opsiyonel alan</small>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <!-- Araç Plakası ve Zeytin Box Kasa (aynı satırda) -->
                        <div class="form-group">
                           
                            <div class="col-sm-12">
                                <div class="row">
                                    <div class="col-sm-6">
                                        <div class="form-group no-margin">
                                            <label for="<%=txtPlakaNo.ClientID %>" class="col-sm-4 control-label">Araç Plakası</label>
                                            <div class="col-sm-8">
                                                <div class="input-group">
                                                    <span class="input-group-addon"><i class="fa fa-car"></i></span>
                                                    <asp:TextBox ID="txtPlakaNo" runat="server" CssClass="form-control" placeholder="34ABC123" ValidationGroup="ZeytinKabulGroup"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                              
                                </div>
                            </div>
                        </div>


                        <div class="form-group">
                            <div class="col-lg-offset-2 col-lg-10">
                                <asp:Button ID="btnKaydet" runat="server" Text="Kaydet ve Kasa Ekle" CssClass="btn btn-primary"
                                    OnClick="btnKaydet_Click" ValidationGroup="ZeytinKabulGroup"
                                    OnClientClick="if(!Page_ClientValidate('ZeytinKabulGroup')) return false;" />
                                <asp:Button ID="btnDeleteRecord" runat="server" Text="Kaydı Sil" 
                                    CssClass="btn btn-danger" OnClick="btnDeleteRecord_Click" Visible="false"
                                    OnClientClick="return confirm('Bu kaydı silmek istediğinizden emin misiniz?');" />
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>

    <!-- Kayıttan sonra Box Kasa ekleme paneli -->
    <asp:Panel ID="pnlBoxKasaManagement" runat="server" Visible="false">
        <div class="row">
            <div class="col-lg-12">
                <section class="panel">
                    <header class="panel-heading">
                        <h5 class="mb-0">
                            <i class="fa fa-cubes me-2"></i> Box Kasa Yönetimi
                        </h5>
                    </header>
                    <div class="panel-body">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Kullanılabilir Box Kasalar</label>
                                            <!-- Arama kutusu ekle -->
                                            <div class="input-group" style="margin-bottom: 5px;">
                                                <span class="input-group-addon"><i class="fa fa-search"></i></span>
                                                <asp:TextBox ID="txtSearchBoxKasa" runat="server" CssClass="form-control" 
                                                    placeholder="Box kasa numarası ile ara (örn: 10, 25, 100)..." 
                                                    AutoPostBack="true" OnTextChanged="txtSearchBoxKasa_TextChanged"></asp:TextBox>
                                            </div>
                                            <div class="box-kasa-container">
                                                <asp:ListBox ID="lstAvailableBoxKasas" runat="server" CssClass="form-control box-kasa-list" 
                                                    SelectionMode="Multiple">
                                                </asp:ListBox>
                                                <div class="box-kasa-counter">
                                                    <span id="availableBoxCount">0 box kasa bulundu</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnAddBoxKasas" runat="server" Text="Seçilen Kasaları Ekle" 
                                                CssClass="btn btn-success" OnClick="btnAddBoxKasas_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <label>Bu Üretime Ekli Box Kasalar</label>
                                            <div class="box-kasa-container">
                                                <asp:ListBox ID="lstAssignedBoxKasas" runat="server" CssClass="form-control box-kasa-list" 
                                                    SelectionMode="Multiple">
                                                </asp:ListBox>
                                                <div class="box-kasa-counter">
                                                    <span id="assignedBoxCount">0 box kasa eklenmiş</span>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="form-group">
                                            <asp:Button ID="btnRemoveBoxKasas" runat="server" Text="Seçilen Kasaları Kaldır" 
                                                CssClass="btn btn-warning" OnClick="btnRemoveBoxKasas_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="form-group">
                                    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                        <ProgressTemplate>
                                            <div class="progress-indicator">
                                                <i class="fa fa-spinner fa-spin"></i> İşlem yapılıyor...
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnAddBoxKasas" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnRemoveBoxKasas" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="txtSearchBoxKasa" EventName="TextChanged" />
                            </Triggers>
                        </asp:UpdatePanel>
                        <div class="form-group mt-3">
                            <asp:Button ID="btnCompleteProcess" runat="server" Text="İşlemi Tamamla" 
                                CssClass="btn btn-primary" OnClick="btnCompleteProcess_Click" />
                        </div>
                    </div>
                </section>
            </div>
        </div>
    </asp:Panel>

    <!-- Form specific script files -->
    <script type="text/javascript" src="/App_Themes/serdarnas_admin_flat/assets/bootstrap-datepicker/js/bootstrap-datepicker.js"></script>
    <script type="text/javascript" src="/App_Themes/serdarnas_admin_flat/assets/bootstrap-daterangepicker/date.js"></script>
    <script type="text/javascript" src="/App_Themes/serdarnas_admin_flat/assets/bootstrap-daterangepicker/daterangepicker.js"></script>
    <script type="text/javascript" src="/App_Themes/serdarnas_admin_flat/assets/bootstrap-inputmask/bootstrap-inputmask.min.js"></script>
    <script type="text/javascript" src="/App_Themes/serdarnas_admin_flat/js/form-component.js"></script>
    
    <!-- Select2 for enhanced multi-select -->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/select2/4.0.13/js/select2.min.js"></script>
    <script type="text/javascript">
        // Show success message notification
        function showSuccessMessage(title, message) {
            // Check if we have toastr library available
            if (typeof toastr !== 'undefined') {
                toastr.success(message, title);
            } else {
                alert(title + ': ' + message);
            }
        }
        
        // Show error message notification
        function showErrorMessage(title, message) {
            // Check if we have toastr library available
            if (typeof toastr !== 'undefined') {
                toastr.error(message, title);
            } else {
                alert(title + ': ' + message);
            }
        }
        
        // Custom validator for Miktar field
        function ValidateMiktar(source, args) {
            // If empty, consider it valid (optional field)
            if (args.Value === '') {
                args.IsValid = true;
                return;
            }
            
            // If not empty, check if it's a number within range
            var value = parseInt(args.Value);
            args.IsValid = !isNaN(value) && value >= 0 && value <= 100000;
        }
        
        // Function to initialize Select2 controls
        function initializeSelect2() {
            // Initialize Select2 for available box kasas
            $('#<%=lstAvailableBoxKasas.ClientID%>').select2({
                theme: 'bootstrap',
                placeholder: 'Eklenecek kasaları seçiniz',
                allowClear: true,
                closeOnSelect: false,
                width: '100%',
                dropdownParent: $('#<%=UpdatePanel1.ClientID%>'),
                language: {
                    noResults: function() {
                        return "Sonuç bulunamadı";
                    },
                    searching: function() {
                        return "Aranıyor...";
                    }
                }
            });
            
            // Initialize Select2 for assigned box kasas
            $('#<%=lstAssignedBoxKasas.ClientID%>').select2({
                theme: 'bootstrap',
                placeholder: 'Kaldırılacak kasaları seçiniz',
                allowClear: true,
                closeOnSelect: false,
                width: '100%',
                dropdownParent: $('#<%=UpdatePanel1.ClientID%>'),
                language: {
                    noResults: function() {
                        return "Sonuç bulunamadı";
                    },
                    searching: function() {
                        return "Aranıyor...";
                    }
                }
            });
        }

        $(document).ready(function() {
            // Initialize on page load
            initializeSelect2();
            
            // Re-initialize Select2 after each AsyncPostBack
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            
            prm.add_endRequest(function() {
                // Re-initialize Select2 after AJAX update
                initializeSelect2();
            });
        });
    </script>
</asp:Content>

