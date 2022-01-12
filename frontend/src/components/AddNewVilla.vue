<template>
    <h3>{{ mode == "Adding" ? "Add new villa!" : "Edit villa!" }}</h3>
    <hr />
    <span class="info"></span>
    <div class="content">
        <div class="content-pane">
            <div class="input-wrapper">
                <span class="label">Villa name:</span>
                <input
                    class="input-field"
                    type="text"
                    v-model="name"
                    :class="[ValidateString(name) ? '' : 'error-outline']"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Price per day:</span>
                <input
                    class="input-field"
                    type="text"
                    v-model="pricePerDay"
                    v-on:keypress="isNumber($event)"
                    :class="[ValidatePrice() ? '' : 'error-outline']"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Address:</span>
                <input
                    class="input-field"
                    type="text"
                    v-model="address"
                    :class="[ValidateString(address) ? '' : 'error-outline']"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">City:</span>
                <input
                    class="input-field"
                    type="text"
                    list="cities"
                    v-model="city"
                    :class="[ValidateCity(city) ? '' : 'error-outline']"
                />
                <datalist id="cities">
                    <option
                        v-for="city in cities"
                        :key="city.id"
                        :value="city.name"
                    />
                </datalist>
            </div>
            <div class="input-wrapper">
                <span class="label">Longitude:</span>
                <input
                    class="input-field"
                    type="number"
                    min="-180"
                    max="80"
                    v-model="longitude"
                    :class="[ValidateLongitude() ? '' : 'error-outline']"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Latitude:</span>
                <input
                    class="input-field"
                    type="number"
                    min="-90"
                    max="90"
                    v-model="latitude"
                    :class="[ValidateLatitude() ? '' : 'error-outline']"
                />
                <span class="label link" @click="showMap = true">View map</span>
                <div v-if="showMap">
                    <teleport to="#mapTeleport">
                        <ServiceMap
                            :coordinates="[longitude, latitude]"
                            :updateShowMap="TurnOfMap"
                            :updateCoords="UpdateCoords"
                        />
                    </teleport>
                </div>
            </div>
            <div class="input-wrapper">
                <span class="label">Capacity:</span>
                <input
                    class="input-field"
                    type="number"
                    min="0"
                    v-model="capacity"
                    :class="[ValidateNumber(capacity) ? '' : 'error-outline']"
                />
            </div>
            <div class="horizontal-wrapper">
                <input
                    type="checkbox"
                    class="input-checkbox"
                    v-model="percentageTakenFromCancelation"
                />
                <span class="label">Charge canceled reservations?</span>
            </div>
            <div class="input-wrapper">
                <span class="label">Percentage to take:</span>
                <input
                    class="input-field"
                    type="number"
                    min="0"
                    max="100"
                    v-model="percentageToTake"
                    :disabled="percentageTakenFromCancelation == false"
                    :class="[
                        ValidatePercentage(capacity) ? '' : 'error-outline',
                    ]"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Number of rooms:</span>
                <input
                    class="input-field"
                    type="number"
                    min="0"
                    max="100"
                    v-model="numberOfRooms"
                    :class="[
                        ValidateNumber(numberOfRooms) ? '' : 'error-outline',
                    ]"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Number of beds:</span>
                <input
                    class="input-field"
                    type="number"
                    min="0"
                    max="100"
                    v-model="numberOfBeds"
                    :class="[
                        ValidateNumber(numberOfBeds) ? '' : 'error-outline',
                    ]"
                />
            </div>
        </div>
        <div class="content-pane">
            <div class="input-wrapper">
                <span class="label">Available range:</span>
                <Datepicker
                    class="date-range"
                    :modelValue="selectedDate"
                    @update:modelValue="UpdateDate"
                    :range="true"
                    :twoCalendars="true"
                    :placeholder="'Select a date range'"
                    :minDate="new Date()"
                />
            </div>
            <div class="input-wrapper">
                <span class="label">Promo description:</span>
                <textarea
                    class="input-textarea"
                    type="text"
                    v-model="promoDescription"
                    :class="[
                        ValidateString(promoDescription) ? '' : 'error-outline',
                    ]"
                ></textarea>
            </div>
            <div class="input-wrapper">
                <span class="label">Terms of use:</span>
                <textarea
                    class="input-textarea"
                    type="text"
                    v-model="termsOfUse"
                    placeholder="Not required"
                ></textarea>
            </div>
            <div class="input-wrapper">
                <span class="label">Additional equipment:</span>
                <textarea
                    class="input-textarea"
                    type="text"
                    v-model="additionalEquipment"
                    placeholder="Not required"
                ></textarea>
            </div>
            <div class="horizontal-wrapper" v-if="mode == 'Editing'">
                <button class="submit-btn" @click="changeMode('VillaImages')">
                    View images
                </button>
                <button class="delete-btn" @click="DeleteVilla()">
                    Delete villa
                </button>
            </div>
            <div class="input-wrapper" v-if="errors.length > 0">
                <span class="label red">Errors:</span>
                <ul>
                    <li class="red" v-for="error in errors" :key="error">
                        *{{ error }}
                    </li>
                </ul>
            </div>
        </div>
    </div>
    <div class="submit-div">
        <button class="submit-btn" @click="SendRequest()">
            {{ mode == "Adding" ? "Create villa!" : "Update Villa!" }}
        </button>
    </div>
