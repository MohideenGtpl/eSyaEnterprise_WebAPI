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
    public class TaxIdentificationRepository : ITaxIdentificationRepository
    {
        public async Task<List<DO_TaxIdentification>> GetTaxIdentificationByISDCode(int ISDCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEccnti.Where(w => w.Isdcode == ISDCode)
                        .Select(x=> new DO_TaxIdentification
                        {
                            Isdcode = x.Isdcode,
                            TaxIdentificationId = x.TaxIdentificationId,
                            TaxIdentificationDesc = x.TaxIdentificationDesc,
                            ActiveStatus = x.ActiveStatus
                        }).OrderBy(o => o.TaxIdentificationId).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoTaxIdentification(DO_TaxIdentification obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var _isTaxDescriptionExist = db.GtEccnti.Where(w => w.Isdcode == obj.Isdcode && w.TaxIdentificationDesc == obj.TaxIdentificationDesc).Count();
                        if (_isTaxDescriptionExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Tax Identification Description is already Exists For Selected ISD Code & Tax Code" };
                        }

                        var _isTaxIdentificationExist = db.GtEccnti.Where(w => w.Isdcode == obj.Isdcode && w.TaxIdentificationId == obj.TaxIdentificationId).Count();
                        if (_isTaxIdentificationExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Tax Identification ID is already Exists For Selected ISD Code & Tax Code" };
                        }

                        //int _taxIdnId = 0;
                        //_taxIdnId = db.GtEccnti.Select(c => c.TaxIdentificationId).DefaultIfEmpty().Max();
                        //_taxIdnId = _taxIdnId + 1;


                        var ap_cd = new GtEccnti
                        {
                            Isdcode = obj.Isdcode,
                            TaxIdentificationId = obj.TaxIdentificationId,
                            TaxIdentificationDesc = obj.TaxIdentificationDesc.Trim(),
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID,

                        };
                        db.GtEccnti.Add(ap_cd);
                        
                       await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Tax Identification Created Successfully." };
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

        public async Task<DO_ReturnParameter> UpdateTaxIdentification(DO_TaxIdentification obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccnti ap_cd = db.GtEccnti.Where(w => w.Isdcode == obj.Isdcode && w.TaxIdentificationId == obj.TaxIdentificationId).FirstOrDefault();
                        if (ap_cd == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Tax Identification Id is not exist For Selected ISD Code." };
                        }

                        GtEccnti ap_cd1 = db.GtEccnti.Where(w => w.Isdcode == obj.Isdcode && w.TaxIdentificationDesc.ToUpper().Replace(" ", "") == obj.TaxIdentificationDesc.ToUpper().Replace(" ", "")
                         && w.TaxIdentificationId != obj.TaxIdentificationId).FirstOrDefault();
                        if (ap_cd1 != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Tax Identification Description is already Exist For Selected ISD Code." };
                        }

                        //IEnumerable<GtEccnti> ls_apct = db.GtEccnti.Where(w => w.Isdcode == obj.Isdcode).ToList();

                        //var is_SameDescExists = ls_apct.Where(w => w.TaxIdentificationDesc.ToUpper().Replace(" ", "") == obj.TaxIdentificationDesc.ToUpper().Replace(" ", "")
                        //        && w.TaxIdentificationId != obj.TaxIdentificationId).Count();
                        //if (is_SameDescExists > 0)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "Tax Identification Description is already Exists" };
                        //}

                        ap_cd.TaxIdentificationId = obj.TaxIdentificationId;
                        ap_cd.TaxIdentificationDesc = obj.TaxIdentificationDesc.Trim();
                        ap_cd.ActiveStatus = obj.ActiveStatus;
                        ap_cd.ModifiedBy = obj.UserID;
                        ap_cd.ModifiedOn = System.DateTime.Now;
                        ap_cd.ModifiedTerminal = obj.TerminalID;

                        await db.SaveChangesAsync();

                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Tax Identification Updated Successfully." };
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveTaxIdentification(bool status, int Isd_code, int TaxIdentificationId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccnti tax_identification = db.GtEccnti.Where(w => w.Isdcode == Isd_code && w.TaxIdentificationId == TaxIdentificationId).FirstOrDefault();
                        if (tax_identification == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Tax Identification is not exist" };
                        }

                        tax_identification.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Tax Identification Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Tax Identification De Activated Successfully." };
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
