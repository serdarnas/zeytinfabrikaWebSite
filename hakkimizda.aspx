<%@ Page Title="" Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true" CodeFile="hakkimizda.aspx.cs" Inherits="hakkimizda" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <title>Hakkımızda - Zeytin Fabrika Yönetim Sistemi</title>
    <meta name="description" content="Olive Edremit Zeytincilik Sanayi Ticaret Limited Şirketi hakkında bilgi, misyon ve vizyon." />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <style>
        .hero-about {
            background: url('/img/zeytinlik.jpg') center/cover no-repeat;
            color: #fff;
            min-height: 300px;
            display: flex;
            align-items: center;
            justify-content: center;
            text-align: center;
        }
        .hero-about h1 {
            font-size: 2.8rem;
            font-weight: bold;
            text-shadow: 2px 2px 8px #333;
        }
        .timeline {
            position: relative;
            margin: 2rem 0 3rem 0;
        }
        .timeline:before {
            content: '';
            position: absolute;
            left: 50%;
            width: 2px;
            background: #28a745;
            top: 0;
            bottom: 0;
            transform: translateX(-50%);
        }
        .timeline-item {
            position: relative;
            width: 50%;
            padding: 1rem 2rem;
            box-sizing: border-box;
        }
        .timeline-item.left {
            left: 0;
            text-align: right;
        }
        .timeline-item.right {
            left: 50%;
            text-align: left;
        }
        .timeline-item .timeline-dot {
            position: absolute;
            top: 1.2rem;
            right: -1.1rem;
            width: 22px;
            height: 22px;
            background: #28a745;
            border-radius: 50%;
            z-index: 1;
        }
        .timeline-item.right .timeline-dot {
            left: -1.1rem;
            right: auto;
        }
        @media (max-width: 767px) {
            .timeline:before { left: 10px; }
            .timeline-item, .timeline-item.left, .timeline-item.right {
                width: 100%;
                left: 0;
                text-align: left;
                padding-left: 2.5rem;
            }
            .timeline-item .timeline-dot, .timeline-item.right .timeline-dot {
                left: -1.1rem;
                right: auto;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Hero Section -->
    <section class="hero-about py-5">
        <div class="container">
            <h1>Hakkımızda</h1>
            <p class="lead mt-3">Türkiye'nin zeytin ve zeytinyağı sektöründe dijitalleşme ve inovasyon için çalışan bir yazılım firmasıyız.</p>
        </div>
    </section>

    <!-- Hakkımızda Yazılım Firması Tanıtım Alanı -->
    <section class="container py-4">
        <div class="row align-items-center">
            <div class="col-md-8">
                <h2 class="mb-3"><i class="fas fa-laptop-code text-success"></i> Hakkımda Yazılım Firması</h2>
                <p class="lead">
                    Türkiye'nin zeytin ve zeytinyağı sektörünün dijitalleşmesi ve gelişmesi için çalışan, sektöre özel yazılım çözümleri geliştiren bir teknoloji firmasıyız. 
                    Amacımız; fabrikaların ve işletmelerin karşılaştığı operasyonel ve finansal zorluklara yenilikçi, pratik ve ekonomik dijital çözümler sunmak.
                </p>
                <ul>
                    <li><b>Ekonomik Yönetim:</b> Fabrikanızın tüm maliyetlerini ve finansal durumunu anlık olarak izleyin, kârlılığınızı artırın.</li>
                    <li><b>Şeffaf ve Kolay Süreç Yönetimi:</b> Tüm üretim, stok, satış ve tedarik süreçlerini tek ekrandan, kolayca yönetin.</li>
                    <li><b>Sektöre Özel Dijital Çözümler:</b> Zeytin ve zeytinyağı sektörünün kendine has tüm ihtiyaçlarına cevap veren yazılım modülleri.</li>
                    <li><b>Veri Odaklı Karar Desteği:</b> Anlık raporlar ve analizlerle fabrikanızın geleceğini güvenle planlayın.</li>
                </ul>
                <p>
                    <b>Hakkımda Yazılım</b> olarak, sektördeki tecrübemiz ve teknolojik altyapımızla, zeytin ve zeytinyağı sektörünün dijital dönüşümüne liderlik ediyoruz.
                </p>
            </div>
            <div class="col-md-4 text-center">
                <img src="/img/yazilim_firma_logo.png" alt="Yazılım Firması Logo" class="img-fluid mb-3" style="max-height:120px;">
                <div>
                    <i class="fas fa-chart-pie fa-2x text-success mx-2"></i>
                    <i class="fas fa-cogs fa-2x text-success mx-2"></i>
                    <i class="fas fa-balance-scale fa-2x text-success mx-2"></i>
                </div>
            </div>
        </div>
    </section>

    <div class="container py-5">
        <!-- Şirket Tanıtım Kartı -->
        <div class="row mb-4">
            <div class="col-md-6 mb-3">
                <div class="card shadow-sm h-100">
                    <div class="card-body">
                        <h2 class="h4 mb-3"><i class="fas fa-industry text-success"></i> Hakkımda Yazılım</h2>
                        <p>
                            <strong>Adres:</strong> Zeytinli Mah. Şehit Hamdi Bey Cad. No:33 Edremit/Balıkesir<br />
                            <strong>Telefon:</strong> +90 546 203 98 45<br />
                            <strong>E-posta:</strong> info@zeytinfabrika.com.tr
                        </p>
                        <div>
                            <a href="https://wa.me/905462039845" target="_blank" class="btn btn-success btn-sm mr-2"><i class="fab fa-whatsapp"></i> WhatsApp</a>
                            <a href="mailto:info@zeytinfabrika.com.tr" class="btn btn-primary btn-sm"><i class="fas fa-envelope"></i> E-posta</a>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-6 mb-3 d-flex align-items-center">
                <img src="/img/fabrika_arka.png" alt="Yazılım Ofisi" class="img-fluid rounded shadow-sm w-100" />
            </div>
        </div>

        <!-- Zaman Çizelgesi -->
        <div class="timeline">
            <div class="timeline-item left">
                <div class="timeline-dot"></div>
                <h5>2010</h5>
                <p>Yazılım sektöründe ilk adımlarımızı attık ve sektörel ihtiyaçları analiz etmeye başladık.</p>
            </div>
            <div class="timeline-item right">
                <div class="timeline-dot"></div>
                <h5>2015</h5>
                <p>Zeytin ve zeytinyağı sektörüne özel dijital çözümler geliştirmeye başladık.</p>
            </div>
            <div class="timeline-item left">
                <div class="timeline-dot"></div>
                <h5>2018</h5>
                <p>Türkiye genelinde birçok fabrikanın dijitalleşme süreçlerine katkı sağladık.</p>
            </div>
            <div class="timeline-item right">
                <div class="timeline-dot"></div>
                <h5>2023</h5>
                <p>Sektörün dijital dönüşümünde öncü yazılım firmalarından biri olduk.</p>
            </div>
        </div>

        <!-- Misyon & Vizyon -->
        <div class="row mb-4">
            <div class="col-md-6 mb-3">
                <div class="card shadow-sm h-100">
                    <div class="card-body text-center">
                        <i class="fas fa-bullseye fa-2x text-success mb-2"></i>
                        <h5 class="h5">Misyonumuz</h5>
                        <p>Zeytin ve zeytinyağı sektöründe dijitalleşmeyi hızlandırmak, işletmelere yenilikçi ve güvenilir yazılım çözümleri sunmak.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-6 mb-3">
                <div class="card shadow-sm h-100">
                    <div class="card-body text-center">
                        <i class="fas fa-lightbulb fa-2x text-warning mb-2"></i>
                        <h5 class="h5">Vizyonumuz</h5>
                        <p>Türkiye'nin zeytin ve zeytinyağı sektöründe dijital dönüşümün lideri olmak, sektöre değer katan teknolojiler geliştirmek.</p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Değerlerimiz -->
        <div class="row mb-4">
            <div class="col-md-4 mb-3">
                <div class="card shadow-sm h-100 text-center">
                    <div class="card-body">
                        <i class="fas fa-leaf fa-2x text-success mb-2"></i>
                        <h6>Yenilikçilik</h6>
                        <p>Sektöre özel yeni teknolojiler ve dijital çözümler geliştiriyoruz.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-3">
                <div class="card shadow-sm h-100 text-center">
                    <div class="card-body">
                        <i class="fas fa-handshake fa-2x text-primary mb-2"></i>
                        <h6>Güven</h6>
                        <p>Müşterilerimizle güvene dayalı, uzun vadeli iş birlikleri kuruyoruz.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4 mb-3">
                <div class="card shadow-sm h-100 text-center">
                    <div class="card-body">
                        <i class="fas fa-tree fa-2x text-success mb-2"></i>
                        <h6>Sürdürülebilirlik</h6>
                        <p>Sektörün sürdürülebilir büyümesine dijital altyapı ile katkı sağlıyoruz.</p>
                    </div>
                </div>
            </div>
        </div>

        <!-- Tesisler & Kalite Politikası -->
        <div class="row mb-4">
            <div class="col-md-6 mb-3">
                <div class="card shadow-sm h-100">
                    <div class="card-body">
                        <h5 class="h5"><i class="fas fa-warehouse text-success"></i> Dijital Altyapımız</h5>
                        <ul>
                            <li>Bulut tabanlı yazılım çözümleri</li>
                            <li>Mobil ve web tabanlı yönetim panelleri</li>
                            <li>Entegre raporlama ve analiz araçları</li>
                            <li>Güvenli veri yönetimi</li>
                        </ul>
                    </div>
                </div>
            </div>
            <div class="col-md-6 mb-3">
                <div class="card shadow-sm h-100">
                    <div class="card-body">
                        <h5 class="h5"><i class="fas fa-certificate text-warning"></i> Kalite Politikamız</h5>
                        <ul>
                            <li>Yüksek yazılım standartları</li>
                            <li>Sürekli güncellenen teknolojik altyapı</li>
                            <li>Müşteri odaklı geliştirme süreçleri</li>
                            <li>Veri güvenliği ve gizliliği</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

        <!-- İletişim Alanı -->
        <div class="card shadow-sm mb-4">
            <div class="card-body">
                <h5 class="h5"><i class="fas fa-phone-alt text-success"></i> İletişim</h5>
                <p>
                    <strong>E-posta:</strong> info@zeytinfabrika.com.tr<br />
                    <strong>Telefon:</strong> +90 546 203 98 45<br />
                    <strong>Adres:</strong> Zeytinli Mah. Şehit Hamdi Bey Cad. No:33 Edremit/Balıkesir
                </p>
                <a href="mailto:info@zeytinfabrika.com.tr" class="btn btn-primary btn-sm mr-2"><i class="fas fa-envelope"></i> E-posta Gönder</a>
                <a href="https://wa.me/905462039845" target="_blank" class="btn btn-success btn-sm"><i class="fab fa-whatsapp"></i> WhatsApp</a>
            </div>
        </div>
    </div>
</asp:Content>


