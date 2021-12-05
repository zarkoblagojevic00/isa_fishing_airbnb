import { createRouter, createWebHistory } from "vue-router";
import Gallery from "../views/Gallery.vue";
import AdventureProfile from "../views/AdventureProfile.vue";
import AdventureQuickReservation from "../views/AdventureQuickReservation.vue";
import AdventureRulesOfConduct from "../views/AdventureRulesOfConduct.vue";
import PriceList from "../views/PriceList.vue";
import InstructorHomePage from "../views/InstructorHomePage.vue";
import Homepage from "../views/Homepage.vue";
import InstructorServices from "../views/InstructorServices.vue";
import InstructorAvailability from "../views/InstructorAvailability.vue";
import InstructorReservations from "../views/InstructorReservations.vue";
import Login from "../views/LoginRegForgotPass.vue";
import Testpage from "../views/TestPage.vue";
import VillaProfile from "../views/VillaProfile.vue";
import NewAdventure from "../views/NewAdventure.vue";
import InstructorEditAvailability from "../views/InstructorEditAvailability.vue";

const routes = [
    {
        path: "/",
        name: "Homepage",
        component: Homepage,
    },
    {
        path: "/adventure/:id/gallery",
        name: "Gallery",
        component: Gallery,
    },
    {
        path: "/adventure/:id",
        name: "AdventureProfile",
        component: AdventureProfile,
    },
    {
        path: "/adventure/:id/rules-of-conduct",
        name: "AdventureRulesOfConduct",
        component: AdventureRulesOfConduct,
    },
    {
        path: "/adventure/:id/price-list",
        name: "PriceList",
        component: PriceList,
    },
    {
        path: "/adventure/:id/quick-reservation",
        name: "AdventureQuickReservation",
        component: AdventureQuickReservation,
    },
    {
        path: "/instructor",
        name: "InstructorHomePage",
        component: InstructorHomePage,
    },
    {
        path: "/instructor/services",
        name: "InstructorServices",
        component: InstructorServices,
    },
    {
        path: "/instructor/availability",
        name: "InstructorAvailability",
        component: InstructorAvailability,
    },
    {
        path: "/instructor/reservations",
        name: "InstructorReservations",
        component: InstructorReservations,
    },
    {
        path: "/login",
        name: "LoginRegForgotPass",
        component: Login,
    },
    {
        path: "/testpage",
        name: "TestPage",
        component: Testpage,
    },
    {
        path: "/villaprofile",
        name: "VillaProfile",
        component: VillaProfile,
    },
    {
        path: "/instructor/new-adventure",
        name: "NewAdventure",
        component: NewAdventure,
    },
    {
        path: "/instructor/availability/edit",
        name: "InstructorEditAvailability",
        component: InstructorEditAvailability,
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export default router;
