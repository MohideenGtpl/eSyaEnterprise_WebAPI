﻿using System.Collections.Generic;
using eSyaConfigSetup.DL.Repository;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusinessStructureController : ControllerBase
    {
        private readonly IBusinessStructureRepository _BusinessStructureRepository;
        public BusinessStructureController(IBusinessStructureRepository businessStructureRepository)
        {
            _BusinessStructureRepository = businessStructureRepository;
        }
        #region Business Entity
        /// <summary>
        /// Getting  Business Entity List.
        /// UI Reffered - Business Entity Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessEntities()
        {
            var b_entities =await _BusinessStructureRepository.GetBusinessEntities();
            return Ok(b_entities);
            
        }

        /// <summary>
        /// Getting  Business Entity Info.
        /// UI Reffered - Business Entity Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessEntityInfo(int BusinessId)
        {
            var b_entities = await _BusinessStructureRepository.GetBusinessEntityInfo(BusinessId);
            return Ok(b_entities);

        }

        /// <summary>
        /// Insert Or Update Business Entity .
        /// UI Reffered -Business Entity
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertBusinessEntity(DO_BusinessEntity businessentity)
        {
            var msg =await _BusinessStructureRepository.InsertBusinessEntity(businessentity);
            return Ok(msg);
        }

        /// <summary>
        /// Insert Business Entity .
        /// UI Reffered -Business Entity
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateBusinessEntity(DO_BusinessEntity businessentity)
        {
            var msg =await _BusinessStructureRepository.UpdateBusinessEntity(businessentity);
            return Ok(msg);
           
        }

        /// <summary>
        /// Delete Business Entity
        /// UI Reffered - Business Entity
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteBusinessEntity(int BusinessEntityId)
        {
            var msg = await _BusinessStructureRepository.DeleteBusinessEntity(BusinessEntityId);
            return Ok(msg);
        }

        /// <summary>
        /// Getting  Business Entity List for dropdown.
        /// UI Reffered - Business Segment & Business Location
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveBusinessEntities()
        {
            var b_entities =await _BusinessStructureRepository.GetActiveBusinessEntities();
            return Ok(b_entities);
            
        }

        #endregion  Business Entity

        #region Business Unit

        /// <summary>
        /// Getting  Business Units by Business Id.
        /// UI Reffered - Business Unit Grid
        /// </summary>
        /// <param name="BusinessId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBusinessUnitsbyBusinessId(int BusinessId)
        {
            var b_units= await _BusinessStructureRepository.GetBusinessUnitsbyBusinessId(BusinessId);
            return Ok(b_units);

        }

        /// <summary>
        /// Getting  Business Units which are Is Book of Account true.
        /// UI Reffered -Business Units Dropdown
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBusinessSubUnits(int BusinessId)
        {
            var sub_units = await _BusinessStructureRepository.GetBusinessSubUnits(BusinessId);
            return Ok(sub_units);

        }

        /// <summary>
        /// Insert Business Unit .
        /// UI Reffered -Business Unit
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertBusinessUnit(DO_BusinessUnit obj)
        {
            var msg = await _BusinessStructureRepository.InsertBusinessUnit(obj);
            return Ok(msg);
        }

        /// <summary>
        /// Insert Business Unit.
        /// UI Reffered -Business Unit
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateBusinessUnit(DO_BusinessUnit obj)
        {
            var msg = await _BusinessStructureRepository.UpdateBusinessUnit(obj);
            return Ok(msg);

        }

        /// <summary>
        /// Active Or De Active Business Unit.
        /// UI Reffered - Business Unit
        /// </summary>
        /// <param name="status-BusinessId-BusinessUnitId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ActiveOrDeActiveBusinessUnit(bool status, int BusinessId,int BusinessUnitId)
        {
            var ac = await _BusinessStructureRepository.ActiveOrDeActiveBusinessUnit(status, BusinessId, BusinessUnitId);
            return Ok(ac);
        }


        #endregion  Business Unit

        #region Business Segment
        /// <summary>
        /// Getting  Business Segment for Grid.
        /// UI Reffered - Business Segment
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessSegmentByBusinessId(int BusinessId)
        {
            var b_segmts =await _BusinessStructureRepository.GetBusinessSegmentByBusinessId(BusinessId);
            return Ok(b_segmts);
            
        }

        /// <summary>
        /// Getting  Business Segment List.
        /// UI Reffered - Business Segment
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessSegment()
        {
            var b_entities = await _BusinessStructureRepository.GetBusinessSegment();
            return Ok(b_entities);

        }

        /// <summary>
        /// Getting  Business Segment Info.
        /// UI Reffered - Business Segment
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessSegmentInfo(int BusinessId, int SegmentId)
        {
            var b_segmts = await _BusinessStructureRepository.GetBusinessSegmentInfo(BusinessId, SegmentId);
            return Ok(b_segmts);

        }

        /// <summary>
        /// Insert Business Segment .
        /// UI Reffered -Business Segment
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertBusinessSegment(DO_BusinessSegment BusinessSegment)
        {
            var msg =await _BusinessStructureRepository.InsertBusinessSegment(BusinessSegment);
            return Ok(msg);

        }

        /// <summary>
        /// Insert Or Update Segment .
        /// UI Reffered -Business Segment
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateBusinessSegment(DO_BusinessSegment BusinessSegment)
        {
            var msg =await _BusinessStructureRepository.UpdateBusinessSegment(BusinessSegment);
            return Ok(msg);

        }


        /// <summary>
        /// Delete Business Segment
        /// UI Reffered - Business Segment
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteBusinessSegment(int BusinessId, int SegmentId)
        {
            var msg = await _BusinessStructureRepository.DeleteBusinessSegment(BusinessId, SegmentId);
            return Ok(msg);
        }

        /// <summary>
        /// Getting  Active Business Segment  for dropdown.
        /// UI Reffered - Business Segment
        /// Parameter BusinessId
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetActiveBusinessSegmentbyBusinessId(int BusinessId)
        {
            var b_sgmts =await _BusinessStructureRepository.GetActiveBusinessSegmentbyBusinessId(BusinessId);
            return Ok(b_sgmts);
           
        }

        /// <summary>
        /// Is Multi Segment
        /// UI Reffered - Business Segment
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> IsAllowMultiSegment(int BusinessId)
        {
            var sp_Param = await _BusinessStructureRepository.IsAllowMultiSegment(BusinessId);
            return Ok(sp_Param);
        }
        #endregion Business Segment

        #region Business Location
        /// <summary>
        /// Getting  Business Location for Grid.
        /// UI Reffered - Business Location
        ///  Parameter BusinessId & SegmentId
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessLocationByBusinessIdandSegmentId(int BusinessId, int SegmentId)
        {
            var locs =await _BusinessStructureRepository.GetBusinessLocationByBusinessIdandSegmentId(BusinessId, SegmentId);
            return Ok(locs);
           
        }

        /// <summary>
        /// Getting  Business Location List.
        /// UI Reffered - Business Location
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessLocation()
        {
            var b_entities = await _BusinessStructureRepository.GetBusinessLocations();
            return Ok(b_entities);

        }

        /// <summary>
        /// Getting  Business Location Info.
        /// UI Reffered - Business Location
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessLocationInfo(int BusinessId, int SegmentId, int LocationId)
        {
            var b_segmts = await _BusinessStructureRepository.GetBusinessLocationInfo(BusinessId, SegmentId, LocationId);
            return Ok(b_segmts);

        }

        /// <summary>
        /// Insert Business Location .
        /// UI Reffered -Business Location
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertBusinessLocation(DO_BusinessLocation location)
        {
            var msg =await _BusinessStructureRepository.InsertBusinessLocation(location);
            return Ok (msg);

        }
        /// <summary>
        /// Insert  Business Location .
        /// UI Reffered -Business Location
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateBusinessLocation(DO_BusinessLocation location)
        {
            var msg =await _BusinessStructureRepository.UpdateBusinessLocation(location);
            return Ok(msg);
        }

        /// <summary>
        /// Delete Business Location
        /// UI Reffered - Business Location
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> DeleteBusinessLocation(int BusinessId, int SegmentId, int LocationId)
        {
            var msg = await _BusinessStructureRepository.DeleteBusinessLocation(BusinessId, SegmentId, LocationId);
            return Ok(msg);
        }
        
        /// <summary>
        /// Getting  Busienss Keys.
        /// UI Reffered - Report Header,Calendar Details
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessKey()
        {
            var b_keys = await _BusinessStructureRepository.GetBusinessKey();
            return Ok(b_keys);

        }

        /// <summary>
        /// Get Business Segment Currency .
        /// UI Reffered - Fill Grid for Business Segment Currency
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetBSCurrency(int BusinessKey)
        {
            var business_Key = await _BusinessStructureRepository.GetBSCurrency(BusinessKey);
            return Ok(business_Key);
        }

        /// <summary>
        /// Is Multi Segment
        /// UI Reffered - Business Location
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> IsAllowMultiLocation(int BusinessId, int SegmentId)
        {
            var sp_Param = await _BusinessStructureRepository.IsAllowMultiLocation(BusinessId, SegmentId);
            return Ok(sp_Param);
        }
        #endregion Business Location

        #region Business Stores
        /// <summary>
        /// InsertOrUpdate Business Stores.
        /// UI Reffered - Business Stores
        /// </summary>
        //[HttpPost]
        //public async Task<IActionResult> InsertOrUpdateBusinessStores(DO_BusinessStores store)
        //{
        //    var msg =await _BusinessStructureRepository.InsertOrUpdateBusinessStores(store);
        //    return Ok(msg);
        //}
        /// <summary>
        /// Getting Business Stores.
        /// UI Reffered - Business Stores Grid
        /// UI-Param-Businesskey
        /// </summary>
        //[HttpGet]
        //public async Task<IActionResult> GetBusinessStores(int Businesskey)
        //{
        //    var stores =await _BusinessStructureRepository.GetBusinessStores(Businesskey);
        //    return Ok(stores);
        //}
        #endregion Business Stores

        #region Business Statutory Details
        /// <summary>
        /// InsertOrUpdate Business Statutory Details.
        /// UI Reffered - Business Statutory Details
        /// </summary>
        //[HttpPost]
        //public async Task<IActionResult> InsertOrUpdateBusinessStatutoryDetails(DO_BusinessStatutoryDetails statutorydetails)
        //{
        //    var msg =await _BusinessStructureRepository.InsertOrUpdateBusinessStatutoryDetails(statutorydetails);
        //    return Ok (msg);
        //}
        /// <summary>
        /// Getting  Business Statutory Details.
        /// UI Reffered -  Business Statutory Details
        /// </summary>
        //[HttpGet]
        //public async Task<IActionResult> GetBusinessStatutoryDetails(int BusinessKey)
        //{
        //    var sta_details =await _BusinessStructureRepository.GetBusinessStatutoryDetails(BusinessKey);
        //    return Ok(sta_details);
        //}
        #endregion Business Statutory Details

        #region Business Subscription
        /// <summary>
        /// Getting  Business Subscription List.
        /// UI Reffered - Business Subscription Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetBusinessSubscription(int BusinessKey)
        {
            var b_entities = await _BusinessStructureRepository.GetBusinessSubscription(BusinessKey);
            return Ok(b_entities);

        }

        /// <summary>
        /// Update Business Subscription .
        /// UI Reffered -Business Subscription
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateBusinessSubscription(DO_BusinessSubscription businessubs)
        {
            var msg = await _BusinessStructureRepository.InsertOrUpdateBusinessSubscription(businessubs);
            return Ok(msg);
        }
        #endregion  Business Subscription

        #region Business Statutory
        /// <summary>
        /// Getting  Business Statutory List.
        /// UI Reffered - Business Statutory Grid
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetStatutoryInformation(int BusinessKey, int isdCode)
        {
            var b_entities = await _BusinessStructureRepository.GetStatutoryInformation(BusinessKey, isdCode);
            return Ok(b_entities);

        }

        /// <summary>
        /// Insert Business Statutory .
        /// UI Reffered -Business Statutory
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> InsertOrUpdateBusinessStatutory(List<DO_BusinessStatutoryDetails> sd)
        {
            var msg = await _BusinessStructureRepository.InsertOrUpdateBusinessStatutory(sd);
            return Ok(msg);

        }
        #endregion  Business Statutory
    }
}