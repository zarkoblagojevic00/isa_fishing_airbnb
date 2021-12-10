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
        this.getAdminInfo();
        if (this.$route.params.data == undefined) this.loadUsers("0");
        else {
            this.loadUsers(this.$route.params.data);
        }
    },
    data() {
        return {
            baseUrlInstructor: "/admin/",
            users: [],
            admin: {},
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
        getAdminInfo() {
            axios.get("/api/Admin/GetUserInfo").then((res) => {
                console.log(res.data);
                if (!res.data.isAccountActive) {
                    this.onChangePassword(res.data);
                }
            });
        },
        async onChangePassword(adminInfo) {
            const { value: formValues } = await this.$swal.fire({
                title: "Password change",
                html:
                    '<input id="swal-input1" class="swal2-input" placeholder="Old password" type="password">' +
                    '<input id="swal-input2" class="swal2-input" placeholder="New password" type="password">' +
                    '<input id="swal-input3" class="swal2-input" placeholder="Confirm new password" type="password">',
                focusConfirm: false,
                preConfirm: () => {
                    return {
                        oldPassword:
                            document.getElementById("swal-input1").value,
                        newPassword:
                            document.getElementById("swal-input2").value,
                        newPasswordConfirmed:
                            document.getElementById("swal-input3").value,
                    };
                },
            });

            if (formValues) {
                console.log(formValues);
                axios.put("/api/Admin/ChangePassword", formValues).then(() => {
                    this.readMode = true;
                    this.editMode = false;

                    adminInfo.isAccountActive = true;
                    this.activateAdmin(adminInfo);
                });
            }
        },
        activateAdmin(admin) {
            console.log(admin);
            const Toast = this.$swal.mixin({
                toast: true,
                position: "top-end",
                showConfirmButton: false,
                timer: 3000,
            });
            axios.put("/api/Admin/ActivateAdmin", admin).then(() => {
                Toast.fire({
                    icon: "success",
                    title: "User information successfully updated.",
                });
            });
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
