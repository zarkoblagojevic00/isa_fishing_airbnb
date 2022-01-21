<template>
    <Navbar :baseUrl="baseUrlInstructor" :navbarItems="navbarItems" />
    <div class="flexbox-container-space-between">
        <h1>New adventure</h1>
    </div>
    <div class="flexbox-container-space-between">
        <form class="form">
            <input
                type="text"
                class="field"
                placeholder="Adventure name"
                v-model="v$.newAdventure.name.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.name.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
            <input
                type="text"
                class="field"
                placeholder="Address"
                v-model="v$.newAdventure.address.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.address.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
            <input
                type="number"
                class="field"
                placeholder="Longitude"
                v-model="v$.newAdventure.longitude.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.longitude.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
            <input
                type="number"
                class="field"
                placeholder="Latitude"
                v-model="v$.newAdventure.latitude.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.latitude.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
            <input
                type="text"
                class="field"
                placeholder="Promo description"
                v-model="v$.newAdventure.promoDescription.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.promoDescription
                    .$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
            <input
                type="number"
                class="field"
                placeholder="Price per day"
                v-model="v$.newAdventure.pricePerDay.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.pricePerDay.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
            <input
                type="number"
                class="field"
                placeholder="Maximum number of people"
                v-model="v$.newAdventure.capacity.$model"
            />
            <div
                class="input-errors"
                v-for="(error, index) of v$.newAdventure.capacity.$errors"
                :key="index"
            >
                <div class="error-msg">{{ error.$message }}</div>
            </div>
            <input
                type="text"
                class="field"
                placeholder="Terms of use"
                v-model="newAdventure.termsOfUse"
            />

            <input
                type="text"
                class="field"
                placeholder="Additional offers"
                v-model="newAdventure.additionalOffers"
            />
            <fieldset
                id="group1"
                style="margin-top: 10px; font-size: 14px; width: 20rem"
            >
                <div>Is percentage taken upon cancellation</div>
                <label for="group1"> Yes </label>
                <input
                    type="radio"
                    value="value1"
                    name="group1"
                    @click="
                        newAdventure.isPercentageTakenFromCanceledReservations = true
                    "
                />
                <label for="group2"> No </label>
                <input
                    type="radio"
                    value="value2"
                    name="group1"
                    @click="noClicked"
                />
            </fieldset>
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
    <button @click="onSubmit" :disabled="v$.newAdventure.$invalid">
        Create adventure
    </button>
</template>

<script>
import Navbar from "@/components/Navbar.vue";
import { useVuelidate } from "@vuelidate/core";
import { required, minValue, maxValue } from "@vuelidate/validators";
import axios from "../api/api.js";

export default {
    name: "New adventure",
    components: {
        Navbar,
    },
    setup() {
        return { v$: useVuelidate() };
    },
    mounted() {},
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
            newAdventure: {
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
                availableFrom: new Date(),
                availableTo: new Date(),
            },
            additionalEquipmentArray: [],

            newTagName: "",
            newTagPrice: "",
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
                pricePerDay: { required, min: minValue(0) },
                capacity: { required, min: minValue(0) },
                percentageToTake: { min: minValue(0), max: maxValue(100) },
            },
        };
    },
    methods: {
        noClicked() {
            this.isPercentageTakenFromCanceledReservations = false;
            this.percentageToTake = 0;
        },
        onSubmit() {
            let eq = this.MergeAdditioanlEquipment();

            console.log(eq);
            this.newAdventure.additionalEquipment = eq;
            const adventure = Object.assign({}, this.newAdventure);
            console.log(adventure);

            axios
                .post("/api/Adventure/AddAdventure", adventure)
                .then(() => {
                    this.$router.push(this.baseUrlInstructor + "services");
                })
                .catch((error) => {
                    if (error.response) {
                        if (
                            error.response.data.Name[0] ==
                            "The service with that name for this user already exists!"
                        ) {
                            this.$swal.fire(
                                "You already have service with this name!"
                            );
                        }
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
        MergeAdditioanlEquipment() {
            let ret = "";
            for (let eq of this.additionalEquipmentArray) {
                ret += eq.name + ":" + eq.price + ";";
            }
            return ret;
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
    align-items: flex-start;
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

.field {
    height: 2rem;
    margin-top: 0.5rem;
    width: 30rem;
}

.input-errors {
    color: red;
    font-size: 10px;
}

button[disabled="disabled"],
button:disabled {
    background-color: rgb(74, 75, 77);
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
</style>
