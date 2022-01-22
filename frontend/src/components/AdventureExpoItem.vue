<template>
    <div
        v-if="adventure"
        class="item-container shadow-item-clickable"
        @click="showDetails()"
    >
        <div class="img-container">
            <div
                class="img"
                :style="getImageStyle(adventure.imageIds[0])"
            ></div>
        </div>
        <div class="expo-container">
            <div>
                <div class="expo-container-header">
                    <div class="expo-container-name">{{ adventure.name }}</div>
                    <div class="expo-container-primary-info">
                        <star-rating
                            v-if="adventure.averageMark"
                            :rating="adventure.averageMark"
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
                        <div>{{ adventure.pricePerDay }} $/hour</div>
                        <div>
                            <font-awesome-icon icon="map-marker-alt" />
                            {{ adventure.address }}, {{ adventure.cityName }}
                        </div>
                    </div>
                </div>
                <hr />
            </div>
            <p class="description">
                {{ adventure.promoDescription }}
            </p>
            <div class="ammenity-container">
                <div
                    class="ammenity"
                    v-for="i in baseEquipmentEntries.length <= 3
                        ? baseEquipmentEntries.length
                        : 3"
                    :key="i"
                >
                    {{ baseEquipmentEntries[i - 1][0] }}
                </div>
                <div
                    class="ammenity"
                    v-for="i in additionalEquipmentEntries.length <= 2
                        ? additionalEquipmentEntries.length
                        : 2"
                    :key="i"
                >
                    {{ additionalEquipmentEntries[i - 1][0] }}
                </div>
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
import BookAdventure from "./BookAdventure.ce.vue";
import generalService from "../services/general-service.js";
import { getId } from "../utils/local-storage-util.js";
import equipmentPicker from "../mixins/equipment-picker.js";
import stateDateLoader from "../mixins/state-date-loader.js";
export default {
    name: "AdventureExpoItem",
    components: {
        StarRating,
    },
    mixins: [
        fetchImageBackground,
        roleValidator,
        vnodeInSwal,
        swalCommons,
        equipmentPicker,
        stateDateLoader,
    ],
    props: {
        adventure: {
            type: Object,
            required: true,
        },
    },
    created() {
        this.parseEquipment(
            // TO DO Should be - this.adventure.additionalEquipment
            undefined ||
                "stavka1:0;stavka2:5;stavka3:10;stavka4:0;stavka5:10;stavka6:5;stavka7:10;stavka8:10;stavka9:10;stavka10:5;stavka11:10;stavka12:10"
        );
        console.log(this.equipment);
    },

    methods: {
        showDetails() {
            localStorage.setItem("adventure", JSON.stringify(this.adventure));
            const id = this.adventure.adventureId;
            this.$router.push(`/adventure/${id}/home`);
        },
        openBookServiceDialog() {
            this.showComponent(
                BookAdventure,
                {
                    adventure: this.adventure,
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
