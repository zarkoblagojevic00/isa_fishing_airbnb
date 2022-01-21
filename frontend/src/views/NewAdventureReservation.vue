<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="heading">
        <h1>New reservation for user</h1>
    </div>
    <div class="flexbox-group">
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
                    readonly="true"
                />
            </div>
            <div class="flexbox-row">
                <div class="item bold">Start:</div>
                <v-date-picker
                    v-model="v$.newReservation.startDateTime.$model"
                    mode="dateTime"
                    is24hr
                    :min-date="
                        v$.newReservation.endDateTime.$model
                            ? v$.newReservation.endDateTime.$model
                            : new Date()
                    "
                    :max-date="
                        v$.newReservation.endDateTime.$model
                            ? v$.newReservation.endDateTime.$model
                            : new Date() + 365
                    "
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
                    :min-date="
                        v$.newReservation.startDateTime.$model
                            ? v$.newReservation.startDateTime.$model
                            : new Date()
                    "
                    :max-date="
                        v$.newReservation.startDateTime.$model
                            ? v$.newReservation.startDateTime.$model
                            : new Date() + 365
                    "
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
                    v-for="(error, index) of v$.newReservation.endDateTime
                        .$errors"
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
        </div>
        <div class="input-wrapper">
            <span class="label">Added benefits:</span>
            <div class="tag-div">
                <div
                    v-for="tag in additionalEquipmentArray"
                    :key="tag.name"
                    class="tag"
                    @click="ClearTag(tag.name)"
                >
                    {{ tag.name + ": " + tag.price }}
                </div>
            </div>
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
                    this.getAdventure(this.reservationForUser.serviceName);
                    console.log(data);
                });
        },
        createReservationForUser() {
            let eq = this.MergeAdditioanlEquipment();
            this.newReservation.additionalEquipment = eq;

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
                    this.$swal.fire("Reservation created.");
                    this.$router.push({
                        path: this.baseUrlInstructor + "reservations",
                    });
                })
                .catch((error) => {
                    if (error.response) {
                        this.$swal(error.response.data);
                    }
                });
        },
        onCreateReservation() {
            if (
                new Date(this.newReservation.startDateTime) >
                new Date(this.newReservation.endDateTime)
            ) {
                this.$swal.fire("Start time must be before end time.");
            } else {
                this.createReservationForUser();
            }
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
        getAdventure(name) {
            axios
                .get("/api/Adventure/GetAdventureByName?name=" + name)
                .then(({ data }) => {
                    this.currentAdventure = data;
                    this.ParseAdditionalEquipment(
                        this.currentAdventure.additionalEquipment
                    );
                });
        },
        ParseAdditionalEquipment(additionalEquipment) {
            this.additionalEquipmentArray = new Array();
            if (
                additionalEquipment == null ||
                additionalEquipment == undefined
            ) {
                return;
            }

            let receivedEq = additionalEquipment.split(";");
            for (let eq of receivedEq) {
                let name = eq.split(":")[0];
                let price = eq.split(":")[1];
                if (name === "" || price === "") {
                    continue;
                }
                this.additionalEquipmentArray.push({
                    name: name,
                    price: price,
                });
            }
        },
        ClearTag(tagName) {
            for (let i = 0; i < this.additionalEquipmentArray.length; i++) {
                if (this.additionalEquipmentArray[i].name == tagName) {
                    this.additionalEquipmentArray.splice(i, 1);
                    return;
                }
            }
        },
        AddTag() {
            if (this.newTagName === "" || this.newTagPrice === "") {
                return;
            }
            for (let tag of this.additionalEquipmentArray) {
                if (tag.name == this.newTagName) {
                    return;
                }
            }
            this.additionalEquipmentArray.push({
                name: this.newTagName,
                price: this.newTagPrice,
            });
            this.newTagName = "";
            this.newTagPrice = "";
        },
        MergeAdditioanlEquipment() {
            let ret = "";
            for (let eq of this.additionalEquipmentArray) {
                ret += eq.name + ":" + eq.price + ";";
            }
            return ret;
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
            currentAdventure: "",
            additionalEquipmentArray: [],
        };
    },
    validations() {
        return {
            newReservation: {
                price: { required, minValue: minValue(0) },
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

.flexbox-group {
    display: flex;
    justify-content: flex-start;
}

.equipment-input {
    height: 50px;
    width: 165px;
    outline: none;
    border-radius: 25px;
    padding-left: 20px;
    padding-right: 10px;
    -moz-appearance: textfield;
}
.equipment-input::-webkit-inner-spin-button {
    -webkit-appearance: none;
}
.add-equipment {
    margin-top: 10px;
    height: 40px;
    outline: none;
    border: none;
    width: 60px;
    border-radius: 25px;
    background-color: #345fed;
    color: white;
    cursor: pointer;
}

.tag-div {
    display: flex;
    flex-flow: row wrap;
    margin-top: 10px;
    max-width: 400px;
}

.tag {
    margin-top: 5px;
    margin-right: 7px;
    font-size: 12px;
    background-color: #345fed;
    color: white;
    padding-left: 5px;
    padding-right: 30px;
    padding-top: 7px;
    padding-bottom: 7px;
    border-radius: 8px;
    cursor: pointer;
    background-image: url("../assets/white-x.png");
    background-repeat: no-repeat;
    background-position: right 2px center;
    background-size: 23px 23px;
}

.input-wrapper {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    margin-bottom: 15px;
    margin-left: 100px;
}

.horizontal {
    display: flex;
    flex-direction: row;
}

.horizontal-part {
    display: flex;
    flex-direction: column;
    margin-top: 10px;
}
.left {
    margin-right: 10px;
}
</style>
