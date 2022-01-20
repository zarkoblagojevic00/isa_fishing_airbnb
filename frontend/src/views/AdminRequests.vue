<template>
    <AdminEntitiesNavbar
        :baseUrl="baseUrlInstructor"
        @requestTypeChanged="onRequestTypeChanged"
    />
    <div class="heading">
        <h1>Requests</h1>
        <router-link to="/admin/new-admin" class="button-new-admin"
            >New admin</router-link
        >
    </div>
    <div class="flexbox-container-column" v-if="requestMode == '0'">
        <b>Registration requests</b>
    </div>
    <div class="flexbox-container" v-if="requestMode == '0'">
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
            <tbody v-for="req in registrationRequests" :key="req.userId">
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
                            @click="onAcceptRegistrationRequest(req.userId)"
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
                            @click="onDeclineRegistrationRequest(req.userId)"
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

    <div class="flexbox-container-column" v-if="requestMode == '1'">
        <b>Mark requests</b>
    </div>
    <div class="flexbox-container" v-if="requestMode == '1'">
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
    <div class="flexbox-container-column" v-if="requestMode == '2'">
        <b>Account deletion requests</b>
    </div>
    <div class="flexbox-container" v-if="requestMode == '2'">
        <table class="reservations-table">
            <thead>
                <tr>
                    <th>Name</th>
                    <th>Surname</th>
                    <th>Email</th>
                    <th>Reason</th>
                    <th></th>
                    <th></th>
                    <th></th>
                </tr>
            </thead>
            <tbody v-for="req in deletionRequests" :key="req.email">
                <tr>
                    <td class="left">{{ req.name }}</td>
                    <td class="left">{{ req.surname }}</td>
                    <td class="left">{{ req.email }}</td>
                    <td class="left">{{ req.reason }}</td>
                    <td class="left" v-if="!req.isReviewed">
                        <button
                            class="button-accept"
                            @click="onAcceptDeletionRequest(req)"
                        >
                            Accept
                        </button>
                    </td>
                    <td class="left" v-if="!req.isReviewed">
                        <button
                            class="button-decline"
                            @click="onDeclineDeletionRequest(req)"
                        >
                            Decline
                        </button>
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
        this.loadRequests(this.$route.params.data);
        if (this.$route.params.data == undefined) this.requestMode = "0";
        else this.requestMode = this.$route.params.data;
    },
    methods: {
        onRequestTypeChanged(type) {
            this.requestMode = type;
            this.loadRequests(type);
        },
        loadRequests(type) {
            if (type == "0") this.loadRegistrationRequests();
            else if (type == "1") this.loadMarkRequests();
            else if (type == "2") this.loadDeletionRequests();
            else this.loadRegistrationRequests();
        },
        loadRegistrationRequests() {
            axios.get("/api/RegistrationReview/GetAll").then((res) => {
                this.registrationRequests = res.data;
            });
        },
        loadMarkRequests() {
            axios.get("/api/Admin/GetUnapprovedMarks").then((res) => {
                this.markRequests = res.data;
            });
        },
        loadDeletionRequests() {
            axios.get("/api/Admin/GetDeletionRequests").then((res) => {
                this.deletionRequests = res.data;
            });
        },
        onAcceptDeletionRequest(req) {
            req.isApproved = true;
            axios
                .put("/api/Admin/RespondAccountDeletionRequest", req)
                .then(() => {
                    this.$swal.fire("Successfully saved.");
                    this.loadRequests("2");
                })
                // .catch((error) => {
                //     if (error.response) {
                //         this.$swal.fire(error.response.data);
                //     }
                // });
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
        },
        onDeclineDeletionRequest(req) {
            req.isApproved = false;
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
                        req.response = reason;
                        axios
                            .put(
                                "/api/Admin/RespondAccountDeletionRequest",
                                req
                            )
                            .then(() => this.loadRequests("2"))

                            .catch((error) => {
                                if (error.response) {
                                    // Request made and server responded
                                    this.$swal.fire(error.response.data);
                                }
                            });
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
        onAcceptRegistrationRequest(userId) {
            axios
                .put("/api/RegistrationReview/ReviewRequest", {
                    userId: userId,
                    result: true,
                    reason: "Meets all conditions.",
                })
                .then(() => {
                    this.$swal.fire("Successfully saved.");
                    this.loadRequests("0");
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
        onDeclineRegistrationRequest(userId) {
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
                            .then(() => this.loadRequests("0"));
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
            registrationRequests: [],
            markRequests: [],
            review: {
                userId: "",
                result: false,
                reason: "",
            },
            requestMode: "0",
            deletionRequests: [],
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
    background-color: #52d146;
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
    background-color: #992d2d;
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
    background-color: #3247be;
}
</style>
