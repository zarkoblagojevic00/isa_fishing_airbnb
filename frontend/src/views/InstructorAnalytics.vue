<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container-reservations">
        <h1>Analytics</h1>
    </div>
    <div class="flexbox-container-reservations">
        <h3>Marked reservations</h3>
    </div>
    <div class="flexbox-container">
        <table class="reservations-table">
            <thead>
                <tr>
                    <th>Service</th>
                    <th>From</th>
                    <th>To</th>
                    <th>Client</th>
                    <th>Price</th>
                    <th>Mark</th>
                </tr>
            </thead>
            <tbody v-for="res in reservations" :key="res.serviceId">
                <tr>
                    <td class="left">{{ res.serviceName }}</td>
                    <td class="left">
                        {{
                            res.serviceFrom ? dateFormat(res.serviceFrom) : "/"
                        }}
                    </td>
                    <td class="left">
                        {{ res.serviceTo ? dateFormat(res.serviceTo) : "/" }}
                    </td>
                    <td class="left">
                        {{ res.usersName }} {{ res.usersSurname }}
                        <font-awesome-icon
                            icon="info-circle"
                            style="cursor: pointer"
                            @click="
                                showUserInfo({
                                    usersName: res.usersName,
                                    usersSurname: res.usersSurname,
                                    usersPhoneNumber: res.usersPhoneNumber,
                                })
                            "
                        />
                    </td>
                    <td class="left">{{ res.price }}</td>
                    <td>
                        {{ res.mark == undefined ? "Not given" : res.mark }}
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div class="flexbox-container-reservations">
        <h3>Average marks</h3>
    </div>

    <div class="flexbox-container">
        <table class="reservations-table">
            <thead>
                <tr>
                    <th>Service</th>
                    <th>Average mark</th>
                </tr>
            </thead>
            <tbody v-for="mark in averageMarks" :key="mark.serviceName">
                <tr>
                    <td class="left">{{ mark.serviceName }}</td>
                    <td class="left">
                        {{ mark.averageMark }}
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="flexbox-container-reservations">
        <h3>Finished reservations</h3>
    </div>
    <div class="flexbox-container">
        <table class="reservations-table">
            <thead>
                <tr>
                    <th>Service</th>
                    <th>From</th>
                    <th>To</th>
                    <th>Client</th>
                    <th>Number of guests</th>
                    <th>Price</th>
                </tr>
            </thead>
            <tbody v-for="res in finishedReservations" :key="res.serviceId">
                <tr>
                    <td class="left">{{ res.serviceName }}</td>
                    <td class="left">
                        {{
                            res.serviceFrom ? dateFormat(res.serviceFrom) : "/"
                        }}
                    </td>
                    <td class="left">
                        {{ res.serviceTo ? dateFormat(res.serviceTo) : "/" }}
                    </td>
                    <td class="left">
                        {{ res.usersName }} {{ res.usersSurname }}
                    </td>
                    <td class="left">{{ res.capacity }}</td>
                    <td class="left">{{ res.price }}</td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="flexbox-container-reservations">
        <h3>Calculate revenue within date range</h3>
        <div class="flexbox-container">
            <v-date-picker v-model="range" is-range />
            <h3 class="revenue">Total revenue: {{ revenue }}</h3>
        </div>
    </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import axios from "../api/api.js";
import moment from "moment";
export default {
    name: "InstructorAnalytics",
    components: {
        Navbar,
    },
    computed: {},
    mounted() {
        axios
            .get("/api/Instructor/GetMarkedReservationsWithBasicUserInfo")
            .then((res) => {
                this.reservations = res.data;
            });
        axios.get("/api/Instructor/GetServiceAverageMarks").then((res) => {
            this.averageMarks = res.data;
        });
        axios
            .get("/api/Instructor/GetFinishedReservationsWithBasicUserInfo")
            .then((res) => {
                this.finishedReservations = res.data;
                this.revenue = this.finishedReservations
                    .map((res) => res.price)
                    .reduce((a, b) => a + b);
                this.revenue = Number(this.revenue).toFixed(2);
            });
    },
    data() {
        return {
            navbarItems: [
                "Services",
                "Reservations",
                "Availability",
                "Analytics",
                "My profile",
            ],
            baseUrlInstructor: "/instructor/",
            reservations: [],
            averageMarks: [],
            finishedReservations: [],
            range: "",
            revenue: 0,
        };
    },
    methods: {
        dateFormat(value) {
            return moment(value).format("YYYY-MM-DD HH:mm");
        },
    },
    watch: {
        range: {
            handler: function () {
                this.range.start.setHours(0, 0, 0, 0);
                this.range.end.setHours(23, 59, 59, 999);

                let finishedReservationsFiltered =
                    this.finishedReservations.filter((el) => {
                        return (
                            new Date(el.serviceTo) >= this.range.start &&
                            new Date(el.serviceTo) <= this.range.end
                        );
                    });

                if (finishedReservationsFiltered.length > 0) {
                    this.revenue = finishedReservationsFiltered
                        .map((res) => res.price)
                        .reduce((a, b) => a + b);
                    this.revenue = Number(this.revenue).toFixed(2);
                } else {
                    this.revenue = 0;
                }
            },
            deep: true,
        },
    },
};
</script>

<style scoped>
.revenue {
    padding: 50px;
}

.flexbox-container {
    padding: 50px;
    display: flex;
    justify-content: space-between;
    flex-wrap: wrap;
}
.flexbox-container-reservations {
    padding: 50px;
    padding-bottom: 1px;
    display: flex;
    justify-content: space-evenly;
    align-items: flex-start;
    flex-direction: column;
}
.reservations-table {
    border: solid 1px #ddeeee;
    border-collapse: collapse;
    border-spacing: 0;
    font: normal 13px Arial, sans-serif;
}
.reservations-table thead th {
    background-color: #ddefef;
    border: solid 1px #ddeeee;
    color: #336b6b;
    padding: 10px;
    text-align: left;
    text-shadow: 1px 1px 1px #fff;
}
.reservations-table tbody td {
    border: solid 1px #ddeeee;
    color: #333;
    padding: 10px;
    text-shadow: 1px 1px 1px #fff;
}
.left {
    text-align: left;
    width: 250px;
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
    width: 100%;
    box-sizing: border-box;
    border: 0;
    font-size: 16px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
}
button:hover {
    background-color: #ccbf05;
}
ul li {
    list-style: none;
    position: relative;
    padding: 10px;
}
li:hover {
    background-color: #c8e6e4;
}
h1 {
    padding: 0px;
}
</style>
