using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
    public interface IMobilePannelRepository
    {
        Task<List<DO_MobilePannel>> GetMobilePannelListbyTemplateType(string LanguageCode, string TemplateType);
        Task<DO_ReturnParameter> InsertIntoMobilePannel(DO_MobilePannel obj);
        Task<DO_ReturnParameter> UpdateMobilePannel(DO_MobilePannel obj);
        Task<DO_ReturnParameter> ActiveOrDeActiveMobilePannel(bool status, int TemplateId, string TemplateType, string LanguageCode);
        Task<DO_MobilePannel> GetMobilePannelbyTemplateType(string LanguageCode, string TemplateType, int TemplateId);
    }
}
