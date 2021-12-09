<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="heading">
        <h1>New quick action</h1>
    </div>
    <div class="flexbox-container">
        <div class="flexbox-row">
            <div class="item bold">Start:</div>
            <v-date-picker
                v-model="v$.quickAction.startDateTime.$model"
                mode="dateTime"
                is24hr
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
                v-for="(error, index) of v$.quickAction.startDateTime.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>

        <div class="flexbox-row">
            <div class="item bold">End:</div>
            <v-date-picker
                v-model="v$.quickAction.endDateTime.$model"
                mode="dateTime"
                is24hr
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
                v-for="(error, index) of v$.quickAction.endDateTime.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Price per day:</div>
            <input
                type="number"
                class="item"
                v-model="v$.quickAction.pricePerDay.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.quickAction.pricePerDay.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Added benefits:</div>
            <input
                type="text"
                class="item"
                v-model="quickAction.addedBenefits"
            />
        </div>
        <div class="flexbox-row">
            <div class="item bold">Capacity:</div>
            <input
                type="number"
                class="item"
                v-model="v$.quickAction.capacity.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.quickAction.capacity.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Place:</div>
            <input
                type="text"
                class="item"
                v-model="v$.quickAction.place.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.quickAction.place.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
    </div>
    <div class="button-row">
        <button class="button-decline" @click="onCancelEdit">Cancel</button>
        <button
            class="button-accept"
            @click="onSaveEdit"
            :disabled="v$.quickAction.$invalid"
        >
            Save
        </button>
    </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import axios from "../api/api.js";

import { useVuelidate } from "@vuelidate/core";
import { required } from "@vuelidate/validators";

export default {
    name: "AdventureNewQuickAction",
    components: {
        Navbar,
    },
    mounted() {},
    methods: {
        onCancelEdit() {
            this.quickAction = {};
        },
        onSaveEdit() {
            this.quickAction.serviceId = this.$route.params.id;
            const quickAction_obj = Object.assign({}, this.quickAction);
            console.log(quickAction_obj);
            axios
                .post("/api/QuickAction/CreateNewQuickAction", quickAction_obj)
                .then(({ data }) => {
                    console.log(data);
                    this.$swal.fire("Quick action created");
                    this.$router.push(
                        "/adventure/" +
                            this.$route.params.id +
                            "/" +
                            "quick-reservation"
                    );
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
    },
    setup() {
        return { v$: useVuelidate() };
    },
    data() {
        return {
            baseUrlInstructor: "/adventure/" + this.$route.params.id + "/",
            quickAction: {
                serviceId: "",
                startDateTime: "",
                endDateTime: "",
                pricePerDay: "",
                addedBenefits: "",
                capacity: "",
                place: "",
            },
            navbarItems: ["Home", "Quick reservation", "Gallery"],
        };
    },
    validations() {
        return {
            quickAction: {
                startDateTime: { required },
                endDateTime: { required },
                pricePerDay: { required },
                capacity: { required },
                place: { required },
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
