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
                @click-item="ClickItem"
            >
                <template #header="{ headerProps }">
                    <calendar-view-header
                        :header-props="headerProps"
                        @input="setShowDate"
                    />
                </template>
            </calendar-view>
        </div>
        <div class="form-wrapper" v-if="currentMode == 'AddPromoAction'">
            <div class="action-form">
                <div class="input-wrapper">
                    <span class="label">Date range:</span>
                    <Datepicker
                        class="date-range"
                        :modelValue="selectedDate"
                        @update:modelValue="UpdateDate"
                        :range="true"
                        :twoCalendars="true"
                        :placeholder="'Select a date range'"
                        :minDate="new Date()"
                    />
                </div>
                <div class="input-wrapper">
                    <span class="label">Price per day:</span>
                    <input
                        class="input-field"
                        type="text"
                        v-model="pricePerDay"
                        v-on:keypress="isNumber($event)"
                        :class="[ValidatePrice() ? '' : 'error-outline']"
                    />
                </div>
                <div class="input-wrapper">
                    <span class="label">Capacity:</span>
                    <input
                        class="input-field"
                        type="number"
                        min="0"
                        v-model="capacity"
                        :class="[
                            ValidateNumber(capacity) ? '' : 'error-outline',
                        ]"
                    />
                </div>
                <button
                    v-if="mode == 'Editing'"
                    class="submit-btn"
                    @click="SwitchToAddingMode"
                >
                    Return to adding mode
                </button>
            </div>
            <div class="action-form">
                <div class="input-wrapper">
                    <span class="label">Additional benefits:</span>
                    <textarea
                        class="input-textarea"
                        type="text"
                        v-model="additionalBenefits"
                        placeholder="Not required"
                    ></textarea>
                </div>
                <div class="input-wrapper" v-if="errors.length != 0">
                    <span
                        class="error-text"
                        v-for="error in errors"
                        :key="error"
                        >*{{ error }}</span
                    >
                </div>
            </div>
        </div>
        <div class="submit-div" v-if="currentMode == 'AddPromoAction'">
            <button class="submit-btn" @click="Submit()">
                {{
                    mode == "Adding"
                        ? "Create promo action!"
                        : "Update promo action!"
                }}
            </button>
        </div>
        <div class="form-wrapper" v-if="currentMode == 'ReserveForUser'">
            <div class="action-form">
                <div class="input-wrapper">
                    <span class="label">Date range:</span>
                    <Datepicker
                        class="date-range width-fix"
                        :modelValue="selectedDate"
                        @update:modelValue="UpdateDate"
                        :range="true"
                        :twoCalendars="true"
                        :placeholder="'Select a date range'"
                        :minDate="new Date()"
                    />
                </div>
                <div class="input-wrapper">
                    <span class="label">Price per day:</span>
                    <input
                        class="input-field"
                        type="text"
                        v-model="pricePerDayReservation"
                        v-on:keypress="isNumber($event)"
                        :class="[ValidatePrice() ? '' : 'error-outline']"
                    />
                </div>
                <div class="input-wrapper">
                    <span class="label">Email of user:</span>
                    <input
                        class="input-field"
                        type="email"
                        min="0"
                        v-model="emailOfUser"
                        :class="[
                            ValidateMail(emailOfUser) ? '' : 'error-outline',
                        ]"
                    />
                </div>
                <button
                    v-if="mode == 'Editing'"
                    class="submit-btn"
                    @click="SwitchToAddingMode"
                >
                    Return to adding mode
                </button>
            </div>
            <div class="action-form">
                <div class="input-wrapper">
                    <span class="label">Additional equipment:</span>
                    <textarea
                        class="input-textarea"
                        type="text"
                        v-model="additionalEquipment"
                        placeholder="Not required"
                    ></textarea>
                </div>
                <div class="input-wrapper" v-if="errors.length != 0">
                    <span
                        class="error-text"
                        v-for="error in errors"
                        :key="error"
                        >*{{ error }}</span
                    >
                </div>
            </div>
        </div>
        <div class="submit-div" v-if="currentMode == 'ReserveForUser'">
            <button class="submit-btn" @click="CreateReservation">
                Create reservation!
            </button>
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

import Datepicker from "vue3-date-time-picker";
import "vue3-date-time-picker/dist/main.css";

