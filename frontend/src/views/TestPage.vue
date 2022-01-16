<template>
    <GeneralHeader :menuitems="menuItems" />
    <GeneralFooter />
</template>

<script>
import GeneralHeader from "@/components/GeneralHeader.vue";
import GeneralFooter from "@/components/GeneralFooter.vue";

export default {
    name: "TestPage",
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
                            invoke: () => (window.location.href = ""),
                        },
                        {
                            text: "boats",
                            invoke: () => (window.location.href = ""),
                        },
                        {
                            text: "adventures",
                            invoke: () => (window.location.href = ""),
                        },
                    ],
                },
                {
                    text: "account",
                    options: [
                        {
                            text: "login",
                            invoke: () => (window.location.href = "/Login"),
                        },
                        {
                            text: "register",
                            invoke: () => (window.location.href = ""),
                        },
                    ],
                },
            ],
        };
    },
};
</script>

<style scoped></style>
