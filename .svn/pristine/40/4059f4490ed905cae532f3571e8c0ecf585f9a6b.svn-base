using eSyaConfigSetup.DL.Repository;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportHeaderController : ControllerBase
    {
        private readonly IReportHeaderRepository _ReportHeaderRepository;
        public ReportHeaderController(IReportHeaderRepository ReportHeaderRepository)
        {
            _ReportHeaderRepository = ReportHeaderRepository;
        }
        /// <summary>
        /// Getting  Report Header List by BusinessKey .
        /// UI Reffered - Report Header Grid
        /// UI Param-BusinessKey
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetReportHeaderList(int BusinessKey)
        {
            var rept_headers =await _ReportHeaderRepository.GetReportHeaderList(BusinessKey);
            return Ok(rept_headers);
        }

        /// <summary>
        /// Insert into Report Header .
        /// UI Reffered -Report Header
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateReportHeader(DO_ReportHeader rpHeader)
        {
            var msg = await _ReportHeaderRepository.InsertOrUpdateReportHeader(rpHeader);
            return Ok(msg);
          
        }
        /// <summary>
        /// Active Or De Active Report Header.
        /// UI Reffered - Report Header
        /// </summary>
        /// <param name="status-Businesskey-ReportHeaderId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveGeneralReport(bool status, int Businesskey, int ReportHeaderId)
        {
            var msg = await _ReportHeaderRepository.ActiveOrDeActiveGeneralReport(status, Businesskey, ReportHeaderId);
            return Ok(msg);
        }
    }
}