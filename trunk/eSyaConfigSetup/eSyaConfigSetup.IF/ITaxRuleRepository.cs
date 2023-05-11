﻿using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface ITaxRuleRepository
    {
        Task<List<DO_TaxRule>> GetTaxRuleByISDandTaxCode(int ISDCode, int TaxCode);

        Task<DO_ReturnParameter> InsertIntoTaxRule(DO_TaxRule obj);

        Task<DO_ReturnParameter> UpdateTaxRule(DO_TaxRule obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveTaxRule(bool status, int Isd_code, int Taxcode, int serialNumber);
    }
}
