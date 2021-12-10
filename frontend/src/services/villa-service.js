import init from "../utils/http-utils/axios-util.js";

const serverEndpoint = init("VillaManagement");

const villaService = {
    getAllVillas: () => serverEndpoint.get({ relPath: "GetAllVillas" }),
};

export { villaService as default };
