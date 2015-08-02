using System.Web.Mvc;

namespace CardChequeModule.Areas.ChequeRequisition
{
    public class ChequeRequisitionAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ChequeRequisition";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ChequeRequisition_default",
                "ChequeRequisition/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}