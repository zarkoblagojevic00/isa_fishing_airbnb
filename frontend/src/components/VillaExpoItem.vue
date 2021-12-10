<template>
    <div class="container" @click="showDetails()">
        <div class="img-container">
            <div class="img" :style="getImageStyle(villa.imageIds[0])"></div>
        </div>
        <div class="expo-container">
            <div class="title-mark-price">
                <div>{{ villa.name }}</div>
                <star-rating
                    :rating="3.6"
                    :increment="0.1"
                    :max-rating="5"
                    :star-size="20"
                    inactive-color="#555"
                    active-color="#ada"
                    read-only
                    :show-rating="true"
                >
                </star-rating>

                <div>{{ villa.pricePerDay }}$ per day</div>
                <div>
                    <font-awesome-icon icon="map-marker-alt" /> Novi Sad, Serbia
                </div>
            </div>
            <hr />
            <p class="description">
                {{ villa.promoDescription }}
            </p>
            <div class="additional">
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

<style scoped>
.container {
    background-color: #fcfcfc;
    max-width: 80%;
    margin: 50px auto;
    min-height: 26vh;
    display: flex;
    justify-content: space-between;
    border-radius: 5px;
    box-shadow: 0px 4px 8px 0 rgba(0, 0, 0, 0.6);
    transition: all 0.2s ease-in;
    cursor: pointer;
    display: flex;
    justify-content: center;
    align-items: center;
}

.container:hover {
    box-shadow: 0px 8px 12px 0 rgba(40, 27, 26, 0.6);
}

.container hr {
    width: 100%;
    height: 2px;
    margin: 0;
    background-color: #aca;
    outline: none;
    border: 1px solid #aca;
    border-radius: 2px;
}

.img-container {
    width: 22%;
    display: flex;
    justify-content: center;
    align-items: center;
}

.img {
    min-height: 170px;
    min-width: 170px;
    border-radius: 5px;
}

.expo-container {
    min-height: 80%;
    width: 75%;
    padding-right: 5%;
    display: flex;
    flex-direction: column;
    justify-content: space-around;
}

.title-mark-price {
    width: 82%;
    display: flex;
    justify-content: space-between;
    align-self: flex-start;
    min-height: 20%;
    font-size: 1.3rem;
}

.avgmark {
    margin-right: -70px;
}

.description {
    min-height: 80px;
    text-align: justify;
    font-size: 0.9 rem;
    overflow: none;
}

.additional {
    display: flex;
    justify-content: flex-start;
    align-items: center;
    min-height: 25%;
    max-height: 25%;
}

.ammenity {
    display: flex;
    justify-content: center;
    align-items: center;
    margin-right: 2rem;
    padding: 0.25em;
    width: auto;
    min-width: 70px;
    height: 25px;
    border-radius: 3px;
    background: #aca;
    outline: none;
    color: #fff;
    border: none;
    font-size: 0.9rem;
}
</style>
