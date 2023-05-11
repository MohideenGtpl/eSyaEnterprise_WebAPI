﻿using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface ITaxStructureRepository
    {
        Task<List<DO_TaxStructure>> GetTaxStructureByISDCode(int ISDCode);

        Task<List<DO_TaxStructure>> GetTaxStructureByTaxCode(int ISDCode, int taxCode);

        Task<DO_ReturnParameter> InsertOrUpdateTaxStructure(DO_TaxStructure obj);

        //Task<DO_ReturnParameter> UpdateTaxStructure(DO_TaxStructure obj);

        Task<List<DO_TaxStructure>> GetTaxCodeByISDCodes(int ISDCode);

        Task<DO_ReturnParameter> ActiveOrDeActiveTaxStructure(bool status, int Isd_code, int Taxcode);
    }
}
