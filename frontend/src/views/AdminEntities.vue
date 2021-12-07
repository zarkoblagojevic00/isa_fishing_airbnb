<template>
    <AdminEntitiesNavbar
        :baseUrl="baseUrlInstructor"
        @serviceTypeChanged="onServiceTypeChanged"
    />
    <h1>Entities</h1>
    <EntitiesTable :services="services" @deleted="reload" />
</template>

<script>
import AdminEntitiesNavbar from "@/components/AdminEntitiesNavbar.vue";
import EntitiesTable from "@/components/EntitiesTable.vue";
import axios from "../api/api.js";

export default {
    name: "AdminEntities",
    components: {
        AdminEntitiesNavbar,
        EntitiesTable,
    },
    data() {
        return {
            baseUrlInstructor: "/admin/",
            services: [],
        };
    },
    mounted() {
        if (this.$route.params.data == undefined) this.loadServices("0");
        else {
            this.loadServices(this.$route.params.data);
        }
    },
    methods: {
        onServiceTypeChanged(type) {
            this.loadServices(type);
        },
        loadServices(type) {
            axios
                .get(
                    "/api/GeneralService/GetServicesByType?serviceType=" + type
                )
                .then((res) => (this.services = res.data));
        },
        reload(type) {
            this.loadServices(type);
        },
    },
};
</script>

<style scoped>
h1 {
    display: flex;
    justify-content: flex-start;
    padding: 50px;
}
</style>
