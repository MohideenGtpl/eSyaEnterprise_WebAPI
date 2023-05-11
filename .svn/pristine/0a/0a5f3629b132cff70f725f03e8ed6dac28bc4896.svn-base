using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface ITaxIdentificationRepository
    {
        Task<List<DO_TaxIdentification>> GetTaxIdentificationByISDCode(int ISDCode);

        Task<DO_ReturnParameter> InsertIntoTaxIdentification(DO_TaxIdentification obj);

        Task<DO_ReturnParameter> UpdateTaxIdentification(DO_TaxIdentification obj);

        Task<DO_ReturnParameter> ActiveOrDeActiveTaxIdentification(bool status, int Isd_code, int TaxIdentificationId);
    }
}
