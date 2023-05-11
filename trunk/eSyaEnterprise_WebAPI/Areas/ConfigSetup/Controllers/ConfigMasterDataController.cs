﻿using eSyaConfigSetup.DL.DataLayer;
using eSyaConfigSetup.DO;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ConfigMasterDataController : ControllerBase
    {

        [HttpGet]
        public async Task<IActionResult> GetApplicationCodesByCodeType(int codeType)
        {
            var ds =await new CommonMethod().GetApplicationCodesByCodeType(codeType);
            return Ok(ds);
        }

        [HttpPost]
        public async Task<IActionResult> GetApplicationCodesByCodeTypeList(List<int> l_codeType)
        {
            var ds =await new CommonMethod().GetApplicationCodesByCodeTypeList(l_codeType);
            return Ok(ds);
        }

        /// <summary>
        /// Get Business key.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var ds = await new CommonMethod().GetBusinessKey();
            return Ok(ds);
        }

        /// <summary>
        /// Get ISDCodes.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetISDCodes()
        {
            var ds = await new CommonMethod().GetISDCodes();
            return Ok(ds);
        }

        /// <summary>
        /// Get Active Tax Codes by ISD Codes.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTaxCodeByISDCodes(int ISDCode)
        {
            var ds = await new CommonMethod().GetTaxCodeByISDCodes(ISDCode);
            return Ok(ds);
        }


        ///// <summary>
        ///// Get Language.
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public async Task<IActionResult> GetLanguage()
        //{
        //    var ds =await new CommonMethod().GetLanguage();
        //    return Ok(ds);
        //}

        /// <summary>
        /// Get Form Detail.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetFormDetails()
        {
            var ds = await new CommonMethod().GetFormDetails();
            return Ok(ds);
        }

        /// <summary>
        /// Get Active Tax Codes by ISD Codes AND Split Applicable True Only.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTaxCode(int ISDCode)
        {
            var ds = await new CommonMethod().GetTaxCode(ISDCode);
            return Ok(ds);
        }

        /// <summary>
        /// Get ISDCodes.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetIndiaISDCodes()
        {
            var ds = await new CommonMethod().GetIndiaISDCodes();
            return Ok(ds);
        }

        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTaxCodesListByISDCode(int Isdcode)
        {
            var ds = await new CommonMethod().GetTaxCodesListByISDCode(Isdcode);
            return Ok(ds);
        }

        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetISDCodeByBusinessKey(int BusinessKey)
        {
            var ds = await new CommonMethod().GetISDCodeByBusinessKey(BusinessKey);
            return Ok(ds);
        }
        /// <summary>
        /// Get Active Currencies for dropdown.
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetActiveCurrencyCodes()
        {
            var ds = await new CommonMethod().GetActiveCurrencyCodes();
            return Ok(ds);
        }
    }
}