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
    public class LocalizationMasterRepository:ILocalizationMasterRepository
    {
        #region Localization Master
        public async Task<List<DO_LocalizationMaster>> GetLocalizationTableMaster()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    return await db.GtEclttm
                    .Select(t => new DO_LocalizationMaster
                    {
                        TableCode = t.TableCode,
                        TableName = t.TableName,
                        SchemaName = t.SchemaName,
                        ActiveStatus = t.ActiveStatus
                    }).ToListAsync();

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateLocalizationTableMaster(DO_LocalizationMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if(obj.IsInsert)
                        {
                            if (db.GtEclttm.Where(t => t.TableCode == obj.TableCode).Count() > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Table Code already exist." };
                            }
                        }

                        if (db.GtEclttm.Where(t => t.TableName.Trim().ToUpper() == obj.TableName.Trim().ToUpper() && t.TableCode != obj.TableCode).Count() > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Table Name already exist." };
                        }
                        else if (db.GtEclttm.Where(t => t.SchemaName.Trim().ToUpper() == obj.SchemaName.Trim().ToUpper() && t.TableCode != obj.TableCode).Count() > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Schema Name already exist." };
                        }
                        else
                        {
                            GtEclttm lm = db.GtEclttm.Where(x => x.TableCode == obj.TableCode).FirstOrDefault();

                            if (lm == null)
                            {
                                lm = new GtEclttm
                                {
                                    TableCode = obj.TableCode,
                                    SchemaName = obj.SchemaName,
                                    TableName = obj.TableName,
                                    ActiveStatus = obj.ActiveStatus,
                                    FormId=obj.FormId,
                                    CreatedBy = obj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = obj.TerminalID
                                };
                                db.GtEclttm.Add(lm);
                            }
                            else
                            {
                                lm.SchemaName = obj.SchemaName;
                                lm.TableName = obj.TableName;
                                lm.ActiveStatus = obj.ActiveStatus;
                                lm.ModifiedBy = obj.UserID;
                                lm.ModifiedOn = System.DateTime.Now;
                                lm.ModifiedTerminal = obj.TerminalID;
                            }

                           await db.SaveChangesAsync();
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, Message = "Table Mapping Created/Updated Successfully." };
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
        public async Task<DO_ReturnParameter> ActiveOrDeActiveLocalizationTableMaster(bool status, int Tablecode)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEclttm table_master = db.GtEclttm.Where(w => w.TableCode == Tablecode).FirstOrDefault();
                        if (table_master == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Table Mapping is not exist" };
                        }

                        table_master.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Table Mapping Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Table Mapping De Activated Successfully." };
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
        #endregion

        #region Localization Language Mapping
        public async Task<List<DO_LocalizationMaster>> GetLocalizationMaster()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    return await db.GtEclttm.Where(x => x.ActiveStatus == true)
                         .Select(t => new DO_LocalizationMaster
                         {
                             TableCode = t.TableCode,
                             TableName = t.TableName,
                         }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateLocalizationLanguageMapping(List<DO_LocalizationLanguageMapping> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        foreach (var l in obj.Where(w=> w.FieldDescLanguage != null && w.FieldDescLanguage != ""))
                        {
                            GtEclttl tm = db.GtEclttl.Where(x => x.LanguageCode == l.LanguageCode
                                            && x.TableCode == l.TableCode
                                            && x.TablePrimaryKeyId == l.TablePrimaryKeyId).FirstOrDefault();
                            if (tm == null)
                            {
                                var add = new GtEclttl
                                {
                                    LanguageCode = l.LanguageCode,
                                    TableCode = l.TableCode,
                                    TablePrimaryKeyId = l.TablePrimaryKeyId,
                                    FieldDescLanguage = l.FieldDescLanguage,
                                    ActiveStatus = true,
                                    FormId=l.FormId,
                                    CreatedBy = l.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = l.TerminalID
                                };
                                db.GtEclttl.Add(add);
                            }
                            else
                            {
                                tm.FieldDescLanguage = l.FieldDescLanguage;
                                tm.ActiveStatus = true;
                                tm.ModifiedBy = l.UserID;
                                tm.ModifiedOn = System.DateTime.Now;
                                tm.ModifiedTerminal = l.TerminalID;
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

        public List <DO_LocalizationLanguageMapping> GetLocalizationLanguageMapping(string languageCode, int tableCode)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var SchemaName = db.GtEclttm.Where(w => w.TableCode == tableCode).FirstOrDefault().SchemaName;

                    var tableMasterDetail = GetTableKeyValue(db, SchemaName);

                    if (tableMasterDetail!=null)
                    {
                        var lm = tableMasterDetail
                            .GroupJoin(db.GtEclttl.Where(w => w.LanguageCode == languageCode
                                    && w.TableCode == tableCode),
                                m => m.TablePrimaryKeyId,
                                l => l.TablePrimaryKeyId,
                                (m, l) => new { m, l = l.FirstOrDefault() }).DefaultIfEmpty()
                                .Select(r => new DO_LocalizationLanguageMapping
                                {
                                    TablePrimaryKeyId = r.m.TablePrimaryKeyId,
                                    FieldDescription = r.m.FieldDesc,
                                    FieldDescLanguage = r.l != null ? r.l.FieldDescLanguage : "",
                                }).ToList();

                        return lm;
                    }
                    else
                       return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IEnumerable<DO_TableField> GetTableKeyValue(eSyaEnterprise db, string SchemaName)
        {
             if (SchemaName.ToUpper() == "GT_ECAPCT")
            {
                return db.GtEcapct.Where(w => w.ActiveStatus)
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.CodeType,
                        FieldDesc = r.CodeTyepDesc
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECAPCD")
            {
                return db.GtEcapcd.Where(w => w.ActiveStatus)
                    .Join(db.GtEcapct,
                    a => a.CodeType,
                    c => c.CodeType,
                    (a, c) => new { a, c })
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.a.ApplicationCode,
                        FieldDesc = r.c.CodeTyepDesc + " - " + r.a.CodeDesc
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECFMFD")
            {
                return db.GtEcfmfd
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.FormId,
                        FieldDesc = r.FormName
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECFMAC")
            {
                return db.GtEcfmac
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.ActionId,
                        FieldDesc = r.ActionDesc
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECMAMN")
            {
                return db.GtEcmamn
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.MainMenuId,
                        FieldDesc = r.MainMenu
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECSBMN")
            {
                return db.GtEcsbmn
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.MenuItemId,
                        FieldDesc = r.MenuItemName
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECBSLN")
            {
                return db.GtEcbsln
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.BusinessKey,
                        FieldDesc = r.LocationDescription
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECCNCD")
            {
                return db.GtEccncd
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.Isdcode,
                        FieldDesc = r.CountryName
                    });
            }
            else if (SchemaName.ToUpper() == "GT_ECSTRM")
            {
                return db.GtEcstrm
                    .Select(r => new DO_TableField
                    {
                        TablePrimaryKeyId = r.StoreCode,
                        FieldDesc = r.StoreDesc
                    });
            }
            return null;
        }



        #endregion

        #region Language Controller
        public async Task<List<string>> GetAllControllers()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    //return await db.GtEcfmfd.Where(x => x.ActiveStatus == true)
                    //   // .OrderBy(t=> t.ControllerName.Substring((t.ControllerName.IndexOf('/')) + 1))
                    //    .Select(t => t.ControllerName.Substring((t.ControllerName.IndexOf('/')) + 1).Replace("Index","Account"))
                    //    .Distinct().OrderBy(c => c).ToListAsync();

                    return await db.GtEbecnt.Where(x => x.ActiveStatus == true)
                            .Select(t => t.Controller.Replace("Index", "Account"))
                            .Distinct().OrderBy(c => c).ToListAsync();

                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_LanguageController>> GetLanguageControllersbyResource(string Resource)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (Resource!="All")
                    {
                      return await db.GtEcltfc.Where(x=>x.ResourceName.ToUpper().Replace(" ", "") == Resource.ToUpper().Replace(" ", ""))
                        .Select(lc => new DO_LanguageController
                        {
                            ResourceId = lc.ResourceId,
                            ResourceName = lc.ResourceName,
                            Key=lc.Key,
                            Value=lc.Value,
                            ActiveStatus=lc.ActiveStatus,
                            FormId=lc.FormId
                        }).ToListAsync();
                    }
                    else
                    {
                        return await db.GtEcltfc
                        .Select(lc => new DO_LanguageController
                        {
                            ResourceId = lc.ResourceId,
                            ResourceName = lc.ResourceName,
                            Key = lc.Key,
                            Value = lc.Value,
                            ActiveStatus = lc.ActiveStatus,
                            FormId = lc.FormId
                        }).ToListAsync();
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateLanguageController(DO_LanguageController lobj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        
                            GtEcltfc lc = db.GtEcltfc.Where(x => x.ResourceId == lobj.ResourceId).FirstOrDefault();

                            if (lc == null)
                            {
                            GtEcltfc is_KeyExists = db.GtEcltfc.FirstOrDefault(k=> k.Key.ToUpper().Replace(" ", "") == lobj.Key.ToUpper().Replace(" ", "") && k.ResourceName.ToUpper().Replace(" ", "") == lobj.ResourceName.ToUpper().Replace(" ", ""));
                            if (is_KeyExists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Key already Exists in this selected Resource." };

                            }
                            int maxval = db.GtEcltfc.Select(c => c.ResourceId).DefaultIfEmpty().Max();
                                int ResourceId = maxval + 1;
                                lc = new GtEcltfc
                                {
                                    ResourceId= ResourceId,
                                    ResourceName= lobj.ResourceName.Trim(),
                                    Key=lobj.Key.Trim(),
                                    Value=lobj.Value.Trim(),
                                    ActiveStatus = lobj.ActiveStatus,
                                    FormId=lobj.FormId,
                                    CreatedBy = lobj.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = lobj.TerminalID
                                };
                                db.GtEcltfc.Add(lc);
                            }
                            else
                            {
                            GtEcltfc is_KeyExists = db.GtEcltfc.FirstOrDefault(k => k.Key.ToUpper().Replace(" ", "") == lobj.Key.ToUpper().Replace(" ", "") && k.ResourceName.ToUpper().Replace(" ", "") == lobj.ResourceName.ToUpper().Replace(" ", "")
                            &&k.ResourceId!=lobj.ResourceId);
                            if (is_KeyExists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Key already Exists in this selected Resource." };

                            }
                            lc.ResourceName = lobj.ResourceName.Trim();
                                lc.Key = lobj.Key.Trim();
                                lc.Value = lobj.Value.Trim();
                                lc.ActiveStatus = lobj.ActiveStatus;
                                lc.ModifiedBy = lobj.UserID;
                                lc.ModifiedOn = System.DateTime.Now;
                                lc.ModifiedTerminal = lobj.TerminalID;
                            }

                            await db.SaveChangesAsync();
                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, Message = "Saved/Updated Successfully." };
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
        public async Task<DO_ReturnParameter> ActiveOrDeActiveLanguageController(bool status, int ResourceId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcltfc lan_controller = db.GtEcltfc.Where(w => w.ResourceId == ResourceId).FirstOrDefault();
                        if (lan_controller == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Language Controller is not exist" };
                        }

                        lan_controller.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Language Controller Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Language Controller De Activated Successfully." };
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
        #endregion Language Controller

        #region Language Culture
        public async Task<List<DO_LanguageController>> GetResources()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    return await db.GtEcltfc.Where(x => x.ActiveStatus == true).Select
                    (r => new DO_LanguageController
                    {
                        ResourceName = r.ResourceName
                    }).Distinct().ToListAsync();

                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task <List<DO_LanguageCulture>> GetLanguageCulture(string Culture, string Resource)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (Resource == "All")
                    {
                        return await db.GtEcltfc
                           .GroupJoin(db.GtEcltcd.Where(x => x.Culture.ToUpper().Trim() == Culture.ToUpper().Trim()),
                           a => a.ResourceId,
                           f => f.ResourceId,
                           (a, f) => new { a, f = f.FirstOrDefault() })
                           .Select(r => new DO_LanguageCulture
                           {
                               ResourceId = r.a.ResourceId,
                               ResourceName = r.a.ResourceName,
                               Key= r.a.Key,
                               Value = r.a.Value,
                               Culture = r.f != null ? r.f.Culture : "",
                               CultureValue = r.f != null ? r.f.Value : ""

                           }).ToListAsync();
                    }
                    else
                    {
                        return await db.GtEcltfc.Where(x => x.ResourceName.ToUpper().Trim() == Resource.ToUpper().Trim())
                          .GroupJoin(db.GtEcltcd.Where(x => x.Culture.ToUpper().Trim() == Culture.ToUpper().Trim()),
                          a => a.ResourceId,
                          f => f.ResourceId,
                          (a, f) => new { a, f = f.FirstOrDefault() })
                          .Select(r => new DO_LanguageCulture
                          {
                              ResourceId = r.a.ResourceId,
                              ResourceName = r.a.ResourceName,
                              Key = r.a.Key,
                              Value = r.a.Value,
                              Culture = r.f != null ? r.f.Culture : "",
                              CultureValue = r.f != null ? r.f.Value : ""

                          }).ToListAsync();
                    }
                  

                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateLanguageCulture(List<DO_LanguageCulture> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        foreach (var rc in obj.Where(x => x.CultureValue != null && x.CultureValue != ""))
                        {
                            GtEcltcd r_culture = db.GtEcltcd.Where(x => x.ResourceId == rc.ResourceId
                                            && x.Culture.ToUpper().Trim() == rc.Culture.ToUpper().Trim()).FirstOrDefault();
                            if (r_culture == null)
                            {
                                var add = new GtEcltcd
                                {
                                    ResourceId = rc.ResourceId,
                                    Culture = rc.Culture,
                                    Value = rc.CultureValue,
                                    ActiveStatus = true,
                                    FormId = rc.FormId,
                                    CreatedBy = rc.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = rc.TerminalID
                                };
                                db.GtEcltcd.Add(add);
                            }
                            else
                            {
                                r_culture.Value = rc.CultureValue;
                                r_culture.ActiveStatus = true;
                                r_culture.ModifiedBy = rc.UserID;
                                r_culture.ModifiedOn = System.DateTime.Now;
                                r_culture.ModifiedTerminal = rc.TerminalID;
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
        #endregion Language Culture

        #region Culture Keys
        public List<DO_LanguageCulture> GetDistinictCultureKeys(string Culture)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result= db.GtEcltfc
                            .GroupJoin(db.GtEcltcd.Where(x => x.Culture.ToUpper().Trim() == Culture.ToUpper().Trim()),
                            a => a.ResourceId,
                            f => f.ResourceId,
                            (a, f) => new { a, f = f.FirstOrDefault() })
                            .Select(r => new DO_LanguageCulture
                            {
                                Key = r.a.Key,
                                Value = r.a.Value,
                                CultureValue = r.f != null ? r.f.Value : ""

                            }).ToList();
                    var DistinctKeys = result.GroupBy(x => x.Key).Select(y => y.First());
                    return DistinctKeys.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertOrUpdateCultureKeys(List<DO_LanguageCulture> obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        
                        foreach (var rc in obj.Where(x => x.CultureValue != null && x.CultureValue != ""))
                        {
                            var ResourceIds = db.GtEcltfc.Where(k => k.Key.ToUpper().Trim() == rc.Key.ToUpper().Trim()).ToList();

                            foreach (var resId in ResourceIds)
                            {
                            GtEcltcd r_culture = db.GtEcltcd.Where(x => x.ResourceId == resId.ResourceId
                                            && x.Culture.ToUpper().Trim() == rc.Culture.ToUpper().Trim()).FirstOrDefault();
                            if (r_culture == null)
                            {
                                var add = new GtEcltcd
                                {
                                    ResourceId = resId.ResourceId,
                                    Culture = rc.Culture,
                                    Value = rc.CultureValue,
                                    ActiveStatus = true,
                                    FormId = rc.FormId,
                                    CreatedBy = rc.UserID,
                                    CreatedOn = System.DateTime.Now,
                                    CreatedTerminal = rc.TerminalID
                                };
                                db.GtEcltcd.Add(add);
                            }
                            else
                            {
                                r_culture.Value = rc.CultureValue;
                                r_culture.ActiveStatus = true;
                                r_culture.ModifiedBy = rc.UserID;
                                r_culture.ModifiedOn = System.DateTime.Now;
                                r_culture.ModifiedTerminal = rc.TerminalID;
                            }
                            await db.SaveChangesAsync();
                            }
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
        #endregion Culture Keys
    }

    public class DO_TableField
    {
        public int TablePrimaryKeyId { get; set; }
        public string FieldDesc { get; set; }
    }
}
