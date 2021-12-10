<template>
    <div class="whole-page">
        <div class="wrapper">
            <div class="menu">
                <div
                    class="menu-item"
                    @click="ChangeMode('ClientBookedReservations')"
                >
                    <span class="menu-text"> Booked Reservations </span>
                </div>

                <div class="menu-item" @click="ToggleSubmenu('villa')">
                    <span class="menu-text"> Reservation history </span>
                </div>
                <div class="submenu" id="sub-villa">
                    <div
                        class="submenu-item"
                        @click="ChangeMode('ClientHistoryVillas')"
                    >
                        <span class="menu-text"> Villas </span>
                    </div>
                    <div
                        class="submenu-item"
                        @click="ChangeMode('ClientHistoryBoats')"
                    >
                        <span class="menu-text"> Boats </span>
                    </div>
                    <div
                        class="submenu-item"
                        @click="ChangeMode('ClientHistoryAdventures')"
                    >
                        <span class="menu-text"> Adventures </span>
                    </div>
                </div>

                <div class="menu-item" @click="ToggleSubmenu('profile')">
                    <span class="menu-text"> Profile </span>
                </div>
                <div class="submenu" id="sub-profile">
                    <div
                        class="submenu-item"
                        @click="ChangeMode('ClientSubscriptions')"
                    >
                        <span class="menu-text"> Subscriptions </span>
                    </div>
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
                        @click="ChangeMode('ClientPenalties')"
                    >
                        <span class="menu-text"> Penalties </span>
                    </div>
                    <div
                        class="submenu-item"
                        @click="ChangeMode('ClientComplaints')"
                    >
                        <span class="menu-text"> Leave a complaint </span>
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

                <!-- <div
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
                </div> -->
            </div>

            <div class="content">
                <ClientWelcome v-if="mode == 'ClientWelcome'" />
                <VillaOwnerProfile v-if="mode == 'ViewProfile'" />
                <VillaOwnerPassChange v-if="mode == 'ChangePass'" />
                <VillaOwnerDeletion v-if="mode == 'RequestDeletion'" />
            </div>
        </div>
    </div>
</template>

<script>
import ClientWelcome from "../components/ClientWelcome.vue";
import VillaOwnerProfile from "../components/VillaOwnerProfile.vue";
import VillaOwnerPassChange from "../components/VillaOwnerPassChange.vue";
import VillaOwnerDeletion from "../components/VillaAccountDeletion.vue";

export default {
    name: "VillaProfile",
    components: {
        ClientWelcome,
        VillaOwnerProfile,
        VillaOwnerPassChange,
        VillaOwnerDeletion,
    },
    data() {
        return {
            mode: "ClientWelcome",
        };
    },
    methods: {
        ToggleSubmenu(name) {
            let newname = "sub-" + name;
            window.$("#" + newname).slideToggle();
        },
        ChangeMode(newMode) {
            if (newMode == "AddNewVilla") {
                this.chosenVilla = 0;
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
    width: 1500px;
    display: flex;
    flex-direction: row;
}

.menu {
    border-radius: 5px;
    background-color: gray;
    height: 100%;
    width: 300px;
    display: flex;
    flex-direction: column;
    margin-right: 10px;
}

.menu-item {
    background-color: #333;
    height: 70px;
    border-bottom: 1px solid #c3c3c3;
    border-radius: 7px;
    background-position: left 25px center;
    background-repeat: no-repeat;
    display: flex;
    justify-content: center;
    align-self: center;
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
    border-bottom: 1px solid #333;
    border-radius: 7px;
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
