﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using eSyaConfigSetup.DL.DataLayer;
using eSyaConfigSetup.DL.Entities;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
namespace eSyaConfigSetup.DL.Repository
{
   public class BusinessStructureRepository:IBusinessStructureRepository
    {
        #region Business Entity
        public async Task<List<DO_BusinessEntity>> GetBusinessEntities()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbsen
                                   .Where(w => w.ActiveStatus)
                                  .Select(be => new DO_BusinessEntity
                                  {
                                      BusinessId = be.BusinessId,
                                      BusinessDesc = be.BusinessId.ToString() + " - " + be.BusinessDesc,
                                      BusinessUnitType=be.BusinessUnitType,
                                      NoOfUnits=be.NoOfUnits,
                                      ActiveNoOfUnits=be.ActiveNoOfUnits,
                                      //IsMultiSegmentApplicable = be.IsMultiSegmentApplicable,
                                      UsageStatus = be.UsageStatus,
                                      ActiveStatus = be.ActiveStatus
                                  }).OrderBy(b => b.BusinessId).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_BusinessEntity> GetBusinessEntityInfo(int BusinessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcbsen
                        .Where(w => w.BusinessId == BusinessId)
                        .Select(r => new DO_BusinessEntity
                        {
                            BusinessId = r.BusinessId,
                            BusinessDesc = r.BusinessDesc,
                            //IsMultiSegmentApplicable = r.IsMultiSegmentApplicable,
                            BusinessUnitType = r.BusinessUnitType,
                            NoOfUnits = r.NoOfUnits,
                            ActiveNoOfUnits = r.ActiveNoOfUnits,
                            UsageStatus = r.UsageStatus,
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

        public async Task<DO_ReturnParameter> InsertBusinessEntity(DO_BusinessEntity businessentity)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsen is_EntityDescExists = db.GtEcbsen.FirstOrDefault(u => u.BusinessDesc.ToUpper().Replace(" ", "") == businessentity.BusinessDesc.ToUpper().Replace(" ", ""));
                        if (is_EntityDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Entity Description already Exists." };

                        }

                        int _businessID = db.GtEcbsen.Select(c => c.BusinessId).DefaultIfEmpty().Max();
                        _businessID = _businessID + 1;

                        var b_Entity = new GtEcbsen
                        {
                            BusinessId = _businessID,
                            BusinessDesc = businessentity.BusinessDesc,
                            //IsMultiSegmentApplicable = false,
                            BusinessUnitType=businessentity.BusinessUnitType,
                            NoOfUnits=businessentity.NoOfUnits,
                            ActiveNoOfUnits=businessentity.ActiveNoOfUnits,
                            UsageStatus = false,
                            ActiveStatus = businessentity.ActiveStatus,
                            FormId = businessentity.FormId,
                            CreatedBy = businessentity.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = businessentity.TerminalID
                        };
                        db.GtEcbsen.Add(b_Entity);
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Business Entity Created Successfully." };
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

        public async Task<DO_ReturnParameter> UpdateBusinessEntity(DO_BusinessEntity businessentity)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsen is_EntityExists = db.GtEcbsen.FirstOrDefault(be => be.BusinessDesc.ToUpper().Replace(" ", "") == businessentity.BusinessDesc.ToUpper().Replace(" ", "") && be.BusinessId != businessentity.BusinessId);
                        if (is_EntityExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Entity already Exists." };

                        }

                        if (!businessentity.ActiveStatus)
                        {
                            var b_seg = await db.GtEcbssg.Where(w => w.BusinessId == businessentity.BusinessId && w.ActiveStatus).ToListAsync();

                            if (b_seg != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Business Segment Active for selected Business Entity, you can't Deactivate Business Entity." };
                            }
                        }

                        GtEcbsen b_Entity = db.GtEcbsen.Where(be => be.BusinessId == businessentity.BusinessId).FirstOrDefault();
                        if (b_Entity != null)
                        {
                            b_Entity.BusinessDesc = businessentity.BusinessDesc;
                            //b_Entity.IsMultiSegmentApplicable = businessentity.IsMultiSegmentApplicable;
                            b_Entity.BusinessUnitType = businessentity.BusinessUnitType;
                            b_Entity.NoOfUnits = businessentity.NoOfUnits;
                            b_Entity.ActiveNoOfUnits = businessentity.ActiveNoOfUnits;
                            b_Entity.ActiveStatus = businessentity.ActiveStatus;
                            b_Entity.ModifiedBy = businessentity.UserID;
                            b_Entity.ModifiedOn = System.DateTime.Now;
                            b_Entity.ModifiedTerminal = businessentity.TerminalID;
                            await db.SaveChangesAsync();

                            //if (!businessentity.ActiveStatus)
                            //{
                            //    var b_seg = await db.GtEcbssg.Where(w => w.BusinessId == businessentity.BusinessId).ToListAsync();

                            //    foreach (GtEcbssg f in b_seg)
                            //    {
                            //        f.ActiveStatus = false;
                            //        f.ModifiedBy = businessentity.UserID;
                            //        f.ModifiedOn = System.DateTime.Now;
                            //        f.ModifiedTerminal = businessentity.TerminalID;
                            //    }
                            //    await db.SaveChangesAsync();

                            //    var b_loc = await db.GtEcbsln.Where(w => w.BusinessId == businessentity.BusinessId).ToListAsync();

                            //    foreach (GtEcbsln f in b_loc)
                            //    {
                            //        f.ActiveStatus = false;
                            //        f.ModifiedBy = businessentity.UserID;
                            //        f.ModifiedOn = System.DateTime.Now;
                            //        f.ModifiedTerminal = businessentity.TerminalID;
                            //    }
                                //await db.SaveChangesAsync();
                            //}

                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, Message = "Business Entity Updated Successfully." };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Entity not found." };

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

        public async Task<DO_ReturnParameter> DeleteBusinessEntity(int BusinessEntityId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsen bu_en = db.GtEcbsen.Where(w => w.BusinessId == BusinessEntityId).FirstOrDefault();
                        if (bu_en == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Busienss Entity is not exist" };
                        }

                        if (bu_en.UsageStatus == true)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Busienss Entity in used, Can't Delete" };
                        }

                        var b_seg = await db.GtEcbssg.Where(w => w.BusinessId == BusinessEntityId && w.ActiveStatus).ToListAsync();

                        if (b_seg != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Segment Active for selected Business Entity, you can't Delete Business Entity." };
                        }

                        db.GtEcbsen.Remove(bu_en);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Deleted Successfully." };
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

        public async Task<List<DO_BusinessEntity>> GetActiveBusinessEntities()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbsen.Where(x=>x.ActiveStatus==true)

                                  .Select(be => new DO_BusinessEntity
                                  {
                                      BusinessId = be.BusinessId,
                                      BusinessDesc = be.BusinessDesc
                                     
                                  }).OrderBy(b => b.BusinessId).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion  Business Entity

        #region Business Unit
        public async Task<List<DO_BusinessUnit>> GetBusinessUnitsbyBusinessId(int BusinessId)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                  
                    var result = db.GtEcbsun.Where(x => x.BusinessId == BusinessId)
                   .Join(db.GtEccuco, p => p.CurrencyCode.ToUpper().Trim(), pc => pc.CurrencyCode.ToUpper().Trim(), (p, pc) => new { p, pc })
                   .Join(db.GtEccncd, ppc => ppc.p.Isdcode, c => c.Isdcode, (ppc, c) => new { ppc, c })
                   .Join(db.GtEccnti, pppc => pppc.ppc.p.TaxIdentification, t => t.TaxIdentificationId,
                    (pppc, t) => new DO_BusinessUnit
                    {
                        BusinessId=pppc.ppc.p.BusinessId,
                        BusinessUnitId= pppc.ppc.p.BusinessId,
                        BusinessKey= pppc.ppc.p.BusinessKey,
                        UnitDesc = pppc.ppc.p.UnitDesc,
                        BusinessName = pppc.ppc.p.BusinessName,
                        Isdcode = pppc.ppc.p.Isdcode,
                        City = pppc.ppc.p.City,
                        Location = pppc.ppc.p.Location,
                        CurrencyCode = pppc.ppc.p.CurrencyCode,
                        IsBookOfAccounts = pppc.ppc.p.IsBookOfAccounts,
                        BusinessSubUnitId = pppc.ppc.p.BusinessSubUnitId,
                        BoacostCentre = pppc.ppc.p.BoacostCentre,
                        TaxIdentification = pppc.ppc.p.TaxIdentification,
                        TolocalCurrency = pppc.ppc.p.TolocalCurrency,
                        TocurrConversion = pppc.ppc.p.TocurrConversion,
                        TorealCurrency = pppc.ppc.p.TorealCurrency,
                        ActiveStatus = pppc.ppc.p.ActiveStatus,
                        CurrencyName=pppc.ppc.pc.CurrencyName,
                        CountryName=pppc.c.CountryName,
                        TaxIdentificationDesc=t.TaxIdentificationDesc
                    }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_BusinessUnit>> GetBusinessSubUnits(int BusinessId)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbsun.Where(x => x.IsBookOfAccounts == true &&x.BusinessId==BusinessId&& x.ActiveStatus==true)

                                  .Select(bu => new DO_BusinessUnit
                                  {
                                      BusinessKey = bu.BusinessKey,
                                      //UnitDesc = bu.UnitDesc
                                      BusinessName=bu.BusinessName
                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        public async Task<DO_ReturnParameter> InsertBusinessUnit(DO_BusinessUnit obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                       
                        GtEcbsun is_unitDescExists = db.GtEcbsun.FirstOrDefault(x=> x.UnitDesc.ToUpper().Replace(" ", "") == obj.UnitDesc.ToUpper().Replace(" ", "")
                         && x.BusinessId == obj.BusinessId);

                        if (is_unitDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Unit Description already Exists for selected Entity." };

                        }

                        //GtEcbsun is_businessExists = db.GtEcbsun.FirstOrDefault(x => x.BusinessName.ToUpper().Replace(" ", "") == obj.BusinessName.ToUpper().Replace(" ", "")
                        //&& x.BusinessId == obj.BusinessId);

                        //if (is_businessExists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "Business Name already Exists for selected Entity." };

                        //}

                        int _bunitID = db.GtEcbsun.Where(x=>x.BusinessId==obj.BusinessId).Select(c => c.BusinessUnitId).DefaultIfEmpty().Max();
                        _bunitID = _bunitID + 1;

                        int Business_Key = Convert.ToInt32(obj.BusinessId.ToString() + _bunitID.ToString());
                        var is_BusinessKeyExist = db.GtEcbsun.Where(x => x.ActiveStatus == true && x.BusinessKey == Business_Key).FirstOrDefault();
                        if (is_BusinessKeyExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Key already Exists for selected Entity." };
                        }
                        if (obj.BusinessSubUnitId == 0)
                        {
                            obj.BusinessSubUnitId = Business_Key;
                        }
                        var b_unit = new GtEcbsun
                        {
                            BusinessId = obj.BusinessId,
                            BusinessUnitId = _bunitID,
                            BusinessKey = Business_Key,
                            UnitDesc = obj.UnitDesc,
                            BusinessName = obj.BusinessName,
                            Isdcode = obj.Isdcode,
                            City = obj.City,
                            Location = obj.Location,
                            CurrencyCode=obj.CurrencyCode,
                            IsBookOfAccounts=obj.IsBookOfAccounts,
                            BusinessSubUnitId=obj.BusinessSubUnitId,
                            BoacostCentre=obj.BoacostCentre,
                            TaxIdentification = obj.TaxIdentification,
                            TolocalCurrency = obj.TolocalCurrency,
                            TocurrConversion = obj.TocurrConversion,
                            TorealCurrency = obj.TorealCurrency,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };

                        db.GtEcbsun.Add(b_unit);
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Business Unit Created Successfully." };

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

        public async Task<DO_ReturnParameter> UpdateBusinessUnit(DO_BusinessUnit obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        //GtEcbsun is_unitDescExists = db.GtEcbsun.FirstOrDefault(x => x.UnitDesc.ToUpper().Replace(" ", "") == obj.UnitDesc.ToUpper().Replace(" ", "") &&
                        //x.BusinessId == obj.BusinessId  && x.BusinessUnitId != obj.BusinessUnitId);
                        //if (is_unitDescExists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "Unit Description already Exists for selected Entity." };

                        //}

                        //GtEcbsun is_businessExists = db.GtEcbsun.FirstOrDefault(x => x.BusinessName.ToUpper().Replace(" ", "") == obj.BusinessName.ToUpper().Replace(" ", "")
                        //&& x.BusinessId == obj.BusinessId && x.BusinessUnitId != obj.BusinessUnitId);
                        //if (is_businessExists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "Business Name already Exists for selected Entity." };

                        //}

                        GtEcbsun b_unit = db.GtEcbsun.Where(bu => bu.BusinessId == obj.BusinessId && bu.BusinessUnitId == obj.BusinessUnitId ).FirstOrDefault();
                        if (obj.BusinessSubUnitId == 0)
                        {
                            obj.BusinessSubUnitId = obj.BusinessKey;
                        }

                        if (b_unit != null)
                        {
                            b_unit.BusinessKey = obj.BusinessKey;
                            b_unit.UnitDesc = obj.UnitDesc;
                            b_unit.BusinessName = obj.BusinessName;
                            b_unit.Isdcode = obj.Isdcode;
                            b_unit.City = obj.City;
                            b_unit.Location = obj.Location;
                            b_unit.CurrencyCode = obj.CurrencyCode;
                            b_unit.BusinessSubUnitId = obj.BusinessSubUnitId;
                            b_unit.BoacostCentre = obj.BoacostCentre;
                            b_unit.IsBookOfAccounts = obj.IsBookOfAccounts;
                            b_unit.TaxIdentification = obj.TaxIdentification;
                            b_unit.TolocalCurrency = obj.TolocalCurrency;
                            b_unit.TocurrConversion = obj.TocurrConversion;
                            b_unit.TorealCurrency = obj.TorealCurrency;
                            b_unit.ActiveStatus = obj.ActiveStatus;
                            b_unit.ModifiedBy = obj.UserID;
                            b_unit.ModifiedOn = System.DateTime.Now;
                            b_unit.ModifiedTerminal = obj.TerminalID;

                            await db.SaveChangesAsync();
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, Message = "Business Unit Updated Successfully." };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Unit not found." };

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveBusinessUnit(bool status, int BusinessId, int BusinessUnitId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsun b_unit = db.GtEcbsun.Where(w => w.BusinessId == BusinessId && w.BusinessUnitId==BusinessUnitId).FirstOrDefault();
                        if (b_unit == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Unit is not exist" };
                        }

                        b_unit.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Business Unit Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Business Unit De Activated Successfully." };
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
        #endregion Business Unit

        #region  Business Segment
        public async Task <List<DO_BusinessSegment>> GetBusinessSegmentByBusinessId(int BusinessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var segments = db.GtEcbssg.Where(x => x.BusinessId == BusinessId && x.ActiveStatus)
                        .Join(db.GtEccuco,
                         x => x.CurrencyCode,
                         y => y.CurrencyCode,
                        (x, y) => new DO_BusinessSegment
                        { 
                            BusinessId= x.BusinessId,
                            SegmentId= x.SegmentId,
                            SegmentDesc= x.SegmentDesc,
                            IsMultiLocationApplicable= x.IsMultiLocationApplicable,
                            Isdcode= x.Isdcode,
                            CurrencyCode= x.CurrencyCode,
                            CurrencyName=y.CurrencyName,
                            ActiveStatus = x.ActiveStatus
                        }).ToListAsync();

                    return await segments;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_BusinessConfiguration> GetBusinessSegment()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    DO_BusinessConfiguration mn = new DO_BusinessConfiguration();

                    mn.l_BusinessEntity = await db.GtEcbsen//.Where(w => w.ActiveStatus == true)
                                    .Select(m => new DO_BusinessEntity()
                                    {
                                        BusinessId = m.BusinessId,
                                        BusinessDesc = m.BusinessId.ToString() + " - " + m.BusinessDesc,
                                        ActiveStatus = m.ActiveStatus
                                    }).ToListAsync();

                    mn.l_BusinessSegment = await db.GtEcbssg//.Where(w => w.ActiveStatus == true)
                                    .Select(s => new DO_BusinessSeg()
                                    {
                                        BusinessId = s.BusinessId,
                                        SegmentId = s.SegmentId,
                                        SegmentDesc = s.SegmentId + " - " + s.SegmentDesc,
                                        ActiveStatus = s.ActiveStatus
                                    }).ToListAsync();

                    return mn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_BusinessSegment> GetBusinessSegmentInfo(int BusinessId, int SegmentId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var segments = db.GtEcbssg.Where(x => x.BusinessId == BusinessId && x.SegmentId == SegmentId)
                        .Join(db.GtEccuco,
                         x => x.CurrencyCode,
                         y => y.CurrencyCode,
                        (x, y) => new DO_BusinessSegment
                        {
                            BusinessId = x.BusinessId,
                            SegmentId = x.SegmentId,
                            SegmentDesc = x.SegmentDesc,
                            IsMultiLocationApplicable = x.IsMultiLocationApplicable,
                            Isdcode = x.Isdcode,
                            CurrencyCode = x.CurrencyCode,
                            CurrencyName = y.CurrencyName,
                            
                            //OrgnDateFormat = x.OrgnDateFormat,
                            
                            ActiveStatus = x.ActiveStatus
                        }).FirstOrDefaultAsync();

                    return await segments;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsAllowMultiSegment(int BusinessId)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var SS = await db.GtEcbsen.Where(x => x.BusinessId == BusinessId && x.IsMultiSegmentApplicable == false && x.ActiveStatus == true).CountAsync();

                    if (SS > 0)
                    {
                        var ds = await db.GtEcbssg
                            .Where(s => s.BusinessId == BusinessId && s.ActiveStatus)
                            .CountAsync();
                        if (ds > 0)
                            return false;
                        else
                            return true;
                    }
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<DO_ReturnParameter> InsertBusinessSegment(DO_BusinessSegment BusinessSegment)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsen is_besActive = db.GtEcbsen.FirstOrDefault(l => l.BusinessId == BusinessSegment.BusinessId && l.ActiveStatus == false);
                        if (is_besActive != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Entity is deactive, You Can't Create Business Segment" };

                        }

                        GtEcbsen Chk_EntityAllowedSingleSegment = db.GtEcbsen.Where(x => x.BusinessId == BusinessSegment.BusinessId && x.IsMultiSegmentApplicable == false && x.ActiveStatus == true).FirstOrDefault();

                        if (Chk_EntityAllowedSingleSegment != null)
                        {
                            GtEcbssg Is_SiglementExistsinoneEntity = db.GtEcbssg.FirstOrDefault(s => s.BusinessId == Chk_EntityAllowedSingleSegment.BusinessId);
                            if (Is_SiglementExistsinoneEntity != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Selected Entity Allowed only one Segment." };
                            }
                        }

                        GtEcbssg is_SegmentDescExists = db.GtEcbssg.FirstOrDefault(s => s.SegmentDesc.ToUpper().Replace(" ", "") == BusinessSegment.SegmentDesc.ToUpper().Replace(" ", "")
                        && s.BusinessId == BusinessSegment.BusinessId);

                        if (is_SegmentDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Segment Description already Exists for selected Entity." };

                        }

                        int _segmentID = db.GtEcbssg.Select(c => c.SegmentId).DefaultIfEmpty().Max();
                        _segmentID = _segmentID + 1;

                        var b_Segment = new GtEcbssg
                        {
                            BusinessId = BusinessSegment.BusinessId,
                            SegmentId = _segmentID,
                            SegmentDesc = BusinessSegment.SegmentDesc,
                            IsMultiLocationApplicable = BusinessSegment.IsMultiLocationApplicable,
                            Isdcode = BusinessSegment.Isdcode,
                            CurrencyCode = BusinessSegment.CurrencyCode,
                            //OrgnDateFormat = BusinessSegment.OrgnDateFormat,
                            ActiveStatus = BusinessSegment.ActiveStatus,
                            FormId = BusinessSegment.FormID,
                            CreatedBy = BusinessSegment.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = BusinessSegment.TerminalID
                        };
                        db.GtEcbssg.Add(b_Segment);
                        await db.SaveChangesAsync();
                        
                        //Usage status true for the Business Entity
                        GtEcbsen b_Entity = db.GtEcbsen.Where(be => be.BusinessId == BusinessSegment.BusinessId).FirstOrDefault();
                        if (b_Entity != null)
                        {
                            b_Entity.UsageStatus = true;
                            b_Entity.ModifiedBy = BusinessSegment.UserID;
                            b_Entity.ModifiedOn = System.DateTime.Now;
                            b_Entity.ModifiedTerminal = BusinessSegment.TerminalID;
                            await db.SaveChangesAsync();
                        }

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Business Segment Created Successfully." };

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

        public async Task<DO_ReturnParameter> UpdateBusinessSegment(DO_BusinessSegment BusinessSegment)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbssg is_SegmentExists = db.GtEcbssg.FirstOrDefault(bs => bs.SegmentDesc.ToUpper().Replace(" ", "") == BusinessSegment.SegmentDesc.ToUpper().Replace(" ", "") &&
                        bs.BusinessId == BusinessSegment.BusinessId && bs.SegmentId != BusinessSegment.SegmentId);
                        if (is_SegmentExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Segment Description already Exists." };

                        }

                        GtEcbsen is_besActive = db.GtEcbsen.FirstOrDefault(l => l.BusinessId == BusinessSegment.BusinessId && l.ActiveStatus == false);
                        if (is_besActive != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Entity is deactive, You Can't active Business Segment" };

                        }

                        if (!BusinessSegment.ActiveStatus)
                        {
                            var b_loc = await db.GtEcbsln.Where(w => w.BusinessId == BusinessSegment.BusinessId && w.ActiveStatus).ToListAsync();

                            if (b_loc != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Business Location Active for selected Business Segment, you can't Deactivate Business Segment." };
                            }
                        }
                        
                        GtEcbssg b_Segment = db.GtEcbssg.Where(bs => bs.SegmentId == BusinessSegment.SegmentId && bs.BusinessId == BusinessSegment.BusinessId).FirstOrDefault();
                        if (b_Segment != null)
                        {
                            b_Segment.SegmentDesc = BusinessSegment.SegmentDesc;
                            b_Segment.IsMultiLocationApplicable = BusinessSegment.IsMultiLocationApplicable;
                            b_Segment.Isdcode = BusinessSegment.Isdcode;
                            b_Segment.CurrencyCode = BusinessSegment.CurrencyCode;
                            //b_Segment.OrgnDateFormat = BusinessSegment.OrgnDateFormat;
                            b_Segment.ActiveStatus = BusinessSegment.ActiveStatus;
                            b_Segment.ModifiedBy = BusinessSegment.UserID;
                            b_Segment.ModifiedOn = System.DateTime.Now;
                            b_Segment.ModifiedTerminal = BusinessSegment.TerminalID;
                            await db.SaveChangesAsync();
                            
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, Message = "Business Segment Updated Successfully." };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Segment not found." };

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

        public async Task<DO_ReturnParameter> DeleteBusinessSegment(int BusinessId, int SegmentId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbssg bu_sg = db.GtEcbssg.Where(w => w.BusinessId == BusinessId && w.SegmentId == SegmentId).FirstOrDefault();
                        if (bu_sg == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Busienss Segment is not exist" };
                        }
                        
                        var b_loc = await db.GtEcbsln.Where(w => w.BusinessId == BusinessId && w.SegmentId == SegmentId && w.ActiveStatus).ToListAsync();

                        if (b_loc != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Location Active for selected Business Segment, you can't Delete Business Segment." };
                        }
                        
                        db.GtEcbssg.Remove(bu_sg);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Deleted Successfully." };
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

        public async Task <List<DO_BusinessSegment>> GetActiveBusinessSegmentbyBusinessId(int BusinessId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var segments = db.GtEcbssg.Where(x => x.BusinessId == BusinessId && x.ActiveStatus==true)
                        .Select(s => new DO_BusinessSegment
                        {
                            SegmentId = s.SegmentId,
                            SegmentDesc = s.SegmentDesc
                            
                        }).ToListAsync();

                    return await segments;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Business Segment

        #region Business Location
        public async Task <List<DO_BusinessLocation>> GetBusinessLocationByBusinessIdandSegmentId(int BusinessId, int SegmentId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var locations = db.GtEcbsln.Where(x=> x.BusinessId == BusinessId &&x.SegmentId==SegmentId)
                        .Select(l => new DO_BusinessLocation
                        {
                            BusinessId = l.BusinessId,
                            SegmentId = l.SegmentId,
                            LocationId = l.LocationId,
                            LocationCode = l.LocationCode,
                            LocationDescription = l.LocationDescription,
                            BusinessKey=l.BusinessKey,
                            TaxIdentification = l.TaxIdentification,
                            ActiveStatus = l.ActiveStatus
                        }).ToListAsync();

                    return await locations;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_BusinessConfiguration> GetBusinessLocations()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    DO_BusinessConfiguration mn = new DO_BusinessConfiguration();

                    mn.l_BusinessEntity = await db.GtEcbsen//.Where(w => w.ActiveStatus == true)
                                    .Select(m => new DO_BusinessEntity()
                                    {
                                        BusinessId = m.BusinessId,
                                        BusinessDesc = m.BusinessId.ToString() + " - "+ m.BusinessDesc,
                                        ActiveStatus = m.ActiveStatus
                                    }).ToListAsync();

                    mn.l_BusinessSegment = await db.GtEcbssg//.Where(w => w.IsMultiLocationApplicable == true)
                                    .Select(s => new DO_BusinessSeg()
                                    {
                                        BusinessId = s.BusinessId,
                                        SegmentId = s.SegmentId,
                                        SegmentDesc = s.SegmentId.ToString() + " - " + s.SegmentDesc,
                                        ActiveStatus = s.ActiveStatus
                                    }).ToListAsync();

                    mn.l_BusinessLocation = await db.GtEcbsln//.Where(w => w.ActiveStatus == true)
                                    .Select(s => new DO_BusinessLoc()
                                    {
                                        BusinessId = s.BusinessId,
                                        SegmentId = s.SegmentId,
                                        LocationId = s.LocationId,
                                        LocationDescription = s.BusinessKey.ToString() + " - " + s.LocationDescription,
                                        ActiveStatus = s.ActiveStatus
                                    }).ToListAsync();

                    return mn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_BusinessLocation> GetBusinessLocationInfo(int BusinessId, int SegmentId, int LocationId)
        {
            try
            {
                string bKey = BusinessId.ToString() + SegmentId.ToString() + LocationId.ToString();
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcbsln
                        .Where(w => w.BusinessKey == Convert.ToInt32(bKey))
                        .Select(r => new DO_BusinessLocation
                        {
                            BusinessId = r.BusinessId,
                            SegmentId = r.SegmentId,
                            LocationId = r.LocationId,
                            LocationCode = r.LocationCode,
                            LocationDescription = r.LocationDescription,
                            BusinessName = r.BusinessName,
                            TaxIdentification = r.TaxIdentification,
                            LicenseType = r.ESyaLicenseType,
                            UserLicenses = Convert.ToInt32(Encoding.UTF8.GetString(r.EUserLicenses)),
                            NoOfBeds = Convert.ToInt32(r.ENoOfBeds != null ? Encoding.UTF8.GetString(r.ENoOfBeds):"0"),
                            ToLocalCurrency = r.TolocalCurrency,
                            ToCurrCurrency = r.TocurrConversion,
                            ToRealCurrency = r.TorealCurrency,
                            ActiveStatus = r.ActiveStatus,
                            l_FormParameter = r.GtEcpabl.Select(p => new DO_eSyaParameter
                            {
                                ParameterID = p.ParameterId,
                                ParmAction = p.ParmAction
                            }).ToList()
                        }).FirstOrDefaultAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> IsAllowMultiLocation(int BusinessId, int SegmentId)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                //    GtEcbssg Chk_SegmentAllowedSingleLocation = db.GtEcbssg.Where(x => x.BusinessId == BusinessId && x.SegmentId == SegmentId && x.IsMultiLocationApplicable == false && x.ActiveStatus == true).FirstOrDefault();

                //    if (Chk_SegmentAllowedSingleLocation != null)
                //    {
                //        GtEcbsln IsLocationExistsinoneSegment = db.GtEcbsln.FirstOrDefault(s => s.SegmentId == Chk_SegmentAllowedSingleLocation.SegmentId);
                //        if (IsLocationExistsinoneSegment != null)
                //        {
                //            return new DO_ReturnParameter() { Status = false, Message = "Selected Entity and Segment Allowed only one Location." };
                //        }
                //    }


                    var SS = await db.GtEcbssg.Where(x => x.BusinessId == BusinessId && x.SegmentId == SegmentId && x.IsMultiLocationApplicable == false && x.ActiveStatus == true).CountAsync();

                    if (SS > 0)
                    {
                        var ds = await db.GtEcbsln
                            .Where(s => s.BusinessId == BusinessId && s.SegmentId == SegmentId && s.ActiveStatus)
                            .CountAsync();
                        if (ds > 0)
                            return false;
                        else
                            return true;
                    }
                    else
                        return true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task <DO_ReturnParameter> InsertBusinessLocation(DO_BusinessLocation location)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbssg is_besActive = db.GtEcbssg.FirstOrDefault(l => l.BusinessId == location.BusinessId && l.SegmentId == location.SegmentId && l.ActiveStatus == false);
                        if (is_besActive != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Segment is deactive, You Can't Create Business Location" };

                        }

                        GtEcbssg Chk_SegmentAllowedSingleLocation = db.GtEcbssg.Where(x => x.SegmentId == location.SegmentId && x.IsMultiLocationApplicable == false && x.ActiveStatus == true).FirstOrDefault();

                        if (Chk_SegmentAllowedSingleLocation != null)
                        {
                            GtEcbsln IsLocationExistsinoneSegment = db.GtEcbsln.FirstOrDefault(s => s.SegmentId == Chk_SegmentAllowedSingleLocation.SegmentId);
                            if (IsLocationExistsinoneSegment != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Selected Entity and Segment Allowed only one Location." };
                            }
                        }

                        GtEcbsln is_LocationCodeExists = db.GtEcbsln.FirstOrDefault(l => l.LocationCode.ToUpper().Replace(" ", "") == location.LocationCode.ToUpper().Replace(" ", "")
                         && l.BusinessId == location.BusinessId && l.SegmentId == location.SegmentId);

                        if (is_LocationCodeExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Location Code already Exists for selected Entity and Segment." };

                        }

                        GtEcbsln is_locDescExists = db.GtEcbsln.FirstOrDefault(l => l.LocationDescription.ToUpper().Replace(" ", "") == location.LocationDescription.ToUpper().Replace(" ", "")
                        && l.BusinessId == location.BusinessId && l.SegmentId == location.SegmentId);

                        if (is_locDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Location Description already Exists for selected Entity and Segment." };

                        }
                        
                        int _locationID = db.GtEcbsln.Select(c => c.LocationId).DefaultIfEmpty().Max();
                        _locationID = _locationID + 1;

                        int Business_Key = Convert.ToInt32(location.BusinessId.ToString() + location.SegmentId.ToString() + _locationID.ToString());
                        var is_BusinessKeyExist = db.GtEcbsln.Where(x => x.ActiveStatus == true && x.BusinessKey == Business_Key).FirstOrDefault();
                        if (is_BusinessKeyExist != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Key already Exists for selected Entity and Segment and Location ID." };
                        }

                        byte[] eBKey = Encoding.UTF8.GetBytes(location.BusinessKey.ToString());

                        byte[] tbUserLicenses = Encoding.UTF8.GetBytes(location.UserLicenses.ToString());
                        byte[] tbActiveUser = Encoding.UTF8.GetBytes(0.ToString());
                        byte[] tbNoOfBeds = Encoding.UTF8.GetBytes(location.NoOfBeds.ToString());

                        var b_Location = new GtEcbsln
                        {
                            BusinessId = location.BusinessId,
                            SegmentId = location.SegmentId,
                            LocationId = _locationID,
                            LocationCode = location.LocationCode,
                            LocationDescription = location.LocationDescription,
                            BusinessName = location.BusinessName,
                            BusinessKey = Business_Key,
                            EBusinessKey = eBKey,
                            TaxIdentification = location.TaxIdentification,
                            ESyaLicenseType = location.LicenseType,
                            TolocalCurrency = location.ToLocalCurrency,
                            TocurrConversion = location.ToCurrCurrency,
                            TorealCurrency = location.ToRealCurrency,
                            EUserLicenses = tbUserLicenses,
                            EActiveUsers = tbActiveUser,
                            ENoOfBeds = tbNoOfBeds,
                            ActiveStatus = location.ActiveStatus,
                            FormId = location.FormID,
                            CreatedBy = location.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = location.TerminalID
                        };

                        db.GtEcbsln.Add(b_Location);
                        await db.SaveChangesAsync();

                        if (location.ActiveStatus == true && (location.ToCurrCurrency || location.ToRealCurrency))
                        {
                            var fa = await db.GtEcbssc.Where(w => w.BusinessKey == Business_Key).ToListAsync();

                            foreach (GtEcbssc f in fa)
                            {
                                f.ActiveStatus = false;
                                f.ModifiedBy = location.UserID;
                                f.ModifiedOn = System.DateTime.Now;
                                f.ModifiedTerminal = location.TerminalID;
                            }
                            await db.SaveChangesAsync();

                            if (location.l_BSCurrency != null)
                            {
                                foreach (DO_BusienssSegmentCurrency i in location.l_BSCurrency)
                                {
                                    var obj_FA = await db.GtEcbssc.Where(w => w.BusinessKey == Business_Key && w.TocurrencyCode == i.CurrencyCode).FirstOrDefaultAsync();
                                    if (obj_FA != null)
                                    {
                                        if (i.ActiveStatus)
                                            obj_FA.ActiveStatus = true;
                                        else
                                            obj_FA.ActiveStatus = false;
                                        obj_FA.ModifiedBy = location.UserID;
                                        obj_FA.ModifiedOn = DateTime.Now;
                                        obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                    }
                                    else
                                    {
                                        obj_FA = new GtEcbssc();
                                        obj_FA.BusinessKey = Business_Key;
                                        obj_FA.TocurrencyCode = i.CurrencyCode;
                                        if (i.ActiveStatus)
                                            obj_FA.ActiveStatus = true;
                                        else
                                            obj_FA.ActiveStatus = false;
                                        obj_FA.FormId = location.FormID;
                                        obj_FA.CreatedBy = location.UserID;
                                        obj_FA.CreatedOn = DateTime.Now;
                                        obj_FA.CreatedTerminal = System.Environment.MachineName;
                                        db.GtEcbssc.Add(obj_FA);
                                    }
                                }
                                await db.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var fa = await db.GtEcbssc.Where(w => w.BusinessKey == Business_Key).ToListAsync();

                            foreach (GtEcbssc f in fa)
                            {
                                f.ActiveStatus = false;
                                f.ModifiedBy = location.UserID;
                                f.ModifiedOn = System.DateTime.Now;
                                f.ModifiedTerminal = location.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }

                        if (location.ActiveStatus == true)
                        {
                            var fa = await db.GtEcpabl.Where(w => w.BusinessKey == Business_Key).ToListAsync();

                            foreach (GtEcpabl f in fa)
                            {
                                f.ParmAction = false;
                                f.ActiveStatus = false;
                                f.ModifiedBy = location.UserID;
                                f.ModifiedOn = System.DateTime.Now;
                                f.ModifiedTerminal = location.TerminalID;
                            }
                            await db.SaveChangesAsync();

                            if (location.l_FormParameter != null)
                            {
                                foreach (DO_eSyaParameter i in location.l_FormParameter)
                                {
                                    var obj_FA = await db.GtEcpabl.Where(w =>  w.BusinessKey == Business_Key && w.ParameterId == i.ParameterID).FirstOrDefaultAsync();
                                    if (obj_FA != null)
                                    {
                                        obj_FA.ParmAction = i.ParmAction;
                                        obj_FA.ActiveStatus = true;
                                        obj_FA.ModifiedBy = location.UserID;
                                        obj_FA.ModifiedOn = DateTime.Now;
                                        obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                    }
                                    else
                                    {
                                        obj_FA = new GtEcpabl();
                                        obj_FA.BusinessKey = Business_Key;
                                        obj_FA.ParameterId = i.ParameterID;
                                        obj_FA.ParmPerc = 0;
                                        obj_FA.ParmAction = i.ParmAction;
                                        obj_FA.ParmDesc = null;
                                        obj_FA.ParmValue = 0;
                                        obj_FA.ActiveStatus = location.ActiveStatus;
                                        obj_FA.FormId = location.FormID;
                                        obj_FA.CreatedBy = location.UserID;
                                        obj_FA.CreatedOn = System.DateTime.Now;
                                        obj_FA.CreatedTerminal = location.TerminalID;

                                        db.GtEcpabl.Add(obj_FA);
                                    }
                                }
                                await db.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            var fa = await db.GtEcpabl.Where(w => w.BusinessKey == Business_Key).ToListAsync();

                            foreach (GtEcpabl f in fa)
                            {
                                f.ParmAction = false;
                                f.ActiveStatus = false;
                                f.ModifiedBy = location.UserID;
                                f.ModifiedOn = System.DateTime.Now;
                                f.ModifiedTerminal = location.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Business Location Created Successfully." };

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

        public async Task<DO_ReturnParameter> UpdateBusinessLocation(DO_BusinessLocation location)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsln is_locDescExists = db.GtEcbsln.FirstOrDefault(l => l.LocationDescription.ToUpper().Replace(" ", "") == location.LocationDescription.ToUpper().Replace(" ", "") &&
                        l.BusinessId == location.BusinessId && l.SegmentId == location.SegmentId && l.LocationId != location.LocationId);
                        if (is_locDescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Location already Exists for selected Entity and Segment." };

                        }

                        GtEcbsln is_LocCodeExists = db.GtEcbsln.FirstOrDefault(l => l.LocationCode.ToUpper().Replace(" ", "") == location.LocationCode.ToUpper().Replace(" ", "")
                        && l.BusinessId == location.BusinessId && l.SegmentId == location.SegmentId && l.LocationId != location.LocationId);
                        if (is_LocCodeExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Location Code already Exists for selected Entity and Segment." };

                        }

                        GtEcbssg is_besActive = db.GtEcbssg.FirstOrDefault(l => l.BusinessId == location.BusinessId && l.SegmentId == location.SegmentId && l.ActiveStatus == false);
                        if (is_besActive != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Segment is deactive, You Can't active Business Location" };

                        }

                        byte[] tbUserLicenses = Encoding.UTF8.GetBytes(location.UserLicenses.ToString());
                        byte[] tbNoOfBeds = Encoding.UTF8.GetBytes(location.NoOfBeds.ToString());

                        GtEcbsln b_loc = db.GtEcbsln.Where(bl => bl.BusinessId == location.BusinessId && bl.SegmentId == location.SegmentId  && bl.LocationId==location.LocationId).FirstOrDefault();
                        if (b_loc != null)
                        {
                            b_loc.LocationCode = location.LocationCode;
                            b_loc.LocationDescription = location.LocationDescription;
                            b_loc.BusinessName = location.BusinessName;
                            b_loc.TaxIdentification = location.TaxIdentification;
                            b_loc.ESyaLicenseType = location.LicenseType;
                            b_loc.EUserLicenses = tbUserLicenses;
                            b_loc.ENoOfBeds = tbNoOfBeds;
                            b_loc.TolocalCurrency = location.ToLocalCurrency;
                            b_loc.TocurrConversion = location.ToCurrCurrency;
                            b_loc.TorealCurrency = location.ToRealCurrency;
                            b_loc.ActiveStatus = location.ActiveStatus;
                            b_loc.ModifiedBy = location.UserID;
                            b_loc.ModifiedOn = System.DateTime.Now;
                            b_loc.ModifiedTerminal = location.TerminalID;

                            await db.SaveChangesAsync();

                            if (location.ActiveStatus == true && (location.ToCurrCurrency || location.ToRealCurrency))
                            {
                                var sc = await db.GtEcbssc.Where(w => w.BusinessKey == b_loc.BusinessKey).ToListAsync();

                                foreach (GtEcbssc f in sc)
                                {
                                    f.ActiveStatus = false;
                                    f.ModifiedBy = location.UserID;
                                    f.ModifiedOn = System.DateTime.Now;
                                    f.ModifiedTerminal = location.TerminalID;
                                }
                                await db.SaveChangesAsync();

                                if (location.l_BSCurrency != null)
                                {
                                    foreach (DO_BusienssSegmentCurrency i in location.l_BSCurrency)
                                    {
                                        var obj_FA = await db.GtEcbssc.Where(w => w.BusinessKey == b_loc.BusinessKey && w.TocurrencyCode == i.CurrencyCode).FirstOrDefaultAsync();
                                        if (obj_FA != null)
                                        {
                                            if (i.ActiveStatus)
                                                obj_FA.ActiveStatus = true;
                                            else
                                                obj_FA.ActiveStatus = false;
                                            obj_FA.ModifiedBy = location.UserID;
                                            obj_FA.ModifiedOn = DateTime.Now;
                                            obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                        }
                                        else
                                        {
                                            obj_FA = new GtEcbssc();
                                            obj_FA.BusinessKey = b_loc.BusinessKey;
                                            obj_FA.TocurrencyCode = i.CurrencyCode;
                                            if (i.ActiveStatus)
                                                obj_FA.ActiveStatus = true;
                                            else
                                                obj_FA.ActiveStatus = false;
                                            obj_FA.FormId = location.FormID;
                                            obj_FA.CreatedBy = location.UserID;
                                            obj_FA.CreatedOn = DateTime.Now;
                                            obj_FA.CreatedTerminal = System.Environment.MachineName;
                                            db.GtEcbssc.Add(obj_FA);
                                        }
                                    }
                                    await db.SaveChangesAsync();
                                }
                            }
                            else
                            {
                                var bsCurrency = db.GtEcbssc.Where(w => w.BusinessKey == b_loc.BusinessKey).ToList();

                                if (bsCurrency != null)
                                    db.GtEcbssc.RemoveRange(bsCurrency);
                                await db.SaveChangesAsync();
                            }

                            var fa = await db.GtEcpabl.Where(w => w.BusinessKey == b_loc.BusinessKey).ToListAsync();

                                foreach (GtEcpabl f in fa)
                                {
                                    f.ParmAction = false;
                                    f.ActiveStatus = false;
                                    f.ModifiedBy = location.UserID;
                                    f.ModifiedOn = System.DateTime.Now;
                                    f.ModifiedTerminal = location.TerminalID;
                                }
                                await db.SaveChangesAsync();

                                if (location.l_FormParameter != null)
                                {
                                    foreach (DO_eSyaParameter i in location.l_FormParameter)
                                    {
                                        var obj_FA = await db.GtEcpabl.Where(w => w.BusinessKey == b_loc.BusinessKey && w.ParameterId == i.ParameterID).FirstOrDefaultAsync();
                                        if (obj_FA != null)
                                        {
                                            obj_FA.ParmAction = i.ParmAction;
                                            obj_FA.ActiveStatus = true;
                                            obj_FA.ModifiedBy = location.UserID;
                                            obj_FA.ModifiedOn = DateTime.Now;
                                            obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                        }
                                        else
                                        {
                                            obj_FA = new GtEcpabl();
                                            obj_FA.BusinessKey = b_loc.BusinessKey;
                                            obj_FA.ParameterId = i.ParameterID;
                                            obj_FA.ParmPerc = 0;
                                            obj_FA.ParmAction = i.ParmAction;
                                            obj_FA.ParmDesc = null;
                                            obj_FA.ParmValue = 0;
                                            obj_FA.ActiveStatus = location.ActiveStatus;
                                            obj_FA.FormId = location.FormID;
                                            obj_FA.CreatedBy = location.UserID;
                                            obj_FA.CreatedOn = System.DateTime.Now;
                                            obj_FA.CreatedTerminal = location.TerminalID;

                                            db.GtEcpabl.Add(obj_FA);
                                        }
                                    }
                                    await db.SaveChangesAsync();
                                }
                           
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, Message = "Business Location Updated Successfully." };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Business Location not found." };

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

        public async Task<DO_ReturnParameter> DeleteBusinessLocation(int BusinessId, int SegmentId, int LocationId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcbsln bu_sg = db.GtEcbsln.Where(w => w.BusinessId == BusinessId && w.SegmentId == SegmentId && w.LocationId == LocationId).FirstOrDefault();
                        if (bu_sg == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Busienss Location is not exist" };
                        }

                        string bKey = BusinessId.ToString() + SegmentId.ToString() + LocationId.ToString();

                        var bsCurrency = db.GtEcbssc.Where(w => w.BusinessKey == Convert.ToUInt32(bKey)).ToList();

                        if (bsCurrency != null)
                            db.GtEcbssc.RemoveRange(bsCurrency);

                        db.GtEcbsln.Remove(bu_sg);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Deleted Successfully." };
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

        public async Task<List<DO_BusinessLocation>> GetBusinessKey()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbsln.Where(x=>x.ActiveStatus==true)

                                  .Select(be => new DO_BusinessLocation
                                  {
                                      BusinessKey = be.BusinessKey,
                                      //LocationDescription = be.LocationDescription
                                      //LocationDescription = r.BusinessName + "-" + r.LocationDescription
                                      LocationDescription = be.BusinessName

                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_BusienssSegmentCurrency>> GetBSCurrency(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEccuco
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_BusienssSegmentCurrency
                        {
                            CurrencyCode = r.CurrencyCode,
                            CurrencyName = r.CurrencyName,
                            ActiveStatus = false
                        }).ToListAsync();

                    foreach (var obj in ds)
                    {
                        GtEcbssc sbCurrency = db.GtEcbssc.Where(x => x.BusinessKey == BusinessKey && x.TocurrencyCode == obj.CurrencyCode).FirstOrDefault();
                        if (sbCurrency != null)
                        {
                            obj.ActiveStatus = sbCurrency.ActiveStatus;
                        }
                        else
                        {
                            obj.ActiveStatus = false;
                        }
                    }

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion Business Location

        #region Business Stores
        //public async Task <DO_ReturnParameter> InsertOrUpdateBusinessStores(DO_BusinessStores store)
        //{
        //    using (var db = new eSyaEnterprise())
        //    {
        //        using (var dbContext = db.Database.BeginTransaction())
        //        {
        //            try
        //            {

        //                GtCmbssc bs_store = db.GtCmbssc.Where(x => x.BusinessKey == store.BusinessKey && x.StoreCode == store.StoreCode).FirstOrDefault();
        //                if (bs_store == null)
        //                {
        //                    var B_Store = new GtCmbssc
        //                    {
        //                        BusinessKey=store.BusinessKey,
        //                        StoreCode=store.StoreCode,
        //                        ActiveStatus = store.ActiveStatus,
        //                        CreatedBy = store.UserID,
        //                        CreatedOn = System.DateTime.Now,
        //                        CreatedTerminal = store.TerminalID
        //                    };
        //                    db.GtCmbssc.Add(B_Store);
        //                   await db.SaveChangesAsync();
        //                    dbContext.Commit();
        //                    return new DO_ReturnParameter() { Status = true, Message = "Business Store Created Successfully." };
        //                }
        //                else
        //                {
        //                    bs_store.ActiveStatus = store.ActiveStatus;
        //                    bs_store.ModifiedBy = store.UserID;
        //                    bs_store.ModifiedOn = System.DateTime.Now;
        //                    bs_store.ModifiedTerminal = store.TerminalID;
        //                    await db.SaveChangesAsync();
        //                    dbContext.Commit();
        //                    return new DO_ReturnParameter() { Status = true, Message = "Business Store Updated Successfully." };
        //                }
        //            }
        //            catch (DbUpdateException ex)
        //            {
        //                dbContext.Rollback();
        //                throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
        //            }
        //            catch (Exception ex)
        //            {
        //                dbContext.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }
        //}

        //public async Task<List<DO_BusinessStores>> GetBusinessStores(int Businesskey)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var b_stores = db.GtCmbssc.Where(x=>x.BusinessKey==Businesskey)

        //            .Select(bs => new DO_BusinessStores
        //            {
        //                BusinessKey=bs.BusinessKey,
        //                StoreCode=bs.StoreCode,
        //                ActiveStatus=bs.ActiveStatus

        //            }).ToListAsync();

        //            return await b_stores;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #endregion Business Stores

        #region Business Statutory Details

        //public async Task <DO_ReturnParameter> InsertOrUpdateBusinessStatutoryDetails(DO_BusinessStatutoryDetails statutorydetails)
        //{
        //    using (var db = new eSyaEnterprise())
        //    {
        //        using (var dbContext = db.Database.BeginTransaction())
        //        {
        //            try
        //            {

        //                GtCmbssd BS_details = db.GtCmbssd.Where(x => x.BusinessKey == statutorydetails.BusinessKey && x.StatutoryCode == statutorydetails.StatutoryCode).FirstOrDefault();
        //                if (BS_details == null)
        //                {
        //                    var statutory_details = new GtCmbssd
        //                    {
        //                        BusinessKey= statutorydetails.BusinessKey,
        //                        StatutoryCode=statutorydetails.StatutoryCode,
        //                        StatutoryDetail=statutorydetails.StatutoryDetail,
        //                        ActiveStatus = statutorydetails.ActiveStatus,
        //                        CreatedBy = statutorydetails.UserID,
        //                        CreatedOn = System.DateTime.Now,
        //                        CreatedTerminal = statutorydetails.TerminalID
        //                    };
        //                    db.GtCmbssd.Add(statutory_details);
        //                   await db.SaveChangesAsync();
        //                    dbContext.Commit();
        //                    return new DO_ReturnParameter() { Status = true, Message = "Business Statutory Details Created Successfully." };
        //                }
        //                else
        //                {
        //                    BS_details.StatutoryDetail = statutorydetails.StatutoryDetail;
        //                    BS_details.ActiveStatus = statutorydetails.ActiveStatus;
        //                    BS_details.ModifiedBy = statutorydetails.UserID;
        //                    BS_details.ModifiedOn = System.DateTime.Now;
        //                    BS_details.ModifiedTerminal = statutorydetails.TerminalID;
        //                   await db.SaveChangesAsync();
        //                    dbContext.Commit();
        //                    return new DO_ReturnParameter() { Status = true, Message = "Business Statutory Details Updated Successfully." };
        //                }
        //            }
        //            catch (DbUpdateException ex)
        //            {
        //                dbContext.Rollback();
        //                throw new Exception(CommonMethod.GetValidationMessageFromException(ex));
        //            }
        //            catch (Exception ex)
        //            {
        //                dbContext.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }
        //}

        //public async Task <List<DO_BusinessStatutoryDetails>> GetBusinessStatutoryDetails(int BusinessKey)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var statutorydetails = db.GtCmbssd.Where(x=>x.BusinessKey== BusinessKey)

        //            .Select(sd => new DO_BusinessStatutoryDetails
        //            {
        //                BusinessKey = sd.BusinessKey,
        //                StatutoryCode=sd.StatutoryCode,
        //                StatutoryDetail=sd.StatutoryDetail,
        //                ActiveStatus = sd.ActiveStatus

        //            }).ToListAsync();

        //            return await statutorydetails;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion Business Statutory Details

        #region Business Subscription
        public async Task<List<DO_BusinessSubscription>> GetBusinessSubscription(int BusinessKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcbssu.Where(bs=> bs.BusinessKey == BusinessKey)
                                  .Select(be => new DO_BusinessSubscription
                                  {
                                      SubscribedFrom = be.SubscribedFrom,
                                      SubscribedTill = be.SubscribedTill,
                                      ActiveStatus = be.ActiveStatus
                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateBusinessSubscription(DO_BusinessSubscription businessSubscription)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (businessSubscription.isEdit == 0)
                        {
                            GtEcbssu is_SubsFrmDateExist = db.GtEcbssu.FirstOrDefault(be => be.BusinessKey == businessSubscription.BusinessKey && be.SubscribedFrom >= businessSubscription.SubscribedFrom);
                            if (is_SubsFrmDateExist != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Subscribed From already Exists." };

                            }

                            GtEcbssu is_SubstoDateExist = db.GtEcbssu.FirstOrDefault(be => be.BusinessKey == businessSubscription.BusinessKey && be.SubscribedTill >= businessSubscription.SubscribedTill);
                            if (is_SubstoDateExist != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Subscribed Till already Exists." };

                            }

                            var is_SubsCheck = db.GtEcbssu.FirstOrDefault(be => be.BusinessKey == businessSubscription.BusinessKey && (be.SubscribedTill >= businessSubscription.SubscribedFrom || businessSubscription.SubscribedTill >= businessSubscription.SubscribedFrom));
                            if (is_SubsCheck != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Check Subscribed From Date" };

                            }
                        }
                        GtEcbssu b_Susbs = db.GtEcbssu.Where(be => be.BusinessKey == businessSubscription.BusinessKey && be.SubscribedFrom == businessSubscription.SubscribedFrom).FirstOrDefault();
                        if (b_Susbs != null)
                        {
                            b_Susbs.SubscribedTill = businessSubscription.SubscribedTill;
                            b_Susbs.ModifiedBy = businessSubscription.UserID;
                            b_Susbs.ModifiedOn = System.DateTime.Now;
                            b_Susbs.ModifiedTerminal = businessSubscription.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Business Subscription Updated Successfully." };
                        }
                        else
                        {
                            var b_Subs = new GtEcbssu
                            {
                                BusinessKey = businessSubscription.BusinessKey,
                                SubscribedFrom = businessSubscription.SubscribedFrom,
                                SubscribedTill = businessSubscription.SubscribedTill,
                                ActiveStatus = businessSubscription.ActiveStatus,
                                FormId = businessSubscription.FormID,
                                CreatedBy = businessSubscription.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = businessSubscription.TerminalID
                            };

                            db.GtEcbssu.Add(b_Subs);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Business Subscription Created Successfully." };
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

        #endregion  Business Subscription

        #region Business Statutory
        
        public async Task<List<DO_BusinessStatutoryDetails>> GetStatutoryInformation(int BusinessKey, int isdCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccnsd
                        .Join(db.GtEcsupa.Where(w => w.ParameterId == 1),
                            x => new { x.Isdcode, x.StatutoryCode },
                        y => new { y.Isdcode, y.StatutoryCode },
                           (x, y) => new { x, y })
                        .GroupJoin(db.GtEcbssd.Where(w => w.BusinessKey == BusinessKey),
                         xy => xy.x.StatutoryCode,
                         c => c.StatutoryCode,
                         (xy, c) => new { xy, c = c.FirstOrDefault() }).DefaultIfEmpty()
                        .Where(w => w.xy.x.Isdcode == isdCode && (bool)w.xy.x.ActiveStatus)
                        .Select(r => new DO_BusinessStatutoryDetails
                        {
                            BusinessKey = BusinessKey,
                            StatutoryCode = r.xy.x.StatutoryCode,
                            StatutoryDescription = r.xy.x.StatutoryDescription,
                            StatutoryValue = r.c != null ? r.c.StatutoryDescription : "",
                            ActiveStatus = r.c != null ? r.c.ActiveStatus : r.xy.x.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateBusinessStatutory(List<DO_BusinessStatutoryDetails> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_StatutoryDetailEnter = obj.Where(w => !String.IsNullOrEmpty(w.StatutoryValue)).Count();
                        if (is_StatutoryDetailEnter <= 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Please Enter Statutory Detail." };
                        }

                        foreach (var sd in obj.Where(w => !String.IsNullOrEmpty(w.StatutoryValue)))
                        {
                            GtEcbssd cs_sd = db.GtEcbssd.Where(x => x.BusinessKey == sd.BusinessKey
                                            && x.StatutoryCode == sd.StatutoryCode).FirstOrDefault();
                            if (cs_sd == null)
                            {
                                var o_cssd = new GtEcbssd
                                {
                                    BusinessKey = sd.BusinessKey,
                                    StatutoryCode = sd.StatutoryCode,
                                    StatutoryDescription = sd.StatutoryValue,
                                    ActiveStatus = sd.ActiveStatus,
                                    FormId = sd.FormID,
                                    CreatedBy = sd.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = sd.TerminalID
                                };
                                db.GtEcbssd.Add(o_cssd);
                            }
                            else
                            {
                                cs_sd.StatutoryDescription = sd.StatutoryValue;
                                cs_sd.ActiveStatus = sd.ActiveStatus;
                                cs_sd.ModifiedBy = sd.UserID;
                                cs_sd.ModifiedOn = System.DateTime.Now;
                                cs_sd.ModifiedTerminal = sd.TerminalID;
                            }
                            await db.SaveChangesAsync();
                        }

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Saved Successfully." };
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
        #endregion Business Statutory
    }
}
