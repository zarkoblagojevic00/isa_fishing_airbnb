import init from "../utils/http-utils/axios-util.js";

const serverEndpoint = init("GeneralService");

const generalService = {
    makeReservation: (reservation) =>
        serverEndpoint.post({
            relPath: "CreateClientReservation",
            data: reservation,
        }),
    getBookedClientReservations: (userId) =>
        serverEndpoint.get({
            relPath: "GetBookedClientReservation",
            params: { userId },
        }),
    getHistoryClientReservations: (userId) =>
        serverEndpoint.get({
            relPath: "GetHistoryClientReservation",
            params: { userId },
        }),
    CancelClientReservation: (reservationId) =>
        serverEndpoint.post({
            relPath: "CancelClientReservation",
            data: { reservationId },
        }),
};

export { generalService as default };
