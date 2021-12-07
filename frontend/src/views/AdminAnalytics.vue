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
</template>

<script>
import AdminEntitiesNavbar from "@/components/AdminEntitiesNavbar.vue";
import axios from "../api/api.js";

export default {
    name: "AdminAnalytics",
    components: {
        AdminEntitiesNavbar,
    },
    mounted() {
        this.loadSystemConfig();
    },
    data() {
        return {
            baseUrlInstructor: "/admin/",
            systemConfig: {},
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
</style>
