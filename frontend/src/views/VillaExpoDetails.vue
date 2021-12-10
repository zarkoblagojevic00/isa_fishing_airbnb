<template>
    <h1>{{ villa.name }}</h1>
    <BaseCarousel :imageIds="villa.imageIds"> </BaseCarousel>

    <div class="info-container">
        <div class="info">
            <h2>Information</h2>
            <div class="mi-info">
                <div class="villaname">
                    <label for="villaname">Name</label>
                    <input type="text" readonly :value="villa.name" />
                </div>
                <div class="avgmark">
                    <label for="villamark">Average Mark</label>
                    <div class="stars">
                        <star-rating
                            :rating="3.6"
                            :increment="0.1"
                            :max-rating="5"
                            :star-size="35"
                            inactive-color="#555"
                            active-color="#ada"
                            read-only
                            :show-rating="true"
                        >
                        </star-rating>
                    </div>
                </div>
                <div class="price">
                    <label for="villaprice">Price per day ($)</label>
                    <input type="text" readonly :value="villa.pricePerDay" />
                </div>
            </div>
            <div class="loc-info">
                <label for="location">Location</label>
                <input
                    type="text"
                    readonly
                    :value="`${villa.address}, Novi Sad, Serbia ${villa.latitude} ${villa.longitude}`"
                />
                <div class="loc-info-map"></div>
            </div>
            <div class="desc-info">
                <label for="description">Description</label>
                <textarea v-model="villa.promoDescription" />
            </div>
            <div class="ammen-info">
                <label>Amennities</label>
                <div class="additional">
                    <div class="ammenity">Beds: {{ villa.numberOfBeds }}</div>
                    <div class="ammenity">Rooms: {{ villa.numberOfRooms }}</div>
                    <div class="ammenity">Wi-Fi</div>
                    <div class="ammenity">Pet friendly</div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import BaseCarousel from "../components/BaseCarousel.vue";
import StarRating from "vue-star-rating";
export default {
    name: "VillaExpoDetails",
    components: {
        BaseCarousel,
        StarRating,
    },
    data() {
        return {
            villa: JSON.parse(localStorage.getItem("villa")),
        };
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
    max-width: 90%;
    min-height: 120vh;
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
    border: 2px solid;
    padding: 4px 0 3px 5px;
}

.loc-info {
    min-height: 45vh;
}

.loc-info label {
    font-size: 1.2rem;
}

.loc-info input {
    min-width: stretch;
    margin-bottom: 10px;
}

.loc-info-map {
    min-height: 350px;
    background-image: url("../assets/map.png");
    background-size: cover;
    background-position: center;
    border-radius: 5px;
}

.desc-info {
    min-height: 20vh;
}

.desc-info {
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
    height: 205px;
    font-size: 0.9em;
    padding: 5px 0.5em;
    font-family: Verdana, Geneva, Tahoma, sans-serif;
}

.ammen-info {
    min-height: 15vh;
    font-size: 1.2rem;
}

.additional {
    margin-top: 10px;
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
