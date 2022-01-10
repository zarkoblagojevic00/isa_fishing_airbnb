<template>
    <div class="wrapperdiv">
        <div class="villa-picker">
            <h3>Pick a villa:</h3>
            <select class="select-villa" v-model="selectedService">
                <option disabled value="">Please select one</option>
                <option
                    v-for="service in allServices"
                    :key="service.id"
                    :value="service.id"
                >
                    {{ service.name }}
                </option>
            </select>
        </div>
        <div class="calendar-parent">
            <calendar-view
                :items="items"
                :show-date="showDate"
                :time-format-options="{ hour: 'numeric', minute: '2-digit' }"
                :enable-drag-drop="true"
                :disable-past="disablePast"
                :disable-future="disableFuture"
                :show-times="showTimes"
                :date-classes="myDateClasses"
                :display-period-uom="displayPeriodUom"
                :display-period-count="displayPeriodCount"
                :starting-day-of-week="startingDayOfWeek"
                :class="themeClasses"
                :period-changed-callback="periodChanged"
                :current-period-label="useTodayIcons ? 'icons' : ''"
                :displayWeekNumbers="displayWeekNumbers"
                :enable-date-selection="false"
                @click-item="GetUserForReservation"
            >
                <template #header="{ headerProps }">
                    <calendar-view-header
                        :header-props="headerProps"
                        @input="setShowDate"
                    />
                </template>
            </calendar-view>
        </div>
        <div class="user-info" v-if="displayUserInfo == true">
            <div class="info-header">
                User information for selected reservation lasting from
                <span class="emphasise">
                    {{
                        moment(
                            selectedReservation.reservation.startDateTime
                        ).format("ddd MMM DD, YYYY")
                    }}
                </span>
                to
                <span class="emphasise">
                    {{
                        moment(
                            selectedReservation.reservation.endDateTime
                        ).format("ddd MMM DD, YYYY")
                    }}
                </span>
            </div>
            <div class="info-wrapper">
                <span>Name: </span>
                <span class="emphasise">{{ user.name }}</span>
            </div>
            <div class="info-wrapper">
                <span>Surname: </span>
                <span class="emphasise">{{ user.surname }}</span>
            </div>
            <div class="info-wrapper">
                <span>Phone: </span>
                <span class="emphasise">{{ user.phone }}</span>
            </div>
            <div class="info-wrapper">
                <span>Email: </span>
                <span class="emphasise">{{ user.email }}</span>
            </div>
            <div class="info-wrapper">
                <span>Address: </span>
                <span class="emphasise">{{ user.address }}</span>
            </div>
            <div class="info-wrapper">
                <span>Country: </span>
                <span class="emphasise">{{ user.country }}</span>
            </div>
            <div class="info-wrapper">
                <span>City: </span>
                <span class="emphasise">{{ user.city }}</span>
            </div>
            <div class="info-wrapper" v-if="selectedReservation.report != null">
                <span>Report: </span>
                <span class="emphasise max-width">{{
                    selectedReservation.report.reportText
                }}</span>
            </div>
            <div
                class="report-wrapper"
                v-if="
                    selectedReservation.report == null &&
                    Date.parse(selectedReservation.reservation.endDateTime) <
                        Date.parse(new Date().toISOString())
                "
            >
                <div>Write a report!</div>
                <textarea
                    class="input-textarea"
                    v-model="reportText"
                ></textarea>

                <div class="horizontal-wrapper">
                    <input
                        type="checkbox"
                        class="input-checkbox"
                        v-model="shownUp"
                    />
                    <span class="label font">User show up?</span>
                </div>

                <div class="horizontal-wrapper">
                    <input
                        type="checkbox"
                        class="input-checkbox"
                        v-model="suggestPenalty"
                    />
                    <span class="label font">Suggest penalty?</span>
                </div>
                <button class="report-submit" @click="Submit">
                    Submit report
                </button>
            </div>
        </div>
    </div>
