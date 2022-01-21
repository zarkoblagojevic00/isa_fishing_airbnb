import init from "../utils/http-utils/axios-util.js";

const serverEndpoint = init("Adventure");

const adventureService = {
    getAllAdventures: () => serverEndpoint.get({ relPath: "GetAllAdventures" }),
    searchAdventures: (params) =>
        serverEndpoint.get({ relPath: "SearchAdventures", params }),
    getQuickActions: () =>
        serverEndpoint.get({ relPath: "GetQuickActionsForClient" }),
};

export { adventureService as default };
