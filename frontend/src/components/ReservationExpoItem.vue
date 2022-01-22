<template>
    <div v-if="service" class="item-container shadow-item-clickable">
        <div class="img-container">
            <div class="img" :style="getImageStyle(service.imageIds[0])">
                <div v-if="reservation.isPromo" class="promo-on-img">Promo</div>
            </div>
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
                        <div v-if="reservation.isPromo">
                            <span class="reservationPricePerDay">{{
                                service.pricePerDay
                            }}</span>
                            <span class="promoPricePerDay">{{
                                reservation.price
                            }}</span>
                            $/day
                            <span class="promoPricePerDay">
                                {{ sale }}% off
                            </span>
                        </div>
                        <div v-else>
                            {{ reservation.price }}
                            $/day
                        </div>
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
                    v-for="i in addedBenefitsEntries.length <= 4
                        ? addedBenefitsEntries.length
                        : 4"
                    :key="i"
                >
                    {{ addedBenefitsEntries[i - 1][0] }}
                </div>

                <div class="promo-expo-item-right">
                    <div class="date-picker-container">
                        <div class="calendar-icon">
                            <font-awesome-icon
                                icon="calendar-check"
                            ></font-awesome-icon>
                        </div>
                        <Datepicker
                            class="date-picker"
                            v-model="fromToDate"
                            readonly
                            range
                            placeholder="Select a date range"
                            :enableTimePicker="false"
                            inputClassName="date-picker-input"
                            hideInputIcon
                        />
                    </div>
                    <div
                        v-if="history && !reservation.isCanceled"
                        class="history-buttons"
                    >
                        <button
                            v-if="!service.isReviewedByUser"
                            class="
                                clickable
                                primary
                                transition-ease
                                history-button
                            "
                            @click="openNewClientMarkDialog"
                        >
                            Review
                        </button>

                        <button
                            v-if="!service.isIssuedByUser"
                            class="
                                clickable
                                danger
                                transition-ease
                                history-button
                            "
                            @click="openNewClientIssueDialog"
                        >
                            Issue
                        </button>
                    </div>
                    <button
                        v-if="!history && !reservation.isCanceled"
                        class="clickable danger transition-ease"
                        @click.stop="CancelReservation"
                    >
                        Cancel
                    </button>
                    <div v-if="reservation.isCanceled" class="promoPricePerDay">
                        Canceled
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import fetchImageBackground from "../mixins/fetch-image-bg.js";
import roleValidator from "../mixins/role-validator.js";
import swalCommons from "../mixins/swal-commons.js";
import equipmentPicker from "../mixins/equipment-picker.js";
import vnodeInSwal from "../mixins/vnode-in-swal.js";

import StarRating from "vue-star-rating";
import generalService from "../services/general-service.js";
import Datepicker from "vue3-date-time-picker";

import NewClientMark from "./NewClientMark.ce.vue";
import NewClientIssue from "./NewClientIssue.ce.vue";

export default {
    components: {
        StarRating,
        Datepicker,
    },
    mixins: [
        fetchImageBackground,
        roleValidator,
        swalCommons,
        equipmentPicker,
        vnodeInSwal,
    ],
    props: {
        service: {
            type: Object,
            required: true,
        },
        reservation: {
            type: Object,
            required: true,
        },
        history: {
            type: Boolean,
            default: false,
        },
    },
    data() {
        return {
            fromToDate: [
                this.reservation.startDateTime,
                this.reservation.endDateTime,
            ],
        };
    },

    methods: {
        async CancelReservation() {
            try {
                const reservationId = this.reservation.reservationId;
                console.log(reservationId);
                await generalService.CancelClientReservation(reservationId);
                // document.location.reload();
                this.toast.fire({
                    icon: "success",
                    title: "You reservation is canceled!",
                });
            } catch (error) {
                this.toast.fire({
                    icon: "error",
                    title: "You reservation was not canceled!",
                });
            }
        },

        openNewClientMarkDialog() {
            this.showComponent(
                NewClientMark,
                {
                    service: this.service,
                    reservation: this.reservation,
                },
                this.sendSetupObject,
                (componentRes, sawlRes) => {
                    if (!sawlRes.isConfirmed) return;
                    if (!this.isMarkValid(componentRes)) {
                        this.toast.fire({
                            icon: "warning",
                            title: "All fields must be filled in order to send your review",
                        });
                        return;
                    }
                    this.handleMarkResult(componentRes);
                }
            );
        },

        async handleMarkResult(mark) {
            console.log(mark);

            try {
                await generalService.sendReview(mark);
                this.toast.fire({
                    icon: "success",
                    title: "Your review was handed in.",
                });
            } catch (error) {
                this.toast.fire({
                    icon: "error",
                    title: "Your review was not handed in",
                });
            }
            document.location.reload();
        },

        openNewClientIssueDialog() {
            this.showComponent(
                NewClientIssue,
                {
                    service: this.service,
                    reservation: this.reservation,
                },
                this.sendSetupObject,
                (componentRes, sawlRes) => {
                    if (!sawlRes.isConfirmed) return;
                    if (!this.isIssueValid(componentRes)) {
                        this.toast.fire({
                            icon: "warning",
                            title: "All fields must be filled in order to send your review",
                        });
                        return;
                    }
                    this.handleIssueResult(componentRes);
                }
            );
        },

        async handleIssueResult(issue) {
            console.log(issue);
            try {
                await generalService.sendIssue(issue);
                this.toast.fire({
                    icon: "success",
                    title: "Your issue was handed in.",
                });
            } catch (error) {
                this.toast.fire({
                    icon: "error",
                    title: "Your issue was not handed in",
                });
            }
            // document.location.reload();
        },

        isMarkValid(mark) {
            return mark.description && mark.givenMark > 0 && mark.givenMark < 5;
        },
        isIssueValid(issue) {
            return issue.reason;
        },
    },
    created() {
        this.parseEquipment(
            this.reservation.additionalEquipment ||
                "stavka1:0;stavka2:5;stavka3:10;stavka4:0;stavka5:10;stavka6:5;stavka7:10;stavka8:10;stavka9:10;stavka10:5;stavka11:10;stavka12:10"
        );
        console.log(this.equipment);
    },
    computed: {
        sale() {
            return (
                100 *
                (1 - this.reservation.price / this.service.pricePerDay).toFixed(
                    2
                )
            );
        },
    },
};
</script>

<style scoped>
.promo-expo-item-right {
    margin-left: auto;
    min-width: 100px;
    width: 50%;
    display: flex;
    justify-content: flex-end;
    align-items: center;
}

.date-picker-container {
    width: 60%;
}

.calendar-icon {
    margin-top: 0.5em;
}

.servicePricePerDay {
    text-decoration: line-through;
}

.promoPricePerDay {
    color: var(--danger);
    margin-left: 15px;
    font-weight: bold;
}

.history-buttons {
    display: flex;
    justify-content: space-between;
    width: 33%;
}
</style>
