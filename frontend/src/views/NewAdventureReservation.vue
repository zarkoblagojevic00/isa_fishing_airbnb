<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="heading">
        <h1>New reservation for user</h1>
    </div>
    <div class="flexbox-container">
        <div class="flexbox-row">
            <div class="item bold">Adventure:</div>
            <input
                type="text"
                class="item"
                v-model="reservationForUser.serviceName"
                list="instructorServices"
            />
            <datalist id="instructorServices">
                <option
                    v-for="service in instructorServices"
                    :key="service.name"
                    :value="service.name"
                />
            </datalist>
        </div>
        <div class="flexbox-row">
            <div class="item bold">User email:</div>
            <input
                type="text"
                class="item"
                v-model="reservationForUser.userEmail"
            />
        </div>
        <div class="flexbox-row">
            <div class="item bold">Start:</div>
            <v-date-picker
                v-model="v$.newReservation.startDateTime.$model"
                mode="dateTime"
                is24hr
                :min-date="new Date()"
            >
                <template v-slot="{ inputValue, inputEvents }">
                    <input
                        class="
                            px-2
                            py-1
                            border
                            rounded
                            focus:outline-none focus:border-blue-300
                        "
                        :value="inputValue"
                        v-on="inputEvents"
                    />
                </template>
            </v-date-picker>
            <div
                class="input-errors"
                v-for="(error, index) of v$.newReservation.startDateTime
                    .$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>

        <div class="flexbox-row">
            <div class="item bold">End:</div>
            <v-date-picker
                v-model="v$.newReservation.endDateTime.$model"
                mode="dateTime"
                is24hr
                :min-date="new Date()"
            >
                <template v-slot="{ inputValue, inputEvents }">
                    <input
                        class="
                            px-2
                            py-1
                            border
                            rounded
                            focus:outline-none focus:border-blue-300
                        "
                        :value="inputValue"
                        v-on="inputEvents"
                    />
                </template>
            </v-date-picker>
            <div
                class="input-errors"
                v-for="(error, index) of v$.newReservation.endDateTime.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Price per hour:</div>
            <input
                type="number"
                class="item"
                v-model="v$.newReservation.price.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.newReservation.price.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Additional equipment:</div>
            <input
                type="text"
                class="item"
                v-model="newReservation.additionalEquipment"
            />
        </div>
    </div>
    <div class="button-row">
        <button
            class="button-accept"
            @click="onCreateReservation"
            :disabled="v$.newReservation.$invalid"
        >
            Save
        </button>
    </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import axios from "../api/api.js";

import { useVuelidate } from "@vuelidate/core";
import { required, minValue } from "@vuelidate/validators";

export default {
    name: "NewAdventureReservation",
    components: {
        Navbar,
    },
    mounted() {
        this.getReservationForUser(this.$route.params.id);
        this.getInstructorServices();
    },
    methods: {
        getReservationForUser(reservationId) {
            axios
                .get(
                    "/api/Instructor/GetReservationForUser?reservationId=" +
                        reservationId
                )
                .then(({ data }) => {
                    this.reservationForUser = data;
                    console.log(data);
                });
        },
        createReservationForUser() {
            this.newReservation.userMail = this.reservationForUser.userEmail;
            this.newReservation.serviceId = this.instructorServices.find(
                (e) => e.name == this.reservationForUser.serviceName
            ).serviceId;
            axios
                .post(
                    "/api/GeneralService/CreateReservationForUser",
                    this.newReservation
                )
                .then(() => {
                    console.log("Success");
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
        },
        onCreateReservation() {
            this.createReservationForUser();
        },
        getInstructorServices() {
            axios
                .get("/api/Instructor/GetInstructorServices")
                .then(({ data }) => {
                    this.instructorServices = data;
                    console.log(data);
                });
        },
        onCancel() {
            console.log(this.reservationForUser);
        },
    },
    setup() {
        return { v$: useVuelidate() };
    },
    data() {
        return {
            reservationForUser: {
                userEmail: "",
                serviceId: "",
                serviceName: "",
            },
            newReservation: {
                userMail: "",
                serviceId: "",
                price: "",
                additionalEquipment: "",
                startDateTime: "",
                endDateTime: "",
            },
            navbarItems: [
                "Services",
                "Reservations",
                "Availability",
                "Analytics",
                "My profile",
            ],
            baseUrlInstructor: "/instructor/",
            instructorServices: [],
        };
    },
    validations() {
        return {
            newReservation: {
                price: { required, minValue: minValue(0) },
                additionalEquipment: { required },
                startDateTime: { required },
                endDateTime: { required },
            },
        };
    },
};
</script>

<style scoped>
.flexbox-container {
    padding: 50px;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: flex-start;
}

.flexbox-row {
    display: flex;
    justify-content: flex-start;
    margin-bottom: 10px;
}

.item {
    width: 15rem;
    display: flex;
    justify-content: flex-start;
    margin-right: 10px;
}

.bold {
    font-weight: bolder;
}

.heading {
    display: flex;
    justify-content: flex-start;
    padding: 50px;
}

input {
    font-size: 14px;
}

.icon:hover {
    cursor: pointer;
}

.button-accept {
    background-color: #0011ff;
    border-radius: 12px;
    color: rgb(255, 255, 255);
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    border: 0;
    font-size: 12px;
    width: 70px;
}

.button-decline {
    background-color: #eb1414;
    border-radius: 12px;
    color: #000;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    border: 0;
    font-size: 12px;
    margin-right: 50px;
    width: 70px;
}

.button-row {
    display: flex;
    justify-content: flex-start;
    padding-left: 50px;
    padding-bottom: 50px;
}

.input-errors {
    color: red;
    font-size: 10px;
}

p {
    display: flex;
    justify-content: flex-start;
    padding-left: 50px;
}

button[disabled="disabled"],
button:disabled {
    background-color: rgb(74, 75, 77);
}
</style>
