<template>
    <div
        v-if="service"
        class="item-container shadow-item-clickable"
        @click="showDetails()"
    >
        <div class="img-container">
            <div class="img" :style="getImageStyle(service.imageIds[0])"></div>
        </div>
        <div class="expo-container">
            <div>
                <div class="expo-container-header">
                    <div class="expo-container-name">{{ service.name }}</div>
                    <div class="expo-container-primary-info">
                        <star-rating
                            v-if="service.averageMark"
                            :rating="service.averageMark"
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
                        <div>{{ service.pricePerDay }} $/day</div>
                        <div>
                            <font-awesome-icon icon="map-marker-alt" />
                            {{ service.address }}, {{ service.cityName }}
                        </div>
                    </div>
                </div>
                <hr />
            </div>
            <p class="description">
                {{ service.promoDescription }}
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
                    class="clickable danger transition-ease book-service"
                    @click.stop="unsubscribe"
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
import swalCommons from "../mixins/swal-commons.js";
import StarRating from "vue-star-rating";
import generalService from "../services/general-service.js";

export default {
    name: "SubscriptionExpoItem",
    components: {
        StarRating,
    },
    mixins: [fetchImageBackground, roleValidator, swalCommons],
    props: {
        service: {
            type: Object,
            required: true,
        },
    },

    created() {
        this.parseEquipment(
            this.service.additionalEquipment ||
                "stavka1:0;stavka2:5;stavka3:10;stavka4:0;stavka5:10;stavka6:5;stavka7:10;stavka8:10;stavka9:10;stavka10:5;stavka11:10;stavka12:10"
        );
        console.log(this.equipment);
    },

    methods: {
        showDetails() {
            localStorage.setItem("service", JSON.stringify(this.service));
            this.$router.push({ name: "VillaExpoDetails" });
        },
        async unsubcribe() {
            this.$router.push({ name: "ClientHomePage" });
            try {
                await generalService.unsubscribe(this.service.serviceId);
                this.toast.fire({
                    icon: "success",
                    title: "You unsubscribed!",
                });
            } catch (error) {
                this.toast.fire({
                    icon: "error",
                    title: "You failed to unsubscribe!",
                });
            }
        },
    },
};
</script>
