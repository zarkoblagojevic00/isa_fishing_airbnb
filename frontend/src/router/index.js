import { createRouter, createWebHistory } from "vue-router";
import Gallery from "../views/Gallery.vue";
import AdventureProfile from "../views/AdventureProfile.vue";
import AdventureQuickReservation from "../views/AdventureQuickReservation.vue";
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
import AdminHomepage from "../views/AdminHomepage.vue";
import AdminRequests from "../views/AdminRequests.vue";
import AdminProfile from "../views/AdminProfile.vue";
import NewAdmin from "../views/NewAdmin.vue";
import AdminEntities from "../views/AdminEntities.vue";
import AdminAnalytics from "../views/AdminAnalytics.vue";
import VillasExpo from "../views/VillasExpo.vue";
import VillaExpoDetails from "../views/VillaExpoDetails";
import BoatsExpo from "../views/BoatsExpo.vue";
import AdventuresExpo from "../views/AdventuresExpo.vue";
import EditAdventure from "../views/EditAdventure.vue";
import AdventureNewQuickAction from "../views/AdventureNewQuickAction.vue";
import InstructorProfile from "../views/InstructorProfile.vue";
import ClientHomepage from "../views/ClientHomepage.vue";
import AdminIssues from "../views/AdminIssues.vue";
import InstructorAnalytics from "../views/InstructorAnalytics.vue";
import BoatProfile from "../views/BoatProfile.vue";
import NewAdventureReservation from "../views/NewAdventureReservation.vue";
import VillasPromoExpo from "../views/VillasPromoExpo.vue";
import AdventuresPromoExpo from "../views/AdventuresPromoExpo.vue";

const routes = [
    {
        path: "/",
        name: "Homepage",
        component: Homepage,
    },
    {
        path: "/boats",
        name: "BoatsExpo",
        component: BoatsExpo,
    },
    {
        path: "/villas",
        name: "VillasExpo",
        component: VillasExpo,
    },
    {
        path: "/villas-promo",
        name: "VillasPromoExpo",
        component: VillasPromoExpo,
    },
    {
        path: "/villas/details",
        name: "VillaExpoDetails",
        component: VillaExpoDetails,
    },
    {
        path: "/adventures",
        name: "AdventuresExpo",
        component: AdventuresExpo,
    },
    {
        path: "/adventures-promo",
        name: "AdventuresPromoExpo",
        component: AdventuresPromoExpo,
    },
    {
        path: "/client",
        name: "ClientHomePage",
        component: ClientHomepage,
    },
    {
        path: "/adventure/:id/home",
        name: "AdventureProfile",
        component: AdventureProfile,
    },
    {
        path: "/adventure/:id/gallery",
        name: "Gallery",
        component: Gallery,
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
        path: "/instructor/new-reservation/:id",
        name: "NewAdventureReservation",
        component: NewAdventureReservation,
    },
    {
        path: "/instructor/analytics",
        name: "InstructorAnalytics",
        component: InstructorAnalytics,
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
    {
        path: "/admin",
        name: "AdminHomepage",
        component: AdminHomepage,
    },
    {
        path: "/admin/requests",
        name: "AdminRequests",
        component: AdminRequests,
    },
    {
        path: "/admin/my-profile",
        name: "AdminProfile",
        component: AdminProfile,
    },
    {
        path: "/admin/new-admin",
        name: "NewAdmin",
        component: NewAdmin,
    },
    {
        path: "/admin/entities",
        name: "AdminEntities",
        component: AdminEntities,
    },
    {
        path: "/admin/analytics",
        name: "AdminAnalytics",
        component: AdminAnalytics,
    },
    {
        path: "/admin/issues",
        name: "AdminIssues",
        component: AdminIssues,
    },
    {
        path: "/adventure/:id/edit",
        name: "EditAdventure",
        component: EditAdventure,
    },
    {
        path: "/adventure/:id/new-quick-action",
        name: "AdventureNewQuickAction",
        component: AdventureNewQuickAction,
    },
    {
        path: "/instructor/my-profile",
        name: "InstructorProfile",
        component: InstructorProfile,
    },
    {
        path: "/boatprofile",
        name: "BoatProfile",
        component: BoatProfile,
    },
];

const router = createRouter({
    history: createWebHistory(process.env.BASE_URL),
    routes,
});

export default router;
