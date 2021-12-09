<template>
    <div class="card">
        <div class="container">
            <table>
                <tbody
                    v-for="action in promoActions"
                    :key="action.promoActionId"
                >
                    <tr>
                        <td class="left">Start time:</td>
                        <td class="right">
                            {{ dateFormat(action.startDateTime) }}
                        </td>
                    </tr>
                    <tr>
                        <td class="left">End time:</td>
                        <td class="right">
                            {{ dateFormat(action.endDateTime) }}
                        </td>
                    </tr>
                    <tr>
                        <td class="left">Price per day:</td>
                        <td class="right">{{ action.pricePerDay }}</td>
                    </tr>
                    <tr>
                        <td class="left">Max people:</td>
                        <td class="right">{{ action.capacity }}</td>
                    </tr>
                    <tr>
                        <td class="left">Added benefits:</td>
                        <td class="right">{{ action.addedBenefits }}</td>
                    </tr>
                    <tr>
                        <td class="left">Place:</td>
                        <td class="right">{{ action.place }}</td>
                    </tr>
                    <hr />
                </tbody>
            </table>
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

.card:hover {
    box-shadow: 0 8px 16px 0 rgba(0, 0, 0, 0.2);
}

.container {
    padding: 2px 16px;
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
</style>
