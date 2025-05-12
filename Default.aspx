<%@ Page Title="" Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <style>
        .hero-section {
            position: relative;
            min-height: 60vh;
            background: url('/img/zeytinlik.jpg') center/cover no-repeat;
            color: #fff;
            display: flex;
            align-items: center;
            justify-content: center;
            text-align: center;
        }
        .hero-section h1 {
            font-size: 3rem;
            font-weight: bold;
            text-shadow:2px 2px 8px #333;
        }
        .hero-section p {
            font-size: 1.3rem;
            text-shadow:1px 1px 6px #222;
        }
        .demo-btn {
            background-color: #28a745;
            border: none;
            padding: 16px 40px;
            font-size: 1.2rem;
            border-radius: 30px;
            color: #fff;
            font-weight: bold;
            transition: all 0.3s ease;
        }
        .demo-btn:hover {
            background-color: #218838;
            transform: translateY(-2px);
        }
        .card i {
            margin-bottom: 12px;
        }
        .testimonial-card {
            background: #fff;
            border-radius: 10px;
            padding: 30px;
            margin: 20px 0;
            box-shadow: 0 2px 8px rgba(0,0,0,0.07);
        }
        .client-logo {
            height: 60px;
            margin-bottom: 20px;
        }
        .bg-light {
            background-color: #f8f9fa !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Hero Section -->
    <section class="hero-section py-5">
        <div class="container">
            <h1 class="mb-3">Zeytin Fabrika Yönetim Sistemi</h1>
            <p class="mb-4">Zeytin ve zeytinyağı sektörüne özel, uçtan uca dijitalleşme!</p>
            <a href="Kayit.aspx" class="demo-btn">Ücretsiz 30 Gün Dene</a>
        </div>
    </section>

    <!-- Modül Kartları -->
    <section class="container py-5">
        <h2 class="text-center mb-5">Başlıca Modüller</h2>
        <div class="row">
            <div class="col-md-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body text-center">
                        <i class="fa fa-user-plus fa-2x mb-3 text-success"></i>
                        <h5 class="card-title">Tanıtım & Üyelik</h5>
                        <p class="card-text">Kolay kayıt, e-posta aktivasyonu, firma yönetim paneli ve ücretsiz deneme.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body text-center">
                        <i class="fa fa-truck fa-2x mb-3 text-success"></i>
                        <h5 class="card-title">Müstahsil & Teslimat</h5>
                        <p class="card-text">Zeytin alım, kalite, fire, yağ çıkışı ve bakiye takibi.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body text-center">
                        <i class="fa fa-cogs fa-2x mb-3 text-success"></i>
                        <h5 class="card-title">Üretim & Makine Takip</h5>
                        <p class="card-text">Makine verimliliği, üretim reçeteleri, tank/depo stokları.</p>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body text-center">
                        <i class="fa fa-users fa-2x mb-3 text-success"></i>
                        <h5 class="card-title">CRM</h5>
                        <p class="card-text">Müşteri, tedarikçi, müstahsil yönetimi, ciro ve bakiye takibi.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body text-center">
                        <i class="fa fa-archive fa-2x mb-3 text-success"></i>
                        <h5 class="card-title">Stok & Ürün Yönetimi</h5>
                        <p class="card-text">Barkod, çoklu depo, stok hareketleri, özel fiyat listeleri.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-4">
                <div class="card h-100 shadow-sm">
                    <div class="card-body text-center">
                        <i class="fa fa-chart-line fa-2x mb-3 text-success"></i>
                        <h5 class="card-title">Finans & Raporlama</h5>
                        <p class="card-text">Nakit akışı, borç/varlık yönetimi, detaylı raporlar ve dashboard.</p>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- Sektörel Avantajlar -->
    <section class="bg-light py-5">
        <div class="container">
            <h2 class="text-center mb-4">Neden Bu Yazılım?</h2>
            <div class="row text-center">
                <div class="col-md-3 mb-3">
                    <i class="fa fa-bolt fa-2x text-success mb-2"></i>
                    <h5>Verimlilik</h5>
                    <p>Üretimden satışa tüm süreçlerde %40'a varan zaman ve maliyet tasarrufu.</p>
                </div>
                <div class="col-md-3 mb-3">
                    <i class="fa fa-eye fa-2x text-success mb-2"></i>
                    <h5>Şeffaflık</h5>
                    <p>Tüm işlemler kayıt altında, izlenebilir ve raporlanabilir.</p>
                </div>
                <div class="col-md-3 mb-3">
                    <i class="fa fa-mobile-alt fa-2x text-success mb-2"></i>
                    <h5>Mobil Erişim</h5>
                    <p>Fabrikanızı her yerden yönetin, anlık bildirimler alın.</p>
                </div>
                <div class="col-md-3 mb-3">
                    <i class="fa fa-lock fa-2x text-success mb-2"></i>
                    <h5>Güvenlik</h5>
                    <p>Kullanıcı yetkilendirme ve veri güvenliği üst düzeyde.</p>
                </div>
            </div>
        </div>
    </section>

    <!-- Müşteri Yorumları -->
    <%--<section class="container py-5">
        <h2 class="text-center mb-5">Müşterilerimiz Ne Diyor?</h2>
        <div class="row">
            <div class="col-md-4">
                <div class="testimonial-card p-4 shadow-sm bg-white rounded">
                    <img src="/img/fabrika_eski.png" alt="Client Logo" class="client-logo mb-3">
                    <p class="mb-4">"Sistemi kullanmaya başladığımızdan beri üretim süreçlerimiz çok daha verimli ve şeffaf."</p>
                    <h5>Ahmet Yılmaz</h5>
                    <p class="text-muted">ABC Zeytin Fabrikası</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="testimonial-card p-4 shadow-sm bg-white rounded">
                    <img src="/img/zeytinfabrika.png" alt="Client Logo" class="client-logo mb-3">
                    <p class="mb-4">"Stok ve finans yönetimi artık elimizin altında, raporlar harika!"</p>
                    <h5>Ayşe Demir</h5>
                    <p class="text-muted">XYZ Zeytin Fabrikası</p>
                </div>
            </div>
            <div class="col-md-4">
                <div class="testimonial-card p-4 shadow-sm bg-white rounded">
                    <img src="/img/fabrika_arka.png" alt="Client Logo" class="client-logo mb-3">
                    <p class="mb-4">"Mobil uygulama ile her yerden fabrikamı yönetebiliyorum."</p>
                    <h5>Mehmet Kaya</h5>
                    <p class="text-muted">123 Zeytin Fabrikası</p>
                </div>
            </div>
        </div>
    </section>--%>

    <!-- Demo/Üyelik Çağrısı -->
    <section class="py-5" style="background: #e9ffe9;">
        <div class="container text-center">
            <h2 class="mb-3">Sektörde Dijital Dönüşümün Parçası Olun!</h2>
            <p class="lead mb-4">Hemen ücretsiz demo hesabınızı oluşturun, avantajları kendiniz keşfedin.</p>
            <a href="Kayit.aspx" class="btn btn-lg btn-success px-5 py-3 font-weight-bold">Ücretsiz Üyelik</a>
        </div>
    </section>
</asp:Content>
