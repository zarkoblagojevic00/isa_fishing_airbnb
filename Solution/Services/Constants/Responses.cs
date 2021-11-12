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

        public static string WrongImageFormat = "Image was not in the correct format";

        public static string DatesOverlap = "Dates overlap";
        public static string PromoActionTaken = "Action is already booked";
    }
}
