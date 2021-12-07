import init from "../utils/http-utils/axios-util.js";
import {
    saveClaimsToLocalStorage,
    clearStorage,
} from "../utils/local-storage-util.js";

const serverEndpoint = init("Login");

const sessionService = {
    login: async (email, password) => {
        const responseSubset = await serverEndpoint.post({
            relPath: "LoginUser",
            data: { email, password },
        });
        const { email: mail, ...claims } = responseSubset;
        saveClaimsToLocalStorage(claims);
        document.cookie = `userId=${claims.userId}; domain=localhost`;
        document.cookie = `email=${mail}; domain=localhost`;
    },
    logout: () => {
        clearStorage();
        document.cookie = `userId=; expires = Thu, 01 Jan 1970 00:00:00 GMT`;
        document.cookie = `email=; expires = Thu, 01 Jan 1970 00:00:00 GMT`;
    },
};

export { sessionService as default };
