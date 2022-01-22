<template>
    <div class="whole-page">
        <div class="container">
            <div class="menu sidebar vertical-scroll-no-bar fix-margin-top">
                <div
                    class="menu-item"
                    @click="ChangeMode('ClientBookedReservations')"
                >
                    <span class="menu-text"> Booked Reservations </span>
                </div>

                <div
                    class="menu-item"
                    @click="ChangeMode('ClientHistoryReservations')"
                >
                    <span class="menu-text"> Reservation history </span>
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
            </div>

            <div class="service-content">
                <ClientWelcome v-if="mode == 'ClientWelcome'" />
                <VillaOwnerProfile v-if="mode == 'ViewProfile'" />
                <VillaOwnerPassChange v-if="mode == 'ChangePass'" />
                <VillaOwnerDeletion v-if="mode == 'RequestDeletion'" />
                <BookedReservationsExpo
                    v-if="mode == 'ClientBookedReservations'"
                />
                <HistoryReservationsExpo
                    v-if="mode == 'ClientHistoryReservations'"
                />
                <ClientSubscriptions v-if="mode == 'ClientSubscriptions'" />
            </div>
        </div>
    </div>
</template>

<script>
import ClientWelcome from "../components/ClientWelcome.vue";
import VillaOwnerProfile from "../components/VillaOwnerProfile.vue";
import VillaOwnerPassChange from "../components/VillaOwnerPassChange.vue";
import VillaOwnerDeletion from "../components/VillaAccountDeletion.vue";
import BookedReservationsExpo from "../components/BookedReservationsExpo.vue";
import HistoryReservationsExpo from "../components/HistoryReservationsExpo.vue";
import ClientSubscriptions from "../components/ClientSubscriptions.vue";

export default {
    name: "ClientHomepage",
    components: {
        ClientWelcome,
        VillaOwnerProfile,
        VillaOwnerPassChange,
        VillaOwnerDeletion,
        BookedReservationsExpo,
        HistoryReservationsExpo,
        ClientSubscriptions,
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
            this.mode = newMode;
        },
    },
};
</script>

<style scoped>
.whole-page {
    width: 80%;
    margin: auto;
    position: relative;
}

.menu {
    margin-top: 30px;
    border-radius: 0.25em;
    background-color: #99a199;
    width: 17%;
    display: flex;
    flex-direction: column;
}

.menu-item {
    background-color: var(--primary-dark);
    height: 70px;
    border-bottom: 1px solid #c3c3c3;
    border-radius: 0.3em;
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
    background-color: var(--primary);
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
    border-radius: 0.3em;
    box-sizing: border-box;
    height: 60px;
}

.submenu-item:hover {
    background-color: var(--primary);
    cursor: pointer;
}

.service-content {
    width: 80%;
}

.item-container {
    width: 100%;
}

@media (max-width: 500px) {
}
</style>
