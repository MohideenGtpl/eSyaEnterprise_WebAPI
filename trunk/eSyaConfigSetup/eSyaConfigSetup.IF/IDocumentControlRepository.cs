using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface IDocumentControlRepository
    {
        #region Old Document Control
        Task<List<DO_DocumentControl>> GetDocumentControls();

        Task<DO_ReturnParameter> InsertDocumentControl(DO_DocumentControl control);

        Task<DO_ReturnParameter> UpdateDocumentControl(DO_DocumentControl control);

        Task<DO_ReturnParameter> ActiveOrDeActiveDocumentControl(bool status, int formId, int documentId);
        #endregion Old Document Control

        #region New Document Control
        Task<List<DO_DocumentControlMaster>> GetDocumentControlMaster();

        Task<DO_ReturnParameter> InsertDocumentControlMaster(DO_DocumentControlMaster obj);

        Task<DO_ReturnParameter> UpdateDocumentControlMaster(DO_DocumentControlMaster obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveDocumentControlMaster(bool status, int documentId);

        #endregion New Document Control

        #region FORM LINK TO DOCUMENT

        Task<List<DO_DocumentControlMaster>> GetActiveDocuments();

        Task<List<DO_FormNames>> GetDocumentRequiredForms();

        Task<DO_ReturnParameter> InsertIntoFormDocumentLink(DO_FormDocumentLink obj);

        Task<List<DO_FormDocumentLink>> GetFormLinkedDocuments();

        Task<DO_ReturnParameter> DeleteFormLinkedDocument(int formId, int documentId);

        #endregion FORM LINK TO DOCUMENT

        #region Document Generation 

        Task<List<DO_StoreMaster>> GetStoresByBusinessKey(int businesskey);

        Task<DO_ReturnParameter> InsertIntoDocumentGeneration(DO_DocumentGeneration obj);

        Task<DO_ReturnParameter> UpdateDocumentGeneration(DO_DocumentGeneration obj);

        Task<List<DO_DocumentGeneration>> GetDocumentGenerationsbyBusinessKey(int businesskey, string Transactionmode);

        Task<DO_ReturnParameter> ActiveOrDeActiveDocumentGeneration(DO_DocumentGeneration obj);
        #endregion Document Generation 

        #region Calendar Defination
        Task<DO_ReturnParameter> InsertCalendarHeaderAndDetails(DO_CalendarDefinition calendarheadar);

        Task<DO_ReturnParameter> UpdateCalendardetails(DO_CalendarDetails caldetails);

        List<DO_CalendarDetails> GetCalendarDetailsbyBusinessKeyAndFinancialYear(int BusinessKey, decimal FinancialYear);

        Task<List<DO_CalendarDefinition>> GetCalendarHeadersbyBusinessKey(int Businesskey);

        Task<List<DO_CalendarDefinition>> GetCalendarHeaders();

        Task<List<DO_CalendarDetails>> GetFinancialYearbyBusinessKey(int Businesskey);
        #endregion Calendar Defination

        #region Document Control Normal Mode 
        Task<DO_ReturnParameter> InsertDocumentControlNormalMode(DO_DocumentControlNormalMode obj);

        Task<DO_ReturnParameter> UpdateDocumentControlNormalMode(DO_DocumentControlNormalMode obj);

        Task<List<DO_DocumentControlNormalMode>> GetDocumentControlNormalModebyBusinessKey(int businesskey);

        Task<DO_ReturnParameter> ActiveOrDeActiveDocumentControlNormalMode(DO_DocumentControlNormalMode obj);
        #endregion Document Control Normal Mode

        #region Document Control Transaction Mode

        Task<DO_ReturnParameter> InsertDocumentControlTransactionMode(DO_DocumentControlTransaction obj);

        Task<DO_ReturnParameter> UpdateDocumentControlTransactionMode(DO_DocumentControlTransaction obj);

        Task<List<DO_DocumentGeneration>> GetDocumentControlTransactionModebyBusinessKey(int businesskey);

        Task<DO_ReturnParameter> ActiveOrDeActiveDocumentControlTransactionMode(DO_DocumentControlTransaction obj);

        #endregion Document Control Transaction Mode  
    }
}
