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
import Login from "../views/Login.vue"; 

const routes = [
  {
    path: "/",
    name: "Homepage",
    component: Homepage,
  },
  {
    path: "/adventure/gallery",
    name: "Gallery",
    component: Gallery,
  },
  {
    path: "/adventure",
    name: "AdventureProfile",
    component: AdventureProfile,
  },
  {
    path: "/adventure/rules-of-conduct",
    name: "AdventureRulesOfConduct",
    component: AdventureRulesOfConduct,
  },
  {
    path: "/adventure/price-list",
    name: "PriceList",
    component: PriceList,
  },
  {
    path: "/adventure/quick-reservation",
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
    name: "Login",
    component: Login,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
