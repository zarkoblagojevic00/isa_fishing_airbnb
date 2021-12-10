<template>
    <div class="container" @click="showDetails()">
        <div class="img-container">
            <div
                class="img"
                :style="getImageStyle(adventure.imageIds[0])"
            ></div>
        </div>
        <div class="expo-container">
            <div class="title-mark-price">
                <div>{{ adventure.name }}</div>
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

                <div>{{ adventure.pricePerDay }}$ per day</div>
                <div>
                    <font-awesome-icon icon="map-marker-alt" /> Novi Sad, Serbia
                </div>
            </div>
            <hr />
            <p class="description">
                {{ adventure.promoDescription }}
            </p>
            <div class="additional"></div>
        </div>
    </div>
</template>

<script>
import fetchImageBackground from "../mixins/fetch-image-bg.js";
import StarRating from "vue-star-rating";
export default {
    name: "AdventureExpoItem",
    components: {
        StarRating,
    },
    mixins: [fetchImageBackground],
    props: {
        adventure: {
            type: Object,
            required: true,
        },
    },
    methods: {
        showDetails() {
            // localStorage.setItem("adventure", JSON.stringify(this.adventure));
            // this.$router.push({ name: "AdventureExpoDetails" });
            const id = this.adventure.adventureId;
            this.$router.push(`/adventure/${id}`);
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
    width: 100%;
    display: flex;
    justify-content: space-between;
    align-self: flex-start;
    min-height: 20%;
    font-size: 1rem;
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
