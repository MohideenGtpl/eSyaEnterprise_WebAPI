using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface IVoucherRepository
    {
        #region  Payment Mode
        Task<List<DO_ApplicationCodes>> GetApplicationCodesByCodeType(int codeType);

        Task<List<DO_PaymentMode>> GetPaymentModes();

        Task<DO_ReturnParameter> InsertOrUpdatePaymentMede(DO_PaymentMode obj);

        Task<DO_ReturnParameter> ActiveOrDeActivePaymentMode(DO_PaymentMode obj);
        #endregion Payment

        #region Transaction Type
        Task<List<DO_PaymentVoucher>> GetPaymentVouchers();

        Task<DO_ReturnParameter> InsertIntoPaymentVoucher(DO_PaymentVoucher obj);

        Task<DO_ReturnParameter> UpdatePaymentVoucher(DO_PaymentVoucher obj);

        Task<DO_ReturnParameter> ActiveOrDeActivePaymentVoucher(bool status, int voucherId);
        #endregion Payment Voucher

        #region Mapping
        Task<List<DO_PaymentMode>> GetActivePaymentModes();

        Task<List<DO_PaymentVoucher>> GetActiveVouchers();

        Task<DO_ReturnParameter> InsertIntoPaymentVoucherLink(DO_PaymentVoucherLink obj);

        Task<List<DO_PaymentVoucherLink>> GetPaymentLinkedVouchers();

        Task<DO_ReturnParameter> DeletePaymentLinkedVoucher(int voucherId, int paymentId);
        #endregion LINK TO Voucher

        #region Voucher Generation
        Task<List<DO_VoucherGeneration>> GetVoucherGenerationsbyBusinesskey(int businesskey);

        Task<DO_ReturnParameter> InsertIntoVoucherGeneration(DO_VoucherGeneration obj);

        Task<DO_ReturnParameter> UpdateVoucherGeneration(DO_VoucherGeneration obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveVoucherGeneration(DO_VoucherGeneration obj);
        #endregion Voucher Generation
    }
}
