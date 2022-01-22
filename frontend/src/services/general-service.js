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
    sendReview: (mark) =>
        serverEndpoint.post({
            relPath: "CreateNewClientMark",
            data: mark,
        }),
    sendIssue: (issue) =>
        serverEndpoint.post({
            relPath: "CreateNewClientIssue",
            data: issue,
        }),
    unsubscribe: (reservationId) =>
        serverEndpoint.post({
            relPath: "CancelClientSubscription",
            data: { reservationId },
        }),
    getSubscriptions: () =>
        serverEndpoint.get({ relPath: "GetClientSubscriptions" }),
};

export { generalService as default };
