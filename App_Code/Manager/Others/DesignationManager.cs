using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using KHSC.Gateway.Others;
using System.Data;
using KHSC.DAO.Others;

namespace KHSC.Manager.Others
{
    public class DesignationManager
    {
        DesignationGateway aDesignationGatewayObj = new DesignationGateway();

        public string GetDesignationAutoId()
        {
            return aDesignationGatewayObj.GetDesignationAutoId();
        }

        public DataTable GetAllDesignationInformation()
        {
            DataTable table = aDesignationGatewayObj.GetAllDesignationInformation();
            return table;
        }

        public void SaveTheDesignationInformation(Designation aDesignationObj)
        {
            aDesignationGatewayObj.SaveTheDesignationInformation(aDesignationObj);
        }

        public void DeleteTheDesig(Designation aDesignationObj)
        {
            aDesignationGatewayObj.DeleteTheDesig(aDesignationObj);
        }

        public void UpdateTheDesig(Designation aDesignationObj)
        {
            aDesignationGatewayObj.UpdateTheOldDesigInforation(aDesignationObj);
        }
    }
}