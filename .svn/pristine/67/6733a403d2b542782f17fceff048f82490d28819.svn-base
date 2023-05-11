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
   public class VoucherRepository:IVoucherRepository
    {
        #region Payment Mode
        public async Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeType(int codeType)
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcapcd
                        .Where(w => w.CodeType == codeType && w.ActiveStatus)
                        .Select(r => new DO_ApplicationCodes
                        {
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

        public async Task<List<DO_PaymentMode>> GetPaymentModes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEcpyid.Join(db.GtEcapcd,
                         x => x.PaymentModeId,
                         y => y.ApplicationCode,
                         (x, y) => new { x, y }).Join(db.GtEcapcd,
                         a => a.x.PaymentModeCategoryId,
                         p => p.ApplicationCode, (a, p) => new { a, p }).Select(r => new DO_PaymentMode
                         {
                             PaymentId = r.a.x.PaymentId,
                             PaymentModeId = r.a.x.PaymentModeId,
                             PaymentMode = r.a.y.CodeDesc,
                             PaymentModeCategoryId = r.a.x.PaymentModeCategoryId,
                             PaymentCategory = r.p.CodeDesc,
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

        public async Task<DO_ReturnParameter> InsertOrUpdatePaymentMede(DO_PaymentMode obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcpyid payment = db.GtEcpyid.Where(p => p.PaymentId == obj.PaymentId && p.PaymentModeId == obj.PaymentModeId && p.PaymentModeCategoryId == obj.PaymentModeCategoryId).FirstOrDefault();

                        if (payment == null)
                        {
                            var _paymentmethod = new GtEcpyid
                            {
                                PaymentId = obj.PaymentId,
                                PaymentModeId = obj.PaymentModeId,
                                PaymentModeCategoryId = obj.PaymentModeCategoryId,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcpyid.Add(_paymentmethod);
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Payment Method Created Successfully." };
                        }
                        else
                        {

                            payment.ActiveStatus = obj.ActiveStatus;
                            payment.ModifiedBy = obj.UserID;
                            payment.ModifiedOn = System.DateTime.Now;
                            payment.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Payment Method Updated Successfully." };
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

        public async Task<DO_ReturnParameter> ActiveOrDeActivePaymentMode(DO_PaymentMode obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        var is_paymentExists = db.GtEcpyid.Where(p => p.PaymentId == obj.PaymentId && p.PaymentModeId == obj.PaymentModeId && p.PaymentModeCategoryId == obj.PaymentModeCategoryId).FirstOrDefault();
                        if (is_paymentExists == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Not  Exists" };
                        }
                        is_paymentExists.ActiveStatus = obj.a_status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        if (obj.a_status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Payment Method Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Payment Method De Activated Successfully." };
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

        #endregion Payment

        #region Transaction Type
        public async Task<List<DO_PaymentVoucher>> GetPaymentVouchers()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var result = db.GtEcvyid.Select(
                        x => new DO_PaymentVoucher
                        {
                            VoucherId = x.VoucherId,
                            TransactionType = x.TransactionType,
                            VoucherDesc = x.VoucherDesc,
                            ActiveStatus = x.ActiveStatus
                        }).ToListAsync();

                    return await result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoPaymentVoucher(DO_PaymentVoucher obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var isvoucherId_Exists = db.GtEcvyid.Where(v => v.VoucherId == obj.VoucherId).FirstOrDefault();
                        if (isvoucherId_Exists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Voucher Id is already exist." };
                        }

                        var _vchr = new GtEcvyid
                        {
                            VoucherId = obj.VoucherId,
                            TransactionType = obj.TransactionType,
                            VoucherDesc = obj.VoucherDesc,
                            ActiveStatus = obj.ActiveStatus,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtEcvyid.Add(_vchr);
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Voucher Created Successfully." };
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

        public async Task<DO_ReturnParameter> UpdatePaymentVoucher(DO_PaymentVoucher obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        GtEcvyid _vochr = db.GtEcvyid.Where(v => v.VoucherId == obj.VoucherId).FirstOrDefault();

                        if (_vochr != null)
                        {

                            _vochr.TransactionType = obj.TransactionType;
                            _vochr.VoucherDesc = obj.VoucherDesc;
                            _vochr.ActiveStatus = obj.ActiveStatus;
                            _vochr.ModifiedBy = obj.UserID;
                            _vochr.ModifiedOn = System.DateTime.Now;
                            _vochr.ModifiedTerminal = obj.TerminalID;
                            await db.SaveChangesAsync();
                            dbContext.Commit();
                            return new DO_ReturnParameter() { Status = true, Message = "Voucher Updated Successfully." };

                        }

                        else
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Voucher does Not Exists." };

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

        public async Task<DO_ReturnParameter> ActiveOrDeActivePaymentVoucher(bool status, int voucherId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcvyid _vchr = db.GtEcvyid.Where(v => v.VoucherId == voucherId).FirstOrDefault();
                        if (_vchr == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Voucher is not exist" };
                        }

                        _vchr.ActiveStatus = status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        if (status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Voucher Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Voucher De Activated Successfully." };
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
        #endregion Payment Voucher

        #region Mapping

        public async Task<List<DO_PaymentMode>> GetActivePaymentModes()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcpyid.Where(x => x.ActiveStatus == true).Join(db.GtEcapcd,
                        d => d.PaymentModeId,
                        f => f.ApplicationCode,
                        (d, f) => new DO_PaymentMode
                        {
                            PaymentId = d.PaymentId,
                            PaymentMode = f.CodeDesc
                        }).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<DO_PaymentVoucher>> GetActiveVouchers()
        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = db.GtEcvyid.Where(x => x.ActiveStatus == true)
                        .Select(r => new DO_PaymentVoucher
                        {
                            VoucherId = r.VoucherId,
                            VoucherDesc = r.VoucherDesc
                        }).OrderBy(o => o.VoucherDesc).ToListAsync();

                    return await ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoPaymentVoucherLink(DO_PaymentVoucherLink obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var is_voucherlinked = db.GtEcpvln.Where(d => d.PaymentId == obj.PaymentId && d.VoucherId == obj.VoucherId).FirstOrDefault();
                        if (is_voucherlinked != null)
                        {
                            is_voucherlinked.ActiveStatus = obj.ActiveStatus;
                            is_voucherlinked.ModifiedBy = obj.UserID;
                            is_voucherlinked.ModifiedOn = System.DateTime.Now;
                            is_voucherlinked.ModifiedTerminal = obj.TerminalID;
                        }
                        else
                        {
                            var pay_link = new GtEcpvln
                            {
                                PaymentId = obj.PaymentId,
                                VoucherId = obj.VoucherId,
                                ActiveStatus = obj.ActiveStatus,
                                CreatedBy = obj.UserID,
                                CreatedOn = System.DateTime.Now,
                                CreatedTerminal = obj.TerminalID
                            };
                            db.GtEcpvln.Add(pay_link);
                        }

                        await db.SaveChangesAsync();
                        dbContext.Commit();

                        return new DO_ReturnParameter() { Status = true, Message = "Payment is linked with selected Voucher Successfully." };
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

        public async Task<List<DO_PaymentVoucherLink>> GetPaymentLinkedVouchers()

        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtEcpvln.Join(db.GtEcvyid,
                         x => x.VoucherId,
                         y => y.VoucherId,
                         (x, y) => new { x, y }).Join(db.GtEcpyid,
                         a => a.x.PaymentId,
                         p => p.PaymentId, (a, p) => new { a, p }).Join(db.GtEcapcd,
                         b => b.p.PaymentModeId,
                         c => c.ApplicationCode, (b, c) => new { b, c }).Select(r => new DO_PaymentVoucherLink
                         {
                             PaymentId = r.b.a.x.PaymentId,
                             VoucherId = r.b.a.x.VoucherId,
                             ActiveStatus = r.b.a.x.ActiveStatus,
                             Voucher = r.b.a.y.VoucherDesc,
                             PaymentMode = r.c.CodeDesc

                         }).ToListAsync();

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> DeletePaymentLinkedVoucher(int voucherId, int paymentId)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        GtEcpvln pv_link = db.GtEcpvln.Where(w => w.PaymentId == paymentId && w.VoucherId == voucherId).FirstOrDefault();
                        if (pv_link == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Not exist" };
                        }

                        db.GtEcpvln.Remove(pv_link);
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

        #endregion LINK TO Voucher

        #region Voucher Generation
        public async Task<List<DO_VoucherGeneration>> GetVoucherGenerationsbyBusinesskey(int businesskey)

        {
            try
            {
                using (var db = new eSyaEnterprise())
                {
                    var ds = await db.GtDnvcdt.Where(k => k.BusinessKey == businesskey).Join(db.GtEcvyid,
                         x => x.VoucherId,
                         y => y.VoucherId,
                         (x, y) => new { x, y }).Join(db.GtEcpyid,
                         a => a.x.PaymentId,
                         p => p.PaymentId, (a, p) => new { a, p }).Join(db.GtEcapcd,
                         b => b.p.PaymentModeId,
                         c => c.ApplicationCode, (b, c) => new { b, c }).Select(r => new DO_VoucherGeneration
                         {
                             BusinessKey = r.b.a.x.BusinessKey,
                             FinancialYear = r.b.a.x.FinancialYear,
                             PaymentId = r.b.a.x.PaymentId,
                             VoucherId = r.b.a.x.VoucherId,
                             VoucherType = r.b.a.x.VoucherType,
                             StartVocucherNumber = r.b.a.x.StartVocucherNumber,
                             CurrentVoucherNumber = r.b.a.x.CurrentVoucherNumber,
                             CurrentVoucherDate = r.b.a.x.CurrentVoucherDate,
                             CreditDebitId = r.b.a.x.CreditDebitId,
                             UsageStatus = r.b.a.x.UsageStatus,
                             ActiveStatus = r.b.a.x.ActiveStatus,
                             Voucher = r.b.a.y.VoucherDesc,
                             PaymentMode = r.c.CodeDesc

                         }).ToListAsync();

                    return ds;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DO_ReturnParameter> InsertIntoVoucherGeneration(DO_VoucherGeneration obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        var is_Exists = db.GtDnvcdt.Where(d => d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear && d.PaymentId == obj.PaymentId
                        && d.VoucherId == obj.VoucherId && d.VoucherType.ToUpper().Replace(" ", "") == obj.VoucherType.ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (is_Exists != null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Already Exists" };
                        }

                        var pv_gen = new GtDnvcdt
                        {
                            BusinessKey = obj.BusinessKey,
                            FinancialYear = obj.FinancialYear,
                            PaymentId = obj.PaymentId,
                            VoucherId = obj.VoucherId,
                            VoucherType = obj.VoucherType,
                            StartVocucherNumber = obj.StartVocucherNumber,
                            //CurrentVoucherNumber=obj.CurrentVoucherNumber,
                            CurrentVoucherNumber = obj.StartVocucherNumber,
                            CurrentVoucherDate = obj.CurrentVoucherDate,
                            CreditDebitId = obj.CreditDebitId,
                            UsageStatus = obj.UsageStatus,
                            ActiveStatus = obj.ActiveStatus,
                            FormId = obj.FormId,
                            CreatedBy = obj.UserID,
                            CreatedOn = System.DateTime.Now,
                            CreatedTerminal = obj.TerminalID
                        };
                        db.GtDnvcdt.Add(pv_gen);
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

        public async Task<DO_ReturnParameter> UpdateVoucherGeneration(DO_VoucherGeneration obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {

                        var pv_gen = db.GtDnvcdt.Where(d => d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear && d.PaymentId == obj.PaymentId
                        && d.VoucherId == obj.VoucherId && d.VoucherType.ToUpper().Replace(" ", "") == obj.VoucherType.ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (pv_gen == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Not Exists" };
                        }
                        pv_gen.StartVocucherNumber = obj.StartVocucherNumber;
                        //pv_gen.CurrentVoucherNumber=obj.CurrentVoucherNumber;
                        pv_gen.CurrentVoucherNumber = obj.StartVocucherNumber;
                        pv_gen.CurrentVoucherDate = obj.CurrentVoucherDate;
                        pv_gen.CreditDebitId = obj.CreditDebitId;
                        pv_gen.UsageStatus = obj.UsageStatus;
                        pv_gen.ActiveStatus = obj.ActiveStatus;
                        pv_gen.ModifiedBy = obj.UserID;
                        pv_gen.ModifiedOn = System.DateTime.Now;
                        pv_gen.ModifiedTerminal = obj.TerminalID;
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

        public async Task<DO_ReturnParameter> ActiveOrDeActiveVoucherGeneration(DO_VoucherGeneration obj)
        {
            using (var db = new eSyaEnterprise())
            {
                using (var dbContext = db.Database.BeginTransaction())
                {
                    try
                    {
                        var pv_gen = db.GtDnvcdt.Where(d => d.BusinessKey == obj.BusinessKey && d.FinancialYear == obj.FinancialYear && d.PaymentId == obj.PaymentId
                                                && d.VoucherId == obj.VoucherId && d.VoucherType.ToUpper().Replace(" ", "") == obj.VoucherType.ToUpper().Replace(" ", "")).FirstOrDefault();

                        if (pv_gen == null)
                        {
                            return new DO_ReturnParameter() { Status = false, Message = "Not  Exists" };
                        }
                        pv_gen.ActiveStatus = obj.a_status;
                        await db.SaveChangesAsync();
                        dbContext.Commit();
                        if (obj.a_status == true)
                            return new DO_ReturnParameter() { Status = true, Message = "Voucher Generation Activated Successfully." };
                        else
                            return new DO_ReturnParameter() { Status = true, Message = "Voucher Generation De Activated Successfully." };

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
        #endregion Voucher Generation
    }
}
