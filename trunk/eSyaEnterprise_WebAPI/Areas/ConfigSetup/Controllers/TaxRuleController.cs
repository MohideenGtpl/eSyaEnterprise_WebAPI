using eSyaConfigSetup.DL.Repository;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TaxRuleController : ControllerBase
    {
        private readonly ITaxRuleRepository _TaxRuleRepository;
        public TaxRuleController(ITaxRuleRepository TaxRuleRepository)
        {
            _TaxRuleRepository = TaxRuleRepository;
        }
        /// <summary>
        /// Get Tax Rule for specific ISD Code & Tax Structure.
        /// UI Reffered - Fill Grid on ISD Code & Tax Structure selection in Tax Rule
        /// </summary>
        /// <param name="CountryCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTaxRuleByISDandTaxCode(int ISDCode, int TaxCode)
        {
            var tax_rules =await _TaxRuleRepository.GetTaxRuleByISDandTaxCode(ISDCode, TaxCode);
            return Ok(tax_rules);
        }
        
        /// <summary>
        /// Insert into Tax Structure Table
        /// UI Reffered - Tax Structure,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoTaxRule(DO_TaxRule obj)
        {
            var msg =await _TaxRuleRepository.InsertIntoTaxRule(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Update into Tax Structure Table
        /// UI Reffered - Tax Structure,
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateTaxRule(DO_TaxRule obj)
        {
            var msg =await _TaxRuleRepository.UpdateTaxRule(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Active Or De Active Tax Rule.
        /// UI Reffered - Tax Rule
        /// </summary>
        /// <param name="status-Isd_code-Taxcode-serialNumber"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveTaxRule(bool status, int Isd_code, int Taxcode, int serialNumber)
        {
            var msg = await _TaxRuleRepository.ActiveOrDeActiveTaxRule(status, Isd_code, Taxcode, serialNumber);
            return Ok(msg);
        }
    }
}