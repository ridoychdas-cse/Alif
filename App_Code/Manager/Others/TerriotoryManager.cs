using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KHSC.Gateway.Others;
using System.Data;
using KHSC.DAO.Others;

namespace KHSC.Manager.Others
{
    public class TerriotoryManager
    {
        TerriotoryGateway aTerriotoryGatewayObj = new TerriotoryGateway();

        internal string GetTerriotoryAutoId()
        {
            return aTerriotoryGatewayObj.GetTerriotoryAutoId();
        }

        internal DataTable GetAllTerriotoryInformaation()
        {
            DataTable table = aTerriotoryGatewayObj.GetAllTerriotoryInformaation();
            return table;
        }

        internal void SaveTheTerrioToryInformation(Terriotory aTerriotoryObj)
        {
            aTerriotoryGatewayObj.SaveTheTerrioToryInformation(aTerriotoryObj);
        }

        internal void UpdateTheTerrioToryInformation(Terriotory aTerriotoryObj)
        {
            aTerriotoryGatewayObj.UpdateTheTerrioToryInformation(aTerriotoryObj);
        }

        internal void DeleteTheTerrioToryInformation(Terriotory aTerriotoryObj)
        {
            aTerriotoryGatewayObj.DeleteTheTerrioToryInformation(aTerriotoryObj);
        }
    }
}