<%@ Page Title="Müşteri Satış" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="MusteriSatis.aspx.cs" Inherits="fabrika_Musteriler_MusteriSatis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Müşteri Satış İşlemi</h3>
                </header>
                <!-- İşlem Butonları -->
                <div class="panel-body">
                    <div class="btn-group">
                        <asp:LinkButton ID="btnProformaSiparis" runat="server" CssClass="btn btn-success" Style="margin-right: 5px;">
                    <i class="icon-file-text"></i> Proforma/Sipariş Kaydet
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnIrsaliyeKaydet" runat="server" CssClass="btn btn-info" Style="margin-right: 5px;">
                    <i class="icon-truck"></i> İrsaliye Kaydet
                        </asp:LinkButton>
                        <asp:LinkButton ID="btnFaturaKaydet" runat="server" CssClass="btn btn-danger" Style="margin-right: 5px;">
                    <i class="icon-file"></i> Fatura Kaydet
                        </asp:LinkButton>
                        <asp:HyperLink ID="hplbtnGeriDon" runat="server" CssClass="btn btn-warning"><i class="icon-arrow-left"></i>Geri Dön</asp:HyperLink>
                        
                            
                  
                    </div>
                </div>
            </section>
        </div>
    </div>

    <div class="row">
        <!-- Müşteri Bilgileri Bölümü -->
        <div class="col-md-4">
            <div class="panel panel-primary" style="border-color: #3a87ad;">
                <div class="panel-heading" style="background-color: #3a87ad;">
                    <h3 class="panel-title">
                        <asp:Label ID="lblMusteriAd" runat="server" Text="Label"></asp:Label></h3>
                    <span class="pull-right">
                        <asp:LinkButton ID="btnBilgiDuzenle" runat="server" CssClass="btn btn-xs btn-default">
                            <i class="icon-info-sign"></i>
                        </asp:LinkButton>
                    </span>
                </div>
                <div class="panel-body">
                    <div class="form-horizontal">
                        <!-- Belge No -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Belge No:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtBelgeNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Tarih -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Tarih:</label>
                            <div class="col-sm-6">
                                <asp:TextBox ID="txtTarih" runat="server" CssClass="form-control" placeholder="06.05.2025"></asp:TextBox>
                            </div>
                            <div class="col-sm-2">
                                <asp:TextBox ID="txtSaat" runat="server" CssClass="form-control" placeholder="17:20"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Vadesi -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Vadesi:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtVadesi" runat="server" CssClass="form-control" placeholder="06.05.2025"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Sipariş Tarih/No -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Sipariş Tarih/No:</label>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSiparisTarih" runat="server" CssClass="form-control" placeholder="Sipariş tarihi"></asp:TextBox>
                            </div>
                            <div class="col-sm-4">
                                <asp:TextBox ID="txtSiparisNo" runat="server" CssClass="form-control" placeholder="Sipariş no"></asp:TextBox>
                            </div>
                        </div>

                        <!-- İrsaliye No -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">İrsaliye No:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtIrsaliyeNo" runat="server" CssClass="form-control"></asp:TextBox>
                            </div>
                        </div>

                        <!-- Sevk Tarihi -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Sevk Tarihi:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtSevkTarihi" runat="server" CssClass="form-control" placeholder="06.05.2025"></asp:TextBox>
                            </div>
                        </div>
                        <!-- Proje -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Pazarlamaci:</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlPazarlama" runat="server" CssClass="form-control" DataTextField="Ad" DataValueField="PazarlamaciID">
                                </asp:DropDownList>
                                <small class="text-muted">(irsaliye için)</small>
                            </div>
                        </div>

                        <!-- Proje -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Proje:</label>
                            <div class="col-sm-8">
                                <asp:DropDownList ID="ddlProje" runat="server" CssClass="form-control" DataTextField="Ad" DataValueField="ProjeID">
                                </asp:DropDownList>
                                <small class="text-muted">(irsaliye için)</small>
                            </div>
                        </div>

                        <!-- Açıklama -->
                        <div class="form-group">
                            <label class="col-sm-4 control-label">Açıklama:</label>
                            <div class="col-sm-8">
                                <asp:TextBox ID="txtAciklama" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Ürün/Hizmetler Bölümü -->
        <div class="col-md-8">
            <div class="panel panel-success" style="border-color: #5cb85c;">
                <div class="panel-heading" style="background-color: #5cb85c;">
                    <h3 class="panel-title">ÜRÜN / HİZMETLER</h3>
                </div>
                <div class="panel-body">
                    <div class="form-group">
                        <div class="input-group">
                            <asp:TextBox ID="txtUrunAra" runat="server" CssClass="form-control" placeholder="Ürün isminden arayın veya barkod okutun"></asp:TextBox>
                            <span class="input-group-btn">
                                <asp:LinkButton ID="btnUrunAra" runat="server" CssClass="btn btn-primary">
                                    <i class="icon-search"></i> Ara
                                </asp:LinkButton>
                            </span>
                        </div>
                    </div>

                    <div class="alert alert-info">
                        <p>Henüz ürün eklenmedi. Ürün eklemek için <a href="#" class="alert-link">tıklayın</a>.</p>
                    </div>

                    <!-- Ürün Tablosu (Boş) -->
                    <div class="table-responsive">
                        <asp:GridView ID="gvUrunler" runat="server" AutoGenerateColumns="false" CssClass="table table-striped table-hover table-bordered" Width="100%">
                            <Columns>
                                <asp:BoundField DataField="UrunKodu" HeaderText="Ürün Kodu" />
                                <asp:BoundField DataField="UrunAdi" HeaderText="Ürün Adı" />
                                <asp:BoundField DataField="Miktar" HeaderText="Miktar" />
                                <asp:BoundField DataField="Birim" HeaderText="Birim" />
                                <asp:BoundField DataField="BirimFiyat" HeaderText="Birim Fiyat" />
                                <asp:BoundField DataField="Iskonto" HeaderText="İskonto" />
                                <asp:BoundField DataField="KDV" HeaderText="KDV" />
                                <asp:BoundField DataField="Tutar" HeaderText="Tutar" />
                                <asp:TemplateField HeaderText="İşlemler">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnDuzenle" runat="server" CssClass="btn btn-xs btn-primary">
                                            <i class="icon-pencil"></i>
                                        </asp:LinkButton>
                                        <asp:LinkButton ID="btnSil" runat="server" CssClass="btn btn-xs btn-danger">
                                            <i class="icon-trash"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <div class="text-center">Henüz ürün eklenmedi</div>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- İşlem Butonları -->


    <script type="text/javascript">
        $(document).ready(function () {
            // Tarih alanları için datepicker
            $("#<%=txtTarih.ClientID %>").datepicker({
                format: 'dd.mm.yyyy',
                autoclose: true,
                language: 'tr'
            });

            $("#<%=txtVadesi.ClientID %>").datepicker({
                format: 'dd.mm.yyyy',
                autoclose: true,
                language: 'tr'
            });

            $("#<%=txtSevkTarihi.ClientID %>").datepicker({
                format: 'dd.mm.yyyy',
                autoclose: true,
                language: 'tr'
            });

            $("#<%=txtSiparisTarih.ClientID %>").datepicker({
                format: 'dd.mm.yyyy',
                autoclose: true,
                language: 'tr'
            });
        });
    </script>
</asp:Content>

