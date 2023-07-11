using Application.Interface;
using Business.Constants;
using Core.Utilities.Results;
using DataAccess.Interfaces;
using DataAccess.Repositories;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductManager : IProductServices
    {
        IProductDal _productDal;

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }

       
        public async Task<IResult> Add(Product entity)
        {
            if (entity!=null)
            {
                await _productDal.CreateAsync(entity);
                return new SuccessResult(ConstMessages.ProductAdded);
            }
            return new ErrorResult(ConstMessages.ProductError);
        }

        public Task<IResult> Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        public  async Task<IDataResults<List<Product>>> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            try
            {
                return new SuccessDataResult<List<Product>>(await _productDal.GetAllAsync());
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List< Product >>();
            }
        }

        public async Task<IDataResults<Product>> GetById(int id)
        {
            try
            {
                return new SuccessDataResult<Product>((await _productDal.GetByFilterAsync(a => a.Id == id)));
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Product>();

            }
        }

        public async Task<List<ProductAndAmountDto>> GetProductAndAmount(List<ProductAndAmountDto> productAndAmountDtos)
        {
            return await _productDal.GetProductAndAmount(productAndAmountDtos);
        }
        public async Task<List<ProductAndAmountDto>> GetProductAndAmountForBestStores(List<ProductAndAmountDto> productAndAmountDtos)
        {
            return await _productDal.GetProductAndAmountForBestStores(productAndAmountDtos);
        }

        //kullanma
        public async Task<IResult> Update(Product entity)
        {
            if (entity != null)
            {
                await _productDal.UpdateAll(entity);
                return new SuccessResult(ConstMessages.ProductUpdate);
            }
            return new ErrorResult(ConstMessages.ProductError);
        }

        public async Task<IResult> UpdateAll(Product entity)
        {
            if (entity != null)
            {
                await _productDal.UpdateAll(entity);
                return new SuccessResult(ConstMessages.ProductUpdate);
            }
            return new ErrorResult(ConstMessages.ProductError);
        }

        public async Task<ProductAmountAndLocation> FindBestCordinat( List<ProductAndAmountDto> productAndAmount)
        {
            List<Coordinate> coordinates = new List<Coordinate>();// Veritabanından koordinatları alır
            List<ProductAndAmountDto> resultProductAndLocation = new List<ProductAndAmountDto>();// Veritabanından koordinatları alır
            foreach (var product in productAndAmount)
            {
                var xy = product.Konum.Split(",");
                coordinates.Add(new() { Latitude = Convert.ToDouble(xy[0], CultureInfo.InvariantCulture), Longitude = Convert.ToDouble(xy[1], CultureInfo.InvariantCulture) });
                product.Latitude = Convert.ToDouble(xy[0], CultureInfo.InvariantCulture);
                product.Longitude = Convert.ToDouble(xy[1], CultureInfo.InvariantCulture);
                resultProductAndLocation.Add(product);
            }
            ProductAmountAndLocation model = new ProductAmountAndLocation()
            {
                ProductAndAmount = resultProductAndLocation,
                Cordinate = coordinates
            };
            return model; //tüm kordinatlar ve ürünler 
        }
       
    }
}
