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
    public class CountryRepository : ICountryRepository
    {
        #region Country Codes
        public async Task<List<DO_CountryCodes>> GetAllCountryCodesAsync()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEccncd.Join(db.GtEccuco,
                         x => x.CurrencyCode,
                         y => y.CurrencyCode,
                        (x, y) => new DO_CountryCodes

                        {
                            Isdcode = x.Isdcode,
                            CountryCode = x.CountryCode,
                            CountryName = x.CountryName,
                            CountryFlag = x.CountryFlag,
                            CurrencyCode = x.CurrencyCode,
                            MobileNumberPattern = x.MobileNumberPattern,
                            Uidlabel = x.Uidlabel,
                            Uidpattern = x.Uidpattern,
                            Nationality = x.Nationality,
                            IsPoboxApplicable = x.IsPoboxApplicable,
                            PoboxPattern = x.PoboxPattern,
                            IsPinapplicable = x.IsPinapplicable,
                            PincodePattern = x.PincodePattern,
                            ActiveStatus = x.ActiveStatus,
                            CurrencyName = y.CurrencyName
                        }).ToListAsync();
                    List<DO_CountryCodes> countrycodes = new List<DO_CountryCodes>();
                    foreach (var item in await result)
                    {
                        DO_CountryCodes country = new DO_CountryCodes();
                        country.Isdcode = item.Isdcode;
                        country.CountryCode = item.CountryCode;
                        country.CountryName = item.CountryName;
                        country.CountryFlag = "/" + item.CountryFlag;
                        country.CurrencyCode = item.CurrencyCode;
                        country.MobileNumberPattern = item.MobileNumberPattern;
                        country.Uidlabel = item.Uidlabel;
                        country.Uidpattern = item.Uidpattern;
                        country.Nationality = item.Nationality;
                        country.IsPoboxApplicable = item.IsPoboxApplicable;
                        country.PoboxPattern = item.PoboxPattern;
                        country.IsPinapplicable = item.IsPinapplicable;
                        country.PincodePattern = item.PincodePattern;
                        country.ActiveStatus = item.ActiveStatus;
                        country.CurrencyName = item.CurrencyName;
                        countrycodes.Add(country);
                    }

                    return countrycodes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoCountryCode(DO_CountryCodes countrycode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_IsdCodeExist = db.GtEccncd.Any(c => c.Isdcode == countrycode.Isdcode);
                        if (is_IsdCodeExist)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "ISD Code already exist." };
                        }

                        var is_CountryCodeExist = db.GtEccncd.Where(c => c.CountryCode.Trim().ToUpper().Replace(" ", "") == countrycode.CountryCode.Trim().ToUpper().Replace(" ", "")).Count();

                        if (is_CountryCodeExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Country Code already exist." };
                        }
                        var is_CountryNameExist = db.GtEccncd.Where(c => c.CountryName.Trim().ToUpper().Replace(" ", "") == countrycode.CountryName.Trim().ToUpper().Replace(" ", "")).Count();

                        if (is_CountryNameExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Country Name already exist." };
                        }
                        var ctr = new GtEccncd
                        {
                            Isdcode = countrycode.Isdcode,
                            CountryCode = countrycode.CountryCode,
                            CountryName = countrycode.CountryName,
                            CountryFlag = countrycode.CountryFlag,
                            CurrencyCode = countrycode.CurrencyCode,
                            MobileNumberPattern = countrycode.MobileNumberPattern,
                            Uidlabel = countrycode.Uidlabel,
                            Uidpattern = countrycode.Uidpattern,
                            Nationality = countrycode.Nationality,
                            IsPoboxApplicable = countrycode.IsPoboxApplicable,
                            PoboxPattern = countrycode.PoboxPattern,
                            IsPinapplicable = countrycode.IsPinapplicable,
                            PincodePattern = countrycode.PincodePattern,
                            ActiveStatus = countrycode.ActiveStatus,
                            FormId = countrycode.FormId,
                            CreatedBy = countrycode.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = countrycode.TerminalID
                        };
                        db.GtEccncd.Add(ctr);
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Country Created Successfully." };
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

        public async Task<DO_ReturnParameter> UpdateCountryCode(DO_CountryCodes countrycode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_CountryCodeExist = db.GtEccncd.Where(c => c.CountryCode.Trim().ToUpper().Replace(" ", "") == countrycode.CountryCode.Trim().ToUpper().Replace(" ", "")
                        && c.Isdcode != countrycode.Isdcode).Count();

                        if (is_CountryCodeExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Country Code already exist." };
                        }
                        var is_CountryNameExist = db.GtEccncd.Where(c => c.CountryName.Trim().ToUpper().Replace(" ", "") == countrycode.CountryName.Trim().ToUpper().Replace(" ", "")
                        && c.Isdcode != countrycode.Isdcode).Count();

                        if (is_CountryNameExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Country Name already exist." };
                        }
                        GtEccncd ctr = db.GtEccncd.Where(x => x.Isdcode == countrycode.Isdcode).FirstOrDefault();


                        ctr.CountryCode = countrycode.CountryCode;
                        ctr.CountryName = countrycode.CountryName;
                        ctr.CountryFlag = countrycode.CountryFlag;
                        ctr.CurrencyCode = countrycode.CurrencyCode;
                        ctr.MobileNumberPattern = countrycode.MobileNumberPattern;
                        ctr.Uidlabel = countrycode.Uidlabel;
                        ctr.Uidpattern = countrycode.Uidpattern;
                        ctr.Nationality = countrycode.Nationality;
                        ctr.IsPoboxApplicable = countrycode.IsPoboxApplicable;
                        ctr.PoboxPattern = countrycode.PoboxPattern;
                        ctr.IsPinapplicable = countrycode.IsPinapplicable;
                        ctr.PincodePattern = countrycode.PincodePattern;
                        ctr.ActiveStatus = countrycode.ActiveStatus;
                        ctr.ModifiedBy = countrycode.UserID;
                        ctr.ModifiedOn = System.DateTime.Now;
                        ctr.ModifiedTerminal = countrycode.TerminalID;

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Country Updated Successfully." };
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

        public async Task<DO_CountryCodes> GetCurrencyNamebyIsdCode(int IsdCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var Currency = db.GtEccncd.Where(c => c.Isdcode == IsdCode).Join(db.GtEccuco,
                         x => x.CurrencyCode,
                         y => y.CurrencyCode,
                        (x, y) => new DO_CountryCodes
                        {
                            CurrencyCode = x.CurrencyCode,
                            CurrencyName = y.CurrencyName
                        }).FirstOrDefaultAsync();


                    return await Currency;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveCountryCode(bool status, int Isd_code)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccncd isd_code = db.GtEccncd.Where(w => w.Isdcode == Isd_code).FirstOrDefault();
                        if (isd_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Country code is not exist" };
                        }

                        isd_code.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Country code Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Country code De Activated Successfully." };
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
        #endregion Country Codes

        #region Statutory Details

        public async Task<List<DO_eSyaParameter>> GetStatutoryCodesParameterList(int IsdCode, int StatutoryCode)
        {
            using (var db = new eSyaEnterprise())
            {
                try
                {
                    var ds = db.GtEcsupa
                        .Where(s => s.StatutoryCode == StatutoryCode && s.Isdcode == IsdCode)
                        .Select(p => new DO_eSyaParameter
                        {
                            ParameterID = p.ParameterId,
                            ParmAction = p.Action
                        }).ToListAsync();
                    return await ds;


                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public async Task<List<DO_CountryStatutoryDetails>> GetStatutoryCodesbyIsdcode(int Isdcode)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var statutorycodes = db.GtEccnsd.Where(x => x.Isdcode == Isdcode)

                                  .Select(st => new DO_CountryStatutoryDetails
                                  {
                                      Isdcode = st.Isdcode,
                                      StatutoryCode = st.StatutoryCode,
                                      StatShortCode = st.StatShortCode,
                                      StatutoryDescription = st.StatutoryDescription,
                                      StatPattern = st.StatPattern,
                                      ActiveStatus = st.ActiveStatus
                                  }).ToListAsync();
                    return await statutorycodes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateStatutoryCodes(DO_CountryStatutoryDetails obj)
        {
            try
            {
                if (obj.StatutoryCode != 0)
                {
                    return await UpdateStatutoryCodes(obj);
                }
                else
                {
                    return await InsertStatutoryCodes(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<DO_ReturnParameter> InsertStatutoryCodes(DO_CountryStatutoryDetails obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccnsd isStatshortcodeExists = db.GtEccnsd.FirstOrDefault(st => st.Isdcode == obj.Isdcode && st.StatShortCode.ToUpper().Replace(" ", "") == obj.StatShortCode.ToUpper().Replace(" ", ""));

                        if (isStatshortcodeExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Statutory Short Code already Exists in this" + obj.Isdcode + "Isd code" };
                        }

                        int maxval = db.GtEccnsd.Where(x => x.Isdcode == obj.Isdcode).Select(c => c.StatutoryCode).DefaultIfEmpty().Max();
                        int statutorycode_ = maxval + 1;
                        var stat_code = new GtEccnsd
                        {
                            Isdcode = obj.Isdcode,
                            StatutoryCode = statutorycode_,
                            StatShortCode = obj.StatShortCode,
                            StatutoryDescription = obj.StatutoryDescription,
                            StatPattern = obj.StatPattern,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEccnsd.Add(stat_code);
                        List<GtEcsupa> stparam = db.GtEcsupa.Where(p=>p.StatutoryCode==obj.StatutoryCode&&p.Isdcode==obj.Isdcode).ToList();
                        if (obj.l_FormParameter != null)
                        {
                            if (stparam.Count > 0)
                            {
                                foreach (var p in stparam)
                                {
                                    db.GtEcsupa.Remove(p);
                                    db.SaveChanges();
                                }

                            }
                            foreach (var param in obj.l_FormParameter)
                            {
                                GtEcsupa objparam = new GtEcsupa
                                {
                                    StatutoryCode = statutorycode_,
                                    Isdcode = obj.Isdcode,
                                    ParameterId = param.ParameterID,
                                    Action = param.ParmAction,
                                    ActiveStatus = param.ActiveStatus,
                                    //FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,

                                };
                                db.GtEcsupa.Add(objparam);
                                await db.SaveChangesAsync();

                            }

                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Statutory Code created Successfully" };
                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Statutory Code creation Failed." };
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

        public async Task<DO_ReturnParameter> UpdateStatutoryCodes(DO_CountryStatutoryDetails obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccnsd isStatshortcodeExists = db.GtEccnsd.FirstOrDefault(st => st.Isdcode == obj.Isdcode && st.StatShortCode.ToUpper().Replace(" ", "") == obj.StatShortCode.ToUpper().Replace(" ", "")
                        && st.StatutoryCode != obj.StatutoryCode);

                        if (isStatshortcodeExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Statutory Short Code already Exists in this" + obj.Isdcode + "Isd code" };
                        }


                        GtEccnsd stat_code = db.GtEccnsd.Where(st => st.StatutoryCode == obj.StatutoryCode && st.Isdcode == obj.Isdcode).FirstOrDefault();
                        if (stat_code == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Statutory Code is not exist" };
                        }

                        stat_code.StatShortCode = obj.StatShortCode;
                        stat_code.StatutoryDescription = obj.StatutoryDescription;
                        stat_code.StatPattern = obj.StatPattern;
                        stat_code.ActiveStatus = obj.ActiveStatus;
                        stat_code.ModifiedBy = obj.UserID;
                        stat_code.ModifiedOn = DateTime.Now;
                        stat_code.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();
                        List<GtEcsupa> stparam = db.GtEcsupa.Where(p => p.StatutoryCode == obj.StatutoryCode && p.Isdcode == obj.Isdcode).ToList();
                        if (obj.l_FormParameter != null)
                        {
                            if (stparam.Count > 0)
                            {
                                foreach (var p in stparam)
                                {
                                    db.GtEcsupa.Remove(p);
                                    db.SaveChanges();
                                }

                            }
                            foreach (var param in obj.l_FormParameter)
                            {
                                GtEcsupa objparam = new GtEcsupa
                                {
                                    StatutoryCode = obj.StatutoryCode,
                                    Isdcode = obj.Isdcode,
                                    ParameterId = param.ParameterID,
                                    Action = param.ParmAction,
                                    ActiveStatus = param.ActiveStatus,
                                    //FormId = obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,

                                };
                                db.GtEcsupa.Add(objparam);
                                await db.SaveChangesAsync();

                            }

                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Statutory Code Updated Successfully." };
                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Statutory Code Updated Failed." };
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

        public async Task<List<DO_CountryStatutoryDetails>> GetActiveStatutoryCodes()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var statutorycodes = db.GtEccnsd.Where(x => x.ActiveStatus == true)

                                  .Select(st => new DO_CountryStatutoryDetails
                                  {
                                      StatutoryCode = st.StatutoryCode,
                                      StatShortCode = st.StatShortCode,
                                      StatutoryDescription = st.StatutoryDescription
                                  }).ToListAsync();
                    return await statutorycodes;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveStatutoryCode(bool status, int Isd_code,int statutorycode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEccnsd statutory = db.GtEccnsd.Where(w => w.Isdcode == Isd_code && w.StatutoryCode==statutorycode).FirstOrDefault();
                        if (statutory == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "statutory code is not exist" };
                        }

                        statutory.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "statutory code Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "statutory code De Activated Successfully." };
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
        #endregion Statutory Details

        #region Tax Identification
        public async Task<List<DO_TaxIdentification>> GetActiveTaxIdentification()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var Taxidentifications = db.GtEccnti.Where(x => x.ActiveStatus == true)
                        .Select(t => new DO_TaxIdentification
                        {
                            Isdcode = t.Isdcode,
                            TaxIdentificationId = t.TaxIdentificationId,
                            TaxIdentificationDesc = t.TaxIdentificationId.ToString() + "-" + t.TaxIdentificationDesc
                        }).ToListAsync();

                    return await Taxidentifications;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_TaxIdentification>> GetTaxIdentificationByBSeg(int BusinessId, int SegmentId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    GtEcbssg bu_sg = db.GtEcbssg.Where(w => w.BusinessId == BusinessId && w.SegmentId == SegmentId).FirstOrDefault();
                    
                    var Taxidentifications = db.GtEccnti.Where(x => x.Isdcode == bu_sg.Isdcode && x.ActiveStatus == true)
                        .Select(t => new DO_TaxIdentification
                        {
                            TaxIdentificationId = t.TaxIdentificationId,
                            TaxIdentificationDesc = t.TaxIdentificationId.ToString() + "-" + t.TaxIdentificationDesc
                        }).ToListAsync();
                    return await Taxidentifications;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        #endregion Identification
    }
}
