using OdevPortaliSon.Models;
using OdevPortaliSon.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace OdevPortaliSon.Controllers
{
    public class OdevController : ApiController
    {
        DB01Entities db = new DB01Entities();
        SonucModel sonuc = new SonucModel();

        [HttpPost]
        [Route("api/odevekle/{dersId}")]
        public SonucModel OdevEkle([FromBody]OdevModel model, [FromUri] string dersId)
        {
            if (db.Odev.Any(x => x.odevAdi == model.odevAdi))
            {
                sonuc.islem = false;
                sonuc.mesaj = "Girilen Ödev Kayıtlıdır!";
                return sonuc;
            }

            Odev yeni = new Odev();
            yeni.odevId = Guid.NewGuid().ToString();
            yeni.odevAdi = model.odevAdi;
            yeni.odevDersId = dersId;


            db.Odev.Add(yeni);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Odev Eklendi";
            return sonuc;
        }

        [HttpGet]
        [Route("api/odevliste")]
        public List<OdevModel> OdevListe()
        {
            List<OdevModel> liste = db.Odev.Select(x => new OdevModel()
            {
                odevAdi = x.odevAdi,
                odevId = x.odevId,
                odevDersId = x.odevDersId,

            }).ToList();
            return liste;
        }

        [HttpDelete]
        [Route("api/odevsil/{odevId}")]
        public SonucModel OdevSil(string odevId)
        {
            Odev kayit = db.Odev.Where(s => s.odevId == odevId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Kayıt Bulunamadı!";
                return sonuc;
            }
            db.Odev.Remove(kayit);
            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ders Silindi";
            return sonuc;
        }

        [HttpPut]
        [Route("api/odevduzenle")]
        public SonucModel OdevDuzenle(OdevModel model)
        {
            Odev kayit = db.Odev.Where(s => s.odevId == model.odevId).SingleOrDefault();
            if (kayit == null)
            {
                sonuc.islem = false;
                sonuc.mesaj = "Ödev Bulunamadı!";
                return sonuc;
            }
            kayit.odevAdi = model.odevAdi;

            db.SaveChanges();
            sonuc.islem = true;
            sonuc.mesaj = "Ödev Düzenlendi";
            return sonuc;
        }

    }
}