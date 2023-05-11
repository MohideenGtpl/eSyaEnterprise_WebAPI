using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSyaConfigSetup.DL.DataLayer;
using eSyaConfigSetup.DL.Entities;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.EntityFrameworkCore;
namespace eSyaConfigSetup.DL.Repository
{
   public class UnitofMeasureRepository:IUnitofMeasureRepository
    {
        public async Task<List<DO_UnitofMeasure>> GetUnitofMeasurements()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEciuom

                                  .Select(u => new DO_UnitofMeasure
                                  {
                                    UnitOfMeasure=u.UnitOfMeasure,
                                    Uompurchase=u.Uompurchase,
                                    Uomstock=u.Uomstock,
                                    Uompdesc=u.Uompdesc,
                                    Uomsdesc=u.Uomsdesc,
                                    ConversionFactor=u.ConversionFactor,
                                    ActiveStatus = u.ActiveStatus
                                  }).OrderBy(o => o.UnitOfMeasure).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateUnitofMeasurement(DO_UnitofMeasure uoms)
        {
            try
            {
                if (uoms.UnitOfMeasure != 0)
                {
                    return await UpdateUnitofMeasurement(uoms);
                }
                else
                {
                    return await InsertUnitofMeasurement(uoms);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertUnitofMeasurement(DO_UnitofMeasure uoms)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEciuom isUompExists = db.GtEciuom.FirstOrDefault(u => u.Uompurchase.ToUpper().Replace(" ", "") == uoms.Uompurchase.ToUpper().Replace(" ", "") &&
                        u.Uomstock.ToUpper().Replace(" ", "") == uoms.Uomstock.ToUpper().Replace(" ", ""));
                        if (isUompExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "The combination of Unit of Measurement Purchase  and Unit of Measurement Stock already Exists." };

                        }
                        //GtEciuom isUomsExists = db.GtEciuom.FirstOrDefault(u => u.Uomstock.ToUpper().Replace(" ", "") == uoms.Uomstock.ToUpper().Replace(" ", ""));
                        //if (isUomsExists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "Unit of Measurement Stock already Exists.." };

                        //}
                        //GtEciuom isUompdescExists = db.GtEciuom.FirstOrDefault(u=> u.Uompdesc.ToUpper().Replace(" ", "") == uoms.Uompdesc.ToUpper().Replace(" ", ""));
                        //if (isUompdescExists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "UOMP Description already Exists." };

                        //}
                        //GtEciuom isUomsdescExists = db.GtEciuom.FirstOrDefault(u => u.Uomsdesc.ToUpper().Replace(" ", "") == uoms.Uomsdesc.ToUpper().Replace(" ", ""));
                        //if (isUomsdescExists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "UOMS Description already Exists." };

                        //}
                       
                            int maxval = db.GtEciuom.Select(c => c.UnitOfMeasure).DefaultIfEmpty().Max();
                            int uomId = maxval + 1;
                            var objuom = new GtEciuom
                            {
                                 UnitOfMeasure=uomId,
                                 Uompurchase=uoms.Uompurchase,
                                 Uomstock=uoms.Uomstock,
                                 Uompdesc= uoms.Uompdesc,
                                 Uomsdesc=uoms.Uomsdesc,
                                 ConversionFactor=uoms.ConversionFactor,
                                 ActiveStatus=uoms.ActiveStatus,
                                 FormId=uoms.FormId,
                                 CreatedBy=uoms.UserID,
                                 CreatedOn=DateTime.Now,
                                 CreatedTerminal=uoms.TerminalID
                            };
                            db.GtEciuom.Add(objuom);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Unit Measurement created Successfully." };
                        
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_ReturnParameter> UpdateUnitofMeasurement(DO_UnitofMeasure uoms)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEciuom isUompExists =  db.GtEciuom.FirstOrDefault(u => u.Uompurchase.ToUpper().Replace(" ", "") == uoms.Uompurchase.ToUpper().Replace(" ", "") &&
                        u.Uomstock.ToUpper().Replace(" ", "") == uoms.Uomstock.ToUpper().Replace(" ", "") && u.UnitOfMeasure!=uoms.UnitOfMeasure);
                        if (isUompExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "The combination of Unit of Measurement Purchase and Unit of Measurement Stock already Exists." };

                        }
                        //GtEciuom isUomsExists =  db.GtEciuom.FirstOrDefault(u => u.Uomstock.ToUpper().Replace(" ", "") == uoms.Uomstock.ToUpper().Replace(" ", "") && u.UnitOfMeasure != uoms.UnitOfMeasure);
                        //if (isUomsExists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "Unit of Measurement Stock already Exists.." };

                        //}
                        //GtEciuom isUompdescExists =  db.GtEciuom.FirstOrDefault(u => u.Uompdesc.ToUpper().Replace(" ", "") == uoms.Uompdesc.ToUpper().Replace(" ", "") && u.UnitOfMeasure != uoms.UnitOfMeasure);
                        //if (isUompdescExists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "UOMP Description already Exists." };

                        //}
                        //GtEciuom isUomsdescExists =  db.GtEciuom.FirstOrDefault(u => u.Uomsdesc.ToUpper().Replace(" ", "") == uoms.Uomsdesc.ToUpper().Replace(" ", "") && u.UnitOfMeasure != uoms.UnitOfMeasure);
                        //if (isUomsdescExists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "UOMS Description already Exists." };

                        //}
                        GtEciuom objuoms = db.GtEciuom.Where(x => x.UnitOfMeasure == uoms.UnitOfMeasure).FirstOrDefault();

                        objuoms.Uompurchase = uoms.Uompurchase;
                        objuoms.Uomstock = uoms.Uomstock;
                        objuoms.Uompdesc = uoms.Uompdesc;
                        objuoms.Uomsdesc = uoms.Uomsdesc;
                        objuoms.ConversionFactor = uoms.ConversionFactor;
                        objuoms.ActiveStatus = uoms.ActiveStatus;
                        objuoms.ModifiedBy = uoms.UserID;
                        objuoms.ModifiedOn = DateTime.Now;
                        objuoms.ModifiedTerminal = uoms.TerminalID;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Unit Measurement Updated Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_UnitofMeasure> GetUOMPDescriptionbyUOMP(string uomp)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    
                    var result = db.GtEciuom.Where(u => u.Uompurchase.ToUpper().Replace(" ", "") == uomp.ToUpper().Replace(" ", "")).Select(x => new DO_UnitofMeasure
                    {
                       Uompdesc = x.Uompdesc
                    }).FirstOrDefaultAsync();

                                  
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_UnitofMeasure> GetUOMSDescriptionbyUOMS(string uoms)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {

                    var result = db.GtEciuom.Where(u => u.Uomstock.ToUpper().Replace(" ", "") == uoms.ToUpper().Replace(" ", "")).Select(x => new DO_UnitofMeasure
                    {
                        Uomsdesc = x.Uomsdesc
                    }).FirstOrDefaultAsync();


                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveUnitofMeasure(bool status, int unitId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEciuom unit_mesure = db.GtEciuom.Where(w => w.UnitOfMeasure == unitId).FirstOrDefault();
                        if (unit_mesure == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Unit of Measure is not exist" };
                        }

                        unit_mesure.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Unit of Measure Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Unit of Measure code De Activated Successfully." };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw new Exception(CommonMethod.GetValidationMessageFromException(ex));

                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }
    }
}
