using eSyaConfigSetup.DL.Repository;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DocumentControlController : ControllerBase
    {
        private readonly IDocumentControlRepository _DocumentControlRepository;

        private readonly IVoucherRepository _VoucherRepository;

        public DocumentControlController(IDocumentControlRepository DocumentControlRepository,IVoucherRepository VoucherRepository)
        {
            _DocumentControlRepository = DocumentControlRepository;
            _VoucherRepository = VoucherRepository;
        }

        #region Document Related Methods
        #region Document Control
        /// <summary>
        /// Getting Document Control List.
        /// UI Reffered - Document Control Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDocumentControls()
        {
            var doc_ctrls =await _DocumentControlRepository.GetDocumentControls();
            return Ok(doc_ctrls);
        }
        /// <summary>
        /// Insert Document Control .
        /// UI Reffered -Document Control
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertDocumentControl(DO_DocumentControl control)
        {
            var msg =await _DocumentControlRepository.InsertDocumentControl(control);
            return Ok(msg);

        }
        /// <summary>
        /// Update Document Control .
        /// UI Reffered -Document Control
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateDocumentControl(DO_DocumentControl control)
        {
            var msg =await _DocumentControlRepository.UpdateDocumentControl(control);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active Document Control.
        /// UI Reffered - Document Control
        /// </summary>
        /// <param name="status-formId-documentId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveDocumentControl(bool status, int formId, int documentId)
        {
            var msg = await _DocumentControlRepository.ActiveOrDeActiveDocumentControl(status, formId, documentId);
            return Ok(msg);
        }
        #endregion Document Control

        #region New Document Control
        /// <summary>
        /// Getting Document Control List.
        /// UI Reffered - Document Control Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDocumentControlMaster()
        {
            var doc_ctrls = await _DocumentControlRepository.GetDocumentControlMaster();
            return Ok(doc_ctrls);
        }
        /// <summary>
        /// Insert Document Control .
        /// UI Reffered -Document Control
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertDocumentControlMaster(DO_DocumentControlMaster obj)
        {
            var msg = await _DocumentControlRepository.InsertDocumentControlMaster(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Update Document Control .
        /// UI Reffered -Document Control
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateDocumentControlMaster(DO_DocumentControlMaster obj)
        {
            var msg = await _DocumentControlRepository.UpdateDocumentControlMaster(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active Document Control.
        /// UI Reffered - Document Control
        /// </summary>
        /// <param name="status-formId-documentId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveDocumentControlMaster(bool status, int documentId)
        {
            var msg = await _DocumentControlRepository.ActiveOrDeActiveDocumentControlMaster(status, documentId);
            return Ok(msg);
        }

        #endregion New Document Control

        #region FORM LINK TO DOCUMENT

        /// <summary>
        /// Getting Active Documents for drop down.
        /// UI Reffered - Document Link 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveDocuments()
        {
            var docs = await _DocumentControlRepository.GetActiveDocuments();
            return Ok(docs);
        }

        /// <summary>
        /// Getting ActiveForms for drop down.
        /// UI Reffered - Document Link 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDocumentRequiredForms()
        {
            var forms = await _DocumentControlRepository.GetDocumentRequiredForms();
            return Ok(forms);
        }

        /// <summary>
        /// Getting Linked forms with documents for Grid.
        /// UI Reffered - Document Link Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFormLinkedDocuments()
        {
            var link_forms = await _DocumentControlRepository.GetFormLinkedDocuments();
            return Ok(link_forms);
        }

        /// <summary>
        /// Insert Form Link with documents .
        /// UI Reffered -Document Link
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoFormDocumentLink(DO_FormDocumentLink obj)
        {
            var msg = await _DocumentControlRepository.InsertIntoFormDocumentLink(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Insert Form Link with documents .
        /// UI Reffered -Document Link
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteFormLinkedDocument(int formId, int documentId)
        {
            var msg = await _DocumentControlRepository.DeleteFormLinkedDocument(formId,documentId);
            return Ok(msg);

        }
        #endregion FORM LINK TO DOCUMENT

        #region Document Generation 

        

        /// <summary>
       /// Getting Storecodes by Businesskey.
      /// UI Reffered - Document Generation Grid
      /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStoresByBusinessKey(int businesskey)
        {
            var stores = await _DocumentControlRepository.GetStoresByBusinessKey(businesskey);
            return Ok(stores);
        }

        /// <summary>
        /// Getting Document Generation  for Grid.
        /// UI Reffered - Document Generation Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDocumentGenerationsbyBusinessKey(int businesskey, string Transactionmode)
        {
            var doc_gen = await _DocumentControlRepository.GetDocumentGenerationsbyBusinessKey(businesskey, Transactionmode);
            return Ok(doc_gen);
        }

        /// <summary>
        /// Insert Document Generation .
        /// UI Reffered -Document Generation
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoDocumentGeneration(DO_DocumentGeneration obj)
        {
            var msg = await _DocumentControlRepository.InsertIntoDocumentGeneration(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Update Document Generation .
        /// UI Reffered -Document Generation
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateDocumentGeneration(DO_DocumentGeneration obj)
        {
            var msg = await _DocumentControlRepository.UpdateDocumentGeneration(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active/De Active Document Generation .
        /// UI Reffered -Document Generation
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActiveDocumentGeneration(DO_DocumentGeneration obj)
        {
            var msg = await _DocumentControlRepository.ActiveOrDeActiveDocumentGeneration(obj);
            return Ok(msg);

        }

        #endregion Document Generation 

        #region Calendar Header

        /// <summary>
        /// Getting Calendar Headers by BusineeKey.
        /// UI Reffered - Calendar Header Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCalendarHeadersbyBusinessKey(int Businesskey)
        {
            var cal_headers = await _DocumentControlRepository.GetCalendarHeadersbyBusinessKey(Businesskey);
            return Ok(cal_headers);
        }

        /// <summary>
        /// Getting Calendar Header.
        /// UI Reffered - Calendar Header Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetCalendarHeaders()
        {
            var cal_headers =await _DocumentControlRepository.GetCalendarHeaders();
            return Ok(cal_headers);
        }


        /// <summary>
        /// Getting Calendar Details.
        /// UI Reffered - Calendar Details Gridrra
        /// UI-Parram -Business Key && Financial Year
        /// </summary>
        [HttpGet]
        public IActionResult GetCalendarDetailsbyBusinessKeyAndFinancialYear(int Businesskey ,decimal FinancialYear)
        {
            var cal_details = _DocumentControlRepository.GetCalendarDetailsbyBusinessKeyAndFinancialYear(Businesskey, FinancialYear);
            return Ok(cal_details);
        }


        /// <summary>
        /// Getting Cbo Financial Year By Business Key.
        /// UI Reffered - Calendar Details for Dropdown
        /// UI-Param-Business Key
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetFinancialYearbyBusinessKey(int Businesskey)
        {
            var fin_years =await _DocumentControlRepository.GetFinancialYearbyBusinessKey(Businesskey);
            return Ok(fin_years);
        }
        /// <summary>
        /// Insert Calendar Header & Details Table .
        /// UI Reffered -Calendar Header
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertCalendarHeaderAndDetails(DO_CalendarDefinition calendarheadar)
        {
            var msg =await _DocumentControlRepository.InsertCalendarHeaderAndDetails(calendarheadar);
            return Ok(msg);

        }
        /// <summary>
        /// Update Calendar details .
        /// UI Reffered -Calendar details
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateCalendardetails(DO_CalendarDetails caldetails)
        {
            var msg =await _DocumentControlRepository.UpdateCalendardetails(caldetails);
            return Ok(msg);

        }

        #endregion Calendar Header

        #region Document Control Normal Mode 
        /// <summary>
        /// Getting Document Control Normal Mode  List.
        /// UI Reffered - Document Control Normal Mode  Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDocumentControlNormalModebyBusinessKey(int businesskey)
        {
            var n_modes = await _DocumentControlRepository.GetDocumentControlNormalModebyBusinessKey(businesskey);
            return Ok(n_modes);
        }
        /// <summary>
        /// Insert Document Control Normal Mode.
        /// UI Reffered -Document Control Normal Mode
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertDocumentControlNormalMode(DO_DocumentControlNormalMode obj)
        {
            var msg = await _DocumentControlRepository.InsertDocumentControlNormalMode(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Update  Document Control Normal Mode
        /// UI Reffered - Document Control Normal Mode
        [HttpPost]
        public async Task<IActionResult> UpdateDocumentControlNormalMode(DO_DocumentControlNormalMode obj)
        {
            var msg = await _DocumentControlRepository.UpdateDocumentControlNormalMode(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active Document Control Normal Mode
        /// UI Reffered - Document Control Normal Mode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActiveDocumentControlNormalMode(DO_DocumentControlNormalMode obj)
        {
            var msg = await _DocumentControlRepository.ActiveOrDeActiveDocumentControlNormalMode(obj);
            return Ok(msg);
        }
        #endregion Document Control Normal Mode 

        #region Document Control Transaction Mode
        /// <summary>
        /// Getting Document Control Transaction Mode List.
        /// UI Reffered - Document Control Transaction Mode Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDocumentControlTransactionModebyBusinessKey(int businesskey)
        {
            var _tmodes = await _DocumentControlRepository.GetDocumentControlTransactionModebyBusinessKey(businesskey);
            return Ok(_tmodes);
        }
        /// <summary>
        /// Insert Document Control Transaction Mode
        /// UI Reffered -Document Control Transaction Mode
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertDocumentControlTransactionMode(DO_DocumentControlTransaction obj)
        {
            var msg = await _DocumentControlRepository.InsertDocumentControlTransactionMode(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Update Document Control Transaction Mode
        /// UI Reffered -Document Control Transaction Mode
        [HttpPost]
        public async Task<IActionResult> UpdateDocumentControlTransactionMode(DO_DocumentControlTransaction obj)
        {
            var msg = await _DocumentControlRepository.UpdateDocumentControlTransactionMode(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active Document Control Transaction Mode
        /// UI Reffered - Document Control Transaction Mode
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActiveDocumentControlTransactionMode(DO_DocumentControlTransaction obj)
        {
            var msg = await _DocumentControlRepository.ActiveOrDeActiveDocumentControlTransactionMode(obj);
            return Ok(msg);
        }
        #endregion Document Control Transaction Mode

        #endregion

        #region Voucher Related Methods
        #region Payment Mode
        /// <summary>
        /// Getting Payment Mode and payment Mode Category dropdown.
        /// UI Reffered - Payment Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesByCodeType(int codeType)
        {
            var pay_modes = await _VoucherRepository.GetApplicationCodesByCodeType(codeType);
            return Ok(pay_modes);
        }


        /// <summary>
        /// Getting Payment Mode List.
        /// UI Reffered - Payment Mode Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPaymentModes()
        {
            var _modes = await _VoucherRepository.GetPaymentModes();
            return Ok(_modes);
        }
        /// <summary>
        /// Insert Payment Mode.
        /// UI Reffered -Payment Mode
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdatePaymentMode(DO_PaymentMode obj)
        {
            var msg = await _VoucherRepository.InsertOrUpdatePaymentMede(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Active Or De Active Payment Mode .
        /// UI Reffered -Payment Mode 
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActivePaymentMode(DO_PaymentMode obj)
        {
            var msg = await _VoucherRepository.ActiveOrDeActivePaymentMode(obj);
            return Ok(msg);

        }
        #endregion Payment

        #region Transaction Type
        /// <summary>
        /// Getting Payment Voucher List.
        /// UI Reffered - Payment Voucher Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPaymentVouchers()
        {
            var _vouchers = await _VoucherRepository.GetPaymentVouchers();
            return Ok(_vouchers);
        }
        /// <summary>
        /// Insert Payment Voucher .
        /// UI Reffered -Payment Voucher
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoPaymentVoucher(DO_PaymentVoucher obj)
        {
            var msg = await _VoucherRepository.InsertIntoPaymentVoucher(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Update Payment Voucher.
        /// UI Reffered -Payment Voucher
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdatePaymentVoucher(DO_PaymentVoucher obj)
        {
            var msg = await _VoucherRepository.UpdatePaymentVoucher(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active Payment Voucher
        /// UI Reffered - Payment Voucher
        /// </summary>
        /// <param name="status-voucherId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActivePaymentVoucher(bool status, int voucherId)
        {
            var msg = await _VoucherRepository.ActiveOrDeActivePaymentVoucher(status, voucherId);
            return Ok(msg);
        }
        #endregion Document Control

        #region Mapping

        /// <summary>
        /// Getting Active Payments for drop down.
        /// UI Reffered - Voucher Link 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActivePaymentModes()
        {
            var _methods = await _VoucherRepository.GetActivePaymentModes();
            return Ok(_methods);
        }

        /// <summary>
        /// Getting Active Vouchers for drop down.
        /// UI Reffered - Voucher Link 
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveVouchers()
        {
            var _vouchers = await _VoucherRepository.GetActiveVouchers();
            return Ok(_vouchers);
        }

        /// <summary>
        /// Getting Linked vouchers with payments for Grid.
        /// UI Reffered - Voucher Link Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetPaymentLinkedVouchers()
        {
            var link_vouchers = await _VoucherRepository.GetPaymentLinkedVouchers();
            return Ok(link_vouchers);
        }

        /// <summary>
        /// Insert Form Link with documents .
        /// UI Reffered -Voucher Link
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoPaymentVoucherLink(DO_PaymentVoucherLink obj)
        {
            var msg = await _VoucherRepository.InsertIntoPaymentVoucherLink(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Delete vouchers Link with payments .
        /// UI Reffered -Voucher Link
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeletePaymentLinkedVoucher(int voucherId, int paymentId)
        {
            var msg = await _VoucherRepository.DeletePaymentLinkedVoucher(voucherId, paymentId);
            return Ok(msg);

        }
        #endregion Payment LINK TO Voucher

        #region Voucher Generation
        /// <summary>
        /// Getting Voucher Generation List.
        /// UI Reffered - Voucher Generation Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetVoucherGenerationsbyBusinesskey(int businesskey)
        {
            var _vgen = await _VoucherRepository.GetVoucherGenerationsbyBusinesskey(businesskey);
            return Ok(_vgen);
        }
        /// <summary>
        /// Insert Voucher Generation.
        /// UI Reffered -Voucher Generation
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoVoucherGeneration(DO_VoucherGeneration obj)
        {
            var msg = await _VoucherRepository.InsertIntoVoucherGeneration(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Update Voucher Generation.
        /// UI Reffered -Voucher Generation
        [HttpPost]
        public async Task<IActionResult> UpdateVoucherGeneration(DO_VoucherGeneration obj)
        {
            var msg = await _VoucherRepository.UpdateVoucherGeneration(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active Voucher Generation
        /// UI Reffered - Voucher Generation
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActiveVoucherGeneration(DO_VoucherGeneration obj)
        {
            var msg = await _VoucherRepository.ActiveOrDeActiveVoucherGeneration(obj);
            return Ok(msg);
        }
        #endregion Voucher Generation


        #endregion

    }
}