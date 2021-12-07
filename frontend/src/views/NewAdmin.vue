<template>
    <AdminEntitiesNavbar :baseUrl="baseUrlInstructor" />
    <div class="heading">
        <h1>Admin profile</h1>
    </div>
    <div class="flexbox-container">
        <div class="flexbox-row">
            <div class="item bold">Name:</div>
            <input type="text" class="item" v-model="v$.userInfo.name.$model" />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.name.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>

        <div class="flexbox-row">
            <div class="item bold">Surname:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.surname.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.surname.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Address:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.address.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.address.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Phone number:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.phone.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.phone.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">Email:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.email.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.email.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <div class="flexbox-row">
            <div class="item bold">City:</div>
            <input
                type="text"
                class="item"
                v-model="v$.userInfo.city.$model"
                list="cities"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.userInfo.city.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </div>
        <datalist id="cities">
            <option v-for="city in cities" :key="city.id" :value="city.name" />
        </datalist>
    </div>
    <p>
        Predefined password will be assigned and admin will be obligated to
        change it upon first signup.
    </p>
    <div class="button-row">
        <button class="button-decline" @click="onCancelEdit">Cancel</button>
        <button class="button-accept" @click="onSaveEdit">Save</button>
    </div>
</template>

<script>
import AdminEntitiesNavbar from "@/components/AdminEntitiesNavbar.vue";
import axios from "../api/api.js";

import { useVuelidate } from "@vuelidate/core";
import { required, email } from "@vuelidate/validators";

export default {
    name: "NewAdmin",
    components: {
        AdminEntitiesNavbar,
    },
    mounted() {
        this.GetCities();
    },
    methods: {
        onCancelEdit() {
            this.userInfo = {};
        },
        onSaveEdit() {
            const Toast = this.$swal.mixin({
                toast: true,
                position: "top-end",
                showConfirmButton: false,
                timer: 3000,
            });
            axios
                .post("/api/Registration/AddNewAdmin", this.userInfo)
                .then(() => {
                    Toast.fire({
                        icon: "success",
                        title: "Verification email sent to address.",
                    });
                    console.log(this.userInfo);
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
        GetCities() {
            axios.get("/api/City/GetCities").then((res) => {
                this.cities = res.data;
            });
        },
    },
    setup() {
        return { v$: useVuelidate() };
    },
    data() {
        return {
            baseUrlInstructor: "/admin/",
            userInfo: {
                name: "",
                surname: "",
                address: "",
                phone: "",
                email: "",
                city: "",
            },
            cities: [],
        };
    },
    validations() {
        return {
            userInfo: {
                name: { required },
                surname: { required },
                address: { required },
                phone: { required },
                email: { email },
                city: { required },
                country: { required },
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

button:hover {
    background-color: #2843d8;
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
</style>
