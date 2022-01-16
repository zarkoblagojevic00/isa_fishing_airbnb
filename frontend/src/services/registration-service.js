import init from "../utils/http-utils/axios-util.js";

const serverEndpoint = init("Registration");

const registrationService = {
    registerServiceUser: (data) =>
        serverEndpoint.post({ relPath: "RegisterUser", data }),
};

export { registrationService as default };
