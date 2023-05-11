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
    public class DocumentControlRepository:IDocumentControlRepository
    {
        #region Old Document Control
        public async Task<List<DO_DocumentControl>> GetDocumentControls()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcdccn.Join(db.GtEcfmfd,
                        d => d.FormId,
                        f => f.FormId,
                        (d, f) => new DO_DocumentControl
                       {
                        FormId = d.FormId,
                        DocumentId  = d.DocumentId,
                        DocumentType= d.DocumentType,
                        DocumentCode= d.DocumentCode,
                        DocCodeDesc = d.DocCodeDesc,
                        DocumentCategory = d.DocumentCategory,
                        DocCatgDesc = d.DocCatgDesc,
                        IsFinancialYearAppl= d.IsFinancialYearAppl,
                        IsStoreLinkAppl = d.IsStoreLinkAppl,
                        IsTransactionModeAppl= d.IsTransactionModeAppl,
                        IsCustomerGroupAppl = d.IsCustomerGroupAppl,
                        LinkToDashboard = d.LinkToDashboard,
                        UsageStatus = d.UsageStatus,
                        ActiveStatus = d.ActiveStatus,
                        FormName=f.FormName

                  }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertDocumentControl(DO_DocumentControl control)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var isdocumentIdExists = db.GtEcdccn.Where(d => d.FormId == control.FormId && d.DocumentId==control.DocumentId).FirstOrDefault();
                        if (isdocumentIdExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Document Id is already exist with selected Form." };
                        }
                        var isdocumentCodeExists = db.GtEcdccn.Where(d => d.DocumentCode.ToUpper().Replace(" ", "") == control.DocumentCode.ToUpper().Replace(" ", "") && d.FormId == control.FormId).FirstOrDefault();
                        if (isdocumentCodeExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Document Code is already exist with selected Form." };
                        }
                        var doc_ctrl = new GtEcdccn
                        {
                            FormId = control.FormId,
                            DocumentId = control.DocumentId,
                            DocumentType = control.DocumentType,
                            DocumentCode = control.DocumentCode,
                            DocCodeDesc = control.DocCodeDesc,
                            DocumentCategory = control.DocumentCategory,
                            DocCatgDesc = control.DocCatgDesc,
                            IsFinancialYearAppl = control.IsFinancialYearAppl,
                            IsStoreLinkAppl = control.IsStoreLinkAppl,
                            IsTransactionModeAppl = control.IsTransactionModeAppl,
                            IsCustomerGroupAppl = control.IsCustomerGroupAppl,
                            LinkToDashboard = control.LinkToDashboard,
                            UsageStatus = control.UsageStatus,
                            ActiveStatus = control.ActiveStatus,
                            CreatedBy = control.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = control.TerminalID
                        };
                       db.GtEcdccn.Add(doc_ctrl);
                       await db.SaveChangesAsync();
                       dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Document Control Created Successfully." };
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

        public async Task<DO_ReturnParameter> UpdateDocumentControl(DO_DocumentControl control)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                       
                        var isdocumentCodeExists = db.GtEcdccn.Where(d => d.DocumentCode.ToUpper().Replace(" ", "") == control.DocumentCode.ToUpper().Replace(" ", "") && d.FormId == control.FormId
                        && d.DocumentId != control.DocumentId).FirstOrDefault();
                        if (isdocumentCodeExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Document Code is already exist with selected Form." };
                        }
                        GtEcdccn doc_ctrl = db.GtEcdccn.Where(dc =>dc.FormId==control.FormId&&dc.DocumentId==control.DocumentId).FirstOrDefault();
                        if (doc_ctrl != null)
                        {
                            doc_ctrl.DocumentType = control.DocumentType;
                            doc_ctrl.DocumentCode = control.DocumentCode;
                            doc_ctrl.DocCodeDesc = control.DocCodeDesc;
                            doc_ctrl.DocumentCategory = control.DocumentCategory;
                            doc_ctrl.DocCatgDesc = control.DocCatgDesc;
                            doc_ctrl.IsFinancialYearAppl = control.IsFinancialYearAppl;
                            doc_ctrl.IsStoreLinkAppl = control.IsStoreLinkAppl;
                            doc_ctrl.IsTransactionModeAppl = control.IsTransactionModeAppl;
                            doc_ctrl.IsCustomerGroupAppl = control.IsCustomerGroupAppl;
                            doc_ctrl.LinkToDashboard = control.LinkToDashboard;
                            doc_ctrl.UsageStatus = control.UsageStatus;
                            doc_ctrl.ActiveStatus = control.ActiveStatus;
                            doc_ctrl.ModifiedBy = control.UserID;
                            doc_ctrl.ModifiedOn = System.DateTime.Now;
                            doc_ctrl.ModifiedTerminal = control.TerminalID;
                           await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Document Control Updated Successfully." };

                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Document Control does Not Exists." };

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveDocumentControl(bool status, int formId, int documentId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcdccn doc_ctrl = db.GtEcdccn.Where(w => w.FormId == formId && w.DocumentId == documentId).FirstOrDefault();
                        if (doc_ctrl == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Document Control is not exist" };
                        }

                        doc_ctrl.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Document Control Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Document Control De Activated Successfully." };
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
        #endregion Old Document Control

        #region New Document Control
        public async Task<List<DO_DocumentControlMaster>> GetDocumentControlMaster()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtDccnst.Select(
                        x => new DO_DocumentControlMaster
                        {
                           DocumentId=x.DocumentId,
                           DocumentDesc=x.DocumentDesc,
                           ShortDesc=x.ShortDesc,
                           DocumentType=x.DocumentType,
                           IsFinancialYearAppl=x.IsFinancialYearAppl,
                           IsStoreLinkAppl=x.IsStoreLinkAppl,
                           IsTransactionModeAppl=x.IsTransactionModeAppl,
                           IsCustomerGroupAppl=x.IsCustomerGroupAppl,
                           SchemeName=x.SchemeName,
                           UsageStatus=x.UsageStatus,
                           ActiveStatus=x.ActiveStatus
                        }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertDocumentControlMaster(DO_DocumentControlMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var isdocId_Exists = db.GtDccnst.Where(d => d.DocumentId == obj.DocumentId).FirstOrDefault();
                        if (isdocId_Exists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Document Id is already exist." };
                        }
                        //var isshotdesc_Exists = db.GtDccnst.Where(d => d.ShortDesc.ToUpper().Replace(" ", "") == obj.ShortDesc.ToUpper().Replace(" ", "")).FirstOrDefault();
                        //if (isshotdesc_Exists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "Document Short Description is already exist." };
                        //}
                        var doc_ctrlmaster = new GtDccnst
                        {
                            DocumentId = obj.DocumentId,
                            DocumentDesc = obj.DocumentDesc,
                            ShortDesc = obj.ShortDesc,
                            IsFinancialYearAppl = obj.IsFinancialYearAppl,
                            IsStoreLinkAppl = obj.IsStoreLinkAppl,
                            IsTransactionModeAppl = obj.IsTransactionModeAppl,
                            IsCustomerGroupAppl = obj.IsCustomerGroupAppl,
                            DocumentType = obj.DocumentType,
                            SchemeName = obj.SchemeName,
                            UsageStatus = obj.UsageStatus,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtDccnst.Add(doc_ctrlmaster);
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Document Control Created Successfully." };
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

        public async Task<DO_ReturnParameter> UpdateDocumentControlMaster(DO_DocumentControlMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        //var isshotdesc_Exists = db.GtDccnst.Where(d => d.ShortDesc.ToUpper().Replace(" ", "") == obj.ShortDesc.ToUpper().Replace(" ", "")
                        //&& d.DocumentId != obj.DocumentId).FirstOrDefault();

                        //if (isshotdesc_Exists != null)
                        //{
                        //    return new DO_ReturnParameter() { Status = false, Message = "Document Short Description is already exist." };
                        //}

                        GtDccnst doc_ctrl = db.GtDccnst.Where(dc => dc.DocumentId == obj.DocumentId).FirstOrDefault();

                        if (doc_ctrl != null)
                        {
                           
                            doc_ctrl.DocumentDesc = obj.DocumentDesc;
                            doc_ctrl.ShortDesc = obj.ShortDesc;
                            doc_ctrl.DocumentType = obj.DocumentType;
                            doc_ctrl.SchemeName = obj.SchemeName;
                            doc_ctrl.IsFinancialYearAppl = obj.IsFinancialYearAppl;
                            doc_ctrl.IsStoreLinkAppl = obj.IsStoreLinkAppl;
                            doc_ctrl.IsTransactionModeAppl = obj.IsTransactionModeAppl;
                            doc_ctrl.IsCustomerGroupAppl = obj.IsCustomerGroupAppl;
                            doc_ctrl.UsageStatus = obj.UsageStatus;
                            doc_ctrl.ActiveStatus = obj.ActiveStatus;
                            doc_ctrl.ModifiedBy = obj.UserID;
                            doc_ctrl.ModifiedOn = System.DateTime.Now;
                            doc_ctrl.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Document Control Updated Successfully." };

                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Document Control does Not Exists." };

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveDocumentControlMaster(bool status, int documentId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtDccnst doc_ctrl = db.GtDccnst.Where(w => w.DocumentId == documentId).FirstOrDefault();
                        if (doc_ctrl == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Document Control is not exist" };
                        }

                        doc_ctrl.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Document Control Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Document Control De Activated Successfully." };
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
        #endregion New Document Control

        #region FORM LINK TO DOCUMENT

        public async Task<List<DO_DocumentControlMaster>> GetActiveDocuments()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtDccnst.Where(x=>x.ActiveStatus==true)
                        .Select(r => new DO_DocumentControlMaster
                        {
                            DocumentId=r.DocumentId,
                            DocumentDesc=r.DocumentDesc
                        }).OrderBy(o => o.DocumentDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_FormNames>> GetDocumentRequiredForms()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcfmfd.Where(x=>x.ActiveStatus==true && x.IsDocumentNumberRequired==true)
                        .Select(r => new DO_FormNames
                        {
                            FormID=r.FormId,
                            FormName=r.FormName
                        }).OrderBy(o => o.FormName).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoFormDocumentLink(DO_FormDocumentLink obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_formlinked = db.GtDncnfd.Where(d => d.FormId == obj.FormId && d.DocumentId == obj.DocumentId).FirstOrDefault();
                        if (is_formlinked != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Selected Form is already linked with selected Document." };
                        }
                        
                        var doc_link = new GtDncnfd
                        {
                            FormId = obj.FormId,
                            DocumentId = obj.DocumentId,
                            ActiveStatus = obj.ActiveStatus,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtDncnfd.Add(doc_link);
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Form is linked with selected document Successfully." };
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

        public async Task<List<DO_FormDocumentLink>> GetFormLinkedDocuments()
         
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds =await db.GtDncnfd.Join(db.GtDccnst,
                         x => x.DocumentId,
                         y => y.DocumentId,
                         (x, y) => new { x, y }).Join(db.GtEcfmfd,
                         a => a.x.FormId,
                         p => p.FormId, (a, p) => new { a, p }).Select(r => new DO_FormDocumentLink
                         {
                             FormId=r.a.x.FormId,
                             FormName=r.p.FormName,
                             DocumentId=r.a.x.DocumentId,
                             DocumentName=r.a.y.DocumentDesc,
                             ActiveStatus = r.a.x.ActiveStatus,
                         }).ToListAsync();

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DO_ReturnParameter> DeleteFormLinkedDocument(int formId, int documentId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtDncnfd doc_link = db.GtDncnfd.Where(w => w.FormId == formId && w.DocumentId == documentId).FirstOrDefault();
                        if (doc_link == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Form is not exist" };
                        }

                        db.GtDncnfd.Remove(doc_link);
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

        #endregion FORM LINK TO DOCUMENT

        #region Document Generation 

        public async Task<List<DO_StoreMaster>> GetStoresByBusinessKey(int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var fa = await db.GtEastbl.Where(w => w.ActiveStatus == true && w.BusinessKey==businesskey)
                     .Join(db.GtEcstrm,
                     a => a.StoreCode,
                     f => f.StoreCode,
                     (a, f) => new { a, f })
                     .Select(r => new DO_StoreMaster
                     {
                         StoreCode = r.a.StoreCode,
                         StoreDesc = r.f.StoreDesc

                     }).ToListAsync();
                    var distinctList = fa.GroupBy(e => e.StoreCode).Select(g => g.First());
                    return distinctList.ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoDocumentGeneration(DO_DocumentGeneration obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.ddlTransmode == "GN")
                        {
                            var is_Exists = db.GtDncn01.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear==obj.FinancialYear).FirstOrDefault();
                            if (is_Exists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Already Exists" };
                            }
                            
                            var dc_gen = new GtDncn01
                            {
                                BusinessKey = obj.BusinessKey,
                                DocumentId = obj.DocumentId,
                                FinancialYear = obj.FinancialYear,
                                StartDocNumber = obj.StartDocNumber,
                                //CurrentDocNumber = obj.CurrentDocNumber,
                                CurrentDocNumber = obj.StartDocNumber,
                                //CurrentDocDate = DateTime.Now,
                                CurrentDocDate = obj.CurrentDocDate,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtDncn01.Add(dc_gen);
                        }
                        if (obj.ddlTransmode == "TR")
                        {
                            var is_Exists = db.GtDncn02.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear
                            &&d.TransactionMode.ToUpper().Replace(" ", "") == obj.TransactionMode.ToUpper().Replace(" ", "")).FirstOrDefault();
                            if (is_Exists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Already Exists" };
                            }

                            var dc_trns = new GtDncn02
                            {
                                BusinessKey = obj.BusinessKey,
                                DocumentId = obj.DocumentId,
                                FinancialYear = obj.FinancialYear,
                                TransactionMode=obj.TransactionMode,
                                StartDocNumber = obj.StartDocNumber,
                                //CurrentDocNumber = obj.CurrentDocNumber,
                                CurrentDocNumber = obj.StartDocNumber,
                                //CurrentDocDate = DateTime.Now,
                                CurrentDocDate = obj.CurrentDocDate,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtDncn02.Add(dc_trns);
                        }
                        if (obj.ddlTransmode == "ST")
                        {
                            var is_Exists = db.GtDncn03.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear
                           && d.TransactionMode.ToUpper().Replace(" ", "") == obj.TransactionMode.ToUpper().Replace(" ", "") && d.StoreCode==obj.StoreCode).FirstOrDefault();
                            if (is_Exists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Already Exists" };
                            }

                            var dc_store = new GtDncn03
                            {
                                BusinessKey = obj.BusinessKey,
                                DocumentId = obj.DocumentId,
                                FinancialYear = obj.FinancialYear,
                                TransactionMode = obj.TransactionMode,
                                StoreCode=obj.StoreCode,
                                StartDocNumber = obj.StartDocNumber,
                                //CurrentDocNumber = obj.CurrentDocNumber,
                                CurrentDocNumber = obj.StartDocNumber,
                                //CurrentDocDate = DateTime.Now,
                                CurrentDocDate = obj.CurrentDocDate,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtDncn03.Add(dc_store);
                        }
                        
                        await db.SaveChangesAsync();
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

        public async Task<DO_ReturnParameter> UpdateDocumentGeneration(DO_DocumentGeneration obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.ddlTransmode == "GN")
                        {
                            GtDncn01 trans_01 = db.GtDncn01.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear).FirstOrDefault();
                            if (trans_01 == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Not Exists" };
                            }
                            trans_01.StartDocNumber = obj.StartDocNumber;
                            trans_01.CurrentDocDate = obj.CurrentDocDate;
                            trans_01.ModifiedBy = obj.UserID;
                            trans_01.ModifiedOn = System.DateTime.Now;
                            trans_01.ModifiedTerminal = obj.TerminalID;
                        }
                        if (obj.ddlTransmode == "TR")
                        {
                            GtDncn02 trans_02 = db.GtDncn02.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear
                            && d.TransactionMode.ToUpper().Replace(" ", "") == obj.TransactionMode.ToUpper().Replace(" ", "")).FirstOrDefault();
                            if (trans_02 == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Not Exists" };
                            }

                            trans_02.StartDocNumber = obj.StartDocNumber;
                            trans_02.CurrentDocDate = obj.CurrentDocDate;
                            trans_02.ModifiedBy = obj.UserID;
                            trans_02.ModifiedOn = System.DateTime.Now;
                            trans_02.ModifiedTerminal = obj.TerminalID;

                           
                        }
                        if (obj.ddlTransmode == "ST")
                        {
                            GtDncn03 trans_03 = db.GtDncn03.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear
                           && d.TransactionMode.ToUpper().Replace(" ", "") == obj.TransactionMode.ToUpper().Replace(" ", "") && d.StoreCode == obj.StoreCode).FirstOrDefault();
                            if (trans_03 == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Already Exists" };
                            }

                            trans_03.StartDocNumber = obj.StartDocNumber;
                            trans_03.CurrentDocDate = obj.CurrentDocDate;
                            trans_03.ModifiedBy = obj.UserID;
                            trans_03.ModifiedOn = System.DateTime.Now;
                            trans_03.ModifiedTerminal = obj.TerminalID;
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Updated Successfully." };
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

        public async Task<List<DO_DocumentGeneration>> GetDocumentGenerationsbyBusinessKey(int businesskey,string Transactionmode)

        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (Transactionmode == "GN")
                    {
                        var ds = db.GtDncn01.Where(x => x.BusinessKey == businesskey).Join(db.GtDccnst,
                          x => x.DocumentId,
                          y => y.DocumentId,
                          (x, y) => new DO_DocumentGeneration
                        {
                           BusinessKey = x.BusinessKey,
                           DocumentId = x.DocumentId,
                           FinancialYear= x.FinancialYear,
                           StartDocNumber= x.StartDocNumber,
                           CurrentDocNumber= x.CurrentDocNumber,
                           CurrentDocDate= x.CurrentDocDate,
                           ActiveStatus= x.ActiveStatus,
                           DocumentName=y.DocumentDesc
                          
                       }).ToListAsync();

                        return await ds;
                    }
                    else if(Transactionmode.ToUpper().Replace(" ", "") == "TR".ToUpper().Replace(" ", ""))
                    {
                        var ds = db.GtDncn02.Where(x => x.BusinessKey == businesskey).Join(db.GtDccnst,
                          x => x.DocumentId,
                          y => y.DocumentId,
                          (x, y) => new DO_DocumentGeneration
                          {
                              BusinessKey = x.BusinessKey,
                              DocumentId = x.DocumentId,
                              FinancialYear = x.FinancialYear,
                              StartDocNumber = x.StartDocNumber,
                              CurrentDocNumber = x.CurrentDocNumber,
                              CurrentDocDate = x.CurrentDocDate,
                              ActiveStatus = x.ActiveStatus,
                              DocumentName = y.DocumentDesc,
                              TransactionMode = x.TransactionMode
                          }).ToListAsync();

                        return await ds;
                    }
                    else
                    {
                       
                        var ds = db.GtDncn03.Where(x => x.BusinessKey == businesskey).
                            Join(db.GtDccnst, lkey => new { lkey.DocumentId },
                            ent => new { ent.DocumentId },
                            (lkey, ent) => new { lkey, ent }).Join(db.GtEcstrm,
                            Bloc => new { Bloc.lkey.StoreCode },
                            seg => new { seg.StoreCode },
                            (Bloc, seg) => new { Bloc, seg })
                            .Select(c => new DO_DocumentGeneration
                            {
                                BusinessKey = c.Bloc.lkey.BusinessKey,
                                DocumentId = c.Bloc.lkey.DocumentId,
                                FinancialYear = c.Bloc.lkey.FinancialYear,
                                StartDocNumber = c.Bloc.lkey.StartDocNumber,
                                CurrentDocNumber = c.Bloc.lkey.CurrentDocNumber,
                                CurrentDocDate = c.Bloc.lkey.CurrentDocDate,
                                ActiveStatus = c.Bloc.lkey.ActiveStatus,
                                //TransactionMode= "ST",
                                TransactionMode = c.Bloc.lkey.TransactionMode,
                                StoreName = c.seg.StoreDesc,
                                StoreCode= c.Bloc.lkey.StoreCode,
                                DocumentName = c.Bloc.ent.DocumentDesc

                            }).ToListAsync();
                        return await ds;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveDocumentGeneration(DO_DocumentGeneration obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                       
                        if (obj.ddlTransmode == "GN")
                        {
                            var is_Exists = db.GtDncn01.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear).FirstOrDefault();
                            if (is_Exists == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Not  Exists" };
                            }
                            is_Exists.ActiveStatus = obj.a_status;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            if (obj.a_status == true)
                                return new DO_ReturnParameter() { Status = true, Message = "Document Generation Activated Successfully." };
                            else
                                return new DO_ReturnParameter() { Status = true, Message = "Document Generation De Activated Successfully." };
                        }

                        if (obj.ddlTransmode == "TR")
                        {
                            var is_Exists = db.GtDncn02.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear
                            && d.TransactionMode.ToUpper().Replace(" ", "") == obj.TransactionMode.ToUpper().Replace(" ", "")).FirstOrDefault();
                            if (is_Exists == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Not Exists" };
                            }

                            is_Exists.ActiveStatus = obj.a_status;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            if (obj.a_status == true)
                                return new DO_ReturnParameter() { Status = true, Message = "Document Generation Activated Successfully." };
                            else
                                return new DO_ReturnParameter() { Status = true, Message = "Document Generation De Activated Successfully." };
                        }

                        if (obj.ddlTransmode == "ST")
                        {
                            var is_Exists = db.GtDncn03.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear
                           && d.TransactionMode.ToUpper().Replace(" ", "") == obj.TransactionMode.ToUpper().Replace(" ", "") && d.StoreCode == obj.StoreCode).FirstOrDefault();
                            if (is_Exists == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Not Exists" };
                            }

                            is_Exists.ActiveStatus = obj.a_status;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            if (obj.a_status == true)
                                return new DO_ReturnParameter() { Status = true, Message = "Document Generation Activated Successfully." };
                            else
                                return new DO_ReturnParameter() { Status = true, Message = "Document Generation De Activated Successfully." };
                        }

                        return new DO_ReturnParameter() { Status = false, Message = "Not  Exists" };
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

        #endregion Document Generation 

        #region Calendar Defination
        public async Task<DO_ReturnParameter> InsertCalendarHeaderAndDetails(DO_CalendarDefinition calendarheadar)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        int financialcalr = Convert.ToInt32(calendarheadar.FinancialYear);

                        var isCalendarExists = db.GtEcclco.Where(x => x.FinancialYear == financialcalr && x.BusinessKey == calendarheadar.BusinessKey).FirstOrDefault();

                        if (isCalendarExists == null)
                        {
                            GtEcclco calheader = new GtEcclco();
                            calheader.FinancialYear = Convert.ToInt32(calendarheadar.FinancialYear);
                            calheader.BusinessKey = calendarheadar.BusinessKey;
                            calheader.FromDate = calendarheadar.FromDate;
                            calheader.TillDate = calendarheadar.TillDate;
                            calheader.Status = calendarheadar.Status;
                            calheader.FormId = calendarheadar.FormId;
                            calheader.CreatedBy = calendarheadar.UserID;
                            calheader.CreatedOn = System.DateTime.Now;
                            calheader.CreatedTerminal = calendarheadar.TerminalID;
                            db.GtEcclco.Add(calheader);
                            await db.SaveChangesAsync();
                            List<int> MonthIds = new List<int>();
                            string months;
                            var financeyear = Convert.ToInt32(calendarheadar.FinancialYear);

                            for (int i = calendarheadar.FromDate.Month; i <= calendarheadar.TillDate.Month; i++)
                            {
                                if (i.ToString().Length == 1)
                                {
                                    string strMonth = 0 + i.ToString();
                                    months = financeyear.ToString() + "" + strMonth;
                                }
                                else
                                {
                                    months = financeyear.ToString() + "" + i.ToString();

                                }

                                MonthIds.Add(Convert.ToInt32(months));
                            }

                            GtEccldt caldetails = new GtEccldt();

                            foreach (var month in MonthIds)
                            {
                                var calendardetailsExists = db.GtEccldt.Where(x => x.BusinessKey == calendarheadar.BusinessKey &&
                                  x.FinancialYear == Convert.ToInt32(calendarheadar.FinancialYear) && x.MonthId == month).FirstOrDefault();
                                if (calendardetailsExists != null)
                                {
                                    return new DO_ReturnParameter() { Status = false, Message = "Month is Already Exists for the Financial Year and Business Key." };
                                }
                                else
                                {
                                    caldetails.BusinessKey = calendarheadar.BusinessKey;
                                    caldetails.FinancialYear = Convert.ToInt32(calendarheadar.FinancialYear);
                                    caldetails.MonthId = month;
                                    caldetails.MonthFreezeHis = false;
                                    caldetails.MonthFreezeFin = false;
                                    caldetails.MonthFreezeHr = false;
                                    caldetails.PatientIdgen = 0;
                                    caldetails.PatientIdserial = "0";
                                    caldetails.BudgetMonth = "Q";
                                    caldetails.ActiveStatus = true;
                                    caldetails.FormId = calendarheadar.FormId;
                                    caldetails.CreatedBy = calendarheadar.UserID;
                                    caldetails.CreatedOn = DateTime.Now;
                                    caldetails.CreatedTerminal = calendarheadar.TerminalID;
                                    db.GtEccldt.Add(caldetails);
                                    await db.SaveChangesAsync();
                                }
                            }

                            //var BusinessKeys = db.GtEcbsln.Where(x => x.ActiveStatus == true).ToList();
                            //foreach (var bkey in BusinessKeys)
                            //{
                            //    foreach (var month in MonthIds)
                            //    {
                            //    var calendardetailsExists = db.GtEccldt.Where(x => x.BusinessKey == bkey.BusinessKey &&
                            //      x.FinancialYear == Convert.ToInt32(calendarheadar.FinancialYear) && x.MonthId == month && x.ActiveStatus==true).FirstOrDefault();
                            //    if (calendardetailsExists != null)
                            //    {
                            //        return new DO_ReturnParameter() { Status = false, Message = "Month is Already Exists for the Financial Year and Business Key." };
                            //    }
                            //    else
                            //    {
                            //        caldetails.BusinessKey = bkey.BusinessKey;
                            //        caldetails.FinancialYear = Convert.ToInt32(calendarheadar.FinancialYear);
                            //        caldetails.MonthId = month;
                            //        caldetails.MonthFreezeHis = false;
                            //        caldetails.MonthFreezeFin = false;
                            //        caldetails.MonthFreezeHr = false;
                            //        caldetails.PatientIdgen = 0;
                            //        caldetails.PatientIdserial = "0";
                            //        caldetails.BudgetMonth = "Q";
                            //        caldetails.ActiveStatus = true;
                            //        caldetails.FormId = calendarheadar.FormId;
                            //        caldetails.CreatedBy = calendarheadar.UserID;
                            //        caldetails.CreatedOn = DateTime.Now;
                            //        caldetails.CreatedTerminal = calendarheadar.TerminalID;
                            //        db.GtEccldt.Add(caldetails);
                            //       await db.SaveChangesAsync();
                            //    }
                            //  }
                            //}

                            dbContext.Commit();

                            return new DO_ReturnParameter() { Status = true, Message = "Calender Created Successfully." };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Calendar Already Exists." };
                        }
                    }

                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }




            }
        }

        public async Task<DO_ReturnParameter> UpdateCalendardetails(DO_CalendarDetails caldetails)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var calendardetails = db.GtEccldt.Where(x => x.MonthId == caldetails.MonthId && x.BusinessKey == caldetails.BusinessKey && x.FinancialYear == Convert.ToInt32(caldetails.FinancialYear)).FirstOrDefault();
                        if (calendardetails != null)
                        {
                            calendardetails.MonthFreezeHis = caldetails.MonthFreezeHis;
                            calendardetails.MonthFreezeFin = caldetails.MonthFreezeFin;
                            calendardetails.MonthFreezeHr = caldetails.MonthFreezeHr;
                            //calendardetails.PatientIdgen = caldetails.PatientIdgen;
                            calendardetails.PatientIdserial = caldetails.PatientIdserial;
                            calendardetails.BudgetMonth = caldetails.BudgetMonth;
                            calendardetails.ActiveStatus = caldetails.ActiveStatus;
                            calendardetails.ModifiedBy = caldetails.UserID;
                            calendardetails.ModifiedOn = DateTime.Now;
                            calendardetails.ModifiedTerminal = caldetails.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Calendar details updated Successfully." };
                        }
                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Calendar details not found." };
                        }

                    }

                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public List<DO_CalendarDetails> GetCalendarDetailsbyBusinessKeyAndFinancialYear(int BusinessKey, decimal FinancialYear)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    if (BusinessKey != 0 && FinancialYear == 0)
                    {
                        return GetCalendarDetailsbyBusinessKey(BusinessKey);
                    }
                    var result = db.GtEccldt.Where(h => h.BusinessKey == BusinessKey && h.FinancialYear == FinancialYear).Join(db.GtEcclco,
                     x => x.FinancialYear,
                     y => y.FinancialYear,

                     (x, y) => new DO_CalendarDetails
                     {

                         BusinessKey = x.BusinessKey,
                         FinancialYear = y.FinancialYear,
                         MonthId = x.MonthId,
                         EditMonthId = x.MonthId,
                         MonthFreezeHis = x.MonthFreezeHis,
                         MonthFreezeFin = x.MonthFreezeFin,
                         MonthFreezeHr = x.MonthFreezeHr,
                         PatientIdgen = x.PatientIdgen,
                         PatientIdserial = x.PatientIdserial,
                         BudgetMonth = x.BudgetMonth,
                         ActiveStatus = x.ActiveStatus,
                         Fromdate = y.FromDate,
                         Tilldate = y.TillDate,

                     }).ToList();

                    List<DO_CalendarDetails> CalendarDtailslist = new List<DO_CalendarDetails>();

                    foreach (var item in result)
                    {
                        DO_CalendarDetails objcalendar = new DO_CalendarDetails();
                        objcalendar.BusinessKey = item.BusinessKey;
                        objcalendar.FinancialYear = item.FinancialYear;
                        objcalendar.MonthId = item.MonthId;
                        objcalendar.EditMonthId = item.MonthId;
                        objcalendar.MonthFreezeHis = item.MonthFreezeHis;
                        objcalendar.MonthFreezeFin = item.MonthFreezeFin;
                        objcalendar.MonthFreezeHr = item.MonthFreezeHr;
                        objcalendar.PatientIdgen = item.PatientIdgen;
                        objcalendar.PatientIdserial = item.PatientIdserial;
                        objcalendar.BudgetMonth = item.BudgetMonth;
                        objcalendar.ActiveStatus = item.ActiveStatus;
                        objcalendar.Fromdate = item.Fromdate;
                        objcalendar.Tilldate = item.Tilldate;
                        String Monthlength = item.MonthId.ToString();
                        String firstletter = "";
                        if (!string.IsNullOrEmpty(Monthlength))
                        {
                            firstletter = Monthlength.Remove(0, 4);
                        }

                        if (firstletter == "01")
                        {
                            objcalendar.MonthDescription = "January";


                        }
                        if (firstletter == "02")
                        {
                            objcalendar.MonthDescription = "February";

                        }
                        if (firstletter == "03")
                        {
                            objcalendar.MonthDescription = "March";

                        }
                        if (firstletter == "04")
                        {
                            objcalendar.MonthDescription = "April";

                        }
                        if (firstletter == "05")
                        {
                            objcalendar.MonthDescription = "May";

                        }
                        if (firstletter == "06")
                        {
                            objcalendar.MonthDescription = "June";

                        }
                        if (firstletter == "07")
                        {
                            objcalendar.MonthDescription = "July";

                        }
                        if (firstletter == "08")
                        {
                            objcalendar.MonthDescription = "August";

                        }
                        if (firstletter == "09")
                        {
                            objcalendar.MonthDescription = "September";

                        }
                        if (firstletter == "10")
                        {
                            objcalendar.MonthDescription = "October";

                        }
                        if (firstletter == "11")
                        {
                            objcalendar.MonthDescription = "November";

                        }
                        if (firstletter == "12")
                        {
                            objcalendar.MonthDescription = "December";

                        }
                        CalendarDtailslist.Add(objcalendar);
                    }


                    return CalendarDtailslist.ToList();


                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<DO_CalendarDetails> GetCalendarDetailsbyBusinessKey(int BusinessKey)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEccldt.Where(k => k.BusinessKey == BusinessKey).Join(db.GtEcclco,
                     x => x.FinancialYear,
                     y => y.FinancialYear,

                     (x, y) => new DO_CalendarDetails
                     {

                         BusinessKey = x.BusinessKey,
                         FinancialYear = y.FinancialYear,
                         MonthId = x.MonthId,
                         EditMonthId = x.MonthId,
                         MonthFreezeHis = x.MonthFreezeHis,
                         MonthFreezeFin = x.MonthFreezeFin,
                         MonthFreezeHr = x.MonthFreezeHr,
                         PatientIdgen = x.PatientIdgen,
                         PatientIdserial = x.PatientIdserial,
                         BudgetMonth = x.BudgetMonth,
                         ActiveStatus = x.ActiveStatus,
                         Fromdate = y.FromDate,
                         Tilldate = y.TillDate,

                     }).ToList();

                    List<DO_CalendarDetails> CalendarDtailslist = new List<DO_CalendarDetails>();

                    foreach (var item in result)
                    {
                        DO_CalendarDetails objcalendar = new DO_CalendarDetails();
                        objcalendar.BusinessKey = item.BusinessKey;
                        objcalendar.FinancialYear = item.FinancialYear;
                        objcalendar.MonthId = item.MonthId;
                        objcalendar.EditMonthId = item.MonthId;
                        objcalendar.MonthFreezeHis = item.MonthFreezeHis;
                        objcalendar.MonthFreezeFin = item.MonthFreezeFin;
                        objcalendar.MonthFreezeHr = item.MonthFreezeHr;
                        objcalendar.PatientIdgen = item.PatientIdgen;
                        objcalendar.PatientIdserial = item.PatientIdserial;
                        objcalendar.BudgetMonth = item.BudgetMonth;
                        objcalendar.ActiveStatus = item.ActiveStatus;
                        objcalendar.Fromdate = item.Fromdate;
                        objcalendar.Tilldate = item.Tilldate;
                        String Monthlength = item.MonthId.ToString();
                        String firstletter = "";
                        if (!string.IsNullOrEmpty(Monthlength))
                        {
                            firstletter = Monthlength.Remove(0, 4);
                        }
                        if (firstletter == "01")
                        {
                            objcalendar.MonthDescription = "January";


                        }
                        if (firstletter == "02")
                        {
                            objcalendar.MonthDescription = "February";

                        }
                        if (firstletter == "03")
                        {
                            objcalendar.MonthDescription = "March";

                        }
                        if (firstletter == "04")
                        {
                            objcalendar.MonthDescription = "April";

                        }
                        if (firstletter == "05")
                        {
                            objcalendar.MonthDescription = "May";

                        }
                        if (firstletter == "06")
                        {
                            objcalendar.MonthDescription = "June";

                        }
                        if (firstletter == "07")
                        {
                            objcalendar.MonthDescription = "July";

                        }
                        if (firstletter == "08")
                        {
                            objcalendar.MonthDescription = "August";

                        }
                        if (firstletter == "09")
                        {
                            objcalendar.MonthDescription = "September";

                        }
                        if (firstletter == "10")
                        {
                            objcalendar.MonthDescription = "October";

                        }
                        if (firstletter == "11")
                        {
                            objcalendar.MonthDescription = "November";

                        }
                        if (firstletter == "12")
                        {
                            objcalendar.MonthDescription = "December";

                        }
                        CalendarDtailslist.Add(objcalendar);
                    }


                    return CalendarDtailslist.ToList();


                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<List<DO_CalendarDefinition>> GetCalendarHeadersbyBusinessKey(int Businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcclco.Where(x => x.BusinessKey == Businesskey)

                                 .Select(c => new DO_CalendarDefinition
                                 {
                                     BusinessKey = c.BusinessKey,
                                     FinancialYear = c.FinancialYear,
                                     FromDate = c.FromDate,
                                     TillDate = c.TillDate,
                                     Status = c.Status
                                 }).OrderByDescending(x => x.FinancialYear).ToListAsync();
                    return await result;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_CalendarDefinition>> GetCalendarHeaders()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcclco

                                 .Select(c => new DO_CalendarDefinition
                                 {
                                     BusinessKey = c.BusinessKey,
                                     FinancialYear = c.FinancialYear,
                                     FromDate = c.FromDate,
                                     TillDate = c.TillDate,
                                     Status = c.Status
                                 }).OrderByDescending(x => x.FinancialYear).ToListAsync();
                    return await result;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_CalendarDetails>> GetFinancialYearbyBusinessKey(int Businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEccldt.Where(x => x.BusinessKey == Businesskey)

                                 .Select(c => new DO_CalendarDetails
                                 {
                                     FinancialYear = c.FinancialYear

                                 }).OrderByDescending(x => x.FinancialYear).Distinct().ToListAsync();
                    return await result;


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion Calendar Defination

        #region Document Control Normal Mode 

        public async Task<DO_ReturnParameter> InsertDocumentControlNormalMode(DO_DocumentControlNormalMode obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                            var is_Exists = db.GtDncn01.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear).FirstOrDefault();
                            if (is_Exists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Already Exists" };
                            }

                            var normal_mode = new GtDncn01
                            {
                                BusinessKey = obj.BusinessKey,
                                DocumentId = obj.DocumentId,
                                FinancialYear = obj.FinancialYear,
                                StartDocNumber = obj.StartDocNumber,
                                //CurrentDocNumber = obj.CurrentDocNumber,
                                CurrentDocNumber = obj.StartDocNumber,
                                //CurrentDocDate = DateTime.Now,
                                CurrentDocDate = obj.CurrentDocDate,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtDncn01.Add(normal_mode);
                        await db.SaveChangesAsync();
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

        public async Task<DO_ReturnParameter> UpdateDocumentControlNormalMode(DO_DocumentControlNormalMode obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        
                            GtDncn01 trans_01 = db.GtDncn01.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear).FirstOrDefault();
                            if (trans_01 == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Not Exists" };
                            }
                            trans_01.StartDocNumber = obj.StartDocNumber;
                            trans_01.CurrentDocDate = obj.CurrentDocDate;
                            trans_01.ModifiedBy = obj.UserID;
                            trans_01.ModifiedOn = System.DateTime.Now;
                            trans_01.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Updated Successfully." };
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

        public async Task<List<DO_DocumentControlNormalMode>> GetDocumentControlNormalModebyBusinessKey(int businesskey)

        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    
                        var ds = db.GtDncn01.Where(x => x.BusinessKey == businesskey).Join(db.GtDccnst,
                          x => x.DocumentId,
                          y => y.DocumentId,
                          (x, y) => new DO_DocumentControlNormalMode
                          {
                              BusinessKey = x.BusinessKey,
                              DocumentId = x.DocumentId,
                              FinancialYear = x.FinancialYear,
                              StartDocNumber = x.StartDocNumber,
                              CurrentDocNumber = x.CurrentDocNumber,
                              CurrentDocDate = x.CurrentDocDate,
                              ActiveStatus = x.ActiveStatus,
                              DocumentName = y.DocumentDesc

                          }).ToListAsync();

                        return await ds;
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveDocumentControlNormalMode(DO_DocumentControlNormalMode obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                         var is_Exists = db.GtDncn01.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear).FirstOrDefault();
                            if (is_Exists == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Not  Exists" };
                            }
                            is_Exists.ActiveStatus = obj.a_status;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            if (obj.a_status == true)
                                return new DO_ReturnParameter() { Status = true, Message = "Activated Successfully." };
                            else
                                return new DO_ReturnParameter() { Status = true, Message = "De Activated Successfully." };
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

        #endregion Document Control Normal Mode  

        #region Document Control Transaction Mode  

        public async Task<DO_ReturnParameter> InsertDocumentControlTransactionMode(DO_DocumentControlTransaction obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                            var is_Exists = db.GtDncn02.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear
                            && d.TransactionMode.ToUpper().Replace(" ", "") == obj.TransactionMode.ToUpper().Replace(" ", "")).FirstOrDefault();
                            if (is_Exists != null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Already Exists" };
                            }

                            var dc_trns = new GtDncn02
                            {
                                BusinessKey = obj.BusinessKey,
                                DocumentId = obj.DocumentId,
                                FinancialYear = obj.FinancialYear,
                                TransactionMode = obj.TransactionMode,
                                StartDocNumber = obj.StartDocNumber,
                                //CurrentDocNumber = obj.CurrentDocNumber,
                                CurrentDocNumber = obj.StartDocNumber,
                                //CurrentDocDate = DateTime.Now,
                                CurrentDocDate = obj.CurrentDocDate,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtDncn02.Add(dc_trns);
                        await db.SaveChangesAsync();
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

        public async Task<DO_ReturnParameter> UpdateDocumentControlTransactionMode(DO_DocumentControlTransaction obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                            GtDncn02 trans_02 = db.GtDncn02.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear
                            && d.TransactionMode.ToUpper().Replace(" ", "") == obj.TransactionMode.ToUpper().Replace(" ", "")).FirstOrDefault();
                            if (trans_02 == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Not Exists" };
                            }

                            trans_02.StartDocNumber = obj.StartDocNumber;
                            trans_02.CurrentDocDate = obj.CurrentDocDate;
                            trans_02.ModifiedBy = obj.UserID;
                            trans_02.ModifiedOn = System.DateTime.Now;
                            trans_02.ModifiedTerminal = obj.TerminalID;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Updated Successfully." };
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

        public async Task<List<DO_DocumentGeneration>> GetDocumentControlTransactionModebyBusinessKey(int businesskey)

        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                  
                   
                        var ds = db.GtDncn02.Where(x => x.BusinessKey == businesskey).Join(db.GtDccnst,
                          x => x.DocumentId,
                          y => y.DocumentId,
                          (x, y) => new DO_DocumentGeneration
                          {
                              BusinessKey = x.BusinessKey,
                              DocumentId = x.DocumentId,
                              FinancialYear = x.FinancialYear,
                              StartDocNumber = x.StartDocNumber,
                              CurrentDocNumber = x.CurrentDocNumber,
                              CurrentDocDate = x.CurrentDocDate,
                              ActiveStatus = x.ActiveStatus,
                              DocumentName = y.DocumentDesc,
                              TransactionMode = x.TransactionMode
                          }).ToListAsync();

                        return await ds;
                }
                  
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> ActiveOrDeActiveDocumentControlTransactionMode(DO_DocumentControlTransaction obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                            var is_Exists = db.GtDncn02.Where(d => d.DocumentId == obj.DocumentId && d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear
                            && d.TransactionMode.ToUpper().Replace(" ", "") == obj.TransactionMode.ToUpper().Replace(" ", "")).FirstOrDefault();
                            if (is_Exists == null)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Not Exists" };
                            }

                            is_Exists.ActiveStatus = obj.a_status;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            if (obj.a_status == true)
                                return new DO_ReturnParameter() { Status = true, Message = "Activated Successfully." };
                            else
                                return new DO_ReturnParameter() { Status = true, Message = "De Activated Successfully." };
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

        #endregion Document Control Transaction Mode  
    }
}

