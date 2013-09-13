using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mvc.Bootstrap.Web.Controllers
{
    public class DocumentationController : Controller
    {
        //
        // GET: /Documentation/

        public ActionResult Index()
        {
            return View();
        }

        // 
        // GET: /Forms/
        public ActionResult Forms()
        {
            return View(new Mvc.Bootstrap.Web.Models.SampleModel());
        }

        //
        // Handle any requests to /Documentation/* and show the view associated with the /* of the URL.
        protected override void HandleUnknownAction(string actionName)
        {
            this.View(actionName).ExecuteResult(this.ControllerContext);
        }

    }
}
