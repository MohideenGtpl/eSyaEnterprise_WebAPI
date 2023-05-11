using eSyaConfigSetup.DL.Repository;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UnitofMeasureController : ControllerBase
    {
        private readonly IUnitofMeasureRepository _UnitofMeasureRepository;
        public UnitofMeasureController(IUnitofMeasureRepository UnitofMeasureRepository)
        {
            _UnitofMeasureRepository = UnitofMeasureRepository;
        }
        /// <summary>
        /// Getting  Unit of Measure List.
        /// UI Reffered - Unit of Measure Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUnitofMeasurements()
        {
            var unit_measures =await _UnitofMeasureRepository.GetUnitofMeasurements();
            return Ok(unit_measures);
        }
        /// <summary>
        /// Insert Or Update Unit of Measure .
        /// UI Reffered -Unit of Measure
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateUnitofMeasurement(DO_UnitofMeasure uoms)
        {
            var msg = await _UnitofMeasureRepository.InsertOrUpdateUnitofMeasurement(uoms);
            return Ok(msg);

        }
        /// <summary>
        /// Getting  Unit of Measure Purchase description by UOMP Code.
        /// UI Reffered - Unit of Measure
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUOMPDescriptionbyUOMP(string uomp)
        {
            var unit_measures = await _UnitofMeasureRepository.GetUOMPDescriptionbyUOMP(uomp);
            return Ok(unit_measures);
        }
        /// <summary>
        /// Getting  Unit of Measure Stock description by UOMS Code.
        /// UI Reffered - Unit of Measure
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetUOMSDescriptionbyUOMS(string uoms)
        {
            var unit_measures = await _UnitofMeasureRepository.GetUOMSDescriptionbyUOMS(uoms);
            return Ok(unit_measures);
        }
        /// <summary>
        /// Active Or De Active Unit of Measurement.
        /// UI Reffered - Unit of Measurement
        /// </summary>
        /// <param name="status-unitId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveUnitofMeasure(bool status, int unitId)
        {
            var msg = await _UnitofMeasureRepository.ActiveOrDeActiveUnitofMeasure(status, unitId);
            return Ok(msg);
        }
    }
}