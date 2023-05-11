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
   public class FormApprovalRepository:IFormApprovalRepository
    {
        public async Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeTypeList(List<int> l_codeType)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcapcd
                        .Where(w => w.ActiveStatus
                        && l_codeType.Contains(w.CodeType))
                        .Select(r => new DO_ApplicationCodes
                        {
                            CodeType = r.CodeType,
                            ApplicationCode = r.ApplicationCode,
                            CodeDesc = r.CodeDesc
                        }).OrderBy(o => o.CodeDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_FormNames>> GetAllActiveForms()
        {
            try
            {
                
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcfmfd
                        .Where(w => w.ActiveStatus)
                        .Select(r => new DO_FormNames
                        {
                            FormID = r.FormId,
                            FormName = r.FormName,
                        }).OrderBy(o => o.FormName).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Form Task Assign
        public async Task<List<DO_FormTaskAssign>> GetFormTaskAssignments()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEcfmta.Join(db.GtEcfmfd,
                          x => x.FormId,
                          y => y.FormId,
                          (x, y) => new { x, y }).Join(db.GtEcapcd,
                          a => a.x.TaskId,
                          p => p.ApplicationCode, (a, p) => new { a, p })
                          .Select(r => new DO_FormTaskAssign
                          {
                              FormId =r.a.x.FormId,
                              TaskId = r.a.x.TaskId,
                              AutoReassignTimeline = r.a.x.AutoReassignTimeline,
                              ActiveStatus = r.a.x.ActiveStatus,
                              FormName=r.a.y.FormName,
                              TaskName=r.p.CodeDesc
                          }).ToListAsync();

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertFormTaskAssignment(DO_FormTaskAssign obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcfmta _isfmExists= db.GtEcfmta.Where(w => w.FormId == obj.FormId && w.TaskId == obj.TaskId).FirstOrDefault();

                        if(_isfmExists!=null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Task is already Exists for Selected Form" };
                        }
                        else
                        {
                            var fm_task = new GtEcfmta
                            {
                                FormId = obj.FormId,
                                TaskId = obj.TaskId,
                                AutoReassignTimeline = obj.AutoReassignTimeline,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID,
                            };
                            db.GtEcfmta.Add(fm_task);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Task created Successfully for selected form." };

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

        public async Task<DO_ReturnParameter> UpdateFormTaskAssignment(DO_FormTaskAssign obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcfmta fm_task = db.GtEcfmta.Where(w => w.FormId == obj.FormId && w.TaskId == obj.TaskId).FirstOrDefault();

                        if (fm_task == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Task not Exists" };
                        }
                        else
                        {
                          
                            fm_task.AutoReassignTimeline = obj.AutoReassignTimeline;
                            fm_task.ActiveStatus = obj.ActiveStatus;
                            fm_task.ModifiedBy = obj.UserID;
                            fm_task.ModifiedOn = System.DateTime.Now;
                            fm_task.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Task Updated Successfully for selected form." };

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveFormTaskAssignment(bool status, int formId, int taskId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcfmta fm_task = db.GtEcfmta.Where(w => w.FormId == formId && w.TaskId == taskId).FirstOrDefault();
                        if (fm_task == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Task doesn't exist" };
                        }

                        fm_task.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Task Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Task De Activated Successfully." };
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

        #region Form Task Approval

        public List<DO_FormTaskAssign> GetFormTaskbyFormId(int  formId)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcfmta.Where(x=>x.FormId==formId)
                            .Join(db.GtEcapcd,
                            a => a.TaskId,
                            f => f.ApplicationCode,
                            (a, f) => 
                             new DO_FormTaskAssign
                            {
                                 TaskId=a.TaskId,
                                 TaskName=f.CodeDesc
                            }).ToList();
                    var DistinctKeys = result.GroupBy(x => x.TaskId).Select(y => y.First());
                    return DistinctKeys.ToList();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_FormTaskApproval>> GetFormTaskApprovalsbyBusinesskey(int businesskey)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEcfmap.Where(k => k.BusinessKey == businesskey).Join(db.GtEcfmfd,
                          x => x.FormId,
                          y => y.FormId,
                          (x, y) => new { x, y }).Join(db.GtEcapcd,
                          a => a.x.TaskId,
                          p => p.ApplicationCode, (a, p) => new { a, p }).Join(db.GtEcapcd,
                          b => b.a.x.UserRole,
                          c => c.ApplicationCode, (b, c) => new { b, c }).Select(r => new DO_FormTaskApproval
                          {
                              BusinessKey = r.b.a.x.BusinessKey,
                              FormId = r.b.a.x.FormId,
                              TaskId = r.b.a.x.TaskId,
                              ApprovalLevelStage = r.b.a.x.ApprovalLevelStage,
                              ApproverPriority = r.b.a.x.ApproverPriority,
                              UserRole = r.b.a.x.UserRole,
                              ApprovalRangeFrom = r.b.a.x.ApprovalRangeFrom,
                              ApprovalRangeTo = r.b.a.x.ApprovalRangeTo,
                              ActiveStatus = r.b.a.x.ActiveStatus,
                              FormName = r.b.a.y.FormName,
                              TaskName = r.b.p.CodeDesc,
                              UserRoleName = r.c.CodeDesc
                          }).ToListAsync();

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertFormTaskApproval(DO_FormTaskApproval obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcfmap _isfmExists = db.GtEcfmap.Where(w => w.BusinessKey == obj.BusinessKey && w.FormId == obj.FormId && w.TaskId == obj.TaskId
                        && w.ApprovalLevelStage == obj.ApprovalLevelStage && w.ApproverPriority == obj.ApproverPriority && w.UserRole == obj.UserRole).FirstOrDefault();

                        if (_isfmExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Already Exists" };
                        }
                        else
                        {
                            var fm_approval = new GtEcfmap
                            {
                                BusinessKey=obj.BusinessKey,
                                FormId = obj.FormId,
                                TaskId = obj.TaskId,
                                ApprovalLevelStage=obj.ApprovalLevelStage,
                                ApproverPriority=obj.ApproverPriority,
                                UserRole = obj.UserRole,
                                ApprovalRangeFrom=obj.ApprovalRangeFrom,
                                ApprovalRangeTo=obj.ApprovalRangeTo,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID,
                            };
                            db.GtEcfmap.Add(fm_approval);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Saved Successfully" };

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

        public async Task<DO_ReturnParameter> UpdateFormTaskApproval(DO_FormTaskApproval obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcfmap _isfmExists = db.GtEcfmap.Where(w => w.BusinessKey == obj.BusinessKey && w.FormId == obj.FormId && w.TaskId == obj.TaskId
                        && w.ApprovalLevelStage == obj.ApprovalLevelStage && w.ApproverPriority == obj.ApproverPriority && w.UserRole == obj.UserRole).FirstOrDefault();

                        if (_isfmExists == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Not Exists" };
                        }
                        else
                        {
                            _isfmExists.ApprovalRangeFrom = obj.ApprovalRangeFrom;
                            _isfmExists.ApprovalRangeTo = obj.ApprovalRangeTo;
                            _isfmExists.ActiveStatus = obj.ActiveStatus;
                            _isfmExists.ModifiedBy = obj.UserID;
                            _isfmExists.ModifiedOn = System.DateTime.Now;
                            _isfmExists.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Updated Successfully." };

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveFormTaskApproval(DO_FormTaskApproval objform)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcfmap _isfmExists = db.GtEcfmap.Where(w => w.BusinessKey == objform.BusinessKey && w.FormId == objform.FormId && w.TaskId == objform.TaskId
                                               && w.ApprovalLevelStage == objform.ApprovalLevelStage && w.ApproverPriority == objform.ApproverPriority && w.UserRole == objform.UserRole).FirstOrDefault();
                        if (_isfmExists == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "doesn't exist" };
                        }

                        _isfmExists.ActiveStatus = objform.status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (objform.status == true)
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
        #endregion
    }
}
