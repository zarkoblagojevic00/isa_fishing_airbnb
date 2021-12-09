<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container-reservations">
        <h1>Reservations</h1>
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
                    <th>Report</th>
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
                    <td class="left">{{ res.capacity }}</td>
                    <td class="left">{{ res.price }}</td>
                    <td class="left">
                        <font-awesome-icon
                            icon="plus-circle"
                            style="cursor: pointer"
                            @click="addReport(res.reservationId)"
                        />
                    </td>
                </tr>
            </tbody>
        </table>
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
    computed: {},
    mounted() {
        axios
            .get("/api/Instructor/GetReservationsWithBasicUserInfo")
            .then((res) => {
                this.reservations = res.data;
                console.log(res.data);
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
        };
    },
    methods: {
        dateFormat(value) {
            return moment(value).format("YYYY-MM-DD HH:mm");
        },
        showUserInfo(user) {
            this.$swal.fire({
                title:
                    "<p>" +
                    user.usersName +
                    " " +
                    user.usersSurname +
                    ", " +
                    user.usersPhoneNumber +
                    "</p>",
                icon: "info",

                focusConfirm: false,
                confirmButtonText: "Great!",
                confirmButtonAriaLabel: "Thumbs up, great!",
            });
        },
        async addReport(reservationId) {
            const { value: text } = await this.$swal.fire({
                input: "textarea",
                inputLabel: "Add report",
                inputPlaceholder: "Type your report here...",
                inputAttributes: {
                    "aria-label": "Type your report here",
                },
                showCancelButton: true,
            });

            if (text) {
                console.log(reservationId, text);
                axios
                    .post("/api/Instructor/SubmitReport", {
                        reservationId: reservationId,
                        reportText: text,
                    })
                    .then(() => {
                        this.$swal.fire("Report added successfully!");
                    })
                    .catch(function (error) {
                        if (error.response) {
                            // Request made and server responded
                            console.log(error.response.data);
                            console.log(error.response.status);
                            console.log(error.response.headers);
                        } else if (error.request) {
                            // The request was made but no response was received
                            console.log(error.request);
                        } else {
                            // Something happened in setting up the request that triggered an Error
                            console.log("Error", error.message);
                        }
                    });
            }
        },
    },
};
</script>

<style scoped>
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
