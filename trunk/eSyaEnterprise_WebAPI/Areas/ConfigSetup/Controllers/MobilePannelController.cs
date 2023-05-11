using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MobilePannelController : ControllerBase
    {
        private readonly IMobilePannelRepository _MobilePannelRepository;
        public MobilePannelController(IMobilePannelRepository MobilePannelRepository)
        {
            _MobilePannelRepository = MobilePannelRepository;


        }
        #region Mobile Pannel
        /// <summary>
        /// Mobile Pannel for Grid.
        /// UI Reffered - Mobile Pannel
        /// </summary>
        /// <param name="LanguageCode-TemplateType"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMobilePannelListbyTemplateType(string LanguageCode, string TemplateType)
        {
            var mob_pannels = await _MobilePannelRepository.GetMobilePannelListbyTemplateType(LanguageCode, TemplateType);
            return Ok(mob_pannels);
        }
        /// <summary>
        /// Insert into Mobile Pannel.
        /// UI Reffered - Mobile Pannel
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertIntoMobilePannel(DO_MobilePannel obj)
        {
            var msg = await _MobilePannelRepository.InsertIntoMobilePannel(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Update into Mobile Pannel.
        /// UI Reffered - Mobile Pannel
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateMobilePannel(DO_MobilePannel obj)
        {
            var msg = await _MobilePannelRepository.UpdateMobilePannel(obj);
            return Ok(msg);

        }
        /// <summary>
        /// Active Or De Active Mobile Pannel.
        /// UI Reffered - Mobile Pannel
        /// </summary>
        /// <param name="status-TemplateId-TemplateType-LanguageCode"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveMobilePannel(bool status, int TemplateId, string TemplateType, string LanguageCode)
        {
            var msg = await _MobilePannelRepository.ActiveOrDeActiveMobilePannel(status, TemplateId, TemplateType,LanguageCode);
            return Ok(msg);
        }

        /// <summary>
        /// Mobile Pannel for Grid.
        /// UI Reffered - Mobile Pannel
        /// </summary>
        /// <param name="LanguageCode-TemplateType-TemplateId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMobilePannelbyTemplateType(string LanguageCode, string TemplateType, int TemplateId)
        {
            var mob_pannel = await _MobilePannelRepository.GetMobilePannelbyTemplateType(LanguageCode, TemplateType, TemplateId);
            return Ok(mob_pannel);
        }
        #endregion
    }
}