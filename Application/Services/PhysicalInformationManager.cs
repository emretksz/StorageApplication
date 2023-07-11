using Application.Interface;
using Core.Utilities.Results;
using DataAccess.Interfaces;
using Entities.Concrete;
using Entities.Dtos;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PhysicalInformationManager : IPhysicalInformationServices
    {
        IPhysicalInformationDal _physicalInformation;
        ITypeDal _type;
        IStateDal _state;
        public PhysicalInformationManager(IPhysicalInformationDal physicalInformation, IStateDal state, ITypeDal type)
        {
            _physicalInformation = physicalInformation;
            _state = state;
            _type = type;
        }



        public Task<IResult> Add(PhysicalInformation entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Delete(PhysicalInformation entity)
        {
            throw new NotImplementedException();
        }

        //public async Task<StateFunctionDto> FizikselBilgiler(List<Prediction> result)
        public async Task<StateFunctionDto> FizikselBilgiler(List<Prediction> result)
        {
            ///durumlar gelecek. 
            ///durumların tipleri olacak. bir durumun birden fazla tipi olabilir.
            ///tip'inde kendi içinde özellikleri var.
            ///

            //fiziki bilgilerin agırlıkları olacak.
            //

            List<StateFunctionDto> list = new List<StateFunctionDto>();
            foreach (var physical in result)
            {

                var splitValue = physical.Class.Split("-");
                var replaceValue = physical.Class.Replace("-", "").ToLower();
                if (Enums.PyhsicalWeight.yolotoyol.ToString() == physical.Class.Replace("-","").ToLower())
                {
                    var states = await _state.GetByFilterAsync(a => a.Name == "Yol");
                    var findState = await _physicalInformation.GetState(states);

                    if (splitValue.Count() > 1)
                    {
                        list.Add(findState.FirstOrDefault(a => a.Type.Name == splitValue[1].ToLower()));
                    }
                }
                else if (replaceValue.Contains("orman"))
                {
                   
                    if (splitValue.Count()>1)
                    {
                        var states = await _state.GetByFilterAsync(a => a.Name == "Orman");
                        var findState = await _physicalInformation.GetState(states);
                        list.Add(findState.FirstOrDefault(a => a.Type.Name == splitValue[1].ToLower()));
                    }
                    
                }
                else if (replaceValue.Contains("su"))
                {

                    if (splitValue.Count() > 1)
                    {
                        var states = await _state.GetByFilterAsync(a => a.Name == "Su");
                        var findState = await _physicalInformation.GetState(states);
                        list.Add(findState.FirstOrDefault(a => a.Type.Name == splitValue[1].ToLower()));
                    }

                }
                else if (replaceValue == "dag"|| replaceValue=="tepe"|| replaceValue=="bina"|| replaceValue=="gokyuzu"|| replaceValue=="insan")
                {
                        var states = await _state.GetByFilterAsync(a => a.Name == replaceValue);
                        var findState = await _physicalInformation.GetState(states);
                        list.Add(findState.FirstOrDefault(a => a.Type.Name == (replaceValue=="tepe"?"tepe":replaceValue=="dag"?"dag":"diger")));
                }

                else
                {
                    //kontrol eklenebilir
                    continue;
                }
            }



            //   Prediction lastState = new();
            StateFunctionDto lastState = new();
            var firstEx = list.Where(a => a != null).FirstOrDefault(a => a.State.Name == "Yol");
            if (firstEx != null)
            {
                //oranlara göre en iyi oran bulunur
                var oran = result.Where(a => a.Class.ToLower().Contains("yol")).OrderByDescending(a => a.Confidence).FirstOrDefault();
                //bulunan oran 100'lük değere çevirilir
                var xx = oran.Confidence.ToString().Substring(0, 4).Replace(",",".");
                firstEx.Type.Oran= (Convert.ToSingle(xx) * 1).ToString();

                //veritabanında oranı kaydetmek için type bulunur
                var findType= await _type.GetByFilterAsync(a => a.Name == firstEx.Type.Name && a.State.Id == firstEx.State.Id);
                //bulunan type'ın oranı güncellenir
                findType.Oran = firstEx.Type.Oran;
                await _type.UpdateAll(findType);

                //veri döndürülür
                return firstEx;
            }

            foreach (var item in list.Where(a => a.State.Name != "Yol"))
            {
                switch (item.State.Name)
                {
                    case "Orman"://2
                        var oran = result.Where(a => a.Class.ToLower().Contains("orman")).OrderByDescending(a => a.Confidence).FirstOrDefault();
                        //bulunan oran 100'lük değere çevirilir
                        var xx = oran.Confidence.ToString().Substring(0, 4).Replace(",", ".");

                        item.Type.Oran = (Convert.ToInt32(xx) * 1).ToString();

                        //veritabanında oranı kaydetmek için type bulunur
                        var findType = await _type.GetByFilterAsync(a => a.Name == item.Type.Name && a.State.Id == item.State.Id);
                        //bulunan type'ın oranı güncellenir
                        findType.Oran = item.Type.Oran;
                        await _type.UpdateAll(findType);


                        return item;

                    case "Dag"://3
                        var counter2 = list.Where(a => a.State.Name == "Orman");
                        if (counter2.Count() > 0)
                            break;
                        lastState = item;

                         oran = result.Where(a => a.Class.ToLower().Contains("dag")).OrderByDescending(a => a.Confidence).FirstOrDefault();
                        //bulunan oran 100'lük değere çevirilir
                         xx = oran.Confidence.ToString().Substring(0, 4).Replace(",", ".");
                        item.Type.Oran = (Convert.ToInt32(xx) * 1).ToString();

                        //veritabanında oranı kaydetmek için type bulunur
                        findType = await _type.GetByFilterAsync(a => a.Name == item.Type.Name && a.State.Id == item.State.Id);
                        //bulunan type'ın oranı güncellenir
                        findType.Oran = item.Type.Oran;
                        await _type.UpdateAll(findType);
                        break;

                    case "Tepe"://3
                        var counter4 = list.Where(a => a.State.Name == "Orman");
                        if (counter4.Count() > 0)
                            break;
                        lastState = item;

                        oran = result.Where(a => a.Class.ToLower().Contains("tepe")).OrderByDescending(a => a.Confidence).FirstOrDefault();
                        //bulunan oran 100'lük değere çevirilir
                        xx = oran.Confidence.ToString().Substring(0, 4).Replace(",", ".");
                        item.Type.Oran = (Convert.ToInt32(xx) * 1).ToString();

                        //veritabanında oranı kaydetmek için type bulunur
                        findType = await _type.GetByFilterAsync(a => a.Name == item.Type.Name && a.State.Id == item.State.Id);
                        //bulunan type'ın oranı güncellenir
                        findType.Oran = item.Type.Oran;
                        await _type.UpdateAll(findType);
                        break;

                    case "Su"://4
                        var counter3 = list.Where(a => a.State.Name == "Orman" || a.State.Name == "Dag" || a.State.Name == "Tepe");
                        if (counter3.Count() > 0)
                            break;
                        lastState = item;
                        oran = result.Where(a => a.Class.ToLower().Contains("su")).OrderByDescending(a => a.Confidence).FirstOrDefault();
                        //bulunan oran 100'lük değere çevirilir
                        xx = oran.Confidence.ToString().Substring(0, 4).Replace(",", ".");
                        item.Type.Oran = (Convert.ToInt32(xx) * 1).ToString();

                        //veritabanında oranı kaydetmek için type bulunur
                        findType = await _type.GetByFilterAsync(a => a.Name == item.Type.Name && a.State.Id == item.State.Id);
                        //bulunan type'ın oranı güncellenir
                        findType.Oran = item.Type.Oran;
                        await _type.UpdateAll(findType);
                        break;
                    case "Bina"://4
                    case "Gokyuzu"://4
                    case "insan"://4
                        continue;
                    default:
                        break;
                }

            }

            //var firstEx= result.Where(a=>a!=null).FirstOrDefault(a => a.Class == "yol-otoyol");
            //if (firstEx!=null)
            //{
            //    var QQ = firstEx.Confidence.ToString().Substring(0, 4);
            //    firstEx.Confidence = Convert.ToDouble(firstEx.Confidence.ToString().Substring(0,4),CultureInfo.InvariantCulture) * 100;
            //    return firstEx;
            //}

            //foreach (var item in result.Where(a=> a.Class!="yol-otoyol"))
            //{
            //    item.Confidence = Convert.ToDouble(item.Confidence.ToString().Substring(0, 4), CultureInfo.InvariantCulture) * 100;
            //    switch (item.Class)
            //    {
            //        case "orman-tarla"://2
            //            return item;

            //        case "dag"://3
            //            var counter2 = result.Where(a => a.Class== "orman-tarla");

            //            if (counter2.Count() > 0)
            //                break;
            //            lastState = item;
            //            break;

            //        case "su"://4
            //            var counter3 = result.Where(a => a.Class == "orman-tarla" || a.Class== "dag");
            //            if (counter3.Count() > 0)
            //                break;
            //            lastState = item;
            //            break;

            //        default:
            //            break;
            //    }

            //}


            return lastState;
        }

        public Task<IDataResults<List<PhysicalInformation>>> GetAll(Expression<Func<PhysicalInformation, bool>> filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDataResults<PhysicalInformation>> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> Update(PhysicalInformation entity)
        {
            throw new NotImplementedException();
        }

        public Task<IResult> UpdateAll(PhysicalInformation entity)
        {
            throw new NotImplementedException();
        }
    }
}
