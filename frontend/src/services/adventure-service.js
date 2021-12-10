import init from "../utils/http-utils/axios-util.js";

const serverEndpoint = init("Adventure");

const adventureService = {
    getAllAdventures: () => serverEndpoint.get({ relPath: "GetAllAdventures" }),
};

export { adventureService as default };
