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
                <label> City </label>
                <div>
                    <input
                        type="text"
                        class="field"
                        placeholder="City"
                        v-model="v$.newAdventure.cityName.$model"
                        list="cities"
                    />
                    <datalist id="cities">
                        <option
                            v-for="city in cities"
                            :key="city.id"
                            :value="city.name"
                        />
                    </datalist>
                    <div
                        class="input-errors"
                        v-for="(error, index) of v$.newAdventure.cityName
                            .$errors"
                        :key="index"
                    >
                        <div class="error-msg">{{ error.$message }}</div>
                    </div>
                </div>
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
        <div class="input-wrapper">
            <span class="label">Additional equipment:</span>
            <div class="horizontal">
                <div class="horizontal-part left">
                    <input
                        type="text"
                        placeholder="Name:"
                        class="equipment-input"
                        v-model="newTagName"
                    />
                </div>
                <div class="horizontal-part">
                    <input
                        type="number"
                        placeholder="Price:"
                        class="equipment-input"
                        v-model="newTagPrice"
                    />
                </div>
            </div>
            <button class="add-equipment" @click="AddTag">Add</button>
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
    <div class="flexbox-row">
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
        this.getCities();
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
                cityName: "",
                promoDescription: "",
                pricePerDay: "",
                capacity: "",
                termsOfUse: "",
                additionalEquipment: "",
                additionalOffers: "",
                isPercentageTakenFromCanceledReservations: false,
                percentageToTake: 0,
            },
            additionalEquipmentArray: [],

            newTagName: "",
            newTagPrice: "",
            cities: [],
        };
    },
    validations() {
        return {
            newAdventure: {
                name: { required },
                address: { required },
                longitude: {
                    required,
                    min: minValue(-180),
                    max: maxValue(180),
                },
                latitude: { required, min: minValue(-90), max: maxValue(90) },
                promoDescription: { required },
                cityName: { required },
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
            let eq = this.MergeAdditioanlEquipment();

            console.log(eq);
            this.newAdventure.additionalEquipment = eq;
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
                    this.$router.push(this.baseUrlInstructor + "home");
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
                    this.ParseAdditionalEquipment(
                        this.newAdventure.additionalEquipment
                    );
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
        MergeAdditioanlEquipment() {
            let ret = "";
            for (let eq of this.additionalEquipmentArray) {
                ret += eq.name + ":" + eq.price + ";";
            }
            return ret;
        },
        getCities() {
            axios.get("/api/City/GetCities").then((res) => {
                this.cities = res.data;
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
