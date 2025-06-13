using System;

public static class Constants
{
    public static class PartiDurumu
    {
        public const string KABUL_EDILDI = "Kabul Edildi";
        public const string KALITE_ONAYLI = "Kalite Onaylı";
        public const string URETIMDE = "Üretimde";
        public const string TAMAMLANDI = "Tamamlandı";
        public const string IPTAL = "İptal";
    }

    public static class MakineDurumu
    {
        public const string MUSAIT = "Müsait";
        public const string MESGUL = "Meşgul";
        public const string BAKIMDA = "Bakımda";
    }

    public static class MakineTipi
    {
        public const string ZEYTINYAGI_MAKINESI = "ZeytinyagiMakinesi";
        public const string MALAKSOR = "Malaksor";
    }

    public static class OperatorDurumu
    {
        public const string MUSAIT = "Müsait";
        public const string MESGUL = "Meşgul";
        public const string IZINLI = "İzinli";
    }

    public static class OperatorUzmanlik
    {
        public const string ZEYTINYAGI = "Zeytinyağı";
        public const string SALAMURA = "Salamura";
        public const string GENEL = "Genel";
    }

    public static class UretimDurumu
    {
        public const string BEKLEMEDE = "Beklemede";
        public const string DEVAM_EDIYOR = "Devam Ediyor";
        public const string TAMAMLANDI = "Tamamlandı";
        public const string IPTAL = "İptal";
    }

    public static class Messages
    {
        public const string URETIM_BASLATILDI = "Üretim başarıyla başlatıldı. Üretim durumu sayfasına yönlendiriliyorsunuz...";
        public const string URETIM_IPTAL_EDILDI = "Üretim iptal edildi. Ana sayfaya yönlendiriliyorsunuz...";
        public const string PARTI_SECIN = "Lütfen bir parti seçin";
        public const string MAKINE_SECIN = "Lütfen bir makine seçin";
        public const string OPERATOR_SECIN = "Lütfen bir operatör seçin";
        public const string GECERSIZ_MIKTAR = "Lütfen geçerli bir işlenecek miktar girin";
        public const string GECERSIZ_SURE = "Lütfen geçerli bir tahmini süre girin";
    }
}
