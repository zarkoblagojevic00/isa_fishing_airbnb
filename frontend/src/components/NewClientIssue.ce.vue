<template>
    <div class="book-container">
        <div class="img-container">
            <div
                class="img img-title-container"
                :style="getImageStyle(service.imageIds[0])"
            >
                <div class="img-primary-content">
                    <div class="img-title-address">
                        <div class="img-service-name">{{ service.name }}</div>
                        <div class="img-service-address">
                            <font-awesome-icon
                                :icon="faMapMarkerAlt"
                            ></font-awesome-icon>
                            {{ service.address }}, {{ service.cityName }}
                        </div>
                    </div>
                    <div class="img-price-per-day">
                        {{ service.pricePerDay }} $/day
                    </div>
                </div>
            </div>
            <div class="book-form-container">
                <div class="book-title">
                    <div>We are grateful for your feedback!</div>
                </div>

                <div class="input-mark-form">
                    <div class="input-comment">
                        <div class="mark-label">Enter your issue</div>
                        <textarea
                            class="input-comment-textarea"
                            v-model="newIssue.reason"
                            placeholder="Enter your description"
                        ></textarea>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import fetchImageBackground from "../mixins/fetch-image-bg.js";
import fawsInShadowRoot from "../mixins/faws-in-shadowroot.js";
import { faMapMarkerAlt } from "@fortawesome/free-solid-svg-icons";

export default {
    props: {
        service: {
            type: Object,
            requred: true,
        },
        reservation: {
            type: Object,
            requred: true,
        },
    },
    mixins: [fetchImageBackground, fawsInShadowRoot],

    data() {
        return {
            newIssue: {
                issuedEntityId: this.reservation.serviceId,
                reason: "",
            },
        };
    },

    methods: {
        getResult() {
            return this.newIssue;
        },
    },

    computed: {
        faMapMarkerAlt() {
            return faMapMarkerAlt;
        },
    },
};
</script>
<style src="../styles/form.css"></style>
<style src="../styles/book.css"></style>
<style>
.mark-label {
    font-size: 2rem;
    margin-bottom: 0.25em;
    margin-top: 0.25em;
}

.input-comment-textarea {
    width: 500px;
    height: 200px;
    font-size: 1.1rem;
    font-family: Verdana, Geneva, Tahoma, sans-serif;
    padding: 0.5em;
}
</style>
