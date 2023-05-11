using eSyaConfigSetup.DO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eSyaConfigSetup.IF
{
   public interface IUnitofMeasureRepository
    {
        Task<List<DO_UnitofMeasure>> GetUnitofMeasurements();

        Task<DO_ReturnParameter> InsertOrUpdateUnitofMeasurement(DO_UnitofMeasure uoms);

        Task<DO_UnitofMeasure> GetUOMPDescriptionbyUOMP(string uomp);

        Task<DO_UnitofMeasure> GetUOMSDescriptionbyUOMS(string uoms);

        Task<DO_ReturnParameter> ActiveOrDeActiveUnitofMeasure(bool status, int unitId);
    }
}
