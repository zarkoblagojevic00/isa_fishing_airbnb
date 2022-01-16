<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container-space-between">
        <h1>Edit adventure</h1>
    </div>
    <div class="flexbox-container-space-between">
        <form class="form">
            <div class="flexbox-row">
                <label> Name </label>
                <input
                    type="text"
                    class="field"
                    placeholder="Adventure name"
                    v-model="v$.newAdventure.name.$model"
                />
            </div>

            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.name.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>

            <div class="flexbox-row">
                <label> Address </label>
                <input
                    type="text"
                    class="field"
                    placeholder="Address"
                    v-model="v$.newAdventure.address.$model"
                />
            </div>

            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.address.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>

            <div class="flexbox-row">
                <label> Longitude </label>
                <input
                    type="number"
                    class="field"
                    placeholder="Longitude"
                    v-model="v$.newAdventure.longitude.$model"
                />
            </div>

            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.longitude.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
            <div class="flexbox-row">
                <label> Latitude </label>
                <input
                    type="number"
                    class="field"
                    placeholder="Latitude"
                    v-model="v$.newAdventure.latitude.$model"
                />
            </div>

            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.latitude.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>

            <div class="flexbox-row">
                <label> Promo description </label>
                <input
                    type="text"
                    class="field"
                    placeholder="Promo description"
                    v-model="v$.newAdventure.promoDescription.$model"
                />
            </div>
            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.promoDescription
                    .$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
            <div class="flexbox-row">
                <label> Price per day </label>

                <input
                    type="number"
                    class="field"
                    placeholder="Price per day"
                    v-model="v$.newAdventure.pricePerDay.$model"
                />
            </div>
            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.pricePerDay.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>

            <div class="flexbox-row">
                <label> Max people </label>

                <input
                    type="number"
                    class="field"
                    placeholder="Maximum number of people"
                    v-model="v$.newAdventure.capacity.$model"
                />
            </div>

            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.capacity.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
            <div class="flexbox-row">
                <label> Terms of use </label>

                <input
                    type="text"
                    class="field"
                    placeholder="Terms of use"
                    v-model="newAdventure.termsOfUse"
                />
            </div>
            <div class="flexbox-row">
                <label> Additional equipment </label>

                <input
                    type="text"
                    class="field"
                    placeholder="Additional equipment"
                    v-model="newAdventure.additionalEquipment"
                />
            </div>

            <div class="flexbox-row">
                <label> Additional offers </label>

                <input
                    type="text"
                    class="field"
                    placeholder="Additional offers"
                    v-model="newAdventure.additionalOffers"
                />
            </div>

            <fieldset
                id="group1"
                style="margin-top: 10px; font-size: 14px; width: 20rem"
            >
                <div class="flexbox-row">
                    <label>Is percentage taken upon cancellation</label>
                    <span> Yes </span>
                    <input
                        type="radio"
                        value="value1"
                        name="group1"
                        class="radio"
                        @click="
                            newAdventure.isPercentageTakenFromCanceledReservations = true
                        "
                    />
                    <span> No </span>
                    <input
                        type="radio"
                        value="value2"
                        name="group1"
                        class="radio"
                        @click="noClicked"
                    />
                </div>
            </fieldset>
            <div class="flexbox-row">
                <label> If yes, how much? </label>

                <input
                    type="number"
                    class="field"
                    placeholder="If yes, how much?"
                    v-model="v$.newAdventure.percentageToTake.$model"
                    :disabled="
                        newAdventure.isPercentageTakenFromCanceledReservations ==
                        false
                    "
                />
            </div>

            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.percentageToTake
                    .$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
        </form>
    </div>
    <div class="flexbox">
        <button
            class="button-update"
            @click="onSubmit"
            :disabled="v$.newAdventure.$invalid"
        >
            Update adventure
        </button>
        <button class="button-remove" @click="onRemove">
            Remove adventure
        </button>
    </div>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import { useVuelidate } from "@vuelidate/core";
import { required, minValue, maxValue } from "@vuelidate/validators";
import axios from "../api/api.js";

