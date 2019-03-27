using System.Globalization;
using System.Text.RegularExpressions;
using ExpenseApp.Models.DB;
using MailKit.Net.Smtp;
using MimeKit;

namespace ExpenseApp.Models
{
    public static class Utility
    {
        // Public Methods
        public static void SendEmailToApprover(ExpenseForm form)
        {
            var message = new MimeMessage();
            // TODO: Change this email address
            message.From.Add(new MailboxAddress("Sender Name", "sender email"));
            message.To.Add(new MailboxAddress("", form.Employee.Approver.Email));

            message.Subject = "ExpenseApp - Expense Report waiting Approval";

            message.Body = new TextPart("plain")
            {
                Text = string.Format("Hi {0},\n\n\t{1} has just submitted an Expense Report ({2}). " +
                    "It is waiting for you to review it. You can access this Expense Reports from the " +
                    "Approver Home Page of the Microsoft Teams ExpenseApp Application." +
                    "\n\nMicrosoft Teams ExpenseApp Application" +
                    "\n\nThis is a generated email. Please do not reply to this email.",
                    form.Employee.Approver.FullName, form.Employee.FullName, form.StatementNumber)
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp server", 0, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // TODO: Change this email address and password
                client.Authenticate("sender email", "sender password");

                client.Send(message);
                client.Disconnect(true);
            }
        }

        public static void SendEmailToUser(ExpenseForm form)
        {
            var message = new MimeMessage();
            // TODO: Change this email address
            message.From.Add(new MailboxAddress("sender name", "sender email"));
            message.To.Add(new MailboxAddress("", form.Employee.Email));
            message.Subject = string.Format("ExpenseApp - Expense Report has been {0}.", form.Status.ToString());

            message.Body = new TextPart("plain")
            {
                Text = string.Format("Hi {0},\n\n\tYour ExpenseForm {1} has been {2} by {3}. " +
                    "You can look at it through the Microsoft Teams ExpenseApp Application." +
                    "\n\nMicrosoft Teams ExpenseApp Application" +
                    "\n\nThis is a generated email. Please do not reply to this email.",
                    form.Employee.FullName, form.StatementNumber, form.Status.ToString(),
                    form.Employee.Approver.FullName)
            };

            using (var client = new SmtpClient())
            {
                client.Connect("smtp server", 0, false);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                // TODO: Change this email address and password
                client.Authenticate("sender email", "sender password");

                client.Send(message);
                client.Disconnect(true);
            }
        }

        /// <summary>
        /// Converts a string into a slug (clean, human-readable url)
        /// </summary>
        /// <param name="text">The string to convert to a slug.</param>
        /// <returns>The slug created from <paramref name="text"/>.</returns>
        public static string Slugify(string text)
        {
            if (null == text)
                return null;

            string slug = text.ToLower();

            // Replace slashes with dashes
            slug = slug.Replace('/', '-');

            //  Remove invalid characters
            slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");

            // Convert multiple spaces into one space
            slug = Regex.Replace(slug, @"\s+", " ").Trim();

            // Cut and trim
            slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();

            // Replace spaces as strings
            slug = Regex.Replace(slug, @"\s", "-");

            return slug;
        }

        public static string ToTitleCase(string text)
        {
            if (null == text)
                return null;

            TextInfo textInfo = new CultureInfo("en-US", true).TextInfo;
            return textInfo.ToTitleCase(text.Trim());
        }
    }
}