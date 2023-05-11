﻿using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface IStoreMasterRepository
    {
        Task<List<DO_StoreMaster>> GetStoreCodes();

        Task<DO_StoreMaster> GetStoreParameterList(int StoreCode);

        Task<DO_ReturnParameter> InsertOrUpdateStoreCodes(DO_StoreMaster storecodes);

        Task<DO_ReturnParameter> DeleteStoreCode(int Storecode);

        Task<List<DO_StoreMaster>> GetActiveStoreCodes();

        Task<List<DO_StoreMaster>> GetStoreList(int BusienssKey);

        Task<DO_StoreBusinessLink> GetStoreBusinessLinkInfo(int BusinessKey, int StoreCode);

        Task<DO_ReturnParameter> InsertUpdateStoreBusinessLink(DO_StoreBusinessLink obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveStoreCode(bool status, string storetype, int storecode);

        Task<List<DO_Forms>> GetFormForStorelinking();
        Task<List<DO_StoreMaster>> GetStoreFormLinked(int formId);
        Task<DO_ReturnParameter> InsertIntoFormStoreLink(List<DO_StoreFormLink> l_obj);
    }
}
