<template>
    <AdminEntitiesNavbar :baseUrl="baseUrlInstructor" />
    <div class="heading">
        <h1>Analytics</h1>
        <button class="button" @click="updatePercentage">
            Update percentage
        </button>
    </div>

    <div class="flexbox-container">
        <div class="flexbox-row">
            <div class="item bold">System takes(%):</div>
            <input
                type="text"
                class="item"
                v-model="systemConfig.value"
                readonly
            />
        </div>
    </div>

    <div class="flexbox-container">
        <h3>Finished reservations</h3>
        <table class="reservations-table">
            <thead>
                <tr>
                    <th>Service</th>
                    <th>From</th>
                    <th>To</th>
                    <th>Client</th>
                    <th>Price</th>
                    <th>Additional equipment</th>
                </tr>
            </thead>
            <tbody v-for="res in reservationRevenues" :key="res.reservationId">
                <tr>
                    <td class="left">{{ res.serviceName }}</td>
                    <td class="left">
                        {{
                            res.serviceStart
                                ? dateFormat(res.serviceStart)
                                : "/"
                        }}
                    </td>
                    <td class="left">
                        {{ res.serviceEnd ? dateFormat(res.serviceEnd) : "/" }}
                    </td>
                    <td class="left">
                        {{ res.userName }} {{ res.userSurname }}
                    </td>
                    <td class="left">{{ res.price }}</td>
                    <td>
                        <div class="right">
                            <div
                                v-for="ben in res.additionalEquipment.split(
                                    ';'
                                )"
                                :key="ben[0]"
                            >
                                {{ ben }}
                            </div>
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div class="flexbox-container-reservations">
        <h3>Calculate revenue within date range</h3>
        <div class="flexbox-revenue">
            <v-date-picker v-model="range" is-range :max-date="new Date()" />
            <h3 class="revenue">
                Total revenue(percentage system takes): {{ overallRevenue }}
            </h3>
        </div>
    </div>
</template>

<script>
import AdminEntitiesNavbar from "@/components/AdminEntitiesNavbar.vue";
import axios from "../api/api.js";
import moment from "moment";

export default {
    name: "AdminAnalytics",
    components: {
        AdminEntitiesNavbar,
    },
    mounted() {
        this.loadSystemConfig();
        this.loadReservationRevenues();
    },
    data() {
        return {
            baseUrlInstructor: "/admin/",
            systemConfig: {},
            reservationRevenues: [],
            overallRevenue: 0,
            range: "",
        };
    },
    methods: {
        loadSystemConfig() {
            axios
                .get("/api/Admin/GetMoneyPercentageSystemTakes")
                .then((res) => {
                    this.systemConfig = res.data;
                    console.log("aaaaa", res.data);
                });
        },
        async updatePercentage() {
            const { value } = await this.$swal.fire({
                title: "How much money should system take upon registration?",
                icon: "question",
                input: "range",
                inputLabel: "Percentage",
                inputAttributes: {
                    min: 0,
                    max: 100,
                    step: 1,
                },
                inputValue: 10,
            });
            this.systemConfig.value = value
                ? value.toString()
                : this.systemConfig.value;
            axios
                .put(
                    "/api/Admin/UpdateMoneyPercentageSystemTakes",
                    this.systemConfig
                )
                .then((res) => {
                    console.log(res.data);
                });
        },
        dateFormat(value) {
            return moment(value).format("YYYY-MM-DD HH:mm");
        },
        loadReservationRevenues() {
            axios.get("/api/Admin/GetReservationRevenue").then((res) => {
                this.reservationRevenues = res.data;
            });
        },
        calculateRevenue(range) {
            axios
                .post("/api/Admin/CalculateFinishedReservationsRevenue", range)
                .then((res) => {
                    this.overallRevenue = res.data;
                    this.overallRevenue = Number(this.overallRevenue).toFixed(
                        2
                    );
                });
        },
    },
    watch: {
        range: {
            handler: function () {
                this.range.start.setHours(0, 0, 0, 0);
                this.range.end.setHours(23, 59, 59, 999);

                this.calculateRevenue(this.range);
            },
            deep: true,
        },
    },
};
</script>

<style scoped>
.heading {
    display: flex;
    justify-content: space-between;
    padding: 50px;
}

.flexbox-container {
    padding: 50px;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: flex-start;
}

.flexbox-revenue {
    padding: 50px;
    display: flex;
    align-items: flex-start;
    justify-content: flex-start;
}

.revenue {
    padding: 50px;
}

.flexbox-row {
    display: flex;
    justify-content: flex-start;
    margin-bottom: 10px;
}

.item {
    width: 20rem;
    display: flex;
    justify-content: flex-start;
    margin-right: 10px;
}

.bold {
    font-weight: bolder;
}

input {
    font-size: 14px;
}

.icon:hover {
    cursor: pointer;
}

.button {
    background-color: #000000;
    border-radius: 12px;
    color: rgb(255, 255, 255);
    cursor: pointer;
    font-weight: bold;
    padding: 10px 10px;
    border: 0;
    font-size: 12px;
    width: 150px;
    height: 50px;
}

button:hover {
    background-color: #2843d8;
}

.flexbox-container-reservations {
    padding: 50px;
    padding-bottom: 1px;
    display: flex;
    justify-content: space-evenly;
    align-items: flex-start;
    flex-direction: column;
}
.reservations-table {
    border: solid 1px #ddeeee;
    border-collapse: collapse;
    border-spacing: 0;
    font: normal 13px Arial, sans-serif;
}
.reservations-table thead th {
    background-color: #ddefef;
    border: solid 1px #ddeeee;
    color: #336b6b;
    padding: 10px;
    text-align: left;
    text-shadow: 1px 1px 1px #fff;
}
.reservations-table tbody td {
    border: solid 1px #ddeeee;
    color: #333;
    padding: 10px;
    text-shadow: 1px 1px 1px #fff;
}
.left {
    text-align: left;
    width: 250px;
}
</style>
