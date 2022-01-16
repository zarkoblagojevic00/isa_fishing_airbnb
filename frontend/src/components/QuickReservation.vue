<template>
    <div class="container" @click="onActionBook">
        <div
            class="card-inner flex-table"
            v-for="action in promoActions"
            :key="action.promoActionId"
        >
            <div class="flex-row">
                <div class="left">Start time:</div>
                <div class="right">
                    {{ dateFormat(action.startDateTime) }}
                </div>
            </div>
            <div class="flex-row">
                <div class="left">End time:</div>
                <div class="right">
                    {{ dateFormat(action.endDateTime) }}
                </div>
            </div>
            <div class="flex-row">
                <div class="left">Price per day:</div>
                <div class="right">{{ action.pricePerDay }}</div>
            </div>
            <div class="flex-row">
                <div class="left">Max people:</div>
                <div class="right">{{ action.capacity }}</div>
            </div>
            <div class="flex-row">
                <div class="left">Added benefits:</div>
                <div class="right">{{ action.addedBenefits }}</div>
            </div>
            <div class="flex-row">
                <div class="left">Place:</div>
                <div class="right">{{ action.place }}</div>
            </div>
        </div>
    </div>
</template>

<script>
import moment from "moment";
import axios from "../api/api.js";

export default {
    mounted() {
        this.loadPromoActions();
    },
    data() {
        return {
            promoAction: {
                startDateTime: "",
                endDateTime: "",
                promoactionId: "",
                serviceId: "",
                pricePerDay: "",
                isTaken: "",
                capacity: "",
                addedBenefits: "",
            },
            promoActions: [],
        };
    },
    methods: {
        dateFormat(value) {
            return moment(value).format("YYYY-MM-DD HH:mm");
        },
        loadPromoActions() {
            axios
                .get(
                    "/api/QuickAction/GetQuickActions?serviceId=" +
                        this.$route.params.id
                )
                .then(({ data }) => {
                    console.log(data);
                    this.promoActions = data;
                });
        },
        onActionBook() {
            if (this.currentRole == "Registered") {
                this.$swal
                    .fire({
                        title: "Do you want book quick action?",
                        showDenyButton: true,
                        showCancelButton: true,
                        confirmButtonText: "Book",
                        denyButtonText: `Don't book`,
                    })
                    .then((result) => {
                        /* Read more about isConfirmed, isDenied below */
                        if (result.isConfirmed) {
                            this.$swal.fire("Booked!", "", "success");
                        }
                    });
            }
        },
    },
};
</script>

<style scoped>
.card {
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
    transition: 0.3s;
    padding: 50px;
    cursor: pointer;
}

.card-inner {
    box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
    transition: 0.3s;
    padding: 20px;
    margin: 20px;
    background: rgb(50, 205, 128);
}

.card:hover {
    box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.container {
    padding: 2px 16px;
    cursor: pointer;
}

.img {
    width: 100%;
}

.left {
    text-align: left;
    width: 250px;
}

.right {
    text-align: left;
    width: 250px;
}

.additional {
    background-color: lightgray;
}

.button-book {
    display: flex;
    justify-content: center;
}

button {
    background-color: #fff000;
    border-radius: 12px;
    color: #000;
    cursor: pointer;
    font-weight: bold;
    padding: 10px 15px;
    text-align: center;
    transition: 200ms;
    width: 100%;
    box-sizing: border-box;
    border: 0;
    font-size: 16px;
    user-select: none;
    -webkit-user-select: none;
    touch-action: manipulation;
}

button:hover {
    background-color: #ccbf05;
}

p {
    font-size: 26px;
}

tbody {
    background-color: rgb(239, 248, 248);
}

.flex-row {
    display: flex;
    justify-content: flex-start;
}

.flex-table {
    display: flex;
    flex-direction: column;
}
</style>
