<template>
    <h1>{{ boat.name }}</h1>
    <BaseCarousel :imageIds="boat.imageIds"> </BaseCarousel>

    <div class="info-container">
        <div class="info">
            <h2>Information</h2>
            <div class="mi-info">
                <div class="villaname">
                    <label for="villaname">Name</label>
                    <input type="text" readonly :value="boat.name" />
                </div>
                <div class="avgmark">
                    <label for="villamark">Average Mark</label>
                    <div class="stars">
                        <star-rating
                            v-if="boat.averageMark"
                            :rating="boat.averageMark"
                            :increment="0.1"
                            :max-rating="5"
                            :star-size="20"
                            inactive-color="#555"
                            active-color="#ada"
                            read-only
                            :show-rating="true"
                        >
                        </star-rating>
                        <div v-else class="expo-details-not-reviewed">
                            Not reviewed
                        </div>
                    </div>
                </div>
                <div class="price">
                    <label for="villaprice">Price per day ($)</label>
                    <input type="text" readonly :value="boat.pricePerDay" />
                </div>
            </div>
            <div class="loc-info">
                <label for="location">Location</label>
                <input
                    type="text"
                    readonly
                    :value="`${boat.address}, Novi Sad, Serbia ${boat.latitude} ${boat.longitude}`"
                />
                <div class="loc-info-map">
                    <DisplayMap
                        :lon="boat.longitude"
                        :lat="boat.latitude"
                    ></DisplayMap>
                </div>
            </div>
            <div class="desc-info">
                <label for="description">Description</label>
                <textarea v-model="boat.promoDescription" />
            </div>
            <div class="ammen-flex">
                <div class="ammen-flex-comp">
                    <div class="ammen-flex-comp-title">Other info</div>
                    <div class="ammen-equip-container vertical-scroll-no-bar">
                        <div class="equip-item shadow-item">
                            <div class="ammen-equip-left">Type</div>
                            <div class="ammen-equip-right">
                                {{ boatType }}
                            </div>
                        </div>
                        <div class="equip-item shadow-item">
                            <div class="ammen-equip-left">Length</div>
                            <div class="ammen-equip-right">
                                {{ boat.length }} m
                            </div>
                        </div>
                        <div class="equip-item shadow-item">
                            <div class="ammen-equip-left">
                                Number of engines
                            </div>
                            <div class="ammen-equip-right">
                                {{ boat.engineNum }}
                            </div>
                        </div>
                        <div class="equip-item shadow-item">
                            <div class="ammen-equip-left">Engine power</div>
                            <div class="ammen-equip-right">
                                {{ boat.enginePower }} kW
                            </div>
                        </div>
                        <div class="equip-item shadow-item">
                            <div class="ammen-equip-left">Capacity</div>
                            <div class="ammen-equip-right">
                                {{ boat.capacity }}
                            </div>
                        </div>
                    </div>
                </div>
                <div class="ammen-flex-comp">
                    <div class="ammen-flex-comp-title">Base ammenities</div>
                    <div class="ammen-equip-container vertical-scroll-no-bar">
                        <div
                            class="equip-item shadow-item"
                            v-for="(tool, index) in boat.navigationToolsObj"
                            :key="index"
                        >
                            <div class="ammen-equip-left">{{ tool.name }}</div>
                            <div class="ammen-equip-right tool-description">
                                {{ tool.description }}
                            </div>
                        </div>
                        <div
                            class="equip-item shadow-item"
                            v-for="(value, name, index) in baseEquipment"
                            :key="index"
                        >
                            {{ name }}
                        </div>
                    </div>
                </div>
                <div class="ammen-flex-comp">
                    <div class="ammen-flex-comp-title">Paid ammenities</div>
                    <div class="ammen-equip-container vertical-scroll-no-bar">
                        <div
                            class="equip-item shadow-item"
                            v-for="(value, name, index) in additionalEquipment"
                            :key="index"
                        >
                            {{ name }}
                            <span class="equip-item-right-end">
                                <span class="money">
                                    {{ value.price }} $/day</span
                                >
                            </span>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <button
        v-if="isRegistered"
        class="clickable primary transition-ease book-service-details"
        @click.stop="openBookServiceDialog"
    >
        Book
    </button>
</template>

<script>
import BaseCarousel from "../components/BaseCarousel.vue";
import StarRating from "vue-star-rating";
import DisplayMap from "../components/DisplayMap.vue";
import equipmentPicker from "../mixins/equipment-picker.js";
import generalService from "../services/general-service.js";
import BookBoat from "../components/BookBoat.ce.vue";
import { getId } from "../utils/local-storage-util.js";
import roleValidator from "../mixins/role-validator.js";
import vnodeInSwal from "../mixins/vnode-in-swal.js";
import swalCommons from "../mixins/swal-commons.js";
import stateDateLoader from "../mixins/state-date-loader.js";

