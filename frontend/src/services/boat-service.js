import init from "../utils/http-utils/axios-util.js";

const serverEndpoint = init("BoatManagement");

const boatService = {
    getAllBoats: () => serverEndpoint.get({ relPath: "GetAllBoats" }),
    searchBoats: (params) =>
        serverEndpoint.get({ relPath: "SearchBoats", params }),
    getQuickActions: () =>
        serverEndpoint.get({ relPath: "GetQuickActionsForClient" }),
};

export { boatService as default };
