using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineJobPortal.Controllers
{
    public class RecruiterController : Controller
    {
        Job_Portal_Context _context = new Job_Portal_Context();

        
        // GET: Recruiter
        public ActionResult Index() //int recruId
        {
            //Session["recruiterId"]=recruId;
            //if (Recuriter_Id > 0)
            //{
            //    Job_Listing job_Listing = new Job_Listing();
            //    job_Listing.Recuriter_Id = Recuriter_Id;
            //    job_Listing.Job_Tittle = Job_Tittle;
            //    job_Listing.Company_Name = Company_Name;
            //    job_Listing.Job_Location = Job_Location;
            //    job_Listing.Job_Type = Job_Type;
            //    job_Listing.Salary = Salary;
            //    job_Listing.Description = Description;
            //    job_Listing.Job_PstedDate = Job_PstedDate;
            //    _context.job_Listing.Add(job_Listing);
            //    _context.SaveChanges();
     
            //}
            return View(); 
        }
        public ActionResult Applications()
        {
            // Assuming `_context` is your database context
            var applications = _context.applications
                .Include("Job")
                .Include("Candidate_Profile")
                .ToList();

            return View(applications); // Pass the application data to the view
        }


        public ActionResult ApplicationDetails(int applicationId)
        {
            var application = _context.applications
                .Include("Job")
                .Include("Candidate_Profile")
                .FirstOrDefault(a => a.Application_Id == applicationId);

            if (application == null)
            {
                return HttpNotFound();
            }

            return View(application);
        }

        [HttpPost]
        public ActionResult PostJob(Job_Listing jobListing)
        {
            if (Session["recruiterId"] == null)
            {
                TempData["ErrorMessage"] = "You must be logged in to post a job.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                // Assign Recruiter_Id from Session
                jobListing.Recuriter_Id = (int)Session["recruiterId"];
                _context.job_Listing.Add(jobListing);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Job posted successfully!";
                return RedirectToAction("Index");
            }

            TempData["ErrorMessage"] = "Error while posting the job.";
            return RedirectToAction("Index");
        }

        public ActionResult AdminResponse()
        {

            try
            {
                // Retrieve jobs from both Approved_Jobs and Rejected_Jobs
                var approvedJobs = _context.approved_Jobs
                    .Select(job => new
                    {
                        job.Job_Tittle,
                        job.Company_Name,
                        job.Job_Location,
                        job.Job_Type,
                        job.Salary,
                        job.Description,
                        job.Job_PstedDate,
                        Status = "Approved"
                    }).ToList();

                var rejectedJobs = _context.rejected_Jobs
                    .Select(job => new
                    {
                        job.Job_Tittle,
                        job.Company_Name,
                        job.Job_Location,
                        job.Job_Type,
                        job.Salary,
                        job.Description,
                        job.Job_PstedDate,
                        Status = "Rejected" // Mark as rejected
                    }).ToList();

                // Combine the two lists
                var allJobs = approvedJobs.Concat(rejectedJobs).ToList();

                // Map the combined data to Job_Listing view model
                var jobListings = allJobs.Select(job => new Job_Listing
                {
                    Job_Tittle = job.Job_Tittle,
                    Company_Name = job.Company_Name,
                    Job_Location = job.Job_Location,
                    Job_Type = job.Job_Type,
                    Salary = job.Salary,
                    Description = job.Description,
                    Job_PstedDate = job.Job_PstedDate
                }).ToList();

                // If no jobs are found, return a message to the view
                if (!jobListings.Any())
                {
                    TempData["ErrorMessage"] = "No job listings available.";
                    return RedirectToAction("Index");
                }

                // Passing both the job listings and the IsApproved flags as a ViewBag
                ViewBag.JobStatus = allJobs.Select(job => job.Status).ToList();

                // Pass the list to the view
                return View(jobListings);
            }
            catch (Exception ex)
            {
                // Log the error and show an error message
                TempData["ErrorMessage"] = $"Error retrieving job listings. Details: {ex.Message}";
                return RedirectToAction("Index");
            }

        }

        public ActionResult ViewCandidates()
        {
            // Retrieve all candidate profiles from the database
            var candidateProfiles = _context.candidate_Profiles.ToList();

            // Pass the data to the view
            return View(candidateProfiles);
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

            // Store the status message in TempData to pass to the Candidate dashboard
            TempData["StatusMessage"] = $"Your application for the job '{application.Job.Job_Tittle}' has been {status}.";

            // Redirect to ViewApplications action in RecruiterController (or another valid action)
            return RedirectToAction("Applications", "Recruiter");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
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