export default {
    name: "BoatExpoDetails",
    components: {
        BaseCarousel,
        StarRating,
        DisplayMap,
    },
    mixins: [
        equipmentPicker,
        vnodeInSwal,
        swalCommons,
        roleValidator,
        stateDateLoader,
    ],
    data() {
        return {
            boat: JSON.parse(localStorage.getItem("boat")),
        };
    },

    computed: {
        boatType() {
            return {
                0: "Bass",
                1: "Cabin",
                2: "Vessel",
            }[this.boat.boatType];
        },
    },

    created() {
        this.parseEquipment(
            this.boat.additionalEquipment ||
                "stavka1:0;stavka2:5;stavka3:10;stavka4:0;stavka5:10;stavka6:5;stavka7:10;stavka8:10;stavka9:10;stavka10:5;stavka11:10;stavka12:10"
        );
        console.log(this.equipment);
    },
    unmounted() {
        localStorage.removeItem("boat");
    },
    methods: {
        openBookServiceDialog() {
            this.showComponent(
                BookBoat,
                {
                    boat: this.boat,
                    fromDate: this.fromDate,
                    toDate: this.toDate,
                    userId: getId(),
                },
                this.bookSetupObject,
                (componentRes, sawlRes) => {
                    if (!sawlRes.isConfirmed) return;
                    this.handleReservationResult(componentRes);
                }
            );
        },
        async handleReservationResult(componentRes) {
            this.$router.push({ name: "ClientHomePage" });
            try {
                await generalService.makeReservation(componentRes);
                this.toast.fire({
                    icon: "success",
                    title: "You reservation was successful. Check your email for reservation details!",
                });
            } catch (error) {
                this.toast.fire({
                    icon: "error",
                    title: "Service was made unavailable or was reserved before you finished reservation",
                });
            }
        },
    },
};
</script>

<style scoped>
h1 {
    color: #ada;
    margin-top: 35px;
}

h2 {
    text-align: left;
    margin: 10px 0;
}

.info-container {
    padding: 1em 0.5em;
    width: 80%;
    background: #fcfcfc;
    color: #ada;
    border-color: #ada;
    margin: 30px auto;
    border-radius: 5px;
}

.info {
    height: 130vh;
    max-width: 90%;
    margin: 10px auto;
    display: flex;
    flex-direction: column;
    justify-content: space-between;
}

.mi-info {
    min-height: 8vh;
    display: flex;
    justify-content: space-between;
    font-size: 1.2rem;
}

.mi-info div {
    width: 320px;
}

label {
    display: block;
    text-align: left;
    margin-bottom: 5px;
}

input {
    min-width: 93%;
    min-height: 46px;
    background: transparent;
    outline: none;
    border-radius: 3px;
    border: 2px solid #ada;
    color: #ada;
    font-size: 1.2rem;
    padding: 0 0.5em;
}

.mi-info .avgmark .stars {
    border-radius: 3px;
    width: 99%;
    min-height: 2.28rem;
    border: 2px solid;
    padding: 4px 0 3px 5px;
}

.expo-details-not-reviewed {
    margin-top: 0.3em;
    margin-left: 0.1em;
    display: flex;
    justify-content: flex-start;
}

.loc-info {
    min-height: 35%;
}

.loc-info label {
    font-size: 1.2rem;
}

.loc-info input {
    min-width: stretch;
    margin-bottom: 10px;
}

.loc-info-map {
    min-width: 100%;
}

.desc-info {
    min-height: 20%;
    font-size: 1.2rem;
}

.desc-info textarea {
    border: 2px solid #ada;
    border-radius: 3px;
    background: transparent;
    outline: none;
    margin: 0;
    color: #ada;
    width: stretch;
    height: 82%;
    font-size: 0.9em;
    padding: 5px 0.5em;
    font-family: Verdana, Geneva, Tahoma, sans-serif;
}

.ammen-flex {
    height: 20%;
    display: flex;
    justify-content: space-between;
}

.ammen-flex-comp {
    text-align: left;
    width: 30%;
}

.ammen-flex-comp-title {
    font-size: 1.4rem;
    padding-bottom: 0.25em;
    margin-bottom: 0.25em;
    border-bottom: 1px solid var(--control-border-color-focused);
}

.ammen-equip-container {
    max-height: 80%;
    padding: 0.5em 0.5em 0.5em 0.5em;
    color: var(--default);
}

.book-service-details {
    width: 8%;
    position: fixed;
    font-size: 1.5rem;
    bottom: 35%;
    right: 10px;
}

.tool-description {
    font-size: 0.8rem;
}
</style>
