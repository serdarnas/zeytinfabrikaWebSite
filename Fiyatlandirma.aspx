<%@ Page Title="" Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true" CodeFile="Fiyatlandirma.aspx.cs" Inherits="Fiyatlandirma" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .pricing-section {
            padding: 80px 0;
        }

        .pricing-card {
            background: white;
            border-radius: 15px;
            padding: 40px;
            margin-bottom: 30px;
            box-shadow: 0 10px 30px rgba(0,0,0,0.1);
            transition: all 0.3s ease;
            height: 100%;
            position: relative;
        }

        .pricing-card:hover {
            transform: translateY(-10px);
            box-shadow: 0 20px 40px rgba(0,0,0,0.2);
        }

        .pricing-card.popular {
            border: 2px solid var(--primary-color);
        }

        .pricing-header {
            text-align: center;
            margin-bottom: 30px;
        }

        .pricing-title {
            font-size: 1.8rem;
            font-weight: 700;
            margin-bottom: 15px;
        }

        .pricing-price {
            font-size: 3rem;
            font-weight: 700;
            color: var(--primary-color);
            margin-bottom: 10px;
        }

        .pricing-period {
            color: var(--secondary-color);
            font-size: 1.1rem;
        }

        .pricing-features {
            margin: 30px 0;
        }

        .pricing-feature {
            margin-bottom: 15px;
            padding-left: 30px;
            position: relative;
        }

        .pricing-feature:before {
            content: "\f00c";
            font-family: "Font Awesome 5 Free";
            font-weight: 900;
            position: absolute;
            left: 0;
            color: var(--primary-color);
        }

        .pricing-feature.disabled {
            opacity: 0.5;
        }

        .pricing-feature.disabled:before {
            content: "\f00d";
            color: var(--danger-color);
        }

        .pricing-ribbon {
            position: absolute;
            top: 0;
            right: 0;
            background: var(--primary-color);
            color: white;
            padding: 5px 15px;
            border-radius: 0 15px 0 15px;
            font-weight: 600;
        }

        .pricing-comparison {
            margin-top: 50px;
        }

        .comparison-table {
            width: 100%;
            border-collapse: collapse;
        }

        .comparison-table th,
        .comparison-table td {
            padding: 15px;
            text-align: center;
            border: 1px solid #eee;
        }

        .comparison-table th {
            background: var(--light-color);
            font-weight: 600;
        }

        .comparison-feature {
            text-align: left;
            font-weight: 600;
        }

        .comparison-check {
            color: var(--success-color);
        }

        .comparison-cross {
            color: var(--danger-color);
        }

        .faq-section {
            background: var(--light-color);
            padding: 80px 0;
        }

        .faq-item {
            margin-bottom: 30px;
        }

        .faq-question {
            font-weight: 600;
            margin-bottom: 10px;
        }

        .faq-answer {
            color: var(--secondary-color);
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <!-- Hero Section -->
    <section class="hero-section">
        <div class="container">
            <div class="row">
                <div class="col-lg-8 hero-content">
                    <h1 class="hero-title">Fiyatlandırma</h1>
                    <p class="lead mb-4">İşletmenizin ihtiyaçlarına uygun paketi seçin</p>
                </div>
            </div>
        </div>
    </section>

    <!-- Pricing Section -->
    <section class="pricing-section">
        <div class="container">
            <div class="row">
                <!-- Başlangıç Paketi -->
                <div class="col-md-4">
                    <div class="pricing-card">
                        <div class="pricing-header">
                            <h3 class="pricing-title">Başlangıç</h3>
                            <div class="pricing-price">₺999</div>
                            <div class="pricing-period">aylık</div>
                        </div>
                        <div class="pricing-features">
                            <div class="pricing-feature">1 kullanıcı</div>
                            <div class="pricing-feature">Temel üretim takibi</div>
                            <div class="pricing-feature">Stok yönetimi</div>
                            <div class="pricing-feature">Temel raporlama</div>
                            <div class="pricing-feature">E-posta desteği</div>
                            <div class="pricing-feature disabled">Gelişmiş kalite kontrol</div>
                            <div class="pricing-feature disabled">Hasat yönetimi</div>
                            <div class="pricing-feature disabled">API erişimi</div>
                        </div>
                        <div class="text-center">
                            <a href="/Kayit.aspx" class="btn btn-outline-primary btn-lg">Başla</a>
                        </div>
                    </div>
                </div>

                <!-- Profesyonel Paket -->
                <div class="col-md-4">
                    <div class="pricing-card popular">
                        <div class="pricing-ribbon">Popüler</div>
                        <div class="pricing-header">
                            <h3 class="pricing-title">Profesyonel</h3>
                            <div class="pricing-price">₺1999</div>
                            <div class="pricing-period">aylık</div>
                        </div>
                        <div class="pricing-features">
                            <div class="pricing-feature">5 kullanıcı</div>
                            <div class="pricing-feature">Tüm Başlangıç özellikleri</div>
                            <div class="pricing-feature">Gelişmiş kalite kontrol</div>
                            <div class="pricing-feature">Hasat yönetimi</div>
                            <div class="pricing-feature">Öncelikli destek</div>
                            <div class="pricing-feature">Gelişmiş raporlama</div>
                            <div class="pricing-feature">Mobil uygulama</div>
                            <div class="pricing-feature disabled">API erişimi</div>
                        </div>
                        <div class="text-center">
                            <a href="/Kayit.aspx" class="btn btn-primary btn-lg">Başla</a>
                        </div>
                    </div>
                </div>

                <!-- Kurumsal Paket -->
                <div class="col-md-4">
                    <div class="pricing-card">
                        <div class="pricing-header">
                            <h3 class="pricing-title">Kurumsal</h3>
                            <div class="pricing-price">₺3999</div>
                            <div class="pricing-period">aylık</div>
                        </div>
                        <div class="pricing-features">
                            <div class="pricing-feature">Sınırsız kullanıcı</div>
                            <div class="pricing-feature">Tüm Profesyonel özellikleri</div>
                            <div class="pricing-feature">API erişimi</div>
                            <div class="pricing-feature">Özel entegrasyonlar</div>
                            <div class="pricing-feature">7/24 destek</div>
                            <div class="pricing-feature">Özel eğitim</div>
                            <div class="pricing-feature">Özel raporlama</div>
                            <div class="pricing-feature">Dedicated sunucu</div>
                        </div>
                        <div class="text-center">
                            <a href="/Kayit.aspx" class="btn btn-outline-primary btn-lg">Başla</a>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Özellik Karşılaştırma Tablosu -->
            <div class="pricing-comparison">
                <h2 class="text-center mb-5">Özellik Karşılaştırması</h2>
                <div class="table-responsive">
                    <table class="comparison-table">
                        <thead>
                            <tr>
                                <th>Özellik</th>
                                <th>Başlangıç</th>
                                <th>Profesyonel</th>
                                <th>Kurumsal</th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td class="comparison-feature">Kullanıcı Sayısı</td>
                                <td>1</td>
                                <td>5</td>
                                <td>Sınırsız</td>
                            </tr>
                            <tr>
                                <td class="comparison-feature">Üretim Takibi</td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                            </tr>
                            <tr>
                                <td class="comparison-feature">Stok Yönetimi</td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                            </tr>
                            <tr>
                                <td class="comparison-feature">Kalite Kontrol</td>
                                <td><i class="fas fa-times comparison-cross"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                            </tr>
                            <tr>
                                <td class="comparison-feature">Hasat Yönetimi</td>
                                <td><i class="fas fa-times comparison-cross"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                            </tr>
                            <tr>
                                <td class="comparison-feature">Mobil Uygulama</td>
                                <td><i class="fas fa-times comparison-cross"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                            </tr>
                            <tr>
                                <td class="comparison-feature">API Erişimi</td>
                                <td><i class="fas fa-times comparison-cross"></i></td>
                                <td><i class="fas fa-times comparison-cross"></i></td>
                                <td><i class="fas fa-check comparison-check"></i></td>
                            </tr>
                            <tr>
                                <td class="comparison-feature">Destek</td>
                                <td>E-posta</td>
                                <td>Öncelikli</td>
                                <td>7/24</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </div>
    </section>

    <!-- FAQ Section -->
    <section class="faq-section">
        <div class="container">
            <h2 class="text-center mb-5">Sıkça Sorulan Sorular</h2>
            <div class="row">
                <div class="col-md-6">
                    <div class="faq-item">
                        <div class="faq-question">Fiyatlar vergi dahil mi?</div>
                        <div class="faq-answer">Evet, tüm fiyatlarımız %20 KDV dahil olarak belirtilmiştir.</div>
                    </div>
                    <div class="faq-item">
                        <div class="faq-question">Ödeme yöntemleri nelerdir?</div>
                        <div class="faq-answer">Kredi kartı, banka havalesi ve EFT ile ödeme yapabilirsiniz.</div>
                    </div>
                    <div class="faq-item">
                        <div class="faq-question">Paket değişikliği yapabilir miyim?</div>
                        <div class="faq-answer">Evet, istediğiniz zaman paketinizi yükseltebilir veya düşürebilirsiniz.</div>
                    </div>
                </div>
                <div class="col-md-6">
                    <div class="faq-item">
                        <div class="faq-question">İptal politikası nedir?</div>
                        <div class="faq-answer">Aboneliğinizi istediğiniz zaman iptal edebilirsiniz. İptal işlemi sonraki fatura döneminde geçerli olur.</div>
                    </div>
                    <div class="faq-item">
                        <div class="faq-question">Ücretsiz deneme süresi var mı?</div>
                        <div class="faq-answer">Evet, tüm paketler için 30 gün ücretsiz deneme süresi sunuyoruz.</div>
                    </div>
                    <div class="faq-item">
                        <div class="faq-question">Veri aktarımı yapılıyor mu?</div>
                        <div class="faq-answer">Evet, mevcut verilerinizi sistemimize ücretsiz olarak aktarıyoruz.</div>
                    </div>
                </div>
            </div>
        </div>
    </section>

    <!-- CTA Section -->
    <section class="py-5 bg-primary text-white text-center">
        <div class="container">
            <h2 class="mb-4">Zeytin Fabrika Yönetim Sistemi'ni Keşfedin</h2>
            <p class="lead mb-4">30 gün ücretsiz deneme sürümümüzü hemen test edin</p>
            <a href="/Kayit.aspx" class="btn btn-light btn-lg">Ücretsiz Demo Talep Edin</a>
        </div>
    </section>
</asp:Content> 