</template>
<script>
import "../../node_modules/vue-simple-calendar/dist/style.css";
import "../../node_modules/vue-simple-calendar/static/css/default.css";
import "../../node_modules/vue-simple-calendar/static/css/holidays-us.css";
import {
    CalendarView,
    CalendarViewHeader,
    CalendarMath,
} from "vue-simple-calendar";
import moment from "moment";
export default {
    name: "ServiceCalendar",
    components: {
        CalendarView,
        CalendarViewHeader,
    },
    props: {
        serviceMode: String,
    },
    data() {
        return {
            showDate: this.thisMonth(1),
            message: "",
            startingDayOfWeek: 0,
            disablePast: false,
            disableFuture: false,
            displayPeriodUom: "month",
            displayPeriodCount: 1,
            displayWeekNumbers: false,
            showTimes: true,
            newItemTitle: "",
            newItemStartDate: "",
            newItemEndDate: "",
            useDefaultTheme: true,
            useHolidayTheme: true,
            useTodayIcons: false,

            selectedService: "",
            allServices: [],
            items: [],

            user: {},
            selectedReservation: {},
            displayUserInfo: false,
            receivedHistory: [],
            moment: moment,

            reportText: "",
            isVilla: this.$props.serviceMode == "villa",
            shownUp: false,
            suggestPenalty: false,
        };
    },
    computed: {
        userLocale() {
            return CalendarMath.getDefaultBrowserLocale;
        },
        dayNames() {
            return CalendarMath.getFormattedWeekdayNames(
                this.userLocale,
                "long",
                0
            );
        },
        themeClasses() {
            return {
                "theme-default": this.useDefaultTheme,
                "holiday-us-traditional": this.useHolidayTheme,
                "holiday-us-official": this.useHolidayTheme,
            };
        },
        myDateClasses() {
            const o = {};
            const theFirst = this.thisMonth(1);
            const ides = [2, 4, 6, 9].includes(theFirst.getMonth()) ? 15 : 13;
            const idesDate = this.thisMonth(ides);
            o[CalendarMath.isoYearMonthDay(idesDate)] = "ides";
            o[CalendarMath.isoYearMonthDay(this.thisMonth(21))] = [
                "do-you-remember",
                "the-21st",
            ];
            return o;
        },
    },
    mounted() {
        this.newItemStartDate = CalendarMath.isoYearMonthDay(
            CalendarMath.today()
        );
        this.newItemEndDate = CalendarMath.isoYearMonthDay(
            CalendarMath.today()
        );
        this.getallServices();
    },
    methods: {
        thisMonth(d, h, m) {
            const t = new Date();
            return new Date(t.getFullYear(), t.getMonth(), d, h || 0, m || 0);
        },
        setShowDate(d) {
            this.message = `Changing calendar view to ${d.toLocaleDateString()}`;
            this.showDate = d;
        },
        getallServices() {
            let vue = this;
            let url = this.isVilla
                ? "/api/VillaManagement/GetOwnedVillas"
                : "/api/BoatManagement/GetOwnedBoats";
            fetch(url, {
                method: "GET",
                header: {
                    "Content-type": "application-json",
                    "Set-Cookie": document.cookie,
                },
            })
                .then((response) => {
                    if (response.status == 200) {
                        return response.json();
                    } else {
                        return response.text();
                    }
                })
                .then((data) => {
                    var stringConstructor = "test".constructor;
                    if (data.constructor == stringConstructor) {
                        alert("Something went wrong!\nError message: " + data);
                        return;
                    }
                    vue.allServices = new Array();
                    for (let service of data) {
                        vue.allServices.push({
                            id: vue.isVilla ? service.villaId : service.boatId,
                            name: service.name,
                        });
                    }
                });
        },
        RefreshCalendar() {
            let vue = this;
            fetch(
                "/api/GeneralService/GetReservationHistory?serviceId=" +
                    this.selectedService,
                {
                    method: "GET",
                    header: {
                        "Content-type": "application-json",
                        "Set-Cookie": document.cookie,
                    },
                }
            )
                .then((response) => {
                    if (response.status == 200) {
                        return response.json();
                    }
                    return response.text();
                })
                .then((data) => {
                    let strconst = "test".constructor;
                    if (data.constructor == strconst) {
                        alert("Something went wrong!\nError message: " + data);
                        return;
                    }

                    vue.items = new Array();
                    vue.receivedHistory = data;
                    for (let i = 0; i < data.length; i++) {
                        let reservation = data[i].reservation;
                        if (!reservation.isCanceled) {
                            let item = {
                                id: i,
                                startDate:
                                    reservation.startDateTime.split("T")[0] +
                                    " " +
                                    reservation.startDateTime
                                        .split("T")[1]
                                        .split(".")[0],
                                endDate:
                                    reservation.endDateTime.split("T")[0] +
                                    " " +
                                    reservation.startDateTime
                                        .split("T")[1]
                                        .split(".")[0],
                            };

                            if (reservation.isServiceUnavailableMarker) {
                                item.title = "Service unavailable";
                                item.style =
                                    "background-color: #D8DC6A; cursor: pointer";
                            } else if (reservation.isPromo) {
                                item.title = "Promo";
                                item.style =
                                    "background-color: #1985A1; cursor: pointer";
                            } else {
                                item.title = "Reservation";
                                item.style =
                                    "background-color: #EB8258; cursor: pointer";
                            }

                            vue.items.push(item);
                        }
                    }
                });
        },
        async GetUserForReservation(evt) {
            if (evt.title == "Service unavailable") {
                return;
            }
            this.selectedReservation = this.receivedHistory[evt.id];

            let vue = this;
            fetch(
                "/api/GeneralService/GetUserInfoFromReservation?reservationId=" +
                    vue.receivedHistory[evt.id].reservation.reservationId,
                {
                    method: "GET",
                    header: {
                        "Content-type": "application/json",
                        "Set-Cookie": document.cookie,
                    },
                }
            )
                .then((response) => {
                    return response.text();
                })
                .then((data) => {
                    let parsed = "";
                    try {
                        parsed = JSON.parse(data);
                    } catch {
                        parsed = data;
                    }

                    let strconstructor = "test".constructor;
                    if (strconstructor == parsed.constructor) {
                        alert(
                            "Something went wrong!\nError message: " + parsed
                        );
                        return;
                    }
                    vue.user = parsed;
                    vue.displayUserInfo = true;
                });
        },
        Submit() {
            let vue = this;
            let dto = {
                reservationId:
                    this.selectedReservation.reservation.reservationId,
                reportText: this.reportText,
                suggestPenalty: this.suggestPenalty,
                shownUp: this.shownUp,
            };

            if (dto.reservationId == undefined) {
                alert("You need to select the reservation first!");
                return;
            }
            if (dto.reportText == undefined || dto.reportText.length == 0) {
                alert("Report text cannot be empty!");
                return;
            }

            fetch("/api/GeneralService/SubmitReport", {
                method: "POST",
                redirect: "follow",
                headers: {
                    "Content-type": "application/json",
                    "Set-Cookie": document.cookie,
                },
                body: JSON.stringify(dto),
            })
                .then((response) => {
                    if (response.ok) {
                        return "";
                    } else {
                        return response.text();
                    }
                })
                .then((data) => {
                    if (data == "") {
                        vue.selectedReservation.report = {
                            reportText: vue.reportText,
                        };
                        for (let history of vue.receivedHistory) {
                            if (
                                history.reservation.reservation !=
                                vue.selectedReservation.reservation
                                    .reservationId
                            ) {
                                continue;
                            }
                            history.report = {
                                reportText: vue.reportText,
                            };
                            break;
                        }

                        vue.reportText = "";
                        return;
                    }
                    let error = "";
                    let strconst = "".constructor;
                    try {
                        error = JSON.parse(data);
                    } catch {
                        error = data;
                    }
                    if (error.constructor == strconst) {
                        alert("Something went wrong!\nError message: " + error);
                    } else {
                        let errors = "";
                        let values = Object.keys(error).map(function (key) {
                            return error[key];
                        });
                        for (let value of values) {
                            for (let error of value) {
                                errors = errors + error + "\n";
                            }
                        }
                    }
                });
        },
    },
    watch: {
        selectedService: function () {
            this.RefreshCalendar();
        },
    },
};
</script>

