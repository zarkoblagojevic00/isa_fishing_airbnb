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
                            {{ villa.cityName }}
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
            </div>
        </div>
    </div>
</template>

<script>
import fetchImageBackground from "../mixins/fetch-image-bg.js";
import StarRating from "vue-star-rating";
export default {
    name: "VillaExpoItem",
    components: {
        StarRating,
    },
    mixins: [fetchImageBackground],
    props: {
        villa: {
            type: Object,
            required: true,
        },
    },
    methods: {
        showDetails() {
            localStorage.setItem("villa", JSON.stringify(this.villa));
            this.$router.push({ name: "VillaExpoDetails" });
        },
    },
};
</script>

<style src="../styles/form.css"></style>
<style src="../styles/expo.css"></style>
