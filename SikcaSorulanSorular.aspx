<%@ Page Title="" Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true" CodeFile="SikcaSorulanSorular.aspx.cs" Inherits="SikcaSorulanSorular" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.15.4/css/all.min.css" />
    <style>
        .faq-section {
            background: #f8f9fa;
            padding: 60px 0;
        }
        .faq-title {
            font-size: 2.2rem;
            font-weight: bold;
            margin-bottom: 30px;
            text-align: center;
        }
        .faq-list {
            max-width: 900px;
            margin: 0 auto;
        }
        .faq-item {
            background: #fff;
            border-radius: 8px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.07);
            margin-bottom: 20px;
            padding: 24px 28px;
        }
        .faq-question {
            font-size: 1.15rem;
            font-weight: 600;
            color: #28a745;
            margin-bottom: 10px;
            display: flex;
            align-items: center;
        }
        .faq-question i {
            margin-right: 10px;
        }
        .faq-answer {
            color: #333;
            font-size: 1.05rem;
        }
    </style> 

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
    <section class="faq-section">
        <div class="container">
            <h1 class="faq-title">Zeytinyağı Fabrikası Yazılımı Hakkında Sıkça Sorulan Sorular</h1>
            <div class="faq-list">
                <div class="faq-item">
                    <div class="faq-question"><i class="fas fa-question-circle"></i> Zeytinyağı Fabrikası Yazılımı nedir ve kimler için tasarlanmıştır?</div>
                    <div class="faq-answer">
                        Bu yazılım, zeytin ve zeytinyağı sektörüne özel olarak geliştirilmiş bir sektörel muhasebe ve üretim yönetim programıdır. Zeytinyağı fabrikalarının tüm operasyonel süreçlerini (zeytin alımı, işleme, üretim, satış, finans, stok vb.) tek bir platformda yönetmelerini sağlamak amacıyla tasarlanmıştır. Özellikle müstahsillerden zeytin alımı yapan, bu zeytinleri işleyerek zeytinyağı üreten ve bu süreci detaylı bir şekilde takip etmek isteyen işletmeler için idealdir.
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question"><i class="fas fa-question-circle"></i> Yazılımın temel özellikleri ve modülleri nelerdir?</div>
                    <div class="faq-answer">
                        Yazılımın ana menü yapısı ve temel modülleri oldukça kapsamlıdır. Tanıtım ve Üyelik Modülü ile başlayan yapı, Müstahsil & Zeytin Teslimatı, Üretim & Makine Takip, CRM (Müşteri-Tedarikçi-Müstahsil), Ürün & Stok Yönetimi, Finansal Panel & Nakit Akışı, Satış & Alış İşlemleri, Yetkilendirme & Kullanıcı Rolleri ve Raporlama & Uyarılar gibi kritik işlevleri barındırır. Bu modüller, zeytinyağı fabrikalarının ihtiyaç duyduğu tüm süreçleri dijitalleştirmeyi hedefler.
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question"><i class="fas fa-question-circle"></i> Müstahsil (zeytin tedarikçisi) yönetimi nasıl işliyor?</div>
                    <div class="faq-answer">
                        Yazılım, müstahsillerden alınan zeytinlerin miktarını, cinsini, asit oranını ve kalitesini kaydetmeye olanak tanır. İşleme sonrası çıkan yağ miktarı ve fire hesaplamaları yapılarak müstahsil bazında teslim edilen yağ miktarı ve bakiye takibi yapılabilir. Ayrıca, satın alma, işleme ve fire maliyetlerine göre müstahsil bazlı maliyetlendirme hesaplamaları da sistemde yer alır.
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question"><i class="fas fa-question-circle"></i> Üretim ve makine takibi ile ilgili hangi bilgiler görülebilir?</div>
                    <div class="faq-answer">
                        Üretim modülü, pres/makine bazlı verimlilik (işlenen zeytin miktarı ve elde edilen yağ miktarı), sezonluk, günlük ve vardiyalık üretim istatistikleri sunar. Üretim reçeteleri oluşturulabilir, fermantasyon ve olgunlaşma süreçleri takip edilebilir. Tank ve depo bazlı stok takibi ve işlem günlükleri de bu modül altında yönetilir, böylece hangi tankta hangi ürünün ne aşamada olduğu kolayca görülebilir.
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question"><i class="fas fa-question-circle"></i> Finansal yönetim ve nakit akışı nasıl takip ediliyor?</div>
                    <div class="faq-answer">
                        Yazılım, işletmenin tüm finansal durumunu tek bir panelde görmeyi sağlar. Açık hesap, çek, senet, kredi ve çalışan borçları gibi borç kalemleri ile nakit, çek, senet, stok gibi varlıklar takip edilebilir. Nakit akış tablosu, gün bazlı analizler ve masraf kalemleri yönetimi ile finansal durum üzerinde tam kontrol sağlanır. Vadesi geçen alacaklar ve yaklaşan ödemeler gibi uyarılar da dashboard üzerinde yer alır.
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question"><i class="fas fa-question-circle"></i> Satış ve alış işlemleri nasıl gerçekleştiriliyor?</div>
                    <div class="faq-answer">
                        Yazılım üzerinden kayıtlı veya yeni müşterilere faturalı, proforma veya perakende satışlar girilebilir. Satış ekranında belge bilgileri, tarih, vade ve ürün seçimi gibi detaylar bulunur. Alış işlemleri de benzer şekilde tedarikçi bazında kaydedilebilir, masraf kalemleri ayrıştırılabilir ve maliyet entegrasyonu sağlanır. İade ve tahsilat/ödeme işlemleri de sistem üzerinden yönetilebilir.
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question"><i class="fas fa-question-circle"></i> Kullanıcı yetkilendirme ve rol tanımlama özellikleri nelerdir?</div>
                    <div class="faq-answer">
                        Yazılımda menü bazlı detaylı yetkilendirme sistemi bulunur. Kullanıcılara menülerin tamamı veya belirli alt menüler için görüntüleme ve işlem yapma yetkisi verilebilir. Ayrıca, kullanıcılara hangi depo, hesap, müşteri veya satışları görüp göremeyeceği gibi özel kısıtlamalar da tanımlanabilir. Şifre yönetimi ve kullanıcı işlem logları da güvenlik için mevcuttur.
                    </div>
                </div>
                <div class="faq-item">
                    <div class="faq-question"><i class="fas fa-question-circle"></i> Yazılım hangi raporlama ve analiz imkanlarını sunar?</div>
                    <div class="faq-answer">
                        Yazılım, satış, alış, kasa, banka, çek, stok, üretim, müstahsil ve makine verimliliği gibi birçok alanda kapsamlı raporlar sunar. Bu raporlar sayesinde işletmeler performanslarını analiz edebilir, karar alma süreçlerini destekleyebilir. Dashboard üzerinde yer alan günlük ve aylık özet tabloları (satış, tahsilat, ciro, masraf, stok değeri vb.) anlık durum takibi için önemli bilgiler sağlar.
                    </div>
                </div>
            </div>
        </div>
    </section>

</asp:Content>

