using eSyaConfigSetup.DL.Repository;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InventoryRulesController : ControllerBase
    {
        private readonly IInventoryRulesRepository _InventoryRulesRepository;
        public InventoryRulesController(IInventoryRulesRepository InventoryRulesRepository)
        {
            _InventoryRulesRepository = InventoryRulesRepository;
        }
        /// <summary>
        /// Getting  Inventory Rules List.
        /// UI Reffered - Inventory Rules Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetInventoryRules()
        {
            var inv_rules =await _InventoryRulesRepository.GetInventoryRules();
            return Ok(inv_rules);
        }
        /// <summary>
        /// Insert Inventory Rules .
        /// UI Reffered -Inventory Rules
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertInventoryRule(DO_InventoryRules inventoryRule)
        {
            var msg =await _InventoryRulesRepository.InsertInventoryRule(inventoryRule);
            return Ok(msg);

        }
        /// <summary>
        /// Update Inventory Rules .
        /// UI Reffered -Inventory Rules
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateInventoryRule(DO_InventoryRules inventoryRule)
        {
            var msg =await _InventoryRulesRepository.UpdateInventoryRule(inventoryRule);
            return Ok(msg);

        }
        /// <summary>
        /// Active Or De Active Inventory Rule.
        /// UI Reffered - Inventory Rule
        /// </summary>
        /// <param name="status-InventoryId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveInventoryRules(bool status, string InventoryId)
        {
            var msg = await _InventoryRulesRepository.ActiveOrDeActiveInventoryRules(status, InventoryId);
            return Ok(msg);
        }
    }
}