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
    public class ProcessMasterRepository:IProcessMasterRepository
    {
        #region Process Master

        public async Task<List<DO_ProcessMaster>> GetProcessMaster()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcprrl
                        .Select(r => new DO_ProcessMaster
                        {
                            ProcessId = r.ProcessId,
                            ProcessDesc = r.ProcessDesc,
                            IsSegmentSpecific = r.IsSegmentSpecific,
                            SystemControl = r.SystemControl,
                            ProcessControl = r.ProcessControl,
                            ActiveStatus = r.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoProcessMaster(DO_ProcessMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        bool is_ProcessIdExist = db.GtEcprrl.Any(a => a.ProcessId == obj.ProcessId);
                        if (is_ProcessIdExist)
                        {
                            return  new  DO_ReturnParameter() { Status = false, Message = "Process Id is already exist." };
                        }

                        var is_ProcessDescExist = db.GtEcprrl.Where(a => a.ProcessDesc.Trim().ToUpper().Replace(" ", "") == obj.ProcessDesc.Trim().ToUpper().Replace(" ", "")).FirstOrDefault();
                               
                        if (is_ProcessDescExist !=null)
                        {
                             return new DO_ReturnParameter() { Status = false, Message = "Process Desc is already exist." };
                        }

                        var pr_ms = new GtEcprrl
                        {
                            ProcessId = obj.ProcessId,
                            ProcessDesc = obj.ProcessDesc,
                            IsSegmentSpecific = obj.IsSegmentSpecific,
                            SystemControl = obj.SystemControl,
                            ProcessControl = obj.ProcessControl,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcprrl.Add(pr_ms);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Process Master Created Successfully." };
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

        public async Task<DO_ReturnParameter> UpdateProcessMaster(DO_ProcessMaster obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_ProcessDescExist = db.GtEcprrl.Where(w => w.ProcessDesc.Trim().ToUpper().Replace(" ", "") == obj.ProcessDesc.Trim().ToUpper().Replace(" ", "")
                                && w.ProcessId != obj.ProcessId).Count();
                        if (is_ProcessDescExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Process Desc is already exist." };
                        }

                        GtEcprrl pr_ms = db.GtEcprrl.Where(w => w.ProcessId == obj.ProcessId).FirstOrDefault();
                        if (pr_ms == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Process Id is not exist" };
                        }

                        pr_ms.ProcessDesc = obj.ProcessDesc;
                        pr_ms.IsSegmentSpecific = obj.IsSegmentSpecific;
                        pr_ms.SystemControl = obj.SystemControl;
                        pr_ms.ProcessControl = obj.ProcessControl;
                        pr_ms.ActiveStatus = obj.ActiveStatus;
                        pr_ms.ModifiedBy = obj.UserID;
                        pr_ms.ModifiedOn = System.DateTime.Now;
                        pr_ms.ModifiedTerminal = obj.TerminalID;

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Process Updated Successfully." };
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

        #region Process Rule

        public async Task<List<DO_ProcessRule>> GetProcessRule()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcaprl
                        .Select(r => new DO_ProcessRule
                        {
                            ProcessId = r.ProcessId,
                            RuleId = r.RuleId,
                            RuleDesc = r.RuleDesc,
                            Notes = r.Notes,
                            ActiveStatus = r.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ProcessMaster>> GetActiveProcessMaster()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcprrl.Where(w => w.ActiveStatus==true)
                        .Select(r => new DO_ProcessMaster
                        {
                            ProcessId = r.ProcessId,
                            ProcessDesc = r.ProcessDesc,

                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ProcessRule>> GetProcessRuleByProcessId(int processId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcaprl.Where(w => w.ProcessId == processId)
                        .Select(r => new DO_ProcessRule
                        {
                            ProcessId = r.ProcessId,
                            RuleId = r.RuleId,
                            RuleDesc = r.RuleDesc,
                            Notes = r.Notes,
                            ActiveStatus = r.ActiveStatus
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoProcessRule(DO_ProcessRule obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_RuleDescExist = db.GtEcaprl.Where(w => w.RuleDesc.Trim().ToUpper().Replace(" ", "") == obj.RuleDesc.Trim().ToUpper().Replace(" ", "")
                                && w.ProcessId == obj.ProcessId).Count();
                        if (is_RuleDescExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Process Rule is already exist." };
                        }
                        bool is_RuleIdExist = db.GtEcaprl.Any(a => a.RuleId == obj.RuleId &&a.ProcessId==obj.ProcessId);
                        if (is_RuleIdExist)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "The combination of Process Id and Rule Id is already exist." };
                        }
                        if (obj.ActiveStatus)
                        {
                            var pr =  db.GtEcprrl.Where(w => w.ProcessId == obj.ProcessId).FirstOrDefault();
                            if (pr.ProcessControl == "S")
                            {
                                if (db.GtEcaprl.Where(w => w.ProcessId == obj.ProcessId && w.ActiveStatus).Count() > 0)
                                    return new DO_ReturnParameter() { Status = false, Message = "Only one rule will be Active" };
                            }
                        }
                       
                        var pr_ru = new GtEcaprl
                        {
                            RuleId = obj.RuleId,
                            ProcessId = obj.ProcessId,
                            RuleDesc = obj.RuleDesc,
                            Notes = obj.Notes,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormID,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcaprl.Add(pr_ru);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Process Rule is Created Successfully." };
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

        public async Task<DO_ReturnParameter> UpdateProcessRule(DO_ProcessRule obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_RuleDescExist = db.GtEcaprl.Where(w => w.RuleDesc.ToUpper().Replace(" ", "") == obj.RuleDesc.ToUpper().Replace(" ", "")
                                && w.ProcessId != obj.ProcessId && w.RuleId != obj.RuleId).Count();
                        if (is_RuleDescExist > 0)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Process Rule is already exist." };
                        }

                        GtEcaprl pr_ru = db.GtEcaprl.Where(w => w.RuleId == obj.RuleId && w.ProcessId==obj.ProcessId).FirstOrDefault();
                        if (pr_ru == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Process Rule is not exist" };
                        }
                        if (obj.ActiveStatus)
                        {
                            var pr = db.GtEcprrl.Where(w => w.ProcessId == obj.ProcessId).FirstOrDefault();
                            if (pr.ProcessControl == "S")
                            {
                                if (db.GtEcaprl.Where(w => w.ProcessId == obj.ProcessId && w.RuleId != obj.RuleId && w.ActiveStatus).Count() > 0)
                                    return new DO_ReturnParameter() { Status = false, Message = "Only one rule will be Active" };
                            }
                        }

                        pr_ru.RuleDesc = obj.RuleDesc;
                        pr_ru.Notes = obj.Notes;
                        pr_ru.ActiveStatus = obj.ActiveStatus;
                        pr_ru.ModifiedBy = obj.UserID;
                        pr_ru.ModifiedOn = System.DateTime.Now;
                        pr_ru.ModifiedTerminal = obj.TerminalID;

                       await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Process Rule Updated Successfully." };
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
        #region Process Rule by segment wise

        public async Task<List<DO_ProcessRule>> GetProcessRulebySegmentwise()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    return await db.GtEcaprl
                              .GroupJoin(db.GtEcaprb,
                              a =>new { a.RuleId,a.ProcessId },
                              f =>new { f.RuleId,f.ProcessId },
                              (a, f) => new { a, f = f.FirstOrDefault() })
                              .Select(r => new DO_ProcessRule
                              {
                                  RuleId=r.a.RuleId,
                                  ProcessId=r.a.ProcessId,
                                  RuleDesc=r.a.RuleDesc,
                                  ActiveStatus = r.f != null ? r.f.ActiveStatus :false,
                              }).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_ProcessRule>> GetProcessRulebyBusinessKey(int BusinessKey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcprrl.Where(x => x.IsSegmentSpecific==true)
                        .Join(db.GtEcaprl,
                            x => new { x.ProcessId, },
                            y => new { y.ProcessId, },
                           (x, y) => new { x, y })
                        .GroupJoin(db.GtEcaprb.Where(w => w.BusinessKey == BusinessKey),
                                  a => new { a.y.RuleId, a.y.ProcessId },
                                  f => new { f.RuleId, f.ProcessId },
                                  (a, f) => new { a, f = f.FirstOrDefault() })
                        .Select(r => new DO_ProcessRule
                        {
                            RuleId = r.a.y.RuleId,
                            ProcessId = r.a.y.ProcessId,
                            ActiveStatus = r.f != null ? r.f.ActiveStatus : false,
                            RuleDesc=r.a.y.RuleDesc,
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertorUpdateProcessRulebySegment(DO_ProcessRulebySegment obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var pr_ruleExists = db.GtEcaprb.Where(x=>x.BusinessKey==obj.BusinessKey && x.ProcessId==obj.ProcessId && x.RuleId==obj.RuleId).FirstOrDefault();
                        if (pr_ruleExists == null)
                        {
                            var pr_rule = new GtEcaprb
                            {
                                BusinessKey=obj.BusinessKey,
                                RuleId=obj.RuleId,
                                ProcessId=obj.ProcessId,
                                ActiveStatus = obj.ActiveStatus,
                                FormId = obj.FormID,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcaprb.Add(pr_rule);  
                        }
                        else
                        {
                            pr_ruleExists.BusinessKey = obj.BusinessKey;
                            pr_ruleExists.ProcessId = obj.ProcessId;
                            pr_ruleExists.RuleId = obj.RuleId;
                            pr_ruleExists.ActiveStatus = obj.ActiveStatus;
                            pr_ruleExists.ModifiedBy = obj.UserID;
                            pr_ruleExists.ModifiedOn = System.DateTime.Now;
                            pr_ruleExists.ModifiedTerminal = obj.TerminalID;
                        }
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        return new DO_ReturnParameter() { Status = true, Message = "Process Rule by Segment wise Updated Successfully" };
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
