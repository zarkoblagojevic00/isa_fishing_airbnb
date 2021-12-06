<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <h1>Requests</h1>
    <div class="flexbox-container">
        <table class="reservations-table">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Surname</th>
                    <th>Address</th>
                    <th>City</th>
                    <th>Country</th>
                    <th>Phone number</th>
                    <th>Email</th>
                    <th>User type</th>
                    <th>Reason</th>
                    <th></th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody v-for="req in requests" :key="req.userId">
                <tr>
                    <td class="left">{{ req.name }}</td>
                    <td class="left">{{ req.surname }}</td>
                    <td class="left">{{ req.address }}</td>
                    <td class="left">{{ req.city }}</td>
                    <td class="left">{{ req.country }}</td>
                    <td class="left">{{ req.phoneNumber }}</td>
                    <td class="left">{{ req.email }}</td>
                    <td class="left">{{ req.userType }}</td>
                    <td class="left">{{ req.reason }}</td>
                    <td class="left" v-if="!req.isAccountVerified">
                        <button
                            class="button-accept"
                            @click="onAcceptRequest(req.userId)"
                        >
                            Accept
                        </button>
                    </td>
                    <td class="left" v-if="!req.isAccountVerified">
                        <button
                            class="button-decline"
                            @click="onDeclineRequest(req.userId)"
                        >
                            Decline
                        </button>
                    </td>
                    <td class="left" v-if="req.isAccountVerified">
                        Account is verified.
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import axios from "../api/api.js";

export default {
    name: "AdminRequests",
    components: {
        Navbar,
    },
    mounted() {
        this.loadRequests();
    },
    methods: {
        loadRequests() {
            axios.get("/api/RegistrationReview/GetAll").then((res) => {
                this.requests = res.data;
                console.log(res.data);
            });
        },
        onAcceptRequest(userId) {
            axios
                .put("/api/RegistrationReview/ReviewRequest", {
                    userId: userId,
                    result: true,
                    reason: "Meets all conditions.",
                })
                .catch((err) => console.log(err));
        },
        onDeclineRequest(userId) {
            axios
                .put("/api/RegistrationReview/ReviewRequest", {
                    userId: userId,
                    result: false,
                    reason: "Does not meet all conditions.",
                })
                .catch((err) => console.log(err));
        },
    },
    data() {
        return {
            navbarItems: [
                "Requests",
                "Users",
                "Entities",
                "Analytics",
                "My profile",
            ],
            baseUrlInstructor: "/admin/",
            user: {
                userId: "",
                name: "",
                surname: "",
                address: "",
                city: "",
                country: "",
                phoneNumber: "",
                email: "",
                userType: "",
                reason: "",
                denialReason: "",
                isAccountActive: false,
                isAccountVerified: false,
            },
            requests: [],
            review: {
                userId: "",
                result: false,
                reason: "",
            },
        };
    },
};
</script>

<style scoped>
h1 {
    display: flex;
    justify-content: flex-start;
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

.button-accept {
    background-color: #15ff00;
    border-radius: 12px;
    color: #000;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    text-align: center;
    transition: 200ms;
    width: 100%;
    box-sizing: border-box;
    border: 0;
    font-size: 12px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
}

.button-decline {
    background-color: #eb1414;
    border-radius: 12px;
    color: #000;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    text-align: center;
    transition: 200ms;
    width: 100%;
    box-sizing: border-box;
    border: 0;
    font-size: 12px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
}

button:hover {
    background-color: #2843d8;
}
</style>
