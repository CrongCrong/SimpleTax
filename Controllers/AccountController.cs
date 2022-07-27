using System.Web.Mvc;

namespace SimpleTax.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult LogIn()
        {

            if (Session[SessionStatus.Admin.ToString()] != null)
            {
                return RedirectToAction("Index", "Customer");
            }
            else
            {
                return View();
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(UserLogInControl userControl)
        {
            //DatabaseHelpers.InitializeUser();
            if (!ModelState.IsValid)
            {
                var userCtrl = new UserLogInControl();
                return View("LogIn", userCtrl);
            }
            User uuser = new User();
            uuser.Username = userControl.Username;
            uuser.Password = userControl.Password;
            userControl.user = uuser;
            User user = DatabaseHelpers.IfValidLogInAdmin(userControl.user);

            string str = SessionStatus.Admin.ToString();
            if (user != null)
            {
                if (user.IsAdmin)
                {
                    Session[SessionStatus.Admin.ToString()] = SessionStatus.Admin.ToString();
                    return RedirectToAction("Index", "Customer");
                }
                else
                {
                    Session[SessionStatus.Customer.ToString()] = SessionStatus.Customer.ToString();
                    return RedirectToAction("Home", "Customer", new { userId = user.Id.ToString() });
                }
            }
            else
            {
                var userCtrl = new UserLogInControl()
                { 
                    IncorrectLogin = true,
                    LoginMessage = "Incorrect username and password.",
                };
                return View(userCtrl);
            }
        }

        public ActionResult LogOut()
        {
            Session[SessionStatus.Admin.ToString()] = null;
            Session[SessionStatus.Customer.ToString()] = null;
            Session["userId"] = null;
            return RedirectToAction("LogIn", "Account");
        }
    }
}