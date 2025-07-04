<%@ Page Title="" Language="C#" MasterPageFile="~/PerakendeSatis/PerakendeMasterPage.master" AutoEventWireup="true" CodeFile="Satis.aspx.cs" Inherits="PerakendeSatis_Satis" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Web.UI" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Perakende Satış</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.1/font/bootstrap-icons.css">
    <style>
        :root {
            --primary-color: #4CAF50;
            --secondary-color: #FFC107;
            --light-bg: #f8f9fa;
            --dark-text: #212529;
            --border-color: #dee2e6;
        }

        .pos-container {
            display: flex;
            height: calc(100vh - 60px);
            padding: 10px;
            gap: 10px;
        }

        .product-grid {
            flex: 3;
            background-color: var(--light-bg);
            border-radius: 8px;
            padding: 15px;
            overflow-y: auto;
        }

        .cart-section {
            flex: 2;
            background-color: #ffffff;
            border-radius: 8px;
            padding: 15px;
            display: flex;
            flex-direction: column;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pos-container">
        <div class="product-grid">
            <ul class="nav nav-tabs category-tabs mb-3" id="categoryTabs">
                <li class="nav-item">
                    <a class="nav-link active" href="#" data-category="all">Tüm Ürünler</a>
                </li>
                <asp:Repeater ID="rptKategoriler" runat="server" OnItemDataBound="rptKategoriler_ItemDataBound">
                    <ItemTemplate>
                        <li class="nav-item">
                            <asp:HyperLink ID="lnkKategori" runat="server" CssClass="nav-link" data-category='<%# DataBinder.Eval(Container.DataItem, "KategoriID") %>'>
                                <%# DataBinder.Eval(Container.DataItem, "KategoriAdi") %>
                            </asp:HyperLink>
                        </li>
                    </ItemTemplate>
                </asp:Repeater>
            </ul>

            <div class="row row-cols-2 row-cols-md-3 row-cols-lg-4 g-3" id="productGrid">
                <asp:Repeater ID="rptUrunler" runat="server" OnItemDataBound="rptUrunler_ItemDataBound">
                    <ItemTemplate>
                        <div class="col">
                            <div class="product-card" data-productid='<%# DataBinder.Eval(Container.DataItem, "UrunID") %>' data-category='<%# DataBinder.Eval(Container.DataItem, "KategoriID") %>'>
                                <asp:Image ID="imgUrun" runat="server" CssClass="card-img-top" AlternateText='<%# DataBinder.Eval(Container.DataItem, "UrunAdi") %>' ImageUrl='<%# DataBinder.Eval(Container.DataItem, "ResimURL", "~/Images/Products/{0}") %>' />
                                <div class="card-body">
                                    <h5 class="card-title"><%# DataBinder.Eval(Container.DataItem, "UrunAdi") %></h5>
                                    <p class="card-text"><%# string.Format("{0:C}", DataBinder.Eval(Container.DataItem, "Fiyat")) %></p>
                                    <button class="btn btn-primary btn-sm w-100 add-to-cart">Sepete Ekle</button>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <div class="cart-section">
            <div class="cart-header">
                <h4>Sepet</h4>
            </div>
            <div class="cart-items">
                <asp:Repeater ID="rptSepet" runat="server">
                    <ItemTemplate>
                        <!-- Sepet öğeleri burada listelenecek -->
                    </ItemTemplate>
                </asp:Repeater>
            </div>
            <div class="cart-summary mt-auto">
                <div class="d-flex justify-content-between">
                    <span>Toplam:</span>
                    <span class="fw-bold" id="totalAmount">0,00 ₺</span>
                </div>
                <button id="btnOdemeYap" class="btn btn-success w-100 mt-3">Ödeme Yap</button>
            </div>
        </div>
    </div>

    <script>
        var sepet = [];

        $(document).ready(function() {
            // Sepete ürün ekleme
            $(document).on('click', '.add-to-cart', function() {
                var urunId = $(this).closest('.product-card').data('productid');
                var urunAdi = $(this).closest('.card-body').find('.card-title').text();
                var fiyat = parseFloat($(this).closest('.card-body').find('.card-text').text().replace(/[^0-9.,]/g, '').replace(',', '.'));
                
                // Ürün sepette var mı kontrol et
                var sepettekiUrun = sepet.find(u => u.id == urunId);
                
                if (sepettekiUrun) {
                    sepettekiUrun.adet++;
                } else {
                    sepet.push({
                        id: urunId,
                        ad: urunAdi,
                        fiyat: fiyat,
                        adet: 1
                    });
                }
                
                sepetiGuncelle();
            });

            // Kategori filtreleme
            $('#categoryTabs .nav-link').click(function(e) {
                e.preventDefault();
                $('#categoryTabs .nav-link').removeClass('active');
                $(this).addClass('active');
                
                var kategoriId = $(this).data('category');
                
                if (kategoriId === 'all') {
                    $('.product-card').show();
                } else {
                    $('.product-card').hide();
                    $('.product-card[data-category="' + kategoriId + '"]').show();
                }
            });

            // Ödeme butonu
            $('#btnOdemeYap').click(function() {
                if (sepet.length === 0) {
                    alert('Sepetiniz boş!');
                    return;
                }
                
                // AJAX ile ödeme işlemi
                $.ajax({
                    url: 'Satis.aspx/OdemeYap',
                    type: 'POST',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    data: JSON.stringify({ urunler: sepet }),
                    success: function(response) {
                        if (response.d.success) {
                            // Fişi yeni pencerede göster
                            var fisPenceresi = window.open('', '_blank', 'width=400,height=600');
                            fisPenceresi.document.write(response.d.fisHTML);
                            fisPenceresi.document.close();
                            
                            // Sepeti temizle
                            sepet = [];
                            sepetiGuncelle();
                            
                            alert('Ödeme başarılı! Fiş numarası: ' + response.d.message);
                        } else {
                            alert('Ödeme sırasında hata: ' + response.d.message);
                        }
                    }
                });
            });
        });

        function sepetiGuncelle() {
            var toplam = 0;
            var html = '';
            
            sepet.forEach(function(urun) {
                var urunToplam = urun.fiyat * urun.adet;
                toplam += urunToplam;
                
                html += '<div class="card mb-2">' +
                         '  <div class="card-body p-2">' +
                         '    <div class="d-flex justify-content-between">' +
                         '      <span>' + urun.ad + '</span>' +
                         '      <span>' + urun.fiyat.toFixed(2) + ' ₺ x ' + urun.adet + '</span>' +
                         '    </div>' +
                         '    <div class="d-flex justify-content-end mt-1">' +
                         '      <button class="btn btn-sm btn-outline-danger me-1 remove-item" data-id="' + urun.id + '">Sil</button>' +
                         '      <button class="btn btn-sm btn-outline-primary me-1 dec-item" data-id="' + urun.id + '">-</button>' +
                         '      <button class="btn btn-sm btn-outline-primary inc-item" data-id="' + urun.id + '">+</button>' +
                         '    </div>' +
                         '  </div>' +
                         '</div>';
            });
            
            $('.cart-items').html(html);
            $('#totalAmount').text(toplam.toFixed(2) + ' ₺');
        }
    </script>
</asp:Content>
