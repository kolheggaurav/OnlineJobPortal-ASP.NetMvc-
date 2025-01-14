using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;


namespace OnlineJobPortal.Controllers
{
    public class CandidateController : Controller
    {
        private readonly Job_Portal_Context _context = new Job_Portal_Context();
        // GET: Candidate
        public ActionResult Index()
        {
            return View();
        }

        // Action to display approved jobs
        public ActionResult Jobs()
        {
            
            var approvedJobs = _context.approved_Jobs.ToList();
            return View(approvedJobs); // Pass approved jobs to the view
        }
        public ActionResult Resume_Template()
        {
            return View();
        }

        public new ActionResult Profile()
        {
            return View(new Candidate_Profile());
            ;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProfile(Candidate_Profile model, HttpPostedFileBase ResumePdf)
        {
            if (ModelState.IsValid)
            {
                var userId = Session["CandidateID"] as int?;

                if (userId == null)
                {
                    TempData["ErrorMessage"] = "User is not logged in.";
                    return RedirectToAction("Index", "Home");
                }

                // Assign the UserID to the Candidate_Profile model
                model.User_Id = userId.Value;

                // Handle file upload if the user selects a file
                if (ResumePdf != null && ResumePdf.ContentLength > 0)
                {
                    // Save the file to the server or database
                    var fileName = Path.GetFileName(ResumePdf.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Content/pdf"), fileName);

                    // Save the file in the "Content/pdf" folder
                    ResumePdf.SaveAs(filePath);

                    // Save file name to the database
                    model.ResumePdf = fileName;

                    // Convert the file to byte[] and save it to the database
                    using (var binaryReader = new BinaryReader(ResumePdf.InputStream))
                    {
                        model.ResumeFile = binaryReader.ReadBytes(ResumePdf.ContentLength);
                    }
                }

                // Save the profile details in the database
                _context.candidate_Profiles.Add(model);
                _context.SaveChanges();

                // Redirect to the profile page or any other page after saving
                return RedirectToAction("Profile");
            }

            // Return the model to the view if validation fails
            return View(model);
        }


        [HttpPost]
        public ActionResult Apply(int jobId)
        {
            if (Session["CandidateID"] == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to apply for a job.";
                return RedirectToAction("Jobs");
            }

            int userId = (int)Session["CandidateID"];

            // Fetch the Approved_Jobs details
            var job = _context.approved_Jobs.FirstOrDefault(j => j.Id == jobId);
            if (job == null)
            {
                TempData["ErrorMessage"] = "Job not found.";
                return RedirectToAction("Jobs");
            }

            // Fetch the Candidate_Profile
            var candidateProfile = _context.candidate_Profiles.FirstOrDefault(c => c.User_Id == userId);
            if (candidateProfile == null)
            {
                TempData["ErrorMessage"] = "Please complete your profile before applying.";
                return RedirectToAction("Profile");
            }

            // Create a new Application entry
            var application = new Application
            {
                Job_Id = job.Id,
                Candidate_Id = candidateProfile.Profile_Id,
                Status = "Pending",
                AppliedDate = DateTime.Now
            };

            try
            {
                _context.applications.Add(application);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Your application has been submitted successfully.";

                // Redirect to a view displaying the application details
                return RedirectToAction("ApplicationDetails", new { applicationId = application.Application_Id });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred: {ex.Message}";
                return RedirectToAction("Jobs");
            }
        }

        public ActionResult ApplicationDetails(int applicationId)
        {
            var application = _context.applications
                .Include("Job")
                .Include("Candidate_Profile")
                .FirstOrDefault(a => a.Application_Id == applicationId);

            if (application == null)
            {
                TempData["ErrorMessage"] = "Application not found.";
                return RedirectToAction("Jobs");
            }

            return View(application);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Status(int applicationId, string status)
        {
            if (string.IsNullOrEmpty(status) || applicationId == 0)
            {
                return RedirectToAction("Index", "Home"); // Or any other fallback page
            }

            var application = _context.applications
                .Include("Job")
                .Include("Candidate_Profile")
                .FirstOrDefault(a => a.Application_Id == applicationId);

            if (application == null)
            {
                return HttpNotFound();
            }

            // Update the application status
            application.Status = status;
            _context.SaveChanges();

            // Notify the candidate (can be via email or in-app notification)
            TempData["Message"] = $"The application has been {status}. The candidate has been notified.";

            // Redirect to ViewApplications action in RecruiterController (or another valid action)
            return RedirectToAction("Applications", "Recruiter"); // This assumes 'ViewApplications' exists in 'RecruiterController'
        }

        public ActionResult CandidateDashboard()
        {
            var userId = Session["CandidateID"] as int?;
            if (userId == null)
            {
                TempData["ErrorMessage"] = "User is not logged in.";
                return RedirectToAction("Index", "Home");
            }

            // Get the candidate's applications
            var applications = _context.applications
                .Where(a => a.Candidate_Id == userId)
                .Include("Job")
                .Include("Candidate_Profile")
                .ToList();

            return View(applications);
        }


         public ActionResult AppliedJob()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogoutAction()
        {
            // Clear the session
            Session.Clear();
            Session.Abandon();

            // Clear authentication
            FormsAuthentication.SignOut();

            // Optionally clear TempData
            TempData.Clear();

            // Redirect to the Home controller's Index action
            return RedirectToAction("Index", "Home");

        }
    }


}