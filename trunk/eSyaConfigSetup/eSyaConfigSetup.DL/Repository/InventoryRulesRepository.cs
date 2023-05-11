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
   public class InventoryRulesRepository:IInventoryRulesRepository
    {

        public async Task<List<DO_InventoryRules>> GetInventoryRules()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcinvr

                    .Select(i => new DO_InventoryRules
                    {
                        InventoryRuleId=i.InventoryRuleId,
                        InventoryRuleDesc=i.InventoryRuleDesc,
                        InventoryRule=i.InventoryRule,
                        ApplyToSrn=i.ApplyToSrn,
                        ActiveStatus = i.ActiveStatus
                    }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertInventoryRule(DO_InventoryRules inventoryRule)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_invRuleId = db.GtEcinvr.Where(i => i.InventoryRuleId.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (is_invRuleId != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Inventory Rule is already exist please select another One." };
                        }
                        var is_invRuledescExists = db.GtEcinvr.Where(i => i.InventoryRuleDesc.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleDesc.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (is_invRuledescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Inventory Rule Description is already exist." };
                        }

                        var inv_Rule = new GtEcinvr
                        {
                            InventoryRuleId = inventoryRule.InventoryRuleId,
                            InventoryRuleDesc = inventoryRule.InventoryRuleDesc,
                            InventoryRule = inventoryRule.InventoryRule,
                            ApplyToSrn = inventoryRule.ApplyToSrn,
                            FormId = inventoryRule.FormId,
                            ActiveStatus = inventoryRule.ActiveStatus,
                            CreatedBy = inventoryRule.UserID,
                            CreatedOn = DateTime.Now,
                            CreatedTerminal= inventoryRule.TerminalID
                        };
                        db.GtEcinvr.Add(inv_Rule);

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Inventory Rule Created Successfully." };
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

        public async Task<DO_ReturnParameter> UpdateInventoryRule(DO_InventoryRules inventoryRule)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_invRuledescExists = db.GtEcinvr.Where(i => i.InventoryRuleDesc.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleDesc.ToUpper().Replace(" ", "")
                        &&i.InventoryRuleId.ToUpper().Replace(" ", "")!= inventoryRule.InventoryRuleId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (is_invRuledescExists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Inventory Rule Description is already exist." };
                        }

                        GtEcinvr inv_Rule = db.GtEcinvr.Where(i=>i.InventoryRuleId.ToUpper().Replace(" ", "") == inventoryRule.InventoryRuleId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (inv_Rule != null)
                        {
                            inv_Rule.InventoryRuleDesc = inventoryRule.InventoryRuleDesc;
                            inv_Rule.InventoryRule = inventoryRule.InventoryRule;
                            inv_Rule.ApplyToSrn = inventoryRule.ApplyToSrn;
                            inv_Rule.ActiveStatus = inventoryRule.ActiveStatus;
                            inv_Rule.ModifiedBy = inventoryRule.UserID;
                            inv_Rule.ModifiedOn = DateTime.Now;
                            inv_Rule.ModifiedTerminal = inventoryRule.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Inventory Rule Updated Successfully." };

                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Inventory Rule does Not Exists." };

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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveInventoryRules(bool status, string InventoryId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcinvr inventory_rule = db.GtEcinvr.Where(w => w.InventoryRuleId.ToUpper().Replace(" ", "") == InventoryId.ToUpper().Replace(" ", "")).FirstOrDefault();
                        if (inventory_rule == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Inventory Rule is not exist" };
                        }

                        inventory_rule.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Inventory Rule Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Inventory Rule code De Activated Successfully." };
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
