using System.Web.Mvc;

namespace CardChequeModule.Areas.CardCheque
{
    public class CardChequeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CardCheque";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CardCheque_default",
                "CardCheque/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}