using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;

namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentCodesRepository _DepartmentCodesRepository;

        public DepartmentController(IDepartmentCodesRepository departmentCodesRepository)
        {
            _DepartmentCodesRepository = departmentCodesRepository;
        }
        #region Department Code
        /// <summary>
        /// Get Department Codes.
        /// UI Reffered - Department Code, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDepartmentCodes()
        {
            var ac = await _DepartmentCodesRepository.GetDepartmentCodes();
            return Ok(ac);
        }

        /// <summary>
        /// Insert into Department Codes Table
        /// UI Reffered - Department Code,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoDepartmentCodes(DO_DepartmentCodes obj)
        {
            var msg = await _DepartmentCodesRepository.InsertIntoDepartmentCodes(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Department Codes Table
        /// UI Reffered - Department Code,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateDepartmentCodes(DO_DepartmentCodes obj)
        {
            var msg = await _DepartmentCodesRepository.UpdateDepartmentCodes(obj);
            return Ok(msg);
        }
        /// <summary>
        /// Active Or De Active Department Codes.
        /// UI Reffered - Department Codes
        /// </summary>
        /// <param name="status-deptId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveDepartmentCodes(bool status, int deptId)
        {
            var msg = await _DepartmentCodesRepository.ActiveOrDeActiveDepartmentCodes(status, deptId);
            return Ok(msg);
        }
        #endregion Department Code
        #region Department Location Link
        /// <summary>
        /// Get Department Location Link.
        /// UI Reffered - Department Location Link, 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetDepartmentLocation(int businessKey, int departmentId)
        {
            var ac = await _DepartmentCodesRepository.GetDepartmentLocation(businessKey, departmentId);
            return Ok(ac);
        }

        /// <summary>
        /// Insert into Department Location Link
        /// UI Reffered - Department Locatino Link,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoDepartmentLocationLink(DO_DepartmentLocation obj)
        {
            var msg = await _DepartmentCodesRepository.InsertIntoDepartmentLocationLink(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Department Location Link
        /// UI Reffered - Department Location,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateDepartmentLocationLink(DO_DepartmentLocation obj)
        {
            var msg = await _DepartmentCodesRepository.UpdateDepartmentLocationLink(obj);
            return Ok(msg);
        }
        /// <summary>
        /// Active Or De Active Department Location Link.
        /// UI Reffered - Department Location Link
        /// </summary>
        /// <param name="status-deptId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveDepartmentLocationLink(bool status, int deptId, int deptlocId, int Businesskey)
        {
            var msg = await _DepartmentCodesRepository.ActiveOrDeActiveDepartmentLocationLink(status, deptId, deptlocId,Businesskey);
            return Ok(msg);
        }
        #endregion Department Location Link
    }
}
