<template>
    <div
        v-if="villa"
        class="item-container shadow-item-clickable"
        @click="showDetails()"
    >
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
                        <div>{{ villa.pricePerDay }} $/day</div>
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
                <div class="ammenity">Wi-Fi</div>
                <div class="ammenity">Pet friendly</div>
                <button
                    v-if="isRegistered"
                    class="clickable primary transition-ease book-service"
                    @click.stop="openBookServiceDialog"
                >
                    Book
                </button>
            </div>
        </div>
    </div>
</template>

<script>
import fetchImageBackground from "../mixins/fetch-image-bg.js";
import roleValidator from "../mixins/role-validator.js";
import vnodeInSwal from "../mixins/vnode-in-swal.js";
import swalCommons from "../mixins/swal-commons.js";
import StarRating from "vue-star-rating";
import BookVilla from "./BookVilla.ce.vue";
import generalService from "../services/general-service.js";
import { getId } from "../utils/local-storage-util.js";

export default {
    name: "VillaExpoItem",
    components: {
        StarRating,
    },
    mixins: [fetchImageBackground, roleValidator, vnodeInSwal, swalCommons],
    props: {
        villa: {
            type: Object,
            required: true,
        },
        fromDate: {
            type: Date,
            required: true,
        },
        toDate: {
            type: Date,
            required: true,
        },
    },

    methods: {
        showDetails() {
            localStorage.setItem("villa", JSON.stringify(this.villa));
            this.$router.push({ name: "VillaExpoDetails" });
        },
        openBookServiceDialog() {
            this.showComponent(
                BookVilla,
                {
                    villa: this.villa,
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
