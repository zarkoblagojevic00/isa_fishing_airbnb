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
        const realClaims = saveClaimsToLocalStorage(claims);
        document.cookie = `userId=${claims.userId}`;
        document.cookie = `email=${mail}`;
        return realClaims;
    },
    logout: () => {
        clearStorage();
        document.cookie = "userId=; expires = Thu, 01 Jan 1970 00:00:00 GMT";
        document.cookie = "email=; expires = Thu, 01 Jan 1970 00:00:00 GMT";
    },
};

export { sessionService as default };
