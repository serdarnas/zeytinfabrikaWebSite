<%@ Page Title="" Language="C#" MasterPageFile="~/fabrika/FabrikaMasterPage.master" AutoEventWireup="true" CodeFile="YeniUrun.aspx.cs" Inherits="fabrika_Urunler_YeniUrun" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="row">
        <div class="col-lg-12">
            <section class="panel">
                <header class="panel-heading">
                    <h3>Yeni Ürün Kayit</h3>
                </header>
                <!-- İşlem Butonları -->
                <div class="panel-body">
                    <div class="btn-group">
                        <%--<asp:Button ID="btnKaydet" runat="server" Text="Kaydet" CssClass="btn btn-shadow btn-success" Style="margin-right: 5px;"/>--%>
                        <asp:LinkButton ID="btnKaydet" runat="server" CssClass="btn btn-shadow btn-success" Style="margin-right: 5px;" OnClick="btnKaydet_Click">
                            <i class="icon-file-text"></i> Kaydet
                        </asp:LinkButton>
                        <asp:HyperLink ID="hplbtnGeriDon" runat="server" CssClass="btn btn-shadow btn-warning" NavigateUrl="~/fabrika/Urunler/UrunListesi.aspx"><i class="icon-arrow-left"></i>Geri Dön</asp:HyperLink>



                    </div>
                </div>
            </section>
        </div>
    </div>
    <div class="row">
        <div class="col-lg-11">
            <!--widget start-->
            <section class="panel">
                <header class="panel-heading tab-bg-dark-navy-blue">
                    <ul class="nav nav-tabs nav-justified ">
                        <li class="active">
                            <a href="#tanim" data-toggle="tab">Tanimlar
                            </a>
                        </li>
                        <li>
                            <a href="#fiyat" data-toggle="tab">Fiyat
                            </a>
                        </li>
                        <li class="">
                            <a href="#detaylar" data-toggle="tab">Detaylar
                            </a>
                        </li>
                        <li class="">
                            <a href="#resimler" data-toggle="tab">Resimler
                            </a>
                        </li>
                        <li class="">
                            <a href="#varyant" data-toggle="tab">Varyanlar
                            </a>
                        </li>
                        
                    </ul>
                </header>
                <div class="panel-body">
                    <div class="tab-content tasi-tab">
                        <div class="tab-pane active" id="tanim">
                            <asp:Panel ID="pnlUrunForm" runat="server">
                                <asp:HiddenField ID="hfUrunID" runat="server" />



                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtUrunAdi">Ürün Adı</label>
                                        <asp:TextBox ID="txtUrunAdi" runat="server" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="ddlKategoriID">Kategori</label>
                                        <asp:DropDownList ID="ddlKategoriID" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="ddlBirimID">Birim</label>
                                        <asp:DropDownList ID="ddlBirimID" runat="server" CssClass="form-control"></asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtStokMiktari">Stok Miktarı</label>
                                        <asp:TextBox ID="txtStokMiktari" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="txtMinimumStok">Minimum Stok</label>
                                        <asp:TextBox ID="txtMinimumStok" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="chkUrunTipiStoklu">Stoklu Ürün</label>
                                        <asp:CheckBox ID="chkUrunTipiStoklu" runat="server" />
                                    </div>
                                </div>
                                <div class="col-md-3">
                                    <div class="form-group">
                                        <label for="chkDurum">Durum (Aktif/Pasif)</label>
                                        <asp:CheckBox ID="chkDurum" runat="server" />
                                    </div>
                                </div>
                             
                            </asp:Panel>
                        </div>
                        <div class="tab-pane" id="fiyat">

                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="txtAlisFiyati">Alış Fiyatı</label>
                                    <asp:TextBox ID="txtAlisFiyati" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="txtAlisKdv">Alış KDV</label>
                                    <asp:TextBox ID="txtAlisKdv" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="ddlAlisParaBirimi">Alış Para Birimi</label>
                                    <asp:DropDownList ID="ddlAlisParaBirimi" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="chkAlisFiyatiKdvDahilmi">Alış Fiyatı KDV Dahil mi?</label>
                                    <asp:CheckBox ID="chkAlisFiyatiKdvDahilmi" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="txtSatisFiyati">Satış Fiyatı</label>
                                    <asp:TextBox ID="txtSatisFiyati" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="txtSatisKdv">Satış KDV</label>
                                    <asp:TextBox ID="txtSatisKdv" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="ddlSatisParaBirimi">Satış Para Birimi</label>
                                    <asp:DropDownList ID="ddlSatisParaBirimi" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="chkSatisFiyatiKdvDahilmi">Satış Fiyatı KDV Dahil mi?</label>
                                    <asp:CheckBox ID="chkSatisFiyatiKdvDahilmi" runat="server" />
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="ddlParaBirimiID">Para Birimi</label>
                                    <asp:DropDownList ID="ddlParaBirimiID" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="txtKDVOrani">KDV Oranı</label>
                                    <asp:TextBox ID="txtKDVOrani" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane " id="detaylar">

                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="ddlMarkaID">Marka</label>
                                    <asp:DropDownList ID="ddlMarkaID" runat="server" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="txtUrunKodu">Ürün Kodu</label>
                                    <asp:TextBox ID="txtUrunKodu" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="txtBarkod">Barkod</label>
                                    <asp:TextBox ID="txtBarkod" runat="server" CssClass="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-group">
                                    <label for="txtNotlar">Notlar</label>
                                    <asp:TextBox ID="txtNotlar" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="3"></asp:TextBox>
                                </div>
                            </div>
                            <div class="col-md-3">
                                <div class="form-group">
                                    <label for="txtOlusturmaTarihi">Oluşturma Tarihi</label>
                                    <asp:TextBox ID="txtOlusturmaTarihi" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="tab-pane" id="resimler">
                            <asp:FileUpload ID="fuResimler" runat="server" AllowMultiple="true" CssClass="form-control" />
                            <asp:Button ID="btnResimYukle" runat="server" Text="Resimleri Yükle" CssClass="btn btn-info" OnClick="btnResimYukle_Click" />
                            <hr />
                            <asp:Repeater ID="rptResimler" runat="server">
                                <ItemTemplate>
                                    <div class="col-md-3" style="margin-bottom:10px;">
                                        <asp:Image ID="imgUrunResim" runat="server" ImageUrl='<%# Eval("ImageUrl") %>' CssClass="img-thumbnail" Width="150" Height="150" />
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="tab-pane" id="varyant">
                            Varyantlar
                        </div>
                    </div>
                </div>
            <div class="footer-legal">
                <div class="form-group">
                    <asp:Button ID="btnKaydet_" runat="server" Text="Kaydet" CssClass="btn btn-success" OnClick="btnKaydet_Click" />
                    <asp:Button ID="btnGuncelle" runat="server" Text="Güncelle" CssClass="btn btn-primary" OnClick="btnGuncelle_Click" />
                    </div>
                </div>
            </section>
            <!--widget end-->
        </div>
    </div>
</asp:Content>

