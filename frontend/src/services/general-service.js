import init from "../utils/http-utils/axios-util.js";

const serverEndpoint = init("GeneralService");

const reservationService = {
    makeReservation: (reservation) =>
        serverEndpoint.post({
            relPath: "CreateClientReservation",
            data: reservation,
        }),
};

export { reservationService as default };
