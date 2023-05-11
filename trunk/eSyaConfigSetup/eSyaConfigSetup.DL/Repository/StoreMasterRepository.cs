using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eSyaConfigSetup.DL.DataLayer;
using eSyaConfigSetup.DL.Entities;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.DO.StaticVariables;
using eSyaConfigSetup.IF;
using Microsoft.EntityFrameworkCore;
namespace eSyaConfigSetup.DL.Repository
{
    public class StoreMasterRepository : IStoreMasterRepository
    {
        #region Store Master
        public async Task<List<DO_StoreMaster>> GetStoreCodes()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcstrm

                                  .Select(s => new DO_StoreMaster
                                  {
                                      StoreType = s.StoreType,
                                      StoreCode = s.StoreCode,
                                      StoreDesc = s.StoreDesc,
                                      //IsMaterial=s.IsMaterial,
                                      //IsPharmacy=s.IsPharmacy,
                                      //IsStationary=s.IsStationary,
                                      //IsCafeteria=s.IsCafeteria,
                                      //IsFandB=s.IsFandB,
                                      //IsCustodianStore=s.IsCustodianStore,
                                      //IsAccountingStore=s.IsAccountingStore,
                                      //IsConsumptionStore=s.IsConsumptionStore,
                                      ActiveStatus = s.ActiveStatus
                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_StoreMaster> GetStoreParameterList(int StoreCode)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var ds = db.GtEcstrm
                        .Where(w => w.StoreCode == StoreCode)
                        .Select(r => new DO_StoreMaster
                        {
                            l_FormParameter = r.GtEcpast.Select(p => new DO_eSyaParameter
                            {
                                ParameterID = p.ParameterId,
                                ParmAction = p.ParamAction
                            }).ToList()
                        }).FirstOrDefaultAsync();

                    return await ds;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateStoreCodes(DO_StoreMaster storecodes)
        {
            try
            {
                if (storecodes.StoreCode != 0)
                {
                    return await UpdateStoreCodes(storecodes);
                }
                else
                {
                    return await InsertStoreCodes(storecodes);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertStoreCodes(DO_StoreMaster storecodes)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcstrm isStorecodeExists = db.GtEcstrm.FirstOrDefault(c => c.StoreDesc.ToUpper().Replace(" ", "") == storecodes.StoreDesc.ToUpper().Replace(" ", ""));

                        if (isStorecodeExists == null)
                        {
                            int maxval = db.GtEcstrm.Select(c => c.StoreCode).DefaultIfEmpty().Max();
                            int storecode_ = maxval + 1;
                            var objstorecode = new GtEcstrm
                            {
                                StoreCode = storecode_,
                                StoreType = storecodes.StoreType,
                                StoreDesc = storecodes.StoreDesc,
                                FormId = storecodes.FormId,
                                ActiveStatus = storecodes.ActiveStatus,
                                CreatedBy = storecodes.UserID,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = storecodes.TerminalID
                            };
                            db.GtEcstrm.Add(objstorecode);

                            foreach (DO_eSyaParameter ip in storecodes.l_FormParameter)
                            {
                                var pMaster = new GtEcpast
                                {
                                    StoreCode = storecode_,
                                    ParameterId = ip.ParameterID,
                                    ParamAction = ip.ParmAction,
                                    ActiveStatus = ip.ActiveStatus,
                                    FormId = storecodes.FormId,
                                    CreatedBy = storecodes.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = storecodes.TerminalID,
                                };
                                db.GtEcpast.Add(pMaster);
                            }

                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Store is created Successfully." };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "This Store Description is already used by another Store Code." };
                        }
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

        public async Task<DO_ReturnParameter> UpdateStoreCodes(DO_StoreMaster storecodes)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcstrm isStorecodeExists = db.GtEcstrm.FirstOrDefault(c => c.StoreCode != storecodes.StoreCode && c.StoreDesc.ToUpper().Replace(" ", "") == storecodes.StoreDesc.ToUpper().Replace(" ", ""));
                        if (isStorecodeExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Store Description is already exist." };
                        }

                        GtEcstrm st_code = db.GtEcstrm.Where(s => s.StoreCode == storecodes.StoreCode).FirstOrDefault();
                        if (st_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Store Code does not exist" };
                        }

                        if (!storecodes.ActiveStatus)
                        {
                            GtEastbl st_bl = db.GtEastbl.Where(s => s.StoreCode == storecodes.StoreCode && s.ActiveStatus).FirstOrDefault();
                            if (st_bl != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Store already Liked with Business, can't Deactivate" };
                            }
                        }

                        st_code.StoreDesc = storecodes.StoreDesc;
                        st_code.ActiveStatus = storecodes.ActiveStatus;
                        st_code.ModifiedBy = storecodes.UserID;
                        st_code.ModifiedOn = DateTime.Now;
                        st_code.ModifiedTerminal = storecodes.TerminalID;

                        //if (storecodes.ActiveStatus == true)
                        //{
                        foreach (DO_eSyaParameter ip in storecodes.l_FormParameter)
                        {
                            GtEcpast sPar = db.GtEcpast.Where(x => x.StoreCode == storecodes.StoreCode && x.ParameterId == ip.ParameterID).FirstOrDefault();
                            if (sPar != null)
                            {
                                sPar.ParamAction = ip.ParmAction;
                                sPar.ActiveStatus = storecodes.ActiveStatus;
                                sPar.ModifiedBy = storecodes.UserID;
                                sPar.ModifiedOn = System.DateTime.Now;
                                sPar.ModifiedTerminal = storecodes.TerminalID;
                            }
                            else
                            {
                                var pMaster = new GtEcpast
                                {
                                    StoreCode = storecodes.StoreCode,
                                    ParameterId = ip.ParameterID,
                                    ParamAction = ip.ParmAction,
                                    ActiveStatus = ip.ActiveStatus,
                                    FormId = storecodes.FormId,
                                    CreatedBy = storecodes.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = storecodes.TerminalID,
                                };
                                db.GtEcpast.Add(pMaster);
                            }
                        }
                        //}
                        //else
                        //{
                        //    var fa = await db.GtEcpast.Where(x => x.StoreCode == storecodes.StoreCode).ToListAsync();

                        //    if (fa != null)
                        //        db.GtEcpast.RemoveRange(fa);
                        //}

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Store Updated Successfully." };
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

        public async Task<DO_ReturnParameter> DeleteStoreCode(int Storecode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcstrm store = db.GtEcstrm.FirstOrDefault(c => c.StoreCode == Storecode);

                        var paramstore = db.GtEcpast.Where(c => c.StoreCode == Storecode).ToList();
                        if (store != null)
                        {
                            db.GtEcpast.RemoveRange(paramstore);
                            db.GtEcstrm.Remove(store);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Store Code Deleted Successfully." };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Store Code Couldn't delete" };
                        }
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

        public async Task<List<DO_StoreMaster>> GetActiveStoreCodes()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcstrm.Where(x => x.ActiveStatus == true)

                                  .Select(s => new DO_StoreMaster
                                  {
                                      StoreCode = s.StoreCode,
                                      StoreDesc = s.StoreDesc
                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveStoreCode(bool status, string storetype, int storecode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcstrm strore_code = db.GtEcstrm.Where(w => w.StoreType.ToUpper().Replace(" ", "") == storetype.ToUpper().Replace(" ", "") && w.StoreCode==storecode).FirstOrDefault();
                        if (strore_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Store code is not exist" };
                        }

                        strore_code.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Store Code Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Store Code code De Activated Successfully." };
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
        #endregion  Store Master

        #region Store Business Link
        public async Task<List<DO_StoreMaster>> GetStoreList(int BusinessKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var sm = await db.GtEcstrm.Where(w => w.ActiveStatus == true)
                                     .Select(m => new DO_StoreMaster()
                                     {
                                         StoreCode = m.StoreCode,
                                         StoreDesc = m.StoreDesc
                                     }).ToListAsync();


                    foreach (var obj in sm)
                    {
                        GtEastbl getlocDesc = db.GtEastbl.Where(c => c.BusinessKey == BusinessKey && c.StoreCode == obj.StoreCode).FirstOrDefault();
                        if (getlocDesc != null)
                        {
                            obj.ActiveStatus = getlocDesc.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
                        }
                    }
                    return sm;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_StoreBusinessLink> GetStoreBusinessLinkInfo(int BusinessKey, int StoreCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEastbl
                        .Where(w => w.BusinessKey == BusinessKey && w.StoreCode == StoreCode)
                        .Select(r => new DO_StoreBusinessLink
                        {
                            IsAccounting = r.IsAccounting,
                            IsCustodian = r.IsCustodian,
                            IsConsumption = r.IsConsumption,
                            IsPointofSales = r.IsPointOfSales,
                            ActiveStatus = r.ActiveStatus
                        }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertUpdateStoreBusinessLink(DO_StoreBusinessLink obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        int isInsert = 0;
                        GtEastbl sPar = db.GtEastbl.Where(x => x.BusinessKey == obj.BusinessKey && x.StoreCode == obj.StoreCode).FirstOrDefault();
                        if (sPar != null)
                        {
                            sPar.IsAccounting = obj.IsAccounting;
                            sPar.IsCustodian = obj.IsCustodian;
                            sPar.IsConsumption = obj.IsConsumption;
                            sPar.IsPointOfSales = obj.IsPointofSales;
                            sPar.ActiveStatus = obj.ActiveStatus;
                            sPar.ModifiedBy = obj.UserID;
                            sPar.ModifiedOn = System.DateTime.Now;
                            sPar.ModifiedTerminal = obj.TerminalID;
                            isInsert = 0;
                        }
                        else
                        {
                            var strbusslnk = new GtEastbl
                            {
                                BusinessKey = obj.BusinessKey,
                                StoreCode = obj.StoreCode,
                                IsAccounting = obj.IsAccounting,
                                IsCustodian = obj.IsCustodian,
                                IsConsumption = obj.IsConsumption,
                                IsPointOfSales = obj.IsPointofSales,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID,
                            };
                            db.GtEastbl.Add(strbusslnk);
                            isInsert = 1;
                        }


                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (isInsert == 1)
                            return new DO_ReturnParameter() { Status = true, Message = "Store Busienss Link Created Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Store Busienss Link Updated Successfully." };
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

        #endregion  Store Business Link

        public async Task<List<DO_Forms>> GetFormForStorelinking()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcfmfd
                        .Join(db.GtEcfmps,
                            f => f.FormId,
                            p => p.FormId,
                            (f, p) => new { f, p })
                       .Where(w => w.f.ActiveStatus
                                  && w.p.ParameterId == ParameterIdValues.Form_isStoreLink
                                  && w.p.ActiveStatus)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.f.FormId,
                                      FormCode = r.f.FormCode,
                                      FormName = r.f.FormName
                                  }).OrderBy(o => o.FormCode).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_StoreMaster>> GetStoreFormLinked(int formId)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var st = db.GtEcstrm.Join(
                        db.GtEcpast.Where(w=>w.ParamAction && w.ActiveStatus),
                        s => s.StoreCode,
                        p => p.StoreCode,
                        (s, p) => new { s, p })
                        .Join(db.GtEcfmps.Where(w => w.FormId == formId),
                        sp => new { sp.p.ParameterId },
                        f => new { ParameterId = f.SubParameterId },
                        (sp, f) => new { sp, f })
                        .Select(r => new
                        {
                             r.sp.s.StoreType,
                             r.sp.s.StoreCode,
                              r.sp.s.StoreDesc,
                        });

                    var result = st
                        .GroupJoin(db.GtEcfmst.Where(w=>w.FormId == formId),
                            s => s.StoreCode,
                            f => f.StoreCode,
                            (s, f) => new { s, f = f.FirstOrDefault() }).DefaultIfEmpty()
                      .Select(s => new DO_StoreMaster
                      {
                          StoreType = s.s.StoreType,
                          StoreCode = s.s.StoreCode,
                          StoreDesc = s.s.StoreDesc,
                          ActiveStatus = s.f != null ? s.f.ActiveStatus : false
                      }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoFormStoreLink(List<DO_StoreFormLink> l_obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var fa = db.GtEcfmst.Where(w => w.FormId == l_obj.FirstOrDefault().FormId);
                        foreach (GtEcfmst f in fa)
                        {
                            f.ActiveStatus = false;
                            f.ModifiedBy = l_obj.FirstOrDefault().UserID;
                            f.ModifiedOn = System.DateTime.Now;
                            f.ModifiedTerminal = l_obj.FirstOrDefault().TerminalID;
                        }
                        await db.SaveChangesAsync();

                        foreach (DO_StoreFormLink s in l_obj)
                        {
                            var fs = db.GtEcfmst.Where(w => w.FormId == s.FormId && w.StoreCode == s.StoreCode).FirstOrDefault();
                            if (fs != null)
                            {
                                fs.ActiveStatus = true;
                                fs.ModifiedBy = s.UserID;
                                fs.ModifiedOn = DateTime.Now;
                                fs.ModifiedTerminal = System.Environment.MachineName;
                            }
                            else
                            {
                                fs = new GtEcfmst();
                                fs.FormId = s.FormId;
                                fs.StoreCode = s.StoreCode;
                                fs.ActiveStatus = true;
                                fs.CreatedBy = s.UserID;
                                fs.CreatedOn = DateTime.Now;
                                fs.CreatedTerminal = System.Environment.MachineName;
                                db.GtEcfmst.Add(fs);
                            }
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Created/Updated Successfully" };
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
