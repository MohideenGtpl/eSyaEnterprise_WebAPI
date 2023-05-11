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
    public class FormsRepository : IFormsRepository
    {
        public async Task<List<DO_AreaController>> GetAreaController()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEbecnt
                                  .Where(w => w.ActiveStatus)
                                  .Select(r => new DO_AreaController
                                  {
                                      Area = r.Area,
                                      Controller = r.Controller,
                                  }).OrderBy(o => o.Area).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_Forms>> GetFormDetails()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcfmfd
                                  .Where(w => w.ActiveStatus)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.FormId,
                                      FormCode = r.FormCode,
                                      FormName = r.FormName,
                                      ControllerName = r.ControllerName
                                  }).OrderBy(o => o.FormCode).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_Forms>> GetInternalFormDetails()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcfmnm
                                  // .Where(w => w.ActiveStatus)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.FormId,
                                      InternalFormNumber = r.FormIntId,
                                      FormName = r.FormDescription,
                                      NavigateURL = r.NavigateUrl
                                  }).ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_Forms> GetFormDetailsByID(int formID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcfmfd
                                  .Where(w => w.FormId == formID && w.ActiveStatus)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.FormId,
                                      FormCode = r.FormCode,
                                      FormName = r.FormName,
                                      ControllerName = r.ControllerName,
                                      ToolTip = r.ToolTip,
                                      //IsDocumentNumberRequired = r.IsDocumentNumberRequired,
                                      //IsStoreLink = r.IsStoreLink,
                                      //IsDoctor = r.IsDoctor,
                                      //l_FormParameter = r.GtEcfmpa.Where(w=>w.FormId == formID).Select(p => new DO_eSyaParameter
                                      //{
                                      //    ParameterID = p.ParameterId,
                                      //    ParmAction = p.ParmAction
                                      //}).ToList()
                                  }).FirstOrDefaultAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_Forms>> GetInternalFormByFormID(int formID)
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var result = db.GtEcfmnm
                                  .Where(w => w.FormId == formID)
                                  .Select(r => new DO_Forms
                                  {
                                      FormID = r.FormId,
                                      InternalFormNumber = r.FormIntId,
                                      NavigateURL = r.NavigateUrl,
                                      FormDescription = r.FormDescription,
                                      ActiveStatus = r.ActiveStatus,
                                  }).Distinct().ToListAsync();
                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertUpdateIntoFormMaster(DO_Forms obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        if (obj.IsInsert)
                        {
                            if (db.GtEcfmfd.Where(w => w.FormCode == obj.FormCode).Count() > 0)
                            {
                                return new DO_ReturnParameter() { Status = false, Message = "Form Code Already Exists" };
                            }
                        }
                        //   if (db.GtEcfmfd.Where(w => w.FormId == obj.FormID).Count() <= 0)
                        if (obj.IsInsert)
                        {
                            obj.FormID = db.GtEcfmfd.Select(s => s.FormId).DefaultIfEmpty(0).Max()+1;
                            var ag = new GtEcfmfd
                            {
                                FormId = obj.FormID,
                                FormCode = obj.FormCode,
                                FormName = obj.FormName,
                                ControllerName = obj.ControllerName,
                                ToolTip = obj.ToolTip,
                                //IsDocumentNumberRequired = obj.IsDocumentNumberRequired,
                                //IsStoreLink = obj.IsStoreLink,
                                //IsDoctor = obj.IsDoctor,
                                ActiveStatus = true,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcfmfd.Add(ag);

                            var formNumber = db.GtEcfmnm.Where(w => w.FormId == obj.FormID).Count();
                            string internalFormID = obj.FormCode.ToString() + "_00";
                            obj.NavigateURL = obj.ControllerName + "/" + internalFormID;
                            if (formNumber <= 0)
                            {
                                formNumber = formNumber + 1;
                                GtEcfmnm fn = new GtEcfmnm
                                {
                                    FormId = obj.FormID,
                                    FormIntId = internalFormID,
                                    NavigateUrl = obj.NavigateURL,
                                    FormDescription = "Standard",
                                    ActiveStatus = obj.ActiveStatus,
                                    CreatedOn = DateTime.Now,
                                    CreatedTerminal = obj.TerminalID,
                                    CreatedBy = obj.UserID,

                                };
                                db.GtEcfmnm.Add(fn);
                            }
                        }
                        else
                        {
                            var ag = db.GtEcfmfd.Where(w => w.FormId == obj.FormID).FirstOrDefault();
                            ag.FormName = obj.FormName;
                            ag.ToolTip = obj.ToolTip;
                            //ag.IsDocumentNumberRequired = obj.IsDocumentNumberRequired;
                            //ag.IsStoreLink = obj.IsStoreLink;
                            //ag.IsDoctor = obj.IsDoctor;
                            ag.ModifiedBy = obj.UserID;
                            ag.ModifiedOn = System.DateTime.Now;
                            ag.ModifiedTerminal = obj.TerminalID;
                        }



                        //if (obj.IsStoreLink)
                        //{
                        //    foreach (var p in obj.l_FormParameter)
                        //    {
                        //        var fp = db.GtEcfmpa.Where(w => w.FormId == obj.FormID && w.ParameterId == p.ParameterID).FirstOrDefault();
                        //        if (fp == null)
                        //        {
                        //            fp = new GtEcfmpa
                        //            {
                        //                FormId = obj.FormID,
                        //                ParameterId = p.ParameterID,
                        //                ParmAction = p.ParmAction,
                        //                ActiveStatus = obj.ActiveStatus,
                        //                CreatedOn = DateTime.Now,
                        //                CreatedTerminal = obj.TerminalID,
                        //                CreatedBy = obj.UserID,
                        //            };
                        //            db.GtEcfmpa.Add(fp);
                        //        }
                        //        else
                        //        {
                        //            fp.ParmAction = p.ParmAction;
                        //            fp.ActiveStatus = obj.ActiveStatus;
                        //            fp.ModifiedBy = obj.UserID;
                        //            fp.ModifiedOn = System.DateTime.Now;
                        //            fp.ModifiedTerminal = obj.TerminalID;
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    await db.GtEcfmpa.Where(w => w.FormId == obj.FormID)
                        //    .ForEachAsync(
                        //        a =>
                        //        {
                        //            a.ParmAction = false;
                        //        }
                        //    );
                        //}

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, ID = obj.FormID, Message = "Created/Updated Successfully" };
                    }
                    catch (DbUpdateException ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                    catch (Exception ex)
                    {
                        dbContext.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public async Task<DO_Forms> GetNextInternalFormByID(int formID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    DO_Forms obj = new DO_Forms();
                    var fm = await db.GtEcfmfd.Where(w => w.FormId == formID).FirstOrDefaultAsync();

                    int formNo = db.GtEcfmnm.Where(w => w.FormId == formID).Count();
                    while(true)
                    {
                        string internalFormNo = fm.FormCode.ToString() + "_"+ formNo.ToString().PadLeft(2,'0');
                        obj.InternalFormNumber = internalFormNo;
                        obj.NavigateURL = fm.ControllerName + "/" + internalFormNo;

                        var f = await db.GtEcfmnm.Where(w => w.FormId == formID && w.NavigateUrl == obj.NavigateURL).FirstOrDefaultAsync();
                        if (f != null)
                        {
                            formNo++;
                            continue;
                        }
                        else
                            break;
                    }

                    return obj;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoInternalForm(DO_Forms obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var fm = db.GtEcfmfd.Where(w => w.FormId == obj.FormID).FirstOrDefault();
                        obj.NavigateURL = fm.ControllerName + "/" + obj.InternalFormNumber;

                        var fn = db.GtEcfmnm.Where(w => w.FormId == obj.FormID && w.FormIntId == obj.InternalFormNumber).FirstOrDefault();
                        if (fn != null)
                        {
                            fn.NavigateUrl = obj.NavigateURL;
                            fn.FormDescription = obj.FormDescription;
                            fn.ActiveStatus = obj.ActiveStatus;
                            fn.ModifiedBy = obj.UserID;
                            fn.ModifiedOn = DateTime.Now;
                            fn.ModifiedTerminal = obj.TerminalID;
                        }
                        else
                        {
                            fn = new GtEcfmnm
                            {
                                FormId = obj.FormID,
                                FormIntId = obj.InternalFormNumber,
                                NavigateUrl = obj.NavigateURL,
                                FormDescription = obj.FormDescription,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedOn = DateTime.Now,
                                CreatedTerminal = obj.TerminalID,
                                CreatedBy = obj.UserID,

                            };
                            db.GtEcfmnm.Add(fn);
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

        public async Task<List<DO_FormAction>> GetFormAction()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var fa = db.GtEcfmac
                        .Select(r => new DO_FormAction
                        {
                            ActionId = r.ActionId,
                            ActionDesc = r.ActionDesc,
                            ActiveStatus = r.ActiveStatus,
                        }).ToListAsync();
                    return await fa;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_FormAction>> GetFormActionByID(int formID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var fa = db.GtEcfmac
                        .GroupJoin(db.GtEcfmal.Where(w => w.FormId == formID),
                        a => a.ActionId,
                        f => f.ActionId,
                        (a, f) => new { a, f = f.FirstOrDefault() })
                        .Select(r => new DO_FormAction
                        {
                            ActionId = r.a.ActionId,
                            ActionDesc = r.a.ActionDesc,
                            ActiveStatus = r.f != null ? r.f.ActiveStatus : false,
                        }).ToListAsync();
                    return await fa;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DO_ReturnParameter> InsertIntoFormAction(DO_Forms obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var fa = db.GtEcfmal.Where(w => w.FormId == obj.FormID);
                        foreach (GtEcfmal f in fa)
                        {
                            f.ActiveStatus = false;
                            f.ModifiedBy = obj.UserID;
                            f.ModifiedOn = System.DateTime.Now;
                            f.ModifiedTerminal = obj.TerminalID;
                        }
                        await db.SaveChangesAsync();

                        if (obj.l_FormAction != null)
                        {
                            foreach (DO_FormAction i in obj.l_FormAction)
                            {
                                var obj_FA = db.GtEcfmal.Where(w => w.FormId == obj.FormID && w.ActionId == i.ActionId).FirstOrDefault();
                                if (obj_FA != null)
                                {
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.ModifiedBy = obj.UserID;
                                    obj_FA.ModifiedOn = DateTime.Now;
                                    obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                }
                                else
                                {
                                    obj_FA = new GtEcfmal();
                                    obj_FA.FormId = obj.FormID;
                                    obj_FA.ActionId = i.ActionId;
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.CreatedBy = obj.UserID;
                                    obj_FA.CreatedOn = DateTime.Now;
                                    obj_FA.CreatedTerminal = System.Environment.MachineName;
                                    db.GtEcfmal.Add(obj_FA);
                                }
                            }
                            await db.SaveChangesAsync();
                        }


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

        public async Task<List<DO_FormParameter>> GetFormParameterByID(int formID)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var fa = db.GtEcparm.Where(w=>w.ParameterType == 13)
                        .GroupJoin(db.GtEcfmpa.Where(w => w.FormId == formID),
                        a => a.ParameterId,
                        f => f.ParameterId,
                        (a, f) => new { a, f = f.FirstOrDefault() })
                        .Select(r => new DO_FormParameter
                        {
                            ParameterId = r.a.ParameterId,
                            ParameterDesc = r.a.ParameterDesc,
                            ActiveStatus = r.f != null ? r.f.ActiveStatus : false,
                        }).ToListAsync();
                    return await fa;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DO_ReturnParameter> InsertIntoFormParameter(DO_Forms obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var fa = db.GtEcfmpa.Where(w => w.FormId == obj.FormID);
                        foreach (GtEcfmpa f in fa)
                        {
                            f.ActiveStatus = false;
                            f.ModifiedBy = obj.UserID;
                            f.ModifiedOn = System.DateTime.Now;
                            f.ModifiedTerminal = obj.TerminalID;
                        }
                        await db.SaveChangesAsync();

                        if (obj.l_FormParameter != null)
                        {
                            foreach (DO_FormParameter i in obj.l_FormParameter)
                            {
                                var obj_FA = db.GtEcfmpa.Where(w => w.FormId == obj.FormID && w.ParameterId == i.ParameterId).FirstOrDefault();
                                if (obj_FA != null)
                                {
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.ModifiedBy = obj.UserID;
                                    obj_FA.ModifiedOn = DateTime.Now;
                                    obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                }
                                else
                                {
                                    obj_FA = new GtEcfmpa();
                                    obj_FA.FormId = obj.FormID;
                                    obj_FA.ParameterId = i.ParameterId;
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.CreatedBy = obj.UserID;
                                    obj_FA.CreatedOn = DateTime.Now;
                                    obj_FA.CreatedTerminal = System.Environment.MachineName;
                                    db.GtEcfmpa.Add(obj_FA);
                                }
                            }
                            await db.SaveChangesAsync();
                        }


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

        public async Task<List<DO_FormSubParameter>> GetFormSubParameterByID(int formID, int parameterId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    if (parameterId == 1)
                    {
                        return await db.GtEcparm.Where(w => w.ParameterType == 8)
                              .GroupJoin(db.GtEcfmps.Where(w => w.FormId == formID && w.ParameterId == parameterId),
                              a => new { a.ParameterId },
                              f => new { ParameterId = f.SubParameterId },
                              (a, f) => new { a, f = f.FirstOrDefault() })
                              .Select(r => new DO_FormSubParameter
                              {
                                  SubParameterId = r.a.ParameterId,
                                  SubParameterDesc = r.a.ParameterDesc,
                                  ActiveStatus = r.f != null ? r.f.ActiveStatus : false,
                              }).ToListAsync();
                    }
                    else
                    {
                        return await db.GtEcparm.Where(w => w.ParameterType == 13 && w.ParameterId == parameterId)
                          .GroupJoin(db.GtEcfmpa.Where(w => w.FormId == formID && w.ParameterId == parameterId),
                          a => a.ParameterId,
                          f => f.ParameterId,
                          (a, f) => new { a, f = f.FirstOrDefault() })
                          .Select(r => new DO_FormSubParameter
                          {
                              SubParameterId = r.a.ParameterId,
                              SubParameterDesc = r.a.ParameterDesc,
                              ActiveStatus = r.f != null ? r.f.ActiveStatus : false,
                          }).ToListAsync();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<DO_ReturnParameter> InsertIntoFormSubParameter(DO_Forms obj)
        {
            using (eSyaEnterprise db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        int parameterId = obj.ParameterId;
                        var fa = db.GtEcfmps.Where(w => w.FormId == obj.FormID && w.ParameterId == parameterId);
                        foreach (GtEcfmps f in fa)
                        {
                            f.ParmAction = true;
                            f.ActiveStatus = false;
                            f.ModifiedBy = obj.UserID;
                            f.ModifiedOn = System.DateTime.Now;
                            f.ModifiedTerminal = obj.TerminalID;
                        }
                        await db.SaveChangesAsync();

                        var obj_P = db.GtEcfmpa.Where(w => w.FormId == obj.FormID && w.ParameterId == parameterId).FirstOrDefault();
                        if (obj_P != null)
                        {
                            obj_P.ActiveStatus = false;
                            obj_P.ModifiedBy = obj.UserID;
                            obj_P.ModifiedOn = DateTime.Now;
                            obj_P.ModifiedTerminal = System.Environment.MachineName;
                        }

                        if (obj.l_FormSubParameter != null)
                        {
                            if (obj_P != null)
                            {
                                obj_P.ActiveStatus = true;
                                obj_P.ModifiedBy = obj.UserID;
                                obj_P.ModifiedOn = DateTime.Now;
                                obj_P.ModifiedTerminal = System.Environment.MachineName;
                            }
                            else
                            {
                                obj_P = new GtEcfmpa();
                                obj_P.FormId = obj.FormID;
                                obj_P.ParameterId = parameterId;
                                obj_P.ActiveStatus = true;
                                obj_P.CreatedBy = obj.UserID;
                                obj_P.CreatedOn = DateTime.Now;
                                obj_P.CreatedTerminal = System.Environment.MachineName;
                                db.GtEcfmpa.Add(obj_P);
                            }

                            foreach (DO_FormSubParameter i in obj.l_FormSubParameter)
                            {
                                var obj_FA = db.GtEcfmps.Where(w => w.FormId == obj.FormID && w.ParameterId == parameterId && w.SubParameterId == i.SubParameterId).FirstOrDefault();
                                if (obj_FA != null)
                                {
                                    obj_FA.ParmAction = true;
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.ModifiedBy = obj.UserID;
                                    obj_FA.ModifiedOn = DateTime.Now;
                                    obj_FA.ModifiedTerminal = System.Environment.MachineName;
                                }
                                else
                                {
                                    obj_FA = new GtEcfmps();
                                    obj_FA.FormId = obj.FormID;
                                    obj_FA.ParameterId = parameterId;
                                    obj_FA.SubParameterId = i.SubParameterId;
                                    obj_FA.ParmAction = true;
                                    obj_FA.ActiveStatus = true;
                                    obj_FA.CreatedBy = obj.UserID;
                                    obj_FA.CreatedOn = DateTime.Now;
                                    obj_FA.CreatedTerminal = System.Environment.MachineName;
                                    db.GtEcfmps.Add(obj_FA);
                                }
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

        #region Form Module
        //public List<DO_Forms> GetForms()
        //{
        //    try
        //    {
        //        using (eSyaEnterprise db = new eSyaEnterprise())
        //        {
        //            var Allforms = db.GtEcfmfd.Where(w => w.ActiveStatus).Select(x => new DO_Forms
        //            {
        //                FormID = x.FormId,
        //                FormName = x.FormName
        //            }).ToList();

        //            var Existsforms = db.GtEacm01.ToList();

        //            if (Existsforms.Count() > 0)
        //            {
        //                foreach (var form in Existsforms)
        //                {
        //                    var exits = Allforms.Where(x => x.FormID == form.FormId).FirstOrDefault();

        //                    if (exits != null)
        //                    {
        //                        Allforms.Remove(exits);
        //                    }

        //                }
        //            }
        //            return Allforms.ToList();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<DO_FormModuleConfiguration> GetFormModules()
        //{
        //    try
        //    {
        //        using (eSyaEnterprise db = new eSyaEnterprise())
        //        {
        //            DO_FormModuleConfiguration mn = new DO_FormModuleConfiguration();

        //            mn.l_Module = await db.GtEcapcd.Where(w => w.ActiveStatus == true && w.CodeType == 1)
        //                            .Select(m => new DO_ApplicationCodes()
        //                            {
        //                                ApplicationCode = m.ApplicationCode,
        //                                CodeDesc = m.CodeDesc,
        //                                ActiveStatus = m.ActiveStatus
        //                            }).ToListAsync();

        //            mn.l_Form = await db.GtEacm01.Join(db.GtEcfmfd,
        //              x => x.FormId,
        //              y => y.FormId,
        //              (x, y) => new DO_FormModule

        //              {
        //                  FormId = x.FormId,
        //                  ModuleId = x.ModuleId,
        //                  TransactionTable = x.TransactionTable,
        //                  RefferedTable = x.RefferedTable,
        //                  ReferenceLink = x.ReferenceLink,
        //                  Description = x.Description,
        //                  AssignedTo = x.AssignedTo,
        //                  AssignedOn = x.AssignedOn,
        //                  Status = x.Status,
        //                  ActiveStatus = x.ActiveStatus,
        //                  FormName = y.FormName
        //              }).ToListAsync();

        //            return mn;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<DO_FormModule> GetFormModulebyFormId(int formId)
        //{
        //    try
        //    {
        //        using (var db = new eSyaEnterprise())
        //        {
        //            var fm = db.GtEacm01.Where(x => x.FormId == formId).Join(db.GtEcfmfd,
        //                x => x.FormId,
        //                y => y.FormId,
        //                (x, y) => new DO_FormModule

        //                {
        //                    FormId = x.FormId,
        //                    ModuleId = x.ModuleId,
        //                    TransactionTable = x.TransactionTable,
        //                    RefferedTable = x.RefferedTable,
        //                    ReferenceLink = x.ReferenceLink,
        //                    Description = x.Description,
        //                    AssignedTo = x.AssignedTo,
        //                    AssignedOn = x.AssignedOn,
        //                    Status = x.Status,
        //                    ActiveStatus = x.ActiveStatus,
        //                    FormName = y.FormName
        //                }).FirstOrDefaultAsync();
        //            return await fm;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public async Task<DO_ReturnParameter> InsertIntoFormModule(DO_FormModule obj)
        //{
        //    using (eSyaEnterprise db = new eSyaEnterprise())
        //    {
        //        using (var dbContext = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var isExists = db.GtEacm01.Where(w => w.FormId == obj.FormId).FirstOrDefault();
        //                if (isExists != null)
        //                {
        //                    return new DO_ReturnParameter() { Status = false, Message = "Form Module is already Exists" };
        //                }
        //                else
        //                {
        //                    var fm = new GtEacm01
        //                    {
        //                        FormId = obj.FormId,
        //                        ModuleId = obj.ModuleId,
        //                        TransactionTable = obj.TransactionTable,
        //                        RefferedTable = obj.RefferedTable,
        //                        ReferenceLink = obj.ReferenceLink,
        //                        Description = obj.Description,
        //                        AssignedTo = obj.AssignedTo,
        //                        AssignedOn = obj.AssignedOn,
        //                        Status = obj.Status,
        //                        ActiveStatus = obj.ActiveStatus,
        //                        CreatedBy = obj.UserID,
        //                        CreatedOn = DateTime.Now,
        //                        CreatedTerminal = obj.TerminalID
        //                    };
        //                    db.GtEacm01.Add(fm);
        //                    await db.SaveChangesAsync();
        //                    dbContext.Commit();
        //                    return new DO_ReturnParameter() { Status = true, Message = "Form Module Created Successfully" };
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                dbContext.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }
        //}

        //public async Task<DO_ReturnParameter> UpdateFormModule(DO_FormModule obj)
        //{
        //    using (eSyaEnterprise db = new eSyaEnterprise())
        //    {
        //        using (var dbContext = db.Database.BeginTransaction())
        //        {
        //            try
        //            {
        //                var fm = db.GtEacm01.Where(w => w.FormId == obj.FormId).FirstOrDefault();
        //                if (fm != null)
        //                {
        //                    fm.FormId = obj.FormId;
        //                    fm.ModuleId = obj.ModuleId;
        //                    fm.TransactionTable = obj.TransactionTable;
        //                    fm.RefferedTable = obj.RefferedTable;
        //                    fm.ReferenceLink = obj.ReferenceLink;
        //                    fm.Description = obj.Description;
        //                    fm.AssignedTo = obj.AssignedTo;
        //                    fm.AssignedOn = obj.AssignedOn;
        //                    fm.Status = obj.Status;
        //                    fm.ActiveStatus = obj.ActiveStatus;
        //                    fm.ModifiedBy = obj.UserID;
        //                    fm.ModifiedOn = DateTime.Now;
        //                    fm.ModifiedTerminal = obj.TerminalID;
        //                    await db.SaveChangesAsync();
        //                    dbContext.Commit();
        //                    return new DO_ReturnParameter() { Status = true, Message = "Form Module Updated Successfully" };
        //                }
        //                else
        //                {
        //                    return new DO_ReturnParameter() { Status = false, Message = "Form Module doesn't Exists" };
        //                }

        //            }
        //            catch (Exception ex)
        //            {
        //                dbContext.Rollback();
        //                throw ex;
        //            }
        //        }
        //    }
        //}

        #endregion Form Module

        #region Area Controller
        public async Task<List<DO_AreaController>> GetAllAreaController()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEbecnt
                        .Select(a => new DO_AreaController
                        {
                            Id = a.Id,
                            Area = a.Area,
                            Controller = a.Controller,
                            ActiveStatus = a.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoAreaController(DO_AreaController obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        //int maxId = db.GtEbecnt.Select(a => a.Id).DefaultIfEmpty().Max();
                        //int Ar_Id = maxId + 1;
                        //Here Id is Identity column

                        var ar_ct = new GtEbecnt
                        {
                           //Id=Ar_Id,
                           Area=obj.Area,
                           Controller=obj.Controller,
                           ActiveStatus=obj.ActiveStatus
                        };
                        db.GtEbecnt.Add(ar_ct);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Area Created Successfully" };
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

        public async Task<DO_ReturnParameter> UpdateAreaController(DO_AreaController obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                       
                        GtEbecnt ar_ct = db.GtEbecnt.Where(x => x.Id == obj.Id).FirstOrDefault();
                        if (ar_ct == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Area is not exist" };
                        }

                        ar_ct.Area = obj.Area;
                        ar_ct.Controller = obj.Controller;
                        ar_ct.ActiveStatus = obj.ActiveStatus;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = " Area Updated Successfully." };
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveAreaController(bool status, int Id)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEbecnt ar_ct = db.GtEbecnt.Where(w => w.Id == Id).FirstOrDefault();
                        if (ar_ct == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Area is not exist" };
                        }

                        ar_ct.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = " Area Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = " Area De Activated Successfully." };
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
    }
}
