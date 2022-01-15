<template>
    <div class="book-container">
        <div class="img-container">
            <div
                class="img img-title-container"
                :style="getImageStyle(villa.imageIds[0])"
            >
                <div class="img-title">{{ villa.name }}</div>
            </div>
            <input type="text" v-model="reservationDetails.additional" />
        </div>
    </div>
</template>

<script>
import fetchImageBackground from "../mixins/fetch-image-bg.js";

export default {
    props: {
        villa: {
            type: Object,
            requred: true,
        },
    },
    mixins: [fetchImageBackground],

    data() {
        return {
            reservationDetails: {
                price: 10,
                additional: "",
            },
        };
    },

    methods: {
        getResult() {
            return this.reservationDetails;
        },
    },
};
</script>
<style src="../styles/form.css"></style>
<style src="../styles/book.css"></style>