export default {
    name: "EditAdventure",
    components: {
        Navbar,
    },
    setup() {
        return { v$: useVuelidate() };
    },
    mounted() {
        this.loadAdventure();
    },
    data() {
        return {
            navbarItems: ["Home", "Quick reservation", "Gallery"],
            baseUrlInstructor: "/adventure/" + this.$route.params.id + "/",
            newAdventure: {
                adventureId: "",
                name: "",
                address: "",
                longitude: "",
                latitude: "",
                promoDescription: "",
                pricePerDay: "",
                capacity: "",
                termsOfUse: "",
                additionalEquipment: "",
                additionalOffers: "",
                isPercentageTakenFromCanceledReservations: false,
                percentageToTake: 0,
            },
        };
    },
    validations() {
        return {
            newAdventure: {
                name: { required },
                address: { required },
                longitude: { required, min: minValue(-180), max: maxValue(0) },
                latitude: { required, min: minValue(-90), max: maxValue(90) },
                promoDescription: { required },
                pricePerDay: { required, min: minValue(0) },
                capacity: { required, min: minValue(0) },
                percentageToTake: { min: minValue(0), max: maxValue(100) },
            },
        };
    },
    methods: {
        noClicked() {
            this.newAdventure.isPercentageTakenFromCanceledReservations = false;
            this.newAdventure.percentageToTake = 0;
        },
        onSubmit() {
            console.log(this.newAdventure);
            const adventure = Object.assign({}, this.newAdventure);
            console.log(adventure);

            axios
                .put("/api/Adventure/UpdateAdventure", adventure)
                .then(() => {
                    this.$swal.fire(
                        "Updated!",
                        "Your adventure has been updated.",
                        "success"
                    );
                    this.$router.push(this.baseUrlInstructor);
                })
                .catch((error) => {
                    if (error.response) {
                        // Request made and server responded
                        console.log(error.response.data);
                        this.$swal.fire(error.response.data);
                    } else if (error.request) {
                        // The request was made but no response was received
                        console.log(error.request);
                    } else {
                        // Something happened in setting up the request that triggered an Error
                        console.log("Error", error.message);
                    }
                });
        },
        loadAdventure() {
            axios
                .get(
                    "/api/Adventure/GetAdventureInfoById?adventureId=" +
                        this.$route.params.id
                )
                .then((res) => {
                    this.newAdventure = res.data;
                    console.log(res.data);
                })
                .catch((err) => console.log(err));
        },
        onRemove() {
            this.$swal
                .fire({
                    title: "Are you sure?",
                    text: "You won't be able to revert this!",
                    icon: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#3085d6",
                    cancelButtonColor: "#d33",
                    confirmButtonText: "Yes, delete it!",
                })
                .then((result) => {
                    if (result.isConfirmed) {
                        axios
                            .delete(
                                "/api/Adventure/DeleteAdventure?adventureId=" +
                                    this.newAdventure.adventureId
                            )
                            .then(() => {
                                this.$swal.fire(
                                    "Deleted!",
                                    "Your adventure has been deleted.",
                                    "success"
                                );
                                this.$router.push("/instructor/services");
                            });
                    }
                });
        },
    },
};
</script>

<style scoped>
.form {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    margin-right: 10rem;
}

.flexbox-container-space-between {
    padding: 50px;
    display: flex;
    justify-content: flex-start;
    align-items: center;
}

.flexbox {
    display: flex;
    justify-content: flex-start;
    padding: 50px;
}

.flexbox-row {
    display: flex;
    justify-content: flex-start;
    align-items: center;
}

label {
    width: 220px;
    font-weight: bolder;
    display: flex;
    justify-content: flex-start;
}

.button-update {
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

.button-remove {
    background-color: rgb(255, 0, 0);
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

.field {
    height: 2rem;
    margin-top: 0.5rem;
    width: 30rem;
}

.input-errors {
    color: red;
    font-size: 10px;
    margin-left: 220px;
}

button[disabled="disabled"],
button:disabled {
    background-color: rgb(74, 75, 77);
}

.radio {
    margin-right: 50px;
}

span {
    margin-left: 100px;
}
</style>
