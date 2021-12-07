<template>
    <AdminEntitiesNavbar
        :baseUrl="baseUrlInstructor"
        @userTypeChanged="onUserTypeChanged"
    />
    <h1>Users</h1>
    <UsersTable :users="users" @deleted="reload" />
</template>

<script>
import AdminEntitiesNavbar from "@/components/AdminEntitiesNavbar.vue";
import UsersTable from "@/components/UsersTable.vue";
import axios from "../api/api.js";

export default {
    name: "AdminHomepage",
    components: {
        AdminEntitiesNavbar,
        UsersTable,
    },
    mounted() {
        if (this.$route.params.data == undefined) this.loadUsers("0");
        else {
            this.loadUsers(this.$route.params.data);
        }
    },
    data() {
        return {
            baseUrlInstructor: "/admin/",
            users: [],
        };
    },
    methods: {
        onUserTypeChanged(type) {
            this.loadUsers(type);
        },
        loadUsers(type) {
            axios
                .get("/api/GeneralUser/GetUsersByRole?userType=" + type)
                .then((res) => (this.users = res.data));
        },
        reload(type) {
            this.loadUsers(type);
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