</template>

<script>
import Datepicker from "vue3-date-time-picker";
import "vue3-date-time-picker/dist/main.css";
import ServiceMap from "../components/ServiceMapComponent.vue";
export default {
    name: "AndNewVilla",
    props: {
        changeMode: Function,
        villaId: Number,
    },
    components: {
        Datepicker,
        ServiceMap,
    },
    data() {
        return {
            name: "",
            pricePerDay: "",
            address: "",
            longitude: 20,
            latitude: 45,
            promoDescription: "",
            termsOfUse: "",
            additionalEquipment: "",
            availableFrom: null,
            availableTo: null,
            capacity: 0,
            percentageTakenFromCancelation: false,
            percentageToTake: 0,
            numberOfBeds: 0,
            numberOfRooms: 0,
            errors: [],
            mode: this.$props.villaId == -1 ? "Adding" : "Editing",
            imageIds: [],
            city: "",
            cities: [],
            selectedDate: ["", ""],
            showMap: false,
        };
    },
    mounted() {
        this.PullData();
        this.GetCities();
    },
    methods: {
        GetCities() {
            fetch("/api/City/GetCities")
                .then((response) => response.json())
                .then((data) => (this.cities = data));
        },
        isNumber(evt) {
            evt = evt ? evt : window.event;
            var charCode = evt.which ? evt.which : evt.keyCode;
            if (
                charCode > 31 &&
                (charCode < 48 || charCode > 57) &&
                charCode !== 46
            ) {
                evt.preventDefault();
            } else {
                return true;
            }
        },
        ValidateCity() {
            for (const c of this.cities)
                if (c.name.toLowerCase() == this.city.toLowerCase())
                    return true;
            return false;
        },
        ValidateString(str) {
            if (str == 0) return false;
            return true;
        },
        ValidatePrice() {
            if (this.pricePerDay.length == 0) return false;

            let input = parseFloat(input);
            if (input == undefined || input == null) return false;

            if (input <= 0) return false;

            return true;
        },
        ValidateLongitude() {
            if (this.longitude > 80 || this.longitude < -180) return false;

            return true;
        },
        ValidateLatitude() {
            if (this.latitude > 90 || this.latitude < -90) return false;

            return true;
        },
        ValidateNumber(num) {
            if (num < 1 || !Number.isInteger(num)) return false;

            return true;
        },
        ValidatePercentage() {
            if (!this.percentageTakenFromCancelation) return true;

            if (this.percentageToTake <= 100 && this.percentageToTake > 0)
                return true;

            return false;
        },
        UpdateDate(evt) {
            if (evt == null || evt == undefined) {
                this.selectedDate = ["", ""];
                return;
            }
            this.selectedDate = [evt[0], evt[1]];
        },
        SendRequest() {
            this.errors = new Array();

            if (
                !this.ValidateString(this.name) ||
                !this.ValidatePrice() ||
                !this.ValidateString(this.address) ||
                !this.ValidateLongitude() ||
                !this.ValidateLatitude() ||
                !this.ValidateNumber(this.capacity) ||
                !this.ValidatePercentage() ||
                !this.ValidateNumber(this.numberOfBeds) ||
                !this.ValidateNumber(this.numberOfRooms) ||
                !this.ValidateString(this.promoDescription)
            ) {
                this.errors.push(
                    "All fields marked with red outline need to be corrected!"
                );
                return false;
            }

            let dto = {
                Name: this.name,
                PricePerDay: this.pricePerDay,
                Address: this.address,
                Longitude: this.longitude,
                Latitude: this.latitude,
                PromoDescription: this.promoDescription,
                TermsOfUse: this.termsOfUse,
                AdditionalEquipment: this.additionalEquipment,
                Capacity: this.capacity,
                IsPercentageTakenFromCanceledReservations:
                    this.percentageTakenFromCancelation,
                PercentageToTake: this.percentageToTake,
                NumberOfBeds: this.numberOfBeds,
                NumberOfRooms: this.numberOfRooms,
                CityName: this.city,
            };

            if (this.selectedDate[0] != "" && this.selectedDate[1] != "") {
                dto.availableFrom = this.selectedDate[0];
                dto.availableTo = this.selectedDate[1];
            }

            let vue = this;
            let url =
                this.mode == "Adding"
                    ? "/api/VillaManagement/CreateVilla"
                    : "/api/VillaManagement/UpdateVilla";
            if (this.mode == "Editing") {
                dto.villaId = this.$props.villaId;
            }
            fetch(url, {
                method: vue.mode == "Adding" ? "POST" : "PUT",
                redirect: "follow",
                headers: {
                    "Content-type": "application/json",
                    "Set-Cookie": document.cookie,
                },
                body: JSON.stringify(dto),
            })
                .then((response) => {
                    if (response.status != 200) {
                        alert(
                            "Something went wrong!\nStatus code: " +
                                response.status
                        );
                        return response.text();
                    }
                    if (vue.mode == "Adding") {
                        alert("Success! New villa has been created!");
                    } else {
                        alert("Success! Villa has been updated!");
                    }
                    vue.changeMode("ViewVillas");
                    return "";
                })
                .then((data) => {
                    vue.errors = new Array();
                    if (data == "") {
                        return;
                    }
                    let error = "";
                    let strconst = "".constructor;
                    try {
                        error = JSON.parse(data);
                    } catch {
                        error = data;
                    }
                    if (error.constructor == strconst) {
                        vue.errors.push(error);
                    } else {
                        if (error.errors != undefined) {
                            for (let er of error.errors) {
                                vue.errors.push(er);
                            }
                        }
                    }
                });

            return true;
        },
        PullData() {
            if (this.mode == "Adding") return;

            let vue = this;
            fetch(
                "/api/VillaManagement/GetVillaInfo?villaId=" +
                    this.$props.villaId,
                {
                    method: "GET",
                    redirect: "follow",
                    headers: {
                        "Content-type": "application/json",
                        "Set-Cookie": document.cookie,
                    },
                }
            )
                .then((response) => {
                    if (response.status != 200) {
                        alert(
                            "Something went wrong!\nStatus code: " +
                                response.status
                        );
                        throw "";
                    }
                    return response.json();
                })
                .then((data) => {
                    vue.name = data.name;
                    vue.pricePerDay = data.pricePerDay;
                    vue.address = data.address;
                    vue.longitude = data.longitude;
                    vue.latitude = data.latitude;
                    vue.capacity = data.capacity;
                    vue.percentageTakenFromCancelation =
                        data.isPercentageTakenFromCanceledReservations;
                    vue.percentageToTake = data.percentageToTake;
                    vue.numberOfRooms = data.numberOfRooms;
                    vue.numberOfBeds = data.numberOfBeds;
                    vue.promoDescription = data.promoDescription;
                    vue.termsOfUse = data.termsOfUse;
                    vue.additionalEquipment = data.additionalEquipment;
                    vue.imageIds = data.imageIds;
                    vue.city = data.cityName;
                    if (
                        data.availableFrom != undefined &&
                        data.availableTo != undefined
                    ) {
                        vue.selectedDate = [
                            data.availableFrom.toString(),
                            data.availableTo.toString(),
                        ];
                    }
                });
        },
        DeleteVilla() {
            if (this.mode == "Adding") return;

            let vue = this;
            fetch(
                "/api/VillaManagement/DeleteVilla?villaId=" +
                    this.$props.villaId,
                {
                    method: "DELETE",
                    redirect: "follow",
                    headers: {
                        "Content-type": "application/json",
                        "Set-Cookie": document.cookie,
                    },
                }
            )
                .then((response) => {
                    if (response.status != 200) {
                        return response.text();
                    } else {
                        alert("Success! The villa has been deleted!");
                        vue.changeMode("ViewVillas");
                    }
                    return "";
                })
                .then((data) => {
                    if (data == undefined || data == "") {
                        return;
                    }

                    alert(data);
                    vue.errors = new Array();
                    vue.errors.push(data);
                });
        },
        UpdateCoords(lon, lat) {
            this.longitude = lon;
            this.latitude = lat;
        },
        TurnOfMap() {
            this.showMap = false;
        },
    },
};
</script>

