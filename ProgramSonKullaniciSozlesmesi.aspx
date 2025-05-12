<%@ Page Title="" Language="C#" MasterPageFile="~/WebMasterPage.master" AutoEventWireup="true" CodeFile="ProgramSonKullaniciSozlesmesi.aspx.cs" Inherits="ProgramSonKullaniciSozlesmesi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style>
        .agreement-section {
            margin-bottom: 30px;
            padding: 20px;
        }
        .agreement-title {
            font-size: 24px;
            font-weight: bold;
            margin-bottom: 20px;
            color: #2c3e50;
        }
        .agreement-subtitle {
            font-size: 18px;
            font-weight: bold;
            margin: 15px 0;
            color: #34495e;
        }
        .agreement-text {
            line-height: 1.6;
            margin-bottom: 15px;
            color: #444;
        }
        .alert-info {
            background-color: #f8f9fa;
            border-left: 4px solid #17a2b8;
            padding: 15px;
            margin: 15px 0;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="container">
        <div class="agreement-section">
            <h1 class="agreement-title">ZEYTİN FABRİKA YAZILIMI SON KULLANICI SÖZLEŞMESİ</h1>
            
            <p class="agreement-text">
                İşbu Kullanıcı Sözleşmesi ("Sözleşme"), Olive Edremit Zeytincilik Sanayi Ticaret Limited Şirketi ("Şirket") ile 
                yazılım kullanıcısı ("Kullanıcı") arasında düzenlenmektedir. Kullanıcı tarafından, yazılımın kullanımına 
                yönelik kayıt oluşturulması esnasında onaylanarak kabul edilen işbu sözleşme, anında yürürlüğe girecek olup; 
                taraflarca Sözleşme'de belirtilen sebepler ortaya çıkmadığı sürece geçerliliğini koruyacaktır.
            </p>

            <h2 class="agreement-subtitle">1. Tanımlar</h2>
            <p class="agreement-text">
                İşbu Sözleşmede yer alan;<br>
                "SÖZLEŞME": İşbu kullanıcı sözleşmesini,<br>
                "TARAFLAR": Şirket ve Kullanıcı'yı,<br>
                "ŞİRKET": Olive Edremit Zeytincilik Sanayi Ticaret Limited Şirketi'ni,<br>
                "KULLANICI": Sözleşme'nin koşullarını onaylayarak yazılımdan yalnızca kendi ihtiyaçları için faydalanacak tüm gerçek ve tüzel hukuk kişilerini,<br>
                "YAZILIM": Şirket tarafından sunulan zeytin fabrikası yönetim yazılımını,<br>
                "İÇERİK": Kullanıcı tarafından yazılıma yüklenen her türlü veriyi, bilgiyi, dosyayı, rakamı, görseli, resmi, yazınsal ve işitsel imgeleri,<br>
                "HİZMET": Şirket tarafından yazılım vasıtasıyla Kullanıcı'ya sunulacak hizmetleri,<br>
                "FİKRİ MÜLKİYET HAKLARI": Her türlü patent, telif hakkı, ticari marka ve diğer fikri mülkiyet haklarını ifade eder.
            </p>

            <h2 class="agreement-subtitle">2. Şirket İletişim Bilgileri</h2>
            <p class="agreement-text">
                Adres: Zeytinli mah Sehit hamdi bey cad no 33 Edremit Balikesir<br>
                Telefon: 05462039845<br>
                E-posta: info@zeytinfabrika.com.tr
            </p>

            <h2 class="agreement-subtitle">3. Sözleşmenin Konusu</h2>
            <p class="agreement-text">
                İşbu sözleşme, Şirket'in sunacağı zeytin fabrikası yönetim yazılımı hizmetlerinden Kullanıcı'nın nasıl faydalanacağını; 
                Kullanıcı tarafından yazılıma yüklenecek her türlü veriyle ilgili koşul ve şartları, içeriği, 
                tarafların hak ve yükümlülüklerini içerir.
            </p>

            <h2 class="agreement-subtitle">4. Kullanıcının Hak ve Yükümlülükleri</h2>
            <p class="agreement-text">
                4.1. Kullanıcı, yazılımdan faydalanabilmek için gerekli olan tüm bilgileri tam, eksiksiz ve doğru olarak verdiğini kabul, beyan ve taahhüt eder.<br><br>
                4.2. Kullanıcı, 18 yaşını doldurmuş olduğunu ve işbu Sözleşme'yi akdetmek için gereken yasal ve hukuki ehliyete sahip bulunduğunu kabul ve beyan eder.<br><br>
                4.3. Kullanıcı, yazılıma kayıt olurken kullanılan verilerin doğruluğundan sorumludur ve gerektiğinde bu verileri tam ve eksiksiz olarak güncelleyecektir.<br><br>
                4.4. Kullanıcı'nın bir işletme tüzel kişiliği adına yazılımı kullanıyor olması halinde, bu işlem için tam yetkiye haiz olduğunu kabul ve beyan eder.
            </p>

            <h2 class="agreement-subtitle">5. Kişisel Verilerin Korunması</h2>
            <p class="agreement-text">
                5.1. Şirket, Kullanıcı tarafından verilen bilgileri veri tabanlarında tasnif edip saklayabilir, yazılımın amaçlarıyla 
                uyumlu olarak kullanabilir, işleyebilir. Kullanıcı bu konuda Şirket'e açıkça izin vermiştir.<br><br>
                5.2. Hassas Bilgiler: Şirket, Kullanıcılardan yasalar gerektirmediği sürece; ırk ve etnik kimlik, dini/siyasi/felsefi düşünce, 
                fiziksel ya da ruhsal sağlık ve özellikler gibi hassas bilgileri talep etmemektedir.<br><br>
                5.3. Kullanıcı, vermiş olduğu kişisel bilgilerin Şirket'e açık olacağını kabul eder.
            </p>

            <h2 class="agreement-subtitle">6. Fikri Mülkiyet Hakları</h2>
            <p class="agreement-text">
                6.1. Yazılımın tüm fikri mülkiyet hakları Şirket'e aittir.<br><br>
                6.2. Kullanıcı, yazılımı yalnızca izin verilen amaçlar doğrultusunda kullanacağını kabul eder.<br><br>
                6.3. Yazılımın kopyalanması, değiştirilmesi veya tersine mühendislik yapılması yasaktır.
            </p>

            <h2 class="agreement-subtitle">7. Gizlilik Koşulları</h2>
            <p class="agreement-text">
                7.1. Şirket, gizlilik koşullarının ihlali durumunda, sistemde mevcut bilgileri reddetme, içerikten çıkarma, 
                silme hakkıyla birlikte önceden haber vermeksizin Kullanıcıların yazılıma erişimini askıya alma, 
                sonlandırma hakkını saklı tutar.<br><br>
                7.2. Bu kural, koşulların Kullanıcı tarafından dolaylı ihlali ya da ihlal girişimi olması durumu için de geçerlidir.
            </p>

            <h2 class="agreement-subtitle">8. Ticari İletişim</h2>
            <p class="agreement-text">
                Şirket, Kullanıcı'ya iletilmiş iletişim adresine, elektronik ileti veya ticari elektronik ileti gönderebilir. 
                Kullanıcı, ticari nitelikteki elektronik iletileri reddetme hakkını her zaman kullanabilir. 
                Red talepleri Şirket tarafından en geç 7 (yedi) iş günü içinde işleme alınacaktır.
            </p>

            <h2 class="agreement-subtitle">9. İletişim ve Bildirimler</h2>
            <p class="agreement-text">
                Yazılım ve sözleşme ile ilgili tüm soru ve bildirimleriniz için yukarıda belirtilen iletişim bilgilerini kullanabilirsiniz.
            </p>

            <div class="alert-info">
                <p class="agreement-text">
                    <strong>Not:</strong> Bu sözleşme, yazılımın kullanımına ilişkin temel kuralları ve şartları belirler. 
                    Sözleşmenin herhangi bir maddesinin ihlali, hesabınızın askıya alınmasına veya sonlandırılmasına neden olabilir.
                </p>
            </div>
        </div>
    </div>
</asp:Content>

