using eSyaConfigSetup.DL.Repository;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FormApprovalController : ControllerBase
    {
       
        private readonly IFormApprovalRepository _formApprovalRepository;

        public FormApprovalController(IFormApprovalRepository formApprovalRepository)
        {
            _formApprovalRepository = formApprovalRepository;
        }

        #region Form Task Assignment

        /// <summary>
        /// Get Application Codes for specific Code Type.
        /// UI Reffered - Form Task Assignment, 
        /// </summary>
        /// <param name="codeType"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetApplicationCodesByCodeTypeList(List<int> l_codeType)
        {
            var ac = await _formApprovalRepository.GetApplicationCodesByCodeTypeList(l_codeType);
            return Ok(ac);
        }

        /// <summary>
        /// Get All Active forms for dropdown.
        /// UI Reffered -Form Task Assignment
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetAllActiveForms()
        {
            var forms = await _formApprovalRepository.GetAllActiveForms();
            return Ok(forms);
        }

        /// <summary>
        /// Get Form Task Assignment for Grid.
        /// UI Reffered -Form Task Assignment
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFormTaskAssignments()
        {
            var fm_task = await _formApprovalRepository.GetFormTaskAssignments();
            return Ok(fm_task);
        }

        /// <summary>
        /// Insert into Form Task Assignment
        /// UI Reffered - Form Task Assignment,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertFormTaskAssignment(DO_FormTaskAssign obj)
        {
            var msg = await _formApprovalRepository.InsertFormTaskAssignment(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Form Task Assignment
        /// UI Reffered - Form Task Assignment,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateFormTaskAssignment(DO_FormTaskAssign obj)
        {
            var msg = await _formApprovalRepository.UpdateFormTaskAssignment(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Active Or De Active Form Task Assignment.
        /// UI Reffered - Form Task Assignment
        /// </summary>
        /// <param name="status-formId-taskId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveFormTaskAssignment(bool status, int formId, int taskId)
        {
            var ac = await _formApprovalRepository.ActiveOrDeActiveFormTaskAssignment(status, formId,taskId);
            return Ok(ac);
        }
        #endregion

        #region Form Task Approval

        /// <summary>
        /// Get Form Task by Form Id  for dropdown.
        /// UI Reffered -Form Task Approval
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetFormTaskbyFormId(int formId)
        {
            var tasks =  _formApprovalRepository.GetFormTaskbyFormId(formId);
            return Ok(tasks);
        }

        /// <summary>
        /// Get Form Task Approval  for Grid.
        /// UI Reffered -Form Task Approval
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFormTaskApprovalsbyBusinesskey(int businesskey)
        {
            var fm_aprovals = await _formApprovalRepository.GetFormTaskApprovalsbyBusinesskey(businesskey);
            return Ok(fm_aprovals);
        }

        /// <summary>
        /// Insert into Form Task Approval
        /// UI Reffered - Form Task Approval,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertFormTaskApproval(DO_FormTaskApproval obj)
        {
            var msg = await _formApprovalRepository.InsertFormTaskApproval(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Form Task Approval
        /// UI Reffered - Form Task Approval,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateFormTaskApproval(DO_FormTaskApproval obj)
        {
            var msg = await _formApprovalRepository.UpdateFormTaskApproval(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Active Or De Active Form Task Approval.
        /// UI Reffered - Form Task Approval
        /// </summary>
        /// <param name="status-objform"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> ActiveOrDeActiveFormTaskApproval(DO_FormTaskApproval objform)
        {
            var ac = await _formApprovalRepository.ActiveOrDeActiveFormTaskApproval(objform);
            return Ok(ac);
        }
        #endregion
    }
}