<style>
.wrapperdiv {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: auto;
    margin-left: auto;
    margin-right: auto;
}
.calendar-parent {
    display: flex;
    min-height: 450px;
    flex-direction: column;
    flex-grow: 1;
    overflow-x: hidden;
    overflow-y: hidden;
    background-color: white;
}
.cv-wrapper.period-month.periodCount-2 .cv-week,
.cv-wrapper.period-month.periodCount-3 .cv-week,
.cv-wrapper.period-year .cv-week {
    min-height: 6rem;
}
.theme-default .cv-item.birthday {
    background-color: #e0f0e0;
    border-color: #d7e7d7;
}
.theme-default .cv-item.birthday::before {
    content: "\1F382";
    margin-right: 0.5em;
}
.theme-default .cv-day.ides {
    background-color: #ffe0e0;
}
.ides .cv-day-number::before {
    content: "\271D";
}
.cv-day.do-you-remember.the-21st .cv-day-number::after {
    content: "\1F30D\1F32C\1F525";
}

.villa-picker {
    height: 70px;
    display: flex;
    flex-flow: row wrap;
    align-items: center;
}

.select-villa {
    width: 200px;
    height: 40px;
    margin-left: 10px;
    outline: none;
    border: none;
}

.user-info {
    position: relative;
    width: auto;
    margin-top: 25px;
}
.info-header {
    width: auto;
    display: flex;
    font-size: 18px;
    font-weight: 100;
}
.info-wrapper {
    width: auto;
    display: flex;
    margin-top: 10px;
    font-size: 18px;
    font-weight: 100;
    margin-left: 25px;
}
.emphasise {
    font-style: italic;
    margin-left: 5px;
    margin-right: 5px;
    font-weight: 900;
}

.report-wrapper {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    margin-top: 10px;
    font-size: 18px;
    font-weight: 100;
    margin-left: 20px;
    width: fit-content;
}

.input-textarea {
    height: 450px;
    max-width: 600px;
    min-width: 500px;
    width: 100%;
    border: 1px solid #c3c3c3;
    border-radius: 15px;
    padding: 15px;
    box-sizing: border-box;
    resize: none;
    font-size: 15px;
    outline: none;
    margin-top: 5px;
    margin-bottom: 5px;
}

.input-wrapper {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    margin-bottom: 15px;
}

.label {
    font-size: 14px;
    color: black;
    margin-bottom: 5px;
    padding-left: 10px;
}

.input-checkbox {
    width: 15px;
    height: 15px;
    position: relative;
}

.report-submit {
    height: 50px;
    border-radius: 15px;
    border: none;
    font-size: 20px;
    color: white;
    background-color: #345fed;
    cursor: pointer;
    font-size: 18px;
    transition: background-color 300ms linear;
    align-self: center;
}

.report-submit:hover {
    background-color: #54cc39;
    color: black;
}

.max-width {
    max-width: 500px;
    text-align: start;
}

.horizontal-wrapper {
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    display: flex;
    margin-bottom: 10px;
    padding-left: 10px;
}

.font {
    font-size: 18px !important;
    font-weight: 100 !important;
}
</style>
