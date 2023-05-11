using eSyaConfigSetup.DL.Repository;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace eSyaEnterprise_WebAPI.Areas.ConfigSetup.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class eSyaConfigureMenuController : ControllerBase
    {
        private readonly IConfigureMenuRepository _ConfigureMenuRepository;

        public eSyaConfigureMenuController(IConfigureMenuRepository ConfigureMenuRepository)
        {
            _ConfigureMenuRepository = ConfigureMenuRepository;
        }
        public async Task<IActionResult> GetMainMenuById(int mainMenuId)
        {
            var main_menus =await _ConfigureMenuRepository.GetMainMenuById(mainMenuId);
            return Ok(main_menus);
        }


        public async Task<IActionResult> InsertIntoMainMenu(DO_MainMenu obj)
        {
            var msg =await _ConfigureMenuRepository.InsertIntoMainMenu(obj);
            return Ok(msg);
        }

        public async Task<IActionResult> UpdateMainMenuIndex(int mainMenuId, bool isMoveUp, bool isMoveDown)
        {
            var msg =await _ConfigureMenuRepository.UpdateMainMenuIndex(mainMenuId, isMoveUp, isMoveDown);
            return Ok(msg);
        }

        public async Task<IActionResult> DeleteMainMenuByID(int mainMenuId)
        {
            var msg =await _ConfigureMenuRepository.DeleteMainMenu(mainMenuId);
            return Ok(msg);
        }


        public async Task<IActionResult> GetSubMenuById(int menuItemId)
        {
            var sub_menus =await _ConfigureMenuRepository.GetSubMenuById(menuItemId);
            return Ok(sub_menus);
        }

        public async Task<IActionResult> InsertIntoSubMenu(DO_SubMenu obj)
        {
            var msg =await _ConfigureMenuRepository.InsertIntoSubMenu(obj);
            return Ok(msg);
        }

        public async Task<IActionResult> UpdateSubMenusIndex(int menuItemId, bool isMoveUp, bool isMoveDown)
        {
            var msg =await _ConfigureMenuRepository.UpdateSubMenusIndex(menuItemId, isMoveUp, isMoveDown);
            return Ok(msg);
        }

        public async Task<IActionResult> DeleteSubMenuByID(int menuItemId)
        {
            var msg =await _ConfigureMenuRepository.DeleteSubMenu(menuItemId);
            return Ok(msg);
        }


        public async Task<IActionResult> GetFormDetailById(int mainMenuId, int menuItemId, int formId)
        {
            var form_details =await _ConfigureMenuRepository.GetFormDetailById(mainMenuId, menuItemId, formId);
            return Ok(form_details);
        }
        public async Task<IActionResult> InsertIntoFormMenu(DO_FormMenu obj)
        {
            var msg =await _ConfigureMenuRepository.InsertIntoFormMenu(obj);
            return Ok(msg);
        }

        public IActionResult UpdateFormsIndex(int mainMenuId, int menuItemId, int formId, bool isMoveUp, bool isMoveDown)
        {
            var msg = _ConfigureMenuRepository.UpdateFormsIndex(mainMenuId, menuItemId, formId, isMoveUp, isMoveDown);
            return Ok(msg);
        }

        public async Task<IActionResult> DeleteFormMenuByID(int mainMenuId, int menuItemId, int formId)
        {
            var msg =await _ConfigureMenuRepository.DeleteFormMenu(mainMenuId, menuItemId, formId);
            return Ok(msg);
        }
       

        public async Task<IActionResult> GetConfigureMenuMaster()
        { var config_menus = await _ConfigureMenuRepository.GetConfigureMenuMaster();
            return Ok(config_menus);
        }

        public async Task<IActionResult> GetConfigureMenulist()
        { var config_Menu = await _ConfigureMenuRepository.GetConfigureMenulist();
            return Ok(config_Menu);
        }
    }
}