using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using ISeeYou.Domain.ApplicationServices.Payments;
using ISeeYou.Platform.Mvc;
using ISeeYou.ViewServices;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("payments")]
    public class PaymentsController : BaseController
    {
        private readonly UsersViewService _users;
        private readonly SubjectViewService _subjects;
        private readonly SiteSettings _settings;

        public PaymentsController(UsersViewService users, SubjectViewService subjects, SiteSettings settings)
        {
            _users = users;
            _subjects = subjects;
            _settings = settings;
        }

        [GET("Create")]
        public ActionResult Create(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("Index", "Subjects");
            }

            var user = _users.GetById(UserId);

            var subject = user.Subjects.FirstOrDefault(x => x.Id == id);
            if (subject == null || subject.Paid)
            {
                return RedirectToAction("Index", "Subjects");
            }

            var model = new PaymentViewModel()
            {
                SubjectId = id,
            };

            return View(model);
        }

        [POST("Create")]
        public ActionResult Create(PaymentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var user = _users.GetById(UserId);
            var subject = user.Subjects.FirstOrDefault(x => x.Id == model.SubjectId);
            if (subject == null || subject.Paid)
            {
                return RedirectToAction("Index", "Subjects");
            }
            var paymentService = new PaymentApplicationService(_settings);
            var creditCard = new CreditCard()
            {
                Cvv = model.Cvv,
                Month = model.ExpirationMounth,
                Year = model.ExpirationYear,
                Number = model.CardNumber
            };

            try
            {
                var processResult = paymentService.Process(user, creditCard);
                if (!processResult.Success)
                {
                     ModelState.AddModelError("", string.Join("<br />", processResult.Errors));
                     return View(model);
                }
                user.BraintreeCustomerId = processResult.CustomerId;
                subject.SubscriptionId = processResult.SubscriptionId;
                subject.NextPayment = DateTime.UtcNow.AddMonths(1);
                subject.Stopped = null;
                _users.Save(user);
                _subjects.Set(int.Parse(subject.Id), x => x.Active, true);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
            return RedirectToAction("Index", "Profile");
        }

    }

    public class PaymentViewModel
    {
        [Required]
        public string CardNumber { get; set; }
        [Required]
        public string Cvv { get; set; }
        [Required]
        public string ExpirationMounth { get; set; }
        [Required]
        public string ExpirationYear { get; set; }
        [Required]
        public string SubjectId { get; set; }

    }
}
