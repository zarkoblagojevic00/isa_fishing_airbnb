<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="heading">
        <h1>New quick action</h1>
    </div>
    <div class="flexbox-group">
        <div class="flexbox-container">
            <div class="flexbox-row">
                <div class="item bold">Start:</div>
                <v-date-picker
                    v-model="v$.quickAction.startDateTime.$model"
                    mode="dateTime"
                    is24hr
                    :min-date="
                        v$.quickAction.endDateTime.$model
                            ? v$.quickAction.endDateTime.$model
                            : new Date()
                    "
                    :max-date="
                        v$.quickAction.endDateTime.$model
                            ? v$.quickAction.endDateTime.$model
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
                    v-for="(error, index) of v$.quickAction.startDateTime
                        .$errors"
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
                    :min-date="
                        v$.quickAction.startDateTime.$model
                            ? v$.quickAction.startDateTime.$model
                            : new Date()
                    "
                    :max-date="
                        v$.quickAction.startDateTime.$model
                            ? v$.quickAction.startDateTime.$model
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
                    v-for="(error, index) of v$.quickAction.endDateTime.$errors"
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
                    v-for="(error, index) of v$.quickAction.place.$errors"
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
import { required, minValue } from "@vuelidate/validators";

export default {
    name: "AdventureNewQuickAction",
    components: {
        Navbar,
    },
    mounted() {
        this.getCities();
        this.getAdventureById(this.$route.params.id);
    },
    methods: {
        onCancelEdit() {
            this.quickAction = {};
        },
        onSaveEdit() {
            let eq = this.MergeAdditioanlEquipment();
            this.quickAction.addedBenefits = eq;
            this.quickAction.serviceId = this.$route.params.id;
            const quickAction_obj = Object.assign({}, this.quickAction);
            if (
                new Date(this.quickAction.startDateTime) >
                new Date(this.quickAction.endDateTime)
            ) {
                this.$swal.fire("Start time must be before end time.");
            } else {
                axios
                    .post(
                        "/api/QuickAction/CreateNewQuickAction",
                        quickAction_obj
                    )
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
                    .catch((error) => {
                        if (error.response) {
                            // Request made and server responded
                            this.$swal(error.response.data);
                        }
                    });
            }
        },
        getCities() {
            axios.get("/api/City/GetCities").then((res) => {
                this.cities = res.data;
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
        MergeAdditioanlEquipment() {
            let ret = "";
            for (let eq of this.additionalEquipmentArray) {
                ret += eq.name + ":" + eq.price + ";";
            }
            return ret;
        },
        getAdventureById(adventureId) {
            axios
                .get(
                    "/api/Adventure/GetAdventureInfoById?adventureId=" +
                        adventureId
                )
                .then((res) => {
                    this.currentAdventure = res.data;
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
            cities: [],
            additionalEquipmentArray: [],

            newTagName: "",
            newTagPrice: "",
            currentAdventure: "",
        };
    },
    validations() {
        return {
            quickAction: {
                startDateTime: {
                    required,
                },
                endDateTime: { required },
                pricePerDay: { required, minValue: minValue(0) },
                capacity: { required, minValue: minValue(0) },
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
