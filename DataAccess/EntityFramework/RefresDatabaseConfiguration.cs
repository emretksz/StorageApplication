using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DataAccess.EntityFramework.RefresDatabaseConfiguration;

namespace DataAccess.EntityFramework
{
    public class RefresDatabaseConfiguration : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Belirli bir saatte güncelleme işlemleri
                if (DateTime.Now.Hour == 8) ///saaat 8 olduğunda db sıfırla
                {
                    try
                    {
                        using (StorageApplicationContext db = new StorageApplicationContext())
                        {
                            var productList = await db.Products.ToListAsync();
                            var storeList = await db.Stores.ToListAsync();
                            var stock = await db.Stocks.Include(a => a.Store).ToListAsync();

                            foreach (var storeItem in storeList)
                            {

                                foreach (var item in stock.Where(a => a.Store.Id == storeItem.Id))
                                {
                                    item.StockAmount = 300;
                                    db.Update(item);
                                    await db.SaveChangesAsync();

                                }

                            }
                        }
                    }
                    catch
                    {


                    }
                    // Veritabanı güncelleme işlemleri burada gerçekleştirilir
                }
                await Task.Delay(TimeSpan.FromHours(4), stoppingToken); // 4 saat bekle tekrar kontrol et
            }
        }
    }
}
