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
using ISeeYou.Views;
using ISeeYou.ViewServices;

namespace ISeeYou.Web.Controllers
{
    [RoutePrefix("payments")]
    public class PaymentsController : BaseController
    {
        private readonly UsersViewService _users;
        private readonly SubjectViewService _subjects;
        private readonly SiteSettings _settings;
        private readonly CouponsViewService _coupons;

        public PaymentsController(UsersViewService users, SubjectViewService subjects, SiteSettings settings, CouponsViewService coupons)
        {
            _users = users;
            _subjects = subjects;
            _settings = settings;
            _coupons = coupons;
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
                subject.SetPayment();
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

        [POST("Coupon")]
        public ActionResult Coupon(CouponViewModel model)
        {
            if (!ModelState.IsValid) return View("Create", new PaymentViewModel(){SubjectId = model.SubjectId, CuponToken = model.Token});
            var coupon = _coupons.GetCoupon();
            if (coupon.Amount <= 0 || !IsValidCoupon(coupon, model.Token))
            {
                ModelState.AddModelError("", "Coupon token not valid");
                return View("Create", new PaymentViewModel() { SubjectId = model.SubjectId, CuponToken = model.Token});
            }
            var user = _users.GetById(UserId);
            var subject = user.Subjects.FirstOrDefault(x => x.Id == model.SubjectId);
            if (subject == null || subject.Paid)
            {
                return RedirectToAction("Index", "Subjects");
            }
            _coupons.Inc(coupon.Id, x => x.Amount, -1);
            subject.SetPayment();
            _users.Save(user);
            _subjects.Set(int.Parse(subject.Id), x => x.Active, true);

            return RedirectToAction("Index", "Profile");
        }

        private bool IsValidCoupon(CouponView coupon, string couponToken)
        {
            return coupon != null && coupon.Amount > 0 && coupon.Token == couponToken.ToLower().Trim();
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
        public string CuponToken { get; set; }
    }

    public class CouponViewModel
    {
        public string SubjectId { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
