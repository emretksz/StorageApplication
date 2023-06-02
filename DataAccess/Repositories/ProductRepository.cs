using Core.DataAccess.EntityFramework.Repository;
using DataAccess.EntityFramework;
using DataAccess.Interfaces;
using Entities.Concrete;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class ProductRepository : Repository<Product, StorageApplicationContext>, IProductDal
    {
      
        
        private string connectionString = @"server=.\;Database=StorageApplicationContext;Trusted_Connection=true";
        public async Task<List<ProductAndAmountDto>> GetProductAndAmount(List<ProductAndAmountDto> productAndAmountDtos)
        {
            List<ProductAndAmountDto> urunListesi = new List<ProductAndAmountDto>();
        
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ProductId", typeof(int));
                dataTable.Columns.Add("Amount", typeof(int));

                foreach (var item in productAndAmountDtos.Where(a=>a.UrunId!=0))
                {
                    dataTable.Rows.Add(item.UrunId, item.Miktar);
                }

                // SqlConnection ve SqlCommand oluşturma
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SqlCommand'u oluşturma
                    SqlCommand command = new SqlCommand("GetProductAndAmound", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    // SqlParameter oluşturma ve DataTable parametresini eklemek
                    SqlParameter parameter = new SqlParameter();
                    parameter.ParameterName = "@ProductAmounts";
                    parameter.SqlDbType = SqlDbType.Structured;
                    parameter.Value = dataTable;
                    command.Parameters.Add(parameter);

                    // Sonuçları okuma
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int productId = (int)reader["ProductId"];
                            int amount = (int)reader["Amount"];
                             string Konum = (string)reader["Konum"];
                            string StoreName = (string)reader["StoreName"];
                            string urunAdi = (string)reader["UrunAdi"];
                            int depoId = (int)reader["DepoId"];

                            urunListesi.Add(new ProductAndAmountDto() {UrunId=productId,Miktar=amount,Konum=Konum,DepoAdi=StoreName,UrunAdi=urunAdi,DepoId=depoId});
                            // Okunan verileri kullanma işlemleri yapılır
                        }
                    }
                }

            return urunListesi;
        }
    }
}
