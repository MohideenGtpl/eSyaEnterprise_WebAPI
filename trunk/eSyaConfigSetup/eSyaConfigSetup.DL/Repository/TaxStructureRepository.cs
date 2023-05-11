﻿using System;
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
    public class TaxStructureRepository:ITaxStructureRepository
    {
        public async Task<List<DO_TaxStructure>> GetTaxStructureByISDCode(int ISDCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccntc
                        .Where(w => w.Isdcode == ISDCode)
                        .Select(r => new DO_TaxStructure
                        {
                            ISDCode = r.Isdcode,
                            TaxCode = r.TaxCode,
                            TaxShortCode = r.TaxShortCode,
                            TaxDescription = r.TaxDescription,
                            SlabOrPerc = (r.SlabOrPerc == "P" ? "Percentage" : "Slab"),
                            IsSplitApplicable = r.IsSplitApplicable,
                            ActiveStatus = r.ActiveStatus
                        }).OrderBy(o => o.TaxDescription).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_TaxStructure>> GetTaxStructureByTaxCode(int ISDCode, int taxCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccntc
                        .Where(w => w.Isdcode == ISDCode && w.TaxCode == taxCode)
                        .Select(r => new DO_TaxStructure
                        {
                            TaxCode = r.TaxCode,
                            TaxShortCode = r.TaxShortCode,
                            TaxDescription = r.TaxDescription,
                            SlabOrPerc = (r.SlabOrPerc == "P" ? "Percentage" : "Slab"),
                            IsSplitApplicable = r.IsSplitApplicable,
                        }).OrderBy(o => o.TaxDescription).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //InsertOrUpdateTaxStructure

        public async Task<DO_ReturnParameter> InsertOrUpdateTaxStructure(DO_TaxStructure obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccntc tx_cd = db.GtEccntc.Where(w => w.Isdcode == obj.ISDCode && w.TaxCode == obj.TaxCode).FirstOrDefault();

                        if (obj.SaveStatus == true && tx_cd != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Tax Code is already Exists" };
                        }
                        
                        if (tx_cd != null)
                        {
                            IEnumerable<GtEccntc> ls_apct = db.GtEccntc.Where(w => w.Isdcode == obj.ISDCode).ToList();

                            var is_SameShortCodeExists = ls_apct.Where(w => w.TaxShortCode.ToUpper().Replace(" ", "") == obj.TaxShortCode.ToUpper().Replace(" ", "")
                                && w.TaxCode != obj.TaxCode).Count();
                            if (is_SameShortCodeExists > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Tax short Code is already Exists" };
                            }

                            tx_cd.TaxShortCode = obj.TaxShortCode.Trim();
                            tx_cd.TaxDescription = obj.TaxDescription.Trim();
                            tx_cd.SlabOrPerc = obj.SlabOrPerc.Substring(0, 1);
                            tx_cd.IsSplitApplicable = obj.IsSplitApplicable;
                            tx_cd.ActiveStatus = obj.ActiveStatus;
                            tx_cd.ModifiedBy = obj.UserID;
                            tx_cd.ModifiedOn = System.DateTime.Now;
                            tx_cd.ModifiedTerminal = obj.TerminalID;

                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Tax Structure Updated Successfully." };
                        }
                        else
                        {
                            var _isTaxShortCodeExist = db.GtEccntc.Where(w => w.Isdcode == obj.ISDCode && w.TaxShortCode == obj.TaxShortCode).Count();
                            if (_isTaxShortCodeExist > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Tax Short Code is already Exists For Selected ISD Code" };
                            }

                            var _isTaxDescriptionExist = db.GtEccntc.Where(w => w.Isdcode == obj.ISDCode && w.TaxDescription == obj.TaxDescription).Count();
                            if (_isTaxDescriptionExist > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Tax Description is already Exists For Selected ISD Code" };
                            }

                            var tax_cd = new GtEccntc
                            {
                                Isdcode = obj.ISDCode,
                                TaxCode = obj.TaxCode,
                                TaxShortCode = obj.TaxShortCode.Trim(),
                                TaxDescription = obj.TaxDescription.Trim(),
                                SlabOrPerc = obj.SlabOrPerc.Substring(0, 1),
                                IsSplitApplicable = obj.IsSplitApplicable,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormId,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID,

                            };

                            db.GtEccntc.Add(tax_cd);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Tax Code Created Successfully." };
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

        //public async Task<DO_ReturnParameter> InsertIntoTaxStructure(DO_TaxStructure obj)
        //{
        //    using (var db = new eSyaEnterprise())
        //    {
        //        using (var dbContext = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var _isTaxShortCodeExist = db.GtEccntc.Where(w => w.Isdcode == obj.ISDCode && w.TaxShortCode == obj.TaxShortCode).Count();
        //                if (_isTaxShortCodeExist > 0)
        //                {
        //                    return new DO_ReturnParameter() { Status = false, Message = "Tax Short Code is already Exists For Selected ISD Code" };
        //                }

        //                var _isTaxDescriptionExist = db.GtEccntc.Where(w => w.Isdcode == obj.ISDCode && w.TaxDescription == obj.TaxDescription).Count();
        //                if (_isTaxDescriptionExist > 0)
        //                {
        //                    return new DO_ReturnParameter() { Status = false, Message = "Tax Description is already Exists For Selected ISD Code" };
        //                }

        //                int _taxCode = 0;
        //                var _isTaxCodeExist = db.GtEccntc.Where(w => w.Isdcode != obj.ISDCode && w.TaxShortCode == obj.TaxShortCode && w.TaxDescription == obj.TaxDescription).Count();
        //                if (_isTaxCodeExist > 0)
        //                {
        //                    _taxCode = db.GtEccntc.Where(w => w.Isdcode != obj.ISDCode && w.TaxShortCode == obj.TaxShortCode && w.TaxDescription == obj.TaxDescription).Select(c => c.TaxCode).DefaultIfEmpty().Max();
        //                }

        //                else
        //                {
        //                    int maxTaxCode = db.GtEccntc.Select(c => c.TaxCode).DefaultIfEmpty().Max();
        //                    _taxCode = maxTaxCode + 1;
        //                }

        //                var ap_cd = new GtEccntc
        //                {
        //                    Isdcode = obj.ISDCode,
        //                    TaxCode = _taxCode,
        //                    TaxShortCode = obj.TaxShortCode.Trim(),
        //                    TaxDescription = obj.TaxDescription.Trim(),
        //                    SlabOrPerc = obj.SlabOrPerc.Substring(0, 1),
        //                    IsSplitApplicable = obj.IsSplitApplicable,
        //                    ActiveStatus = obj.ActiveStatus,
        //                    FormId = obj.FormId,
        //                    CreatedBy = obj.UserID,
        //                    CreatedOn = System.DateTime.Now,
        //                    CreatedTerminal = obj.TerminalID,

        //                };
        //                db.GtEccntc.Add(ap_cd);
                        
        //               await db.SaveChangesAsync();
        //                dbContext.Commit();
        //                return new DO_ReturnParameter() { Status = true, Message = "Tax Code Created Successfully." };
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

        //public async Task<DO_ReturnParameter> UpdateTaxStructure(DO_TaxStructure obj)
        //{
        //    using (var db = new eSyaEnterprise())
        //    {
        //        using (var dbContext = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                GtEccntc ap_cd = db.GtEccntc.Where(w => w.Isdcode == obj.ISDCode && w.TaxCode == obj.TaxCode).FirstOrDefault();
        //                if (ap_cd == null)
        //                {
        //                    return new DO_ReturnParameter() { Status = false, Message = "Tax Code is not exist For Selected ISD Code." };
        //                }

        //                IEnumerable<GtEccntc> ls_apct = db.GtEccntc.Where(w => w.Isdcode == obj.ISDCode).ToList();

        //                var is_SameDescExists = ls_apct.Where(w => w.TaxDescription.ToUpper().Replace(" ", "") == obj.TaxDescription.ToUpper().Replace(" ", "")
        //                        && w.TaxCode != obj.TaxCode).Count();
        //                if (is_SameDescExists > 0)
        //                {
        //                    return new DO_ReturnParameter() { Status = false, Message = "Tax Description is already Exists" };
        //                }

        //                var is_SameShortCodeExists = ls_apct.Where(w => w.TaxShortCode.ToUpper().Replace(" ", "") == obj.TaxShortCode.ToUpper().Replace(" ", "")
        //                        && w.TaxCode != obj.TaxCode).Count();
        //                if (is_SameShortCodeExists > 0)
        //                {
        //                    return new DO_ReturnParameter() { Status = false, Message = "Tax short Code is already Exists" };
        //                }
                        
        //                ap_cd.TaxShortCode = obj.TaxShortCode.Trim();
        //                ap_cd.TaxDescription = obj.TaxDescription.Trim();
        //                ap_cd.SlabOrPerc = obj.SlabOrPerc.Substring(0, 1);
        //                ap_cd.IsSplitApplicable = obj.IsSplitApplicable;
        //                ap_cd.ActiveStatus = obj.ActiveStatus;
        //                ap_cd.ModifiedBy = obj.UserID;
        //                ap_cd.ModifiedOn = System.DateTime.Now;
        //                ap_cd.ModifiedTerminal = obj.TerminalID;

        //                await db.SaveChangesAsync();

        //                dbContext.Commit();
        //                return new DO_ReturnParameter() { Status = true, Message = "Tax Structure Updated Successfully." };
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

        public async Task<List<DO_TaxStructure>> GetTaxCodeByISDCodes(int ISDCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccntc
                        .Where(w => w.Isdcode == ISDCode)
                        .Select(r => new DO_TaxStructure
                        {
                            TaxCode = r.TaxCode,
                            TaxDescription = r.TaxDescription,
                        }).OrderBy(o => o.TaxDescription).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveTaxStructure(bool status, int Isd_code, int Taxcode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccntc tx_cd = db.GtEccntc.Where(w => w.Isdcode == Isd_code && w.TaxCode == Taxcode).FirstOrDefault();
                        if (tx_cd == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Tax Structure is not exist" };
                        }

                        tx_cd.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Tax Structure Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Tax Structure De Activated Successfully." };
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
