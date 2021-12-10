<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container">
        <h1>Availability overview</h1>
        <button @click="$router.push('/instructor/availability/edit')">
            Edit availability
        </button>
    </div>
    <!-- use custom calendar with template  -->
    <div class="flexbox-container">
        <v-calendar
            class="custom-calendar max-w-full"
            :masks="masks"
            :attributes="attributes"
            disable-page-swipe
            is-expanded
        >
            <template v-slot:day-content="{ day, attributes }">
                <div class="slot">
                    <span>
                        <b>{{ day.day }}</b>
                    </span>
                    <div>
                        <div
                            v-for="attr in attributes"
                            :key="attr.key"
                            :class="attr.customData.class"
                        >
                            {{ attr.customData.title }}
                        </div>
                    </div>
                </div>
            </template>
        </v-calendar>
    </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import axios from "../api/api.js";
import moment from "moment";

export default {
    name: "InstructorServices",
    components: {
        Navbar,
    },
    mounted() {
        this.loadAvailabilityPeriod();
    },
    data() {
        const month = new Date().getMonth();
        const year = new Date().getFullYear();
        return {
            masks: {
                weekdays: "WWW",
            },
            attributes: [
                {
                    key: 1,
                    customData: {
                        title: "12:00 - 17:45",
                        status: "booked",
                        class: "booked-slot",
                    },
                    dates: new Date(year, month, 1),
                },
                {
                    key: 2,
                    customData: {
                        title: "11:00 - 19:00",
                        status: "booked",
                        class: "booked-slot",
                    },
                    dates: new Date(year, month, 2),
                },
                {
                    key: 3,
                    customData: {
                        title: "14:00 - 17:45",
                        status: "free",
                        class: "free-slot",
                    },
                    dates: new Date(year, month, 5),
                },
                {
                    key: 4,
                    customData: {
                        title: "15:00 - 22:30",
                        status: "free",
                        class: "free-slot",
                    },
                    dates: new Date(year, month, 9),
                },
                {
                    key: 5,
                    customData: {
                        title: "08:00 - 14:30",
                        status: "free",
                        class: "free-slot",
                    },
                    dates: new Date(year, month, 11),
                },
            ],
            navbarItems: [
                "Services",
                "Reservations",
                "Availability",
                "Analytics",
                "My profile",
            ],
            baseUrlInstructor: "/instructor/",
            range: {},
        };
    },
    methods: {
        loadAvailabilityPeriod() {
            axios
                .get("/api/Instructor/GetAdditionalInstructorInfo")
                .then(({ data }) => {
                    this.range = data;
                    const datesWithinRange = this.getDatesWithinRange(
                        data.startDateTime,
                        data.endDateTime
                    );

                    this.attributes = datesWithinRange.map((date) => {
                        let idx = datesWithinRange.indexOf(date);
                        if (idx == 0 || idx == datesWithinRange.length - 1) {
                            return {
                                key: idx,
                                customData: {
                                    class: "free-slot",
                                    title: moment(date).format("hh:mm"),
                                },
                                dates: date,
                            };
                        } else {
                            return {
                                key: idx,
                                customData: {
                                    class: "free-slot",
                                    title: "FREE",
                                },
                                dates: date,
                            };
                        }
                    });
                });
        },
        getDatesWithinRange(start, end) {
            //let startDay = new Date(moment(new Date(start)).startOf("day"));
            let startDay = new Date(start);
            let endDay = new Date(end);
            var datesWithinRange = [];
            for (var d = startDay; d <= endDay; d.setDate(d.getDate() + 1)) {
                datesWithinRange.push(new Date(d));
            }

            let validationDates = datesWithinRange.map((date) => {
                return moment(new Date(date)).startOf("day").format();
            });

            if (
                validationDates.indexOf(
                    moment(new Date(end)).startOf("day").format()
                ) == -1
            ) {
                datesWithinRange.push(endDay);
            }
            return datesWithinRange;
        },
    },
};
</script>

<style scoped>
.flexbox-container {
    display: flex;
    justify-content: space-between;
    padding: 50px;
    padding-bottom: 20px;
    padding-top: 20px;
}
.calendar {
    min-height: 400px;
    min-width: 400px;
}
.booked-slot {
    background: rgb(248, 125, 150);
}
.free-slot {
    background: rgb(20, 220, 110);
}
button {
    background-color: #fff000;
    border-radius: 12px;
    color: #000;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 15px;
    text-align: center;
    transition: 200ms;
    height: 50px;
    box-sizing: border-box;
    border: 0;
    font-size: 16px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
    margin-top: 7px;
}
button:hover {
    background-color: #ccbf05;
}
</style>
