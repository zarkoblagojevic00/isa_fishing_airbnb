<template>
    <div v-if="villa" class="item-container shadow-item-clickable">
        <div class="img-container">
            <div class="img" :style="getImageStyle(villa.imageIds[0])"></div>
        </div>
        <div class="expo-container">
            <div>
                <div class="expo-container-header">
                    <div class="expo-container-name">{{ villa.name }}</div>
                    <div class="expo-container-primary-info">
                        <star-rating
                            v-if="villa.averageMark"
                            :rating="villa.averageMark"
                            :increment="0.1"
                            :max-rating="5"
                            :star-size="20"
                            inactive-color="#555"
                            active-color="#ada"
                            read-only
                            :show-rating="true"
                        >
                        </star-rating>
                        <div v-else class="expo-contaner-not-reviewed">
                            Not reviewed
                        </div>
                        <div>
                            <span class="villaPricePerDay">{{
                                villa.pricePerDay
                            }}</span>
                            <span class="promoPricePerDay">{{
                                promo.pricePerDay
                            }}</span>
                            $/day
                            <span class="promoPricePerDay">
                                {{ sale }}% off
                            </span>
                        </div>
                        <div>
                            <font-awesome-icon icon="map-marker-alt" />
                            {{ villa.address }}, {{ villa.cityName }}
                        </div>
                    </div>
                </div>
                <hr />
            </div>
            <p class="description">
                {{ villa.promoDescription }}
            </p>
            <div class="ammenity-container">
                <div class="ammenity">Beds: {{ villa.numberOfBeds }}</div>
                <div class="ammenity">Rooms: {{ villa.numberOfRooms }}</div>
                <div
                    class="ammenity"
                    v-for="i in addedBenefitsEntries.length <= 4
                        ? addedBenefitsEntries.length
                        : 4"
                    :key="i"
                >
                    {{ addedBenefitsEntries[i - 1][0] }}
                </div>

                <div class="promo-expo-item-right">
                    <div class="date-picker-container">
                        <div class="calendar-icon">
                            <font-awesome-icon
                                icon="calendar-check"
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
                    <button
                        v-if="isRegistered"
                        class="clickable danger transition-ease"
                        @click.stop="makeReservation"
                    >
                        Book
                    </button>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import fetchImageBackground from "../mixins/fetch-image-bg.js";
import roleValidator from "../mixins/role-validator.js";
import swalCommons from "../mixins/swal-commons.js";
import equipmentPicker from "../mixins/equipment-picker.js";

import StarRating from "vue-star-rating";
import generalService from "../services/general-service.js";
import { getId } from "../utils/local-storage-util.js";
import Datepicker from "vue3-date-time-picker";

export default {
    components: {
        StarRating,
        Datepicker,
    },
    mixins: [fetchImageBackground, roleValidator, swalCommons, equipmentPicker],
    props: {
        villa: {
            type: Object,
            required: true,
        },
        promo: {
            type: Object,
            required: true,
        },
    },
    data() {
        return {
            fromToDate: [this.promo.startDateTime, this.promo.endDateTime],
        };
    },

    methods: {
        async makeReservation() {
            const reservation = this.buildReservation();
            try {
                await generalService.makeReservation(reservation);
                this.$router.push({ name: "ClientHomePage" });
                this.toast.fire({
                    icon: "success",
                    title: "You reservation was successful. Check your email for reservation details!",
                });
            } catch (error) {
                this.toast.fire({
                    icon: "error",
                    title: "Promo action was made unavailable or was reserved before you finished reservation",
                });
            }
        },
        buildReservation() {
            return {
                userId: getId(),
                serviceId: this.villa.villaId,
                additionalEquipment: this.promo.addedBenefits,
                price: this.promo.pricePerDay,
                startDateTime: this.promo.startDateTime,
                endDateTime: this.promo.endDateTime,
                promoId: this.promo.promoActionId,
            };
        },
    },
    created() {
        this.parseEquipment(
            this.promo.addedBenefits ||
                "stavka1:0;stavka2:5;stavka3:10;stavka4:0;stavka5:10;stavka6:5;stavka7:10;stavka8:10;stavka9:10;stavka10:5;stavka11:10;stavka12:10"
        );
        console.log(this.equipment);
    },
    computed: {
        sale() {
            return (
                100 *
                (1 - this.promo.pricePerDay / this.villa.pricePerDay).toFixed(2)
            );
        },
    },
};
</script>

<style scoped>
.promo-expo-item-right {
    margin-left: auto;
    min-width: 100px;
    width: 50%;
    display: flex;
    justify-content: flex-end;
    align-items: center;
}

.date-picker-container {
    width: 60%;
}

.calendar-icon {
    margin-top: 0.5em;
}

.villaPricePerDay {
    text-decoration: line-through;
}

.promoPricePerDay {
    color: var(--danger);
    margin-left: 15px;
    font-weight: bold;
}
</style>
