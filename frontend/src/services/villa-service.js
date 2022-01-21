import init from "../utils/http-utils/axios-util.js";

const serverEndpoint = init("VillaManagement");

const villaService = {
    getAllVillas: () => serverEndpoint.get({ relPath: "GetAllVillas" }),
    searchVillas: (params) =>
        serverEndpoint.get({ relPath: "SearchVillas", params }),
    getQuickActions: () =>
        serverEndpoint.get({ relPath: "GetQuickActionsForClient" }),
};

export { villaService as default };
