using eSyaConfigSetup.DL.Repository;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FormsController : ControllerBase
    {
        private readonly IFormsRepository _FormsRepository;
        public FormsController(IFormsRepository FormsRepository)
        {
            _FormsRepository = FormsRepository;
        }

        public async Task<IActionResult> GetAreaController()
        {
            var form_details = await _FormsRepository.GetAreaController();
            return Ok(form_details);
        }

        public async Task<IActionResult> GetFormDetails()
        {
            var form_details = await _FormsRepository.GetFormDetails();
            return Ok(form_details);
        }

        public async Task<IActionResult> GetFormDetailsByID(int formID)
        {
            var form_detail = await _FormsRepository.GetFormDetailsByID(formID);
            return Ok(form_detail);
        }

        public async Task<IActionResult> GetInternalFormDetails()
        {
            var intform_details = await _FormsRepository.GetInternalFormDetails();
            return Ok(intform_details);
        }

        public async Task<IActionResult> GetInternalFormByFormID(int formID)
        {
            var intform_detail = await _FormsRepository.GetInternalFormByFormID(formID);
            return Ok(intform_detail);
        }

        public async Task<IActionResult> InsertUpdateIntoFormMaster(DO_Forms obj)
        {
            var msg = await _FormsRepository.InsertUpdateIntoFormMaster(obj);
            return Ok(msg);
        }

        public async Task<IActionResult> GetNextInternalFormByID(int formID)
        {
            var intform_details = await _FormsRepository.GetNextInternalFormByID(formID);
            return Ok(intform_details);
        }

        public async Task<IActionResult> InsertIntoInternalForm(DO_Forms obj)
        {
            var msg = await _FormsRepository.InsertIntoInternalForm(obj);
            return Ok(msg);
        }

        public async Task<IActionResult> GetFormAction()
        {
            var form_actions = await _FormsRepository.GetFormAction();
            return Ok(form_actions);
        }

        public async Task<IActionResult> GetFormActionByID(int formID)
        {
            var form_action = await _FormsRepository.GetFormActionByID(formID);
            return Ok(form_action);
        }

        public async Task<IActionResult> InsertIntoFormAction(DO_Forms obj)
        {
            var msg = await _FormsRepository.InsertIntoFormAction(obj);
            return Ok(msg);
        }

        public async Task<IActionResult> GetFormParameterByID(int formID)
        {
            var form_action = await _FormsRepository.GetFormParameterByID(formID);
            return Ok(form_action);
        }
        public async Task<IActionResult> InsertIntoFormParameter(DO_Forms obj)
        {
            var msg = await _FormsRepository.InsertIntoFormParameter(obj);
            return Ok(msg);
        }
        public async Task<IActionResult> GetFormSubParameterByID(int formID, int parameterId)
        {
            var form_action = await _FormsRepository.GetFormSubParameterByID(formID, parameterId);
            return Ok(form_action);
        }
        public async Task<IActionResult> InsertIntoFormSubParameter(DO_Forms obj)
        {
            var msg = await _FormsRepository.InsertIntoFormSubParameter(obj);
            return Ok(msg);
        }
        #region Form Module
        //public  IActionResult GetForms()
        //{
        //    var fms =  _FormsRepository.GetForms();
        //    return Ok(fms);
        //}

        //public async Task<IActionResult> GetFormModules()
        //{
        //    var fms = await _FormsRepository.GetFormModules();
        //    return Ok(fms);
        //}

        //public async Task<IActionResult> GetFormModulebyFormId(int formId)
        //{
        //    var fm = await _FormsRepository.GetFormModulebyFormId(formId);
        //    return Ok(fm);
        //}

        //public async Task<IActionResult> InsertIntoFormModule(DO_FormModule obj)
        //{
        //    var msg = await _FormsRepository.InsertIntoFormModule(obj);
        //    return Ok(msg);
        //}

        //public async Task<IActionResult> UpdateFormModule(DO_FormModule obj)
        //{
        //    var msg = await _FormsRepository.UpdateFormModule(obj);
        //    return Ok(msg);
        //}

        #endregion Form Module

        #region Area Controller
        [HttpGet]
        public async Task<IActionResult> GetAllAreaController()
        {
            var ar_ctrls = await _FormsRepository.GetAllAreaController();
            return Ok(ar_ctrls);
        }
        [HttpPost]
        public async Task<IActionResult> InsertIntoAreaController(DO_AreaController obj)
        {
            var msg = await _FormsRepository.InsertIntoAreaController(obj);
            return Ok(msg);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateAreaController(DO_AreaController obj)
        {
            var msg = await _FormsRepository.UpdateAreaController(obj);
            return Ok(msg);
        }
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveAreaController(bool status, int Id)
        {
            var msg = await _FormsRepository.ActiveOrDeActiveAreaController(status, Id);
            return Ok(msg);
        }
        #endregion
    }
}