using Application.Constants;
using Application.Interface;
using Application.Services;
using DataAccess.EntityFramework;
using DataAccess.Migrations;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Tasks.Deployment.Bootstrapper;
using Newtonsoft.Json;
using NuGet.Packaging;
using StorageApplication.Helpers;
using StorageApplication.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using static Application.Constants.StorageApplicationConst;
using static StorageApplication.Controllers.HomeController;
using static StorageApplication.Helpers.EnumHelper;

namespace StorageApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductServices _productManager;
        private readonly IStockServices _stockManager;
        StorageApplicationContext db = new StorageApplicationContext();
      

        public HomeController(ILogger<HomeController> logger, IProductServices productManager, IStockServices stockManager)
        {
            _logger = logger;
            _productManager = productManager;
            _stockManager = stockManager;
        }
        public class Coordinate
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public double CalculateDistance(Coordinate coord1, Coordinate coord2)
        {
            const double earthRadius = 6371; // Dünya yarıçapı (km)

            var dLat = DegreesToRadians(coord2.Latitude - coord1.Latitude);
            var dLon = DegreesToRadians(coord2.Longitude - coord1.Longitude);

            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(DegreesToRadians(coord1.Latitude)) * Math.Cos(DegreesToRadians(coord2.Latitude)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = earthRadius * c;

            return distance;
        }

        private double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180;
        }
        public async Task<IActionResult> Index()
        {
         
            ViewBag.Product = (await _productManager.GetAll()).Data;
            var model = new EnumViewModel
            {
                WeightOptions = EnumHelper.GetEnumSelectList<StorageApplicationConst.Weight>(),
                WeightSelectedValue = "Seçim Yapınız",
                AmountOptions = EnumHelper.GetEnumSelectList<StorageApplicationConst.Amount>(),
                AmountSelectedValue = "Seçim Yapınız"
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GetStoreLocationAndProduct([FromBody] List<ProductAndAmountDto> productAndAmountDto,string l,string l2)
        {
            List<ProductAndAmountDto> resultProductAndLocation = new List<ProductAndAmountDto>();
            List<ProductAndAmountDto> stoktanSilinecekler = new List<ProductAndAmountDto>();
            var sayfadanGelenUrunListesi = productAndAmountDto.Where(item => item != null).ToList();
            stoktanSilinecekler.AddRange(sayfadanGelenUrunListesi.Select(item => new ProductAndAmountDto
            {
                UrunId = item.UrunId,
                Miktar = item.Miktar,
            }));



            var resultProductList = await _productManager.GetProductAndAmount(productAndAmountDto.Where(item => item != null).ToList());
            if (resultProductList.Count==0)
            {
                return Json("0");
            }



            List<Coordinate> coordinates = new List<Coordinate>();// Veritabanından koordinatları alır
            foreach (var product in resultProductList)
            {
                var xy = product.Konum.Split(",");
                coordinates.Add(new() { Latitude = Convert.ToDouble(xy[0], CultureInfo.InvariantCulture), Longitude = Convert.ToDouble(xy[1], CultureInfo.InvariantCulture) });
                product.Latitude = Convert.ToDouble(xy[0], CultureInfo.InvariantCulture);
                product.Longitude = Convert.ToDouble(xy[1], CultureInfo.InvariantCulture);
                resultProductAndLocation.Add(product);
            }


            //en kısa yolu seçer ve ilk 3'ünü alır
            Coordinate sourceCoordinate = new() { Latitude = Convert.ToDouble(l, CultureInfo.InvariantCulture), Longitude= Convert.ToDouble(l2, CultureInfo.InvariantCulture) };//lat: 37.022882115499755, lng: 30.60387288257859 // Kaynak koordinatı belirleyin
            List<Coordinate> shortestCoordinates = coordinates
                .OrderBy(c => CalculateDistance(sourceCoordinate, c))
                //.Take(4)
                .ToList();



            List<int>tempId=new List<int>();
            List<ProductAndAmountDto> checkProductAndLocation = new List<ProductAndAmountDto>();
            // en kısa kordinatlara sahip ürünlerin bilgileri alınır
            List<ProductAndAmountDto> sayfadaGosterilecekListe = new List<ProductAndAmountDto>();

            foreach (var veritabanindanAlinanan in resultProductList.OrderBy(a => a.UrunId))
            {
                var indexSayfasindanGonderilen = productAndAmountDto.Where(item => item != null).FirstOrDefault(a => a.UrunId == veritabanindanAlinanan.UrunId);
                //ana sayfadan gönderilen ürünlerle en kısa yol kordinatları bulunan ürünler karşılaştırılır.
                if (veritabanindanAlinanan.UrunId== indexSayfasindanGonderilen.UrunId&& indexSayfasindanGonderilen.Miktar>0&& shortestCoordinates.Where(a=>a.Latitude== veritabanindanAlinanan.Latitude&&a.Longitude== veritabanindanAlinanan.Longitude).Count()>0)
                {
                    //burada birden fazla depoya gitmek zorunda kalma durumunu da içine alır.
                    //miktar sayfadan gelen miktardan düşülür ve if blogunda miktar kontrolü yapılır. miktar 0 olana kadar ürün ıd'ler eşletikçe kısa kordinatlar eklenir(yeni depo adresleri).
                    indexSayfasindanGonderilen.Miktar = indexSayfasindanGonderilen.Miktar - veritabanindanAlinanan.Miktar;
                    //listeme ekler.
                    sayfadaGosterilecekListe.Add(veritabanindanAlinanan);
                }
            }



          
                 
               
                
            List<RemoveStockDto> removeStockCount = new List<RemoveStockDto>();
            // stoktan düşülecek ürünler listelenir. burada miktarı almak için bir döngü döner
            foreach (var silinecekUrun in sayfadaGosterilecekListe)
            {
                var bulunanUrun = stoktanSilinecekler.FirstOrDefault(a => a.UrunId == silinecekUrun.UrunId);
                if (bulunanUrun != null)
                {  
                //veritanında bulunan urun silienecek üründen fazla olup olmama durumu kontrol ediliir
                    if (bulunanUrun.Miktar> silinecekUrun.Miktar)
                    {   
                        bulunanUrun.Miktar = bulunanUrun.Miktar - silinecekUrun.Miktar;
                        //fazlaysa silinecek urun miktarı kaydedilir
                        removeStockCount.Add(new() { Amount = silinecekUrun.Miktar, ProductId = silinecekUrun.UrunId, StoreId = silinecekUrun.DepoId });
                 
                    }
                    else
                    {
                        //veritanında bulunan urun silienecek üründen azsa
                        if (silinecekUrun.Miktar>= bulunanUrun.Miktar)
                        {
                            //veritabanında bulunan miktar kadar stoktan düş
                            removeStockCount.Add(new() { Amount = bulunanUrun.Miktar, ProductId = silinecekUrun.UrunId, StoreId = silinecekUrun.DepoId });
                            bulunanUrun.Miktar = 0;
                        }
                        else
                        {
                            ///aksi bir durumla kaşılaşılması durumunda bu if blogunu koydum
                            if (bulunanUrun.Miktar>0)
                            {
                                removeStockCount.Add(new() { Amount = bulunanUrun.Miktar, ProductId = silinecekUrun.UrunId, StoreId = silinecekUrun.DepoId });
                                bulunanUrun.Miktar = bulunanUrun.Miktar - silinecekUrun.Miktar ;
                            }
                            
                        }
                    }
                   
                }
            }
            // bulunan ürünler stoktan düşürülmesi için servise gider
            List<string> errorProductList = new List<string>();
            var removeStoreStock = await _stockManager.RemoveStockForStore(removeStockCount);
            if (removeStoreStock=="0")
            {
                ///stok boş gelirse uyarı getir
                return Json("0");
            }
            else
            {
                //stok sayfasında eğer güncelleme  işlemi yaparken stok miktarı adet miktarını aşma durumu olursa  bu kurala uymayan ürünlerin ismini döner
                ///dönen ürünler sayfada gösterilir. bu ürünlerin stokları yok mesajıyla
                var splitErrorResult = removeStoreStock.Split(";");

                ///hata mesajı yoksa liste count 1 olarak gelir ilk elemanı "" olduğu için 1'den başlatılır
                if (splitErrorResult.Where(a=>a!="").Count()>1)
                {
                    foreach (var error in splitErrorResult)
                    {
                        //varsa hatalı ürünler eklenir
                        errorProductList.Add(error);
                    }
                }
            }

            ///stoktan dönen ürünlerden bağımsız eğer oradan hatalı bir mesaj gelmezse 2. bir kontrol yapılır
                foreach (var error in sayfadanGelenUrunListesi)
                {

                //sayfadan gönderdiğim veriyle burada işleyerek tekrar sayfaya göndereceğim verilerin sayısı eşitmi
                //stoğu olmayan ürünler neler burada kontrolü yapılır.
                   var kontrol= sayfadaGosterilecekListe.FirstOrDefault(a => a.UrunId == error.UrunId);
                    //eğer sayfada gösterilecek ürünler listesinde, benim sipariş ettiğim ürün yoksa o ürünü bulur ve adını alır
                    if (kontrol == null)
                    {
                    ///ürünü bul ve adını al
                        errorProductList.Add((await _productManager.GetById(error.UrunId)).Data.Name);
                    }
                }
               
                ///sayfada stoğu olmayan ürünleri göstermek için ilk ürününün içine bir liste eklenir bu liste sayfada açılarak hatalı ürünler alert olarak sayfaya basılır.
            sayfadaGosterilecekListe.FirstOrDefault().ErrorMessage= errorProductList;

            return Json(sayfadaGosterilecekListe);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}