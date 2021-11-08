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

        public static string EmailExists = "Email already registered";

        public static string VerificationKeyUsed = "Verification key was already used";
        public static string VerificationKeyNotFound = "Verification key was not found";
    }
}
