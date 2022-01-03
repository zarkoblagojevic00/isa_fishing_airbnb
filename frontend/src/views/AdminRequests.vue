<template>
    <AdminEntitiesNavbar :baseUrl="baseUrlInstructor" />
    <div class="heading">
        <h1>Requests</h1>
        <router-link to="/admin/new-admin" class="button-new-admin"
            >New admin</router-link
        >
    </div>
    <div class="flexbox-container-column"><b>Registration requests</b></div>
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
                    <td
                        class="left"
                        v-if="!req.isAccountActive && !req.denialReason"
                    >
                        <button
                            class="button-accept"
                            @click="onAcceptRequest(req.userId)"
                        >
                            Accept
                        </button>
                    </td>
                    <td
                        class="left"
                        v-if="!req.isAccountActive && !req.denialReason"
                    >
                        <button
                            class="button-decline"
                            @click="onDeclineRequest(req.userId)"
                        >
                            Decline
                        </button>
                    </td>
                    <td class="left" v-if="req.isAccountActive">
                        Account is active.
                    </td>
                    <td class="left" v-if="req.denialReason">
                        Request denied.
                    </td>
                </tr>
            </tbody>
        </table>
    </div>

    <div class="flexbox-container-column"><b>Mark requests</b></div>
    <div class="flexbox-container">
        <table class="reservations-table">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Surname</th>
                    <th>Email</th>
                    <th>Service name</th>
                    <th>Given mark</th>
                    <th>Mark description</th>
                    <th></th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody v-for="req in markRequests" :key="req.email">
                <tr>
                    <td class="left">{{ req.userName }}</td>
                    <td class="left">{{ req.userSurname }}</td>
                    <td class="left">{{ req.email }}</td>
                    <td class="left">{{ req.serviceName }}</td>
                    <td class="left">{{ req.mark }}</td>
                    <td class="left">{{ req.description }}</td>
                    <td class="left" v-if="!req.isReviewed">
                        <button
                            class="button-accept"
                            @click="onAcceptMarkRequest(req)"
                        >
                            Accept
                        </button>
                    </td>
                    <td class="left" v-if="!req.isReviewed">
                        <button
                            class="button-decline"
                            @click="onDeclineMarkRequest(req)"
                        >
                            Decline
                        </button>
                    </td>
                    <td class="left" v-if="req.isApproved">
                        Account is active.
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</template>

<script>
import AdminEntitiesNavbar from "@/components/AdminEntitiesNavbar.vue";
import axios from "../api/api.js";

export default {
    name: "AdminRequests",
    components: {
        AdminEntitiesNavbar,
    },
    mounted() {
        this.loadRequests();
    },
    methods: {
        loadRequests() {
            this.loadRegistrationRequests();
            this.loadMarkRequests();
        },
        loadRegistrationRequests() {
            axios.get("/api/RegistrationReview/GetAll").then((res) => {
                this.requests = res.data;
            });
        },
        loadMarkRequests() {
            axios.get("/api/Admin/GetUnapprovedMarks").then((res) => {
                this.markRequests = res.data;
            });
        },
        onAcceptRequest(userId) {
            axios
                .put("/api/RegistrationReview/ReviewRequest", {
                    userId: userId,
                    result: true,
                    reason: "Meets all conditions.",
                })
                .then(() => {
                    this.$swal.fire("Successfully saved.");
                    this.loadRequests();
                });
        },
        onAcceptMarkRequest(markRequest) {
            axios.put("/api/Admin/ApproveMarkRequest", markRequest).then(() => {
                this.$swal.fire("Mark approved!");
                this.loadMarkRequests();
            });
        },
        onDeclineMarkRequest(markRequest) {
            axios.put("/api/Admin/DeclineMarkRequest", markRequest).then(() => {
                this.$swal.fire("Mark declined!");
                this.loadMarkRequests();
            });
        },
        onDeclineRequest(userId) {
            this.$swal
                .fire({
                    title: "Enter reason for declining request.",
                    input: "text",
                    inputAttributes: {
                        autocapitalize: "off",
                    },
                    showCancelButton: true,
                    confirmButtonText: "Submit",
                    showLoaderOnConfirm: true,
                    preConfirm: (reason) => {
                        console.log("reason", reason);
                        axios
                            .put("/api/RegistrationReview/ReviewRequest", {
                                userId: userId,
                                result: false,
                                reason: reason,
                            })
                            .then(() => this.loadRequests());
                    },
                    allowOutsideClick: () => !this.$swal.isLoading(),
                })
                .then((result) => {
                    console.log("result", result);
                    if (result.isConfirmed) {
                        this.$swal.fire("Successfully saved.");
                    }
                });
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
            markRequests: [],
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
.heading {
    display: flex;
    justify-content: space-between;
    padding: 50px;
}

.flexbox-container {
    padding: 50px;
    display: flex;
    justify-content: space-between;
    flex-wrap: wrap;
}
.flexbox-container-column {
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

.button-new-admin {
    background-color: #000000;
    border-radius: 12px;
    color: rgb(255, 255, 255);
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    text-align: center;
    width: 120px;
    height: 50px;
    box-sizing: border-box;
    border: 0;
    font-size: 12px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
    text-decoration: none;
    align-items: center;
    font-size: 14px;
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
