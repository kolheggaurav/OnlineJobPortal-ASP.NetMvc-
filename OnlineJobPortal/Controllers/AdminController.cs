using OnlineJobPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OnlineJobPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly Job_Portal_Context _context = new Job_Portal_Context();

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        // Action to display the list of jobs
        public ActionResult Action()
        {
            try
            {
                var jobs = _context.job_Listing.ToList();
                return View(jobs);
            }
            catch (Exception ex)
            {
                // Log the error (use a logging library like Serilog or log4net)
                throw new Exception("Error retrieving job listings", ex);
            }
        }

        [HttpPost]
        public ActionResult ApproveJob(int jobId)
        {
            // Find the job in Job_Listing table
            var job = _context.job_Listing.Find(jobId);
            if (job != null)
            {
                // Create a new record for Approved_Jobs table
                var approvedJob = new Approved_Jobs
                {
                    Job_Tittle = job.Job_Tittle,
                    Company_Name = job.Company_Name,
                    Job_Location = job.Job_Location,
                    Job_Type = job.Job_Type,
                    Salary = job.Salary,
                    Description = job.Description,
                    Job_PstedDate = job.Job_PstedDate
                };

                // Add approved job to the Approved_Jobs table
                _context.approved_Jobs.Add(approvedJob);

                // Remove the job from Job_Listing table (job approved)
                _context.job_Listing.Remove(job);
                _context.SaveChanges();
            }

            return RedirectToAction("Action");
        }

        [HttpPost]
        public ActionResult RejectJob(int jobId)
        {
            // Find the job in Job_Listing table
            var job = _context.job_Listing.Find(jobId);
            if (job != null)
            {
                // Create a new record for Approved_Jobs table
                var rejectedjob = new RejectedJob
                {
                    Job_Tittle = job.Job_Tittle,
                    Company_Name = job.Company_Name,
                    Job_Location = job.Job_Location,
                    Job_Type = job.Job_Type,
                    Salary = job.Salary,
                    Description = job.Description,
                    Job_PstedDate = job.Job_PstedDate
                };

                // Add approved job to the Approved_Jobs table
                _context.rejected_Jobs.Add(rejectedjob);

                // Remove the job from Job_Listing table (job approved)
                _context.job_Listing.Remove(job);
                _context.SaveChanges();
            }

            return RedirectToAction("Action");
        }
        
        // this is the running state code 
        //public ActionResult TotalUser()
        //{
        //    // Fetch all registered users from the Registration table
        //    var users = _context.registrations.ToList();
        //    if (users == null || users.Count == 0)
        //    {
        //        TempData["Message"] = "No users found.";
        //    }
        //    return View(users); // Pass users to the view }
        //}




        public ActionResult TotalUser(string searchEmail)
        {
            List<Registration> users;

            if (!string.IsNullOrEmpty(searchEmail))
            {
                // Search by email
                users = _context.registrations.Where(u => u.Email.Contains(searchEmail)).ToList();
                if (users == null || users.Count == 0)
                {
                    TempData["Message"] = "No users found with the specified email.";
                }
            }
            else
            {
                // Fetch all users
                users = _context.registrations.ToList();
                if (users == null || users.Count == 0)
                {
                    TempData["Message"] = "No users found.";
                }
            }

            return View(users); // Pass users to the view
        }


        public ActionResult EditUser(int id)
        {
            // Find the user by ID
            var user = _context.registrations.Find(id);
            if (user == null)
            {
                TempData["Message"] = "User not found.";
                return RedirectToAction("TotalUser");
            }

            return View(user);
        }


        [HttpPost]
        public ActionResult EditUser(Registration updatedUser)
        {
            if (ModelState.IsValid)
            {
                var user = _context.registrations.Find(updatedUser.UserID);
                if (user != null)
                {
                    user.Email = updatedUser.Email;
                    user.password = updatedUser.password;
                    user.your_Role = updatedUser.your_Role;

                    _context.SaveChanges();
                    TempData["Message"] = "User updated successfully.";
                }
                else
                {
                    TempData["Message"] = "User not found.";
                }
            }

            return RedirectToAction("TotalUser");
        }

        public ActionResult DeleteUser(int id)
        {
            // Find the user by ID
            var user = _context.registrations.Find(id);
            if (user != null)
            {
                _context.registrations.Remove(user);
                _context.SaveChanges();
                TempData["Message"] = "User deleted successfully.";
            }
            else
            {
                TempData["Message"] = "User not found.";
            }

            return RedirectToAction("TotalUser");
        }
        public ActionResult Notifications()
        {
            var notifications = _context.Notifications
                .Where(n => !n.IsRead)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();

            ViewBag.NotificationCount = notifications.Count;
            return View(notifications);
        }


        [HttpPost]
        public ActionResult MarkAsRead(int notificationId)
        {
            var notification = _context.Notifications.Find(notificationId);
            if (notification != null)
            {
                notification.IsRead = true;
                _context.SaveChanges();
            }
            return RedirectToAction("Notifications");
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