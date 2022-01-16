<template>
    <div style="whole-page">
        <h3>Report for {{ serviceKind }}</h3>
        <hr />
        <select class="select-villa" v-model="selectedService">
            <option disabled value="">Please select one</option>
            <option
                v-for="service in allServices"
                :key="service.id"
                :value="service.id"
            >
                {{ service.name }}
            </option>
        </select>
        <select class="select-villa" v-model="bussinessMode">
            <option value="weekly">Weekly</option>
            <option value="monthly">Monthly</option>
            <option value="yearly">Yearly</option>
        </select>
        <select class="select-villa" v-model="chartDataMode">
            <option value="moneyMade">Money made</option>
            <option value="numberOfReservations">Number of reservations</option>
        </select>
        <vue3-chart-js
            style="max-height: 800px; background: white; margin-top: 20px"
            :id="lineChart.id"
            :type="lineChart.type"
            :data="lineChart.data"
            ref="chartRef"
        ></vue3-chart-js>
    </div>
</template>

<script>
import { ref } from "vue";
import Vue3ChartJs from "@j-t-mcc/vue3-chartjs";
export default {
    name: "ServiceCharts",
    components: {
        Vue3ChartJs,
    },
    props: {
        serviceType: String,
    },
    setup() {
        const chartRef = ref(null);
        const lineChart = {
            id: "line",
            type: "line",
            data: {
                labels: [],
                datasets: [
                    {
                        backgroundColor: ["#41B883"],
                        data: [],
                    },
                ],
            },
        };

        const updateChart = (
            receivedBussinessSummary,
            chartLabel,
            chartMode,
            bussinessMode
        ) => {
            lineChart.data.datasets[0].label = chartLabel;
            lineChart.data.labels = new Array();
            lineChart.data.datasets[0].data = new Array();
            if (bussinessMode == "weekly") {
                for (let week of receivedBussinessSummary.weekly) {
                    lineChart.data.labels.push(week.name);
                    if (chartMode == "moneyMade") {
                        lineChart.data.datasets[0].data.push(week.moneyMade);
                    } else {
                        lineChart.data.datasets[0].data.push(
                            week.numberOfReservations
                        );
                    }
                }
            } else if (bussinessMode == "monthly") {
                for (let week of receivedBussinessSummary.monthly) {
                    lineChart.data.labels.push(week.name);
                    if (chartMode == "moneyMade") {
                        lineChart.data.datasets[0].data.push(week.moneyMade);
                    } else {
                        lineChart.data.datasets[0].data.push(
                            week.numberOfReservations
                        );
                    }
                }
            } else {
                for (let week of receivedBussinessSummary.yearly) {
                    lineChart.data.labels.push(week.name);
                    if (chartMode == "moneyMade") {
                        lineChart.data.datasets[0].data.push(week.moneyMade);
                    } else {
                        lineChart.data.datasets[0].data.push(
                            week.numberOfReservations
                        );
                    }
                }
            }

            chartRef.value.update();
        };

        return {
            chartRef,
            lineChart,
            updateChart,
        };
    },
    data() {
        return {
            serviceKind: this.$props.serviceType,
            selectedService: "",
            allServices: [],
            receivedBussinessSummary: [],
            bussinessMode: "weekly",
            chartDataMode: "moneyMade",
        };
    },
    mounted() {
        this.GetServices();
    },
    methods: {
        GetServices() {
            let vue = this;
            let url =
                this.serviceKind == "villa"
                    ? "/api/VillaManagement/GetOwnedVillas"
                    : "/api/BoatManagement/GetOwnedBoats";
            fetch(url, {
                method: "GET",
                header: {
                    "Content-type": "application-json",
                    "Set-Cookie": document.cookie,
                },
            })
                .then((response) => {
                    if (response.status == 200) {
                        return response.json();
                    } else {
                        return response.text();
                    }
                })
                .then((data) => {
                    var stringConstructor = "test".constructor;
                    if (data.constructor == stringConstructor) {
                        alert("Something went wrong!\nError message: " + data);
                        return;
                    }
                    vue.allServices = new Array();
                    for (let service of data) {
                        vue.allServices.push({
                            id:
                                vue.serviceKind == "villa"
                                    ? service.villaId
                                    : service.boatId,
                            name: service.name,
                        });
                    }
                });
        },
        RefreshChart() {
            let vue = this;
            fetch(
                "/api/GeneralService/GetBusinessSummary?serviceId=" +
                    this.selectedService,
                {
                    method: "GET",
                    header: {
                        "Content-type": "application-json",
                        "Set-Cookie": document.cookie,
                    },
                }
            )
                .then((response) => {
                    if (response.ok) {
                        return response.json();
                    }
                    return response.text();
                })
                .then((data) => {
                    let message = "";
                    try {
                        message = JSON.parse(data);
                    } catch {
                        message = data;
                    }
                    if (message.constructor == "".constructor) {
                        alert("Something went wrong!\n" + message);
                        return;
                    }
                    vue.receivedBussinessSummary = message;
                    vue.SetupData();
                });
        },
        SetupData() {
            let serviceName = "";
            for (let service of this.allServices) {
                if (service.id == this.selectedService) {
                    serviceName = service.name;
                }
            }
            this.updateChart(
                this.receivedBussinessSummary,
                serviceName,
                this.chartDataMode,
                this.bussinessMode
            );
        },
    },
    watch: {
        selectedService: function () {
            this.RefreshChart();
        },
        chartDataMode: function () {
            this.SetupData();
        },
        bussinessMode: function () {
            this.SetupData();
        },
    },
};
</script>

<style scoped>
.whole-page {
    width: 100%;
    height: 100%;
}
.select-villa {
    width: 200px;
    height: 40px;
    margin-left: 10px;
    outline: none;
    border: none;
}
</style>
