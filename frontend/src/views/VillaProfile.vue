<template>
    <div class="whole-page">
        <div class="wrapper">
            <div class="menu">
                <div
                    class="menu-item villa-icon"
                    @click="ToggleSubmenu('villa')"
                >
                    <span class="menu-text"> Villas </span>
                </div>
                <div class="submenu" id="sub-villa">
                    <div
                        class="submenu-item"
                        @click="ChangeMode('AddNewVilla')"
                    >
                        <span class="menu-text"> Add new villa </span>
                    </div>
                    <div class="submenu-item" @click="ChangeMode('ViewVillas')">
                        <span class="menu-text"> View villas </span>
                    </div>
                    <div
                        class="submenu-item"
                        @click="ChangeMode('CheckVillaSchedule')"
                    >
                        <span class="menu-text"> Check villa schedule </span>
                    </div>
                    <div
                        class="submenu-item"
                        @click="ChangeMode('AddPromoAction')"
                    >
                        <span class="menu-text"> Add promo action </span>
                    </div>
                    <div
                        class="submenu-item"
                        @click="ChangeMode('ReserveForUser')"
                    >
                        <span class="menu-text"> Create a reservation </span>
                    </div>
                </div>

                <div
                    class="menu-item user-icon"
                    @click="ToggleSubmenu('profile')"
                >
                    <span class="menu-text"> Profile </span>
                </div>
                <div class="submenu" id="sub-profile">
                    <div
                        class="submenu-item"
                        @click="ChangeMode('ViewProfile')"
                    >
                        <span class="menu-text"> View profile </span>
                    </div>
                    <div class="submenu-item" @click="ChangeMode('ChangePass')">
                        <span class="menu-text"> Change password </span>
                    </div>
                    <div
                        class="submenu-item"
                        @click="ChangeMode('RequestDeletion')"
                    >
                        <span class="menu-text">
                            Request account deletion
                        </span>
                    </div>
                </div>

                <div
                    class="menu-item report-icon"
                    @click="ToggleSubmenu('report')"
                >
                    <span class="menu-text"> Reports </span>
                </div>
                <div class="submenu" id="sub-report">
                    <div class="submenu-item">
                        <span
                            class="menu-text"
                            @click="ChangeMode('VillaReport')"
                        >
                            Report for villa
                        </span>
                    </div>
                    <div
                        class="submenu-item"
                        @click="ChangeMode('GeneralReport')"
                    >
                        <span class="menu-text"> Report for all villas </span>
                    </div>
                </div>
            </div>

            <div class="content">
                <AddNewVilla
                    v-if="mode == 'AddNewVilla'"
                    :changeMode="ChangeMode"
                    :villaId="-1"
                />
                <AddNewVilla
                    v-if="mode == 'UpdateVilla'"
                    :changeMode="ChangeMode"
                    :villaId="chosenVilla"
                />
                <ViewVillas
                    v-if="mode == 'ViewVillas'"
                    :changeMode="ChangeMode"
                    :changeChosenVilla="ChangeSelectedVilla"
                />
                <VillaImages
                    v-if="mode == 'VillaImages'"
                    :changeMode="ChangeMode"
                    :villaId="chosenVilla"
                />
                <ServiceCalendar
                    v-if="mode == 'CheckVillaSchedule'"
                    :serviceMode="'villa'"
                />
                <AddPromoAction
                    v-if="mode == 'AddPromoAction' || mode == 'ReserveForUser'"
                    :currentMode="mode"
                    :promoMode="'villa'"
                />
                <VillaOwnerProfile v-if="mode == 'ViewProfile'" />
                <VillaOwnerPassChange v-if="mode == 'ChangePass'" />
                <VillaOwnerDeletion v-if="mode == 'RequestDeletion'" />
            </div>
        </div>
    </div>
</template>

<script>
import AddNewVilla from "../components/AddNewVilla.vue";
import ViewVillas from "../components/ViewVillas.vue";
import VillaImages from "../components/VillaImages.vue";
import ServiceCalendar from "../components/ServiceCalendar.vue";
import AddPromoAction from "../components/AddPromoAction.vue";
import VillaOwnerProfile from "../components/VillaOwnerProfile.vue";
import VillaOwnerPassChange from "../components/VillaOwnerPassChange.vue";
import VillaOwnerDeletion from "../components/VillaAccountDeletion.vue";

export default {
    name: "VillaProfile",
    components: {
        AddNewVilla,
        ViewVillas,
        VillaImages,
        ServiceCalendar,
        AddPromoAction,
        VillaOwnerProfile,
        VillaOwnerPassChange,
        VillaOwnerDeletion,
    },
    data() {
        return {
            mode: "ViewVillas",
            chosenVilla: 0,
        };
    },
    mounted() {
        let cookie = document.cookie;
        if (cookie.userId == undefined || cookie.email == undefined) {
            this.$router.push("/");
        }
    },
    methods: {
        ToggleSubmenu(name) {
            let newname = "sub-" + name;
            window.$("#" + newname).slideToggle();
        },
        ChangeMode(newMode) {
            if (newMode == "AddNewVilla") {
                this.chosenVilla = -1;
            }
            this.mode = newMode;
        },
        ChangeSelectedVilla(newVilla) {
            this.chosenVilla = newVilla;
        },
    },
};
</script>

<style scoped>
.whole-page {
    height: 1000px;
    padding: 20px;
    background-color: #cecece;
    display: flex;
    justify-content: center;
    position: relative;
}

.wrapper {
    height: 100%;
    width: 1400px;
    display: flex;
    flex-direction: row;
}

.menu {
    background-color: gray;
    height: 100%;
    width: 300px;
    display: flex;
    flex-direction: column;
    margin-right: 10px;
}

.menu-item {
    height: 70px;
    border-bottom: 1px solid #c3c3c3;
    background-position: left 25px center;
    background-repeat: no-repeat;
    padding-left: 90px;
    width: 100%;
    box-sizing: border-box;
    cursor: pointer;
}

.menu-item:hover {
    background-color: #a5978b;
}

.menu-text {
    display: flex;
    height: 100%;
    justify-content: start;
    align-items: center;
    color: white;
}

.villa-icon {
    background-image: url("../assets/villa-white-outline.png");
}
.user-icon {
    background-image: url("../assets/user-white-outline.png");
}
.report-icon {
    background-image: url("../assets/report-white-outline.png");
}

.content {
    width: 100%;
    overflow: auto;
}

.content::-webkit-scrollbar {
    display: none;
}

.submenu {
    width: 100%;
    border-bottom: 1px solid #c3c3c3;
    flex-direction: column;
    display: none;
}

.submenu-item {
    width: 100%;
    padding-left: 20px;
    box-sizing: border-box;
    height: 60px;
}

.submenu-item:hover {
    background-color: #a5978b;
    cursor: pointer;
}

@media (max-width: 500px) {
}
</style>
