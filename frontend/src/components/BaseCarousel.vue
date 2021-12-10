<template>
    <div class="container" v-if="imageIds.length">
        <carousel :items-to-show="1.0">
            <slide v-for="(id, index) in imageIds" :key="index">
                <div class="img-container">
                    <div class="img" :style="getImageStyle(id)"></div>
                </div>
            </slide>

            <template #addons>
                <navigation />
                <pagination />
            </template>
        </carousel>
    </div>
</template>

<script>
import "vue3-carousel/dist/carousel.css";
import { Carousel, Slide, Navigation, Pagination } from "vue3-carousel";
import fetchImageBackground from "../mixins/fetch-image-bg.js";
export default {
    name: "BaseCarousel",
    mixins: [fetchImageBackground],
    components: {
        Carousel,
        Slide,
        Navigation,
        Pagination,
    },
    props: {
        imageIds: {
            type: Array,
            required: true,
        },
    },
};
</script>

<style scoped>
.container {
    min-width: 100%;
    display: flex;
    justify-content: center;
}

.carousel {
    min-width: 80%;
    min-height: 60vh;
    --vc-nav-width: 70px;
    --vc-nav-color: #333;
    --vc-nav-background-color: transparent;
    --vc-pgn-margin: 5px;
    --vc-pgn-height: 10px;
    --vc-pgn-width: 8px;
    --vc-pgn-background-color: rgba(0, 0, 0, 0.3);
    --vc-pgn-active-color: #ada;
}

.img-container {
    background-color: #aca;
    min-width: 75%;
    border-radius: 5px;
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.6);
    margin-top: 25px;
    margin-bottom: 20px;
    transition: all 0.2s ease-in;
}

.img-container:hover {
    box-shadow: 0 8px 12px 0 rgba(59, 54, 54, 0.8);
    cursor: grab;
}

.img-container:active {
    cursor: grabbing;
}

.img {
    margin: 10px;
    min-height: 550px;
    max-height: 550px;
    border-radius: 5px;
}

.carousel__pagination {
    margin-right: 4vw;
}
</style>
