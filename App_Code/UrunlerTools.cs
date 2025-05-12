using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UrunlerTools
/// </summary>
public class UrunlerTools
{
    public static class UrunGorsel
    {
        //
        // TODO: Add constructor logic here
        //
        // Açık Bakiye Hesaplama
        public static String UrunGorselKapak(string _UrunID)
        {
            int _Urun_Id = int.Parse(_UrunID);
            using (var db = new FabrikaDataClassesDataContext())
            {
                string urun_gorsel;
                var _UrunGorselleri = db.UrunGorselleris.FirstOrDefault(x => x.UrunID == _Urun_Id && x.Kapak == true);
                if (_UrunGorselleri == null)
                {
                    urun_gorsel = "~/fabrika/Urunler/Sablonlar/Urun_Gecici_Gorseli.png";
                }
                else
                {
                    urun_gorsel = _UrunGorselleri.ResimYol;
                }

                // Finans tablosunda ilgili müşterinin toplam bakiyesi
                // var bakiye = db.Finans.Where(f => f.musteriId == musteriId).Sum(f => (decimal?)f.Tutar) ?? 0;

                return urun_gorsel;
            }
        }


        
    }
}

