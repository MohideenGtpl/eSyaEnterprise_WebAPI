using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface ILocalizationMasterRepository
    {
        #region Localization Master
        Task<List<DO_LocalizationMaster>> GetLocalizationTableMaster();

        Task<DO_ReturnParameter> InsertOrUpdateLocalizationTableMaster(DO_LocalizationMaster obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveLocalizationTableMaster(bool status, int Tablecode);
        #endregion

        #region Localization Language Mapping
        Task<List<DO_LocalizationMaster>> GetLocalizationMaster();

        Task<DO_ReturnParameter> InsertOrUpdateLocalizationLanguageMapping(List<DO_LocalizationLanguageMapping> obj);

        List<DO_LocalizationLanguageMapping> GetLocalizationLanguageMapping(string languageCode, int tableCode);

        #endregion

        #region Language Controller
        Task<List<string>> GetAllControllers();
        Task<List<DO_LanguageController>> GetLanguageControllersbyResource(string Resource);
        Task<DO_ReturnParameter> InsertOrUpdateLanguageController(DO_LanguageController lobj);
        Task<DO_ReturnParameter> ActiveOrDeActiveLanguageController(bool status, int ResourceId);
        #endregion Language Controller

        #region Language Culture
        Task<List<DO_LanguageController>> GetResources();

        Task<List<DO_LanguageCulture>> GetLanguageCulture(string Culture, string Resource);

        Task<DO_ReturnParameter> InsertOrUpdateLanguageCulture(List<DO_LanguageCulture> obj);
        #endregion Language Culture
        #region Culture Keys
        List<DO_LanguageCulture> GetDistinictCultureKeys(string Culture);
        Task<DO_ReturnParameter> InsertOrUpdateCultureKeys(List<DO_LanguageCulture> obj);
        #endregion Culture Keys
    }
}