export default {
    name: "AddPromoAction",
    components: {
        CalendarView,
        CalendarViewHeader,
        Datepicker,
    },
    props: {
        currentMode: String,
        promoMode: String,
    },
    data() {
        return {
            showDate: this.thisMonth(1),
            message: "",
            startingDayOfWeek: 0,
            disablePast: true,
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

            receivedItems: [],

            mode: "Adding",
            pricePerDay: 0,
            capacity: 0,
            additionalBenefits: "",
            selectedDate: ["", ""],
            selectedAction: {},
            errors: [],

            selectedDateReservation: ["", ""],
            emailOfUser: "",
            pricePerDayReservation: "",
            additionalEquipment: "",

            isVilla: this.$props.promoMode == "villa",
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
        this.GetAllServices();
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
        GetAllServices() {
            let vue = this;
            let url =
                this.$props.promoMode == "villa"
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
        async RefreshCalendar() {
            let vue = this;
            this.items = new Array();
            await fetch(
                "/api/QuickAction/GetQuickActions?serviceId=" +
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

                    vue.receivedItems = new Array();
                    for (let i = 0; i < data.length; i++) {
                        vue.receivedItems.push(data[i]);
                        let promo = data[i];

                        let style = "cursor: pointer; background-color: ";
                        if (promo.isTaken) {
                            style += "#F22B29";
                        } else {
                            style += "#64F58D";
                        }

                        let item = {
                            id: i,
                            startDate: promo.startDateTime,
                            endDate: promo.endDateTime,
                            title: promo.isTaken ? "Taken" : "Available",
                            style: style,
                        };
                        vue.items.push(item);
                    }
                });

            fetch(
                "/api/GeneralService/GetNonPromoReservations?serviceId=" +
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
                    let beginingNum = vue.items.length;
                    for (let i = 0; i < data.length; i++) {
                        let reservation = data[i];

                        let style = "cursor: pointer; background-color: ";
                        if (reservation.isServiceUnavailableMarker) {
                            style += "#456990";
                        } else {
                            style += "#EEB868";
                        }

                        let item = {
                            id: beginingNum + i,
                            startDate: reservation.startDateTime,
                            endDate: reservation.endDateTime,
                            title: reservation.isServiceUnavailableMarker
                                ? "Unavailable"
                                : "Reservation",
                            style: style,
                        };
                        vue.items.push(item);
                    }
                });
        },
        ValidateString(str) {
            if (str == 0) return false;
            return true;
        },
        ValidatePrice() {
            let input = "";
            if (this.currentMode == "AddPromoAction") {
                input = this.pricePerDay;
            } else {
                input = this.pricePerDayReservation;
            }
            if (input.length == 0) return false;

            input = parseFloat(input);
            if (input == undefined || input == null) return false;

            if (input == 0) return false;

            return true;
        },
        isNumber(evt) {
            evt = evt ? evt : window.event;
            var charCode = evt.which ? evt.which : evt.keyCode;
            if (
                charCode > 31 &&
                (charCode < 48 || charCode > 57) &&
                charCode !== 46
            ) {
                evt.preventDefault();
            } else {
                return true;
            }
        },
        ValidateNumber(num) {
            if (num < 1 || !Number.isInteger(num)) return false;

            return true;
        },
        ClickItem(evt) {
            let relevantAction = this.receivedItems[evt.id];
            if (relevantAction == null || relevantAction.isTaken) {
                return;
            }

            this.mode = "Editing";

            this.pricePerDay = relevantAction.pricePerDay;
            this.capacity = relevantAction.capacity;
            this.additionalBenefits = relevantAction.addedBenefits;

            this.selectedDate = [
                relevantAction.startDateTime,
                relevantAction.endDateTime,
            ];
            this.selectedAction = relevantAction;
        },
        SwitchToAddingMode() {
            this.mode = "Adding";

            (this.pricePerDay = 0),
                (this.capacity = 0),
                (this.additionalBenefits = "");
            this.selectedDate = ["", ""];
        },
        UpdateDate(evt) {
            if (this.currentMode == "AddPromoAction") {
                this.selectedDate = [evt[0], evt[1]];
            } else {
                this.selectedDateReservation = [evt[0], evt[1]];
            }
        },
        Submit() {
            this.errors = new Array();
            if (
                this.selectedDate.length != 2 ||
                this.selectedDate[0] == "" ||
                this.selectedDate[1] == ""
            ) {
                this.errors.push("You need to specify the date range");
            }
            if (this.selectedService == "") {
                this.errors.push("You need to select a villa first");
            }
            if (!this.ValidatePrice()) {
                this.errors.push("Price isn't valid");
            }
            if (!this.ValidateNumber(this.capacity)) {
                this.errors.push("Capacity isn't valid");
            }

            if (this.errors.length > 0) {
                return;
            }

            let dto = {
                serviceId: this.selectedService,
                startDateTime: this.selectedDate[0],
                endDateTime: this.selectedDate[1],
                pricePerDay: this.pricePerDay,
                addedBenefits: this.additionalBenefits,
                capacity: this.capacity,
            };

            let url = "/api/QuickAction/";
            if (this.mode == "Adding") {
                url += "CreateNewQuickAction";
            } else {
                url += "UpdateQuickAction";
                dto.promoActionId = this.selectedAction.promoActionId;
            }

            let vue = this;

            fetch(url, {
                method: vue.mode == "Adding" ? "POST" : "PUT",
                headers: {
                    "Content-type": "application/json",
                    "Set-Cookie": document.cookie,
                },
                body: JSON.stringify(dto),
            })
                .then((response) => {
                    if (response.status == 200) {
                        vue.RefreshCalendar();
                        vue.SwitchToAddingMode();
                        return "";
                    } else {
                        return response.text();
                    }
                })
                .then((data) => {
                    if (data == "") {
                        return;
                    }
                    let error = data;
                    let parsed = "";
                    try {
                        parsed = JSON.parse(error);
                    } catch (err) {
                        parsed = error;
                    }
                    let strconst = "test".constructor;
                    if (parsed.constructor == strconst) {
                        vue.errors.push(parsed);
                    } else {
                        console.log(parsed);
                        parsed = parsed.errors;
                        let values = Object.keys(parsed).map(function (key) {
                            return parsed[key];
                        });
                        for (let value of values) {
                            for (let error of value) {
                                vue.errors.push(error);
                            }
                        }
                    }
                });
        },
        ValidateMail() {
            if (!this.ValidateString(this.emailOfUser)) return false;

            let format =
                /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (!this.emailOfUser.match(format)) {
                return false;
            }
            return true;
        },
        async CreateReservation() {
            this.errors = new Array();
            if (
                this.selectedService == undefined ||
                this.selectedService == ""
            ) {
                this.errors.push("A villa needs to be chosen first!");
            }
            if (!this.ValidateMail()) {
                this.errors.push("Mail isn't in correct format!");
            }
            if (!this.ValidatePrice()) {
                this.errors.push("Price isn't in correct format!");
            }
            if (
                this.selectedDateReservation[0] == "" ||
                this.selectedDateReservation[1] == ""
            ) {
                this.errors.push("Date needs to be specified!");
            }
            if (this.errors.length > 0) {
                return;
            }

            let dto = {
                userMail: this.emailOfUser,
                serviceId: this.selectedService,
                price: this.pricePerDayReservation,
                additionalEquipment: this.additionalEquipment,
                startDateTime: this.selectedDateReservation[0],
                endDateTime: this.selectedDateReservation[1],
            };

            let response = await fetch(
                "/api/GeneralService/CreateReservationForUser",
                {
                    method: "POST",
                    redirect: "follow",
                    headers: {
                        "Content-type": "application/json",
                        "Set-Cookie": document.cookie,
                    },
                    body: JSON.stringify(dto),
                }
            );

            if (response.ok) {
                this.RefreshCalendar();
                this.selectedDateReservation = ["", ""];
                this.emailOfUser = "";
                this.additionalEquipment = "";
                this.pricePerDayReservation = "";
                return;
            }

            let message = await response.text();
            let error = "";
            try {
                error = JSON.parse(message);
            } catch {
                error = message;
            }

            let strconst = "".constructor;
            if (strconst == error.constructor) {
                this.errors.push(error);
                alert("Something went wrong!\nError message: " + error);
            }
        },
    },
    watch: {
        selectedService: function () {
            this.RefreshCalendar();
        },
    },
};
</script>

