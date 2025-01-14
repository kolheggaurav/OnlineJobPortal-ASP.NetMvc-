using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OnlineJobPortal.Controllers
{
    public class HomeController : Controller
    {
        private readonly Job_Portal_Context _context;

        public HomeController()
        {
            _context = new Job_Portal_Context(); // Initialize the DbContext
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyOTP(string enteredOTP)
        {
            if (Session["OTP"] != null && Session["SignUpModel"] != null)
            {
                var sessionOtp = Session["OTP"].ToString();

                if (sessionOtp == enteredOTP)
                {
                    // OTP is valid, save the user
                    var model = (Registration)Session["SignUpModel"];
                    _context.registrations.Add(model);
                    _context.SaveChanges();

                    // Clear session
                    Session["OTP"] = null;
                    Session["SignUpModel"] = null;

                    TempData["SuccessMessage"] = "Registration successful!";
                    return RedirectToAction("Index", model.your_Role);
                }
                else
                {
                    TempData["ErrorMessage"] = "Invalid OTP. Please try again.";
                    return RedirectToAction("OTP");
                }
            }

            TempData["ErrorMessage"] = "Session expired. Please try again.";
            return RedirectToAction("Index");
        }

        public ActionResult OTP()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SaveSignUpData(Registration model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.registrations.FirstOrDefault(u => u.Email == model.Email);

                if (existingUser != null)
                {
                    TempData["ErrorMessage"] = "Email is already registered.";
                    return RedirectToAction("Index");
                }

                // Temporarily save user data in Session
                Session["SignUpModel"] = model;

                // Generate OTP
                var otp = new Random().Next(100000, 999999); // Generate a 6-digit OTP
                Session["OTP"] = otp; // Save OTP in session

                // Send OTP to the dynamically fetched email address
                try
                {
                    SendOTPEmail(model.Email, otp); // Call the email-sending method
                    TempData["SuccessMessage"] = "OTP has been sent to your email. Please verify.";
                    return RedirectToAction("OTP");
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Failed to send OTP. Error: {ex.Message}";
                    return RedirectToAction("Index");
                }
            }

            TempData["ErrorMessage"] = "Please fill in all required fields.";
            return RedirectToAction("Index");
        }

        private void SendOTPEmail(string recipientEmail, int otp)
        {
            try
            {
                // Create the email message
                MailMessage message = new MailMessage
                {
                    From = new MailAddress("jobportal9325@gmail.com"), // Replace with your email
                    Subject = "Your OTP for Job Portal",
                    Body = $"Your OTP is: {otp}",
                    IsBodyHtml = true // Set to true if you want to send HTML emails
                };

                message.To.Add(recipientEmail); // Add recipient's email address

                // Configure the SMTP client
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("jobportal9325@gmail.com", "cjwr irec kkmn mrld"), // Use your Gmail credentials
                    EnableSsl = true
                };

                // Send the email
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error sending email: {ex.Message}");
            }
        }


        [HttpPost]
        public ActionResult Log_In(string email, string password, string role)
        {
            // Validate user credentials
            var user = _context.registrations.FirstOrDefault(u => u.Email == email);
            Session["recruiterId"] = user.UserID;
            Session["CandidateID"] = user.UserID;
            Session["AdminID"] = user.UserID;

            //Session["Candidate_Id"] = user.UserID;

            //if (user != null && user.Email == "pratikshasurvase355@gmail.com" && user.password == "admin@123")
            //{
            //    if (role == "admin")
            //    {
            //        return RedirectToAction("Index", "Admin", new { adminid = user.UserID });
            //    }
            //}
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            if (user == null || user.password != password || user.your_Role != role)
            {
                TempData["LoginError"] = "Incorrect email or password.";
                return RedirectToAction("Index"); // Redirect to the same page with error
            }

            // Redirect based on role
            if (role == "admin")
                return RedirectToAction("Index", "Admin", new { adminid = user.UserID });
            else if (role == "recruiter")
                return RedirectToAction("Index", "Recruiter" ,new { recruId = user.UserID });
            else if (role == "candidate")
                return RedirectToAction("Index", "Candidate", new { candid = user.UserID });
            else
                return RedirectToAction("Index", "Home");

           
           }

        
        public ActionResult Jobs()
        {
            var approvedJobs = _context.approved_Jobs.ToList();
            return View(approvedJobs); // Pass approved jobs to the view
        }


        [HttpPost]
        public ActionResult Apply(int jobId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // User is not authenticated
                TempData["AlertMessage"] = "You need to log in to apply for a job.";
                return RedirectToAction("AvailableJobs");
            }

            // User is authenticated
            // Proceed with the application process
            // ...

            TempData["AlertMessage"] = "Your application has been submitted successfully.";
            return RedirectToAction("AvailableJobs");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose(); // Dispose of the DbContext
            }
            base.Dispose(disposing);
        }
    }
}