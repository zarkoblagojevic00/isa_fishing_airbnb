// Redundant
<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container">
        <h1>Edit availability</h1>
    </div>
    <div class="flexbox">
        <v-date-picker v-model="range" mode="dateTime" is-range />
        <p>
            Availability period is set from
            <b>{{ dateFormat(range.start) }}h</b> to
            <b>{{ dateFormat(range.end) }}h</b>.
        </p>
        <button @click="updateAvailability">Save changes</button>
    </div>
    <p>{{ this.successMessage }}</p>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import moment from "moment";
import axios from "../api/api.js";

export default {
    name: "InstructorEditAvailability",
    components: {
        Navbar,
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
            range: {
                start: new Date(),
                end: new Date(),
            },
            successMessage: "",
        };
    },
    mounted() {
        axios
            .get("/api/Instructor/GetAdditionalInstructorInfo")
            .then(({ data }) => {
                this.range.start = data.startDateTime;
                this.range.end = data.endDateTime;
            });
    },
    methods: {
        dateFormat(value) {
            return moment(value).format("YYYY-MM-DD HH:mm");
        },
        updateAvailability() {
            const range_obj = Object.assign({}, this.range);
            console.log(range_obj);
            axios
                .put(
                    "/api/Instructor/UpdateAdditionalInstructorInfo",
                    range_obj
                )
                .then(() => {
                    this.successMessage = "Updated successfully.";
                })
                .catch(() => {
                    this.successMessage = "Error occured while updating info.";
                });
        },
    },
};
</script>

<style scoped>
.flexbox-container {
    display: flex;
    justify-content: flex-start;
    padding: 50px;
    padding-bottom: 20px;
    padding-top: 20px;
}

.flexbox {
    display: flex;
    justify-content: space-evenly;
    padding: 50px;
    padding-bottom: 20px;
    padding-top: 20px;
}

button {
    background-color: #000;
    border-radius: 12px;
    color: #fff;
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
    float: left;
    margin-left: 50px;
    margin-bottom: 50px;
}

button:hover {
    background-color: rgb(35, 46, 97);
}
</style>