<style scoped>
.link {
    text-decoration: underline;
    color: #345fed !important;
    margin-top: 10px;
    cursor: pointer;
}
.content {
    min-height: calc(100% - 75px);
    display: flex;
    flex-flow: row wrap;
    position: relative;
}

.content-pane {
    display: flex;
    flex-direction: column;
    height: auto;
    width: 50%;
    min-width: 145px;
    padding-left: 10px;
    padding-right: 10px;
    box-sizing: border-box;
}

.input-wrapper {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    margin-bottom: 15px;
}

.label {
    font-size: 14px;
    color: black;
    margin-bottom: 5px;
    padding-left: 10px;
}

.input-field {
    height: 50px;
    border-radius: 15px;
    outline: none;
    border: 1px solid #c3c3c3;
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    padding-left: 15px;
    padding-right: 15px;
    box-sizing: border-box;
    font-size: 15px;
}

.input-textarea {
    height: 150px;
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    border: 1px solid #c3c3c3;
    border-radius: 15px;
    padding: 15px;
    box-sizing: border-box;
    resize: none;
    font-size: 15px;
    outline: none;
}

.horizontal-wrapper {
    max-width: 400px;
    min-width: 200px;
    width: 100%;
    display: flex;
    margin-bottom: 10px;
    padding-left: 10px;
}

.input-checkbox {
    width: 15px;
    height: 15px;
    position: relative;
    top: -2px;
}

.submit-btn {
    height: 50px;
    border-radius: 15px;
    border: none;
    font-size: 20px;
    color: white;
    background-color: #345fed;
    cursor: pointer;
    font-size: 18px;
    transition: background-color 300ms linear;
}

.submit-btn:hover {
    background-color: #54cc39;
    color: black;
}

.red {
    color: red !important;
}

.delete-btn {
    background: red;
    outline: none;
    margin-left: 10px;
    border-radius: 15px;
    border: none;
    color: white;
    font-size: 18px;
    transition: background-color 300ms linear;
}

.delete-btn:hover {
    background-color: rgb(255, 184, 24);
    color: black;
    cursor: pointer;
}

li {
    display: flex;
    color: red;
    font-size: 13px;
}

.error-outline {
    border: 1px solid red !important;
}

h3 {
    display: flex;
    padding-left: 25px;
}

.info {
    display: flex;
    text-align: left;
    margin-bottom: 15px;
    padding: 0px 15px;
}

.date-range {
    max-width: 400px;
    min-width: 200px;
    width: 100%;
}
</style>
