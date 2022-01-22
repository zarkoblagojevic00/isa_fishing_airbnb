<template>
    <div class="book-container">
        <div class="img-container">
            <div
                class="img img-title-container"
                :style="getImageStyle(boat.imageIds[0])"
            >
                <div class="img-primary-content">
                    <div class="img-title-address">
                        <div class="img-service-name">{{ boat.name }}</div>
                        <div class="img-service-address">
                            <font-awesome-icon
                                :icon="faMapMarkerAlt"
                            ></font-awesome-icon>
                            {{ boat.address }}, {{ boat.cityName }}
                        </div>
                    </div>
                    <div class="img-price-per-day">
                        {{ boat.pricePerDay }} $/day
                    </div>
                </div>
            </div>
            <div class="book-form-container">
                <div class="book-title">
                    <div>Your reservation</div>
                    <div class="date-picker-container">
                        <div class="calendar-icon">
                            <font-awesome-icon
                                :icon="faCalendarCheck"
                            ></font-awesome-icon>
                        </div>
                        <Datepicker
                            class="date-picker"
                            v-model="fromToDate"
                            readonly
                            range
                            placeholder="Select a date range"
                            :enableTimePicker="false"
                            inputClassName="date-picker-input"
                            hideInputIcon
                        />
                    </div>
                </div>

                <div class="equip-cost">
                    <div class="choose-captain">
                        <div class="choose-captain-left">
                            Do you want to be accompanied by:
                        </div>
                        <div class="choose-captain-right">
                            <div>
                                <input
                                    type="radio"
                                    :value="true"
                                    v-model="isCaptain"
                                />
                                <label for="one">Captain</label>
                            </div>
                            <div>
                                <input
                                    type="radio"
                                    :value="false"
                                    v-model="isCaptain"
                                />
                                <label for="two">First Assistant</label>
                            </div>
                        </div>
                    </div>
                    <div class="equip-container vertical-scroll-no-bar">
                        <div class="left-container">
                            <div>
                                <span class="input-label"
                                    >Pick additional equipment</span
                                >
                                <div
                                    class="equip-item shadow-item"
                                    v-for="(
                                        value, name, index
                                    ) in notChosenEquipment"
                                    :key="index"
                                >
                                    {{ name }}
                                    <span class="equip-item-right-end">
                                        <span class="money">
                                            {{ value.price }} $/day</span
                                        >
                                        <button
                                            @click="addEquipment(name)"
                                            class="
                                                equip-button
                                                clickable
                                                primary-outline
                                                transition-ease
                                            "
                                        >
                                            <font-awesome-icon
                                                :icon="faPlus"
                                            ></font-awesome-icon>
                                        </button>
                                    </span>
                                </div>
                            </div>
                        </div>
                        <div class="right-container">
                            <div>
                                <span class="input-label"
                                    >Chosen equipment</span
                                >
                                <div
                                    class="equip-item shadow-item"
                                    v-for="(
                                        value, name, index
                                    ) in chosenEquipment"
                                    :key="index"
                                >
                                    {{ name }}
                                    <span
                                        v-if="value.isAdditional"
                                        class="equip-item-right-end"
                                    >
                                        <span class="money">
                                            {{ value.price }} $/day</span
                                        >
                                        <button
                                            @click="removeEquipment(name)"
                                            class="
                                                equip-button
                                                clickable
                                                danger-outline
                                                transition-ease
                                            "
                                        >
                                            <font-awesome-icon
                                                :icon="faMinus"
                                            ></font-awesome-icon>
                                        </button>
                                    </span>
                                    <span v-else class="money"
                                        >Base equipment
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="final-cost">
                    <div class="final-cost-label">Final cost</div>
                    <div class="cost">
                        {{ numOfDays }} days *
                        {{ finalCostPerDay + this.boat.pricePerDay }} $/day =
                        {{ totalFinalCost }} $
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import fetchImageBackground from "../mixins/fetch-image-bg.js";
import Datepicker from "vue3-date-time-picker";
import fawsInShadowRoot from "../mixins/faws-in-shadowroot.js";
import equipmentPicker from "../mixins/equipment-picker.js";
import moment from "moment";

import {
    faPlus,
    faMinus,
    faCalendarCheck,
    faMapMarkerAlt,
} from "@fortawesome/free-solid-svg-icons";

export default {
    props: {
        boat: {
            type: Object,
            requred: true,
        },
        fromDate: {
            type: Date,
            required: true,
        },
        toDate: {
            type: Date,
            required: true,
        },
        userId: {
            type: Number,
            required: true,
        },
        promoId: {
            type: Number,
            default: -1,
        },
    },
    mixins: [fetchImageBackground, fawsInShadowRoot, equipmentPicker],

    components: {
        Datepicker,
    },

    data() {
        return {
            fromToDate: [this.fromDate, this.toDate],
            numOfDays: moment(this.toDate)
                .endOf("day")
                .diff(moment(this.fromDate), "days"),
            isCaptain: true,
        };
    },

    created() {
        this.parseEquipment(
            this.boat.additionalEquipment ||
                "stavka1:0;stavka2:5;stavka3:10;stavka4:0;stavka5:10;stavka6:5;stavka7:10;stavka8:10;stavka9:10;stavka10:5;stavka11:10;stavka12:10"
        );
        console.log(this.equipment);
    },

    computed: {
        faPlus() {
            return faPlus;
        },

        faMinus() {
            return faMinus;
        },

        faCalendarCheck() {
            return faCalendarCheck;
        },

        faMapMarkerAlt() {
            return faMapMarkerAlt;
        },

        totalFinalCost() {
            return (
                this.numOfDays * (this.finalCostPerDay + this.boat.pricePerDay)
            );
        },
    },

    methods: {
        getResult() {
            const additionalEquipment = this.chosenEquipmentToString();
            const price = this.finalCostPerDay + this.boat.pricePerDay;
            return {
                userId: this.userId,
                serviceId: this.boat.boatId,
                additionalEquipment,
                price,
                startDateTime: this.fromDate,
                endDateTime: this.toDate,
                promoId: this.promoId,
                isCaptain: this.isCaptain,
            };
        },
    },
};
</script>
<style src="../styles/form.css"></style>
<style src="../styles/book.css"></style>
<style>
.equip-cost {
    min-height: 35vh;
    max-height: 32vh;
}

.choose-captain {
    display: flex;
    justify-content: space-between;
    font-size: 0.95rem;
    margin-bottom: 0.5em;
}

.choose-captain-right {
    width: 35%;
    display: flex;
    justify-content: space-between;
}
</style>
