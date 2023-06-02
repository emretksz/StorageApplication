using Application.Interface;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Interfaces;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
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
        IStoreDal _storeRepository;
        public StockManager(IStockDal stockRepository, IStoreDal storeRepository)
        {
            _stockRepository = stockRepository;
            _storeRepository = storeRepository;
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
