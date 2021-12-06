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
                    </td>
                    <td class="left">{{ res.capacity }}</td>
                    <td class="left">{{ res.price }}</td>
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
