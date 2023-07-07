using Application.Constants;
using Application.Interface;
using Application.Services;
using DataAccess.EntityFramework;
using DataAccess.Interfaces;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.AspNetCore.Http;
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
using System.Security.Cryptography;
using static Application.Constants.StorageApplicationConst;
using static StorageApplication.Controllers.HomeController;
using static StorageApplication.Helpers.CoordinateHelper;
using static StorageApplication.Helpers.EnumHelper;

namespace StorageApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ILoggerServices _loggerServices;
        private readonly ILoggerDal _loggerRepository;
        private readonly IProductServices _productManager;
        private readonly IStockServices _stockManager;
        private readonly IWebHostEnvironment _evn;
        private readonly IWeightDal _weightRepository;
        private readonly IAmountDal _amounRepository;
        private readonly IPhysicalInformationServices physicalInformationServices;

        public HomeController(ILogger<HomeController> logger, IProductServices productManager, IStockServices stockManager, IWebHostEnvironment evn, IWeightDal weightRepository, IAmountDal amounRepository, IPhysicalInformationServices physicalInformationServices, ILoggerServices loggerServices, ILoggerDal loggerRepository)
        {
            _logger = logger;
            _productManager = productManager;
            _stockManager = stockManager;
            _evn = evn;
            _weightRepository = weightRepository;
            _amounRepository = amounRepository;
            this.physicalInformationServices = physicalInformationServices;
            _loggerServices = loggerServices;
            _loggerRepository = loggerRepository;
        }


        public async Task<IActionResult> Index()
        {
          
            ViewBag.Product = (await _productManager.GetAll()).Data;
            ViewBag.Amount = await _amounRepository.GetAllAsync();
            ViewBag.Weight = await _weightRepository.GetAllAsync();
          
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> GetStoreLocationAndProduct([FromBody] List<ProductAndAmountDto> productAndAmountDto,string l,string l2,string onemli)
        {
            bool onemliMi = false;
            if (onemli == "onemsiz")
            {
                onemliMi = true;
            }
            var sortedIds = productAndAmountDto
             .OrderBy(a => a.Agirlik)
             .Select(a => a.UrunId)
             .ToList();

            List<ProductAndAmountDto> stoktanSilinecekler = new List<ProductAndAmountDto>();
            var sayfadanGelenUrunListesi = productAndAmountDto.Where(item => item != null).ToList();
            stoktanSilinecekler.AddRange(sayfadanGelenUrunListesi.Select(item => new ProductAndAmountDto
            {
                UrunId = item.UrunId,
                Miktar = item.Miktar,
                DepoId = item.DepoId,
            }));


            var resultProductList = new List<ProductAndAmountDto>();
            if (!onemliMi)
                resultProductList = await _productManager.GetProductAndAmount(productAndAmountDto.Where(item => item != null).ToList());
            else
            {
                resultProductList = await _productManager.GetProductAndAmountForBestStores(productAndAmountDto.Where(item => item != null).ToList());
                //sadece en yüksek konumlara bakıldığında depo bulunamazsa beni 0'dan büyük olan depolara bakması için diğer sp'ye gönderir.
                if (resultProductList.Count==0)
                {
                    resultProductList = await _productManager.GetProductAndAmount(productAndAmountDto.Where(item => item != null).ToList());
                }
            }
            foreach (var item in productAndAmountDto)
            {
                foreach (var item2 in resultProductList)
                {
                    if (item.UrunId == item2.UrunId)
                    {
                        item2.Agirlik = item.Agirlik;
                        item2.Zaman = item.Zaman;
                        item2.Mod = item.Mod;

                    }
                }
            }
            if (resultProductList.Count==0)
                return Json("0");


        

            ProductAmountAndLocation productAmountAndLocation = await _productManager.FindBestCordinat(resultProductList);
            
            ///dosya oluşturma
            WriteText(productAmountAndLocation.ProductAndAmount);

            //en kısa yolu seçer ve ilk 3'ünü alır
            Coordinate sourceCoordinate = new() { Latitude = Convert.ToDouble(l, CultureInfo.InvariantCulture), Longitude= Convert.ToDouble(l2, CultureInfo.InvariantCulture) };
            List<Coordinate> shortestCoordinates = productAmountAndLocation.Cordinate
                .OrderBy(c => CoordinateHelper.CalculateDistance(sourceCoordinate, c))
                //.Take(4)
                .ToList();

         

            //sayfada gösterilecek liste filtrelenir
           var sayfadaGosterilecekListe=  SayfadaGosterilecekListe(shortestCoordinates,resultProductList, productAndAmountDto);


            // stoktan düşülecek ürünler listelenir. burada miktarı almak için bir döngü döner

            var removeStockProduct = await _stockManager.GetRemoveStockProductAndCount(sayfadaGosterilecekListe,stoktanSilinecekler);
            List<int>logIdTemp= new List<int>();
            foreach (var item in removeStockProduct)
            {
                logIdTemp.Add(item.LogId);
            }
         

            // bulunan ürünler stoktan düşürülmesi için servise gider
            List<string> errorProductList = new List<string>();
            var removeStoreStock = await _stockManager.RemoveStockForStore(removeStockProduct);
            if (removeStoreStock=="0")
                return Json("0");
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
                        errorProductList.Add((await _productManager.GetById(error.UrunId)).Data.Name);
                }
                
            ///önceliğe göre ayarlama
            sayfadaGosterilecekListe = sayfadaGosterilecekListe
                .OrderByDescending(item => sortedIds.IndexOf(item.UrunId))
                .ToList();


                ///sayfada stoğu olmayan ürünleri göstermek için ilk ürününün içine bir liste eklenir bu liste sayfada açılarak hatalı ürünler alert olarak sayfaya basılır.
            sayfadaGosterilecekListe.FirstOrDefault().ErrorMessage= errorProductList;
            sayfadaGosterilecekListe.FirstOrDefault().logIdTemp= logIdTemp;

           
            return Json(sayfadaGosterilecekListe);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> LogAddDistance([FromBody] LogModelDto model)
        {
         
            foreach (var item in model.logId)
            {
                var log=await _loggerRepository.GetByFilterAsync(a=>a.Id==item);
                var splitTime = model.time.Split(",");
                log.YolculukUzunlugu = splitTime[0];
                log.YolculukSuresi = splitTime[1];
                await _loggerRepository.UpdateAll(log);
            }
            var listem =  await _loggerRepository.GetAllAsync(a => model.logId.Contains(Convert.ToInt32(a.Id)));
            var convertList = JsonConvert.SerializeObject(listem);
            HttpContext.Session.SetString("amacFonksiyonu", convertList);
            return Json("");
        }

        public bool WriteText(List<ProductAndAmountDto> model)
        {

            #region TEXT ÇIKTISI İÇİN
            var rootPath = _evn.WebRootPath;

            string kayitYolu = "DepoveUrun.txt";

            string dosyaYolu = Path.Combine(rootPath, kayitYolu);

            using (StreamWriter dosya = new StreamWriter(dosyaYolu))
            {
                foreach (var item in model)
                {
                    dosya.WriteLine(
                        "Depo Adı" + " " + item.DepoAdi + "\n" +
                        "Depo Konumu" + " " + item.Konum + "\n" +
                        "Depo Büyüklüğü" + " " + item.Miktar + "\n" +
                        "Ürün Adı" + " " + item.UrunAdi + "\n");
                }
            }
            #endregion
            return true;
        }

        public List<ProductAndAmountDto> SayfadaGosterilecekListe(List<Coordinate> shortestCoordinates,List<ProductAndAmountDto> resultProductList, List<ProductAndAmountDto> productAndAmountDto)
        {
            List<ProductAndAmountDto> sayfadaGosterilecekListe = new List<ProductAndAmountDto>();
            foreach (var veritabanindanAlinanan in resultProductList.OrderBy(a => a.UrunId))
            {
                var indexSayfasindanGonderilen = productAndAmountDto.Where(item => item != null).FirstOrDefault(a => a.UrunId == veritabanindanAlinanan.UrunId);
                //ana sayfadan gönderilen ürünlerle en kısa yol kordinatları bulunan ürünler karşılaştırılır.
                if (veritabanindanAlinanan.UrunId == indexSayfasindanGonderilen.UrunId && indexSayfasindanGonderilen.Miktar > 0 && shortestCoordinates.Where(a => a.Latitude == veritabanindanAlinanan.Latitude && a.Longitude == veritabanindanAlinanan.Longitude).Count() > 0)
                {
                    //burada birden fazla depoya gitmek zorunda kalma durumunu da içine alır.
                    //miktar sayfadan gelen miktardan düşülür ve if blogunda miktar kontrolü yapılır. miktar 0 olana kadar ürün ıd'ler eşletikçe kısa kordinatlar eklenir(yeni depo adresleri).
                    indexSayfasindanGonderilen.Miktar = indexSayfasindanGonderilen.Miktar - veritabanindanAlinanan.Miktar;
                    //listeme ekler.
                    sayfadaGosterilecekListe.Add(veritabanindanAlinanan);
                }
            }
            return sayfadaGosterilecekListe;
        }
        [HttpPost]
        public async Task<IActionResult> AddPicture(List<IFormFile> images)
        {
            // Her bir resim için işlemler yapabilirsiniz
            foreach (var image in images)
            {
                if (image != null && image.Length > 0)
                {
                    // Resimle ilgili işlemleri gerçekleştirin (örneğin, kaydetmek veya işlemek)
                    // Örneğin, resimleri sunucuda bir klasöre kaydetmek için:
                    string folder = _evn.WebRootPath;   
                    string specificFolder = Path.Combine(folder, "UploadImage");

                    //guid Id aynı resim kayıt işlemi olmamasını sağlar. Kapatıldı  yüklenecek resim adları birbirinden farklı olmalı !!!!!!!!!!!!
                    var fileName = /*Guid.NewGuid().ToString() + "_" +*/ image.FileName;
                    var filePath = Path.Combine(specificFolder, fileName);
                    if (!Directory.Exists(specificFolder))
                        Directory.CreateDirectory(specificFolder);
                    
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        image.CopyTo(fileStream);
                    }
                }
            }

            // Gerekli işlemleri tamamladıktan sonra uygun bir yanıt döndürün
            List<State> state = new List<State>();
            state.Add(new()
            {
                Name = "Orman",
            });
            state.Add(new()
            {
                Name = "Su",
            });
            var result = await physicalInformationServices.FizikselBilgiler(state);
            var convertList = JsonConvert.SerializeObject(result);
            HttpContext.Session.SetString("stateFunc", convertList);
            return Json(result);
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public double GetFirstFunction()
        {
            var sessionModel = HttpContext.Session.GetString("amacFonksiyonu");
            var logModel = JsonConvert.DeserializeObject<List<LogModel>>(sessionModel);
            var result = new ScopeFunction().GetFirstFunction(logModel);
            return result;
        }
        public double GetSecondFunction()
        {
            var sessionModel = HttpContext.Session.GetString("amacFonksiyonu");
            var state = HttpContext.Session.GetString("stateFunc");
            var logModel = JsonConvert.DeserializeObject<List<LogModel>>(sessionModel);
            var stateModel = JsonConvert.DeserializeObject<StateFunctionDto>(state);
            LogAndStateDto dto = new()
            {
                LogModel = logModel,
                StateFunctionDto = stateModel

            };
           
            var result = new ScopeFunction().GetSecondFunction(dto);
            return result;
        }
    }
}