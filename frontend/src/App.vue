<template>
    <GeneralHeader
        :menuitems="menuItems"
        v-if="ShouldNotTurnOffHeaderAndFooter()"
    />
    <router-view />
    <GeneralFooter v-if="ShouldNotTurnOffHeaderAndFooter()" />
</template>

<script>
import sessionService from "@/services/session-service.js";
import GeneralHeader from "@/components/GeneralHeader.vue";
import GeneralFooter from "@/components/GeneralFooter.vue";

export default {
    name: "App",
    components: {
        GeneralHeader,
        GeneralFooter,
    },
    data() {
        return {
            menuItems: [
                {
                    text: "browse",
                    options: [
                        {
                            text: "villas",
                            invoke: () => (window.location.href = "/villas"),
                        },
                        {
                            text: "boats",
                            invoke: () => (window.location.href = "/boats"),
                        },
                        {
                            text: "adventures",
                            invoke: () =>
                                (window.location.href = "/adventures"),
                        },
                    ],
                },
                {
                    text: "account",
                    options: !document.cookie
                        ? [
                              {
                                  text: "login",
                                  invoke: () =>
                                      (window.location.href = "/login"),
                              },
                              {
                                  text: "register",
                                  invoke: () =>
                                      (window.location.href = "/Login"),
                              },
                          ]
                        : [
                              {
                                  text: "log out",
                                  invoke: this.logout,
                              },
                          ],
                },
            ],
        };
    },
    methods: {
        ShouldNotTurnOffHeaderAndFooter() {
            if (
                window.location.href.indexOf("Login") != -1 ||
                window.location.href.indexOf("login") != -1
            ) {
                return false;
            }
            return true;
        },

        logout() {
            sessionService.logout();
            location.href = "/";
        },
    },
};
</script>

<style>
#app {
    font-family: Verdana;
    -webkit-font-smoothing: antialiased;
    -moz-osx-font-smoothing: grayscale;
    text-align: center;
    color: #2c3e50;
}

body {
    margin: 0px;
    max-width: 100vw;
}
</style>
