using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Constants
{
    public static class Responses
    {
        public static string Ok = "Ok";
        public static string Success = "Success";
        public static string Error = "Error";
        public static string NotFound = "Resource not found";

        public static string EmailExists = "Email already registered";

        public static string VerificationKeyUsed = "Verification key was already used";
        public static string VerificationKeyNotFound = "Verification key was not found";

        public static string MissingCookieInfo = "Missing information in cookie";

        public static string ServiceOwnerNotLinked = "Service and Owner specified are not linked";
        public static string ServiceNotAvailableAtGivenTime = "Service is not available at that time";

        public static string WrongImageFormat = "Image was not in the correct format";

        public static string DatesOverlap = "Dates overlap";
        public static string PromoActionTaken = "Action is already booked";

        public static string CannotChangeService = "Service is booked and cannot be changed";

        public static string OngoingReservation = "Reservation is still not over";

        public static string ReportAlreadySubmitted = "Report already submitted";

        public static string AccountDeletionRequestSubmitted = "Account deletion request was already submitted for this user";

        public static string BadCredentials = "User with given email and password doesn't exist";

        public static string UserAccountNotVerified = "Your account is not yet verified. Please verify your account via email.";

        public static string CannotRequestDeletion = "You cannot request account deletion because you have reservations in the future.";

        public static string UserCannotBeDeleted = "User cannot be deleted.";

        public static string UnavailableRightNow = "Service you are trying to reach is unavailable right now.";

        public static string MissingResponsibility = "Missing boat responsibility";

        public static string InstructorUnavailable = "Instructor unavailable at given time.";

        public static string UserReservationNotLinked = "User can only create reservation for himself.";

        public static string PromoAlreadyTaken = "PromoActionIsAlreadyTaken.";
    }
}
