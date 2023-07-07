using Application.Constants;
using Application.Interface;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Interfaces;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Logging;
using StorageApplication.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StockManager : IStockServices
    {
        IStockDal _stockRepository;
        readonly ILoggerDal _loggerRepository;
        readonly IBaseLoggerDal _baseLoggerRepository;
        IStoreDal _storeRepository;
        public StockManager(IStockDal stockRepository, IStoreDal storeRepository, ILoggerDal loggerRepository, IBaseLoggerDal baseLoggerRepository)
        {
            _stockRepository = stockRepository;
            _storeRepository = storeRepository;
            _loggerRepository = loggerRepository;
            _baseLoggerRepository = baseLoggerRepository;
        }

        public async Task<IResult> Add(Stock entity)
        {
            if (entity!=null)
            {
                await _stockRepository.CreateAsync(entity);
                return new SuccessResult(ConstMessages.StockAdded);
            }
            return new ErrorResult(ConstMessages.StockError);
        }

        public Task<IResult> Delete(Stock entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IDataResults<List<Stock>>> GetAll(Expression<Func<Stock, bool>> filter = null)
        {
            try
            {
                return new SuccessDataResult<List<Stock>>(await _stockRepository.GetAllAsync());
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<Stock>>();
            }
        }

        //public async Task<IResult> GetAllQuery(Stock entity)
        //{
        //    return  
        //}

        public  async Task<IDataResults<Stock>> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Stock>((await _stockRepository.GetByFilterAsync(a=>a.Id==id)));
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Stock>();
            }
        }

     
        public async Task<string> RemoveStockForStore(List<RemoveStockDto> productList)
        {
            //stoğu olmayan ürün isimlerini toplamak için
                string checkProductName = "";
      
            try
            {
                foreach (var item in productList)
                {
                    var stock = await (_stockRepository.GetAll().Where(a => a.Store.Id == item.StoreId && a.Product.Id == item.ProductId).Include(a => a.Product).Include(a => a.Store)).FirstOrDefaultAsync();

                    if (stock.StockAmount > 0)
                    {
                        stock.StockAmount = stock.StockAmount-item.Amount;
                        if (stock.StockAmount <0)
                        {
                            //stoğu yetersizse olmayan ürün listesine ekle
                            checkProductName += stock.Product.Name + ";";
                            continue;
                        }
                        await UpdateAll(stock);
                    }
                    else
                    {
                        //stoğu yoksa ekleme yap
                        checkProductName+= stock.Product.Name+";";
                    }

                }
            }
            catch (Exception ex)
            {
                ///veritabanıyla ilgili bir hata olursa stok uyarısı ver
                return "0";
            }

            // hazırlanan string'i gönder. boş giderse sorun yok dolu giderse çağırıldığı yerse split edilip hatalı ürünler alınmalı
            return checkProductName;

        }
        public async Task<List<RemoveStockDto>> GetRemoveStockProductAndCount(List<ProductAndAmountDto> sayfadaGosterilecekListe, List<ProductAndAmountDto> stoktanSilinecekler)
        {
            var baseLogId = await _baseLoggerRepository.CreateAsyncReturnId(new() { LogName = "Bilgi Logu" });
            List<RemoveStockDto> removeStockCount = new List<RemoveStockDto>();
            foreach (var silinecekUrun in sayfadaGosterilecekListe)
            {
                var bulunanUrun = stoktanSilinecekler.FirstOrDefault(a => a.UrunId == silinecekUrun.UrunId);
                if (bulunanUrun != null)
                {
                    //veritanında bulunan urun silienecek üründen fazla olup olmama durumu kontrol ediliir
                    if (bulunanUrun.Miktar > silinecekUrun.Miktar)
                    {
                        var kalanMiktar= bulunanUrun.Miktar - silinecekUrun.Miktar;
                        var depoMiktari = await _storeRepository.GetByFilterAsync(a => a.Id == silinecekUrun.DepoId);
                        LogModel models = new()
                        {
                            DepoId = silinecekUrun.DepoId,
                            AracKapasitesi = ConstHelper.VehicleSize(),
                            DepoSiparisOncesiMiktar= silinecekUrun.Miktar,
                            DepoSiparisSonrasiMiktar= kalanMiktar,
                            AracId=1,
                            IslemZamani=DateTime.Now,
                            Konum=silinecekUrun.Konum,
                            UrunAdi=silinecekUrun.UrunAdi,
                          UrunMiktarı = bulunanUrun.Miktar,
                          UrunId = silinecekUrun.UrunId,
                            Agirlik=silinecekUrun.Agirlik,
                            Mod= silinecekUrun.Mod,
                            Zaman = silinecekUrun.Zaman,
                            DepoKapasitesi= depoMiktari.StoreSize.ToString()

                        };
                        var ids = await (new LogHelpers(_loggerRepository, _baseLoggerRepository).CreateLog(models, Convert.ToInt32(baseLogId)));
                        bulunanUrun.Miktar = kalanMiktar;
                        //fazlaysa silinecek urun miktarı kaydedilir
                        removeStockCount.Add(new() { Amount = silinecekUrun.Miktar, ProductId = silinecekUrun.UrunId, StoreId = silinecekUrun.DepoId , LogId=Convert.ToInt32(ids) });

                      
                    }
                    else
                    {
                        //veritanında bulunan urun silienecek üründen azsa
                        if (silinecekUrun.Miktar >= bulunanUrun.Miktar)
                        {
                            //veritabanında bulunan miktar kadar stoktan düş
                            var kalanMiktar =  silinecekUrun.Miktar- bulunanUrun.Miktar ;
                            var depoMiktari = await _storeRepository.GetByFilterAsync(a => a.Id == silinecekUrun.DepoId);

                            LogModel models = new()
                            {
                                DepoId = silinecekUrun.DepoId,
                                AracKapasitesi = ConstHelper.VehicleSize(),
                                DepoSiparisOncesiMiktar = silinecekUrun.Miktar,
                                DepoSiparisSonrasiMiktar = kalanMiktar,
                                AracId = 1,
                                IslemZamani = DateTime.Now,
                                Konum = silinecekUrun.Konum,
                                UrunAdi = silinecekUrun.UrunAdi,
                                UrunMiktarı = bulunanUrun.Miktar,
                                Mod = silinecekUrun.Mod,
                                Zaman = silinecekUrun.Zaman,
                                UrunId = silinecekUrun.UrunId,
                                Agirlik = silinecekUrun.Agirlik,
                                DepoKapasitesi = depoMiktari.StoreSize.ToString()
                            };
                            var id = await ( new LogHelpers(_loggerRepository, _baseLoggerRepository).CreateLog(models, Convert.ToInt32(baseLogId)));

                            removeStockCount.Add(new() { Amount = bulunanUrun.Miktar, ProductId = silinecekUrun.UrunId, StoreId = silinecekUrun.DepoId,LogId = Convert.ToInt32(id) });
                            bulunanUrun.Miktar = 0;
                        }
                        else
                        {
                            ///aksi bir durumla kaşılaşılması durumunda bu if blogunu koydum
                            if (bulunanUrun.Miktar > 0)
                            {
                                removeStockCount.Add(new() { Amount = bulunanUrun.Miktar, ProductId = silinecekUrun.UrunId, StoreId = silinecekUrun.DepoId });
                                bulunanUrun.Miktar = bulunanUrun.Miktar - silinecekUrun.Miktar;
                            }

                        }
                    }

                }
            }
            return removeStockCount;
        }
        public Task<IResult> Update(Stock entity)
        {
            throw new NotImplementedException();
        }

        public async Task<IResult> UpdateAll(Stock entity)
        {
            if (entity != null)
            {
                await _stockRepository.UpdateAll(entity);
                return new SuccessResult(ConstMessages.StockUpdate);
            }
            return new ErrorResult(ConstMessages.StockUpdateError);
        }
     

    }
}
