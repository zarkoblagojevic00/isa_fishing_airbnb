import init from "../utils/http-utils/axios-util.js";

const serverEndpoint = init("Registration");

// {
//     "email": "user@example.com",
//     "password": "string",
//     "confirmPassword": "string",
//     "name": "string",
//     "surname": "string",
//     "address": "string",
//     "cityId": 0,
//     "phoneNumber": "string"
// }

const registrationService = {
  registerServiceUser: (data) =>
    serverEndpoint.post({ relPath: "RegisterUser", data }),
};

export { registrationService as default };
