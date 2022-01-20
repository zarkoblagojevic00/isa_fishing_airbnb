export default {
    data() {
        return {
            fromDate: new Date(Date.parse(localStorage.getItem("fromDate"))),
            toDate: new Date(Date.parse(localStorage.getItem("toDate"))),
        };
    },
};
