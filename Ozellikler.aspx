<%@ Page Title="" Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true" CodeFile="Ozellikler.aspx.cs" Inherits="Ozellikler" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <style>
        .hero-section {
            background: url('/img/zeytinlik.jpg') center/cover no-repeat;
            color: #fff;
            min-height: 300px;
            display: flex;
            align-items: center;
            justify-content: center;
            text-align: center;
        }
        .hero-section h1 {
            font-size: 2.5rem;
            font-weight: bold;
            text-shadow: 2px 2px 8px #333;
        }
        .hero-section p {
            font-size: 1.2rem;
            text-shadow: 1px 1px 6px #222;
        }
        .feature-card {
            background: #fff;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.07);
            padding: 30px 20px;
            margin-bottom: 30px;
            text-align: center;
            height: 100%;
        }
        .feature-card i {
            font-size: 2.5rem;
            color: #28a745;
            margin-bottom: 15px;
        }
        .feature-card h5 {
            font-weight: bold;
            margin-bottom: 10px;
        }
        .feature-list {
            list-style: none;
            padding-left: 0;
        }
        .feature-list li {
            margin-bottom: 10px;
            padding-left: 24px;
            position: relative;
        }
        .feature-list li:before {
            content: "\f058";
            font-family: "Font Awesome 5 Free";
            font-weight: 900;
            position: absolute;
            left: 0;
            color: #28a745;
        }
        .section-title {
            font-size: 2rem;
            font-weight: bold;
            margin-bottom: 30px;
            text-align: center;
        }
        .bg-light {
            background: #f8f9fa;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Hero Section -->
    <section class="hero-section py-5">
        <div class="container">
            <h1>Zeytinyağı Fabrikası Özel Muhasebe ve Üretim Yazılımı</h1>
            <p class="mt-3">Zeytin ve zeytinyağı sektörünün tüm süreçlerine özel, entegre ve yenilikçi dijital çözüm</p>
        </div>
    </section>

    <!-- Ana Temalar ve Faydalar -->
    <section class="container py-5">
        <div class="row mb-4">
            <div class="col-md-12">
                <h2 class="section-title">Sektöre Özel Dijitalleşme ve Entegre Yönetim</h2>
                <ul class="feature-list">
                    <li><b>Sektörel Odak:</b> Zeytinyağı fabrikalarının benzersiz üretim ve operasyonel süreçlerine özel modüller.</li>
                    <li><b>Uçtan Uca Süreç Yönetimi:</b> Müstahsilden zeytin alımından, üretim, stok, finans ve raporlamaya kadar tüm adımlar tek platformda.</li>
                    <li><b>Verimlilik ve Maliyet Takibi:</b> Üretim verimliliği, makine performansı, maliyet ve kalite analizleri.</li>
                    <li><b>Detaylı Stok ve Üretim Takibi:</b> Tank/depo bazlı stok, fermantasyon, olgunlaşma ve ürün varyant yönetimi.</li>
                    <li><b>Kapsamlı CRM ve Finansal Yönetim:</b> Müşteri, tedarikçi, müstahsil, nakit akışı ve borç/varlık yönetimi.</li>
                    <li><b>Yetkilendirme ve Raporlama:</b> Menü ve işlem bazlı yetki, detaylı raporlar ve anlık finansal göstergeler.</li>
                </ul>
            </div>
        </div>
    </section>

    <!-- Modül Kartları -->
    <section class="container py-3">
        <h2 class="section-title">Başlıca Modüller</h2>
        <div class="row">
            <div class="col-md-4">
                <div class="feature-card">
                    <i class="fas fa-user-plus"></i>
                    <h5>Tanıtım & Üyelik</h5>
                    <p>Landing page, ücretsiz deneme, şirket ve kullanıcı kaydı, e-posta aktivasyonu.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <i class="fas fa-tractor"></i>
                    <h5>Müstahsil & Zeytin Teslimatı</h5>
                    <p>Müstahsil kaydı, zeytin alım (miktar, cins, kalite, asit), işleme sonrası yağ ve fire hesaplamaları, bakiye takibi.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <i class="fas fa-cogs"></i>
                    <h5>Üretim & Makine Takip</h5>
                    <p>Makine bazlı verimlilik, üretim istatistikleri, reçeteler, fermantasyon ve tank/depo stok takibi.</p>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="feature-card">
                    <i class="fas fa-users"></i>
                    <h5>CRM</h5>
                    <p>Müşteri, tedarikçi, müstahsil yönetimi, finansal bilgiler, satış/alış geçmişi, teklif ve panel yönetimi.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <i class="fas fa-boxes"></i>
                    <h5>Ürün & Stok Yönetimi</h5>
                    <p>Detaylı ürün kartları, çoklu depo, stok hareketleri, transferler, katalog ve varyantlar.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <i class="fas fa-chart-line"></i>
                    <h5>Finansal Panel & Nakit Akışı</h5>
                    <p>Borç/alacak, nakit akışı, varlık/borç özeti, masraf yönetimi ve finansal analizler.</p>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="feature-card">
                    <i class="fas fa-shopping-cart"></i>
                    <h5>Satış & Alış İşlemleri</h5>
                    <p>Faturalı, proforma, perakende satış, alış kayıtları, masraf ayrıştırma, iade ve tahsilat/ödeme işlemleri.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <i class="fas fa-user-shield"></i>
                    <h5>Yetkilendirme & Kullanıcı Rolleri</h5>
                    <p>Menü ve işlem bazlı yetki, kullanıcıya özel veri erişimi, şifre yönetimi.</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="feature-card">
                    <i class="fas fa-chart-pie"></i>
                    <h5>Raporlama & Uyarılar</h5>
                    <p>Kapsamlı raporlar, finansal ve operasyonel uyarılar, dashboard ve analizler.</p>
                </div>
            </div>
        </div>
    </section>

    <!-- Sektöre Özel Farklar ve Avantajlar -->
    <section class="container py-5 bg-light">
        <h2 class="section-title">Sektöre Özel Fark Yaratan Özellikler</h2>
        <div class="row">
            <div class="col-md-6 mb-4">
                <div class="feature-card">
                    <i class="fas fa-clipboard-list"></i>
                    <h5>Zeytin Kabul ve İşleme Takibi</h5>
                    <p>Müstahsilden alınan zeytinin miktarı, kalitesi, işleme sonrası yağ miktarı ve müstahsile teslim edilen yağın detaylı takibi.</p>
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="feature-card">
                    <i class="fas fa-tachometer-alt"></i>
                    <h5>Makine Bazlı Verimlilik</h5>
                    <p>Her makinenin sezonluk/günlük işlediği zeytin, elde edilen yağ miktarı ve verimlilik oranlarının izlenmesi.</p>
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="feature-card">
                    <i class="fas fa-warehouse"></i>
                    <h5>Tank/Depo ve Fermantasyon Takibi</h5>
                    <p>Hangi tankta hangi ürünün olduğu, fermantasyon ve olgunlaşma süreçlerinin kolayca izlenmesi.</p>
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="feature-card">
                    <i class="fas fa-coins"></i>
                    <h5>Kapsamlı Finansal Gösterge Paneli</h5>
                    <p>Ana sayfada nakit ciro, satışlar, masraflar, stok değeri, varlıklar ve borçlar gibi temel finansal verilerin anlık görüntülenmesi.</p>
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="feature-card">
                    <i class="fas fa-user-lock"></i>
                    <h5>Menü ve İşlem Bazlı Yetkilendirme</h5>
                    <p>Kullanıcıların menü ve işlem düzeyinde yetkilendirilmesi, veri erişim sınırlandırmaları ve güvenlik.</p>
                </div>
            </div>
            <div class="col-md-6 mb-4">
                <div class="feature-card">
                    <i class="fas fa-file-invoice-dollar"></i>
                    <h5>Hesap Ekstreleri ve Şeffaflık</h5>
                    <p>Müstahsil, müşteri ve tedarikçi için detaylı hesap hareketleri ve ekstreler, tam şeffaflık ve muhasebe kolaylığı.</p>
                </div>
            </div>
        </div>
    </section>

    <!-- CTA Section -->
    <section class="py-5 bg-primary text-white text-center">
        <div class="container">
            <h2 class="mb-4">Zeytinyağı Fabrikası Dijital Dönüşümünü Şimdi Başlatın</h2>
            <p class="lead mb-4">30 gün ücretsiz deneme ile fabrikanızın tüm süreçlerini dijitalleştirin</p>
            <a href="/Kayit.aspx" class="btn btn-light btn-lg">Ücretsiz Demo Talep Edin</a>
        </div>
    </section>
</asp:Content>

