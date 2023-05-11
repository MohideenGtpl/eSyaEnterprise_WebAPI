using eSyaConfigSetup.DL.Entities;
using eSyaConfigSetup.DO;
using eSyaConfigSetup.IF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.DL.Repository
{
    public class UserAccountRepository: IUserAccountRepository
    {
        public async Task<List<DO_MainMenu>> GeteSyaMenulist()
        {
            try
            {
                using (eSyaEnterprise db = new eSyaEnterprise())
                {
                    var menuList = db.GtEcmamn.Where(w => w.ActiveStatus == true)
                                   .Select(m => new DO_MainMenu()
                                   {
                                       MainMenuId = m.MainMenuId,
                                       MainMenu = m.MainMenu,
                                       MenuIndex = m.MenuIndex,
                                       l_SubMenu = db.GtEcsbmn.Where(w => w.MainMenuId == m.MainMenuId && w.ActiveStatus == true)
                                        .Select(s => new DO_SubMenu()
                                        {
                                            MainMenuId = s.MainMenuId,
                                            MenuItemId = s.MenuItemId,
                                            MenuItemName = s.MenuItemName,
                                            MenuIndex = s.MenuIndex,
                                            l_FormMenu = db.GtEcmnfl
                                                        .Join(db.GtEcfmnm,
                                                        f => f.FormId,
                                                        i => i.FormId,
                                                        (f,i)=>new { f,i})
                                                .Where(w => w.f.MenuItemId == s.MenuItemId && w.f.ActiveStatus == true)
                                                .AsQueryable()
                                                .Select(f => new DO_FormMenu()
                                                {
                                                    FormId = f.f.FormId,
                                                    FormInternalID = f.i.FormIntId,
                                                    FormNameClient = f.f.FormNameClient,
                                                    NavigateUrl = f.i.NavigateUrl,
                                                    Area = f.i.NavigateUrl.Split('/', StringSplitOptions.None)[0],
                                                    Controller = f.i.NavigateUrl.Split('/', StringSplitOptions.None)[1],
                                                    View = f.i.NavigateUrl.Split('/', StringSplitOptions.None)[2],
                                                    FormIndex = f.f.FormIndex,
                                                    MenuKey = f.f.MenuKey
                                                }).ToList(),

                                        }).ToList()
                                   });
                    return await menuList.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