<style scoped>
.wrapperdiv {
    display: flex;
    flex-direction: column;
    width: 100%;
    height: 100%;
    margin-left: auto;
    margin-right: auto;
}
.calendar-parent {
    display: flex;
    min-height: 450px;
    max-height: 450px;
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

.error-outline {
    border: 1px solid red !important;
}

.select-villa {
    width: 200px;
    height: 40px;
    margin-left: 10px;
    outline: none;
    border: none;
}

.action-form {
    height: auto;
    width: 100%;
    margin-top: 25px;
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

.input-field {
    height: 50px;
    border-radius: 15px;
    outline: none;
    border: 1px solid #c3c3c3;
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    padding-left: 15px;
    padding-right: 15px;
    box-sizing: border-box;
    font-size: 15px;
}

.input-textarea {
    height: 150px;
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    border: 1px solid #c3c3c3;
    border-radius: 15px;
    padding: 15px;
    box-sizing: border-box;
    resize: none;
    font-size: 15px;
    outline: none;
}

.date-range {
    max-width: 400px;
    min-width: 200px;
    width: 100%;
}

.form-wrapper {
    display: flex;
    flex-direction: row;
    height: 100%;
}

.error-text {
    color: red;
    margin-left: 15px;
    margin-top: 5px;
    font-size: 14px;
}

.submit-div {
    display: flex;
    justify-content: center;
}

.submit-btn {
    height: 50px;
    border-radius: 15px;
    border: none;
    font-size: 20px;
    color: white;
    background-color: #345fed;
    cursor: pointer;
    font-size: 18px;
    transition: background-color 300ms linear;
}

.submit-btn:hover {
    background-color: #54cc39;
}

.width-fix {
    width: 250px;
}
</style>
