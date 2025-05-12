using System;
using System.Collections.Generic;
using System.Linq;

public static class FabrikaTools
{
    public static class Musteri
    {
        // Açık Bakiye Hesaplama
        public static decimal Acikbakiye(int musteriId)
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                // Finans tablosunda ilgili müşterinin toplam bakiyesi
                // var bakiye = db.Finans.Where(f => f.musteriId == musteriId).Sum(f => (decimal?)f.Tutar) ?? 0;
                var bakiye = 10;
                return bakiye;
            }
        }

        // Çek/Senet Bakiye Hesaplama
        public static decimal CekSenetBakiye(int musteriId)
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                // CekSenetler tablosunda ilgili müşterinin toplam bakiyesi
                // var bakiye = db.CekSenetler.Where(c => c.musteriId == musteriId).Sum(c => (decimal?)c.Tutar) ?? 0;
                var bakiye = 20;
                return bakiye;
            }
        }

        // Çek/Senet Bakiye Hesaplama
        public static List<Musteriler> MusteriListesi(int _SirketID, bool _MusteriDurumu)
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                List<Musteriler> _musteriler = db.Musterilers
                    .Where(x => x.SirketID == _SirketID && x.Durum == _MusteriDurumu)
                    .ToList();
                return _musteriler;
            }
        }
    }


    public static class Tedarikci
    {
        // Açık Bakiye Hesaplama
        public static decimal Acikbakiye(int TedarikciId)
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                // Finans tablosunda ilgili müşterinin toplam bakiyesi
                // var bakiye = db.Finans.Where(f => f.TedarikciId == TedarikciId).Sum(f => (decimal?)f.Tutar) ?? 0;
                var bakiye = 10;
                return bakiye;
            }
        }

        // Çek/Senet Bakiye Hesaplama
        public static decimal CekSenetBakiye(int TedarikciId)
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                // CekSenetler tablosunda ilgili müşterinin toplam bakiyesi
                // var bakiye = db.CekSenetler.Where(c => c.TedarikciId == TedarikciId).Sum(c => (decimal?)c.Tutar) ?? 0;
                var bakiye = 20;
                return bakiye;
            }
        }

        // Çek/Senet Bakiye Hesaplama
        public static List<Tedarikciler> TedarikciListesi(int _SirketID, bool _TedarikciDurumu)
        {
            using (var db = new FabrikaDataClassesDataContext())
            {
                List<Tedarikciler> _Tedarikciler = db.Tedarikcilers
                    .Where(x => x.SirketID == _SirketID && x.Durum == _TedarikciDurumu)
                    .ToList();
                return _Tedarikciler;
            }
        }

    }




}